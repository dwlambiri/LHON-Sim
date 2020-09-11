small_medium = 0.342 * 2;
medium_large = 0.487 * 2;
grid_siz = 5;
n_bins = 20 - 1;

r_min = 0.19;
r_max = 6.87;

edges = r_min:((r_max-r_min)/n_bins):r_max;

%%

[mdl, setts, death_itr] = ReadSim;

if isempty(mdl), return; end

%%

MaxY = 0;
maxy = 0;
for k = 1:3
    
    fig = figure('position', [50 50 650 600], 'Color', 'w');
    
    if k == 1 % intial state
        which_axons = true(1,mdl.n_axons);
        state = 'Init State';
    elseif k == 2 % final state
        which_axons = death_itr == 0;
        state = 'Final State';
    else
        which_axons = death_itr > 0;
        state = 'Dead Axons';
    end
    
    tit = sprintf('%s (n = %.0f)', state, sum(which_axons));
    
    axon_coor = mdl.axon_coor;
    x = axon_coor(which_axons,1);
    y = axon_coor(which_axons,2);
    d = 2 * axon_coor(which_axons,3);
    
    m = min(x);
    r = max(x) - m;
    
    find_within = @(x, m, M) x > m & x < M;
    all_ax = [];   
    
    for k1 = 1:grid_siz
        for k2 = 1:grid_siz
            ids = find_within(x, m + (r / grid_siz) * (k1-1), m + (r / grid_siz) * k1) & ...
                find_within(y, m + (r / grid_siz) * (k2-1), m + (r / grid_siz) * k2);
            
            ax = subplot(grid_siz, grid_siz, k1 + (grid_siz - k2)*grid_siz);
            all_ax(k1,k2) = ax;
            diams = d(ids);
            sa_counts = sum(diams < small_medium);
            ma_counts = sum(medium_large > diams & diams > small_medium);
            la_counts = sum(medium_large < diams);
            
            if k == 1 
                MaxY = max([sa_counts, ma_counts, la_counts, MaxY]);
                MaxY2 = MaxY*1.2;
            end
            
            hold on
            bar(1, sa_counts, 'FaceColor', [1 0 0]);
            bar(2, ma_counts, 'FaceColor', [1 0.5 0.5]);
            bar(3, la_counts, 'FaceColor', [0.6 0.6 0.6]);
            
            set(gca, 'xtick', 1:3, 'xticklabel', {'Small' 'Meduim' 'Large'});
            
            text(0.28, 0.8, {[' n = ' num2str(sum(ids))] ['$\bar{d}$ = ' sprintf('%.2f', mean(diams))]},...
                'interpreter','Latex', 'Units', 'normalized', 'FontWeight', 'bold', 'FontSize', 9);
            % xlim([0 5.5]); xticks(0:1:5);
            ylim([0 length(x)]);
            set(ax, 'units', 'normalized', 'FontSize', 9)
            drawnow
            set(ax, 'position', get(ax, 'outerposition') + [1 1 -2 -2]*0.002);
            set(gca, 'FontName','Arial');
            ax.XTickLabelRotation = 90;
        end
    end
    
    p = 10^floor(log10(abs(MaxY2)));
    most_d = floor(MaxY2/p);
    
    middle = round((grid_siz+1)/2);
    
    for k1 = 1:grid_siz
        for k2 = 1:grid_siz
            axes(all_ax(k1,k2));
            ylim([0 MaxY2]); yticks(0:(most_d/5*p):MaxY2);
            if k1 ~= 1
                yticks([]);
            end
            if k2 ~= 1
                xticks([]);
            end
            
            if k1 == 1 && k2 == middle
                ylabel('Count')
            end
            if k1 == middle && k2 == 1
                xlabel('Diameter (um)')
            end
            
            if k1 == middle && k2 == grid_siz
                title(tit, 'FontWeight', 'Normal', 'FontSize', 14)
            end
        end
    end
    set(findall(fig, '-property', 'FontSize'), 'FontName', 'Arial');
    drawnow
    
    saveas(fig, sprintf('%s.emf', state)); % close
   
    %%
    fig = figure('position', [50 50 550 550], 'Color', 'w');
    
    diams = d;
    
    sa_counts = sum(diams < small_medium);
    ma_counts = sum(medium_large > diams & diams > small_medium);
    la_counts = sum(medium_large < diams);

    hold on
    bar(1, sa_counts, 'FaceColor', [1 0 0]);
    bar(2, ma_counts, 'FaceColor', [1 0.5 0.5]);
    bar(3, la_counts, 'FaceColor', [0.6 0.6 0.6]);
    
    if k == 1 
        maxy = max([sa_counts, ma_counts, la_counts, maxy]);
        maxy2 = maxy*1.2;
    end
    
    set(gca, 'xtick', 1:3, 'xticklabel', {'Small' 'Meduim' 'Large'});
    
    text(0.3, 0.95, [' n = ' num2str(length(d)) ' \hspace{2mm} $\bar{d}$ = ' sprintf('%.2f', mean(diams))],...
        'interpreter','Latex', 'Units', 'normalized', 'FontWeight', 'bold', 'FontSize', 15);
    
    set(gca, 'FontName','Arial');
    xlabel('Diameter (um)');
    ylabel('Count');
    ylim([0 maxy2]);
    title(sprintf('Overall %s Histogram', state), 'FontWeight', 'Normal', 'FontSize', 15);
    
    saveas(fig, sprintf('%s Overal.emf', state)); % close
    
    drawnow
    
end
