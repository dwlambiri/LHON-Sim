function str = Toxication_Boundary(varargin)

%% Input Parsing

input_path_base = '..\LHON-Form\LHON-Form\bin\Debug\Progression';
output_path_base = '..\Paper\';

str = [];
sim_mode_nam = '';

num_sims = 0;
sim_par_name = {};
data = {};
no_gui = 0;

if nargin == 0
    no_gui = 1;
    choice = questdlg('Please choose selection type:', 'Input Selection', ...
        'Sweep batch', 'Individual', 'Cancel', 'Cancel');
    
    switch choice
        case 'Sweep batch'
            sim_mode_nam = 'User Sweep Batch';
            Path_name = uigetdir(input_path_base);
            if Path_name == 0, return; end
            read_sweep(Path_name);
            
        case 'Individual'
            sim_mode_nam = 'User Individual Selection';
            while(true)
                [nam, Path_name] = uigetfile('*.prgim', 'Select Sim', input_path_base);
                if nam ~= 0
                    read_individ([Path_name nam]);
                else
                    if num_sims == 0, return; end
                end
                
                choice = questdlg('Do you want to load another simulation?', ...
                    'Load More?', ...
                    'Yes', 'No', 'Yes');
                if strcmp(choice, 'No'), break; end
            end
            
        case 'Cancel'
            return;
        
    end
    
