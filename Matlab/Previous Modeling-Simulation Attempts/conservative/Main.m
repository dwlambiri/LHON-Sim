n_neur = 1000;
neur = rand(n_neur, 3); % x, y, r

%%

neur = M.neuron{1}';
n_neur = length(neur);

death_t = inf(n_neur, 1);

death_t(1) = 0; % Triggering cell
n_dead = 1;
dt = 1;
t = 0;

del = @(x) fprintf(repmat('\b',1,x));
nDispChar = 0; last_disp_num = -1;

while n_dead < n_neur
    t = t + dt;
    
    new_disp_num = round(100*n_dead/n_neur);
    if new_disp_num ~= last_disp_num
        del(nDispChar); nDispChar = fprintf('%.0f%%\t', new_disp_num);
        last_disp_num = new_disp_num;
    end
    
    t_passed_death = t - death_t;
    live = t_passed_death < 0;
    dead = ~live;
    n_dead = sum(dead);
    n_live = sum(live);
    dead_neur = neur(dead, :);
    dead_neur_t = t_passed_death(dead);
    live_neur = neur(live, :);
    tox = zeros(n_live,1);
    for k = 1:n_dead
        tox = tox + 700*dead_neur(k,3)*fcn(live_neur(:,1), live_neur(:,2), dead_neur_t(k), dead_neur(k,1:2));
    end
    % Update death_t
    d_l = death_t(live);
    d_l(tox > live_neur(:,3)) = t;
    death_t(live) = d_l;
end

%%

% plot(neur(:,1), neur(:,2), '.', 'markersize', 10)
M.plot.model();

hold on
h = plot(0, Inf, 'r.', 'markersize', 15);
axis equal

for t = 0:01:min(max(death_t)+.1, 1e10)
    dead = t > death_t;
    try
        set(h, 'XData', neur(dead, 1), 'YData', neur(dead, 2));
    catch
        break;
    end
    drawnow
    pause(0.01)
end
