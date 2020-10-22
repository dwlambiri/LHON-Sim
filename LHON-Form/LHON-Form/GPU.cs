using System;
using System.Diagnostics;
using System.Windows.Forms;

using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace LHON_Form
{
    public partial class Main_Form : Form
    {
        private volatile GPGPU gpu;
        private readonly bool recompile_cuda = true;

       // const int max_resident_blocks = 16;
       // const int max_resident_threads = 2048;
       // const int warp_size = 32;

        public bool Init_gpu()
        {
            try { gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId); }
            catch { return true; }

            int deviceCount = CudafyHost.GetDeviceCount(CudafyModes.Target);
            if (deviceCount == 0) return true;

            string gpu_name = gpu.GetDeviceProperties(false).Name;

            //if (gpu is CudaGPU && gpu.GetDeviceProperties().Capability < new Version(1, 2))
            //{
            //    Debug.WriteLine("Compute capability 1.2 or higher required for atomics.");
            //    append_stat_ln(gpu_name + " not supported.");
            //    return true;
            //}

            Append_stat_ln("Running on " + gpu_name);

            CudafyModule km = CudafyModule.TryDeserialize();
            if (recompile_cuda && (km == null || !km.TryVerifyChecksums()))
            {
                // eArchitecture arch = gpu.GetArchitecture();
                km = CudafyTranslator.Cudafy(arch: eArchitecture.sm_50);
                km.TrySerialize();
            }
            gpu.LoadModule(km);

            return false;
        }

        // ==================================================================
        //                 Copy from GPU to CPU and vice-versa 
        // ==================================================================

        private dim3 blocks_per_grid_2D_pix;
        private int blocks_per_grid_1D_axons;

        private void Load_gpu_from_cpu()
        {
            GPGPU gpuLocal = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);

            gpuLocal.FreeAll(); gpuLocal.Synchronize();

            tox_dev = gpuLocal.Allocate<float>((2+ setts.no3dLayers)*im_size*im_size); gpuLocal.Set(tox_dev); gpuLocal.CopyToDevice(tox, 0, tox_dev, 0, tox.Length);
            //tox_new_dev = gpuLocal.Allocate<float>(im_size * im_size); gpuLocal.Set(tox_new_dev);

            rate_dev = gpuLocal.Allocate(rate); gpuLocal.CopyToDevice(rate, rate_dev);
            rate_values_dev = gpu.Allocate(rate_values); gpu.CopyToDevice(rate_values, rate_values_dev);
            detox_dev = gpuLocal.Allocate(detox); gpuLocal.CopyToDevice(detox, detox_dev);
            tox_prod_dev = gpuLocal.Allocate(tox_prod); gpuLocal.CopyToDevice(tox_prod, tox_prod_dev);

            axons_cent_pix_dev = gpuLocal.Allocate(axons_cent_pix); gpuLocal.CopyToDevice(axons_cent_pix, axons_cent_pix_dev);
            id_center_axon_dev = gpuLocal.Allocate(id_center_axon); gpuLocal.CopyToDevice(id_center_axon, id_center_axon_dev);
            axon_is_alive_dev = gpuLocal.Allocate(axon_is_alive); gpuLocal.CopyToDevice(axon_is_alive, axon_is_alive_dev);

            pix_idx_dev = gpuLocal.Allocate(pix_idx); gpuLocal.CopyToDevice(pix_idx, pix_idx_dev);

            num_alive_axons_dev = gpuLocal.Allocate<int>(1); gpuLocal.CopyToDevice(num_alive_axons, num_alive_axons_dev);
            death_itr_dev = gpuLocal.Allocate(death_itr); gpuLocal.CopyToDevice(death_itr, death_itr_dev);
            bmp_bytes_dev = gpuLocal.Allocate(bmp_bytes); gpuLocal.CopyToDevice(bmp_bytes, bmp_bytes_dev);
            init_insult_mask_dev = gpuLocal.Allocate<byte>(bmp_im_size, bmp_im_size);

            sum_tox_dev = gpuLocal.Allocate<float>(1);
            progress_dev = gpuLocal.Allocate<float>(3);

            progression_image_sum_float_dev = gpuLocal.Allocate<float>(prog_im_siz, prog_im_siz);
            progress_image_num_averaged_pix_dev = gpuLocal.Allocate<uint>(prog_im_siz, prog_im_siz);
            progression_image_dev = gpuLocal.Allocate<byte>(prog_im_siz, prog_im_siz);

            // ==================== Constants

            int tmp = (int)Math.Ceiling(Math.Sqrt((double)pix_idx_num / (double)threads_per_block_1D));
            blocks_per_grid_2D_pix = new dim3(tmp, tmp);
            blocks_per_grid_1D_axons = mdl.n_axons / threads_per_block_1D + 1;

            show_opts_dev = gpuLocal.Allocate(show_opts); gpuLocal.CopyToDevice(show_opts, show_opts_dev);
            
            axons_inside_pix_dev = gpuLocal.Allocate(axons_inside_pix); gpuLocal.CopyToDevice(axons_inside_pix, axons_inside_pix_dev);
            axons_inside_pix_idx_dev = gpuLocal.Allocate(axons_inside_pix_idx); gpuLocal.CopyToDevice(axons_inside_pix_idx, axons_inside_pix_idx_dev);

            axon_surr_rate_dev = gpuLocal.Allocate(axons_surr_rate); gpuLocal.CopyToDevice(axons_surr_rate, axon_surr_rate_dev);
            axon_surr_rate_idx_dev = gpuLocal.Allocate(axons_surr_rate_idx); gpuLocal.CopyToDevice(axons_surr_rate_idx, axon_surr_rate_idx_dev);

            axon_mask_dev = gpuLocal.Allocate(axon_mask); gpuLocal.CopyToDevice(axon_mask, axon_mask_dev);

            gpuLocal.Synchronize();

            Debug.WriteLine("GPU used memory: " + (100.0 * (1 - (double)gpuLocal.FreeMemory / (double)gpuLocal.TotalMemory)).ToString("0.0") + " %\n");
        }
        /*
        private void load_cpu_from_gpu()
        {
            //GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);

            gpu.CopyFromDevice(tox_dev, tox);
            gpu.CopyFromDevice(rate_dev, rate);
            gpu.CopyFromDevice(detox_dev, detox);
            gpu.CopyFromDevice(tox_prod_dev, tox_prod);

            gpu.CopyFromDevice(axon_is_alive_dev, axon_is_alive);
            gpu.CopyFromDevice(death_itr_dev, death_itr);
        }
        */

        // ==================================================================
        //         Dummy functions (defined in native cuda @ cuda/...)
        // ==================================================================

        [CudafyDummy]
        public static void cuda_update_live() { }
        [CudafyDummy]
        public static void cuda_diffusion1() { }
        [CudafyDummy]
        public static void cuda_diffusion2() { }
        [CudafyDummy]
        public static void cuda_diffusion3() { }
        [CudafyDummy]
        public static void cuda_update_image() { }
        [CudafyDummy]
        public static void cuda_tox_sum() { }
        [CudafyDummy]
        public static void cuda_prep0() { }
        [CudafyDummy]
        public static void cuda_prep1() { }
        [CudafyDummy]
        public static void cuda_update_init_insult() { }
    }
}

