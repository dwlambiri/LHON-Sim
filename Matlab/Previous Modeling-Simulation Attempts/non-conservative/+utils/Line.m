function h = Line(l, dir, varargin) % dir = 'V' or 'H'
if (max(l) - min(l)) == 1
    l = find(l);
end

if dir == 'V'
    x = l;
    y = get(gca,'ylim');
    hold on
    h = line([x x], y, varargin{:});
elseif dir == 'H'
    y = l;
    x = get(gca,'xlim');
    hold on
    h = line(x, [y y], varargin{:});
end

end