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
        private List<PokemonPersonalData> starterPersonalData;

        private int[] starters = {0, 0 , 0};

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

            textFile = new TextArchive(190);
            InitializeStarters();            
        }

        private void InitializeStarters() {
            switch(RomInfo.gameFamily) {
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

        private void ShowPokemonPic(int starter) {
            PictureBox pb = pokePics[starter];
            int species = starters[starter] > 0 ? starters[starter] : 0;
            pokePics[starter].Image = DSUtils.GetPokePic(species, pb.Width, pb.Height);
        }

        private void chosenStarter1_SelectedIndexChanged(object sender, EventArgs e) {            
            starters[0] = chosenStarter1.SelectedIndex;
            starterPersonalData[0] = new PokemonPersonalData(starters[0]);
            if (starterPersonalData[0].type1 != starterPersonalData[0].type2) {
                textFile.messages[1] = "Professor Elm: So, you like " + pokenames[starters[0]] + ",\\nthe " 
                    + Enum.GetName(typeof(PokemonType), starterPersonalData[0].type1)
                    + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[0].type2)
                    + " type Pokémon?";
                textFile.messages[4] = pokenames[starters[0]] + ", the " 
                    + Enum.GetName(typeof(PokemonType), starterPersonalData[0].type1)
                    + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[0].type2)
                    + " type\\nPokémon is in this Poké Ball!";
            } else {
                textFile.messages[1] = "Professor Elm: So, you like " + pokenames[starters[0]] + ",\\nthe " + Enum.GetName(typeof(PokemonType), starterPersonalData[0].type1) + " type Pokémon?";
                textFile.messages[4] = pokenames[starters[0]] + ", the " + Enum.GetName(typeof(PokemonType), starterPersonalData[0].type1) + " type Pokémon, is\\nin this Poké Ball!";
            }
            
            ShowPokemonPic(0);
        }

        private void chosenStarter2_SelectedIndexChanged(object sender, EventArgs e) {            
            starters[1] = chosenStarter2.SelectedIndex;
            starterPersonalData[1] = new PokemonPersonalData(starters[1]);
            if (starterPersonalData[1].type1 != starterPersonalData[1].type2) {
                textFile.messages[2] = "Professor Elm: You'll take " + pokenames[starters[1]] + ",\\nthe " 
                    + Enum.GetName(typeof(PokemonType), starterPersonalData[1].type1)
                    + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[1].type2)
                    + " type Pokémon?";
                textFile.messages[5] = pokenames[starters[1]] + ", the "
                    + Enum.GetName(typeof(PokemonType), starterPersonalData[1].type1)
                    + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[1].type2)
                    + " type\\nPokémon is in this Poké Ball!";
            } else {
                textFile.messages[2] = "Professor Elm: You'll take " + pokenames[starters[1]] + ",\\nthe " + Enum.GetName(typeof(PokemonType), starterPersonalData[1].type1) + " type Pokémon?";
                textFile.messages[5] = pokenames[starters[1]] + ", the " + Enum.GetName(typeof(PokemonType), starterPersonalData[1].type1) + " type Pokémon, is\\nin this Poké Ball!";
            }
            ShowPokemonPic(1);
        }

        private void chosenStarter3_SelectedIndexChanged(object sender, EventArgs e) {            
            starters[2] = chosenStarter3.SelectedIndex;
            starterPersonalData[2] = new PokemonPersonalData(starters[2]);
            if (starterPersonalData[2].type1 != starterPersonalData[2].type2) {
                textFile.messages[3] = "Professor Elm: Do you want " + pokenames[starters[2]] + ",\\nthe " + 
                    Enum.GetName(typeof(PokemonType), starterPersonalData[2].type1)
                    + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[2].type2)
                    + " type Pokémon?";
                textFile.messages[6] = pokenames[starters[2]] + ", the " 
                    + Enum.GetName(typeof(PokemonType), starterPersonalData[2].type1)
                    + " and " + Enum.GetName(typeof(PokemonType), starterPersonalData[2].type2)
                    + " type\\nPokémon is in this Poké Ball!";
            } else {
                textFile.messages[3] = "Professor Elm: Do you want " + pokenames[starters[2]] + ",\\nthe " + Enum.GetName(typeof(PokemonType), starterPersonalData[2].type1) + " type Pokémon?";
                textFile.messages[6] = pokenames[starters[2]] + ", the " + Enum.GetName(typeof(PokemonType), starterPersonalData[2].type1) + " type Pokémon, is\\nin this Poké Ball!";
            }
            ShowPokemonPic(2);
        }

        private void button1_Click(object sender, EventArgs e) {
            // TODO : Add confirmation message
            textFile.SaveToFileDefaultDir(190, showSuccessMessage: false);
            ARM9.WriteBytes(BitConverter.GetBytes(starters[0]), RomInfo.starterOffsets[0]);
            ARM9.WriteBytes(BitConverter.GetBytes(starters[1]), RomInfo.starterOffsets[1]);
            ARM9.WriteBytes(BitConverter.GetBytes(starters[2]), RomInfo.starterOffsets[2]);
        }
    }
}
