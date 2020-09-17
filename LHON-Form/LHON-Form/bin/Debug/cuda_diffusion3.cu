
extern "C" __global__  void cuda_diffusion3(int* pix_idx, int pix_idx_num, unsigned short im_size,
	int tox_switch, float* tox, float* rate, float* detox, float* tox_prod, unsigned int* id_center_axon, 
	float on_death_tox, float k_rate_dead_axon, float k_detox_extra, float death_tox_thres,
	unsigned int* axons_cent_pix, unsigned int* axons_inside_pix, unsigned int* axons_inside_pix_idx, unsigned int* axon_surr_rate, unsigned int* axon_surr_rate_idx,
	bool* axon_is_alive, unsigned char* axon_mask, int* num_alive_axons, int* death_itr, int iteration)
{
	int idx = (blockIdx.x * gridDim.y + blockIdx.y) * blockDim.x + threadIdx.x;
	if (idx < pix_idx_num)
	{
		int xy = pix_idx[idx];

		int xy0 = xy + im_size;
		int xy1 = xy - im_size;
		int xy2 = xy + 1;
		int xy3 = xy - 1;
		int xy4 = xy * 4;

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
			(tox_old[xy0] - t) * rate[xy4] +
			(tox_old[xy1] - t) * rate[xy4 + 1] +
			(tox_old[xy2] - t) * rate[xy4 + 2] +
			(tox_old[xy3] - t) * rate[xy4 + 3] +
			tox_prod[xy];

		tox_new[xy] *= detox[xy];
		
		int n = id_center_axon[xy];
		if (n && axon_is_alive[n] && tox[axons_cent_pix[n]] > death_tox_thres)
		{ 	// Kill the axon
			for (int p = axons_inside_pix_idx[n]; p < axons_inside_pix_idx[n + 1]; p++)
			{
				int idx = axons_inside_pix[p];

				detox[idx] = k_detox_extra;
				tox[idx] += on_death_tox;
				tox_prod[idx] = 0;
				axon_mask[idx] = 2; // dead
			}

			for (int p = axon_surr_rate_idx[n]; p < axon_surr_rate_idx[n + 1]; p++)
				rate[axon_surr_rate[p]] = k_rate_dead_axon;

			/*
			int idx4 = 4 * idx;
			rate[idx4] = k_rate_dead_axon;
			rate[idx4 + 1] = k_rate_dead_axon;
			rate[idx4 + 2] = k_rate_dead_axon;
			rate[idx4 + 3] = k_rate_dead_axon;
			*/

			axon_is_alive[n] = false;
			death_itr[n] = iteration;
			atomicAdd(&num_alive_axons[0], -1);
		}
	}
	
}
