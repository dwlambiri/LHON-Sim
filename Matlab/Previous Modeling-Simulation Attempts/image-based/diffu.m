function im = diffu(im, barr, rate)

s = size(im);

dif = zeros(s);

for k1 = 2:s(1)-1
    for k2 = 2:s(2)-1
        if (~im(k1,k2)), continue; end
        if (barr(k1,k2)), continue; end
        
        num = 4 - (barr(k1-1, k2) + barr(k1+1, k2) + barr(k1, k2-1) + barr(k1, k2+1));
        
        if (num == 0), continue; end
        
        if (~barr(k1-1, k2)), dif(k1-1, k2) = dif(k1-1, k2) + im(k1,k2)/num; end
        if (~barr(k1+1, k2)), dif(k1+1, k2) = dif(k1+1, k2) + im(k1,k2)/num; end
        if (~barr(k1, k2-1)), dif(k1, k2-1) = dif(k1, k2-1) + im(k1,k2)/num; end
        if (~barr(k1, k2+1)), dif(k1, k2+1) = dif(k1, k2+1) + im(k1,k2)/num; end
    end
end

for k1 = 1:s(1)
    for k2 = 1:s(2)
        im(k1,k2) = rate*im(k1,k2) + (1-rate) * dif(k1,k2);
    end
end

% im = rate*im + (1-rate) * dif;

