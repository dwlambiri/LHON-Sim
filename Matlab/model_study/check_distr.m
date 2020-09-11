close all

export_path = 'C:\Dropbox\PhD\LHON\LHON Shared\LHON-Form\LHON-Form\Project_Output\Exported';
exps = dir(export_path);
fil = [export_path '\' exps(end).name];

dat = dlmread(fil);

nerve_r = dat(1,1);
vein_rat = dat(1,2);
clearance = dat(1,3);

dat = dat(2:end,:);

n_divs = 6;

M = max(dat(:,1));
m = min(dat(:,1));

r = M - m;

figure('position', [147.4 269 1253.6 420])

find_within = @(m, M) dat(:,1) > m & dat(:,1) < M;

for k = 1:n_divs
    ids = find_within(m + (r / n_divs) * (k-1), m + (r / n_divs) * k);
    subplot(1, n_divs, k)
    diams = 2 * dat(ids,3);
    histogram(diams)
    h = xlabel({['Num: ' num2str(sum(ids))] ['D_a_v_g: ' sprintf('%.2f', mean(diams))]}, 'Units', 'normalized', 'position', [0.6, 1, 0], 'FontWeight', 'bold', 'FontSize', 12);
    xlim([0 6])
end


%%
figure;

title('All');
diams = dat(:,3) * 2;
histogram(diams)
h = xlabel({['Num: ' num2str(sum(ids))] ['D_a_v_g: ' sprintf('%.2f', mean(diams))]}, 'Units', 'normalized', 'position', [0.6, 1, 0], 'FontWeight', 'bold', 'FontSize', 12);
xlim([0 6])