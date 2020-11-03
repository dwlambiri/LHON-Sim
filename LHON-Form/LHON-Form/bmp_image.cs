using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using Cudafy;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace LHON_Form
{
    public partial class Main_Form : Form
    {
        private ushort bmp_im_size = 1024; // will be rounded to a multiple of threads_per_block_bmp
        private int threads_per_block_bmp_1D = 32;

        private Bitmap bmp;
        private IntPtr bmp_scan0;
        private byte[,,] bmp_bytes, bmp_bytes_dev;
        private byte[,] init_insult_mask_dev;
        private float bmp_image_compression_ratio;
        private bool[] show_opts = new bool[3];
        private bool[] show_opts_dev = new bool[3];
        private int blocks_per_grid_bmp;
        private dim3 update_bmp_gride_size_2D, update_bmp_block_size_2D;

        private void Init_bmp_write()
        {
            bmp_im_size = (ushort)((bmp_im_size / threads_per_block_bmp_1D) * threads_per_block_bmp_1D);
            bmp_image_compression_ratio = (float)im_size / (float)bmp_im_size;
            blocks_per_grid_bmp = bmp_im_size / threads_per_block_bmp_1D;

            update_bmp_gride_size_2D = new dim3(blocks_per_grid_bmp, blocks_per_grid_bmp);
            update_bmp_block_size_2D = new dim3(threads_per_block_bmp_1D, threads_per_block_bmp_1D);

            bmp = new Bitmap(bmp_im_size, bmp_im_size);
            Rectangle rect = new Rectangle(0, 0, bmp_im_size, bmp_im_size);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            bmp_scan0 = bmpData.Scan0;
            bmp.UnlockBits(bmpData);
            bmp_bytes = new byte[bmp_im_size, bmp_im_size, 4];

            for (int y = 0; y < bmp_im_size; y++)
                for (int x = 0; x < bmp_im_size; x++)
                    bmp_bytes[y, x, 3] = 255;
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        unsafe private void Update_bmp_image(int showdir, int layerToDisplay, int imsq)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Update_bmp_image(showdir, layerToDisplay, imsq)));
            else
            {
                //[DWL] changed the denominator of 'red' to be 'on_death_tox' as opposed to 'death_threashold'
                //[DWL] this way the chain reaction becomes clearly visible.

                gpu.Launch(update_bmp_gride_size_2D, update_bmp_block_size_2D).cuda_update_image(im_size, bmp_im_size, bmp_image_compression_ratio,
                    bmp_bytes_dev, tox_dev, axon_mask_dev, init_insult_mask_dev, death_tox_thres*(1+(chk_var_thr.Checked?death_var_thr:0)), show_opts_dev, setts.no3dLayers > 0 ? showdir: 0, setts.no3dLayers > 0 ? layerToDisplay: 0, imsq, setts.no3dLayers> 0 ? headLayer: 0, setts.no3dLayers);

                gpu.CopyFromDevice(bmp_bytes_dev, bmp_bytes);

                fixed (byte* dat = &bmp_bytes[0, 0, 0])
                    CopyMemory(bmp_scan0, (IntPtr)dat, (uint)bmp_bytes.Length);
                picB.Image = bmp;

            }
        }

        private void Record_bmp_gif()
        {
            if (InvokeRequired)
                Invoke(new Action(() => Record_bmp_gif()));
            else
            {
                // AVI:
                //aviStream.AddFrame((Bitmap)bmp.Clone());
                // GIF:
                var bmh = bmp.GetHbitmap();
                var opts = BitmapSizeOptions.FromWidthAndHeight(bmp_im_size, bmp_im_size);
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmh, IntPtr.Zero, System.Windows.Int32Rect.Empty, opts);
                gifEnc.Frames.Add(BitmapFrame.Create(src));
            }
        }

        private float insult_x, insult_y, insult_r; // in um

        private void Update_init_insult()
        {
            int insult_x_p = bmp_im_size - ((int)(insult_y * setts.resolution / bmp_image_compression_ratio) + bmp_im_size / 2);
            int insult_y_p = (int)(insult_x * setts.resolution / bmp_image_compression_ratio) + bmp_im_size / 2;
            int insult_r2_p = (int)(Pow2(insult_r * setts.resolution / bmp_image_compression_ratio));

            gpu.Launch(update_bmp_gride_size_2D, update_bmp_block_size_2D).cuda_update_init_insult(
                bmp_im_size, insult_x_p, insult_y_p, insult_r2_p, init_insult_mask_dev);
        }

        private int picB_offx, picB_offy;
        private float picB_ratio;

        private void PicB_Resize(object sender, EventArgs e)
        {
            float picW = picB.Size.Width;
            float picH = picB.Size.Height;

            float asp_im = (float)im_size / (float)im_size;
            float asp_box = picW / picH;

            if (asp_im > asp_box)
            {
                picB_ratio = picW / im_size;
                picB_offx = 0;
                picB_offy = (int)((picH - picB_ratio * (float)im_size) / 2f);
            }
            else
            {
                picB_ratio = picH / im_size;
                picB_offx = (int)((picW - picB_ratio * (float)im_size) / 2f);
                picB_offy = 0;
            }
        }

        private float[] get_mouse_click_um(MouseEventArgs e)
        {
            float[] um = new float[2];
            int x = (int)((e.X - picB_offx) / picB_ratio);
            int y = (int)((e.Y - picB_offy) / picB_ratio);
            if (x >= 0 && x < im_size && y >= 0 && y < im_size)
            {
                // Sets the initial insult location
                um[1] = (float)(im_size - y - 1) / setts.resolution - mdl_nerve_r;
                um[0] = (float)(x - 1) / setts.resolution - mdl_nerve_r;
            }
            return um;
        }

        private void mouse_click(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || sim_stat == Sim_stat_enum.Running) return;

            // Sets the initial insult location
            float[] um = get_mouse_click_um(e);
            insult_x = um[0];
            insult_y = um[1];

            Debug.WriteLine(insult_x + "  " + insult_y);

            Preprocess_model();
            //reset_state();
        }

        // picB.Pain += (s, e) => {...}
        private void picB_Paint(object sender, PaintEventArgs e)
        {
            if (!show_axon_order_mdl_gen && axon_lbl != null)
            {
                // the X on the first axon
                //var nlbl0 = axon_lbl[first_axon_idx];
                //SizeF textSize0 = e.Graphics.MeasureString(nlbl0.lbl, this.Font);
                //e.Graphics.DrawString(nlbl0.lbl, this.Font, Brushes.Beige, nlbl0.x * picB_ratio + picB_offx - (textSize0.Width / 2), nlbl0.y * picB_ratio + picB_offy - (textSize0.Height / 2));
            }

            if (show_axon_order_mdl_gen)
            {
                // comment
                if (mdl_axon_lbl != null && mdl_axon_lbl.Length > 0)
                    for (int i = 0; i < mdl_n_axons; i++)
                    {
                        var lbli = mdl_axon_lbl[i];
                        if (lbli != null)
                        {
                            SizeF textSize = e.Graphics.MeasureString(lbli.lbl, this.Font);
                            float x = lbli.x * picB_ratio + picB_offx - (textSize.Width / 2);
                            float y = lbli.y * picB_ratio + picB_offy - (textSize.Height / 2);
                            e.Graphics.DrawString(lbli.lbl, this.Font, Brushes.White, x, y);
                        }
                    }
            }
        }

    }
}
