function h = SUBplot(Siz, Gap, POS)

% subplot dimension
n1 = Siz(1); % number of rows
n2 = Siz(2); % number of columns

% These values would define the space between the graphs
% if equal to 1 there will be no space between graphs
nw = 1 - Gap(1); % normalized width
nh = 1 - Gap(2); % normalized height

% relative pos
relpos = @(k1,k2) [(1-nw)/n2/2 + (k2-1)/n2, (1-nh)/n1/2 + 1-k1/n1,nw/n2 nh/n1];
% abs pos
Pos = @(x) [POS(1:2) + x(1:2).*POS(3:4),x(3:4).*POS(3:4)];

h = zeros(Siz);

for k1 = 1:n1
    for k2 = 1:n2
        h(k1,k2) = axes('Units', 'Normalized', 'Position', Pos(relpos(k1,k2)));
        % turn off the labels
        set(gca,'xtick',[],'ytick',[]);
        axis off
    end
end
