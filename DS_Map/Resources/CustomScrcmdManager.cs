using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DSPRE.Resources
{
    public partial class CustomScrcmdManager : Form
    {

        private const string FilePath = "Resources/customScrcmd.json";

        public class CustomScrcmdSetting
        {
            public string Name { get; set; }
            public string JsonPath { get; set; }
        }

        public CustomScrcmdManager()
        {
            InitializeComponent();
            UpdateDataGrid(CustomScrcmdDataGrid);
        }

        private void UpdateDataGrid(DataGridView grid)
        {
            var settings = LoadSettings();

            grid.Rows.Clear();

            // Populate rows
            foreach (var setting in settings)
            {
                grid.Rows.Add(setting.Name, setting.JsonPath);
            }
        }

        public static List<CustomScrcmdSetting> LoadSettings()
        {
            if (!File.Exists(FilePath)) return new List<CustomScrcmdSetting>();
            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<CustomScrcmdSetting>>(json);
        }

        public static void SaveSettings(List<CustomScrcmdSetting> settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static void AddSetting(CustomScrcmdSetting newSetting)
        {
            var settings = LoadSettings();
            settings.Add(newSetting);
            SaveSettings(settings);
        }

        private void AddScrcmdButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select a file";
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (dialog.ShowDialog() == DialogResult.OK) { 
                    AddSetting(new CustomScrcmdSetting
                    {
                        Name = "test",
                        JsonPath = dialog.FileName
                    });

                    UpdateDataGrid(CustomScrcmdDataGrid);
                } else
                {
                    MessageBox.Show("Something wrong happened.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }

        private void SaveScrcmdButton_Click(object sender, EventArgs e)
        {
            var settings = new List<CustomScrcmdSetting>();

            for (int i = 0; i < CustomScrcmdDataGrid.Rows.Count; i++)
            {
                var currentrow = CustomScrcmdDataGrid.Rows[i];
                var setting = new CustomScrcmdSetting
                    {
                    Name = currentrow.Cells[0].Value.ToString(),
                    JsonPath = currentrow.Cells[1].Value.ToString()
                    };
                settings.Add(setting);
            }

            SaveSettings(settings);
        }

        private void RemoveScrcmdButton_Click(object sender, EventArgs e)
        {
            if (CustomScrcmdDataGrid.SelectedRows.Count == 0) return;

            var row = CustomScrcmdDataGrid.SelectedRows[0];

            CustomScrcmdDataGrid.Rows.RemoveAt(row.Index);
            
        }
    }
}
