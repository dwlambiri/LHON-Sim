N = 40;
x = linspace(-N, N, 200);
[X, Y] = meshgrid(x);

h = [];

after = @(t, t1) (t - t1) * (t > t1);
figure('units', 'normalized', 'position', [.1 .1 .8 .8]);

% colormap jet
colormap hot
% colormap colorcube

for t = 0:0.1:150
    z = 1*fcn(X, Y, 1 + after(t, 0), [3 -1]) ...
         + 5.5*fcn(X, Y, 1 + after(t, 5), [-3 -2]) ...
         + 1.5*fcn(X, Y, 1 + after(t, 8), [5 5]) ...
         + 4.5*fcn(X, Y, 1 + after(t, 10), [10 -6]) ...
     ;
    if isempty(h)
        h = surf(X, Y, z);
        shading('interp'), axis('tight')
        caxis manual
        caxis([0,.2])
        axis equal
        axis manual
        
        view(0,90);
    elseif ishandle(h)
        set(h, 'ZData', z);
    else
        break
    end
    drawnow
end