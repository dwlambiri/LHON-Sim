
#define diff_dead_index 4

/*
* [DWL] Need to keep the constants in this file in line with the values in preprocess.cs!!
private readonly byte diff_zero_index = 0;
private readonly byte diff_live_index = 1;
private readonly byte diff_bound_index_a2e = 2;
private readonly byte diff_bound_index_e2a = 3;
private readonly byte diff_dead_index = 4;
private readonly byte diff_extra_index = 5;
private readonly byte diff_one_index = 6;
*/

extern "C" __global__  void cuda_update_live(int n_axons, float* tox, unsigned char* rate, float* detox, float* tox_prod, float on_death_tox, float k_detox_extra, float* death_tox_thres,
	unsigned int * axons_cent_pix, unsigned int* axons_inside_pix, unsigned int* axons_inside_pix_idx, unsigned int* axon_surr_rate, unsigned int* axon_surr_rate_idx,
	bool* axon_is_alive, unsigned char* axon_mask, int* num_alive_axons, int* death_itr, int iteration, int offset, int pixelNeighbourNumbers)
{
	int n = threadIdx.x + blockIdx.x * blockDim.x;

	if (n < n_axons)
	{
		// [DWL]: I made death_tox_threshold to be an array INSTEAD of a constant 
		//			This way we can set the death threshold DIFFERENTLY for each axon
		//			The death is calculated at the head of the axon
		if (axon_is_alive[n] && tox[offset+axons_cent_pix[n]] >= death_tox_thres[n])
		{ 	// Kill the axon
			for (int p = axons_inside_pix_idx[n]; p < axons_inside_pix_idx[n + 1]; p++)
			{
				int idx = axons_inside_pix[p];

				detox[idx] = k_detox_extra;
				tox[offset+idx] += on_death_tox;
				tox_prod[idx] = 0;
				axon_mask[idx] = 2; // dead
				
				/*
				int idxN = pixelNeighbourNumbers * idx;
				for (int i = 0; i < pixelNeigbourNumbers; i++) {
					rate[idxN + i] = diff_dead_index;
				}
				*/
				
			}

			for (int p = axon_surr_rate_idx[n]; p < axon_surr_rate_idx[n + 1]; p++)
				rate[axon_surr_rate[p]] = diff_dead_index;
			
			
			axon_is_alive[n] = false;
			death_itr[n] = iteration;
			atomicAdd(&num_alive_axons[0], -1);
		}
	}
}

