
#define diff_live_index 1
#define diff_bound_index 2
#define diff_dead_index 3
#define diff_extra_index 4
//#define rate_dimensions 4

extern "C" __global__  void cuda_prep0(unsigned short im_size, int nerve_cent_pix, int nerve_r_pix_2, int vein_r_pix_2, float k_rate_extra, float k_detox_extra,
	unsigned char* pix_out_of_nerve, unsigned char* rate, float* detox, unsigned int rate_dimensions)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (x < im_size && y < im_size) {

		int xy = x * im_size + y;
		int xyN = xy * rate_dimensions;

		int dx = x - nerve_cent_pix;
		int dy = y - nerve_cent_pix;
		int dis2 = dx * dx + dy * dy;

		bool outside = nerve_r_pix_2 - dis2 < 0 || vein_r_pix_2 - dis2 > 0;
		pix_out_of_nerve[xy] = outside ? 1 : 0;

		if (!outside)
		{
			rate[xyN] = diff_extra_index;
			rate[xyN + 1] = diff_extra_index;
			rate[xyN + 2] = diff_extra_index;
			rate[xyN + 3] = diff_extra_index;
			if (rate_dimensions > 4) {
				rate[xyN + 4] = diff_extra_index;
				rate[xyN + 5] = diff_extra_index;
			}
			

			detox[xy] = k_detox_extra;

			if (pix_out_of_nerve[xy + im_size]) rate[xyN] = 0;
			if (pix_out_of_nerve[xy - im_size]) rate[xyN + 1] = 0;
			if (pix_out_of_nerve[xy + 1])		rate[xyN + 2] = 0;
			if (pix_out_of_nerve[xy - 1])		rate[xyN + 3] = 0;
		}
		else {
			rate[xyN] = 0;
			rate[xyN + 1] = 0;
			rate[xyN + 2] = 0;
			rate[xyN + 3] = 0;
			if (rate_dimensions > 4) {
				rate[xyN + 4] = 0;
				rate[xyN + 5] = 0;
			}
		}
	}
}

//bool[,] pix_out_of_nerve = new bool[im_size, im_size];
//for (int y = 0; y < im_size; y++)
//    for (int x = 0; x < im_size; x++)
//    {
//        int dx = x - nerve_cent_pix;
//        int dy = y - nerve_cent_pix;
//        int dis2 = dx * dx + dy * dy;

//        bool outside = nerve_r_pix_2 - dis2 < 0 || vein_r_pix_2 - dis2 > 0;
//        pix_out_of_nerve[x, y] = outside;
//        if (!outside)
//        {
//            pix_idx[pix_idx_num++] = x * im_size + y;
//            for (uint k = 0; k < 4; k++)
//                rate[x, y, k] = k_rate_extra;
//            detox[x, y] = k_detox_extra;
//        }
//    }

