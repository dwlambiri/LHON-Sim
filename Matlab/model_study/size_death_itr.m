
M = dlmread('C:\Dropbox\PhD\LHON\LHON Matlab\LHON-Form\LHON-Form\bin\Debug\Exported\2016 - 07 - 26 @22 - 12 - 43.txt');

[~,sort_idx] = sort(M(:,4));

r = M(sort_idx,3);
death_itr = M(sort_idx,4)/1e4;

%%

figure

hold on

h = [];

for k = min(r):max(r)
    r_k = r == k;
    % h(k-min(r)+1) = plot(death_itr(r_k), sum(r_k):-1:1);
    plot3(death_itr(r_k), ones(sum(r_k), 1) * k, (sum(r_k):-1:1)./sum(r_k)*100, '-*');
end

% legend(h, strsplit(num2str(min(r):max(r))))

title('Neuron Death Over Time for Different Radii')
xlabel('Unit Time')
ylabel('Raduis')
zlabel('Percentage of living Neurons')

view([-228 20])
grid on

