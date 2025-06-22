using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DSPRE.Resources
{

    public class ScrcmdDatabase
    {
        public Dictionary<string, Command> Scrcmd { get; set; }
        public Dictionary<string, string> Movements { get; set; }
        public Dictionary<string, string> ComparisonOperators { get; set; }
        public Dictionary<string, string> SpecialOverworlds { get; set; }
        public Dictionary<string, string> OverworldDirections { get; set; }
}

    public class Command
    {
        public string Name { get; set; }
        public List<int> Parameters { get; set; }
        public string Description { get; set; }
    }

    public class ScriptDatabaseJSON
    {

        public string SelectedScrcmd { get; set; }

        public  ScrcmdDatabase GetDatabaseFromJSON(string json = "")
        {
            var jsonText = "";
            if(json == null)
            {
                switch (RomInfo.gameFamily)
                {
                    case RomInfo.GameFamilies.HGSS:
                        json = "Resources/DPPtScrcmd.json";
                        break;
                    case RomInfo.GameFamilies.DP:
                        json = "Resources/DPPtScrcmd.json";
                        break;
                    case RomInfo.GameFamilies.Plat:
                        json = "Resources/DPPtScrcmd.json";
                        break;

                    default:
                        json = "Resources/DPPtScrcmd.json";
                        break;

                }
            }
            
            jsonText = File.ReadAllText(json);

            SelectedScrcmd = json;
            
            return JsonConvert.DeserializeObject<ScrcmdDatabase>(jsonText);

        }

    }
}
