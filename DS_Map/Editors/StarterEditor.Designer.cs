namespace DSPRE.Editors {
    partial class StarterEditor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StarterEditor));
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.labelStarter1 = new System.Windows.Forms.Label();
            this.labelStarter2 = new System.Windows.Forms.Label();
            this.labelStarter3 = new System.Windows.Forms.Label();
            this.picStarter1 = new System.Windows.Forms.PictureBox();
            this.picStarter2 = new System.Windows.Forms.PictureBox();
            this.picStarter3 = new System.Windows.Forms.PictureBox();
            this.chosenStarter1 = new DSPRE.InputComboBox();
            this.chosenStarter2 = new DSPRE.InputComboBox();
            this.chosenStarter3 = new DSPRE.InputComboBox();
            this.mainLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStarter1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStarter2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStarter3)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 11;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.mainLayoutPanel.Controls.Add(this.labelStarter1, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.chosenStarter1, 1, 1);
            this.mainLayoutPanel.Controls.Add(this.chosenStarter2, 5, 1);
            this.mainLayoutPanel.Controls.Add(this.picStarter1, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.labelStarter2, 4, 0);
            this.mainLayoutPanel.Controls.Add(this.chosenStarter3, 9, 1);
            this.mainLayoutPanel.Controls.Add(this.picStarter2, 4, 1);
            this.mainLayoutPanel.Controls.Add(this.picStarter3, 8, 1);
            this.mainLayoutPanel.Controls.Add(this.labelStarter3, 8, 0);
            this.mainLayoutPanel.Controls.Add(this.button1, 4, 2);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.mainLayoutPanel.RowCount = 3;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.80488F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.41463F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.29268F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(800, 215);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mainLayoutPanel.SetColumnSpan(this.button1, 3);
            this.button1.Location = new System.Drawing.Point(347, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save Changes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelStarter1
            // 
            this.labelStarter1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelStarter1.AutoSize = true;
            this.mainLayoutPanel.SetColumnSpan(this.labelStarter1, 3);
            this.labelStarter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStarter1.Location = new System.Drawing.Point(76, 26);
            this.labelStarter1.Name = "labelStarter1";
            this.labelStarter1.Size = new System.Drawing.Size(78, 24);
            this.labelStarter1.TabIndex = 1;
            this.labelStarter1.Text = "Starter 1";
            // 
            // labelStarter2
            // 
            this.labelStarter2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelStarter2.AutoSize = true;
            this.mainLayoutPanel.SetColumnSpan(this.labelStarter2, 3);
            this.labelStarter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStarter2.Location = new System.Drawing.Point(356, 26);
            this.labelStarter2.Name = "labelStarter2";
            this.labelStarter2.Size = new System.Drawing.Size(78, 24);
            this.labelStarter2.TabIndex = 2;
            this.labelStarter2.Text = "Starter 2";
            // 
            // labelStarter3
            // 
            this.labelStarter3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelStarter3.AutoSize = true;
            this.mainLayoutPanel.SetColumnSpan(this.labelStarter3, 3);
            this.labelStarter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStarter3.Location = new System.Drawing.Point(641, 26);
            this.labelStarter3.Name = "labelStarter3";
            this.labelStarter3.Size = new System.Drawing.Size(78, 24);
            this.labelStarter3.TabIndex = 3;
            this.labelStarter3.Text = "Starter 3";
            // 
            // picStarter1
            // 
            this.picStarter1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picStarter1.Location = new System.Drawing.Point(13, 79);
            this.picStarter1.Name = "picStarter1";
            this.picStarter1.Size = new System.Drawing.Size(64, 64);
            this.picStarter1.TabIndex = 4;
            this.picStarter1.TabStop = false;
            // 
            // picStarter2
            // 
            this.picStarter2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picStarter2.Location = new System.Drawing.Point(293, 80);
            this.picStarter2.Name = "picStarter2";
            this.picStarter2.Size = new System.Drawing.Size(64, 63);
            this.picStarter2.TabIndex = 5;
            this.picStarter2.TabStop = false;
            // 
            // picStarter3
            // 
            this.picStarter3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picStarter3.Location = new System.Drawing.Point(573, 80);
            this.picStarter3.Name = "picStarter3";
            this.picStarter3.Size = new System.Drawing.Size(64, 63);
            this.picStarter3.TabIndex = 6;
            this.picStarter3.TabStop = false;
            // 
            // chosenStarter1
            // 
            this.chosenStarter1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chosenStarter1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.chosenStarter1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.mainLayoutPanel.SetColumnSpan(this.chosenStarter1, 2);
            this.chosenStarter1.FormattingEnabled = true;
            this.chosenStarter1.Location = new System.Drawing.Point(83, 101);
            this.chosenStarter1.Name = "chosenStarter1";
            this.chosenStarter1.Size = new System.Drawing.Size(134, 21);
            this.chosenStarter1.TabIndex = 7;
            this.chosenStarter1.SelectedIndexChanged += new System.EventHandler(this.chosenStarter1_SelectedIndexChanged);
            // 
            // chosenStarter2
            // 
            this.chosenStarter2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chosenStarter2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.chosenStarter2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.mainLayoutPanel.SetColumnSpan(this.chosenStarter2, 2);
            this.chosenStarter2.FormattingEnabled = true;
            this.chosenStarter2.Location = new System.Drawing.Point(363, 101);
            this.chosenStarter2.Name = "chosenStarter2";
            this.chosenStarter2.Size = new System.Drawing.Size(134, 21);
            this.chosenStarter2.TabIndex = 8;
            this.chosenStarter2.SelectedIndexChanged += new System.EventHandler(this.chosenStarter2_SelectedIndexChanged);
            // 
            // chosenStarter3
            // 
            this.chosenStarter3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chosenStarter3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.chosenStarter3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.mainLayoutPanel.SetColumnSpan(this.chosenStarter3, 2);
            this.chosenStarter3.FormattingEnabled = true;
            this.chosenStarter3.Location = new System.Drawing.Point(644, 101);
            this.chosenStarter3.Name = "chosenStarter3";
            this.chosenStarter3.Size = new System.Drawing.Size(142, 21);
            this.chosenStarter3.TabIndex = 9;
            this.chosenStarter3.SelectedIndexChanged += new System.EventHandler(this.chosenStarter3_SelectedIndexChanged);
            // 
            // StarterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 215);
            this.Controls.Add(this.mainLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StarterEditor";
            this.Text = "Starter Editor";
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStarter1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStarter2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStarter3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelStarter1;
        private System.Windows.Forms.Label labelStarter2;
        private System.Windows.Forms.Label labelStarter3;
        private System.Windows.Forms.PictureBox picStarter1;
        private System.Windows.Forms.PictureBox picStarter2;
        private System.Windows.Forms.PictureBox picStarter3;
        private InputComboBox chosenStarter1;
        private InputComboBox chosenStarter2;
        private InputComboBox chosenStarter3;
    }
}