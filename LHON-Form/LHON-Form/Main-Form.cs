using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;

using Cudafy;
using Cudafy.Host;

// Speed inside bundle: Fastest, outside bundles slower, boundaries slowest, 

namespace LHON_Form
{
    //[System.ComponentModel.DesignerCategory("Form")]
    public partial class Main_Form : Form
    {

        private void Main_Form_Load(object sender, EventArgs e)
        {
            alg_worker.DoWork += (s, ev) => Run_Alg_GPU(); alg_worker.WorkerSupportsCancellation = true;
            new_model_worker.DoWork += (s, ev) => New_model();

            Init_settings_gui();

            if (Init_gpu())
            {
                MessageBox.Show("No Nvidia GPU detected! This program requires an Nvidia GPU.", "Fatal Error");
                this.Close();
                return;
            }

            string[] fileEntries = Directory.GetFiles(ProjectOutputDir + @"Models\");
            if (fileEntries.Length > 0) Load_model(fileEntries[fileEntries.Length - 1]);

            fileEntries = Directory.GetFiles(ProjectOutputDir + @"Settings\");
            if (fileEntries.Length > 0) Load_settings(fileEntries[fileEntries.Length - 1]);

            if (mdl.n_axons > 0 && mdl.n_axons < 100000 && setts.resolution > 0)
                Preprocess_model();
        }

        // =============================== MAIN LOOP

        private bool en_prof = false;
        private float dt;
        private float lvl_tox_last = 0;
        private int duration_of_no_change = 0; // itr
        private int stop_sim_at_duration_of_no_change = 2000; // itr

        private bool tox_switch = false;
        private int dstl = 0;
        private int tl = 0;
        private int ml = 0;
        private int bl = 0;

        private int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        unsafe private void Run_Alg_GPU()
        {
            gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId); // should be reloaded for reliability
            alg_prof.Time(0);

            int gui_iteration_period = 10;

            if (iteration == 0)
            {
                Load_gpu_from_cpu();
                tt_sim.Restart();
                Tic();
                dt = 1F / Pow2(setts.resolution);
                gui_iteration_period = Read_int(txt_rec_inerval);
            }
            tt_sim.Start();
            
            headLayer = 2;
            int layerToDisplay = setts.layerToDisplay % (2 + setts.no3dLayers);
            int totalPlanes = setts.no3dLayers + 2;

            while (true)
            {
                iteration++;
                realTime += dt;

                bool update_gui = iteration % gui_iteration_period == 0;

                alg_prof.Time(-1);

                int offset = 0;
                if(setts.no3dLayers != 0)
                {
                    offset = headLayer * im_size * im_size;
                }
                
                gpu.Launch(blocks_per_grid_1D_axons, threads_per_block_1D).cuda_update_live(mdl.n_axons, tox_dev, rate_dev, detox_dev, tox_prod_dev, on_death_tox, k_detox_extra, death_thr_array_dev,
                    axons_cent_pix_dev, axons_inside_pix_dev, axons_inside_pix_idx_dev, axon_surr_rate_dev, axon_surr_rate_idx_dev,
                    axon_is_alive_dev, axon_mask_dev, num_alive_axons_dev, death_itr_dev, iteration, offset);
                
                if (en_prof) { gpu.Synchronize(); alg_prof.Time(1); }

                

                if(setts.no3dLayers == 0)
                {
                    gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion1(pix_idx_dev, pix_idx_num, im_size,
                        tox_switch ? 1 : 0, tox_dev, detox_dev, tox_prod_dev, rate_dev, rate_values_dev, rate_dimensions);
                }
                else
                {
                    dstl = Mod(headLayer - 2, totalPlanes);
                    tl = Mod(headLayer, totalPlanes);
                    ml = Mod(headLayer, totalPlanes);
                    bl = Mod(headLayer+1, totalPlanes);

                    bool injury = setts.toxLayerStart == 0;

                    gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion2(pix_idx_dev, pix_idx_num, im_size,
                                            tox_dev, detox_dev, tox_prod_dev, rate_dev, rate_values_dev, rate_dimensions,
                                            dstl, tl, ml, bl, 1, 0, injury ? 1 : 0);

                    for (int j = 1; j < setts.no3dLayers-1; j++)
                    {
                        dstl = Mod(headLayer - 2 + j, totalPlanes);
                        tl   = Mod(headLayer - 1 + j, totalPlanes);
                        ml   = Mod(headLayer + j, totalPlanes);
                        bl   = Mod(headLayer + j + 1, totalPlanes);
                        injury = (j >= setts.toxLayerStart) && (j <= setts.toxLayerStop);
                        gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion2(pix_idx_dev, pix_idx_num, im_size,
                                            tox_dev, detox_dev, tox_prod_dev, rate_dev, rate_values_dev, rate_dimensions,
                                            dstl, tl, ml, bl, 0, 0, injury ? 1 : 0);
                    }

                    dstl = Mod(headLayer - 2 + setts.no3dLayers - 1, totalPlanes);
                    tl   = Mod(headLayer - 1 + setts.no3dLayers - 1, totalPlanes);
                    ml   = Mod(headLayer   + setts.no3dLayers - 1, totalPlanes);
                    bl   = Mod(headLayer   + setts.no3dLayers - 1, totalPlanes);
                    injury = (setts.no3dLayers - 1 <= setts.toxLayerStop);
                    gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion2(pix_idx_dev, pix_idx_num, im_size,
                                            tox_dev, detox_dev, tox_prod_dev, rate_dev, rate_values_dev, rate_dimensions,
                                            dstl, tl, ml, bl, 0, 1, injury ? 1 : 0);

                    headLayer = Mod(headLayer - 2, totalPlanes);
                    if (setts.no3dLayers < 0)
                    {
                        Debug.WriteLine("ERROR: no layers is less than zero!");
                    }
                    if(headLayer < 0)
                    {
                        Debug.WriteLine("ERROR: headLayer is less than zero!");
                    }
                    
                }


                /*
                gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion3(pix_idx_dev, pix_idx_num, im_size,
                    tox_switch ? 1 : 0, tox_dev, rate_dev, detox_dev, tox_prod_dev, id_center_axon_dev, on_death_tox, k_rate_dead_axon, k_detox_extra, death_tox_thres,
                    axons_cent_pix_dev, axons_inside_pix_dev, axons_inside_pix_idx_dev, axon_surr_rate_dev, axon_surr_rate_idx_dev,
                    axon_is_alive_dev, axon_mask_dev, num_alive_axons_dev, death_itr_dev, iteration);
                */
                tox_switch = !tox_switch;

                //gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion2(pix_idx_dev, pix_idx_num, tox_new_dev, tox_dev);

                if (en_prof) { gpu.Synchronize(); alg_prof.Time(2); }

                if (update_gui)
                {
                    //gpu.CopyFromDevice(axon_is_alive_dev, axon_is_alive); // ?
                    gpu.CopyFromDevice(num_alive_axons_dev, num_alive_axons);

                    // Calc tox_sum for sanity check
                    gpu.Set(sum_tox_dev);
                    if (setts.no3dLayers != 0)
                    {
                        offset = headLayer * im_size * im_size;
                    }
                    else
                    {
                        offset = 0;
                    }
                    gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_tox_sum(pix_idx_dev, pix_idx_num, tox_dev, sum_tox_dev, offset);
                    gpu.CopyFromDevice(sum_tox_dev, out sum_tox);

                    if ((stop_at_iteration == 0) && Math.Abs(sum_tox - lvl_tox_last) < 1000F) { 
                    
                      duration_of_no_change += gui_iteration_period;
                      if (duration_of_no_change >= stop_sim_at_duration_of_no_change)
                      {
                        Stop_sim(Sim_stat_enum.Successful);
                      }
                        
                    }
                    else
                    {
                        lvl_tox_last = sum_tox;
                        duration_of_no_change = 0;
                    }

                    if (max_sum_tox < sum_tox)
                    {
                        max_sum_tox = sum_tox;
                    }

                    Update_gui_labels();

                    if (en_prof) { gpu.Synchronize(); alg_prof.Time(3); }

                    if (setts.no3dLayers != 0)
                    {
                        layerToDisplay = Mod(headLayer  + setts.layerToDisplay, totalPlanes);
                    }

                    Update_bmp_image(layerToDisplay);

                    if (sim_stat == Sim_stat_enum.Running && chk_rec_avi.Checked)
                        Record_bmp_gif();

                    if (en_prof) alg_prof.Time(4);
                }

                if (sim_stat != Sim_stat_enum.Running) break;
                if (iteration == stop_at_iteration || (stop_at_time > 0 && realTime >= stop_at_time))
                    Stop_sim(Sim_stat_enum.Successful); // >>>>>>>>>>>>>>>>>> TEMP should be Paused

                if (main_loop_delay > 0)
                    Thread.Sleep(main_loop_delay * 10);
            }

            tt_sim.Pause();

            if (en_prof) alg_prof.report();
            else Debug.WriteLine("Sim took " + (Toc() / 1000).ToString("0.000") + " secs\n");

            simulationDone = true;
        }


        // ==================== Reset State  =======================

        private void Reset_state()
        {
            if (InvokeRequired)
                Invoke(new Action(() => Reset_state()));
            else
            {
                sum_tox = 0;
                max_sum_tox = 0;
                for (int y = 0; y < im_size; y++)
                    for (int x = 0; x < im_size; x++)
                        sum_tox += tox[x * im_size + y];

                iteration = 0;
                duration_of_no_change = 0;
                realTime = 0;

                Update_gui_labels();

                for (int i = 0; i < mdl.n_axons; i++) axon_lbl[i].lbl = "";

                prog_im_siz = prog_im_siz_default;
                resolution_reduction_ratio = (double)prog_im_siz / (double)im_size;
                if (resolution_reduction_ratio > 1)
                {
                    resolution_reduction_ratio = 1;
                    prog_im_siz = (ushort)im_size;
                }
                //areal_progression_image_stack = new byte[progress_num_frames, prog_im_siz, prog_im_siz];
                //chron_progression_image_stack = new byte[progress_num_frames, prog_im_siz, prog_im_siz];
                //areal_progress_chron_val = new float[progress_num_frames];
                //chron_progress_areal_val = new float[progress_num_frames];

                num_alive_axons[0] = mdl.n_axons - 1;

                Load_gpu_from_cpu();

                Update_show_opts();

                Update_init_insult();
                Update_bmp_image(0);
                PicB_Resize(null, null);

                sim_stat = Sim_stat_enum.None;

            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void txt_on_death_tox_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl_tox_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }


        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
