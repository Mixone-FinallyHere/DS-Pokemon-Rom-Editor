namespace DSPRE {
    partial class MatrixEditor {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixEditor));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            this.setSpawnPointButton = new System.Windows.Forms.Button();
            this.saveMatrixButton = new System.Windows.Forms.Button();
            this.locateCurrentMatrixFile = new System.Windows.Forms.Button();
            this.resetColorTableButton = new System.Windows.Forms.Button();
            this.importColorTableButton = new System.Windows.Forms.Button();
            this.importMatrixButton = new System.Windows.Forms.Button();
            this.exportMatrixButton = new System.Windows.Forms.Button();
            this.removeMatrixButton = new System.Windows.Forms.Button();
            this.addMatrixButton = new System.Windows.Forms.Button();
            this.removeHeightsButton = new System.Windows.Forms.Button();
            this.removeHeadersButton = new System.Windows.Forms.Button();
            this.addHeightsButton = new System.Windows.Forms.Button();
            this.addHeadersButton = new System.Windows.Forms.Button();
            this.labelMatrices = new System.Windows.Forms.Label();
            this.matrixNameLabel = new System.Windows.Forms.Label();
            this.matrixTabControl = new System.Windows.Forms.TabControl();
            this.headersTabPage = new System.Windows.Forms.TabPage();
            this.headersGridView = new System.Windows.Forms.DataGridView();
            this.heightsTabPage = new System.Windows.Forms.TabPage();
            this.heightsGridView = new System.Windows.Forms.DataGridView();
            this.mapFilesTabPage = new System.Windows.Forms.TabPage();
            this.mapFilesGridView = new System.Windows.Forms.DataGridView();
            this.matrixNameTextBox = new System.Windows.Forms.TextBox();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthLabel = new System.Windows.Forms.Label();
            this.selectMatrixComboBox = new System.Windows.Forms.ComboBox();
            this.matrixTabControl.SuspendLayout();
            this.headersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headersGridView)).BeginInit();
            this.heightsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightsGridView)).BeginInit();
            this.mapFilesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapFilesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // setSpawnPointButton
            // 
            this.setSpawnPointButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setSpawnPointButton.Image = global::DSPRE.Properties.Resources.spawnCoordsMatrixeditorIcon;
            this.setSpawnPointButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.setSpawnPointButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.setSpawnPointButton.Location = new System.Drawing.Point(4, 509);
            this.setSpawnPointButton.Name = "setSpawnPointButton";
            this.setSpawnPointButton.Size = new System.Drawing.Size(117, 43);
            this.setSpawnPointButton.TabIndex = 56;
            this.setSpawnPointButton.Text = "Set Spawn\r\nto Selection";
            this.setSpawnPointButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setSpawnPointButton.UseVisualStyleBackColor = true;
            this.setSpawnPointButton.Click += new System.EventHandler(this.setSpawnPointButton_Click);
            // 
            // saveMatrixButton
            // 
            this.saveMatrixButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveMatrixButton.Image = global::DSPRE.Properties.Resources.save_rom;
            this.saveMatrixButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.saveMatrixButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.saveMatrixButton.Location = new System.Drawing.Point(4, 558);
            this.saveMatrixButton.Name = "saveMatrixButton";
            this.saveMatrixButton.Size = new System.Drawing.Size(117, 43);
            this.saveMatrixButton.TabIndex = 55;
            this.saveMatrixButton.Text = "Save Matrix";
            this.saveMatrixButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveMatrixButton.UseVisualStyleBackColor = true;
            this.saveMatrixButton.Click += new System.EventHandler(this.saveMatrixButton_Click);
            // 
            // locateCurrentMatrixFile
            // 
            this.locateCurrentMatrixFile.Image = global::DSPRE.Properties.Resources.open_file;
            this.locateCurrentMatrixFile.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.locateCurrentMatrixFile.Location = new System.Drawing.Point(43, 224);
            this.locateCurrentMatrixFile.Name = "locateCurrentMatrixFile";
            this.locateCurrentMatrixFile.Size = new System.Drawing.Size(41, 38);
            this.locateCurrentMatrixFile.TabIndex = 54;
            this.locateCurrentMatrixFile.UseVisualStyleBackColor = true;
            // 
            // resetColorTableButton
            // 
            this.resetColorTableButton.Image = global::DSPRE.Properties.Resources.resetColorTable;
            this.resetColorTableButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.resetColorTableButton.Location = new System.Drawing.Point(4, 438);
            this.resetColorTableButton.Name = "resetColorTableButton";
            this.resetColorTableButton.Size = new System.Drawing.Size(117, 32);
            this.resetColorTableButton.TabIndex = 53;
            this.resetColorTableButton.Text = "Reset Color Table";
            this.resetColorTableButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.resetColorTableButton.UseVisualStyleBackColor = true;
            this.resetColorTableButton.Click += new System.EventHandler(this.resetColorTableButton_Click);
            // 
            // importColorTableButton
            // 
            this.importColorTableButton.Image = global::DSPRE.Properties.Resources.loadColorTable;
            this.importColorTableButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.importColorTableButton.Location = new System.Drawing.Point(4, 471);
            this.importColorTableButton.Name = "importColorTableButton";
            this.importColorTableButton.Size = new System.Drawing.Size(117, 32);
            this.importColorTableButton.TabIndex = 52;
            this.importColorTableButton.Text = "Import Color Table";
            this.importColorTableButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importColorTableButton.UseVisualStyleBackColor = true;
            this.importColorTableButton.Click += new System.EventHandler(this.importColorTableButton_Click);
            // 
            // importMatrixButton
            // 
            this.importMatrixButton.Image = global::DSPRE.Properties.Resources.importArrow;
            this.importMatrixButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.importMatrixButton.Location = new System.Drawing.Point(4, 144);
            this.importMatrixButton.Name = "importMatrixButton";
            this.importMatrixButton.Size = new System.Drawing.Size(117, 29);
            this.importMatrixButton.TabIndex = 51;
            this.importMatrixButton.Text = "Replace Matrix";
            this.importMatrixButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importMatrixButton.UseVisualStyleBackColor = true;
            this.importMatrixButton.Click += new System.EventHandler(this.importMatrixButton_Click);
            // 
            // exportMatrixButton
            // 
            this.exportMatrixButton.Image = global::DSPRE.Properties.Resources.exportArrow;
            this.exportMatrixButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exportMatrixButton.Location = new System.Drawing.Point(4, 114);
            this.exportMatrixButton.Name = "exportMatrixButton";
            this.exportMatrixButton.Size = new System.Drawing.Size(117, 29);
            this.exportMatrixButton.TabIndex = 50;
            this.exportMatrixButton.Text = "Export Matrix";
            this.exportMatrixButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exportMatrixButton.UseVisualStyleBackColor = true;
            this.exportMatrixButton.Click += new System.EventHandler(this.exportMatrixButton_Click);
            // 
            // removeMatrixButton
            // 
            this.removeMatrixButton.Image = ((System.Drawing.Image)(resources.GetObject("removeMatrixButton.Image")));
            this.removeMatrixButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeMatrixButton.Location = new System.Drawing.Point(57, 175);
            this.removeMatrixButton.Name = "removeMatrixButton";
            this.removeMatrixButton.Size = new System.Drawing.Size(64, 35);
            this.removeMatrixButton.TabIndex = 49;
            this.removeMatrixButton.Text = "Delete Last";
            this.removeMatrixButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeMatrixButton.UseVisualStyleBackColor = true;
            this.removeMatrixButton.Click += new System.EventHandler(this.removeMatrixButton_Click);
            // 
            // addMatrixButton
            // 
            this.addMatrixButton.Image = ((System.Drawing.Image)(resources.GetObject("addMatrixButton.Image")));
            this.addMatrixButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addMatrixButton.Location = new System.Drawing.Point(4, 175);
            this.addMatrixButton.Name = "addMatrixButton";
            this.addMatrixButton.Size = new System.Drawing.Size(51, 35);
            this.addMatrixButton.TabIndex = 36;
            this.addMatrixButton.Text = "Add";
            this.addMatrixButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addMatrixButton.UseVisualStyleBackColor = true;
            this.addMatrixButton.Click += new System.EventHandler(this.addMatrixButton_Click);
            // 
            // removeHeightsButton
            // 
            this.removeHeightsButton.Image = ((System.Drawing.Image)(resources.GetObject("removeHeightsButton.Image")));
            this.removeHeightsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeHeightsButton.Location = new System.Drawing.Point(4, 394);
            this.removeHeightsButton.Name = "removeHeightsButton";
            this.removeHeightsButton.Size = new System.Drawing.Size(117, 35);
            this.removeHeightsButton.TabIndex = 48;
            this.removeHeightsButton.Text = "Remove Heights";
            this.removeHeightsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeHeightsButton.UseVisualStyleBackColor = true;
            this.removeHeightsButton.Click += new System.EventHandler(this.removeHeightsButton_Click);
            // 
            // removeHeadersButton
            // 
            this.removeHeadersButton.Image = ((System.Drawing.Image)(resources.GetObject("removeHeadersButton.Image")));
            this.removeHeadersButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeHeadersButton.Location = new System.Drawing.Point(4, 311);
            this.removeHeadersButton.Name = "removeHeadersButton";
            this.removeHeadersButton.Size = new System.Drawing.Size(117, 35);
            this.removeHeadersButton.TabIndex = 47;
            this.removeHeadersButton.Text = "Remove Headers";
            this.removeHeadersButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeHeadersButton.UseVisualStyleBackColor = true;
            this.removeHeadersButton.Click += new System.EventHandler(this.removeHeadersButton_Click);
            // 
            // addHeightsButton
            // 
            this.addHeightsButton.Image = ((System.Drawing.Image)(resources.GetObject("addHeightsButton.Image")));
            this.addHeightsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addHeightsButton.Location = new System.Drawing.Point(4, 358);
            this.addHeightsButton.Name = "addHeightsButton";
            this.addHeightsButton.Size = new System.Drawing.Size(117, 35);
            this.addHeightsButton.TabIndex = 46;
            this.addHeightsButton.Text = "Add Heights Tab";
            this.addHeightsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addHeightsButton.UseVisualStyleBackColor = true;
            this.addHeightsButton.Click += new System.EventHandler(this.addHeightsButton_Click);
            // 
            // addHeadersButton
            // 
            this.addHeadersButton.Image = ((System.Drawing.Image)(resources.GetObject("addHeadersButton.Image")));
            this.addHeadersButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addHeadersButton.Location = new System.Drawing.Point(4, 275);
            this.addHeadersButton.Name = "addHeadersButton";
            this.addHeadersButton.Size = new System.Drawing.Size(117, 35);
            this.addHeadersButton.TabIndex = 45;
            this.addHeadersButton.Text = "Add Header Tab";
            this.addHeadersButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addHeadersButton.UseVisualStyleBackColor = true;
            this.addHeadersButton.Click += new System.EventHandler(this.addHeaderSectionButton_Click);
            // 
            // labelMatrices
            // 
            this.labelMatrices.AutoSize = true;
            this.labelMatrices.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelMatrices.Location = new System.Drawing.Point(2, 3);
            this.labelMatrices.Name = "labelMatrices";
            this.labelMatrices.Size = new System.Drawing.Size(47, 13);
            this.labelMatrices.TabIndex = 44;
            this.labelMatrices.Text = "Matrices";
            // 
            // matrixNameLabel
            // 
            this.matrixNameLabel.AutoSize = true;
            this.matrixNameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.matrixNameLabel.Location = new System.Drawing.Point(3, 47);
            this.matrixNameLabel.Name = "matrixNameLabel";
            this.matrixNameLabel.Size = new System.Drawing.Size(64, 13);
            this.matrixNameLabel.TabIndex = 43;
            this.matrixNameLabel.Text = "Matrix name";
            // 
            // matrixTabControl
            // 
            this.matrixTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.matrixTabControl.Controls.Add(this.headersTabPage);
            this.matrixTabControl.Controls.Add(this.heightsTabPage);
            this.matrixTabControl.Controls.Add(this.mapFilesTabPage);
            this.matrixTabControl.Location = new System.Drawing.Point(154, 3);
            this.matrixTabControl.Multiline = true;
            this.matrixTabControl.Name = "matrixTabControl";
            this.matrixTabControl.SelectedIndex = 0;
            this.matrixTabControl.Size = new System.Drawing.Size(1028, 610);
            this.matrixTabControl.TabIndex = 42;
            // 
            // headersTabPage
            // 
            this.headersTabPage.Controls.Add(this.headersGridView);
            this.headersTabPage.Location = new System.Drawing.Point(4, 22);
            this.headersTabPage.Name = "headersTabPage";
            this.headersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.headersTabPage.Size = new System.Drawing.Size(1020, 584);
            this.headersTabPage.TabIndex = 1;
            this.headersTabPage.Text = "Map Headers";
            this.headersTabPage.UseVisualStyleBackColor = true;
            // 
            // headersGridView
            // 
            this.headersGridView.AllowUserToAddRows = false;
            this.headersGridView.AllowUserToDeleteRows = false;
            this.headersGridView.AllowUserToResizeColumns = false;
            this.headersGridView.AllowUserToResizeRows = false;
            this.headersGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.headersGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
            this.headersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle30.Format = "D4";
            dataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.headersGridView.DefaultCellStyle = dataGridViewCellStyle30;
            this.headersGridView.Location = new System.Drawing.Point(0, 0);
            this.headersGridView.MultiSelect = false;
            this.headersGridView.Name = "headersGridView";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle31.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.headersGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle31;
            this.headersGridView.RowHeadersWidth = 50;
            this.headersGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headersGridView.RowsDefaultCellStyle = dataGridViewCellStyle32;
            this.headersGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.headersGridView.RowTemplate.Height = 18;
            this.headersGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.headersGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.headersGridView.ShowCellErrors = false;
            this.headersGridView.Size = new System.Drawing.Size(1032, 576);
            this.headersGridView.TabIndex = 1;
            this.headersGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.headersGridView_CellFormatting);
            this.headersGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.headersGridView_CellMouseDoubleClick);
            this.headersGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.headersGridView_CellValueChanged);
            this.headersGridView.SelectionChanged += new System.EventHandler(this.headersGridView_SelectionChanged);
            // 
            // heightsTabPage
            // 
            this.heightsTabPage.Controls.Add(this.heightsGridView);
            this.heightsTabPage.Location = new System.Drawing.Point(4, 22);
            this.heightsTabPage.Name = "heightsTabPage";
            this.heightsTabPage.Size = new System.Drawing.Size(1020, 584);
            this.heightsTabPage.TabIndex = 2;
            this.heightsTabPage.Text = "Map Heights";
            this.heightsTabPage.UseVisualStyleBackColor = true;
            // 
            // heightsGridView
            // 
            this.heightsGridView.AllowUserToAddRows = false;
            this.heightsGridView.AllowUserToDeleteRows = false;
            this.heightsGridView.AllowUserToResizeColumns = false;
            this.heightsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.heightsGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle33;
            this.heightsGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.heightsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.heightsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle35.Format = "D2";
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.heightsGridView.DefaultCellStyle = dataGridViewCellStyle35;
            this.heightsGridView.Location = new System.Drawing.Point(0, 0);
            this.heightsGridView.MultiSelect = false;
            this.heightsGridView.Name = "heightsGridView";
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.heightsGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle36;
            this.heightsGridView.RowHeadersWidth = 50;
            this.heightsGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle37.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heightsGridView.RowsDefaultCellStyle = dataGridViewCellStyle37;
            this.heightsGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.heightsGridView.RowTemplate.Height = 18;
            this.heightsGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.heightsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.heightsGridView.Size = new System.Drawing.Size(1032, 576);
            this.heightsGridView.TabIndex = 2;
            this.heightsGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.heightsGridView_CellFormatting);
            this.heightsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.headersGridView_CellMouseDoubleClick);
            this.heightsGridView.SelectionChanged += new System.EventHandler(this.heightsGridView_SelectionChanged);
            // 
            // mapFilesTabPage
            // 
            this.mapFilesTabPage.Controls.Add(this.mapFilesGridView);
            this.mapFilesTabPage.Location = new System.Drawing.Point(4, 22);
            this.mapFilesTabPage.Name = "mapFilesTabPage";
            this.mapFilesTabPage.Size = new System.Drawing.Size(1020, 584);
            this.mapFilesTabPage.TabIndex = 3;
            this.mapFilesTabPage.Text = "Map Files";
            this.mapFilesTabPage.UseVisualStyleBackColor = true;
            // 
            // mapFilesGridView
            // 
            this.mapFilesGridView.AllowUserToAddRows = false;
            this.mapFilesGridView.AllowUserToDeleteRows = false;
            this.mapFilesGridView.AllowUserToResizeColumns = false;
            this.mapFilesGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mapFilesGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle38;
            this.mapFilesGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle39.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle39.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle39.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle39.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle39.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle39.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mapFilesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle39;
            this.mapFilesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle40.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle40.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle40.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle40.Format = "D4";
            dataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle40.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mapFilesGridView.DefaultCellStyle = dataGridViewCellStyle40;
            this.mapFilesGridView.Location = new System.Drawing.Point(0, 0);
            this.mapFilesGridView.MultiSelect = false;
            this.mapFilesGridView.Name = "mapFilesGridView";
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle41.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle41.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            dataGridViewCellStyle41.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle41.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle41.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle41.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mapFilesGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle41;
            this.mapFilesGridView.RowHeadersWidth = 50;
            this.mapFilesGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle42.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapFilesGridView.RowsDefaultCellStyle = dataGridViewCellStyle42;
            this.mapFilesGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.mapFilesGridView.RowTemplate.Height = 18;
            this.mapFilesGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.mapFilesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.mapFilesGridView.Size = new System.Drawing.Size(1032, 576);
            this.mapFilesGridView.TabIndex = 2;
            this.mapFilesGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.mapFilesGridView_CellFormatting);
            this.mapFilesGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.mapFilesGridView_CellMouseDoubleClick);
            this.mapFilesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.mapFilesGridView_CellValueChanged);
            this.mapFilesGridView.SelectionChanged += new System.EventHandler(this.mapFilesGridView_SelectionChanged);
            // 
            // matrixNameTextBox
            // 
            this.matrixNameTextBox.Location = new System.Drawing.Point(6, 61);
            this.matrixNameTextBox.MaxLength = 16;
            this.matrixNameTextBox.Name = "matrixNameTextBox";
            this.matrixNameTextBox.Size = new System.Drawing.Size(112, 20);
            this.matrixNameTextBox.TabIndex = 41;
            this.matrixNameTextBox.TextChanged += new System.EventHandler(this.matrixNameTextBox_TextChanged);
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(77, 90);
            this.heightUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.heightUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightUpDown.Name = "heightUpDown";
            this.heightUpDown.Size = new System.Drawing.Size(37, 20);
            this.heightUpDown.TabIndex = 40;
            this.heightUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightUpDown.ValueChanged += new System.EventHandler(this.heightUpDown_ValueChanged);
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point(34, 90);
            this.widthUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.widthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthUpDown.Name = "widthUpDown";
            this.widthUpDown.Size = new System.Drawing.Size(37, 20);
            this.widthUpDown.TabIndex = 39;
            this.widthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthUpDown.ValueChanged += new System.EventHandler(this.widthUpDown_ValueChanged);
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.widthLabel.Location = new System.Drawing.Point(3, 93);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(27, 13);
            this.widthLabel.TabIndex = 38;
            this.widthLabel.Text = "Size";
            // 
            // selectMatrixComboBox
            // 
            this.selectMatrixComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectMatrixComboBox.FormattingEnabled = true;
            this.selectMatrixComboBox.Location = new System.Drawing.Point(6, 19);
            this.selectMatrixComboBox.Name = "selectMatrixComboBox";
            this.selectMatrixComboBox.Size = new System.Drawing.Size(112, 21);
            this.selectMatrixComboBox.TabIndex = 37;
            this.selectMatrixComboBox.SelectedIndexChanged += new System.EventHandler(this.selectMatrixComboBox_SelectedIndexChanged);
            // 
            // MatrixEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.setSpawnPointButton);
            this.Controls.Add(this.saveMatrixButton);
            this.Controls.Add(this.locateCurrentMatrixFile);
            this.Controls.Add(this.resetColorTableButton);
            this.Controls.Add(this.importColorTableButton);
            this.Controls.Add(this.importMatrixButton);
            this.Controls.Add(this.exportMatrixButton);
            this.Controls.Add(this.removeMatrixButton);
            this.Controls.Add(this.addMatrixButton);
            this.Controls.Add(this.removeHeightsButton);
            this.Controls.Add(this.removeHeadersButton);
            this.Controls.Add(this.addHeightsButton);
            this.Controls.Add(this.addHeadersButton);
            this.Controls.Add(this.labelMatrices);
            this.Controls.Add(this.matrixNameLabel);
            this.Controls.Add(this.matrixTabControl);
            this.Controls.Add(this.matrixNameTextBox);
            this.Controls.Add(this.heightUpDown);
            this.Controls.Add(this.widthUpDown);
            this.Controls.Add(this.widthLabel);
            this.Controls.Add(this.selectMatrixComboBox);
            this.Name = "MatrixEditor";
            this.Size = new System.Drawing.Size(1185, 633);
            this.matrixTabControl.ResumeLayout(false);
            this.headersTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headersGridView)).EndInit();
            this.heightsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.heightsGridView)).EndInit();
            this.mapFilesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapFilesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setSpawnPointButton;
        private System.Windows.Forms.Button saveMatrixButton;
        private System.Windows.Forms.Button locateCurrentMatrixFile;
        private System.Windows.Forms.Button resetColorTableButton;
        private System.Windows.Forms.Button importColorTableButton;
        private System.Windows.Forms.Button importMatrixButton;
        private System.Windows.Forms.Button exportMatrixButton;
        private System.Windows.Forms.Button removeMatrixButton;
        private System.Windows.Forms.Button addMatrixButton;
        private System.Windows.Forms.Button removeHeightsButton;
        private System.Windows.Forms.Button removeHeadersButton;
        private System.Windows.Forms.Button addHeightsButton;
        private System.Windows.Forms.Button addHeadersButton;
        private System.Windows.Forms.Label labelMatrices;
        private System.Windows.Forms.Label matrixNameLabel;
        private System.Windows.Forms.TabControl matrixTabControl;
        private System.Windows.Forms.TabPage headersTabPage;
        private System.Windows.Forms.DataGridView headersGridView;
        private System.Windows.Forms.TabPage heightsTabPage;
        private System.Windows.Forms.DataGridView heightsGridView;
        private System.Windows.Forms.TabPage mapFilesTabPage;
        private System.Windows.Forms.DataGridView mapFilesGridView;
        private System.Windows.Forms.TextBox matrixNameTextBox;
        private System.Windows.Forms.NumericUpDown heightUpDown;
        private System.Windows.Forms.NumericUpDown widthUpDown;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.ComboBox selectMatrixComboBox;
    }
}
