
%%
ncells = 1;
neurs = rand(ncells,3);

%%

res = 50;
rate = 2/res;

[x, y] = meshgrid(1:res);

im = zeros(res);

for k = 1:ncells
    
    im = im + double((x-res*neurs(k,1)).^2 + (y-res*neurs(k,2)).^2 < (res/2*neurs(k,3)).^2);
end

h = imagesc(im, [0 1]);
axis equal
% pause(.5)

cnt = 0;

barr = false(size(im));
barr(1,:) = 1; barr(:,1) = 1;
barr(end,:) = 1; barr(:,end) = 1;

while cnt < 200
    cnt = cnt + 1;
    if ~ishandle(h), break; end
    im2 = diffu(im, barr, rate);
    
    if ~mod(cnt, 5)
        set(h,'CData', im2);
        drawnow
    end
%     pause(40/res.^2)
    im = im2;
    fprintf('%f\n', sum(im(:)));
end
