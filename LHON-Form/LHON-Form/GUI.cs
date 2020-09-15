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

            chk_show_axons.CheckedChanged += (o, e) => Update_show_opts();
            chk_show_tox.CheckedChanged += (o, e) => Update_show_opts();

            txt_stop_itr.TextChanged += (s, e) => stop_at_iteration = Read_int(s);
            txt_stop_time.TextChanged += (s, e) => stop_at_time = Read_float(s);
            txt_delay_ms.TextChanged += (s, e) => main_loop_delay = Read_int(s);

            txt_block_siz.TextChanged += (s, e) => threads_per_block_1D = (ushort)Read_int(s);

            btn_reset.Click += (s, e) =>
            {
                if (rate == null)
                {
                    Append_stat_ln("You must preprocess the model before resetting the state.\n");
                    return;
                }
                if (sim_stat == sim_stat_enum.Running)
                {
                    Append_stat_ln("You must stop the simulation before resetting the states.\n");
                    return;
                }
                Reset_state(); Set_btn_start_txt("&Start");
            };

            btn_start.Click += (s, e) =>
            {
                if (sim_stat == sim_stat_enum.None || sim_stat == sim_stat_enum.Paused)
                    Start_sim();
                else if (sim_stat == sim_stat_enum.Running)
                    Stop_sim(sim_stat_enum.Paused);
            };

            btn_redraw.Click += (s, e) => { if (sim_stat != sim_stat_enum.Running && !new_model_worker.IsBusy) new_model_worker.RunWorkerAsync(); };

            btn_preprocess.Click += (s, e) =>
            {
                if (sim_stat == sim_stat_enum.Running)
                {
                    Append_stat_ln("You must stop the simulation before preprogress.\n");
                    return;
                }
                Preprocess_model();
            };

            btn_clr.Click += (s, e) => txt_status.Text = "";

            cmb_sw_sel1.SelectedIndex = 0;
            cmb_sw_sel2.SelectedIndex = 0;

            chk_rec_avi.CheckedChanged += (s, e) =>
            {
                if (chk_rec_avi.Checked)
                {
                    chk_rec_avi.Text = "Recording";
                    //avi_file = ProjectOutputDir + @"Recordings\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + '(' + (im_size * im_size / 1e6).ToString("0.0") + "Mpix).avi";
                    //aviManager = new AviManager(avi_file, false);
                    //Avi.AVICOMPRESSOPTIONS options = new Avi.AVICOMPRESSOPTIONS();
                    //options.fccType = (uint)Avi.mmioStringToFOURCC("vids", 5);
                    //options.fccHandler = (uint)Avi.mmioStringToFOURCC("CVID", 5);
                    //options.dwQuality = 1;
                    //aviStream = aviManager.AddVideoStream(options, 10, bmp);
                }
                else
                {
                    chk_rec_avi.Text = "Record";
                    //aviManager.Close();
                    //Process.Start(avi_file);
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

            Append_stat_ln("Welcome to LHON-2D Simulation software!\n");

            btn_snapshot.Click += (s, e) =>
            {
                string adr = ProjectOutputDir + @"Snapshots\" + DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss") + ".jpg";
                Append_stat_ln("Snapshot saved to: " + adr);
                bmp.Save(adr);
            };

            txt_block_siz.Text = threads_per_block_1D.ToString("0");

            picB.MouseWheel += (s, e) =>
            {
                float[] um = get_mouse_click_um(e);
                float dx = insult_x - um[0], dy = insult_y - um[1];
                float dist = dx * dx + dy * dy;
                if (insult_r * insult_r - dist > 0 || dist < 100)
                {
                    insult_r += (float)e.Delta / 100;
                    if (insult_r < 0) insult_r = 0;
                    Update_init_insult();
                    Update_bmp_image();
                    //Debug.WriteLine(insult_r);
                }
            };
            picB.Click += (s, e) => mouse_click(e as MouseEventArgs);
        }

        private void Update_show_opts()
        {
            show_opts[0] = chk_show_axons.Checked;
            show_opts[1] = chk_show_tox.Checked;

            gpu.CopyToDevice(show_opts, show_opts_dev);
            Update_bmp_image();
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
            if (sim_stat == sim_stat_enum.None || sim_stat == sim_stat_enum.Paused)
            {
                sim_stat = sim_stat_enum.Running;
                alg_worker.RunWorkerAsync();
                Set_btn_start_txt("&Pause");
                Update_bottom_stat("Simulation is Running");
            }
        }

        private void Stop_sim(sim_stat_enum stat)
        {
            if (sim_stat == sim_stat_enum.Running)
            {
                sim_stat = stat;
                if (sim_stat == sim_stat_enum.Paused) Set_btn_start_txt("&Continue");
                else Set_btn_start_txt("&Start");

                Update_bottom_stat("Simulation is " + sim_stat.ToString());
            }
        }
        // ====================================================================
        //                           Low level GUI
        // ====================================================================

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            sim_stat = sim_stat_enum.Paused;
            Thread.Sleep(10);
        }

        private void Set_btn_start_txt(string s)
        {
            if (InvokeRequired) Invoke(new Action(() => btn_start.Text = s));
            else btn_start.Text = s;
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
                lbl_tox.Text = (sum_tox / 1000000).ToString("0.00") + " nMol";
                lbl_real_time.Text = time.ToString("0.0");
                lbl_alive_axons_perc.Text = ((float)num_alive_axons[0] * 100 / mdl.n_axons).ToString("0.0") + "%";
                var span = TimeSpan.FromSeconds(now / 1000);
                lbl_sim_time.Text = string.Format("{0:00}:{1:00}:{2:00}", span.Minutes, span.Seconds, span.Milliseconds);

                float itr_p_s = 0;
                if (sim_stat == sim_stat_enum.Running)
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
                    lbl_rem_time.Text = string.Format("{0}:{1:00}:{2:00}", (int)span.TotalHours, span.Minutes, span.Seconds);
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
            // Model parameters
            txt_nerve_scale.TextChanged += (s, e) =>
            {
                mdl.nerve_scale_ratio = Read_float(s) / 100F;
                lbl_nerve_siz.Text = (mdl.nerve_scale_ratio * mdl_real_nerve_r * 2).ToString(".0") + " um";
            };

            // Preprocess parameters

            txt_resolution.TextChanged += (s, e) => setts.resolution = Read_float(s);

            txt_detox_extra.TextChanged += (s, e) => setts.detox_extra = Read_float(s);
            txt_detox_intra.TextChanged += (s, e) => setts.detox_intra = Read_float(s);

            txt_rate_bound.TextChanged += (s, e) => setts.rate_bound = Read_float(s);
            txt_rate_dead.TextChanged += (s, e) => setts.rate_dead = Read_float(s);
            txt_rate_extra.TextChanged += (s, e) => setts.rate_extra = Read_float(s);
            txt_rate_live.TextChanged += (s, e) => setts.rate_live = Read_float(s);
            txt_tox_prod_rate.TextChanged += (s, e) => setts.tox_prod = Read_float(s);
            txt_death_tox_threshold.TextChanged += (s, e) => setts.death_tox_thres = Read_float(s);
            txt_insult_tox.TextChanged += (s, e) => setts.insult_tox = Read_float(s);
            txt_on_death_tox.TextChanged += (s, e) => setts.on_death_tox = Read_float(s);

            txt_clearance.TextChanged += (s, e) => mdl_clearance = Read_float(s);


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

            txt_rate_bound.Text = setts.rate_bound.ToString();
            txt_rate_dead.Text = setts.rate_dead.ToString();
            txt_rate_extra.Text = setts.rate_extra.ToString();
            txt_rate_live.Text = setts.rate_live.ToString();

            txt_tox_prod_rate.Text = setts.tox_prod.ToString();
            txt_death_tox_threshold.Text = setts.death_tox_thres.ToString();
            txt_insult_tox.Text = setts.insult_tox.ToString();
            txt_on_death_tox.Text = setts.on_death_tox.ToString();

            txt_clearance.Text = mdl_clearance.ToString();

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
