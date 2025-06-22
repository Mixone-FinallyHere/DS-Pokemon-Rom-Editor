
namespace DSPRE.Resources {
    partial class CommandsDatabase {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.scriptcmdDataGridView = new System.Windows.Forms.DataGridView();
            this.CommandID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Params = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scriptcmdSearchTextBox = new System.Windows.Forms.TextBox();
            this.startSearchButtonScripts = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.criteriaGroupBoxScripts = new System.Windows.Forms.GroupBox();
            this.matchCBScripts = new System.Windows.Forms.RadioButton();
            this.containsCBScripts = new System.Windows.Forms.RadioButton();
            this.startsWithCBScripts = new System.Windows.Forms.RadioButton();
            this.criteriaGroupBoxActions = new System.Windows.Forms.GroupBox();
            this.matchCBActions = new System.Windows.Forms.RadioButton();
            this.containsCBActions = new System.Windows.Forms.RadioButton();
            this.startsWithCBActions = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.startSearchButtonActions = new System.Windows.Forms.Button();
            this.actioncmdSearchTextBox = new System.Windows.Forms.TextBox();
            this.actionDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.compOPDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exportScrcmdButton = new System.Windows.Forms.Button();
            this.manageScrcmdsButton = new System.Windows.Forms.Button();
            this.customScrcmdSelector = new DSPRE.InputComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.useGameDefaultScrcmdButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scriptcmdDataGridView)).BeginInit();
            this.criteriaGroupBoxScripts.SuspendLayout();
            this.criteriaGroupBoxActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.compOPDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptcmdDataGridView
            // 
            this.scriptcmdDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.scriptcmdDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.scriptcmdDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scriptcmdDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CommandID,
            this.CommandName,
            this.ParamsCount,
            this.Params,
            this.Description});
            this.scriptcmdDataGridView.Location = new System.Drawing.Point(13, 66);
            this.scriptcmdDataGridView.Name = "scriptcmdDataGridView";
            this.scriptcmdDataGridView.ReadOnly = true;
            this.scriptcmdDataGridView.RowHeadersVisible = false;
            this.scriptcmdDataGridView.Size = new System.Drawing.Size(707, 621);
            this.scriptcmdDataGridView.TabIndex = 0;
            // 
            // CommandID
            // 
            this.CommandID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Format = "X4";
            this.CommandID.DefaultCellStyle = dataGridViewCellStyle1;
            this.CommandID.FillWeight = 15F;
            this.CommandID.HeaderText = "Command ID";
            this.CommandID.MaxInputLength = 10;
            this.CommandID.Name = "CommandID";
            this.CommandID.ReadOnly = true;
            // 
            // CommandName
            // 
            this.CommandName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CommandName.FillWeight = 35F;
            this.CommandName.HeaderText = "Script Command Name";
            this.CommandName.MaxInputLength = 200;
            this.CommandName.MinimumWidth = 90;
            this.CommandName.Name = "CommandName";
            this.CommandName.ReadOnly = true;
            // 
            // ParamsCount
            // 
            this.ParamsCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ParamsCount.FillWeight = 20F;
            this.ParamsCount.HeaderText = "Parameter Count";
            this.ParamsCount.MaxInputLength = 10;
            this.ParamsCount.MinimumWidth = 20;
            this.ParamsCount.Name = "ParamsCount";
            this.ParamsCount.ReadOnly = true;
            // 
            // Params
            // 
            this.Params.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Params.FillWeight = 30F;
            this.Params.HeaderText = "Parameters";
            this.Params.MaxInputLength = 200;
            this.Params.MinimumWidth = 85;
            this.Params.Name = "Params";
            this.Params.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 85;
            // 
            // scriptcmdSearchTextBox
            // 
            this.scriptcmdSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scriptcmdSearchTextBox.Location = new System.Drawing.Point(13, 28);
            this.scriptcmdSearchTextBox.Name = "scriptcmdSearchTextBox";
            this.scriptcmdSearchTextBox.Size = new System.Drawing.Size(141, 22);
            this.scriptcmdSearchTextBox.TabIndex = 1;
            this.scriptcmdSearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scriptcmdSearchTextBox_KeyDown);
            // 
            // startSearchButtonScripts
            // 
            this.startSearchButtonScripts.Image = global::DSPRE.Properties.Resources.wideLensImage;
            this.startSearchButtonScripts.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startSearchButtonScripts.Location = new System.Drawing.Point(432, 17);
            this.startSearchButtonScripts.Name = "startSearchButtonScripts";
            this.startSearchButtonScripts.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.startSearchButtonScripts.Size = new System.Drawing.Size(87, 43);
            this.startSearchButtonScripts.TabIndex = 17;
            this.startSearchButtonScripts.Text = "Start\r\nSearch";
            this.startSearchButtonScripts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.startSearchButtonScripts.UseVisualStyleBackColor = true;
            this.startSearchButtonScripts.Click += new System.EventHandler(this.startSearchButtonScripts_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Command name or ID:";
            // 
            // criteriaGroupBoxScripts
            // 
            this.criteriaGroupBoxScripts.Controls.Add(this.matchCBScripts);
            this.criteriaGroupBoxScripts.Controls.Add(this.containsCBScripts);
            this.criteriaGroupBoxScripts.Controls.Add(this.startsWithCBScripts);
            this.criteriaGroupBoxScripts.Location = new System.Drawing.Point(160, 12);
            this.criteriaGroupBoxScripts.Name = "criteriaGroupBoxScripts";
            this.criteriaGroupBoxScripts.Size = new System.Drawing.Size(266, 46);
            this.criteriaGroupBoxScripts.TabIndex = 19;
            this.criteriaGroupBoxScripts.TabStop = false;
            this.criteriaGroupBoxScripts.Text = "Search Criteria";
            // 
            // matchCBScripts
            // 
            this.matchCBScripts.Appearance = System.Windows.Forms.Appearance.Button;
            this.matchCBScripts.AutoSize = true;
            this.matchCBScripts.Location = new System.Drawing.Point(144, 15);
            this.matchCBScripts.Name = "matchCBScripts";
            this.matchCBScripts.Size = new System.Drawing.Size(113, 23);
            this.matchCBScripts.TabIndex = 2;
            this.matchCBScripts.Text = "Match (Ignore Case)";
            this.matchCBScripts.UseVisualStyleBackColor = true;
            // 
            // containsCBScripts
            // 
            this.containsCBScripts.Appearance = System.Windows.Forms.Appearance.Button;
            this.containsCBScripts.AutoSize = true;
            this.containsCBScripts.Checked = true;
            this.containsCBScripts.Location = new System.Drawing.Point(8, 15);
            this.containsCBScripts.Name = "containsCBScripts";
            this.containsCBScripts.Size = new System.Drawing.Size(58, 23);
            this.containsCBScripts.TabIndex = 1;
            this.containsCBScripts.TabStop = true;
            this.containsCBScripts.Text = "Contains";
            this.containsCBScripts.UseVisualStyleBackColor = true;
            // 
            // startsWithCBScripts
            // 
            this.startsWithCBScripts.Appearance = System.Windows.Forms.Appearance.Button;
            this.startsWithCBScripts.AutoSize = true;
            this.startsWithCBScripts.Location = new System.Drawing.Point(72, 15);
            this.startsWithCBScripts.Name = "startsWithCBScripts";
            this.startsWithCBScripts.Size = new System.Drawing.Size(66, 23);
            this.startsWithCBScripts.TabIndex = 0;
            this.startsWithCBScripts.Text = "Starts with";
            this.startsWithCBScripts.UseVisualStyleBackColor = true;
            // 
            // criteriaGroupBoxActions
            // 
            this.criteriaGroupBoxActions.Controls.Add(this.matchCBActions);
            this.criteriaGroupBoxActions.Controls.Add(this.containsCBActions);
            this.criteriaGroupBoxActions.Controls.Add(this.startsWithCBActions);
            this.criteriaGroupBoxActions.Location = new System.Drawing.Point(888, 12);
            this.criteriaGroupBoxActions.Name = "criteriaGroupBoxActions";
            this.criteriaGroupBoxActions.Size = new System.Drawing.Size(266, 46);
            this.criteriaGroupBoxActions.TabIndex = 24;
            this.criteriaGroupBoxActions.TabStop = false;
            this.criteriaGroupBoxActions.Text = "Search Criteria";
            // 
            // matchCBActions
            // 
            this.matchCBActions.Appearance = System.Windows.Forms.Appearance.Button;
            this.matchCBActions.AutoSize = true;
            this.matchCBActions.Location = new System.Drawing.Point(144, 15);
            this.matchCBActions.Name = "matchCBActions";
            this.matchCBActions.Size = new System.Drawing.Size(113, 23);
            this.matchCBActions.TabIndex = 2;
            this.matchCBActions.Text = "Match (Ignore Case)";
            this.matchCBActions.UseVisualStyleBackColor = true;
            // 
            // containsCBActions
            // 
            this.containsCBActions.Appearance = System.Windows.Forms.Appearance.Button;
            this.containsCBActions.AutoSize = true;
            this.containsCBActions.Checked = true;
            this.containsCBActions.Location = new System.Drawing.Point(8, 15);
            this.containsCBActions.Name = "containsCBActions";
            this.containsCBActions.Size = new System.Drawing.Size(58, 23);
            this.containsCBActions.TabIndex = 1;
            this.containsCBActions.TabStop = true;
            this.containsCBActions.Text = "Contains";
            this.containsCBActions.UseVisualStyleBackColor = true;
            // 
            // startsWithCBActions
            // 
            this.startsWithCBActions.Appearance = System.Windows.Forms.Appearance.Button;
            this.startsWithCBActions.AutoSize = true;
            this.startsWithCBActions.Location = new System.Drawing.Point(72, 15);
            this.startsWithCBActions.Name = "startsWithCBActions";
            this.startsWithCBActions.Size = new System.Drawing.Size(66, 23);
            this.startsWithCBActions.TabIndex = 0;
            this.startsWithCBActions.Text = "Starts with";
            this.startsWithCBActions.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(739, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Action name or ID:";
            // 
            // startSearchButtonActions
            // 
            this.startSearchButtonActions.Image = global::DSPRE.Properties.Resources.wideLensImage;
            this.startSearchButtonActions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startSearchButtonActions.Location = new System.Drawing.Point(1160, 17);
            this.startSearchButtonActions.Name = "startSearchButtonActions";
            this.startSearchButtonActions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.startSearchButtonActions.Size = new System.Drawing.Size(87, 43);
            this.startSearchButtonActions.TabIndex = 22;
            this.startSearchButtonActions.Text = "Start\r\nSearch";
            this.startSearchButtonActions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.startSearchButtonActions.UseVisualStyleBackColor = true;
            this.startSearchButtonActions.Click += new System.EventHandler(this.startSearchButtonActions_Click);
            // 
            // actioncmdSearchTextBox
            // 
            this.actioncmdSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actioncmdSearchTextBox.Location = new System.Drawing.Point(741, 28);
            this.actioncmdSearchTextBox.Name = "actioncmdSearchTextBox";
            this.actioncmdSearchTextBox.Size = new System.Drawing.Size(141, 22);
            this.actioncmdSearchTextBox.TabIndex = 21;
            this.actioncmdSearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.actioncmdSearchTextBox_KeyDown);
            // 
            // actionDataGridView
            // 
            this.actionDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.actionDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.actionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.actionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.actionDataGridView.Location = new System.Drawing.Point(741, 65);
            this.actionDataGridView.Name = "actionDataGridView";
            this.actionDataGridView.ReadOnly = true;
            this.actionDataGridView.RowHeadersVisible = false;
            this.actionDataGridView.Size = new System.Drawing.Size(245, 622);
            this.actionDataGridView.TabIndex = 20;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Format = "X4";
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.FillWeight = 30F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Command ID";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Action Command Name";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 200;
            this.dataGridViewTextBoxColumn2.MinimumWidth = 90;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // compOPDataGridView
            // 
            this.compOPDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.compOPDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.compOPDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.compOPDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.compOPDataGridView.Location = new System.Drawing.Point(1002, 65);
            this.compOPDataGridView.Name = "compOPDataGridView";
            this.compOPDataGridView.ReadOnly = true;
            this.compOPDataGridView.RowHeadersVisible = false;
            this.compOPDataGridView.Size = new System.Drawing.Size(245, 622);
            this.compOPDataGridView.TabIndex = 25;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Format = "X4";
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn3.FillWeight = 30F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Operator ID";
            this.dataGridViewTextBoxColumn3.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 60F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Comparison Operator Name";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 200;
            this.dataGridViewTextBoxColumn4.MinimumWidth = 90;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // exportScrcmdButton
            // 
            this.exportScrcmdButton.Image = global::DSPRE.Properties.Resources.scriptDBIconExport;
            this.exportScrcmdButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exportScrcmdButton.Location = new System.Drawing.Point(6, 19);
            this.exportScrcmdButton.Name = "exportScrcmdButton";
            this.exportScrcmdButton.Size = new System.Drawing.Size(126, 40);
            this.exportScrcmdButton.TabIndex = 26;
            this.exportScrcmdButton.Text = "Export Scrcmd";
            this.exportScrcmdButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.exportScrcmdButton.UseVisualStyleBackColor = true;
            this.exportScrcmdButton.Click += new System.EventHandler(this.exportScrcmdButton_Click);
            // 
            // manageScrcmdsButton
            // 
            this.manageScrcmdsButton.Image = global::DSPRE.Properties.Resources.scriptDBIcon;
            this.manageScrcmdsButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.manageScrcmdsButton.Location = new System.Drawing.Point(138, 19);
            this.manageScrcmdsButton.Name = "manageScrcmdsButton";
            this.manageScrcmdsButton.Size = new System.Drawing.Size(126, 40);
            this.manageScrcmdsButton.TabIndex = 28;
            this.manageScrcmdsButton.Text = "Manage Custom Scrcmd";
            this.manageScrcmdsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.manageScrcmdsButton.UseVisualStyleBackColor = true;
            this.manageScrcmdsButton.Click += new System.EventHandler(this.manageScrcmdsButton_Click);
            // 
            // customScrcmdSelector
            // 
            this.customScrcmdSelector.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.customScrcmdSelector.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.customScrcmdSelector.FormattingEnabled = true;
            this.customScrcmdSelector.Location = new System.Drawing.Point(6, 76);
            this.customScrcmdSelector.Name = "customScrcmdSelector";
            this.customScrcmdSelector.Size = new System.Drawing.Size(258, 21);
            this.customScrcmdSelector.TabIndex = 29;
            this.customScrcmdSelector.SelectedIndexChanged += new System.EventHandler(this.customScrcmdSelector_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.useGameDefaultScrcmdButton);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.manageScrcmdsButton);
            this.groupBox1.Controls.Add(this.exportScrcmdButton);
            this.groupBox1.Controls.Add(this.customScrcmdSelector);
            this.groupBox1.Location = new System.Drawing.Point(12, 693);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 115);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom Scrcmd";
            // 
            // useGameDefaultScrcmdButton
            // 
            this.useGameDefaultScrcmdButton.Image = global::DSPRE.Properties.Resources.scriptDBIcon;
            this.useGameDefaultScrcmdButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.useGameDefaultScrcmdButton.Location = new System.Drawing.Point(270, 65);
            this.useGameDefaultScrcmdButton.Name = "useGameDefaultScrcmdButton";
            this.useGameDefaultScrcmdButton.Size = new System.Drawing.Size(126, 40);
            this.useGameDefaultScrcmdButton.TabIndex = 31;
            this.useGameDefaultScrcmdButton.Text = "Use Game Default Scrcmd";
            this.useGameDefaultScrcmdButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.useGameDefaultScrcmdButton.UseVisualStyleBackColor = true;
            this.useGameDefaultScrcmdButton.Click += new System.EventHandler(this.useGameDefaultScrcmdButton_Click);
            // 
            // button2
            // 
            this.button2.Image = global::DSPRE.Properties.Resources.scriptDBIconImport;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.Location = new System.Drawing.Point(270, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 40);
            this.button2.TabIndex = 30;
            this.button2.Text = "Save Current Custom Scrcmd";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CommandsDatabase
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1262, 820);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.compOPDataGridView);
            this.Controls.Add(this.criteriaGroupBoxActions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startSearchButtonActions);
            this.Controls.Add(this.actioncmdSearchTextBox);
            this.Controls.Add(this.actionDataGridView);
            this.Controls.Add(this.criteriaGroupBoxScripts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startSearchButtonScripts);
            this.Controls.Add(this.scriptcmdSearchTextBox);
            this.Controls.Add(this.scriptcmdDataGridView);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommandsDatabase";
            this.Text = "Script Commands Database";
            ((System.ComponentModel.ISupportInitialize)(this.scriptcmdDataGridView)).EndInit();
            this.criteriaGroupBoxScripts.ResumeLayout(false);
            this.criteriaGroupBoxScripts.PerformLayout();
            this.criteriaGroupBoxActions.ResumeLayout(false);
            this.criteriaGroupBoxActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.compOPDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView scriptcmdDataGridView;
        private System.Windows.Forms.TextBox scriptcmdSearchTextBox;
        private System.Windows.Forms.Button startSearchButtonScripts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox criteriaGroupBoxScripts;
        private System.Windows.Forms.RadioButton matchCBScripts;
        private System.Windows.Forms.RadioButton containsCBScripts;
        private System.Windows.Forms.RadioButton startsWithCBScripts;
        private System.Windows.Forms.GroupBox criteriaGroupBoxActions;
        private System.Windows.Forms.RadioButton matchCBActions;
        private System.Windows.Forms.RadioButton containsCBActions;
        private System.Windows.Forms.RadioButton startsWithCBActions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button startSearchButtonActions;
        private System.Windows.Forms.TextBox actioncmdSearchTextBox;
        private System.Windows.Forms.DataGridView actionDataGridView;
        private System.Windows.Forms.DataGridView compOPDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommandID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamsCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Params;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.Button exportScrcmdButton;
        private System.Windows.Forms.Button manageScrcmdsButton;
        private InputComboBox customScrcmdSelector;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button useGameDefaultScrcmdButton;
    }
}