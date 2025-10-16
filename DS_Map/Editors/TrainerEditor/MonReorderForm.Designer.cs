namespace DSPRE.Editors.TrainerEditor
{
    partial class MonReorderForm
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
            this.monListBox = new System.Windows.Forms.ListBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // monListBox
            // 
            this.monListBox.FormattingEnabled = true;
            this.monListBox.Location = new System.Drawing.Point(12, 12);
            this.monListBox.Name = "monListBox";
            this.monListBox.Size = new System.Drawing.Size(292, 355);
            this.monListBox.TabIndex = 0;
            this.monListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.monListBox_DragDrop);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(229, 373);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // MonReorderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 404);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.monListBox);
            this.Name = "MonReorderForm";
            this.Text = "Reorder Party";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonReorderForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox monListBox;
        private System.Windows.Forms.Button saveButton;
    }
}