using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DSPRE.RomInfo;

namespace DSPRE.ROMFiles {
    public enum EvoMethod {
        EVO_NONE = 0,
        EVO_FRIENDSHIP,
        EVO_FRIENDSHIP_DAY,
        EVO_FRIENDSHIP_NIGHT,
        EVO_LEVEL,
        EVO_TRADE,
        EVO_TRADE_ITEM,
        EVO_STONE,
        EVO_LEVEL_ATK_GT_DEF,
        EVO_LEVEL_ATK_EQ_DEF,
        EVO_LEVEL_ATK_LT_DEF,
        EVO_LEVEL_PID_LO,
        EVO_LEVEL_PID_HI,
        EVO_LEVEL_NINJASK,
        EVO_LEVEL_SHEDINJA,
        EVO_BEAUTY,
        EVO_STONE_MALE,
        EVO_STONE_FEMALE,
        EVO_ITEM_DAY,
        EVO_ITEM_NIGHT,
        EVO_HAS_MOVE,
        EVO_OTHER_PARTY_MON,
        EVO_LEVEL_MALE,
        EVO_LEVEL_FEMALE,
        EVO_CORONET,
        EVO_ETERNA,
        EVO_ROUTE217,
        EVO_LEVEL_DAY,
        EVO_LEVEL_NIGHT,
        EVO_LEVEL_DUSK,
        EVO_LEVEL_RAIN,
        EVO_HAS_MOVE_TYPE,
    }

    public struct Evolution {
        public EvoMethod method;
        public ushort param;
        public ushort target;
    }    

    public class PokemonEvoData : RomFile {
        public Evolution[] evolutions = new Evolution[7];

        public PokemonEvoData(Stream stream) {
            using (BinaryReader reader = new BinaryReader(stream)) {
                // Deserialize the object from binary                
                for (int i = 0; i < evolutions.Length; i++) {
                    evolutions[i] = new Evolution {
                        method = (EvoMethod)reader.ReadUInt16(),
                        param = reader.ReadUInt16(),
                        target = reader.ReadUInt16()
                    };
                }
            }
        }

        public PokemonEvoData(int ID) : this(new FileStream(RomInfo.gameDirs[DirNames.evoData].unpackedDir + "\\" + ID.ToString("D4"), FileMode.Open)) { }

        public override byte[] ToByteArray() {
            using (MemoryStream stream = new MemoryStream()) {
                using (BinaryWriter writer = new BinaryWriter(stream)) {
                    // Serialize the object to binary                    
                    for (int i = 0; i < evolutions.Length; i++) {
                        writer.Write((ushort)evolutions[i].method);
                        writer.Write(evolutions[i].param);
                        writer.Write(evolutions[i].target);
                    }
                    writer.Write((uint)0);                    
                }
                return stream.ToArray();
            }
        }

        public void SaveToFileDefaultDir(int IDtoReplace, bool showSuccessMessage = true) {
            SaveToFileDefaultDir(DirNames.evoData, IDtoReplace, showSuccessMessage);
        }

        public void SaveToFileExplorePath(string suggestedFileName, bool showSuccessMessage = true) {
            SaveToFileExplorePath("Gen IV Pokémon Evolution data", "bin", suggestedFileName, showSuccessMessage);
        }
    }
}
