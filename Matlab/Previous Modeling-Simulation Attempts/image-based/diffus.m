
function grop = diffus(grod, rate)
%Diffusion iterates one step of diffusive process 

z1 = zeros(size(grod, 1),1);
z2 = zeros(1,size(grod, 2));

bottom = [grod(2:end-1,:)/4; grod(end,:); z2];
top = [z2; grod(1:(end-1),:)];
right = [grod(:,2:end), z1];
left = [z1, grod(:,1:(end-1))];

grop = rate*grod + (1-rate) * (top + bottom + left + right) / 4;
