using System;
using System.Diagnostics;
using System.Linq;
using Cudafy;
using Cudafy.Host;
using MathNet.Numerics.LinearAlgebra.Solvers;

namespace LHON_Form
{
    public partial class Main_Form
    {

        // ========================
        //      Constants
        // ========================

        private readonly float min_res = 10F;
        private int process_clearance = 2;

        // ========================
        //      Parameters
        // ========================

        /* Units:
        Constant                        UI Unit                  Algorithm Unit          Conversion Factor
        k_detox, k_rate                  1 / sec                   1 / itr               K / resolution ^ 2
        k_tox_prod                       micromol / um^2 / sec     tox / pix / itr       K / resolution ^ 4
        death_tox_thres, insult_tox      micromol / um^2           tox / pix             K / resolution ^ 2
        
            sec = (CONSTANT / resolution ^ 2) * itr
            micromol / um^2 = (CONSTANT * resolution ^ 2) * tox / pix
        */

        private float k_detox_intra, k_detox_extra, k_tox_prod, death_tox_thres, insult_tox, on_death_tox,
            k_rate_live_axon, k_rate_boundary, k_rate_dead_axon, k_rate_extra;


        // ====================================
        //              Variables
        // ====================================

        private float[] tox, tox_init, tox_dev, tox_new_dev; // Tox
        private float[] rate, rate_init, rate_dev; // Rate
        private float[] detox, detox_init, detox_dev; // Detox
        private float[] tox_prod, tox_prod_init, tox_prod_dev; // Tox_prod
        private uint[] axons_cent_pix, axons_cent_pix_dev; // Center pixel of each axon
        private uint[] axons_inside_pix, axons_inside_pix_dev; // 1D array for all axons
        private uint[] axons_inside_pix_idx, axons_inside_pix_idx_dev; // indices for the above 1D array
        private uint[] axons_surr_rate, axon_surr_rate_dev; // 1D indices of rate array that have the boundary rate and are outside axons
        private uint[] axons_surr_rate_idx, axon_surr_rate_idx_dev; // indices for above array
        private uint[] id_center_axon, id_center_axon_dev; //axon id for center pixels; rest are zeros
        private uint[] death_itr, death_itr_dev; // death iteration of each axon
        private byte[] axon_mask, axon_mask_init, axon_mask_dev;

        // Index of pixels inside the nerve (used for cuda functions)
        private int[] pix_idx, pix_idx_dev;
        private int pix_idx_num; // number of pixels inside the nerve

        private bool[] axon_is_alive, axon_is_alive_init, axon_is_alive_dev;
        private int[] num_alive_axons = new int[1], num_alive_axons_dev;
        private bool[] axon_is_init_insult;
        private bool[] axon_is_large; // For display purposes

        private ushort im_size;

        // ====================================
        //        Model Preprocessing
        // ====================================
        private ushort Calc_im_siz()
        {
            return (ushort)((mdl_nerve_r * setts.resolution + 2) * 2);
        }

