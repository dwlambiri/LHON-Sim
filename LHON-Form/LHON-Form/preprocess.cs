﻿
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

        private readonly float min_res_c = 10F;
        private int process_clearance = 2;

        // ========================
        //      Parameters
        // ========================

        /* Units:
        [DWL] Changed the constants from umol to zeptomol (zmol). 
        [DWL] The realTime constant has to be x*minutes or some other larger value
        [DWL] We know that the evolution is about 6 to 9 months. Thus
        [DWL] the iteration constant must be 9month/numIter.
        [DWL] RTU = real realTime unit (realTime measured in units of physical realTime)
        Constant                        UI Unit                  Algorithm Unit          Conversion Factor
        k_detox, k_rate                  1 / RTU                   1 / itr               K / timeRes
        k_tox_prod                       zmol / um^2 /RTU          tox / pix / itr       K / spatialRes ^ 2 / timeRes
        death_tox_thres, insult_tox      zmol / um^2               tox / pix             K / spatialRes ^ 2
        
            Value RTU          = (Value / timeRes)  itr
            Value zmol / um^2  = (Value / spatialRes ^ 2)  zmol / pix
        */

        private float k_detox_intra, k_detox_extra, k_tox_prod, death_tox_thres, death_var_thr, insult_tox, on_death_tox,
            k_rate_live_axon, k_rate_boundary, k_rate_dead_axon, k_rate_extra;


        // ====================================
        //              Variables
        // ====================================

        private float[] tox, tox_dev; // Tox
        private byte[] rate, rate_dev; // Rate
        private float[] rate_values, rate_values_dev; // vector of diffusion rate values. rate_value[0]=0; rate[5]=1;
        private float[] detox, detox_dev; // Detox
        private float[] tox_prod, tox_prod_dev; // Tox_prod
        private float[] death_thr_array, death_thr_array_dev; // [DWL] death threshold for each axon; either constant or dependent on location in optic nerve
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

        private readonly byte diff_live_index = 1;
        private readonly byte diff_bound_index = 2;
        private readonly byte diff_dead_index = 3;
        private readonly byte diff_extra_index = 4;

        private readonly uint rateUpLayerIndex = 4;
        private readonly uint rateDownLayerIndex = 5;

        
        private readonly uint plane_neighbours = 4;
        private readonly uint space_neighbours = 6;

        private int rate_dimensions = 4;

        private ushort im_size;
        private bool preprocessDone = false;
        private bool simulationDone = false;

        private int headLayer = 2;

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

            float rate_conv = Pow2(res / min_res_c);
            float upperValue_c = 1F;
            if (setts.rate_live > upperValue_c ||
                setts.rate_bound > upperValue_c ||
                setts.rate_dead > upperValue_c ||
                setts.rate_extra > upperValue_c)
            {
                Append_stat_ln("Error: Diffusion param greater than " + upperValue_c.ToString() + ". Results are undetermined.");
                //return;
            }

            float lowerValue_c = 0F;
            if (setts.rate_live < lowerValue_c ||
                setts.rate_bound < lowerValue_c ||
                setts.rate_dead < lowerValue_c ||
                setts.rate_extra < lowerValue_c)
            {
                Append_stat_ln("Error: Diffusion param less  than " + lowerValue_c.ToString() + ". Results are undetermined.");
                //return;
            }

            
            if (setts.detox_intra > 1F ||
                setts.detox_extra> 1F)
            {
                Append_stat_ln("Error: Detox param greater than 1. Results are undetermined.");
                //return;
            }



            // "setts" are user input with physical units

            // 1 - real detox rate to reduce computation ->  tox[x_y] *= detox[x_y]
            k_detox_intra = 1F - setts.detox_intra;
            k_detox_extra = 1F - setts.detox_extra;
            k_tox_prod = 2*setts.tox_prod / Pow2(res);

            float fiveF = 5F;

            // User inputs 0 to 1 for rate values 
            k_rate_live_axon = setts.rate_live / fiveF * rate_conv;
            k_rate_boundary = setts.rate_bound / fiveF * rate_conv;
            k_rate_dead_axon = setts.rate_dead / fiveF * rate_conv;
            k_rate_extra = setts.rate_extra / fiveF * rate_conv;

            if (k_rate_live_axon > 0.25 || 
                k_rate_boundary > 0.25 ||
                k_rate_dead_axon > 0.25 ||
                k_rate_extra > 0.25)
            {
                Append_stat_ln("Error: Diffusion param greater than 0.25. Results are undetermined.");
                //return;
            }

            // 
            bool variableDeathThreshold = chk_var_thr.Checked;
            death_tox_thres = setts.death_tox_thres / Pow2(res);
            death_var_thr = setts.death_var_thr;
            insult_tox = setts.insult_tox / Pow2(res);
            on_death_tox = setts.on_death_tox / Pow2(res);

            if(setts.death_tox_thres + setts.on_death_tox > 220/ res)
            {
                Append_stat_ln("Info: Death Thr + On Death Extra > 220 / Resolution. SOX Density in axon too high.");
            }

            if(setts.detox_intra != 0 && setts.death_tox_thres != 0)
            {
                Append_stat_ln("Info: Axons with diameter under " + (4 * setts.tox_prod *(1-setts.detox_intra)/ setts.detox_intra / setts.death_tox_thres).ToString() +" um will die");
            }
            else
            {
                Append_stat_ln("Info: All axons will die...");
            }

            prep_prof.Time(0);
            Tic();

            Update_bottom_stat("Preprocessing ...");

            im_size = Calc_im_siz();
            Update_image_siz_lbl();

            Init_bmp_write();

            // ======== Pixel Properties =========
            rate_dimensions += setts.no3dLayers !=0 ? 2 : 0;
            rate = new byte[im_size * im_size * rate_dimensions];
            rate_values = new float[6];
            rate_values[0] = 0;
            rate_values[1] = k_rate_live_axon;
            rate_values[2] = k_rate_boundary;
            rate_values[3] = k_rate_dead_axon;
            rate_values[4] = k_rate_extra;
            rate_values[5] = 1;
            detox = new float[im_size * im_size];
            tox = new float[im_size * im_size];
            tox_prod = new float[im_size * im_size];

            id_center_axon = new uint[im_size * im_size];

            axon_mask = axon_mask_init = new byte[im_size * im_size]; // for display. if 1: axon is live, if 2: axon is dead, otherwise 0.
            pix_idx = new int[im_size * im_size]; // linear index of pixels within nerve

            // ======== Axon Properties =========

            axons_cent_pix = new uint[mdl.n_axons];
            death_thr_array = new float[mdl.n_axons];

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
            rate_dev = localGPUVar.Allocate<byte>(im_size * im_size * rate_dimensions);
            
            detox_dev = localGPUVar.Allocate<float>(im_size * im_size);
            pix_idx_dev = localGPUVar.Allocate<int>(im_size * im_size);
            byte[] pix_out_of_nerve_dev = localGPUVar.Allocate<byte>(im_size * im_size);
            byte[] pix_out_of_nerve = new byte[im_size * im_size];

            int prep_siz = 32;
            dim3 block_siz_prep = new dim3(prep_siz, prep_siz);
            int tmp = (int)Math.Ceiling((double)im_size / (double)prep_siz);
            dim3 grid_siz_prep = new dim3(tmp, tmp);

            //localGPUVar.CopyToDevice(rate_values, rate_values_dev);

            localGPUVar.Launch(grid_siz_prep, block_siz_prep).cuda_prep0(im_size, nerve_cent_pix, nerve_r_pix_2, vein_r_pix_2, k_rate_extra, k_detox_extra,
                pix_out_of_nerve_dev, rate_dev, detox_dev, rate_dimensions);

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

            uint noNeighbours = (setts.no3dLayers == 0) ? plane_neighbours : space_neighbours;

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

                if(variableDeathThreshold == false)
                {
                    death_thr_array[i] = death_tox_thres;
                }
                else
                {
                    death_thr_array[i] = death_tox_thres* (1 + death_var_thr * xCenter / im_size);
                }
                
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
                            //       *reaL* axon circumference (2*pi*r) => r is radius in um *not* pixels
                            // AxonTox = tox*pi*(pix)^2=2*k_tox*pix^2/(r*(res)^2)
                            //         =2*k_tox*(r*res)^2/r/res^2=2*k_tox*r
                            tox_prod[lin_idx] = k_tox_prod / (mdl.axon_coor[i][2]);
                            detox[lin_idx] = k_detox_intra;
                            // AxonInsult = i_pix*pi*pix^2= i_t/res^2*pi*pix^2=i_t/res^2*pi*(r*res)^2
                            //            = i_t*pi*r^2
                            // insult_tox has been resized before the assignment
                            if (axon_is_init_insult[i])
                                tox[lin_idx] = insult_tox;
                        }
                    }
                prep_prof.Time(4);

                int cnt1 = 0, cnt2 = 0;

                for (int y = box_y_min[i] + 1; y < box_y_max[i]; y++)
                {
                    for (int x = box_x_min[i] + 1; x < box_x_max[i]; x++)
                    {
                        int x_rel = x - box_x_min[i];
                        int y_rel = y - box_y_min[i];

                        int[] neighbors_x = new int[] { x_rel + 1, x_rel - 1, x_rel, x_rel };
                        int[] neighbors_y = new int[] { y_rel, y_rel, y_rel + 1, y_rel - 1 };

                        bool xy_inside = is_inside_this_axon[x_rel, y_rel];
                        uint lin_idx_base = xy_to_lin_idx(x, y) * noNeighbours;

                        for (uint k = 0; k < plane_neighbours; k++)
                        {
                            bool neigh_k_inside = is_inside_this_axon[neighbors_x[k], neighbors_y[k]];

                            uint lin_idx = lin_idx_base + k;

                            if (xy_inside != neigh_k_inside)
                            {
                                rate[lin_idx] = diff_bound_index;
                            }
                            else if (xy_inside)
                            {
                                rate[lin_idx] = diff_live_index;
                                axons_surr_rate[axons_surr_rate_idx[i + 1]++] = lin_idx;
                            }
                        }
                        if (xy_inside)
                        {
                            rate[lin_idx_base + rateDownLayerIndex] = diff_live_index;
                            rate[lin_idx_base + rateUpLayerIndex] = diff_live_index;
                        }
                    }
                }

                if (cnt1 / 2 != cnt2)
                    Debug.WriteLine("Increase clearance");
                prep_prof.Time(5);
                // Verify radius
                // Debug.WriteLine("{0} vs {1}", (Math.Pow(mdl.axon_coor[i][2] * res, 2) * Math.PI).ToString("0.0"), axons_inside_pix_idx[i + 1] - axons_inside_pix_idx[i]);
            }

            localGPUVar.CopyToDevice(rate, rate_dev);
            localGPUVar.Launch(grid_siz_prep, block_siz_prep).cuda_prep1(im_size, pix_out_of_nerve_dev, rate_dev, rate_dimensions);
            localGPUVar.CopyFromDevice(rate_dev, rate);

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

            preprocessDone = true;
            simulationDone = false;

            prep_prof.report();
        }
    }
}
