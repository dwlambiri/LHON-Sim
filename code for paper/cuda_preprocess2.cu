
extern "C" __global__  void cuda_prep1(unsigned short im_size, unsigned char* pix_out_of_nerve, float* rate)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	int y = blockIdx.y * blockDim.y + threadIdx.y;

	if (x < im_size && y < im_size)
	{
		int xy = x * im_size + y;
		int xy4 = xy * 4;

		if (pix_out_of_nerve[xy]) {
			rate[xy4] = 0;
			rate[xy4 + 1] = 0;
			rate[xy4 + 2] = 0;
			rate[xy4 + 3] = 0;
		}
		else {
			if (pix_out_of_nerve[xy + im_size]) rate[xy4] = 0;
			if (pix_out_of_nerve[xy - im_size]) rate[xy4 + 1] = 0;
			if (pix_out_of_nerve[xy + 1])		rate[xy4 + 2] = 0;
			if (pix_out_of_nerve[xy - 1])		rate[xy4 + 3] = 0;
		}
	}
}