        // Requires full Model info and assigns tox, rate, etc
        private void Preprocess_model()
        {
            

            
            // =======================================
            //              Init Parameters
            // =======================================

            float res = setts.resolution;
            mdl_nerve_r = mdl.nerve_scale_ratio * mdl_real_nerve_r;

            float rate_conv = Pow2(res / min_res);
            float temp = 1F / rate_conv;
            if (setts.rate_live > temp ||
                setts.rate_bound > temp ||
                setts.rate_dead > temp ||
                setts.rate_extra > temp)
            {
                Append_stat_ln("Error: Diffusion param greater than " + temp.ToString() + ". Preprocessing aborted.");
                return;
            }

            if (setts.detox_intra > 1F ||
                setts.detox_extra> 1F)
            {
                Append_stat_ln("Error: Detox param greater than 1. Preprocessing aborted.");
                return;
            }



            // "setts" are user input with physical units

            // 1 - real detox rate to reduce computation ->  tox[x_y] *= detox[x_y]
            k_detox_intra = 1F - setts.detox_intra;
            k_detox_extra = 1F - setts.detox_extra;
            k_tox_prod = setts.tox_prod;

            float fiveF = 5F;

            // User inputs 0 to 1 for rate values 
            k_rate_live_axon = setts.rate_live / fiveF * rate_conv;
            k_rate_boundary = setts.rate_bound / fiveF * rate_conv;
            k_rate_dead_axon = setts.rate_dead / fiveF * rate_conv;
            k_rate_extra = setts.rate_extra / fiveF * rate_conv;

            // 
            death_tox_thres = setts.death_tox_thres;
            insult_tox = setts.insult_tox;
            on_death_tox = setts.on_death_tox;

            prep_prof.Time(0);
            Tic();

            Update_bottom_stat("Preprocessing ...");

            im_size = Calc_im_siz();
            Update_image_siz_lbl();

            Init_bmp_write();

            // ======== Pixel Properties =========
            rate = rate_init = new float[im_size * im_size * 4];
            detox = detox_init = new float[im_size * im_size];
            tox = tox_init = new float[im_size * im_size];
            tox_prod = tox_prod_init = new float[im_size * im_size];

            id_center_axon = new uint[im_size * im_size];

            axon_mask = axon_mask_init = new byte[im_size * im_size]; // for display. if 1: axon is live, if 2: axon is dead, otherwise 0.
            pix_idx = new int[im_size * im_size]; // linear index of pixels within nerve

            // ======== Axon Properties =========

            axons_cent_pix = new uint[mdl.n_axons];
            axon_is_alive = axon_is_alive_init = Enumerable.Repeat(true, mdl.n_axons).ToArray(); // init to true

            // temp variable
            int max_pixels_in_nerve = (int)(Pow2(mdl_nerve_r * res) * (1 - Pow2(mdl_vessel_ratio)) * Math.PI);

            axons_inside_pix = new uint[max_pixels_in_nerve * 3 / 4];
            axons_inside_pix_idx = new uint[mdl.n_axons + 1];

            axons_surr_rate = new uint[max_pixels_in_nerve / 5];
            axons_surr_rate_idx = new uint[mdl.n_axons + 1];

            axon_is_init_insult = new bool[mdl.n_axons];

            axon_is_large = new bool[mdl.n_axons]; // For display purposes

            death_itr = new uint[mdl.n_axons];

            axon_lbl = new AxonLabelClass[mdl.n_axons]; // for GUI

            // ======== Local Constants =========
            int nerve_cent_pix = im_size / 2;
            int nerve_r_pix = (int)(mdl_nerve_r * res);
            int vein_r_pix = (int)(mdl_vessel_ratio * mdl_nerve_r * res);

            int nerve_r_pix_2 = Pow2(nerve_r_pix);
            int vein_r_pix_2 = Pow2(vein_r_pix);

            // GPU init for prep0 and prep1
            GPGPU localGPUVar = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            localGPUVar.FreeAll(); 
            localGPUVar.Synchronize();

            pix_idx_num = 0;
            rate_dev = localGPUVar.Allocate<float>(im_size * im_size * 4);
            detox_dev = localGPUVar.Allocate<float>(im_size * im_size);
            pix_idx_dev = localGPUVar.Allocate<int>(im_size * im_size);
            byte[] pix_out_of_nerve_dev = localGPUVar.Allocate<byte>(im_size * im_size);
            byte[] pix_out_of_nerve = new byte[im_size * im_size];

            int prep_siz = 32;
            dim3 block_siz_prep = new dim3(prep_siz, prep_siz);
            int tmp = (int)Math.Ceiling((double)im_size / (double)prep_siz);
            dim3 grid_siz_prep = new dim3(tmp, tmp);

            localGPUVar.Launch(grid_siz_prep, block_siz_prep).cuda_prep0(im_size, nerve_cent_pix, nerve_r_pix_2, vein_r_pix_2, k_rate_extra, k_detox_extra,
                pix_out_of_nerve_dev, rate_dev, detox_dev);

            localGPUVar.Synchronize();

            localGPUVar.CopyFromDevice(pix_out_of_nerve_dev, pix_out_of_nerve);
            localGPUVar.CopyFromDevice(rate_dev, rate);
            localGPUVar.CopyFromDevice(detox_dev, detox);

            prep_prof.Time(1);

            for (int idx = 0; idx < im_size * im_size; idx++)
                if (pix_out_of_nerve[idx] == 0)
                    pix_idx[pix_idx_num++] = idx;

            prep_prof.Time(2);

            float[,] AxCorPix = new float[mdl.n_axons, 3]; // axon coordinate in pixels

            // ======== Individual Axon Properties Initialization =========

            int[] box_y_min = new int[mdl.n_axons],
                box_y_max = new int[mdl.n_axons],
                box_x_min = new int[mdl.n_axons],
                box_x_max = new int[mdl.n_axons],
                box_siz_x = new int[mdl.n_axons],
                box_siz_y = new int[mdl.n_axons];


            Func<int, int, uint> xy_to_lin_idx = (x, y) => (uint)x * (uint)im_size + (uint)y;
            Func<uint, uint, uint> xy_to_lin_idx_u = (x, y) => (uint)x * (uint)im_size + (uint)y;

            for (int i = 0; i < mdl.n_axons; i++)
            {
                axon_is_large[i] = mdl.axon_coor[i][2] > axon_max_r_mean;

                axon_is_init_insult[i] = Pow2(insult_r - mdl.axon_coor[i][2]) > Pow2(insult_x - mdl.axon_coor[i][0]) + Pow2(insult_y - mdl.axon_coor[i][1]);

                // Change coordinates from um to pixels
                float xCenter = nerve_cent_pix + mdl.axon_coor[i][0] * res;
                float yCenter = nerve_cent_pix + mdl.axon_coor[i][1] * res;
                float radiusCircle = mdl.axon_coor[i][2] * res;
                AxCorPix[i, 0] = xCenter; AxCorPix[i, 1] = yCenter; AxCorPix[i, 2] = radiusCircle;
                death_itr[i] = 0;
                axons_cent_pix[i] = xy_to_lin_idx_u((uint)xCenter, (uint)yCenter);
                id_center_axon[xy_to_lin_idx_u((uint)xCenter, (uint)yCenter)] = (uint) i;
                axon_lbl[i] = new AxonLabelClass { lbl = "", x = xCenter, y = yCenter };
                axons_surr_rate_idx[i + 1] = axons_surr_rate_idx[i];

                float rc_1 = radiusCircle + process_clearance;
                box_y_min[i] = Max((int)(yCenter - rc_1), 0);
                box_y_max[i] = Min((int)(yCenter + rc_1), im_size - 1);
                box_x_min[i] = Max((int)(xCenter - rc_1), 0);
                box_x_max[i] = Min((int)(xCenter + rc_1), im_size - 1);
                box_siz_x[i] = box_y_max[i] - box_y_min[i] + 2;
                box_siz_y[i] = box_x_max[i] - box_x_min[i] + 2;
            }
            prep_prof.Time(3);

            for (int i = 0; i < mdl.n_axons; i++)
            {
                bool[,] is_inside_this_axon = new bool[box_siz_x[i], box_siz_y[i]];
                axons_inside_pix_idx[i + 1] = axons_inside_pix_idx[i];

                for (int y = box_y_min[i]; y <= box_y_max[i]; y++)
                    for (int x = box_x_min[i]; x <= box_x_max[i]; x++)
                    {
                        float dx = (float)x - AxCorPix[i, 0];
                        float dy = (float)y - AxCorPix[i, 1];
                        bool inside = AxCorPix[i, 2] * AxCorPix[i, 2] - (dx * dx + dy * dy) > 0;
                        if (inside)
                        { // inside axon
                            uint lin_idx = xy_to_lin_idx(x, y);
                            is_inside_this_axon[x - box_x_min[i], y - box_y_min[i]] = true;
                            axon_mask[lin_idx] = 1; // alive
                            axons_inside_pix[axons_inside_pix_idx[i + 1]++] = lin_idx;
                            // [DWL] Make tox production per pixel inverse proportional with
                            //       axon circumference (2*pi*r)
                            tox_prod[lin_idx] = 2*k_tox_prod / (mdl.axon_coor[i][2] * res);
                            detox[lin_idx] = k_detox_intra;
                            if (axon_is_init_insult[i])
                                tox[lin_idx] = insult_tox;
                        }
                    }
                prep_prof.Time(4);

                int cnt1 = 0, cnt2 = 0;

                for (int y = box_y_min[i] + 1; y < box_y_max[i]; y++)
                    for (int x = box_x_min[i] + 1; x < box_x_max[i]; x++)
                    {
                        int x_rel = x - box_x_min[i];
                        int y_rel = y - box_y_min[i];

                        int[] neighbors_x = new int[] { x_rel + 1, x_rel - 1, x_rel, x_rel };
                        int[] neighbors_y = new int[] { y_rel, y_rel, y_rel + 1, y_rel - 1 };

                        for (uint k = 0; k < 4; k++)
                        {
                            bool xy_inside = is_inside_this_axon[x_rel, y_rel];
                            bool neigh_k_inside = is_inside_this_axon[neighbors_x[k], neighbors_y[k]];

                            uint lin_idx = xy_to_lin_idx(x, y) * 4 + k;

                            if (xy_inside != neigh_k_inside)
                            {
                                // rate[lin_idx] = k_rate_boundary;
                            }
                            else if (xy_inside)
                            {
                                // rate[lin_idx] = k_rate_live_axon;
                                axons_surr_rate[axons_surr_rate_idx[i + 1]++] = lin_idx;
                            }
                        }
                    }
                if (cnt1 / 2 != cnt2)
                    Debug.WriteLine("Increase clearance");
                prep_prof.Time(5);
                // Verify radius
                // Debug.WriteLine("{0} vs {1}", (Math.Pow(mdl.axon_coor[i][2] * res, 2) * Math.PI).ToString("0.0"), axons_inside_pix_idx[i + 1] - axons_inside_pix_idx[i]);
            }

            //localGPUVar.Launch(grid_siz_prep, block_siz_prep).cuda_prep1(im_size, pix_out_of_nerve_dev, rate_dev);
            //localGPUVar.CopyFromDevice(rate_dev, rate);

            prep_prof.Time(6);

            // Keep backup of inital state 

            //tox_init = null; tox_init = (float[,])tox.Clone();
            //rate_init = null; rate_init = (float[,,])rate.Clone();
            //detox_init = null; detox_init = (float[,])detox.Clone();
            //tox_init = null; tox_init = null; tox_prod_init = (float[,])tox_prod.Clone();
            //axon_mask_init = null; axon_mask_init = (byte[,])axon_mask.Clone();
            //axon_is_alive_init = null; axon_is_alive_init = (bool[])axon_is_alive.Clone();

            Reset_state();

            // variable size study
            //((rate.Length + tox.Length + detox.Length + tox_prod.Length + axon_mask.Length + axon_is_alive.Length)*4)/1024/1024 // MB

            Update_bottom_stat("Preprocess Done! (" + (Toc() / 1000).ToString("0.0") + " secs)");
            // Debug.WriteLine("inside: {0} vs allocated {1}", axons_inside_pix_idx[mdl.n_axons - 1], axons_inside_pix.Length);

            prep_prof.report();
        }
    }
}
