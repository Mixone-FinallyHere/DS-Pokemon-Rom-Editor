using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DSPRE.OverlayUtils;
using static ScintillaNET.Style;

namespace DSPRE { 
    public partial class OverlayEditor : Form {
        
        private List<OverlayEntry> overlays;
        private bool currentValComp = true;
        private bool currentValMark = true;

        public OverlayEditor() {
            InitializeComponent();
            overlayDataGrid.DataSource = overlays;
            overlayDataGrid.Columns[0].HeaderText = "Overlay ID";
            overlayDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            overlayDataGrid.AllowUserToResizeColumns = false;
            overlayDataGrid.Columns[1].HeaderText = "Compressed";
            overlayDataGrid.Columns[2].HeaderText = "Marked Compressed";
            overlayDataGrid.Columns[3].HeaderText = "RAM Address";
            overlayDataGrid.Columns[4].HeaderText = "Uncompressed Size";
            overlayDataGrid.Columns[0].ReadOnly = true;
            overlayDataGrid.Columns[3].ReadOnly = true;
            overlayDataGrid.Columns[4].ReadOnly = true;
        }

        private void isMarkedCompressedButton_Click(object sender, EventArgs e) 
        {
        }
        private void isCompressedButton_Click(object sender, EventArgs e) 
        {
        }

        private void revertChangesButton_Click(object sender, EventArgs e) 
        {
        }

        private void overlayDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) 
        {       
        }

        private void saveChangesButton_Click(object sender, EventArgs e) 
        {

            //DialogResult d = MessageBox.Show("This operation will modify the following overlays: " + Environment.NewLine
            //    + String.Join(", ", modifiedNumbers)
            //    + "\nProceed?", "Confirmation required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (d == DialogResult.Yes) {
            //    foreach (Overlay overlay in modifiedOverlays) {
            //        OverlayUtils.OverlayTable.SetDefaultCompressed(overlay.id, overlay.isMarkedCompressed);
            //        if (overlay.compressed && !OverlayUtils.IsCompressed(overlay.id))
            //            OverlayUtils.Compress(overlay.id);
            //        if (!overlay.compressed && OverlayUtils.IsCompressed(overlay.id))
            //            OverlayUtils.Decompress(overlay.id);
            //    }
            //}
        }

        private bool FindMismatches(bool paintThem = true) {
            foreach (DataGridViewRow row in overlayDataGrid.Rows) {
                if ((bool)row.Cells[1].Value != (bool)row.Cells[2].Value) {
                    if (paintThem) {
                        row.Cells[1].Style.BackColor = Color.Red;
                        row.Cells[2].Style.BackColor = Color.Red;
                    } else {
                        return true;
                    }                    
                } else {
                    if (paintThem) {
                        row.Cells[1].Style.BackColor = Color.White;
                        row.Cells[2].Style.BackColor = Color.White;
                    }                    
                }
            }
            return false;
        }

        private void overlayDataGrid_SelectionChanged(object sender, EventArgs e) {
            overlayDataGrid.ClearSelection();
        }

        private void overlayDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            FindMismatches();
        }
    }

    
}
