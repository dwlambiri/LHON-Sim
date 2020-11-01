
#define diff_live_index 1
#define diff_bound_index 2
#define diff_dead_index 3
#define diff_extra_index 4

extern "C" __global__  void cuda_update_live(int n_axons, float* tox, unsigned char* rate, float* detox, float* tox_prod, float on_death_tox, float k_detox_extra, float* death_tox_thres,
	unsigned int * axons_cent_pix, unsigned int* axons_inside_pix, unsigned int* axons_inside_pix_idx, unsigned int* axon_surr_rate, unsigned int* axon_surr_rate_idx,
	bool* axon_is_alive, unsigned char* axon_mask, int* num_alive_axons, int* death_itr, int iteration, int offset)
{
	int n = threadIdx.x + blockIdx.x * blockDim.x;

	if (n < n_axons)
	{
		// [DWL]: I made death_tox_threshold to be an array INSTEAD of a constant 
		//			This way we can set the death threshold DIFFERENTLY for each axon
		//			The death is calculated at the head of the axon
		if (axon_is_alive[n] && tox[offset+axons_cent_pix[n]] > death_tox_thres[n])
		{ 	// Kill the axon
			for (int p = axons_inside_pix_idx[n]; p < axons_inside_pix_idx[n + 1]; p++)
			{
				int idx = axons_inside_pix[p];

				detox[idx] = k_detox_extra;
				tox[offset+idx] += on_death_tox;
				tox_prod[idx] = 0;
				axon_mask[idx] = 2; // dead
			}

			for (int p = axon_surr_rate_idx[n]; p < axon_surr_rate_idx[n + 1]; p++)
				rate[axon_surr_rate[p]] = diff_dead_index;
			
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

