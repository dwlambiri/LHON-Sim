
extern "C" __global__  void cuda_update_image(unsigned short im_size, unsigned short bmp_im_size, float bmp_image_compression_ratio, float bmp_z_compression_ratio,
	unsigned char* bmp, float* bmp_tox, float* tox, unsigned char* axon_mask, unsigned char* init_insult_mask, float tox_max, bool* show_opts, int showdir, int lineToDisplay, int imsq, int head, int no3d, int showRGBSox, int displayAtTop)
{
	int x_bmp = blockIdx.x * blockDim.x + threadIdx.x;
	int y_bmp = blockIdx.y * blockDim.y + threadIdx.y;

	if (x_bmp < bmp_im_size && y_bmp > 0) {

		int xy_bmp = x_bmp * bmp_im_size + y_bmp;
		int xy4_bmp = xy_bmp * 4;
		

		unsigned char red = 0, green = 0, blue = 0;
		float tox_pix_value = 0;
		
		switch (showdir) {
		case 1: { 
			// XZ (vertical slider)
			//green = blue = 0;

			if (displayAtTop) {
				if (show_opts[2]) {
					float xf = x_bmp * bmp_z_compression_ratio;
					int layer = (head+(int)(xf)) % (no3d + 2);
					int xpos = (int)((float)(bmp_im_size - lineToDisplay) * bmp_image_compression_ratio);
					int ypos = (int)((float)y_bmp * bmp_image_compression_ratio);
					int xy = ypos * im_size + xpos;
					//int xy = xpos * im_size + ypos;
					tox_pix_value = tox[xy + imsq * layer];
					float tmp = tox_pix_value / tox_max;
					if (tmp > 1) tmp = 1;
					red = (unsigned char)(tmp * 255); // 0 - 255
					if (showRGBSox) {
						float gt = tmp * 255 - red;
						if (gt > 0) {
							green = (unsigned char)(gt * 255);
							gt = gt * 255 - green;
							if (gt > 0) {
								blue = (unsigned char)(gt * 255);
							}
						}
					}
				}
			}
			else {
				if ((x_bmp >= lineToDisplay) && (x_bmp < lineToDisplay + no3d) && show_opts[2]) {
					int layer = (head + x_bmp - lineToDisplay) % (no3d + 2);
					int xpos = (int)((float)(bmp_im_size - lineToDisplay) * bmp_image_compression_ratio);
					int ypos = (int)((float)y_bmp * bmp_image_compression_ratio);
					int xy = ypos * im_size + xpos;
					//int xy = xpos * im_size + ypos;
					tox_pix_value = tox[xy + imsq * layer];
					float tmp = tox_pix_value / tox_max;
					if (tmp > 1) tmp = 1;
					red = (unsigned char)(tmp * 255); // 0 - 255
					if (showRGBSox) {
						float gt = tmp * 255 - red;
						if (gt > 0) {
							green = (unsigned char)(gt * 255);
							gt = gt * 255 - green;
							if (gt > 0) {
								blue = (unsigned char)(gt * 255);
							}
						}
					}
					//red = 255;
				}
			}
			break;
		}
		case 2: {
			// YZ (horizontal slider)
			//green = blue = 0;
			if (displayAtTop) {
				if (show_opts[2]) {
					int xpos = (int)((float)(bmp_im_size - x_bmp) * bmp_image_compression_ratio);
					int ypos = (int)((float)lineToDisplay * bmp_image_compression_ratio);
					int xy = ypos * im_size + xpos;
					//int xy = xpos * im_size + ypos;
					float yf = y_bmp * bmp_z_compression_ratio;
					int layer = (head + (int)(yf)) % (no3d + 2);
					tox_pix_value = tox[xy + imsq * layer];
					float tmp = tox_pix_value / tox_max;
					if (tmp > 1) tmp = 1;
					red = (unsigned char)(tmp * 255); // 0 - 255
					//red = 255;
					red = (unsigned char)(tmp * 255); // 0 - 255
					if (showRGBSox) {
						float gt = tmp * 255 - red;
						if (gt > 0) {
							green = (unsigned char)(gt * 255);
							gt = gt * 255 - green;
							if (gt > 0) {
								blue = (unsigned char)(gt * 255);
							}
						}
					}
				}
			}
			else {
				if ((y_bmp >= lineToDisplay) && (y_bmp < lineToDisplay + no3d) && show_opts[2]) {
					int xpos = (int)((float)(bmp_im_size - x_bmp) * bmp_image_compression_ratio);
					int ypos = (int)((float)lineToDisplay * bmp_image_compression_ratio);
					int xy = ypos * im_size + xpos;
					//int xy = xpos * im_size + ypos;
					int layer = (head + y_bmp - lineToDisplay) % (no3d + 2);
					tox_pix_value = tox[xy + imsq * layer];
					float tmp = tox_pix_value / tox_max;
					if (tmp > 1) tmp = 1;
					red = (unsigned char)(tmp * 255); // 0 - 255
					//red = 255;
					red = (unsigned char)(tmp * 255); // 0 - 255
					if (showRGBSox) {
						float gt = tmp * 255 - red;
						if (gt > 0) {
							green = (unsigned char)(gt * 255);
							gt = gt * 255 - green;
							if (gt > 0) {
								blue = (unsigned char)(gt * 255);
							}
						}
					}
				}
			}
			break;
		}
		default: {
			int xpos = (int)((float)(bmp_im_size - x_bmp) * bmp_image_compression_ratio);
			int ypos = (int)((float)y_bmp * bmp_image_compression_ratio);
			int xy = ypos * im_size + xpos;
			//int xy = xpos * im_size + ypos;
			tox_pix_value = tox[imsq * lineToDisplay + xy];
			float tmp = tox_pix_value / tox_max;
			if (tmp > 1) tmp = 1;
			unsigned char normalized_toxin = (unsigned char)(tmp * 255); // 0 - 255
			if (init_insult_mask[xy_bmp]) { blue = green = 127; /*red = 0;*/ }
			else
			{
				if (show_opts[0] && show_opts[1]) {
					if (axon_mask[xy] == 1) { green = 100; } // live
					if (axon_mask[xy] == 2) { blue = 255; /*green = 0;*/ } // dead
					// else: the pixel doesn't belongs to any axon
				}
				else if (show_opts[0]) {
					if (axon_mask[xy] == 1) { green = 100; } // live
					//if (axon_mask[xy] == 2) { green = 0; } // dead
					blue = 0;
				}
				else if (show_opts[1]) {
					//if (axon_mask[xy] == 1) { blue = 0; } // live
					if (axon_mask[xy] == 2) { blue = 255; } // dead
					green = 0;
				}
				else {
					//blue = green = 0;
				}

				if (show_opts[2]) {
					if (show_opts[0] == 0 && show_opts[1] == 0) {
						red = normalized_toxin;
						if (showRGBSox) {
							float gt = tmp * 255 - red;
							if (gt > 0) {
								green = (unsigned char)(gt * 255);
								gt = gt * 255 - green;
								if (gt > 0) {
									blue = (unsigned char)(gt * 255);
								}
							}
						}
					}
					else {
						red = normalized_toxin;
					}
					// green = 255 - normalized_toxin;
				}
				//else { red = 0; }
			}
		}
		}
		

		bmp[xy4_bmp] = blue;
		bmp[xy4_bmp + 1] = green;
		bmp[xy4_bmp + 2] = red;

		bmp_tox[xy_bmp] = tox_pix_value;
	}
}

/*

// Jet colormap: https://www.mathworks.com/help/matlab/ref/jet.html

if (normalized_toxin < 64) { r = 0; g = 4 * v; b = 255; }
else if (normalized_toxin < 128) { r = 0; b = 255 + 4 * (64 - v); g = 255; }
else if (normalized_toxin < 192) { r = 4 * (v - 128); b = 0; g = 255; }
else { g = 255 + 4 * (192 - normalized_toxin); b = 0; r = 255; }

*/
