
#define diff_live_index 1
#define diff_bound_index 2
#define diff_dead_index 3
#define diff_extra_index 4

extern "C" __global__  void cuda_diffusion1(int* pix_idx, int pix_idx_num, unsigned short im_size,
	int tox_switch, float* tox, float* detox, float* tox_prod, unsigned char* rate, float* rate_values, int rate_dimensions)
{
	//float rate_values[] = { 0,0,0,0,0,0 };
	int idx = (blockIdx.x * gridDim.y + blockIdx.y) * blockDim.x + threadIdx.x;
	if (idx < pix_idx_num)
	{
		int xy = pix_idx[idx];

		int xy0 = xy + im_size;
		int xy1 = xy - im_size;
		int xy2 = xy + 1;
		int xy3 = xy - 1;
		int xyN = xy * rate_dimensions;

		float *tox_new, *tox_old;

		if (tox_switch > 0) {
			tox_old = &tox[im_size*im_size];
			tox_new = &tox[0];
		}
		else {
			tox_new = &tox[im_size*im_size];
			tox_old = &tox[0];
		}

		float t = tox_old[xy];

		tox_new[xy] = t +
			(tox_old[xy0] - t) * rate_values[rate[xyN]] +
			(tox_old[xy1] - t) * rate_values[rate[xyN + 1]] +
			(tox_old[xy2] - t) * rate_values[rate[xyN + 2]] +
			(tox_old[xy3] - t) * rate_values[rate[xyN + 3]] +
			tox_prod[xy];

		tox_new[xy] *= detox[xy];
	}
}
