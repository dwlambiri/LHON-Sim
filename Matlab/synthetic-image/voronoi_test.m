
im_siz = 200;

num = round(im_siz/20);

max_dis = 2;

% x = rand(1, num)'*im_siz;
% y = rand(1, num)'*im_siz;

[x, y] = meshgrid(1:num);

x = (x + rand(num)'/1.5) * round(im_siz/num);
y = (y + rand(num)'/1.5) * round(im_siz/num);

figure, voronoi(x,y); axis equal; drawnow

[v,c] = voronoin([x(:) y(:)]);

%%

point_to_line = @(pt, v1, v2) norm((v1(1) - v2(1)) * (pt(2) - v2(2)) - (v1(2) - v2(2)) * (pt(1) - v2(1))) / norm(v1 - v2);

im = false(im_siz);
im0 = zeros(im_siz);

[xim, yim] = meshgrid(1:im_siz);


for k = 1:length(c)
    if any(c{k} == 1), continue, end
    xx = v(c{k}, 1); yy = v(c{k}, 2);
    xx = [xx; xx(1)]; yy = [yy; yy(1)];
    
    A = polyarea(xx,yy);
    
    in = inpolygon(xim, yim, xx, yy);
    in = find(in);
    too_close_inds = false(length(in), 1);
    for k1 = 1:length(in)
        too_close = 0;
        for k2 = 1:length(xx)-1
            dis = point_to_line([xim(in(k1)) yim(in(k1))], [xx(k2) yy(k2)], [xx(k2+1) yy(k2+1)]);
            thickness = min(sqrt(A)'/40, 1) * max_dis;
            if dis < max_dis
                too_close = 1;
                too_close_inds(k1) = 1;
                break;
            end
        end
    end
    
    im(in(~too_close_inds)) = 1;
    im0(in) = k;
end

showim = @(x) imshow(x, [], 'InitialMagnification', 'fit');

H = fspecial('disk', max_dis*2);
% H = fspecial('gaussian', [50 50], .2);
blurred = imfilter(im,H,'replicate');
figure, showim(im0);
figure, showim(im);
figure, showim(blurred);
