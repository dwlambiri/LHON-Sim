
s = 51;
h = figure;

[x, y] = meshgrid(1:s);
dis = (x - (s+1)/2).^2 + (y - (s+1)/2).^2;

im = zeros(s,s,3);

for r = 1:1:20
    if ~ishandle(h), break; end
    im(:,:,1) = abs(dis - r^2) <= r;
    r2 = r + 1;
    im(:,:,2) = abs(dis - (r2)^2) <= (r2);
    
    try
        imshow(im, [], 'initialmagnification', 1000);
    end
    pause(1);
end
