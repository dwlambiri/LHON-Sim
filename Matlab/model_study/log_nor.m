a = 8.97; % 8.97
b = -0.088; % -0.088
c = 0.59; %  0.59

f = @(x) a*exp(-((log(x)-(b))/(c)).^2);

fx = 0:0.01:6.1;

fy = f(fx);

plot(x, y,'.'), hold on
plot(fx, fy)

