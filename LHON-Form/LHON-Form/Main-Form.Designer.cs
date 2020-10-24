using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CustomControls;

namespace LHON_Form
{
    partial class Main_Form
    {
        const bool first_compile = true;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_save_model = new System.Windows.Forms.Button();
            this.btn_load_model = new System.Windows.Forms.Button();
            this.btn_redraw = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_num_axons = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_clearance = new System.Windows.Forms.TextBox();
            this.txt_nerve_scale = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_nerve_siz = new System.Windows.Forms.Label();
            this.chk_strict_rad = new System.Windows.Forms.CheckBox();
            this.chk_var_death = new System.Windows.Forms.GroupBox();
            this.chk_var_thr = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.txt_var_death = new System.Windows.Forms.TextBox();
            this.txt_on_death_tox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_insult_tox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_tox_prod_rate = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.txt_death_tox_threshold = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label26 = new System.Windows.Forms.Label();
            this.txt_rate_live = new System.Windows.Forms.TextBox();
            this.txt_rate_bound = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_rate_dead = new System.Windows.Forms.TextBox();
            this.txt_rate_extra = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_detox_extra = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txt_detox_intra = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.txt_resolution = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbl_image_siz = new System.Windows.Forms.Label();
            this.btn_preprocess = new System.Windows.Forms.Button();
            this.btn_load_setts = new System.Windows.Forms.Button();
            this.btn_save_setts = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_save_prog = new System.Windows.Forms.Button();
            this.btn_sweep = new System.Windows.Forms.Button();
            this.cmb_sw_sel1 = new System.Windows.Forms.ComboBox();
            this.chk_save_sw_prog = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label40 = new System.Windows.Forms.Label();
            this.txt_layer_to_display = new System.Windows.Forms.TextBox();
            this.txt_rec_inerval = new System.Windows.Forms.TextBox();
            this.chk_show_axons = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.chk_show_tox = new System.Windows.Forms.CheckBox();
            this.chk_rec_avi = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_sim_time = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_alive_axons_perc = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_itr = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label37 = new System.Windows.Forms.Label();
            this.lbl_max_tox = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_itr_s = new System.Windows.Forms.Label();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_max_density = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_tox = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_density = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statlbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statlbl_sweep = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txt_sw_range1 = new CustomControls.CueTextBox();
            this.cmb_sw_sel2 = new System.Windows.Forms.ComboBox();
            this.txt_sw_range2 = new CustomControls.CueTextBox();
            this.txt_delay_ms = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_snapshot = new System.Windows.Forms.Button();
            this.txt_stop_itr = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_stop_time = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txt_block_siz = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btn_clr = new System.Windows.Forms.Button();
            this.txt_status = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txt_3d_layers = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.txt_3d_tox_start = new System.Windows.Forms.TextBox();
            this.txt_3d_tox_stop = new System.Windows.Forms.TextBox();
            this.picB = new CustomControls.PictureBoxWithInterpolationMode();
            this.groupBox1.SuspendLayout();
            this.chk_var_death.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picB)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_start.Location = new System.Drawing.Point(16, 9);
            this.btn_start.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(216, 52);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "&Start";
            this.btn_start.UseVisualStyleBackColor = true;
            // 
            // btn_reset
            // 
            this.btn_reset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_reset.Location = new System.Drawing.Point(268, 9);
            this.btn_reset.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(210, 52);
            this.btn_reset.TabIndex = 5;
            this.btn_reset.Text = "&Reset";
            this.btn_reset.UseVisualStyleBackColor = true;
            // 
            // btn_save_model
            // 
            this.btn_save_model.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_save_model.Location = new System.Drawing.Point(70, 227);
            this.btn_save_model.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_save_model.Name = "btn_save_model";
            this.btn_save_model.Size = new System.Drawing.Size(210, 70);
            this.btn_save_model.TabIndex = 9;
            this.btn_save_model.Text = "Save Model";
            this.btn_save_model.UseVisualStyleBackColor = true;
            // 
            // btn_load_model
            // 
            this.btn_load_model.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_load_model.Location = new System.Drawing.Point(294, 227);
            this.btn_load_model.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_load_model.Name = "btn_load_model";
            this.btn_load_model.Size = new System.Drawing.Size(210, 70);
            this.btn_load_model.TabIndex = 10;
            this.btn_load_model.Text = "Load Model";
            this.btn_load_model.UseVisualStyleBackColor = true;
            // 
            // btn_redraw
            // 
            this.btn_redraw.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_redraw.Location = new System.Drawing.Point(514, 227);
            this.btn_redraw.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_redraw.Name = "btn_redraw";
            this.btn_redraw.Size = new System.Drawing.Size(178, 70);
            this.btn_redraw.TabIndex = 11;
            this.btn_redraw.Text = "Generate";
            this.btn_redraw.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(74, 174);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(199, 32);
            this.label4.TabIndex = 19;
            this.label4.Text = "Num of Axons:";
            // 
            // lbl_num_axons
            // 
            this.lbl_num_axons.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_num_axons.AutoSize = true;
            this.lbl_num_axons.ForeColor = System.Drawing.Color.Maroon;
            this.lbl_num_axons.Location = new System.Drawing.Point(288, 174);
            this.lbl_num_axons.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_num_axons.Name = "lbl_num_axons";
            this.lbl_num_axons.Size = new System.Drawing.Size(31, 32);
            this.lbl_num_axons.TabIndex = 18;
            this.lbl_num_axons.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txt_clearance);
            this.groupBox1.Controls.Add(this.txt_nerve_scale);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lbl_nerve_siz);
            this.groupBox1.Controls.Add(this.chk_strict_rad);
            this.groupBox1.Controls.Add(this.btn_redraw);
            this.groupBox1.Controls.Add(this.btn_save_model);
            this.groupBox1.Controls.Add(this.btn_load_model);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbl_num_axons);
            this.groupBox1.Location = new System.Drawing.Point(1486, 23);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox1.Size = new System.Drawing.Size(746, 308);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(338, 118);
            this.label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(191, 32);
            this.label15.TabIndex = 44;
            this.label15.Text = "Clearance um";
            // 
            // txt_clearance
            // 
            this.txt_clearance.Location = new System.Drawing.Point(534, 116);
            this.txt_clearance.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_clearance.Name = "txt_clearance";
            this.txt_clearance.Size = new System.Drawing.Size(100, 38);
            this.txt_clearance.TabIndex = 43;
            this.txt_clearance.Text = "0";
            // 
            // txt_nerve_scale
            // 
            this.txt_nerve_scale.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_nerve_scale.Location = new System.Drawing.Point(372, 54);
            this.txt_nerve_scale.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_nerve_scale.Name = "txt_nerve_scale";
            this.txt_nerve_scale.Size = new System.Drawing.Size(132, 38);
            this.txt_nerve_scale.TabIndex = 39;
            this.txt_nerve_scale.Text = "10";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(74, 54);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(262, 32);
            this.label11.TabIndex = 38;
            this.label11.Text = "Nerve Diameter (%)";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // lbl_nerve_siz
            // 
            this.lbl_nerve_siz.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_nerve_siz.AutoSize = true;
            this.lbl_nerve_siz.ForeColor = System.Drawing.Color.Maroon;
            this.lbl_nerve_siz.Location = new System.Drawing.Point(549, 54);
            this.lbl_nerve_siz.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_nerve_siz.Name = "lbl_nerve_siz";
            this.lbl_nerve_siz.Size = new System.Drawing.Size(77, 32);
            this.lbl_nerve_siz.TabIndex = 37;
            this.lbl_nerve_siz.Text = "0 um";
            // 
            // chk_strict_rad
            // 
            this.chk_strict_rad.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chk_strict_rad.AutoSize = true;
            this.chk_strict_rad.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chk_strict_rad.Location = new System.Drawing.Point(80, 110);
            this.chk_strict_rad.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_strict_rad.Name = "chk_strict_rad";
            this.chk_strict_rad.Size = new System.Drawing.Size(214, 36);
            this.chk_strict_rad.TabIndex = 34;
            this.chk_strict_rad.Text = "Strict Radius";
            this.chk_strict_rad.UseVisualStyleBackColor = false;
            // 
            // chk_var_death
            // 
            this.chk_var_death.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_var_death.Controls.Add(this.chk_var_thr);
            this.chk_var_death.Controls.Add(this.label41);
            this.chk_var_death.Controls.Add(this.txt_var_death);
            this.chk_var_death.Controls.Add(this.txt_on_death_tox);
            this.chk_var_death.Controls.Add(this.label10);
            this.chk_var_death.Controls.Add(this.txt_insult_tox);
            this.chk_var_death.Controls.Add(this.label6);
            this.chk_var_death.Controls.Add(this.txt_tox_prod_rate);
            this.chk_var_death.Controls.Add(this.label29);
            this.chk_var_death.Controls.Add(this.label36);
            this.chk_var_death.Controls.Add(this.txt_death_tox_threshold);
            this.chk_var_death.Controls.Add(this.groupBox7);
            this.chk_var_death.Controls.Add(this.groupBox6);
            this.chk_var_death.Controls.Add(this.txt_resolution);
            this.chk_var_death.Controls.Add(this.label9);
            this.chk_var_death.Controls.Add(this.label12);
            this.chk_var_death.Controls.Add(this.lbl_image_siz);
            this.chk_var_death.Controls.Add(this.btn_preprocess);
            this.chk_var_death.Controls.Add(this.btn_load_setts);
            this.chk_var_death.Controls.Add(this.btn_save_setts);
            this.chk_var_death.Location = new System.Drawing.Point(1486, 356);
            this.chk_var_death.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_var_death.Name = "chk_var_death";
            this.chk_var_death.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_var_death.Size = new System.Drawing.Size(746, 889);
            this.chk_var_death.TabIndex = 26;
            this.chk_var_death.TabStop = false;
            this.chk_var_death.Text = "Settings";
            // 
            // chk_var_thr
            // 
            this.chk_var_thr.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chk_var_thr.AutoSize = true;
            this.chk_var_thr.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chk_var_thr.Location = new System.Drawing.Point(538, 427);
            this.chk_var_thr.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_var_thr.Name = "chk_var_thr";
            this.chk_var_thr.Size = new System.Drawing.Size(203, 36);
            this.chk_var_thr.TabIndex = 45;
            this.chk_var_thr.Text = "Use Var Thr";
            this.chk_var_thr.UseVisualStyleBackColor = false;
            // 
            // label41
            // 
            this.label41.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(31, 428);
            this.label41.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(365, 32);
            this.label41.TabIndex = 48;
            this.label41.Text = "Var Death DT*(1+VD*x/dim)";
            // 
            // txt_var_death
            // 
            this.txt_var_death.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_var_death.Location = new System.Drawing.Point(400, 425);
            this.txt_var_death.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_var_death.Name = "txt_var_death";
            this.txt_var_death.Size = new System.Drawing.Size(126, 38);
            this.txt_var_death.TabIndex = 47;
            this.txt_var_death.Text = "0.4";
            // 
            // txt_on_death_tox
            // 
            this.txt_on_death_tox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_on_death_tox.Location = new System.Drawing.Point(404, 573);
            this.txt_on_death_tox.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_on_death_tox.Name = "txt_on_death_tox";
            this.txt_on_death_tox.Size = new System.Drawing.Size(126, 38);
            this.txt_on_death_tox.TabIndex = 46;
            this.txt_on_death_tox.Text = "5";
            this.txt_on_death_tox.TextChanged += new System.EventHandler(this.txt_on_death_tox_TextChanged);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 576);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(351, 32);
            this.label10.TabIndex = 45;
            this.label10.Text = "On death Extra [zmol/um2]";
            // 
            // txt_insult_tox
            // 
            this.txt_insult_tox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_insult_tox.Location = new System.Drawing.Point(404, 524);
            this.txt_insult_tox.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_insult_tox.Name = "txt_insult_tox";
            this.txt_insult_tox.Size = new System.Drawing.Size(126, 38);
            this.txt_insult_tox.TabIndex = 44;
            this.txt_insult_tox.Text = "0";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 525);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(229, 32);
            this.label6.TabIndex = 43;
            this.label6.Text = "Insult [zmol/um2]";
            // 
            // txt_tox_prod_rate
            // 
            this.txt_tox_prod_rate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_tox_prod_rate.Location = new System.Drawing.Point(402, 473);
            this.txt_tox_prod_rate.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_tox_prod_rate.Name = "txt_tox_prod_rate";
            this.txt_tox_prod_rate.Size = new System.Drawing.Size(126, 38);
            this.txt_tox_prod_rate.TabIndex = 42;
            this.txt_tox_prod_rate.Text = "0.003";
            // 
            // label29
            // 
            this.label29.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(31, 475);
            this.label29.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(341, 32);
            this.label29.TabIndex = 41;
            this.label29.Text = "Prod [~2/r] [zmol/um2/iter]";
            this.label29.Click += new System.EventHandler(this.label29_Click);
            // 
            // label36
            // 
            this.label36.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(24, 384);
            this.label36.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(348, 32);
            this.label36.TabIndex = 39;
            this.label36.Text = "Death Thr (DT) [zmol/um2]";
            // 
            // txt_death_tox_threshold
            // 
            this.txt_death_tox_threshold.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_death_tox_threshold.Location = new System.Drawing.Point(401, 379);
            this.txt_death_tox_threshold.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_death_tox_threshold.Name = "txt_death_tox_threshold";
            this.txt_death_tox_threshold.Size = new System.Drawing.Size(126, 38);
            this.txt_death_tox_threshold.TabIndex = 40;
            this.txt_death_tox_threshold.Text = "7";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.tableLayoutPanel2);
            this.groupBox7.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox7.Location = new System.Drawing.Point(16, 101);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox7.Size = new System.Drawing.Size(718, 262);
            this.groupBox7.TabIndex = 35;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Diffusion Rates (=value/5*(res/10)^2";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.20926F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.79074F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tableLayoutPanel2.Controls.Add(this.label26, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txt_rate_live, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_rate_bound, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txt_rate_dead, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txt_rate_extra, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label30, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label31, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label32, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label33, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label25, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(60, 41);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(616, 221);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // label26
            // 
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(190, 177);
            this.label26.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(138, 32);
            this.label26.TabIndex = 33;
            this.label26.Text = "Extra Cell";
            // 
            // txt_rate_live
            // 
            this.txt_rate_live.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_rate_live.Location = new System.Drawing.Point(340, 8);
            this.txt_rate_live.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_rate_live.Name = "txt_rate_live";
            this.txt_rate_live.Size = new System.Drawing.Size(102, 38);
            this.txt_rate_live.TabIndex = 32;
            this.txt_rate_live.Text = "0.5";
            // 
            // txt_rate_bound
            // 
            this.txt_rate_bound.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_rate_bound.Location = new System.Drawing.Point(340, 118);
            this.txt_rate_bound.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_rate_bound.Name = "txt_rate_bound";
            this.txt_rate_bound.Size = new System.Drawing.Size(102, 38);
            this.txt_rate_bound.TabIndex = 27;
            this.txt_rate_bound.Text = "0.05";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(191, 121);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 32);
            this.label8.TabIndex = 26;
            this.label8.Text = "Boundary";
            // 
            // txt_rate_dead
            // 
            this.txt_rate_dead.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_rate_dead.Location = new System.Drawing.Point(340, 63);
            this.txt_rate_dead.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_rate_dead.Name = "txt_rate_dead";
            this.txt_rate_dead.Size = new System.Drawing.Size(102, 38);
            this.txt_rate_dead.TabIndex = 21;
            this.txt_rate_dead.Text = "0.5";
            // 
            // txt_rate_extra
            // 
            this.txt_rate_extra.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_rate_extra.Location = new System.Drawing.Point(340, 174);
            this.txt_rate_extra.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_rate_extra.Name = "txt_rate_extra";
            this.txt_rate_extra.Size = new System.Drawing.Size(102, 38);
            this.txt_rate_extra.TabIndex = 34;
            this.txt_rate_extra.Text = "0.5";
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(535, 11);
            this.label30.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(75, 32);
            this.label30.TabIndex = 35;
            this.label30.Text = "0→1";
            // 
            // label31
            // 
            this.label31.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(535, 66);
            this.label31.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(75, 32);
            this.label31.TabIndex = 36;
            this.label31.Text = "0→1";
            // 
            // label32
            // 
            this.label32.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(535, 121);
            this.label32.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(75, 32);
            this.label32.TabIndex = 37;
            this.label32.Text = "0→1";
            // 
            // label33
            // 
            this.label33.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(535, 177);
            this.label33.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(75, 32);
            this.label33.TabIndex = 38;
            this.label33.Text = "0→1";
            // 
            // label25
            // 
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(188, 11);
            this.label25.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(140, 32);
            this.label25.TabIndex = 29;
            this.label25.Text = "Live Axon";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(86, 66);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(242, 32);
            this.label7.TabIndex = 20;
            this.label7.Text = "Dead Axon [1/iter]";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.tableLayoutPanel15);
            this.groupBox6.Location = new System.Drawing.Point(0, 620);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox6.Size = new System.Drawing.Size(734, 165);
            this.groupBox6.TabIndex = 32;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Detoxification Rates";
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel15.ColumnCount = 3;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.53247F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.46753F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel15.Controls.Add(this.txt_detox_extra, 1, 1);
            this.tableLayoutPanel15.Controls.Add(this.label27, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.txt_detox_intra, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.label34, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.label35, 2, 1);
            this.tableLayoutPanel15.Controls.Add(this.label28, 0, 1);
            this.tableLayoutPanel15.Location = new System.Drawing.Point(70, 45);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(622, 99);
            this.tableLayoutPanel15.TabIndex = 12;
            // 
            // txt_detox_extra
            // 
            this.txt_detox_extra.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_detox_extra.Location = new System.Drawing.Point(351, 55);
            this.txt_detox_extra.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_detox_extra.Name = "txt_detox_extra";
            this.txt_detox_extra.Size = new System.Drawing.Size(104, 38);
            this.txt_detox_extra.TabIndex = 27;
            this.txt_detox_extra.Text = "0.001";
            // 
            // label27
            // 
            this.label27.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(100, 8);
            this.label27.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(239, 32);
            this.label27.TabIndex = 20;
            this.label27.Text = "Detox Intra [1/iter]";
            // 
            // txt_detox_intra
            // 
            this.txt_detox_intra.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_detox_intra.Location = new System.Drawing.Point(351, 5);
            this.txt_detox_intra.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_detox_intra.Name = "txt_detox_intra";
            this.txt_detox_intra.Size = new System.Drawing.Size(104, 38);
            this.txt_detox_intra.TabIndex = 21;
            this.txt_detox_intra.Text = "0.01";
            // 
            // label34
            // 
            this.label34.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(541, 8);
            this.label34.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(75, 32);
            this.label34.TabIndex = 36;
            this.label34.Text = "0→1";
            // 
            // label35
            // 
            this.label35.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(541, 58);
            this.label35.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(75, 32);
            this.label35.TabIndex = 37;
            this.label35.Text = "0→1";
            // 
            // label28
            // 
            this.label28.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(90, 58);
            this.label28.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(249, 32);
            this.label28.TabIndex = 26;
            this.label28.Text = "Detox Extra [1/iter]";
            // 
            // txt_resolution
            // 
            this.txt_resolution.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_resolution.Location = new System.Drawing.Point(270, 43);
            this.txt_resolution.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_resolution.Name = "txt_resolution";
            this.txt_resolution.Size = new System.Drawing.Size(132, 38);
            this.txt_resolution.TabIndex = 34;
            this.txt_resolution.Text = "5";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(86, 48);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(174, 32);
            this.label9.TabIndex = 33;
            this.label9.Text = "Res (pix/um)";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Maroon;
            this.label12.Location = new System.Drawing.Point(426, 48);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(164, 32);
            this.label12.TabIndex = 31;
            this.label12.Text = "Image Size:";
            // 
            // lbl_image_siz
            // 
            this.lbl_image_siz.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_image_siz.AutoSize = true;
            this.lbl_image_siz.ForeColor = System.Drawing.Color.Maroon;
            this.lbl_image_siz.Location = new System.Drawing.Point(602, 48);
            this.lbl_image_siz.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_image_siz.Name = "lbl_image_siz";
            this.lbl_image_siz.Size = new System.Drawing.Size(24, 32);
            this.lbl_image_siz.TabIndex = 30;
            this.lbl_image_siz.Text = "-";
            // 
            // btn_preprocess
            // 
            this.btn_preprocess.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_preprocess.Location = new System.Drawing.Point(512, 806);
            this.btn_preprocess.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_preprocess.Name = "btn_preprocess";
            this.btn_preprocess.Size = new System.Drawing.Size(178, 70);
            this.btn_preprocess.TabIndex = 29;
            this.btn_preprocess.Text = "Preprocess";
            this.btn_preprocess.UseVisualStyleBackColor = true;
            // 
            // btn_load_setts
            // 
            this.btn_load_setts.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_load_setts.Location = new System.Drawing.Point(290, 806);
            this.btn_load_setts.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_load_setts.Name = "btn_load_setts";
            this.btn_load_setts.Size = new System.Drawing.Size(210, 70);
            this.btn_load_setts.TabIndex = 29;
            this.btn_load_setts.Text = "Load Settings";
            this.btn_load_setts.UseVisualStyleBackColor = true;
            // 
            // btn_save_setts
            // 
            this.btn_save_setts.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_save_setts.Location = new System.Drawing.Point(70, 806);
            this.btn_save_setts.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_save_setts.Name = "btn_save_setts";
            this.btn_save_setts.Size = new System.Drawing.Size(210, 70);
            this.btn_save_setts.TabIndex = 20;
            this.btn_save_setts.Text = "Save Settings";
            this.btn_save_setts.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Location = new System.Drawing.Point(32, 23);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox3.Size = new System.Drawing.Size(810, 185);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Simulation";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.47208F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.72589F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.80203F));
            this.tableLayoutPanel4.Controls.Add(this.btn_save_prog, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_start, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_reset, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(24, 48);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(790, 70);
            this.tableLayoutPanel4.TabIndex = 24;
            // 
            // btn_save_prog
            // 
            this.btn_save_prog.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_save_prog.Location = new System.Drawing.Point(520, 9);
            this.btn_save_prog.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_save_prog.Name = "btn_save_prog";
            this.btn_save_prog.Size = new System.Drawing.Size(248, 52);
            this.btn_save_prog.TabIndex = 43;
            this.btn_save_prog.Text = "Save Progress";
            this.btn_save_prog.UseVisualStyleBackColor = true;
            // 
            // btn_sweep
            // 
            this.btn_sweep.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_sweep.Location = new System.Drawing.Point(25, 8);
            this.btn_sweep.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_sweep.Name = "btn_sweep";
            this.btn_sweep.Size = new System.Drawing.Size(216, 52);
            this.btn_sweep.TabIndex = 24;
            this.btn_sweep.Text = "S&weep";
            this.btn_sweep.UseVisualStyleBackColor = true;
            this.btn_sweep.Click += new System.EventHandler(this.Btn_sweep_Click);
            // 
            // cmb_sw_sel1
            // 
            this.cmb_sw_sel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmb_sw_sel1.FormattingEnabled = true;
            this.cmb_sw_sel1.Location = new System.Drawing.Point(369, 14);
            this.cmb_sw_sel1.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cmb_sw_sel1.Name = "cmb_sw_sel1";
            this.cmb_sw_sel1.Size = new System.Drawing.Size(154, 39);
            this.cmb_sw_sel1.TabIndex = 34;
            // 
            // chk_save_sw_prog
            // 
            this.chk_save_sw_prog.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chk_save_sw_prog.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_save_sw_prog.AutoSize = true;
            this.chk_save_sw_prog.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chk_save_sw_prog.Location = new System.Drawing.Point(41, 81);
            this.chk_save_sw_prog.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_save_sw_prog.Name = "chk_save_sw_prog";
            this.chk_save_sw_prog.Size = new System.Drawing.Size(184, 42);
            this.chk_save_sw_prog.TabIndex = 37;
            this.chk_save_sw_prog.Text = "Save Sweep";
            this.chk_save_sw_prog.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label40);
            this.groupBox4.Controls.Add(this.txt_layer_to_display);
            this.groupBox4.Controls.Add(this.txt_rec_inerval);
            this.groupBox4.Controls.Add(this.chk_show_axons);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.chk_show_tox);
            this.groupBox4.Controls.Add(this.chk_rec_avi);
            this.groupBox4.Location = new System.Drawing.Point(856, 23);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox4.Size = new System.Drawing.Size(618, 214);
            this.groupBox4.TabIndex = 28;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Display";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(428, 171);
            this.label40.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(153, 32);
            this.label40.TabIndex = 49;
            this.label40.Text = "Layer SOX";
            this.label40.Click += new System.EventHandler(this.label40_Click);
            // 
            // txt_layer_to_display
            // 
            this.txt_layer_to_display.Location = new System.Drawing.Point(346, 166);
            this.txt_layer_to_display.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_layer_to_display.Name = "txt_layer_to_display";
            this.txt_layer_to_display.Size = new System.Drawing.Size(73, 38);
            this.txt_layer_to_display.TabIndex = 50;
            this.txt_layer_to_display.Text = "0";
            this.txt_layer_to_display.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_rec_inerval
            // 
            this.txt_rec_inerval.Location = new System.Drawing.Point(200, 118);
            this.txt_rec_inerval.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_rec_inerval.Name = "txt_rec_inerval";
            this.txt_rec_inerval.Size = new System.Drawing.Size(100, 38);
            this.txt_rec_inerval.TabIndex = 45;
            this.txt_rec_inerval.Text = "200";
            // 
            // chk_show_axons
            // 
            this.chk_show_axons.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chk_show_axons.AutoSize = true;
            this.chk_show_axons.Checked = true;
            this.chk_show_axons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_show_axons.Location = new System.Drawing.Point(392, 49);
            this.chk_show_axons.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_show_axons.Name = "chk_show_axons";
            this.chk_show_axons.Size = new System.Drawing.Size(210, 36);
            this.chk_show_axons.TabIndex = 26;
            this.chk_show_axons.Text = "Show Axons";
            this.chk_show_axons.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(44, 122);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(145, 32);
            this.label17.TabIndex = 44;
            this.label17.Text = "Period [itr]";
            // 
            // chk_show_tox
            // 
            this.chk_show_tox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chk_show_tox.AutoSize = true;
            this.chk_show_tox.Checked = true;
            this.chk_show_tox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_show_tox.Location = new System.Drawing.Point(392, 113);
            this.chk_show_tox.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_show_tox.Name = "chk_show_tox";
            this.chk_show_tox.Size = new System.Drawing.Size(191, 36);
            this.chk_show_tox.TabIndex = 27;
            this.chk_show_tox.Text = "Show SOX";
            this.chk_show_tox.UseVisualStyleBackColor = true;
            // 
            // chk_rec_avi
            // 
            this.chk_rec_avi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_rec_avi.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_rec_avi.AutoSize = true;
            this.chk_rec_avi.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chk_rec_avi.Location = new System.Drawing.Point(60, 60);
            this.chk_rec_avi.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.chk_rec_avi.Name = "chk_rec_avi";
            this.chk_rec_avi.Size = new System.Drawing.Size(116, 42);
            this.chk_rec_avi.TabIndex = 25;
            this.chk_rec_avi.Text = "Record";
            this.chk_rec_avi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chk_rec_avi.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel9.ColumnCount = 4;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.88488F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.27184F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.9362F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.18447F));
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel14, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel8, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel7, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel12, 3, 0);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel6, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel13, 3, 1);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(32, 1543);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1442, 95);
            this.tableLayoutPanel9.TabIndex = 29;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.16667F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.83333F));
            this.tableLayoutPanel14.Controls.Add(this.lbl_sim_time, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel14.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(335, 51);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(337, 35);
            this.tableLayoutPanel14.TabIndex = 40;
            // 
            // lbl_sim_time
            // 
            this.lbl_sim_time.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_sim_time.AutoSize = true;
            this.lbl_sim_time.Location = new System.Drawing.Point(188, 1);
            this.lbl_sim_time.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_sim_time.Name = "lbl_sim_time";
            this.lbl_sim_time.Size = new System.Drawing.Size(31, 32);
            this.lbl_sim_time.TabIndex = 3;
            this.lbl_sim_time.Text = "0";
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(42, 1);
            this.label19.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 32);
            this.label19.TabIndex = 7;
            this.label19.Text = "Sim Time";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.16667F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.83333F));
            this.tableLayoutPanel8.Controls.Add(this.lbl_alive_axons_perc, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label22, 0, 0);
            this.tableLayoutPanel8.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(335, 4);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(337, 35);
            this.tableLayoutPanel8.TabIndex = 36;
            // 
            // lbl_alive_axons_perc
            // 
            this.lbl_alive_axons_perc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_alive_axons_perc.AutoSize = true;
            this.lbl_alive_axons_perc.Location = new System.Drawing.Point(188, 1);
            this.lbl_alive_axons_perc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_alive_axons_perc.Name = "lbl_alive_axons_perc";
            this.lbl_alive_axons_perc.Size = new System.Drawing.Size(88, 32);
            this.lbl_alive_axons_perc.TabIndex = 1;
            this.lbl_alive_axons_perc.Text = "100%";
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(164, 35);
            this.label22.TabIndex = 8;
            this.label22.Text = "Alive Axons (%):";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.61765F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.38235F));
            this.tableLayoutPanel10.Controls.Add(this.lbl_itr, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel10.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(6, 4);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(317, 35);
            this.tableLayoutPanel10.TabIndex = 35;
            // 
            // lbl_itr
            // 
            this.lbl_itr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_itr.AutoSize = true;
            this.lbl_itr.Location = new System.Drawing.Point(185, 1);
            this.lbl_itr.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_itr.Name = "lbl_itr";
            this.lbl_itr.Size = new System.Drawing.Size(31, 32);
            this.lbl_itr.TabIndex = 3;
            this.lbl_itr.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Iteration:";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.label37, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lbl_max_tox, 1, 0);
            this.tableLayoutPanel5.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(684, 4);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(360, 35);
            this.tableLayoutPanel5.TabIndex = 32;
            // 
            // label37
            // 
            this.label37.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(15, 1);
            this.label37.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(159, 32);
            this.label37.TabIndex = 49;
            this.label37.Text = "H Max Sox:";
            // 
            // lbl_max_tox
            // 
            this.lbl_max_tox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_max_tox.AutoSize = true;
            this.lbl_max_tox.Location = new System.Drawing.Point(186, 1);
            this.lbl_max_tox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_max_tox.Name = "lbl_max_tox";
            this.lbl_max_tox.Size = new System.Drawing.Size(100, 32);
            this.lbl_max_tox.TabIndex = 43;
            this.lbl_max_tox.Text = "0 aMol";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.61765F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.38235F));
            this.tableLayoutPanel7.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.lbl_itr_s, 1, 0);
            this.tableLayoutPanel7.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(6, 51);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(317, 35);
            this.tableLayoutPanel7.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 32);
            this.label3.TabIndex = 16;
            this.label3.Text = "iterations/s:";
            // 
            // lbl_itr_s
            // 
            this.lbl_itr_s.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_itr_s.AutoSize = true;
            this.lbl_itr_s.Location = new System.Drawing.Point(185, 1);
            this.lbl_itr_s.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_itr_s.Name = "lbl_itr_s";
            this.lbl_itr_s.Size = new System.Drawing.Size(31, 32);
            this.lbl_itr_s.TabIndex = 15;
            this.lbl_itr_s.Text = "0";
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.50237F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.49763F));
            this.tableLayoutPanel12.Controls.Add(this.lbl_max_density, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel12.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(1056, 4);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(380, 35);
            this.tableLayoutPanel12.TabIndex = 38;
            // 
            // lbl_max_density
            // 
            this.lbl_max_density.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_max_density.AutoSize = true;
            this.lbl_max_density.Location = new System.Drawing.Point(213, 1);
            this.lbl_max_density.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_max_density.Name = "lbl_max_density";
            this.lbl_max_density.Size = new System.Drawing.Size(101, 32);
            this.lbl_max_density.TabIndex = 15;
            this.lbl_max_density.Text = "0 g/m3";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(43, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(158, 35);
            this.label14.TabIndex = 16;
            this.label14.Text = "H Max Sox Dens";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.lbl_tox, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel6.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(684, 51);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(360, 35);
            this.tableLayoutPanel6.TabIndex = 34;
            // 
            // lbl_tox
            // 
            this.lbl_tox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_tox.AutoSize = true;
            this.lbl_tox.Location = new System.Drawing.Point(186, 1);
            this.lbl_tox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_tox.Name = "lbl_tox";
            this.lbl_tox.Size = new System.Drawing.Size(100, 32);
            this.lbl_tox.TabIndex = 1;
            this.lbl_tox.Text = "0 aMol";
            this.lbl_tox.Click += new System.EventHandler(this.lbl_tox_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 32);
            this.label2.TabIndex = 8;
            this.label2.Text = "H Sum Sox:";
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.9763F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.0237F));
            this.tableLayoutPanel13.Controls.Add(this.lbl_density, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel13.ForeColor = System.Drawing.Color.Maroon;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(1056, 51);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(380, 35);
            this.tableLayoutPanel13.TabIndex = 39;
            this.tableLayoutPanel13.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel13_Paint);
            // 
            // lbl_density
            // 
            this.lbl_density.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_density.AutoSize = true;
            this.lbl_density.Location = new System.Drawing.Point(214, 1);
            this.lbl_density.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_density.Name = "lbl_density";
            this.lbl_density.Size = new System.Drawing.Size(101, 32);
            this.lbl_density.TabIndex = 3;
            this.lbl_density.Text = "0 g/m3";
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 1);
            this.label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(193, 32);
            this.label18.TabIndex = 7;
            this.label18.Text = "H Sox Density";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statlbl,
            this.toolStripStatusLabel3,
            this.statlbl_sweep,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1637);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 26, 0);
            this.statusStrip1.Size = new System.Drawing.Size(2274, 54);
            this.statusStrip1.TabIndex = 30;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statlbl
            // 
            this.statlbl.Name = "statlbl";
            this.statlbl.Size = new System.Drawing.Size(146, 41);
            this.statlbl.Text = "                ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(234, 41);
            this.toolStripStatusLabel3.Text = "                           ";
            // 
            // statlbl_sweep
            // 
            this.statlbl_sweep.Name = "statlbl_sweep";
            this.statlbl_sweep.Size = new System.Drawing.Size(210, 41);
            this.statlbl_sweep.Text = "                        ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1414, 41);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(138, 41);
            this.toolStripStatusLabel2.Text = "               ";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 38);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tableLayoutPanel11);
            this.groupBox5.Location = new System.Drawing.Point(32, 216);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox5.Size = new System.Drawing.Size(810, 203);
            this.groupBox5.TabIndex = 33;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Parameter Sweep";
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.32487F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.61929F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.80203F));
            this.tableLayoutPanel11.Controls.Add(this.label21, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.label20, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.btn_sweep, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.cmb_sw_sel1, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.txt_sw_range1, 3, 0);
            this.tableLayoutPanel11.Controls.Add(this.chk_save_sw_prog, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.cmb_sw_sel2, 2, 1);
            this.tableLayoutPanel11.Controls.Add(this.txt_sw_range2, 3, 1);
            this.tableLayoutPanel11.Location = new System.Drawing.Point(24, 48);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(790, 136);
            this.tableLayoutPanel11.TabIndex = 24;
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(282, 86);
            this.label21.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(63, 32);
            this.label21.TabIndex = 40;
            this.label21.Text = "2nd";
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(287, 18);
            this.label20.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 32);
            this.label20.TabIndex = 37;
            this.label20.Text = "1st";
            // 
            // txt_sw_range1
            // 
            this.txt_sw_range1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_sw_range1.Cue = null;
            this.txt_sw_range1.Location = new System.Drawing.Point(552, 15);
            this.txt_sw_range1.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_sw_range1.Name = "txt_sw_range1";
            this.txt_sw_range1.Size = new System.Drawing.Size(218, 38);
            this.txt_sw_range1.TabIndex = 24;
            // 
            // cmb_sw_sel2
            // 
            this.cmb_sw_sel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmb_sw_sel2.FormattingEnabled = true;
            this.cmb_sw_sel2.Location = new System.Drawing.Point(369, 82);
            this.cmb_sw_sel2.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.cmb_sw_sel2.Name = "cmb_sw_sel2";
            this.cmb_sw_sel2.Size = new System.Drawing.Size(154, 39);
            this.cmb_sw_sel2.TabIndex = 38;
            // 
            // txt_sw_range2
            // 
            this.txt_sw_range2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_sw_range2.Cue = null;
            this.txt_sw_range2.Location = new System.Drawing.Point(552, 83);
            this.txt_sw_range2.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_sw_range2.Name = "txt_sw_range2";
            this.txt_sw_range2.Size = new System.Drawing.Size(218, 38);
            this.txt_sw_range2.TabIndex = 39;
            // 
            // txt_delay_ms
            // 
            this.txt_delay_ms.Location = new System.Drawing.Point(1056, 235);
            this.txt_delay_ms.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_delay_ms.Name = "txt_delay_ms";
            this.txt_delay_ms.Size = new System.Drawing.Size(100, 38);
            this.txt_delay_ms.TabIndex = 35;
            this.txt_delay_ms.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(898, 236);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(143, 32);
            this.label13.TabIndex = 34;
            this.label13.Text = "Delay(ms)";
            // 
            // btn_snapshot
            // 
            this.btn_snapshot.Location = new System.Drawing.Point(1254, 245);
            this.btn_snapshot.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_snapshot.Name = "btn_snapshot";
            this.btn_snapshot.Size = new System.Drawing.Size(210, 52);
            this.btn_snapshot.TabIndex = 36;
            this.btn_snapshot.Text = "Snapshot";
            this.btn_snapshot.UseVisualStyleBackColor = true;
            // 
            // txt_stop_itr
            // 
            this.txt_stop_itr.Location = new System.Drawing.Point(1056, 343);
            this.txt_stop_itr.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_stop_itr.Name = "txt_stop_itr";
            this.txt_stop_itr.Size = new System.Drawing.Size(100, 38);
            this.txt_stop_itr.TabIndex = 38;
            this.txt_stop_itr.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(908, 347);
            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(133, 32);
            this.label16.TabIndex = 37;
            this.label16.Text = "Stop@ Itr";
            // 
            // txt_stop_time
            // 
            this.txt_stop_time.Location = new System.Drawing.Point(1356, 346);
            this.txt_stop_time.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_stop_time.Name = "txt_stop_time";
            this.txt_stop_time.Size = new System.Drawing.Size(100, 38);
            this.txt_stop_time.TabIndex = 41;
            this.txt_stop_time.Text = "0";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(902, 296);
            this.label24.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(143, 32);
            this.label24.TabIndex = 39;
            this.label24.Text = "Block size";
            // 
            // txt_block_siz
            // 
            this.txt_block_siz.Location = new System.Drawing.Point(1056, 292);
            this.txt_block_siz.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_block_siz.Name = "txt_block_siz";
            this.txt_block_siz.Size = new System.Drawing.Size(100, 38);
            this.txt_block_siz.TabIndex = 40;
            this.txt_block_siz.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1252, 347);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 32);
            this.label5.TabIndex = 42;
            this.label5.Text = "TEST";
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.btn_clr);
            this.groupBox8.Controls.Add(this.txt_status);
            this.groupBox8.Location = new System.Drawing.Point(1502, 1254);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.groupBox8.Size = new System.Drawing.Size(730, 375);
            this.groupBox8.TabIndex = 38;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Output";
            // 
            // btn_clr
            // 
            this.btn_clr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_clr.Location = new System.Drawing.Point(630, 37);
            this.btn_clr.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.btn_clr.Name = "btn_clr";
            this.btn_clr.Size = new System.Drawing.Size(86, 52);
            this.btn_clr.TabIndex = 34;
            this.btn_clr.Text = "clr";
            this.btn_clr.UseVisualStyleBackColor = true;
            // 
            // txt_status
            // 
            this.txt_status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_status.Location = new System.Drawing.Point(12, 37);
            this.txt_status.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_status.Multiline = true;
            this.txt_status.Name = "txt_status";
            this.txt_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_status.Size = new System.Drawing.Size(698, 330);
            this.txt_status.TabIndex = 33;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(38, 165);
            this.label23.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(143, 32);
            this.label23.TabIndex = 43;
            this.label23.Text = "3D Layers";
            this.label23.Click += new System.EventHandler(this.label23_Click);
            // 
            // txt_3d_layers
            // 
            this.txt_3d_layers.Location = new System.Drawing.Point(179, 162);
            this.txt_3d_layers.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_3d_layers.Name = "txt_3d_layers";
            this.txt_3d_layers.Size = new System.Drawing.Size(100, 38);
            this.txt_3d_layers.TabIndex = 44;
            this.txt_3d_layers.Text = "0";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(300, 168);
            this.label38.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(185, 32);
            this.label38.TabIndex = 45;
            this.label38.Text = "3D SOX Start";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(587, 171);
            this.label39.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(176, 32);
            this.label39.TabIndex = 46;
            this.label39.Text = "3D SOX End";
            // 
            // txt_3d_tox_start
            // 
            this.txt_3d_tox_start.Location = new System.Drawing.Point(478, 165);
            this.txt_3d_tox_start.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_3d_tox_start.Name = "txt_3d_tox_start";
            this.txt_3d_tox_start.Size = new System.Drawing.Size(100, 38);
            this.txt_3d_tox_start.TabIndex = 47;
            this.txt_3d_tox_start.Text = "0";
            // 
            // txt_3d_tox_stop
            // 
            this.txt_3d_tox_stop.Location = new System.Drawing.Point(763, 166);
            this.txt_3d_tox_stop.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.txt_3d_tox_stop.Name = "txt_3d_tox_stop";
            this.txt_3d_tox_stop.Size = new System.Drawing.Size(100, 38);
            this.txt_3d_tox_stop.TabIndex = 48;
            this.txt_3d_tox_stop.Text = "0";
            // 
            // picB
            // 
            this.picB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picB.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.picB.Location = new System.Drawing.Point(46, 440);
            this.picB.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.picB.Name = "picB";
            this.picB.Size = new System.Drawing.Size(1418, 1075);
            this.picB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picB.TabIndex = 0;
            this.picB.TabStop = false;
            this.picB.Resize += new System.EventHandler(this.PicB_Resize);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2274, 1691);
            this.Controls.Add(this.txt_3d_tox_stop);
            this.Controls.Add(this.txt_3d_tox_start);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.txt_3d_layers);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_stop_time);
            this.Controls.Add(this.txt_block_siz);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.txt_stop_itr);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btn_snapshot);
            this.Controls.Add(this.txt_delay_ms);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel9);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.chk_var_death);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.Name = "Main_Form";
            this.Text = "LHON";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Form_FormClosing);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.chk_var_death.ResumeLayout(false);
            this.chk_var_death.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel13.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private PictureBoxWithInterpolationMode picB;
        private CueTextBox txt_sw_range1;
        private CueTextBox txt_sw_range2;

        private System.Windows.Forms.Button btn_start;
        private Button btn_reset;
        private Button btn_save_model;
        private Button btn_load_model;
        private Button btn_redraw;
        private Label label4;
        private Label lbl_num_axons;
        private GroupBox groupBox1;
        private GroupBox chk_var_death;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox groupBox4;
        private Button btn_load_setts;
        private Button btn_save_setts;
        private Button btn_preprocess;
        private TableLayoutPanel tableLayoutPanel9;
        private TableLayoutPanel tableLayoutPanel7;
        private Label label3;
        private Label lbl_itr_s;
        private Button btn_sweep;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statlbl;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel statlbl_sweep;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ComboBox cmb_sw_sel1;
        private Label label12;
        private Label lbl_image_siz;
        private CheckBox chk_save_sw_prog;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel11;
        private CheckBox chk_strict_rad;
        private TextBox txt_delay_ms;
        private Label label13;
        private TableLayoutPanel tableLayoutPanel8;
        private TableLayoutPanel tableLayoutPanel10;
        private Label label14;
        private Label lbl_max_density;
        private Label label1;
        private Label lbl_itr;
        private TableLayoutPanel tableLayoutPanel12;
        private Label lbl_density;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label2;
        private Label lbl_tox;
        private TableLayoutPanel tableLayoutPanel13;
        private Label label19;
        private Label lbl_sim_time;
        private Button btn_snapshot;
        private Label label21;
        private Label label20;
        private ComboBox cmb_sw_sel2;
        private TableLayoutPanel tableLayoutPanel14;
        private TextBox txt_stop_itr;
        private Label label16;
        private GroupBox groupBox6;
        private TableLayoutPanel tableLayoutPanel15;
        private TextBox txt_detox_extra;
        private Label label27;
        private Label label28;
        private TextBox txt_detox_intra;
        private TextBox txt_resolution;
        private Label label9;
        private GroupBox groupBox7;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txt_rate_live;
        private TextBox txt_rate_bound;
        private Label label7;
        private Label label8;
        private TextBox txt_rate_dead;
        private Label label25;
        private Label label26;
        private TextBox txt_rate_extra;
        private Label label30;
        private Label label31;
        private Label label32;
        private Label label33;
        private Label label34;
        private Label label35;
        private Label label36;
        private TextBox txt_death_tox_threshold;
        private TextBox txt_tox_prod_rate;
        private Label label29;
        private TextBox txt_stop_time;
        private Label label24;
        private TextBox txt_block_siz;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel5;
        private Label label22;
        private Label lbl_alive_axons_perc;
        private Label label18;
        private Label lbl_nerve_siz;
        private TextBox txt_insult_tox;
        private Label label6;
        private GroupBox groupBox8;
        private Button btn_clr;
        private TextBox txt_status;
        private TextBox txt_nerve_scale;
        private Label label11;
        private Button btn_save_prog;
        private TextBox txt_on_death_tox;
        private Label label10;
        private Label label15;
        private TextBox txt_clearance;
        private TextBox txt_rec_inerval;
        private CheckBox chk_show_axons;
        private Label label17;
        private CheckBox chk_show_tox;
        private CheckBox chk_rec_avi;
        private Label lbl_max_tox;
        private ToolStripProgressBar toolStripProgressBar1;
        private Label label23;
        private TextBox txt_3d_layers;
        private Label label38;
        private Label label39;
        private TextBox txt_3d_tox_start;
        private TextBox txt_3d_tox_stop;
        private Label label40;
        private TextBox txt_layer_to_display;
        private CheckBox chk_var_thr;
        private Label label41;
        private TextBox txt_var_death;
        private Label label37;
    }
}

