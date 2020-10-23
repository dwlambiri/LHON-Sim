using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using AviFile;
using System.Linq;

namespace LHON_Form
{
    public partial class Main_Form : Form
    {
        private const string ProjectOutputDir = @"..\..\Project_Output\";
        private ushort threads_per_block_1D = 1024;
        
        // ======================================================

        private BackgroundWorker alg_worker = new BackgroundWorker(), new_model_worker = new BackgroundWorker();
        private AviManager aviManager;
        private string avi_file;
        private VideoStream aviStream;
        private float sum_tox, max_sum_tox;
        //private float areal_progress, chron_progress;
        private float[] progress_dat = new float[3];
        private const int progress_num_frames = 20;
        private double resolution_reduction_ratio;
        private ushort prog_im_siz, prog_im_siz_default = 100;
        private byte[,] progression_image_dev;
        //private byte[,,] areal_progression_image_stack, chron_progression_image_stack;
        //private float[] areal_progress_chron_val, chron_progress_areal_val;
        //private uint areal_progression_image_stack_cnt, chron_progression_image_stack_cnt;
        private float[,] progression_image_sum_float_dev;
        private uint[,] progress_image_num_averaged_pix_dev;
        //private float areal_progress_lim;
        private bool stop_sweep_req = false, sweep_is_running = false;
        private float[] sum_tox_dev, progress_dev;
        private uint iteration = 0;
        private float realTime;

        // float[] init_insult = new float[2] { 0, 0 };

        private enum Sim_stat_enum { None, Running, Paused, Successful, Failed };

        private Sim_stat_enum sim_stat = Sim_stat_enum.None;

        private class AxonLabelClass
        {
            public string lbl;
            public float x;
            public float y;
        }

        private AxonLabelClass[] axon_lbl;
        
        [Serializable]
        public class Setts
        {
            public float resolution;

            public float rate_live;
            public float rate_dead;
            public float rate_bound;
            public float rate_extra;
            
            public float tox_prod;
            public float on_death_tox;

            public float detox_intra;
            public float detox_extra;

            public float death_tox_thres;
            public float death_var_thr;

            public float[] insult;

            public float insult_tox;

            public int no3dLayers;

            public int toxLayerStart;

            public int toxLayerStop;

            public int layerToDisplay;
        }

        private Model mdl = new Model();
        private Setts setts = new Setts();

        // ============= Main loop vars =================

        //private float progress_step, next_areal_progress_snapshot, next_chron_progress_snapshot;
        private Tictoc tt_sim = new Tictoc();
        //private float sim_time = 0;
        private uint last_itr = 1;
       // private float last_areal_prog;

        // =============== Profiling Class =============

        private class Profiler
        {
            private const int max = 100;
            private double[] T = new double[max];
            private int[] num_occur = new int[max];
            private Stopwatch sw = Stopwatch.StartNew();
            private Stopwatch sw_tot = Stopwatch.StartNew();
            public void Time(int idx) // Pass 0 start of main program to start tot_time
            {
                if (idx > 0)
                {
                    T[idx] += sw.Elapsed.TotalMilliseconds;
                    num_occur[idx]++;
                }
                else if (idx == 0)
                {
                    Array.Clear(T, 0, max);
                    Array.Clear(num_occur, 0, max);
                    sw_tot = Stopwatch.StartNew();
                }
                sw = Stopwatch.StartNew(); // Pass negative to reset only sw
            }
            public void report() // This will stop and conclude tot_time
            {
                sw_tot.Stop();
                sw.Stop();
                double tot_time = sw_tot.Elapsed.TotalMilliseconds;
                Debug.WriteLine("Total: " + (tot_time / 1000).ToString("0.000") + "s");
                for (int k = 0; k < T.Length; k++)
                    if (T[k] > 0)
                        Debug.WriteLine("{0}:\t{1}%\t{2}ms\t{3}K >> {4}ms", k, (T[k] / tot_time * 100).ToString("00.0"), T[k].ToString("000000"), (num_occur[k] / 1000).ToString("0000"), (T[k] / num_occur[k]).ToString("000.000"));
            }
        }

        private Profiler gpu_prof = new Profiler();
        private Profiler alg_prof = new Profiler();
        private Profiler prep_prof = new Profiler();


        // ======= Basic Math Functions =========

        private float Pow2(float x){return x * x;}
        private int Pow2(int x) { return x * x; }

        private float Maxf(float v1, float v2)
        {
            return (v1 > v2) ? v1 : v2;
        }
        private float Minf(float v1, float v2)
        {
            return (v1 < v2) ? v1 : v2;
        }


        public int Max(int v1, int v2)
        {
            return (v1 > v2) ? v1 : v2;
        }

        private int Min(int v1, int v2)
        {
            return (v1 < v2) ? v1 : v2;
        }

        private float Within_circle2(int x, int y, float xc, float yc, float rc)
        {
            float dx = (float)x - xc;
            float dy = (float)y - yc;
            return rc * rc - (dx * dx + dy * dy);
        }

        // ========== tic toc ==========

        private Stopwatch sw = new Stopwatch();

        private void Tic()
        {
            sw = Stopwatch.StartNew();
        }

        private float Toc()
        {
            float t = sw.ElapsedMilliseconds;
            Tic();
            return t;
        }

        private class Tictoc
        {
            private Stopwatch sw = new Stopwatch();
            public void Restart()
            {
                sw = Stopwatch.StartNew();
            }
            public float Read()
            {
                return sw.ElapsedMilliseconds;
            }
            public void Pause()
            {
                sw.Stop();
            }
            public void Start()
            {
                sw.Start();
            }
        }

        private const string CharList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string Dec2B36(long value)
        {
            string result = string.Empty;
            do
            {
                int rem = (int)(value % 36);
                result = CharList[rem] + result;
                value /= 36;
            }
            while (value > 0);

            return result;
        }

        public static long B36toDec(string input)
        {
            var reversed = input.Reverse();
            long result = 0;
            int pos = 0;
            foreach (char c in reversed)
            {
                result += CharList.IndexOf(c) * (long)Math.Pow(36, pos);
                pos++;
            }
            return result;
        }
        
    }
}
