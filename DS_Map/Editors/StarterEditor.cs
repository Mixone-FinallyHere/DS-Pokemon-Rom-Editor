using DSPRE.Resources;
using DSPRE.ROMFiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DSPRE.DSUtils;

namespace DSPRE.Editors {
    public partial class StarterEditor : Form {

        private readonly string[] pokenames;
        private readonly List<PictureBox> pokePics;
        private TextArchive textFile;
        private TextArchive pokeFamFile;
        private List<PokemonPersonalData> starterPersonalData;
        private int starterMessageNum;

        private int[] starters = { 0, 0, 0 };

        public StarterEditor() {
            InitializeComponent();
            pokePics = new List<PictureBox> { picStarter1, picStarter2, picStarter3 };
            int count = RomInfo.GetPersonalFilesCount();
            pokenames = RomInfo.GetPokemonNames();
            List<string> fileNames = new List<string>(count);
            fileNames.AddRange(pokenames);

            for (int i = 0; i < count - pokenames.Length; i++) {
                PokeDatabase.PersonalData.PersonalExtraFiles extraEntry = PokeDatabase.PersonalData.personalExtraFiles[i];
                fileNames.Add(fileNames[extraEntry.monId] + " - " + extraEntry.description);
            }

            chosenStarter1.Items.AddRange(fileNames.ToArray());
            chosenStarter2.Items.AddRange(fileNames.ToArray());
            chosenStarter3.Items.AddRange(fileNames.ToArray());

            switch (RomInfo.gameFamily) {
                case RomInfo.GameFamilies.DP:
                    starterMessageNum = 320;
                    break;
                case RomInfo.GameFamilies.Plat:
                    starterMessageNum = 360;
                    break;
                default: // HGSS
                    starterMessageNum = 190;
                    break;
            }

            textFile = new TextArchive(starterMessageNum);
            InitializePokemonFamilyText();
            InitializeStarters();
        }

        private void InitializePokemonFamilyText() {
            switch (RomInfo.gameFamily) {
                case RomInfo.GameFamilies.DP:
                    pokeFamFile = new TextArchive(621);
                    break;
                case RomInfo.GameFamilies.Plat:
                    pokeFamFile = new TextArchive(711);
                    break;
                default: // HGSS
                    pokeFamFile = new TextArchive(816);
                    break;
            }
        }

        private void InitializeStarters() {
            switch (RomInfo.gameFamily) {
                case RomInfo.GameFamilies.HGSS:
                    starters[0] = BitConverter.ToInt32(ARM9.ReadBytes(RomInfo.starterOffsets[0], 4), 0);
                    starters[1] = BitConverter.ToInt32(ARM9.ReadBytes(RomInfo.starterOffsets[1], 4), 0);
                    starters[2] = BitConverter.ToInt32(ARM9.ReadBytes(RomInfo.starterOffsets[2], 4), 0);
                    break;
                default: // DPPt
                    if (OverlayUtils.IsCompressed(78)) {
                        OverlayUtils.Decompress(78);
                    }
                    using (DSUtils.EasyReader f = new EasyReader(OverlayUtils.GetPath(78), RomInfo.starterOffsets[0])) {
                        starters[0] = (int)f.ReadUInt32();
                        starters[1] = (int)f.ReadUInt32();
                        starters[2] = (int)f.ReadUInt32();
                    }
                    break;
            }
            starterPersonalData = new List<PokemonPersonalData> { new PokemonPersonalData(starters[0]), new PokemonPersonalData(starters[1]), new PokemonPersonalData(starters[2]) };
            chosenStarter1.SelectedIndex = starters[0];
            chosenStarter2.SelectedIndex = starters[1];
            chosenStarter3.SelectedIndex = starters[2];
        }

        private void SetStarterSprites() {
            switch (RomInfo.gameFamily) {
                case RomInfo.GameFamilies.Plat:
                    for (int i = 0; i < starters.Length; i++) {
                        int offset = 0x690 + i * 14;
                        using (DSUtils.EasyWriter f = new EasyWriter(OverlayUtils.GetPath(78), offset)) {
                            f.Write(starters[0]);
                            f.Write(starters[1]);
                            f.Write(starters[2]);
                        }
                    }
                    break;
                default: // DP
                    break;
            }
            
        }

