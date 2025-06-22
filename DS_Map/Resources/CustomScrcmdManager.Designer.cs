namespace DSPRE.Resources
{
    partial class CustomScrcmdManager
    {
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
            this.CustomScrcmdDataGrid = new System.Windows.Forms.DataGridView();
            this.customScrcmdName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomScrcmdPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddScrcmdButton = new System.Windows.Forms.Button();
            this.RemoveScrcmdButton = new System.Windows.Forms.Button();
            this.SaveScrcmdButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CustomScrcmdDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomScrcmdDataGrid
            // 
            this.CustomScrcmdDataGrid.AllowUserToAddRows = false;
            this.CustomScrcmdDataGrid.AllowUserToDeleteRows = false;
            this.CustomScrcmdDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustomScrcmdDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customScrcmdName,
            this.CustomScrcmdPath});
            this.CustomScrcmdDataGrid.Location = new System.Drawing.Point(12, 12);
            this.CustomScrcmdDataGrid.MultiSelect = false;
            this.CustomScrcmdDataGrid.Name = "CustomScrcmdDataGrid";
            this.CustomScrcmdDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CustomScrcmdDataGrid.Size = new System.Drawing.Size(463, 284);
            this.CustomScrcmdDataGrid.TabIndex = 0;
            // 
            // customScrcmdName
            // 
            this.customScrcmdName.HeaderText = "Name";
            this.customScrcmdName.Name = "customScrcmdName";
            // 
            // CustomScrcmdPath
            // 
            this.CustomScrcmdPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CustomScrcmdPath.HeaderText = "Json File Path";
            this.CustomScrcmdPath.Name = "CustomScrcmdPath";
            this.CustomScrcmdPath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CustomScrcmdPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AddScrcmdButton
            // 
            this.AddScrcmdButton.Image = global::DSPRE.Properties.Resources.addIcon;
            this.AddScrcmdButton.Location = new System.Drawing.Point(12, 302);
            this.AddScrcmdButton.Name = "AddScrcmdButton";
            this.AddScrcmdButton.Size = new System.Drawing.Size(75, 32);
            this.AddScrcmdButton.TabIndex = 1;
            this.AddScrcmdButton.Text = "Add";
            this.AddScrcmdButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.AddScrcmdButton.UseVisualStyleBackColor = true;
            this.AddScrcmdButton.Click += new System.EventHandler(this.AddScrcmdButton_Click);
            // 
            // RemoveScrcmdButton
            // 
            this.RemoveScrcmdButton.Image = global::DSPRE.Properties.Resources.deleteIcon;
            this.RemoveScrcmdButton.Location = new System.Drawing.Point(93, 302);
            this.RemoveScrcmdButton.Name = "RemoveScrcmdButton";
            this.RemoveScrcmdButton.Size = new System.Drawing.Size(75, 32);
            this.RemoveScrcmdButton.TabIndex = 2;
            this.RemoveScrcmdButton.Text = "Remove";
            this.RemoveScrcmdButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.RemoveScrcmdButton.UseVisualStyleBackColor = true;
            this.RemoveScrcmdButton.Click += new System.EventHandler(this.RemoveScrcmdButton_Click);
            // 
            // SaveScrcmdButton
            // 
            this.SaveScrcmdButton.Image = global::DSPRE.Properties.Resources.saveButton;
            this.SaveScrcmdButton.Location = new System.Drawing.Point(400, 302);
            this.SaveScrcmdButton.Name = "SaveScrcmdButton";
            this.SaveScrcmdButton.Size = new System.Drawing.Size(75, 32);
            this.SaveScrcmdButton.TabIndex = 3;
            this.SaveScrcmdButton.Text = "Save";
            this.SaveScrcmdButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.SaveScrcmdButton.UseVisualStyleBackColor = true;
            this.SaveScrcmdButton.Click += new System.EventHandler(this.SaveScrcmdButton_Click);
            // 
            // CustomScrcmdManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 344);
            this.Controls.Add(this.SaveScrcmdButton);
            this.Controls.Add(this.RemoveScrcmdButton);
            this.Controls.Add(this.AddScrcmdButton);
            this.Controls.Add(this.CustomScrcmdDataGrid);
            this.MaximumSize = new System.Drawing.Size(506, 383);
            this.MinimumSize = new System.Drawing.Size(506, 383);
            this.Name = "CustomScrcmdManager";
            this.ShowIcon = false;
            this.Text = "Custom Scrcmd Manager";
            ((System.ComponentModel.ISupportInitialize)(this.CustomScrcmdDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView CustomScrcmdDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn customScrcmdName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomScrcmdPath;
        private System.Windows.Forms.Button AddScrcmdButton;
        private System.Windows.Forms.Button RemoveScrcmdButton;
        private System.Windows.Forms.Button SaveScrcmdButton;
    }
}