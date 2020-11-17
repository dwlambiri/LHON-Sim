using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;

namespace LHON_Form
{
    public partial class Main_Form : Form
    {
        private GifBitmapEncoder gifEnc = new GifBitmapEncoder();

        /*
        bool mouse_r_pressed;
        int mouse_r_press_x, mouse_r_press_y;
        picB.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    mouse_r_pressed = true;
                    mouse_r_press_x = e.X;
                    mouse_r_press_y = e.Y;
                }
        };
        picB.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    mouse_r_pressed = false;
                }
            };
        picB.MouseMove += (s, e) => if (mouse_r_pressed) ...
        */

        private int stop_at_iteration = 0;
        private float stop_at_time = 0;
        private int main_loop_delay = 0;

        public Main_Form()
        {
            InitializeComponent();
            this.CenterToScreen();
            DoubleBuffered = true;

            Init_sweep();

            chk_var_thr.CheckedChanged += (o, e) => {
                setts.varToxProd = chk_var_thr.Checked;
                };

            chk_show_live_axons.CheckedChanged += (o, e) => Update_show_opts();
            chk_show_dead_axons.CheckedChanged += (o, e) => Update_show_opts();
            chk_show_tox.CheckedChanged += (o, e) =>
            {
                    if (chk_show_tox.Checked == true)
                    {
                        direction_group_box.Enabled = true;
                        if (xy_direction_button.Checked) sox_track_bar_xy.Enabled = true;
                        if (yz_direction_button.Checked) sox_track_bar_yz.Enabled = true;
                        if (xz_direction_button.Checked) sox_track_bar_xz.Enabled = true;
                    }
                    else
                    {
                        direction_group_box.Enabled = false;
                        sox_track_bar_xz.Enabled = false;
                        sox_track_bar_xy.Enabled = false;
                        sox_track_bar_yz.Enabled = false;
                    }

                Update_show_opts();

            };
            txt_layer_to_display.TextChanged += (s, e) => Update_show_opts();


            txt_stop_itr.TextChanged += (s, e) => stop_at_iteration = Read_int(s);
            txt_stop_time.TextChanged += (s, e) => stop_at_time = Read_float(s);
            txt_delay_ms.TextChanged += (s, e) => main_loop_delay = Read_int(s);

            txt_block_siz.TextChanged += (s, e) => threads_per_block_1D = (ushort)Read_int(s);

            btn_reset.Click += (s, e) =>
            {
                if (rate == null)
                {
                    Append_stat_ln("Info: Calling preprocess.....");
                    Preprocess_model();
                    Set_btn_start_txt("&Start", System.Drawing.Color.Green); btn_start.Enabled = true;
                    Append_stat_ln("Info: Preprocessing complete.");
                    return;
                }
                if (sim_stat == Sim_stat_enum.Running)
                {
                    Append_stat_ln("Warning: You must stop the simulation before resetting the states.");
                    return;
                }
                Reset_state(); Set_btn_start_txt("&Start", System.Drawing.Color.Green); btn_start.Enabled = true;
                Update_bottom_stat("Simulation state was reset!");
            };

            btn_start.Click += (s, e) =>
            {
                if (sim_stat == Sim_stat_enum.None || sim_stat == Sim_stat_enum.Paused)
                {
                    
                    Start_sim();
                }
                else if (sim_stat == Sim_stat_enum.Running)
                {
                    Stop_sim(Sim_stat_enum.Paused);
                } 
            };

            btn_generate_model.Click += (s, e) => { // [DWL] Generate Model Button
                if (sim_stat != Sim_stat_enum.Running && !new_model_worker.IsBusy)
                {
                    Stop_sim(Sim_stat_enum.Stopped);
                    new_model_worker.RunWorkerAsync();
                }
            };

            btn_preprocess.Click += (s, e) =>
            {
                if (sim_stat == Sim_stat_enum.Running)
                {
                    Append_stat_ln("Warning: You must stop the simulation before preprogress.");
                    return;
                }
                Append_stat_ln("Info: Preprocessing model.");
                Preprocess_model();
                Append_stat_ln("Info: Preprocessing done.");
                Set_btn_start_txt("&Start", System.Drawing.Color.Green); btn_start.Enabled = true;
            };

            btn_clr.Click += (s, e) => txt_status.Text = "";

            cmb_sw_sel1.SelectedIndex = 0;
            cmb_sw_sel2.SelectedIndex = 0;

            chk_rec_avi.CheckedChanged += (s, e) =>
            {
                if (chk_rec_avi.Checked)
                {
                    chk_rec_avi.Text = "Recording";
#if false
                    avi_file = ProjectOutputDir + @"Recordings\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + '(' + (im_size * im_size / 1e6).ToString("0.0") + "Mpix).avi";
                    aviManager = new AviManager(avi_file, false);
                    Avi.AVICOMPRESSOPTIONS options = new Avi.AVICOMPRESSOPTIONS();
                    options.fccType = (uint)Avi.mmioStringToFOURCC("vids", 5);
                    options.fccHandler = (uint)Avi.mmioStringToFOURCC("CVID", 5);
                    options.dwQuality = 1;
                    aviStream = aviManager.AddVideoStream(options, 10, bmp); 
#endif
                }
                else
                {
                    chk_rec_avi.Text = "Record";
#if false
                    aviManager.Close();
                    Process.Start(avi_file);
#endif
                    if (gifEnc.Frames.Count > 0)
                    {
                        var path = ProjectOutputDir + @"Recordings\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + '(' + (im_size * im_size / 1e6).ToString("0.0") + "Mpix).gif";
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                            gifEnc.Save(fs);
                    }
                }
            };