else
    Path_name = varargin{1};
    sim_mode_nam = varargin{2};
    
    if ~exist([input_path_base '\' Path_name], 'file')
        disp('file not found');
        return
    end
    
    % create output folder
    create_dir(output_path_base, sim_mode_nam);
    
    create_dir([output_path_base sim_mode_nam '\'], 'Figures');
    output_path_figs = [output_path_base sim_mode_nam '\Figures\'];
    
    create_dir([output_path_base sim_mode_nam '\'], 'Tables');
    output_path_tbls = [output_path_base sim_mode_nam '\Tables\'];
    
    if any(strfind(Path_name, '.prgim'));
        read_individ([input_path_base '\' Path_name]);
    else
        read_sweep([input_path_base '\' Path_name]);
    end
end

    function create_dir(path, name)
        if ~exist([path name], 'dir')
            mkdir(path, name);
        end
    end

%% Base Funcs

    function read_individ(pth)
        num_sims = num_sims + 1;
        [~, nam, ext] = fileparts(pth);
        sim_par_name(num_sims) = get_sim_name([nam ext]);
        fid = fopen(pth, 'r');
        data{num_sims} = fread(fid, 'uint8=>uint8');
        fclose(fid);
    end

    function read_sweep(prog_sweep_path)
        
        pgims = dir(prog_sweep_path);
        dr = cellfun(@(x) strcmp(x, '.') || strcmp(x, '..'), {pgims.name}, 'UniformOutput', false);
        pgims([dr{:}]) = [];
        if isletter(pgims(1).name(1))
            [~,sorted_idx] = sort([pgims.datenum]);
        else
            [~,sorted_idx] = sort(cellfun(@str2num, arrayfun(@(x) x.name(1:end-6), pgims, 'UniformOutput', false)));
        end
        num_sims = length(pgims);
        data = cell(num_sims,1);
        for kk = 1:num_sims
            nam = pgims(sorted_idx(kk)).name;
            fid = fopen([prog_sweep_path '\' nam], 'r');
            data{kk} = fread(fid, 'uint8=>uint8');
            sim_par_name(kk) = get_sim_name(nam);
            fclose(fid);
        end
    end

areal_prog_start = [];
chron_prog_start = [];


    function n = get_sim_name(nam)
        tkn = regexp(nam, '([\w.\s-]+).prgim', 'tokens');
        n = tkn{1};
    end

    function o = read_data(src, type, siz)
        o = double(typecast(src(cnt:cnt+siz-1), type));
        cnt = cnt + siz;
    end

%% File Processing

no_death_itr = any(strfind(Path_name, 'no_death_itr'));

for k1 = 1:num_sims
    cnt = 1;
    nerve_r(k1) = read_data(data{k1}, 'single', 4);
    max_r(k1) = read_data(data{k1}, 'single', 4);
    min_r(k1) = read_data(data{k1}, 'single', 4);
    clearance(k1) = read_data(data{k1}, 'single', 4);
    
    n_neur(k1) = read_data(data{k1}, 'int32', 4);
    resolution(k1) = read_data(data{k1}, 'single', 4);
    tolerance(k1) = read_data(data{k1}, 'single', 4);
    neur_rate(k1) = read_data(data{k1}, 'single', 4);
    
    org_im_siz(k1) = uint32(read_data(data{k1}, 'int32', 4));
    im_siz(k1) = uint32(read_data(data{k1}, 'uint16', 2));
    init_insult(k1,1) = read_data(data{k1}, 'single', 4);
    init_insult(k1,2) = read_data(data{k1}, 'single', 4);
    
    prog_n_frames(k1) = read_data(data{k1}, 'int32', 4);
    sim_time_ms(k1) = read_data(data{k1}, 'single', 4);
    if ~no_death_itr
        last_itr(k1) = read_data(data{k1}, 'uint32', 4);
    end
    
    for k2 = 1:prog_n_frames(k1)
        areal_progress_chron_val(k1, k2) = read_data(data{k1}, 'single', 4);
    end
    
    for k2 = 1:prog_n_frames(k1)
        chron_progress_areal_val(k1, k2) = read_data(data{k1}, 'single', 4);
    end
    if ~no_death_itr
        for k2 = 1:n_neur(k1)
            neur{k1}(k2,1) = read_data(data{k1}, 'uint8', 1) / 256 - .5;    % X
            neur{k1}(k2,2) = read_data(data{k1}, 'uint8', 1) / 256 - .5;    % Y
            neur{k1}(k2,3) = read_data(data{k1}, 'uint8', 1) / 40;          % Rad
            neur{k1}(k2,4) = read_data(data{k1}, 'uint8', 1) / 256;         % Death chron-prog
        end
    end
    
    areal_prog_start(k1) = cnt;
    chron_prog_start(k1) = cnt + prog_n_frames(k1) * im_siz(k1) * im_siz(k1);
end

str.neur = neur;
str.nerve_r = nerve_r;
str.n_neur = n_neur;
str.init_insult = init_insult;
str.sim_time_ms = sim_time_ms;
str.areal_progress_chron_val = areal_progress_chron_val;
str.chron_progress_areal_val = chron_progress_areal_val;

num_frames = prog_n_frames(1);

for k1 = 1:num_sims
    for k2 = 1:num_frames
        data{k1}([areal_prog_start(k1) chron_prog_start(k1)] + double(im_siz(k1)*im_siz(k1)*k2)) = 255;
    end
end

get_areal_im = @(m, x) reshape(data{m}((im_siz(m)*im_siz(m)*(x-1) + areal_prog_start(m)) : (im_siz(m)*im_siz(m)*x + areal_prog_start(m) - 1)), [im_siz(m) im_siz(m)]);

get_chron_im = @(m, x) reshape(data{m}((im_siz(m)*im_siz(m)*(x-1) + chron_prog_start(m)) : (im_siz(m)*im_siz(m)*x + chron_prog_start(m) - 1)), [im_siz(m) im_siz(m)]);

str.get_areal_im = get_areal_im;
str.get_chron_im = get_chron_im;

%% GUI Functions
str.Open_main_win = @Open_main_win;

f_main = [];
h_image = [];

mean_h = -inf;
k = 1;
show_cont = 1;
h_cont = [];

get_im = [];
cont = [];
f_cntl = [];

Gap = [.02 .02];

progress_mode = 0;
prog_mode_name = [];

str.sim_par_name = sim_par_name;

if no_gui
    Open_main_win();
    open_controls();
end

    function open_controls()
        f_cntl = figure('units', 'normalized', 'NumberTitle', 'off',...
            'Name', 'Controls', 'menu', 'none', 'CloseRequestFcn', @close_req);
        
        pos = 1;
        
        num_butts = 3;
        butt('Plot_RMS_difference', @Toxic_Difference_RMS);
        butt('Plot_Mean_Image', @Mean_Image);
        butt('Plot_Toxic_Sum_Comparison', @Toxic_Sum_Comparison);
        
        function butt(name, cb)
            uicontrol('String', name, 'Style', 'pushbutton', 'units', 'normalized',...
                'callback', @(~,~) cb(), 'position', [0 1-(pos/num_butts) 1 (1/num_butts)]);
            pos = pos + 1;
        end
    end

    function close_req(~,~)
        function get_pos_del(f)
            if ishandle(f)
                delete(f);
            end
        end
        get_pos_del(f_rms);
        get_pos_del(f_mean);
        get_pos_del(f_tox_sum);
        get_pos_del(f_main);
        get_pos_del(f_cntl);
    end

    function Open_main_win()
        
        f_main = figure('units', 'normalized', 'KeyPressFcn', @key_fcn, 'ButtonDownFcn', @mouse_fcn, 'WindowScrollWheelFcn', @wheel_fcn,...
            'NumberTitle', 'off', 'CloseRequestFcn', @close_req);
        
        % Find best axis dimensions
        screensize = get( groot, 'Screensize' );
        rat = screensize(3)/screensize(4);
        n_cols = round(sqrt(num_sims/rat));
        
        ax_dims = round([n_cols num_sims/n_cols]);
        
        axs_im = Utils.SUBplot(ax_dims, Gap, [0 0 1 1]);
        axs_im = axs_im';
        axs_cnt = Utils.SUBplot(ax_dims, Gap, [0 0 1 1]);
        axs_cnt = axs_cnt';
        linkaxes([axs_im(:); axs_cnt(:)]);
        
        for kk = 1:num_sims
            axes(axs_im(kk))
            [h_image(kk), h_cont(kk)] = draw_image(get_chron_im, kk, 1, axs_cnt(kk));
            axis on, set(gca, 'Xtick', [], 'Ytick', []);
            xlabel(sim_par_name{kk}, 'position', [50, 0, 0], 'color', 'r', 'FontWeight', 'bold', 'FontSize', 12);
            if (kk == num_sims) colorbar(axs_im(kk));
            end
        end
                
        upd_progr_mode_state();
        
    end

    function [h_image, h_cont] = draw_image(get_im, s, k, axs_cnt, varargin)
        f = @(f) (cellfun(@(x) ischar(x) && strcmp(x,f), varargin));
        avg = any(f('avg'));
        
        number_of_contours = 5;
        if ~avg
            im = double(get_im(s, k));
        else
            avg_im = zeros(im_siz(1));
            for ss = s(2:end)
                avg_im = avg_im + double(get_im(ss, k));
            end
            im = avg_im./num_sims;
            s = s(1);
        end
        
        h_image = imshow(im./256, [], 'initialMagnification', 200);
        
        hold on
        colormap default
        set(get(gca,'Children'),'HitTest','off');
        
        if axs_cnt == 0
            axs_cnt = axes('Units', get(gca, 'Units'), 'Position', get(gca, 'Position'));
        else
            axes(axs_cnt)
        end
        [~, h_cont] = imcontour(im, number_of_contours);
        colormap(axs_cnt, jet(number_of_contours));
        set(axs_cnt, 'Color', 'None');
        set(h_cont, 'LineWidth', 1);
        
        hold on
        ims = double(im_siz(s));
        bound = @(x) max(min(x, ims-1), 2);
        plot(bound((init_insult(s,1)/nerve_r(s)/2 + 0.5)*ims), bound((init_insult(s,2)/nerve_r(s)/2 + 0.5)*ims), 'r.');
        
        axis off
    end

%% Main Update

    function key_fcn(~,evt)
        if strcmp(evt.Key, 'rightarrow')
            k = k + 1; k = min(k, num_frames);
        elseif strcmp(evt.Key, 'leftarrow')
            k = k - 1; k = max(k, 1);
        elseif strcmp(evt.Key, 'home')
            k = 1;
        elseif strcmp(evt.Key, 'end')
            k = num_frames;
        elseif strcmp(evt.Key, 'c')
            show_cont = ~show_cont;
        elseif strcmp(evt.Key, 'p')
            progress_mode = ~progress_mode;
            upd_progr_mode_state();
        end
        update_frames();
    end

    function upd_progr_mode_state
        if progress_mode
            get_im = get_areal_im;
            prog_mode_name = 'Areal Progression';
        else
            get_im = get_chron_im;
            prog_mode_name = 'Chronological Progression';
        end
    end

    function update_frames
        set(f_main, 'Name', [sim_mode_nam sprintf(': %.0f %%', k*100/num_frames) ' - ' prog_mode_name]);
        mean_im(:) = 0;
        for n = 1:num_sims
            im_tmp = get_im(n,k);
            set(h_image(n), 'Cdata', im_tmp);
            
            if show_cont, cont = double(im_tmp);
            else cont = []; end
            set(h_cont(n), 'ZData', cont);
            
            mean_im = mean_im + double(im_tmp);
        end
        if ishandle(mean_h)
            set(mean_h, 'Cdata', uint8(mean_im ./ num_sims));
        end
    end

    function mouse_fcn(~,~)
        if (strcmp(get(f_main, 'selectiontype'), 'extend'))
            show_cont = ~show_cont;
            update_frames();
        end
    end

    function wheel_fcn(~, evt)
        k = k + evt.VerticalScrollCount;
        k = min(k, num_frames);
        k = max(k, 1);
        update_frames();
    end

%% Plot Data Analysis Functions (Helpers)

str.Mean_Image = @Mean_Image;
str.Toxic_Sum_Comparison = @Toxic_Sum_Comparison;
str.Dead_Axons_tbl = @Dead_Axons_tbl;
str.Mean_Distr = @Mean_Distr;
str.Areal_Prog_tbl = @Areal_Prog_tbl;
str.TwoD_Toxic_Level = @TwoD_Toxic_Level;
str.Toxic_Difference_RMS = @Toxic_Difference_RMS;
str.Dead_Axons_3D = @Dead_Axons_3D;
str.Anova1_Dead_Axons = @Anova1_Dead_Axons;

    function [getim, xlb, tim_nam] = tim_type(tim)
        if tim == 'c'
            getim = get_chron_im;
            xlb = 'Unit of Time';
            tim_nam = 'Chronological';
        else
            getim = get_areal_im;
            xlb = 'Areal Progression (%)';
            tim_nam = 'Areal';
        end
    end

    function cfg = create_savable_fig(fig_type_nam)
        fgh = figure('Color', 'w', 'KeyPressFcn', @key_fcn, 'CloseRequestFcn', @close_cb);
        fil_nam = [output_path_figs fig_type_nam '.emf'];
        function key_fcn(~,evt)
            if strcmp(evt.Key, 's')
                save_and_close_fig();
            elseif strcmp(evt.Key, 'c')
                fprintf('Copying to clipboard...');
                print('-clipboard', '-dmeta')
                fprintf(' Done.\n');
            end
        end
        cfg = @save_and_close_fig;
        function save_and_close_fig()
            drawnow
            ps = get(fgh, 'Position');
            fgh.PaperPosition = [0 0 ps(3) ps(4)]/70;
            fprintf('Saving to %s...', fil_nam);
            print(fil_nam, '-dmeta')
            print('-clipboard', '-dmeta');
            fprintf(' Done.\n');
            delete(fgh);
        end
        function close_cb(~,~)
            if (evalin('base', 'save_on_close'))
                save_and_close_fig();
            else
                delete(fgh);
            end
        end
    end

	function save_anova1_plot(dat, nam)
        fig_type_nam = {['Anova1 Test (Boxplot) - ' nam] ['Anova1 Test (Table) - ' nam]};
        
        anova1(dat');
        for k = 1:2
            set(gcf, 'Color', 'w');
            title(fig_type_nam{k});
            ylabel('Unit of Time');
            xlabel('Index of Simulation');
            fil_nam = [output_path_figs fig_type_nam{k} '.emf'];
            fprintf('Saving to %s...', fil_nam);
            print(fil_nam, '-dmeta')
            print('-clipboard', '-dmeta');
            fprintf(' Done.\n');
            delete(gcf)
            drawnow
        end
    end

%% Plot Data Analysis Functions

    function Mean_Distr(n_divs)
        
        n_bins = 20;
        
        cfg = create_savable_fig('Axon-Distr');
        
        find_within = @(s, m, M) neur{s}(:,1) > m & neur{s}(:,1) < M;
        
        for k = 1:n_divs
            rads = [];
            n_axons = [];
            for s = 1:num_sims
                ids = find_within(s, (k-1)/n_divs - 0.5, k/n_divs - 0.5);
                n_axons(s) = sum(ids);
                rads = [rads; neur{s}(ids,3)];
            end
            
            hs(k) = subplot(1, n_divs, k);
            histogram(rads, n_bins, 'Normalization', 'probability')
            xlabel({sprintf('Number: %.0f (%.1f)', mean(n_axons), std(n_axons)) sprintf('Radius: %.2f (%.3f)', mean(rads), std(rads)/num_sims)},...
                'Units', 'normalized', 'position', [0.6, 0.95, 0], 'FontWeight', 'bold', 'FontSize', 8);
            if k == 1
                ylabel('Probability');
            end
            if k == round(n_divs/2)
                htxt = text(0, 0, 'Radius(um)');
                set(htxt, 'Units', 'normalized', 'Position', [0.35 -0.09 0]);
                title(sprintf('Axon Radii Distribution for %.0f random models', num_sims));
            end
        end
        linkaxes(hs)
        if evalin('base', 'save_and_close_auto'), cfg(); end
        
        % set(gca, 'ylim', [0 16000])
    end

    function Mean_Image()
        figure('units', 'normalized', 'Color', 'w');
        mean_im = double(get_areal_im(1,1));
        Utils.SUBplot([1 1], Gap, [0 0 1 1]);
        mean_h = imshow(uint8(mean_im), 'initialMagnification', 500);
        colormap default
        colorbar
    end

    function TwoD_Toxic_Level(tim, varargin)
        f = @(f) (cellfun(@(x) ischar(x) && strcmp(x,f), varargin));
        avg = any(f('avg'));
        
        [getim, xlb, tim_nam] = tim_type(tim);
        cfg = create_savable_fig(['2D Toxic Level (' tim_nam ')']);
        selected_frames = [15 25 45 70 95] / 5;
        % axs_im = Utils.SUBplot([num_sims length(selected_frames)], Gap, [0.05 0.1 .85 .85]);
        if avg
            ns = 1;
        else
            ns = num_sims;
        end
        [axs_im, clb] = Utils.Image_Array([ns length(selected_frames)], [1 1]*100, 'sc', 1);
        for k = 1:ns
            for kk = 1:length(selected_frames)
                if ishandle(axs_im(k, kk))
                    axes(axs_im(k, kk));
                else
                    return;
                end
                if avg
                    draw_image(getim, 1:num_sims, selected_frames(kk), 0, 'avg');
                else
                    draw_image(getim, k, selected_frames(kk), 0);
                end
                
                axis on, set(gca, 'XTick', [], 'YTick', []);
                if k == 1 && kk == round(length(selected_frames)/2)
                    title(['Comparison of Different ' sim_mode_nam 's']);
                end
                if k == ns
                    xlabel(sprintf('%.0f', selected_frames(kk)/num_frames * 100));
                    if kk == round(length(selected_frames)/2)
                        htxt = text(0, 0, xlb);
                        set(htxt, 'Units', 'normalized', 'Position', [-.3 -0.3 0]);
                    end
                end
                if kk == 1
                    hyl = ylabel(sim_par_name{k}, 'color', 'r', 'FontWeight', 'bold', 'FontSize', 12);
                end
                drawnow
            end
        end
        clb();
        
        if evalin('base', 'save_and_close_auto'), cfg(); end
    end

    function Toxic_Difference_RMS(tim)
        [getim, xlb, tim_nam] = tim_type(tim);
        
        cfg = create_savable_fig(['Toxic Difference RMS (' tim_nam ')']);
        
        for f = 1:num_frames
            mean_im(:) = 0;
            for n = 1:num_sims
                im_tmp = getim(n,f);
                mean_im = mean_im + double(im_tmp);
            end
            mn = mean_im ./ num_sims;
            for n = 1:num_sims
                im_tmp = double(getim(n,f)) - mn;
                im_tmp = im_tmp .* im_tmp;
                rms(n, f) = sqrt(sum(im_tmp(:)) / numel(im_tmp));
            end
        end
        
        hp = plot((1:num_frames)*100/num_frames, rms);
        
        title(['RMS of Toxic Difference Among ' sim_mode_nam]);
        ylabel('RMS of Normalized Toxic Difference');
        xlabel(xlb)
        legend(hp, sim_par_name, 'Location', 'northwest');
        
        if evalin('base', 'save_and_close_auto'), cfg(); end
    end

    function Toxic_Sum_Comparison()
        
        cfg = create_savable_fig('Toxic Sum Comparison');
        
        hold on
        for n = 1:num_sims
            for m = 1:num_frames
                im = get_chron_im(n,m);
                s(m) = sum(im(:));
            end
            hp(n) = plot((1:num_frames)*100/num_frames, s);
        end
        title('Sum of Released Toxic');
        ylabel('Toxic level');
        xlabel('Unit of Time')
        legend(hp, sim_par_name, 'Location', 'northwest');
        if evalin('base', 'save_and_close_auto'), cfg(); end
        
    end

    function Areal_Prog_tbl(write_table, ano, varargin)
        f = @(f) (cellfun(@(x) ischar(x) && strcmp(x,f), varargin));
        avg = any(f('avg'));
        no_fig = write_table == 2; % Table only
        
        name = 'Areal-Prog';
        if avg, name = [name '-Avg']; end
        
        if ~no_fig
            cfg = create_savable_fig(name);
            hold on
        end
        
        anov = [];
        
        a = chron_progress_areal_val * 100;
        c = (1:num_frames)/num_frames * 100;
        
        if ~no_fig
            if ~avg
                plot(c, a, '.-');
                legend(sim_par_name, 'Location', 'NorthWest');
                title('Areal Progression vs Time')
            else
                plot(c, mean(a), '.-');
                title('Averaged Areal Progression vs Time')
            end
            xlabel('Unit of Time');
            ylabel('Areal Progression (%)')
            
        end
        
        if write_table
            file = [output_path_tbls 'Areal-Prog'];
            xlswrite(file,['Unit of Time' sim_par_name; num2cell([c' a'])]);
            if ~evalin('base', 'save_and_close_auto'), winopen([file '.xls']); end
        end
        
        if ano, save_anova1_plot(a, 'Areal Prog'); end
        
        if ~no_fig && evalin('base', 'save_and_close_auto'), cfg(); end
    end

    function Dead_Axons_tbl(write_table, ano)
        
        no_fig = write_table == 2; % Table only
        
        table_every_percent = 5; % every 5%
        
        number_of_groups = 3;
        Group_Names = {'Small' 'Medium' 'Large'};
        
        for ks = 1:num_sims
            M = neur{ks};
            [~,sort_idx] = sort(M(:,4));
            r{ks} = M(sort_idx, 3);
            death_itr{ks} = M(sort_idx,4);
        end
        mr = 0.19;
        Mr = 3;
        
        stp = (Mr - mr) / number_of_groups;
        grp = 0;
        
        anov = [];
        
        n_cols = 100/table_every_percent;
        
        for k = mr:stp:Mr-stp
            grp = grp + 1;
            if ~no_fig
                cfg = create_savable_fig(['Dead Axons - ' Group_Names{grp}]);
            end
            hold on
            
            for ks = 1:num_sims
                r_k = (r{ks} > k & r{ks} <= k + stp);
                n_n = sum(r_k);
                dead = death_itr{ks}(r_k);
                n_act = sum(dead > 0);
                dead(dead == 0) = [];
                if ~no_fig
                    plot(dead*100, (1:n_act)./n_n*100, '.-');
                end
                [N, ~] = histcounts(dead, n_cols, 'BinLimits', [0 1]);
                tbl(ks, :) = cumsum(N)/n_n * 100;
                anov(ks, (grp-1)*n_cols+1:grp*n_cols, grp) = tbl(ks, :);
            end
            if ~no_fig
                title(sprintf('Death of %s Axons Over Time (%.1f um to %0.1f um)', Group_Names{grp}, k, k + stp));
                xlabel('Unit of Time');
                ylabel('# of Dead Axons / # of Total Axons (%)');
                legend(sim_par_name, 'Location', 'northwest');
                if evalin('base', 'save_and_close_auto'), cfg(); end
            end
            
            if write_table
                f_data = ['Unit of Time' sim_par_name; num2cell([(table_every_percent:table_every_percent:100)' tbl'])];
                file = [output_path_tbls 'Dead_Axons-' Group_Names{grp} '-' sim_mode_nam];
                xlswrite(file, f_data);
                if ~evalin('base', 'save_and_close_auto'), winopen([file '.xls']); end
            end
        end
        if ano
            for grp = 1:number_of_groups
                save_anova1_plot(anov(:,:,grp), ['Dead Axons - ' Group_Names{grp}]);
            end
        end
        
    end

    function Dead_Axons_3D()
        
        table_every_percent = 5; % every 5%
        
        number_of_groups = 3;
        Group_Names = {'Small' 'Medium' 'Large'};
        
        for ks = 1
            M = neur{ks};
            [~,sort_idx] = sort(M(:,4));
            r{ks} = M(sort_idx, 3);
            death_itr{ks} = M(sort_idx,4);
        end
        mr = 0.19;
        Mr = 3;
        
        stp = (Mr - mr) / number_of_groups;
        grp = 0;
        
        cfg = create_savable_fig('Dead Axons - 3D');
        title('Death of Axons Over Time');
        hold on
        
        for k = mr:stp:Mr-stp
            grp = grp + 1;
            
            r_k = (r{ks} > k & r{ks} <= k + stp);
            n_n = sum(r_k);
            dead = death_itr{ks}(r_k);
            n_act = sum(dead > 0);
            dead(dead == 0) = [];
            mn(grp) = mean(r{ks}(r_k));
            st(grp) = std(r{ks}(r_k));
            pop(grp) = n_n;
            
            plot3(dead*100, mn(grp)*ones(1, n_act), (1:n_act)./n_n*100, '.-');
            [N, ~] = histcounts(dead, 100/table_every_percent, 'BinLimits', [0 1]);
            tbl(ks, :) = cumsum(N)/n_n * 100;
            
        end
        
        xlabel('Unit of Time');
        ylabel('Axon Radius');
        zlabel('# of Dead Axons / # of Total Axons (%)');
        for grp = 1:length(Group_Names)
            grp_txt = sprintf('%6s (%.1fum\\pm%.2fum) Pop: %.0f',Group_Names{grp}, mn(grp), st(grp), pop(grp));
            % text(0, mn(grp), 100, grp_txt);
            leg{grp} = grp_txt;
        end
        legend(leg, 'Location', 'northwest');
        view([-30 60]);
        grid on
        if evalin('base', 'save_and_close_auto'), cfg(); end
        
    end

end
