
dat = [...
3   10  416     5710
15  15  608     10580
40  20  816     15970
95  25  1008    24450
200 30  1216    34700
];

resolution = dat(:,2);
image_siz = dat(:,3);
time = dat(:,1);
itr = dat(:,4);

figure, plot(resolution.^2, itr, '-o')
title('iteration over resolution^2');
% 
figure, plot(itr.^2, time, '-o')
title('time over iteration^2');

figure, plot(resolution.^4, time, 'o')
title('time over resolution^4');

