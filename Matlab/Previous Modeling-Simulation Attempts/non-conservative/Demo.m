
%% Create Model
M = generate_model('file', 'mdl2', 'GUI'); % Create an eye nerve model

M.plot.model();
M.plot.histogram();

%% Process Model
M = M.create_csg(M); % Create Constructive Solid Geometry Model
M = generate_mesh(M, 'refine', 0, 'plot_mesh'); % Generate Mesh

%% Run Simulation
M = propogation_alg(M, 'init_insult', [-0 -20], 'GUI'); % Run Propogation Algorithm

%% Playback/record

M.P.anim('show_cont', 'dt', 200); % playback

