
save_and_close_auto = 0; % Save and Close figures Automatically?
save_on_close = 0;

%% Init and Read file

str_res = Toxication_Boundary('Resolution', 'Resolution');
str_tol = Toxication_Boundary('Tolerance', 'Tolerance');
str_rate = Toxication_Boundary('Neur Rate', 'Axon Diffusion Rate');
str_rep = Toxication_Boundary('Repeat-57', 'Repetition');
str_rep5 = Toxication_Boundary('Repeat-5', 'Repetition');
str_ins_rad = Toxication_Boundary('Insult-Rad', 'Radial Insult');
str_ins_per = Toxication_Boundary('Insult-Peri', 'Peripheral Insult');

%% Resolution

str_res.TwoD_Toxic_Level('a');
str_res.Toxic_Sum_Comparison();
str_res.Toxic_Difference_RMS('a');
str_res.Areal_Prog_tbl(1, 1);
str_res.Dead_Axons_tbl(1, 1);

%% Tolerance

str_tol.TwoD_Toxic_Level('c');
str_tol.Toxic_Sum_Comparison();
str_tol.Toxic_Difference_RMS('c');
str_tol.Areal_Prog_tbl(1, 1);
str_tol.Dead_Axons_tbl(1, 1);


%% Rate

str_rate.TwoD_Toxic_Level('c');
str_rate.Toxic_Sum_Comparison();
str_rate.Toxic_Difference_RMS('c');
str_rate.Areal_Prog_tbl(1, 1);
str_rate.Dead_Axons_tbl(1, 1);

%% Repeat

% str_rep.Mean_Distr(5); % Quintile

str_rep.TwoD_Toxic_Level('c', 'avg');
str_rep.Dead_Axons_3D();
str_rep5.Toxic_Sum_Comparison();
str_rep.Toxic_Sum_Comparison(); % Average(TODO)
str_rep.Toxic_Difference_RMS('c');
str_rep.Areal_Prog_tbl(2, 1);
str_rep.Dead_Axons_tbl(2, 1);


%% Insult-Rad

str_ins_rad.TwoD_Toxic_Level('c');
str_ins_rad.Toxic_Sum_Comparison();
str_ins_rad.Toxic_Difference_RMS('c');
str_ins_rad.Areal_Prog_tbl(1, 1);
str_ins_rad.Dead_Axons_tbl(1, 1);

%% Insult-Peri

str_ins_per.TwoD_Toxic_Level('c');
str_ins_per.Toxic_Sum_Comparison();
str_ins_per.Toxic_Difference_RMS('c');
str_ins_per.Areal_Prog_tbl(1, 1);
str_ins_per.Dead_Axons_tbl(1, 1);

