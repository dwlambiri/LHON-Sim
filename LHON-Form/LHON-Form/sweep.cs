using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LHON_Form
{
    public partial class Main_Form
    {
        // =======================================

        private void Btn_sweep_Click(object sender, EventArgs e)
        {

            if (sweep_is_running)
            {
                stop_sweep_req = true;
                Stop_sim(Sim_stat_enum.Paused);
                btn_sweep.Text = "S&weep";
                Append_stat_ln("Sweeping Terminated by User!");
            }
            else Sweep();
        }

        private readonly string[] parameters_name = new string[]
        {
            "Repeat",
            "Nerve Scale",

            "Resolution",

            "Live Rate",
            "Boundary Rate",
            "Extra & Dead Rate",
            "All Rates",
            "Death Thr",
            "Tox Prod",
            "On Death",

            "Detox Intra",
            "Detox Extra",

            "Insult Conc",
            "Insult X",
            "Insult Y",
            "Insult R",
        };

        private void Init_sweep()
        {
            foreach (var s in parameters_name)
            {
                cmb_sw_sel1.Items.Add(s);
                cmb_sw_sel2.Items.Add(s);
            }
        }

        private async void Sweep()
        {
            if (sim_stat == Sim_stat_enum.Running) return;

            try
            {
                int delay_ms = 2000;

                int sweep_repetitions1, sweep_repetitions2 = 0;
                float start1 = 0, end1 = 0;
                float start2 = 0, end2 = 0;
                int selection1 = cmb_sw_sel1.SelectedIndex;
                int selection2 = cmb_sw_sel2.SelectedIndex;

                // Get first dimension of sweep
                try
                {
                    string[] values = txt_sw_range1.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    sweep_repetitions1 = int.Parse(values[0]);
                    if (selection1 > 0)
                    {
                        start1 = float.Parse(values[1]);
                        if (sweep_repetitions1 > 1)
                        {
                            end1 = float.Parse(values[2]);
                        }
                        else
                        {
                            end1 = start1;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Not enough or bad input for Sweep command!");
                    return;
                }
                // Get Second dimension
                try
                {
                    string[] values = txt_sw_range2.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    sweep_repetitions2 = int.Parse(values[0]);
                    if (selection2 > 0)
                    {
                        start2 = float.Parse(values[1]);
                        if (sweep_repetitions2 > 1)
                        {
                            end2 = float.Parse(values[2]);
                        }
                        else
                        {
                            end2 = start2;
                        }
                    }
                }
                catch {
                    MessageBox.Show("Error in second parameter to Sweep command!");
                    return;
                }

                btn_sweep.Text = "S&top";
                sweep_is_running = true;

                string parameter_name1 = parameters_name[selection1];
                string parameter_name2 = parameters_name[selection2];

                int failures = 0;
                string dir_name = null;

                Append_stat_ln(parameter_name1 + " , " + parameter_name2 + " ,  Percent Alive");

                for (int i1 = 0; i1 < sweep_repetitions1; i1++)
                {
                    float val1 = Sweep_upd_param(selection1, start1, end1, i1, sweep_repetitions1);
                    float val2 = 0;
                    //Append_stat_ln(parameter_name1 + " : " + val1.ToString());

                    bool sel1_regenerate_model = selection1 < 2;
                    bool sel2_regenerate_model = selection2 < 2;
                    int i2 = 0;
                    do
                    {
                        if (sweep_repetitions2 > 0)
                        {
                            val2 = Sweep_upd_param(selection2, start2, end2, i2, sweep_repetitions2);
                            Append_stat(val1.ToString() + "   ,   " + val2.ToString() + "   ,    ");
                        }
                        Update_mdl_and_setts_ui();

                        if (sel1_regenerate_model || (sweep_repetitions2 > 0 && sel2_regenerate_model))
                        {
                            sel1_regenerate_model = false;
                            // Regenerate Model
                            new_model_worker.RunWorkerAsync();
                            await Task.Delay(delay_ms);
                            while (sim_stat == Sim_stat_enum.Running)
                            {
                                await Task.Delay(delay_ms);
                                if (stop_sweep_req) { stop_sweep_req = false; sweep_is_running = false; return; }
                            }
                        }
                        else
                            Preprocess_model();

                        Start_sim();
                        //Debug.WriteLine("simulation should've been started" + sim_stat);
                        while (sim_stat != Sim_stat_enum.Successful && sim_stat != Sim_stat_enum.Failed)
                        {
                            await Task.Delay(delay_ms);
                            if (stop_sweep_req) { stop_sweep_req = false; sweep_is_running = false; return; }
                        }
                        await Task.Delay(delay_ms / 5);

                        if (sim_stat == Sim_stat_enum.Failed)
                        {
                            failures++;
                        }
                        //successful
                        else {
                            Append_stat_ln(((float)num_alive_axons[0] * 100 / mdl.n_axons).ToString("0.0") + "%");
                            if (chk_save_sw_prog.Checked)
                            {
                                if (dir_name == null)
                                {
                                    string par_nam = "(" + parameter_name1 + ")";
                                    if (sweep_repetitions2 > 0)
                                        par_nam += "(" + parameter_name2 + ")";
                                    dir_name = string.Format(ProjectOutputDir + "Progression\\{0} {1}", DateTime.Now.ToString("yyyy-MM-dd @HH-mm-ss"), par_nam);
                                    Directory.CreateDirectory(dir_name);
                                }
                                string par_val;
                                if (parameters_name[selection1] == "Repeat")
                                    par_val = val1.ToString("(00)");
                                else
                                    par_val = val1.ToString("(0.00)");
                                if (sweep_repetitions2 > 0)
                                    if (parameters_name[selection2] == "Repeat")
                                        par_val += val2.ToString("(00)");
                                    else
                                        par_val += val2.ToString("(0.00)");
                                Save_Progress(string.Format("{0}\\{1}.prgim", dir_name, par_val));
                            }
                        }
                        i2++;
                    }
                    while (i2 < sweep_repetitions2);
                }

                Append_stat_ln(string.Format("Sweeping finished after {0} repetitions and {1} failure(s).", (sweep_repetitions2 > 0 ? sweep_repetitions2 * sweep_repetitions1 : sweep_repetitions1).ToString(), failures > 0 ? failures.ToString() : "no"));
                sweep_is_running = false;
                btn_sweep.Text = "S&weep";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private float Sweep_upd_param(int selection, float start, float end, int i, int sweep_repetitions)
        {
            float sig = end < start ? -1 : 1;
            float step_siz;
            if (selection == 0 || sweep_repetitions == 1) step_siz = 1;
            else step_siz = Math.Abs(end - start) / (float)(sweep_repetitions - 1);
            float val = start + sig * step_siz * i;

            switch (parameters_name[selection])
            {
                case "Nerve Scale":
                    mdl.nerve_scale_ratio = val;
                    break;
                case "Resolution":
                    setts.resolution = val;
                    break;
                case "Live Rate":
                    setts.rate_live = val;
                    break;
                case "Boundary Rate":
                    setts.rate_bound = val;
                    break;
                case "Extra & Dead Rate":
                    setts.rate_extra = val;
                    setts.rate_dead = val;
                    break;
                case "All Rates":
                    setts.rate_live = val;
                    setts.rate_bound = val;
                    setts.rate_extra = val;
                    setts.rate_dead = val;
                    break;
                case "Death Thr":
                    setts.death_tox_thres = val;
                    break;
                case "Tox Prod":
                    setts.tox_prod = val;
                    break;
                case "On Death":
                    setts.on_death_tox = val;
                    break;
                case "Detox Intra":
                    setts.detox_intra = val;
                    break;
                case "Detox Extra":
                    setts.detox_extra = val;
                    break;
                case "Insult Conc":
                    setts.insult_tox = val;
                    break;
                case "Insult X":
                    setts.insult[0] = val;
                    break;
                case "Insult Y":
                    setts.insult[1] = val;
                    break;
                case "Insult R":
                    setts.insult[2] = val;
                    break;
            }
            return val;
        }
        /*
        string[] parameters_name = new string[]
        {
            "Repeat",
            "Nerve Scale",

            "Resolution",

            "Live Rate",
            "Dead Rate",
            "Boundary Rate",
            "Extra Rate",
            "Death Thr",
            "Tox Prod",

            "Detox Intra",
            "Detox Extra",

            "Insult Conc",
            "Insult X",
            "Insult Y",
            "Insult R",
        };*/
    }
}
