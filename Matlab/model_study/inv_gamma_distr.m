%% Init
% close

theory = 0;
numeri = 1;

alpha = 2;

Igama = @(X,A,B) B^A/gamma(A)*X.^(-A-1).*exp(-B./X);

x = 0:.002:1;

min_r = 0.19;
max_r = 6.87;

transform_x = @(x) min_r + x * (max_r - min_r);
transform_back_x = @(x) (x - min_r) / (max_r - min_r);

%% Theoretical
if theory
    
    figure, hold on
    k = 0;
    leg = [];
    
    Mean = 0.9:0.1:1.3;
    
    for m = Mean
        beta = transform_back_x(m) * (alpha-1);
        plot(transform_x(x), Igama(x, alpha, beta));
        k = k + 1;
        leg{k} = num2str(m);
    end
    
    legend(leg)
    
end

%% Numerical

if numeri
    
    m = 1.1;
    
    beta = transform_back_x(m) * (alpha-1);
    nums0 = transform_x(1./gamrnd(alpha, 1/beta, 500000, 1));
    
    figure
    nums = nums0;
    % nums = nums.^1.5;
    nums(nums0 > 6) = [];
    
    fprintf('std: %.1f\n', std(nums))
    fprintf('mean: %.1f\n', mean(nums))
    
    histogram(nums, 200)
    
    xlim([0 max_r])
    
end

