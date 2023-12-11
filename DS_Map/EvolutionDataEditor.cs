using DSPRE.Resources;
using DSPRE.ROMFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSPRE {
    public partial class EvolutionDataEditor : Form {
        private bool disableHandlers = false;

        private readonly string[] fileNames;
        private readonly string[] pokeNames;
        private readonly string[] itemNames;

        private int currentLoadedId = 0;
        private PokemonEvoData currentLoadedFile = null;

        private static bool dirty = false;
        private static readonly string formName = "Evolution Data Editor";

        public EvolutionDataEditor(string[] itemnames) {
            this.fileNames = RomInfo.GetPokemonNames().ToArray(); ;

            InitializeComponent();

            disableHandlers = true;

            this.itemNames = itemnames;

            method1InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));
            method2InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));
            method3InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));
            method4InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));
            method5InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));
            method6InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));
            method7InputComboBox.DataSource = Enum.GetValues(typeof(EvoMethod));

            /* ---------------- */
            int count = RomInfo.GetPersonalFilesCount();
            this.pokeNames = RomInfo.GetPokemonNames();
            List<string> fileNames = new List<string>(count);
            fileNames.AddRange(pokeNames);

            for (int i = 0; i < count - pokeNames.Length; i++) {
                PokeDatabase.PersonalData.PersonalExtraFiles extraEntry = PokeDatabase.PersonalData.personalExtraFiles[i];
                fileNames.Add(fileNames[extraEntry.monId] + " - " + extraEntry.description);
            }


            this.fileNames = fileNames.ToArray();
            monNumberNumericUpDown.Maximum = fileNames.Count - 1;
            pokemonNameInputComboBox.Items.AddRange(this.fileNames);
            target1InputComboBox.Items.AddRange(this.fileNames);
            target2InputComboBox.Items.AddRange(this.fileNames);
            target3InputComboBox.Items.AddRange(this.fileNames);
            target4InputComboBox.Items.AddRange(this.fileNames);
            target5InputComboBox.Items.AddRange(this.fileNames);
            target6InputComboBox.Items.AddRange(this.fileNames);
            target7InputComboBox.Items.AddRange(this.fileNames);

            /* ---------------- */

            disableHandlers = false;

            pokemonNameInputComboBox.SelectedIndex = 1;
        }

        private bool CheckDiscardChanges() {
            if (!dirty) {
                return true;
            }

            DialogResult res = MessageBox.Show("There are unsaved changes to the current Pokémon data.\nDiscard and proceed?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res.Equals(DialogResult.Yes)) {
                return true;
            }

            monNumberNumericUpDown.Value = currentLoadedId;
            pokemonNameInputComboBox.SelectedIndex = currentLoadedId;


            return false;
        }

        private void pokemonNameInputComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (disableHandlers) {
                return;
            }

            disableHandlers = true;
            if (CheckDiscardChanges()) {
                int newNumber = pokemonNameInputComboBox.SelectedIndex;
                monNumberNumericUpDown.Value = newNumber;
                ChangeLoadedFile(newNumber);

            }
            disableHandlers = false;
        }

        private void monNumberNumericUpDown_ValueChanged(object sender, EventArgs e) {
            if (disableHandlers) {
                return;
            }

            disableHandlers = true;
            if (CheckDiscardChanges()) {

                int newNumber = (int)monNumberNumericUpDown.Value;
                pokemonNameInputComboBox.SelectedIndex = newNumber;
                ChangeLoadedFile(newNumber);

            }
            disableHandlers = false;
        }

        private void ChangeLoadedFile(int toLoad) {
            currentLoadedId = toLoad;
            currentLoadedFile = new PokemonEvoData(currentLoadedId);

            method1InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[0].method;
            target1InputComboBox.SelectedIndex = currentLoadedFile.evolutions[0].target;
            param1NumericUpDown.Value = currentLoadedFile.evolutions[0].param;
            method2InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[1].method;
            target2InputComboBox.SelectedIndex = currentLoadedFile.evolutions[1].target;
            param2NumericUpDown.Value = currentLoadedFile.evolutions[1].param;
            method3InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[2].method;
            target3InputComboBox.SelectedIndex = currentLoadedFile.evolutions[2].target;
            param3NumericUpDown.Value = currentLoadedFile.evolutions[2].param;
            method4InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[3].method;
            target4InputComboBox.SelectedIndex = currentLoadedFile.evolutions[3].target;
            param4NumericUpDown.Value = currentLoadedFile.evolutions[3].param;
            method5InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[4].method;
            target5InputComboBox.SelectedIndex = currentLoadedFile.evolutions[4].target;
            param5NumericUpDown.Value = currentLoadedFile.evolutions[4].param;
            method6InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[5].method;
            target6InputComboBox.SelectedIndex = currentLoadedFile.evolutions[5].target;
            param6NumericUpDown.Value = currentLoadedFile.evolutions[5].param;
            method7InputComboBox.SelectedIndex = (int)currentLoadedFile.evolutions[6].method;
            target7InputComboBox.SelectedIndex = currentLoadedFile.evolutions[6].target;
            param7NumericUpDown.Value = currentLoadedFile.evolutions[6].param;

            int excess = toLoad - pokeNames.Length;
            if (excess >= 0) {
                toLoad = PokeDatabase.PersonalData.personalExtraFiles[excess].iconId;
            }
            pokemonPictureBox.Image = DSUtils.GetPokePic(toLoad, pokemonPictureBox.Width, pokemonPictureBox.Height);

            setDirty(false);
        }

        private void setDirty(bool status) {
            if (status) {
                dirty = true;
                this.Text = formName + "*";
            } else {
                dirty = false;
                this.Text = formName;
            }
        }

        private void param1NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[0].param = (ushort)param1NumericUpDown.Value;
            if (currentLoadedFile.evolutions[0].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[0].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[0].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[0].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[0].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[0].method == EvoMethod.EVO_STONE) {
                param1Label.Text = "Item " + itemNames[(int)param1NumericUpDown.Value];
                if (currentLoadedFile.evolutions[0].method == EvoMethod.EVO_ITEM_DAY) {
                    param1Label.Text += " during Day";
                } 
                else if (currentLoadedFile.evolutions[0].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param1Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[0].method == EvoMethod.EVO_TRADE_ITEM) {
                    param1Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[0].method == EvoMethod.EVO_STONE_FEMALE) {
                    param1Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[0].method == EvoMethod.EVO_STONE_MALE) {
                    param1Label.Text += " if Male";
                }
            }                
            else param1Label.Text = "";
            setDirty(true);
        }

        private void param2NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[1].param = (ushort)param2NumericUpDown.Value;
            if (currentLoadedFile.evolutions[1].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[1].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[1].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[1].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[1].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[1].method == EvoMethod.EVO_STONE) {
                param2Label.Text = "Item " + itemNames[(int)param2NumericUpDown.Value];
                if (currentLoadedFile.evolutions[1].method == EvoMethod.EVO_ITEM_DAY) {
                    param2Label.Text += " during Day";
                } else if (currentLoadedFile.evolutions[1].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param2Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[1].method == EvoMethod.EVO_TRADE_ITEM) {
                    param2Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[1].method == EvoMethod.EVO_STONE_FEMALE) {
                    param2Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[1].method == EvoMethod.EVO_STONE_MALE) {
                    param2Label.Text += " if Male";
                }
            } else param2Label.Text = "";
            setDirty(true);
        }

        private void param3NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[2].param = (ushort)param3NumericUpDown.Value;
            if (currentLoadedFile.evolutions[2].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[2].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[2].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[2].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[2].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[2].method == EvoMethod.EVO_STONE) {
                param3Label.Text = "Item " + itemNames[(int)param3NumericUpDown.Value];
                if (currentLoadedFile.evolutions[2].method == EvoMethod.EVO_ITEM_DAY) {
                    param3Label.Text += " during Day";
                } else if (currentLoadedFile.evolutions[2].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param3Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[2].method == EvoMethod.EVO_TRADE_ITEM) {
                    param3Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[2].method == EvoMethod.EVO_STONE_FEMALE) {
                    param3Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[2].method == EvoMethod.EVO_STONE_MALE) {
                    param3Label.Text += " if Male";
                }
            } else param3Label.Text = "";
            setDirty(true);
        }

        private void param4NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[3].param = (ushort)param4NumericUpDown.Value;
            if (currentLoadedFile.evolutions[3].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[3].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[3].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[3].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[3].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[3].method == EvoMethod.EVO_STONE) {
                param4Label.Text = "Item " + itemNames[(int)param4NumericUpDown.Value];
                if (currentLoadedFile.evolutions[3].method == EvoMethod.EVO_ITEM_DAY) {
                    param4Label.Text += " during Day";
                } else if (currentLoadedFile.evolutions[3].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param4Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[3].method == EvoMethod.EVO_TRADE_ITEM) {
                    param4Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[3].method == EvoMethod.EVO_STONE_FEMALE) {
                    param4Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[3].method == EvoMethod.EVO_STONE_MALE) {
                    param4Label.Text += " if Male";
                }
            } else param4Label.Text = "";
            setDirty(true);
        }

        private void param5NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[4].param = (ushort)param5NumericUpDown.Value;
            if (currentLoadedFile.evolutions[4].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[4].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[4].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[4].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[4].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[4].method == EvoMethod.EVO_STONE) {
                param5Label.Text = "Item " + itemNames[(int)param5NumericUpDown.Value];
                if (currentLoadedFile.evolutions[4].method == EvoMethod.EVO_ITEM_DAY) {
                    param5Label.Text += " during Day";
                } else if (currentLoadedFile.evolutions[4].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param5Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[4].method == EvoMethod.EVO_TRADE_ITEM) {
                    param5Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[4].method == EvoMethod.EVO_STONE_FEMALE) {
                    param5Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[4].method == EvoMethod.EVO_STONE_MALE) {
                    param5Label.Text += " if Male";
                }
            } else param5Label.Text = "";
            setDirty(true);
        }

        private void param6NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[5].param = (ushort)param6NumericUpDown.Value;
            if (currentLoadedFile.evolutions[5].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[5].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[5].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[5].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[5].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[5].method == EvoMethod.EVO_STONE) {
                param6Label.Text = "Item " + itemNames[(int)param6NumericUpDown.Value];
                if (currentLoadedFile.evolutions[5].method == EvoMethod.EVO_ITEM_DAY) {
                    param6Label.Text += " during Day";
                } else if (currentLoadedFile.evolutions[5].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param6Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[5].method == EvoMethod.EVO_TRADE_ITEM) {
                    param6Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[5].method == EvoMethod.EVO_STONE_FEMALE) {
                    param6Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[5].method == EvoMethod.EVO_STONE_MALE) {
                    param6Label.Text += " if Male";
                }
            } else param6Label.Text = "";
            setDirty(true);
        }

        private void param7NumericUpDown_ValueChanged(object sender, EventArgs e) {
            currentLoadedFile.evolutions[6].param = (ushort)param7NumericUpDown.Value;
            if (currentLoadedFile.evolutions[6].method == EvoMethod.EVO_ITEM_DAY ||
                currentLoadedFile.evolutions[6].method == EvoMethod.EVO_ITEM_NIGHT ||
                currentLoadedFile.evolutions[6].method == EvoMethod.EVO_TRADE_ITEM ||
                currentLoadedFile.evolutions[6].method == EvoMethod.EVO_STONE_MALE ||
                currentLoadedFile.evolutions[6].method == EvoMethod.EVO_STONE_FEMALE ||
                currentLoadedFile.evolutions[6].method == EvoMethod.EVO_STONE) {
                param7Label.Text = "Item " + itemNames[(int)param7NumericUpDown.Value];
                if (currentLoadedFile.evolutions[6].method == EvoMethod.EVO_ITEM_DAY) {
                    param7Label.Text += " during Day";
                } else if (currentLoadedFile.evolutions[6].method == EvoMethod.EVO_ITEM_NIGHT) {
                    param7Label.Text += " during Night";
                } else if (currentLoadedFile.evolutions[6].method == EvoMethod.EVO_TRADE_ITEM) {
                    param7Label.Text += " during Trade";
                } else if (currentLoadedFile.evolutions[6].method == EvoMethod.EVO_STONE_FEMALE) {
                    param7Label.Text += " if Female";
                } else if (currentLoadedFile.evolutions[6].method == EvoMethod.EVO_STONE_MALE) {
                    param7Label.Text += " if Male";
                }
            } else param7Label.Text = "";
            setDirty(true);
        }

        private void method1InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void method2InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void method3InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void method4InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void method5InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void method6InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void method7InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target1InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target2InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target3InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target4InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target5InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target6InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void target7InputComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }
}