        private void SetStarterText(int starter) {
            switch (RomInfo.gameFamily) {
                case RomInfo.GameFamilies.DP:
                    textFile.messages[1 + starter] = pokeFamFile.messages[starters[starter]] + " " + pokenames[starters[starter]] + "!\\nWill you take this Pokémon?";
                    break;
                case RomInfo.GameFamilies.Plat:
                    textFile.messages[1 + starter] = pokeFamFile.messages[starters[starter]] + " " + pokenames[starters[starter]] + "!\\nWill you take this Pokémon?";
                    break;
                default: // HGSS
                    if (starterPersonalData[starter].type1 != starterPersonalData[starter].type2) {
                        textFile.messages[1 + starter] = "Professor Elm: So, you like " + pokenames[starters[starter]] + ",\\nthe "
                            + Enum.GetName(typeof(PokemonType), starterPersonalData[starter].type1)
                            + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[starter].type2)
                            + " type Pokémon?";
                        textFile.messages[4 + starter] = pokenames[starters[starter]] + ", the "
                            + Enum.GetName(typeof(PokemonType), starterPersonalData[starter].type1)
                            + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[starter].type2)
                            + " type\\nPokémon is in this Poké Ball!";
                    } else {
                        textFile.messages[1 + starter] = "Professor Elm: So, you like " + pokenames[starters[starter]] + ",\\nthe "
                            + Enum.GetName(typeof(PokemonType), starterPersonalData[starter].type1) + " type Pokémon?";
                        textFile.messages[4 + starter] = pokenames[starters[starter]] + ", the "
                            + Enum.GetName(typeof(PokemonType), starterPersonalData[starter].type1) + " type Pokémon, is\\nin this Poké Ball!";
                    }
                    break;
            }
        }

        private void ShowPokemonPic(int starter) {
            PictureBox pb = pokePics[starter];
            int species = starters[starter] > 0 ? starters[starter] : 0;
            pokePics[starter].Image = DSUtils.GetPokePic(species, pb.Width, pb.Height);
        }

        private void chosenStarter1_SelectedIndexChanged(object sender, EventArgs e) {
            starters[0] = chosenStarter1.SelectedIndex;
            starterPersonalData[0] = new PokemonPersonalData(starters[0]);
            SetStarterText(0);
            ShowPokemonPic(0);
        }

        private void chosenStarter2_SelectedIndexChanged(object sender, EventArgs e) {
            starters[1] = chosenStarter2.SelectedIndex;
            starterPersonalData[1] = new PokemonPersonalData(starters[1]);
            SetStarterText(1);
            ShowPokemonPic(1);
        }

        private void chosenStarter3_SelectedIndexChanged(object sender, EventArgs e) {
            starters[2] = chosenStarter3.SelectedIndex;
            starterPersonalData[2] = new PokemonPersonalData(starters[2]);
            SetStarterText(2);
            ShowPokemonPic(2);
        }

        private void button1_Click(object sender, EventArgs e) {
            // TODO : Add confirmation message
            switch (RomInfo.gameFamily) {
                case RomInfo.GameFamilies.DP:
                    textFile.SaveToFileDefaultDir(320, showSuccessMessage: false);
                    using (DSUtils.EasyWriter f = new EasyWriter(OverlayUtils.GetPath(78), RomInfo.starterOffsets[0])) {
                        f.Write(starters[0]);
                        f.Write(starters[1]);
                        f.Write(starters[2]);
                    }
                    break;
                case RomInfo.GameFamilies.Plat:
                    textFile.SaveToFileDefaultDir(360, showSuccessMessage: false);
                    using (DSUtils.EasyWriter f = new EasyWriter(OverlayUtils.GetPath(78), RomInfo.starterOffsets[0])) {
                        f.Write(starters[0]);
                        f.Write(starters[1]);
                        f.Write(starters[2]);
                    }
                    break;
                default: // HGSS
                    textFile.SaveToFileDefaultDir(190, showSuccessMessage: false);
                    ARM9.WriteBytes(BitConverter.GetBytes(starters[0]), RomInfo.starterOffsets[0]);
                    ARM9.WriteBytes(BitConverter.GetBytes(starters[1]), RomInfo.starterOffsets[1]);
                    ARM9.WriteBytes(BitConverter.GetBytes(starters[2]), RomInfo.starterOffsets[2]);
                    break;
            }
            MessageBox.Show("New starters saved successfully.", "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
