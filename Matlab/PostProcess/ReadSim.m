
function [mdl, setts, death_itr] = ReadSim

input_path_base = '..\..\LHON-Form\LHON-Form\Project_Output\Progression';

mdl = []; setts = []; death_itr = [];
[nam, Path_name] = uigetfile('*.prgim', 'Select Sim', input_path_base);
if Path_name == 0, return; end

fid = fopen([Path_name nam], 'r');

uid = fread(fid, 1, 'int64=>int64');
mdl = ReadModel(uid);

sing = 'single=>single';

setts.resolution = fread(fid, 1, sing);

setts.rate_live = fread(fid, 1, sing);
setts.rate_dead = fread(fid, 1, sing);
setts.rate_bound = fread(fid, 1, sing);
setts.rate_extra = fread(fid, 1, sing);

setts.tox_prod = fread(fid, 1, sing);
setts.detox_intra = fread(fid, 1, sing);
setts.detox_extra = fread(fid, 1, sing);

setts.death_tox_thres = fread(fid, 1, sing);
setts.init_insult = fread(fid, 3, sing);
setts.insult_tox = fread(fid, 1, sing);

death_itr = fread(fid, sing);

fclose(fid);

