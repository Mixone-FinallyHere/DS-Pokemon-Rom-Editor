using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using static ScintillaNET.Style;
using static Tao.Platform.Windows.Winmm;

namespace DSPRE.Resources {
    public partial class CommandsDatabase : Form {
        private DataGridViewRow currentrow;

        private List<RadioButton> scriptRadioButtons;
        private List<RadioButton> actionRadioButtons;
        ScriptDatabaseJSON scriptDatabaseJSON;

        public CommandsDatabase(ScriptDatabaseJSON db) {

            var customScrcmds = CustomScrcmdManager.LoadSettings();
            scriptDatabaseJSON = db;

            InitializeComponent();
            InitializeTables();

            if (customScrcmds.Count > 0)
            {
                customScrcmdSelector.Enabled = true;
               

                switch (RomInfo.gameFamily)
                {
                    case RomInfo.GameFamilies.HGSS:
                        useGameDefaultScrcmdButton.Image = DSPRE.Properties.Resources.scriptDBIconHGSS;
                        break;
                    case RomInfo.GameFamilies.DP:
                        useGameDefaultScrcmdButton.Image = DSPRE.Properties.Resources.scriptDBIconDP;

                        break;
                    case RomInfo.GameFamilies.Plat:
                        useGameDefaultScrcmdButton.Image = DSPRE.Properties.Resources.scriptDBIconPt;
                        break;

                    default:
                        useGameDefaultScrcmdButton.Image = DSPRE.Properties.Resources.scriptDBIcon;
                        break;

                }
                //DSPRE.Properties.Resources.scriptDBIconDP
                foreach (var cusScrcmd in customScrcmds)
                {
                    customScrcmdSelector.Items.Add(cusScrcmd.Name);
                }
            }
            else
            {
                customScrcmdSelector.Enabled = false;
                useGameDefaultScrcmdButton.Enabled = false;
            }

            scriptRadioButtons = new List<RadioButton>() {
                containsCBScripts,
                startsWithCBScripts,
                matchCBScripts
            };
            actionRadioButtons = new List<RadioButton>() {
                containsCBActions,
                startsWithCBActions,
                matchCBActions
            };
        }

        private void InitializeTables()
        {

            string selectedScrcmd = scriptDatabaseJSON.SelectedScrcmd;
            var db = scriptDatabaseJSON.GetDatabaseFromJSON(selectedScrcmd);

            if (selectedScrcmd == null) {
                useGameDefaultScrcmdButton.Enabled = false;
            } else
            {
                useGameDefaultScrcmdButton.Enabled = true;
            }

            SetupScriptCommandsTable(scriptcmdDataGridView, db.Scrcmd);
            SetupFromScriptDictionaries(actionDataGridView, db.Movements);
            SetupFromScriptDictionaries(compOPDataGridView, db.ComparisonOperators);
        }

        private void SetupFromScriptDictionaries(DataGridView table, Dictionary<string, string> sourceNamesDict) {
            table.Rows.Clear();

            int index = 0;
            foreach (var kvp in sourceNamesDict)
            {
                table.Rows.Add();
                DataGridViewRow r = table.Rows[index];
                r.Cells[0].Value = kvp.Key;
                r.Cells[1].Value = kvp.Value;

                index++;
 
            }
        }

        private void SetupScriptCommandsTable(DataGridView table, Dictionary<string, Command> scrmcd)
        {
            table.Rows.Clear();

            int index = 0;
            foreach (var kvp in scrmcd)
            {
                table.Rows.Add();
                DataGridViewRow r = table.Rows[index];

                r.Cells[0].Value = kvp.Key;
                r.Cells[1].Value = kvp.Value.Name;
                r.Cells[2].Value = kvp.Value.Parameters.Count;
                r.Cells[3].Value = string.Join("", kvp.Value.Parameters.Select(p => $"{p}B;"));
                r.Cells[4].Value = kvp.Value.Description;
                index++;

            }
        }

        private void startSearchButtonScripts_Click(object sender, EventArgs e) {
            StartSearch(scriptcmdDataGridView, scriptcmdSearchTextBox, scriptRadioButtons);
        }
        private void startSearchButtonActions_Click(object sender, EventArgs e) {
            StartSearch(actionDataGridView, actioncmdSearchTextBox, actionRadioButtons);
        }
        private void StartSearch(DataGridView table, TextBox searchBox, List<RadioButton> rbl) {
            try {
                if (rbl[0].Checked) { //Contains
                    scanAllRows(table,
                        (x) => x.Value.ToString().IndexOf(searchBox.Text, StringComparison.InvariantCultureIgnoreCase) >= 0);
                } else if (rbl[1].Checked) { //StartsWith
                    scanAllRows(table, 
                        (x) => x.Value.ToString().StartsWith(searchBox.Text, StringComparison.InvariantCultureIgnoreCase));
                } else if (rbl[2].Checked) { //Exact Match
                    scanAllRows(table,
                        (x) => x.Value.ToString().IgnoreCaseEquals(searchBox.Text));
                }
            } catch (OperationCanceledException) {
                table.ClearSelection();
                table.FirstDisplayedScrollingRowIndex = currentrow.Index;
                currentrow.Selected = true;
                return;
            }
        }

        private void scanAllRows(DataGridView table, Func<DataGridViewCell, bool> function) {
            for (int i = 0; i < table.Rows.Count; i++) {
                currentrow = table.Rows[i];

                if (currentrow.Cells[1].Value == null) {
                    continue;
                }
                try {
                    if (function(currentrow.Cells[1])) { //Cancel research when found
                        throw new OperationCanceledException();
                    }
                } catch (NullReferenceException) { }
            }
        }

        private void scriptcmdSearchTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                StartSearch(scriptcmdDataGridView, scriptcmdSearchTextBox, scriptRadioButtons);
            }
        }

        private void actioncmdSearchTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                StartSearch(actionDataGridView, actioncmdSearchTextBox, actionRadioButtons);
            }
        }

        private void exportScrcmdButton_Click(object sender, EventArgs e)
        {
            // Export json of current selected scrcmd.
        }

        private void manageScrcmdsButton_Click(object sender, EventArgs e)
        {
            using (CustomScrcmdManager editor = new CustomScrcmdManager())
                editor.ShowDialog();
        }

        private void useGameDefaultScrcmdButton_Click(object sender, EventArgs e)
        {
            customScrcmdSelector.SelectedIndex = -1;
        }

        private void customScrcmdSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var customScrcmds = CustomScrcmdManager.LoadSettings();
            int selectedIndex = customScrcmdSelector.SelectedIndex;
            if (selectedIndex >= 0)
            {
                scriptDatabaseJSON.SelectedScrcmd = customScrcmds.ElementAt(selectedIndex).JsonPath;
            } else
            {
                scriptDatabaseJSON.SelectedScrcmd = null;
            }
            

            InitializeTables();
        }
    }
}