            chk_save_sw_prog.CheckedChanged += (s, e) =>
            {
                if (chk_save_sw_prog.Checked) chk_save_sw_prog.Text = "Saving Sweep";
                else chk_save_sw_prog.Text = "Save Sweep";
            };

            btn_save_prog.Click += (s, e) =>
            {
                Save_Progress(ProjectOutputDir + @"Progression\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + ".prgim");
            };

            Append_stat_ln("Welcome to LHON-2D/3D Simulation software!\n");

            btn_snapshot.Click += (s, e) =>
            {
                string adr = ProjectOutputDir + @"Snapshots\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + ".jpg";
                Append_stat_ln("Snapshot saved to: " + adr);
                bmp.Save(adr);
            };

            txt_block_siz.Text = threads_per_block_1D.ToString("0");

            picB.MouseWheel += (s, e) =>
            {
                if (simInProgress == false) {
                    float[] um = get_mouse_click_um(e);
                    float dx = insult_x - um[0], dy = insult_y - um[1];
                    float dist = dx * dx + dy * dy;
                    if (insult_r * insult_r - dist > 0 || dist < 100)
                    {
                        insult_r += (float)e.Delta / 100;
                        if (insult_r < 0) insult_r = 0;
                        Update_init_insult();
                        Update_bmp_image(0, 0, im_size * im_size);
                        //Debug.WriteLine(insult_r);
                    }
                }
            };

            picB.Click += (s, e) => {

                int[] um = get_mouse_location(e as MouseEventArgs);

               tox_image_value.Show( (1000*bmp_tox[um[0]+um[1]* bmp_im_size]).ToString() + "*10^-3", picB, um[2], um[3], 10000);


            };

            //picB.Click += (s, e) => mouse_click(e as MouseEventArgs);
        }

        private void Update_show_opts()
        {

            if(preprocessDone == false)
            {
                return;
            }
            show_opts[0] = chk_show_live_axons.Checked;
            show_opts[1] = chk_show_dead_axons.Checked;
            show_opts[2] = chk_show_tox.Checked;
            //setts.layerToDisplay = Read_int(txt_layer_to_display);

            gpu.CopyToDevice(show_opts, show_opts_dev);
            int layerToDisplay = 0;
            int currentShow = showdir;
            if (setts.no3dLayers != 0)
            {
                if (showdir == 0)
                {
                    layerToDisplay = Mod(headLayer + Mod(setts.layerToDisplay, setts.no3dLayers), setts.no3dLayers +2);
                }
                else
                {
                    layerToDisplay = setts.layerToDisplay;
                }
            }
            else
            {
                currentShow = 0;
            }
            if(sim_stat != Sim_stat_enum.Running)
                Update_bmp_image(currentShow, layerToDisplay, im_size* im_size);
            //Update_bmp_image(0);
        }

        // ====================================================================
        //                       Start / Stop Simulation
        // ====================================================================
        private void Start_sim()
        {
            if (rate == null)
            {
                Append_stat_ln("You must preprocess the model before running simulation.\n");
                return;
            }
            if (sim_stat == Sim_stat_enum.None || sim_stat == Sim_stat_enum.Stopped || sim_stat == Sim_stat_enum.Successful || sim_stat == Sim_stat_enum.Failed)
            {
                DisableButtons();

                sim_stat = Sim_stat_enum.Running;
                alg_worker.RunWorkerAsync();
                Set_btn_start_txt("&Pause", System.Drawing.Color.DarkRed);
                Update_bottom_stat("Simulation is Running");
            } else if(sim_stat == Sim_stat_enum.Paused)
            {
                DisableButtons();

                sim_stat = Sim_stat_enum.Running;
                Set_btn_start_txt("&Pause",  System.Drawing.Color.DarkRed);
                Update_bottom_stat("Simulation is Running");
            }
        }

