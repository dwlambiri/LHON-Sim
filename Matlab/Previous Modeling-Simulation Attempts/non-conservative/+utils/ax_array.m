function ax = ax_array(s, varargin)

r = s(1); c = s(2);

% figure('units','normalized','position',[.2 .4 .6 .3]);
ax = zeros(s);
for k1 = 1:c
    for k2 = 1:r
        ax(r+1-k2,k1) = axes('units','normalized','outerposition',[.1+(k1-1)/c, .1+(k2-1)/r, 1/c, 1/r]*.8, varargin{:});
    end
end

