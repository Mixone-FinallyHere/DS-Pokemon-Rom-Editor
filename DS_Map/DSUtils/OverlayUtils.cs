﻿using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static DSPRE.RomInfo;

namespace DSPRE {
    public static class OverlayUtils {

        public static string GetPath(int overlayNumber)
        {
            return $"{overlayPath}\\ov{overlayNumber:D3}.bin";
        }

        public static class OverlayTable {

            private static List<OverlayEntry> Overlays = new List<OverlayEntry>();

            public static void LoadOverlayTable()
            {
                var deserializer = new DeserializerBuilder()
                   .WithNamingConvention(UnderscoredNamingConvention.Instance)
                   .Build();

                var reader = new StreamReader(RomInfo.overlayTablePath);
                var data = deserializer.Deserialize<OverlayFile>(reader);
                Overlays = data.overlays;
            }

            // If marked as compressed overlay will automatically be recompressed when ROM is built
            public static bool IsCompressed(int ovNumber)
            {
                return Overlays[ovNumber].compressed;
            }
            // Can set overlay as uncompressed here to disabled recompression
            public static void SetCompressed(int ovNumber, bool compressStatus) {
                Overlays[ovNumber].compressed = compressStatus;
            }

            public static uint GetRAMAddress(int ovNumber) {
                return Overlays[ovNumber].base_address;
            }
            public static long GetUncompressedSize(int ovNumber) {
                return new FileInfo($@"{overlayPath}/ov{ovNumber:D3}.bin").Length;
            }
            public static string GetFileName(int ovNumber) {
                return Overlays[ovNumber].file_name;
            }

            /**
            * Gets number of overlays
            **/
            public static int GetNumberOfOverlays() {
                return Overlays.Count;
            }
        }

        public class OverlayEntry
        {
            public int id { get; set; }
            public uint base_address { get; set; }
            public uint code_size { get; set; }
            public uint bss_size { get; set; }
            public uint ctor_start { get; set; }
            public uint ctor_end { get; set; }
            public uint file_id { get; set; }
            public bool compressed { get; set; }
            public bool signed { get; set; }
            public string file_name { get; set; }

        }

        public class OverlayFile
        {
            public bool table_signed { get; set; }
            public List<OverlayEntry> overlays { get; set; }
        }

    }
}