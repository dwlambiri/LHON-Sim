function z = fcn(x, y, r, c) % bundle

% b_c = bundle(1:2);
% b_r = bundle(3);

dis = dis_f(x - c(1), y - c(2));
% norm_dis = dis/raduis;

%% Guassian (not good because of volume)
% sig = raduis/2;
% v = (2*pi*sig^2);
% z = exp(-dis2./(2*sig))/v;

%% Linear

% Reflect back on bundle perimeter
% dis_cents = dis_f(c(1) - b_c(1), c(2) - b_c(2));
% if (dis_cents + r) > b_r % if waste has reached bundle perimeter 
% end

inside = (dis < r);
v = pi/3*r^2;
z = zeros(size(x));
z(inside) = (1 - dis(inside)/r)/v;


end

function o = dis_f(dx, dy)
    o = sqrt(dx.^2 + dy.^2);
end