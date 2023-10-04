using System;
using System.Collections.Generic;

namespace LibNDSFormats.NSBMD {

    public class NSBMDPalette {
        public int color0;
        public RGBA[] paldata;
        public List<uint> palmatid = new List<uint>();
        public string palname = String.Empty;
        public UInt32 paloffset;
        public UInt32 palsize;
    }
}