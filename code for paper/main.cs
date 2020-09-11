
bool tox_switch = false;

while (true)
{
	iteration++;
	time += dt;

	gpu.Launch(blocks_per_grid_1D_axons, threads_per_block_1D).cuda_update_live(mdl.n_axons, tox_dev, rate_dev, detox_dev,
		tox_prod_dev, on_death_tox, k_rate_dead_axon, k_detox_extra, death_tox_thres,
		axons_cent_pix_dev, axons_inside_pix_dev, axons_inside_pix_idx_dev, axon_surr_rate_dev, axon_surr_rate_idx_dev,
		axon_is_alive_dev, axon_mask_dev, num_alive_axons_dev, death_itr_dev, iteration);

	gpu.Launch(blocks_per_grid_2D_pix, threads_per_block_1D).cuda_diffusion(pix_idx_dev, pix_idx_num, im_size,
		tox_switch ? 1 : 0, tox_dev, rate_dev, detox_dev, tox_prod_dev);

	tox_switch = !tox_switch;
	
	save_and_plot_frame(iteration);
}

