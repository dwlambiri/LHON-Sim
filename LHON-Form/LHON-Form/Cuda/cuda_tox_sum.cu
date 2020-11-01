
extern "C" __global__  void cuda_tox_sum(int* pix_idx, int pix_idx_num, float* tox, float* tox_sum, int offset, int imsquare, int no3dLayers)
{
	int idx = (blockIdx.x * gridDim.y + blockIdx.y) * blockDim.x + threadIdx.x;
	if (idx < pix_idx_num)
	{
		int xy = pix_idx[idx];
		if (no3dLayers) {
			int sum = 0;
			for (int i = 0; i < no3dLayers; i++) {
				sum += tox[((offset + i) % (no3dLayers + 2))*imsquare + xy];
			}
			atomicAdd(tox_sum, sum);
		}
		else {
			atomicAdd(tox_sum, tox[offset*imsquare + xy]);
		}
		
	}
}
