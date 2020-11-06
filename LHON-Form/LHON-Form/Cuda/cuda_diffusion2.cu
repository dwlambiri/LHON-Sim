
#define rateUpLayerIndex  4
#define rateDownLayerIndex  5

extern "C" __global__  void cuda_diffusion2(int* pix_idx, int pix_idx_num, unsigned short im_size,
	float* tox, float* detox, float* tox_prod, float* randTable, unsigned char* rate, float* rate_values, int rate_dimensions, 
	int dstl, int tl, int ml, int bl, int top, int bottom, int injury, int index)
{
	int idx = (blockIdx.x * gridDim.y + blockIdx.y) * blockDim.x + threadIdx.x;
	if (idx < pix_idx_num)
	{
		int xy = pix_idx[idx];

		int sq = im_size * im_size;

		int xy0 = xy + im_size;
		int xy1 = xy - im_size;
		int xy2 = xy + 1;
		int xy3 = xy - 1;
		
		int xyN = xy * rate_dimensions;

		float* tox_new = &tox[dstl * sq];
	    float* tox_old = &tox[ml * sq];
		float* tox_up = &tox[tl * sq];
		float* tox_down = &tox[bl * sq];

		float t = tox_old[xy];

		tox_new[xy] = t +
			(tox_old[xy0] - t) * rate_values[rate[xyN]] +
			(tox_old[xy1] - t) * rate_values[rate[xyN + 1]] +
			(tox_old[xy2] - t) * rate_values[rate[xyN + 2]] +
			(tox_old[xy3] - t) * rate_values[rate[xyN + 3]];

		if(top == false)
			tox_new[xy] += (tox_up[xy] - t) * rate_values[rate[xyN + rateUpLayerIndex]];

		if (bottom == false)
			tox_new[xy] += (tox_down[xy] - t) * rate_values[rate[xyN + rateDownLayerIndex]];

		if(injury)
			tox_new[xy] += tox_prod[xy] * randTable[index];

		tox_new[xy] *= detox[xy];
	}
}