        private void Stop_sim(Sim_stat_enum stat)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Stop_sim(stat)));
            else
            {
                if (sim_stat == Sim_stat_enum.Running)
                {
                    sim_stat = stat;
                    if (sim_stat == Sim_stat_enum.Paused)
                    {
                        Set_btn_start_txt("&Continue", System.Drawing.Color.Green);
                    }
                    else
                    {
                        Set_btn_start_txt("&Start", System.Drawing.Color.Gray);
                        btn_start.Enabled = false;
                    }
                    Update_bottom_stat("Simulation is " + sim_stat.ToString());
                }
                else if (sim_stat == Sim_stat_enum.Paused)
                {
                    sim_stat = stat;
                    Set_btn_start_txt("&Start", System.Drawing.Color.Green);
                    Update_bottom_stat("Simulation is " + sim_stat.ToString());
                }
                EnableButtons();
            }
        }
        // ====================================================================
        //                           Low level GUI
        // ====================================================================

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            sim_stat = Sim_stat_enum.Paused;
            Thread.Sleep(10);
        }

        private void Set_btn_start_txt(string s, System.Drawing.Color color)
        {
            if (InvokeRequired) Invoke(new Action(() => { btn_start.Text = s; btn_start.BackColor = color; }));
            else
            {
                btn_start.Text = s;
                btn_start.BackColor = color;
            }
        }

        private void Update_num_axons_lbl()
        {
            if (InvokeRequired)
                Invoke(new Action(() => Update_num_axons_lbl()));
            else
                lbl_num_axons.Text = mdl.n_axons.ToString() + " Expected: " +
                    (Math.Pow(mdl.nerve_scale_ratio, 2) * mdl_real_num_axons).ToString("0");
        }

        private void Update_mdl_prog(float prog)
        {
            Update_bottom_stat("Generating Model ... " + (prog * 100).ToString("0.0") + " %");
        }

        private void Update_image_siz_lbl()
        {
            string s = string.Format("{0} x {0}", im_size);
            if (InvokeRequired) Invoke(new Action(() => lbl_image_siz.Text = s));
            else lbl_image_siz.Text = s;
        }

        private void Update_bottom_stat(string s)
        {
            statlbl.Text = s;
            if (InvokeRequired) Invoke(new Action(() => statlbl.Text = s));
            else statlbl.Text = s;
        }

        private void update_stat_sw_lbl(string s)
        {
            if (InvokeRequired) Invoke(new Action(() => statlbl_sweep.Text = s));
            else statlbl_sweep.Text = s;
        }

        private void Append_stat(string s)
        {
            if (InvokeRequired) Invoke(new Action(() => Append_stat(s)));
            else txt_status.AppendText(s.Replace("\n", Environment.NewLine));
        }

        private void Append_stat_ln(string s) { Append_stat(s + Environment.NewLine); }

        private uint prev_itr = 0;
        private float prev_itr_t = 0;

        private void Update_gui_labels()
        {
            Invoke(new Action(() =>
            {
                float now = tt_sim.Read();
                lbl_itr.Text = iteration.ToString("0");
                if(sum_tox < 100)
                {
                    lbl_tox.Text = (sum_tox).ToString("0.0") + " yMol";
                }
                else
                {
                    lbl_tox.Text = (sum_tox / 1000).ToString("0.0") + " zMol";
                }
                
                if( max_sum_tox < 100)
                {
                    lbl_max_tox.Text = (max_sum_tox).ToString("0.0") + " yMol";
                }
                else
                {
                    lbl_max_tox.Text = (max_sum_tox / 1000).ToString("0.0") + " zMol";
                }
                
                // [DWL] yMol/area/height yM/V
                // OXYGEN: 5L of molecular oxygen in body
                //         5L / 0.1 m^3 = 50L / m^3
                //         1 mol ~ 22.4 L => 1L ~ 1/22.4 mol
                //         5/22.4 mol of oxygen in body
                //         density = 50/22.4 mol/m^3 = 32*50/22.4 mol/m^3 = 71.4 g/m^4
                //         SOX has to be x percent of the overall density to a max of 100%
                //         possible SOX upper limit  < 10% => 7 g / m^3 = 7000 mg / m^3
                // DeathThr / Volume = the ymol*res/um^3 = thr*10^-24*res*10^18= thr*res mM/L < UpperLimit
                // thr < UpperLimit/32*10^3/res ~ 220 / resolution
                // thr+onDeath < UpperLimit/32*10^3/res ~ 220/ resolution
                // dead diam < 4*prod/(scav*thr)
                //Append_stat_ln(" mdl_nerve_r " + mdl_nerve_r + " resolution " + setts.resolution);
                lbl_max_density.Text = (max_sum_tox/ (Pow2(mdl_nerve_r) * Math.PI * (setts.no3dLayers+1)/ setts.resolution )).ToString("0.00") + " mM/L";  //  in mMol/l => max should not be over 35
                lbl_density.Text = (sum_tox  / (Pow2(mdl_nerve_r) * Math.PI * (setts.no3dLayers + 1)/ setts.resolution )).ToString("0.00") + " mM/L"; //  in mMol/l => max should not be over 35
                lbl_alive_axons_perc.Text = ((float)num_alive_axons[0] * 100 / mdl.n_axons).ToString("0.0") + "%";
                var span = TimeSpan.FromSeconds(now / 1000);
                lbl_sim_time.Text = string.Format("{0:00}:{1:00}:{2:00}", span.Minutes, span.Seconds, span.Milliseconds);

                float itr_p_s = 0;
                if (sim_stat == Sim_stat_enum.Running)
                {
                    if (iteration < prev_itr || now < prev_itr_t)
                        itr_p_s = iteration / now * 1000;
                    else
                        itr_p_s = (iteration - prev_itr) / (now - prev_itr_t) * 1000;

                    prev_itr_t = now;
                    prev_itr = iteration;
                }

                string s = itr_p_s.ToString("0.0");
                lbl_itr_s.Text = s;

                float x = (float)iteration / last_itr;
                float rat = 0.3F;
                float m = itr_p_s / (1 - x * rat);
                float estimated_total_itr_s = (m + (m * rat) * (1F - x)) / 2;

                if (!float.IsInfinity(estimated_total_itr_s) && !float.IsNaN(estimated_total_itr_s) && estimated_total_itr_s > 0)
                {
                    span = TimeSpan.FromSeconds((last_itr - iteration) / estimated_total_itr_s);
                    //lbl_density.Text = string.Format("{0}:{1:00}:{2:00}", (int)span.TotalHours, span.Minutes, span.Seconds);
                }
                
            }));
        }

        // ====================================================================
        //                               Settings
        // ====================================================================

        private bool model_is_saved = false;
        private long model_id = 0;

        private void Init_settings_gui()
        {

            chk_rand_mult.CheckedChanged += (s, e) =>
            {

                if (sim_stat != Sim_stat_enum.Running)
                {
                    setts.useRandProdFactor = chk_rand_mult.Checked;
                    SimParamsChanged();
                }

            };

            // Model parameters
            txt_nerve_scale.TextChanged += (s, e) =>
            {

                if (sim_stat != Sim_stat_enum.Running)
                {
                    preprocessDone = false;
                    sim_stat = Sim_stat_enum.None;

                    mdl.nerve_scale_ratio = Read_float(s) / 100F;
                    lbl_nerve_siz.Text = (mdl.nerve_scale_ratio * mdl_real_nerve_r * 2).ToString(".0") + " um";
                    SimParamsChanged();
                }
            };

            // Preprocess parameters

            txt_resolution.TextChanged += (s, e) =>
            {
                

                if (sim_stat != Sim_stat_enum.Running)
                {
                    preprocessDone = false;
                    sim_stat = Sim_stat_enum.None;
                    setts.resolution = Read_float(s);
                    setts.no3dLayers = (int) (setts.resolution * Read_float(txt_3d_layers));
                    setts.toxLayerStart = (int)(setts.resolution * Read_float(txt_3d_tox_start));
                    setts.toxLayerStop = (int)(setts.resolution * Read_float(txt_3d_tox_stop));
                    SimParamsChanged();
                }

            };

            txt_rec_interval.TextChanged += (s, e) => {

                int tmp = Read_int(txt_rec_interval);
                if(tmp <= 0 )
                {
                    tmp = 1;
                    txt_rec_interval.Text = "1"; // minimum value!!
                }
                setts.gui_iteration_period = tmp;
            };

            txt_detox_extra.TextChanged += (s, e) =>
            {
                setts.detox_extra = Read_float(s);
                SimParamsChanged();
            };
            txt_detox_intra.TextChanged += (s, e) =>
            {
                setts.detox_intra = Read_float(s);
                SimParamsChanged();
            };

            txt_rate_bound_a2e.TextChanged += (s, e) =>
            {
                setts.rate_bound_a2e = Read_float(s);
                SimParamsChanged();
            };

            txt_rate_bound_e2a.TextChanged += (s, e) =>
            {
                setts.rate_bound_e2a = Read_float(s);
                SimParamsChanged();
            };
            txt_rate_dead.TextChanged += (s, e) =>
            {
                setts.rate_dead = Read_float(s);
                SimParamsChanged();
            };
            txt_rate_extra.TextChanged += (s, e) =>
            {
                setts.rate_extra = Read_float(s);
                SimParamsChanged();
            };
            txt_rate_extra_z.TextChanged += (s, e) =>
            {
                setts.rate_extra_z = Read_float(s);
                SimParamsChanged();
            };
            txt_rate_live.TextChanged += (s, e) =>
            {
                setts.rate_live = Read_float(s);
                SimParamsChanged();
            };
            txt_rate_live_z.TextChanged += (s, e) =>
            {
                setts.rate_live_z = Read_float(s);
                SimParamsChanged();
            };
            txt_tox_prod_rate.TextChanged += (s, e) =>
            {
                setts.tox_prod = Read_float(s);
                SimParamsChanged();
            };
            txt_death_tox_threshold.TextChanged += (s, e) =>
            {
                setts.death_tox_thres = Read_float(s);
                SimParamsChanged();
            };
            txt_var_death.TextChanged += (s, e) =>
            {
                setts.death_var_thr = Read_float(s);
                SimParamsChanged();
            };
            txt_insult_tox.TextChanged += (s, e) =>
            {
                setts.insult_tox = Read_float(s);
                SimParamsChanged();
            };
            txt_on_death_tox.TextChanged += (s, e) =>
            {
                setts.on_death_tox = Read_float(s);
                SimParamsChanged();
            };

            txt_clearance.TextChanged += (s, e) =>
            {
                mdl_clearance = Read_float(s);
            };
            txt_3d_layers.TextChanged += (s, e) =>
            {
                if (sim_stat != Sim_stat_enum.Running)
                {
                    Stop_sim(Sim_stat_enum.Stopped);
                    float layers = Read_int(s);
                    setts.no3dLayers = (int)(setts.resolution * layers);
                    setts.toxLayerStart = (int)(setts.resolution * Read_float(txt_3d_tox_start));
                    setts.toxLayerStop = (int)(setts.resolution * Read_float(txt_3d_tox_stop));
                    if (setts.no3dLayers == 0)
                    {
                        xy_direction_button.Checked = true;
                        xz_direction_button.Checked = false;
                        yz_direction_button.Checked = false;
                        xy_direction_button.Enabled = true;
                        xz_direction_button.Enabled = false;
                        yz_direction_button.Enabled = false;
                        txt_rate_extra_z.Enabled = false;
                        txt_rate_live_z.Enabled = false;
                    }
                    else
                    {
                        xy_direction_button.Enabled = true;
                        xz_direction_button.Enabled = true;
                        yz_direction_button.Enabled = true;
                        txt_rate_extra_z.Enabled = true;
                        txt_rate_live_z.Enabled = true;
                    }
                    Append_stat_ln("Info: Number of layers changed. Call preprocess next.....");
                    //Preprocess_model();
                    //Append_stat_ln("Info: Preprocessing complete.");
                    Set_btn_start_txt("&Start", System.Drawing.Color.Gray); btn_start.Enabled = false;
                }
                else
                {
                    Append_stat_ln("Warning: Cannot change the number of layers while the sim is running.");
                }

            };

            txt_3d_tox_start.TextChanged += (s, e) =>
            {
                if (sim_stat != Sim_stat_enum.Running)
                {

                    setts.toxLayerStart = (int)(setts.resolution * Read_float(s));
                   
                    SimParamsChanged();
                }
            };

            txt_3d_tox_stop.TextChanged += (s, e) =>
            {
                if (sim_stat != Sim_stat_enum.Running)
                {
                    setts.toxLayerStop = (int)(setts.resolution * Read_float(s));
                    SimParamsChanged();
                }
            };

            //txt_layer_to_display.TextChanged += (s, e) => setts.layerToDisplay = Read_int(s);

            xy_direction_button.CheckedChanged += (s, e) =>
            {
                if (direction_group_box.Enabled && xy_direction_button.Checked)
                {
                    sox_track_bar_xy.Enabled = true;
                    chk_show_dead_axons.Enabled = true;
                    chk_show_live_axons.Enabled = true;
                    showdir = 0;
                    int layerToDisplay = sox_track_bar_xy.Value;
                    if(setts.no3dLayers <= 0)
                    {
                        layerToDisplay = 0;
                    }
                    else 
                    {
                        layerToDisplay = Mod(layerToDisplay, setts.no3dLayers);
                    }

                    setts.layerToDisplay = layerToDisplay;
                    sox_track_bar_xy.Value = setts.layerToDisplay;
                    txt_layer_to_display.Text = sox_track_bar_xy.Value.ToString();
                   // Append_stat_ln("Info: XY " + setts.layerToDisplay);
                    if (preprocessDone)
                    {
                        Update_show_opts();
                    }

                }
                else
                {
                    sox_track_bar_xy.Enabled = false;
                }
            };

            xz_direction_button.CheckedChanged += (s, e) =>
            {
                if (direction_group_box.Enabled && xz_direction_button.Checked)
                {
                    sox_track_bar_xz.Enabled = true;
                    chk_show_dead_axons.Enabled = false;
                    chk_show_live_axons.Enabled = false;
                    showdir = 1;
                    int layerToDisplay = bmp_im_size - sox_track_bar_xz.Value;
                    if(layerToDisplay < 0)
                    {
                        layerToDisplay = 0;
                    }
                    if (layerToDisplay >= bmp_im_size)
                    {
                        layerToDisplay = bmp_im_size;
                    }
                    setts.layerToDisplay = layerToDisplay;
                    //Append_stat_ln("Info: XZ " + setts.layerToDisplay);
                    if (preprocessDone)
                    {
                       Update_show_opts();
                    }
                }
                else
                {
                    sox_track_bar_xz.Enabled = false;
                }
            };

            yz_direction_button.CheckedChanged += (s, e) =>
            {
                if (direction_group_box.Enabled && yz_direction_button.Checked)
                {
                    chk_show_dead_axons.Enabled = false;
                    chk_show_live_axons.Enabled = false;
                    sox_track_bar_yz.Enabled = true;
                    showdir = 2;
                    setts.layerToDisplay = sox_track_bar_yz.Value;
                    if (setts.layerToDisplay >= bmp_im_size)
                    {
                        setts.layerToDisplay = bmp_im_size;
                    }
                    //Append_stat_ln("Info: YZ " + setts.layerToDisplay);
                    if (preprocessDone)
                    {
                        Update_show_opts();
                    }
                }
                else
                {
                    sox_track_bar_yz.Enabled = false;
                }
            };

            sox_track_bar_xz.ValueChanged += (s, e) =>
            {
                if (direction_group_box.Enabled && xz_direction_button.Checked)
                {
                    int layerToDisplay = bmp_im_size - sox_track_bar_xz.Value;
                    if (layerToDisplay < 0)
                    {
                        layerToDisplay = 0;
                    }
                    if(layerToDisplay >= bmp_im_size)
                    {
                        layerToDisplay = bmp_im_size;
                    }
                    setts.layerToDisplay = layerToDisplay;
                    //Append_stat_ln("Info: XZ " + setts.layerToDisplay);
                    if (preprocessDone)
                    {
                        Update_show_opts();
                    }
                }
                else
                {
                    sox_track_bar_xz.Enabled = false;
                }
            };

            sox_track_bar_yz.ValueChanged += (s, e) =>
            {
                if (direction_group_box.Enabled && yz_direction_button.Checked)
                {
                    setts.layerToDisplay = sox_track_bar_yz.Value;
                    if (setts.layerToDisplay >= bmp_im_size)
                    {
                        setts.layerToDisplay = bmp_im_size;
                    }
                    //Append_stat_ln("Info: YZ " + setts.layerToDisplay);
                    if (preprocessDone)
                    {
                        Update_show_opts();
                    }
                }
                else
                {
                    sox_track_bar_yz.Enabled = false;
                }
            };

            sox_track_bar_xy.ValueChanged += (s, e) =>
            {
                if (direction_group_box.Enabled && xy_direction_button.Checked)
                {
                    int layerToDisplay = sox_track_bar_xy.Value;
                    if(setts.no3dLayers <= 0 )
                    {
                        layerToDisplay = 0;
                    }
                    else 
                    {
                        layerToDisplay = Mod(layerToDisplay, setts.no3dLayers);
                    }

                    setts.layerToDisplay = layerToDisplay;
                    sox_track_bar_xy.Value = setts.layerToDisplay;
                    txt_layer_to_display.Text = sox_track_bar_xy.Value.ToString();
                    //Append_stat_ln("Info: XY " + setts.layerToDisplay);
                    if (preprocessDone)
                    {
                        Update_show_opts();
                    }
                }
                else {
                    sox_track_bar_xy.Enabled = false;
                }
            };

            // The Maximum property sets the value of the track bar when
            // the slider is all the way to the right.
            //sox_track_bar.Minimum = 0;
            //sox_track_bar.Maximum = setts.no3dLayers;

            // The TickFrequency property establishes how many positions
            // are between each tick-mark.
            //sox_track_bar.TickFrequency = 1;

            // The LargeChange property sets how many positions to move
            // if the bar is clicked on either side of the slider.
            //sox_track_bar.LargeChange = 1;

            // The SmallChange property sets how many positions to move
            // if the keyboard arrows are used to move the slider.
            //sox_track_bar.SmallChange = 1;

            btn_save_model.Click += (s, e) =>
            {
                if (model_is_saved) { Append_stat_ln("Model is already saved."); return; }
                if (model_id == 0) { Append_stat_ln("Model is not yet generated."); return; }
                Debug.WriteLine(model_id);
                var fil_name = ProjectOutputDir + @"Models\" + Dec2B36(model_id) + " " + (mdl.nerve_scale_ratio * 100).ToString("0") + "%" + ".mdat";
                Save_mdl(fil_name);
                Append_stat_ln("Model saved to " + fil_name);
                model_is_saved = true;                
            };

            btn_load_model.Click += (s, e) =>
            {
                var FD = new OpenFileDialog()
                {
                    InitialDirectory = ProjectOutputDir + @"Models\",
                    Title = "Load Model",
                    Filter = "Model Data files (*.mdat) | *.mdat",
                    RestoreDirectory = true,
                    AutoUpgradeEnabled = false
                };
                if (FD.ShowDialog() != DialogResult.OK) return;
                Load_model(FD.FileName);
                Preprocess_model();
                Append_stat_ln("Info: Preprocessing done.");
                Set_btn_start_txt("&Start", System.Drawing.Color.Green); btn_start.Enabled = true;
            };

            btn_save_setts.Click += (s, e) =>
            {
                var fil_name = ProjectOutputDir + @"Settings\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + ".sdat";
                setts.insult = new float[] { insult_x, insult_y, insult_r };
                WriteToBinaryFile(fil_name, setts);
                Append_stat_ln("Settings saved to " + fil_name);
            };

            btn_load_setts.Click += (s, e) =>
            {
                var FD = new OpenFileDialog()
                {
                    InitialDirectory = ProjectOutputDir + @"Settings\",
                    Title = "Load Settings",
                    Filter = "Setting files (*.sdat) | *.sdat",
                    RestoreDirectory = true,
                    AutoUpgradeEnabled = false
                };
                if (FD.ShowDialog() != DialogResult.OK) return;
                Load_settings(FD.FileName);
            };
        }


        private void ResetPlaneViewer()
        {
            if (InvokeRequired)
                Invoke(new Action(() => ResetPlaneViewer()));
            else
            {
                sox_track_bar_xy.Maximum = setts.no3dLayers;
                setts.layerToDisplay = 0;
                sox_track_bar_xy.Value = setts.layerToDisplay;
                txt_layer_to_display.Text = sox_track_bar_xy.Value.ToString();
            }
        }


        private void DisableButtons()
        {
            if (InvokeRequired)
                Invoke(new Action(() => DisableButtons()));
            else
            {
                txt_3d_layers.Enabled = false;
                txt_3d_tox_start.Enabled = false;
                txt_3d_tox_stop.Enabled = false;
                btn_preprocess.Enabled = false;
                btn_reset.Enabled = false;
                btn_load_setts.Enabled = false;
                btn_load_model.Enabled = false;
                btn_generate_model.Enabled = false;
                txt_resolution.Enabled = false;
                txt_nerve_scale.Enabled = false;
                txt_death_tox_threshold.Enabled = false;
                txt_detox_extra.Enabled = false;
                txt_detox_intra.Enabled = false;
                txt_on_death_tox.Enabled = false;
                txt_rate_bound_a2e.Enabled = false;
                txt_rate_bound_e2a.Enabled = false;
                txt_rate_dead.Enabled = false;
                txt_rate_extra.Enabled = false;
                txt_rate_extra_z.Enabled = false;
                txt_rate_live.Enabled = false;
                txt_rate_live_z.Enabled = false;
                txt_tox_prod_rate.Enabled = false;
                txt_var_death.Enabled = false;
                chk_var_death.Enabled = false;
                chk_strict_rad.Enabled = false;
                txt_clearance.Enabled = false;
                chk_rand_mult.Enabled = false;

            }
        }

        private void EnableButtons()
        {
            if (InvokeRequired)
                Invoke(new Action(() => EnableButtons()));
            else
            {
                txt_3d_layers.Enabled = true;
                txt_3d_tox_start.Enabled = true;
                txt_3d_tox_stop.Enabled = true;
                btn_preprocess.Enabled = true;
                btn_reset.Enabled = true;
                btn_load_setts.Enabled = true;
                btn_load_model.Enabled = true;
                btn_generate_model.Enabled = true;
                txt_resolution.Enabled = true;
                txt_nerve_scale.Enabled = true;
                txt_death_tox_threshold.Enabled = true;
                txt_detox_extra.Enabled = true;
                txt_detox_intra.Enabled = true;
                txt_on_death_tox.Enabled = true;
                txt_rate_bound_a2e.Enabled = true;
                txt_rate_bound_e2a.Enabled = true;
                txt_rate_dead.Enabled = true;
                txt_rate_extra.Enabled = true;
                if (setts.no3dLayers > 0) txt_rate_extra_z.Enabled = true;
                txt_rate_live.Enabled = true;
                if (setts.no3dLayers > 0) txt_rate_live_z.Enabled = true;
                txt_tox_prod_rate.Enabled = true;
                txt_var_death.Enabled = true;
                chk_var_death.Enabled = true;
                chk_strict_rad.Enabled = true;
                txt_clearance.Enabled = true;
                chk_rand_mult.Enabled = true;
            }
        }

        private void SimParamsChanged()
        {
            if (InvokeRequired)
                Invoke(new Action(() => SimParamsChanged()));
            else
            {
                Stop_sim(Sim_stat_enum.Stopped);
                Set_btn_start_txt("&Start", System.Drawing.Color.Gray); btn_start.Enabled = false;
                Append_stat_ln("Info: Simulation parameter changed. Call preprocess next.....");
            }
        }

        private void Load_model(string path)
        {
            if (!File.Exists(path)) return;
            Load_mdl(path);
            Update_mdl_and_setts_ui();
            Append_stat_ln("Model Successfully Loaded.");
            Update_bottom_stat("Model Successfully Loaded.");
            model_is_saved = true;
        }

        private void Load_settings(string path)
        {
            if (!File.Exists(path)) return;
            setts = ReadFromBinaryFile<Setts>(path);
            insult_x = setts.insult[0];
            insult_y = setts.insult[1];
            insult_r = setts.insult[2];
            Update_mdl_and_setts_ui();
        }

        private void Update_mdl_and_setts_ui()
        {
            txt_nerve_scale.Text = (mdl.nerve_scale_ratio * 100F).ToString();

            txt_resolution.Text = setts.resolution.ToString();
            txt_detox_extra.Text = setts.detox_extra.ToString();
            txt_detox_intra.Text = setts.detox_intra.ToString();

            txt_rate_bound_a2e.Text = setts.rate_bound_a2e.ToString();
            txt_rate_bound_e2a.Text = setts.rate_bound_e2a.ToString();
            txt_rate_dead.Text = setts.rate_dead.ToString();
            txt_rate_extra.Text = setts.rate_extra.ToString();
            txt_rate_extra_z.Text = setts.rate_extra_z.ToString();
            txt_rate_live.Text = setts.rate_live.ToString();
            txt_rate_live_z.Text = setts.rate_live_z.ToString();

            txt_tox_prod_rate.Text = setts.tox_prod.ToString();
            txt_death_tox_threshold.Text = setts.death_tox_thres.ToString();
            txt_insult_tox.Text = setts.insult_tox.ToString();
            txt_on_death_tox.Text = setts.on_death_tox.ToString();

            txt_3d_layers.Text = setts.no3dLayers.ToString();
            txt_3d_tox_start.Text = setts.toxLayerStart.ToString();
            txt_3d_tox_stop.Text = setts.toxLayerStop.ToString();
            txt_var_death.Text = setts.death_var_thr.ToString();

            txt_clearance.Text = mdl_clearance.ToString();

            txt_rec_interval.Text = setts.gui_iteration_period.ToString();

            chk_var_thr.Checked = setts.varToxProd;

            chk_rand_mult.Checked = setts.useRandProdFactor;

            Update_num_axons_lbl();
        }

        private float Read_float(object o)
        {
            TextBox txtB = (TextBox)o;
            float num;
            if (!float.TryParse(txtB.Text, out num)) return 0;
            return num;
        }

        private int Read_int(object o)
        {
            TextBox txtB = (TextBox)o;
            int num;
            if (!int.TryParse(txtB.Text, out num))
            {
                //txtB.Text = "0";
                //txtB.SelectionStart = 0;
                //txtB.SelectionLength = txtB.Text.Length;
                return 0;
            }
            return num;
        }
    }
}
