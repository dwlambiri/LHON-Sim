
function mdl = ReadModel(uid)

mdl_path = '..\..\LHON-Form\LHON-Form\Project_Output\Models\';

mdl = [];
if nargin == 0
    [nam, Path_name] = uigetfile('*.mdat', 'Select Sim', mdl_path);
    if Path_name == 0, return; end
    fil = [Path_name nam];
else
    dr = dir(mdl_path);
    uid_str = dec2base(uid, 36);
    idx = strncmp({dr.name}, uid_str, length(uid_str));
    if ~any(idx)
        error('No match found for the UID');
    elseif sum(idx) > 1
        error('more than one match for UID');
    end
    fil = [mdl_path dr(idx).name];
end

fid = fopen(fil, 'r');

mdl.scale = fread(fid, 1, 'single=>single');
mdl.n_axons = fread(fid, 1, 'int32=>int32');
mdl.axon_coor = reshape(fread(fid, 'single=>single'), 3, [])';

fclose(fid);

