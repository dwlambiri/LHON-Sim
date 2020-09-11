
% ========= OLD GAMMA Study

Igama = @(X,A,B) B^A/gamma(A)*X.^(-A-1).*exp(-B./X);

x_max = 1;
x = 0:.002:x_max;

figure, hold on
fixed_beta = 1;
k = 0;
leg = [];
for p = 0.1:0.15:.6
    k = k + 1;
    if fixed_beta
        alpha = p;
        beta = .15;
    else
        alpha = 3;
        beta = p;
    end
    
    plot(0.4 + x * 5.6 / x_max, Igama(x, alpha, beta));
    %beta/(alpha+1)
    leg{k} = num2str(beta/(alpha-1));
end

legend(leg)