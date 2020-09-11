v = [1.04 1.19	1.15    1.21    1.28    1.27
    .97  .95    1.01    1.16    1.24    1.26
    .93  .92    1.07    1.16    1.22    1.28
    .94  .93    1.08    1.19    1.18    1.17
    .92  1.02   1.02    1.14    1.16    1.24
    1.01 1.1    1.1     1.22    1.22    1.21  ] * neuron_scale;
% imshow(-v, [], 'initialmagnification', 6000)
stp = .01; % Interpolation step

[x, y] = meshgrid(-1:(2/5):1);
[xq, yq] = meshgrid(-1:stp:1); % Interpolate data
r_avg = interp2(x,y,v,xq,yq);
r_avg = imfilter(r_avg, fspecial('disk', .2/stp), 'symmetric');
r_avg((xq.^2+yq.^2) > 1) = nan;

surf(xq, yq, r_avg, 'edgecolor', 'none')
daspect([1 1 .5])
colorbar
axis vis3d