using DSPRE.Editors;
using DSPRE.MessageEnc;
using DSPRE.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using static DSPRE.RomInfo;

namespace DSPRE.ROMFiles
{
    /// <summary>
    /// Class to store message data from DS Pokémon games
    /// </summary>
    public class TextArchive : RomFile
    {
        #region Fields

        public int ID { get;}
        public List<string> messages;
        private UInt16 key = 0;

        #endregion Fields

        #region Constructors (1)

        public TextArchive(int ID, List<string> msg = null)
        {
            this.ID = ID;

            if (msg != null)
            {
                messages = msg;
                return;
            }

            // First try to read from plain text file if it exists
            if (TryReadPlainTextFile())
            {
                return;
            }

            // If not, extract from the .bin file
            if (!ReadFromBinFile())
            {
                MessageBox.Show("Failed to read messages from .bin file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }


        #endregion Constructors (1)

        #region Methods (2)

        public static (string binPath, string txtPath) GetFilePaths(int ID)
        {
            string baseDir = gameDirs[DirNames.textArchives].unpackedDir;
            string binPath = Path.Combine(baseDir, $"{ID:D4}");
            string expandedDir = Path.Combine(RomInfo.workDir, "expanded", "textArchives");
            string txtPath = Path.Combine(expandedDir, $"{ID:D4}.txt");
            return (binPath, txtPath);
        }

        private bool TryReadPlainTextFile()
        {
            string txtPath = GetFilePaths(ID).txtPath;
            string binPath = GetFilePaths(ID).binPath;

            if (!File.Exists(txtPath))
            {
                return false;
            }

            if (File.GetLastWriteTimeUtc(txtPath) < File.GetLastWriteTimeUtc(binPath))
            {
                //AppLogger.Debug($"Skipped expanding {ID:D4} — already up to date.");
                return false;
            }

            try
            {
                string[] lines = File.ReadAllLines(txtPath);
                messages = lines.ToList();
                return true;
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Error reading text file {txtPath}: {ex.Message}. Bin file will be reextracted.");
                return false;
            }
        }

        private bool ReadFromBinFile()
        {
            string binPath = GetFilePaths(ID).binPath;
            if (!File.Exists(binPath))
            {
                MessageBox.Show($"The .bin file for Text Archive ID {ID:D4} does not exist at the expected path: {binPath}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                using (FileStream fs = new FileStream(binPath, FileMode.Open, FileAccess.Read))
                {
                    messages = TextConverter.ReadMessageFromStream(fs, out key);
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading .bin file {binPath}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, messages);
        }

        public override byte[] ToByteArray()
        {
            Stream stream = new MemoryStream();
            TextConverter.WriteMessagesToStream(ref stream, messages, key);

            return ((MemoryStream)stream).ToArray();
        }

        public void SaveToExpandedDir(int IDtoReplace, bool showSuccessMessage = true)
        {
            string baseDir = gameDirs[DirNames.textArchives].unpackedDir;
            string expandedDir = Path.Combine(RomInfo.workDir, "expanded", "textArchives");

            if (!Directory.Exists(expandedDir))
            {
                Directory.CreateDirectory(expandedDir);
            }

            string expandedPath = GetFilePaths(IDtoReplace).txtPath;

            var utf8WithoutBom = new UTF8Encoding(false);

            File.WriteAllText(expandedPath, string.Join(Environment.NewLine, messages), utf8WithoutBom);
        }

        public void SaveToDefaultDir(int IDtoReplace, bool showSuccessMessage = true)
        {
            SaveToFileDefaultDir(DirNames.textArchives, IDtoReplace, showSuccessMessage);
        }

        public void SaveToFileExplorePath(string suggestedFileName, bool showSuccessMessage = true)
        {
            SaveToFileExplorePath("Gen IV Text Archive", "msg", suggestedFileName, showSuccessMessage);
        }

        #endregion Methods (2)
    }
}