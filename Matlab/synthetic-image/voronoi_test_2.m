
im_siz = 200;

c = [150 80 30;
    80 140 45
    50 50 40];

im = false(im_siz);

[xim, yim] = meshgrid(1:im_siz);

for k = 1:size(c, 1)
    ins = (xim - c(k,1)).^2 + (yim - c(k,2)).^2 < c(k,3)^2;
    im(ins) = 1;
end

showim = @(x) imshow(x, [], 'InitialMagnification', 'fit');

%%

im2 = bwmorph(im,'thicken', 10);

showim(im2)
