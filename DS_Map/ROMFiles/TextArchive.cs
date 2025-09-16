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
        #region Fields (2)

        public List<string> messages;
        public int initialKey;

        #endregion Fields (2)

        #region Constructors (1)

        public TextArchive(int ID, List<string> msg = null, bool discardLines = false)
        {
            if (msg != null)
            {
                messages = msg;
                return;
            }

            string baseDir = gameDirs[DirNames.textArchives].unpackedDir;
            string expandedPath = Path.Combine(baseDir, "expanded", $"{ID:D4}.txt");

            TextEditor.ExpandTextFile(ID);

            string rawText = File.ReadAllText(expandedPath);
            // Replace all possible line endings with \r\n because f me thats why
            rawText = rawText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n\n", "\n").Replace("\n", "\r\n");
            messages = rawText.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            // Get rid of trailing empty line if it exists
            if (messages.Count > 0 && messages[messages.Count - 1] == "")
            {
                messages.RemoveAt(messages.Count - 1);
            }
        }

        #endregion Constructors (1)

        #region Methods (2)

        public override string ToString()
        {
            return string.Join(Environment.NewLine, messages);
        }

        public override byte[] ToByteArray()
        {
            string tempTxt = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            string tempMsg = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".msg");

            try
            {
                // Write messages in the format expected by msgenc (text\n\ntext\n\n...)
                File.WriteAllText(tempTxt, string.Join(Environment.NewLine + Environment.NewLine, messages));

                string toolPath = Path.Combine(Application.StartupPath, "Tools", "msgenc.exe");
                string charmapPath = Path.Combine("Tools", "charmap.txt");

                Process process = new Process();
                process.StartInfo.FileName = toolPath;
                process.StartInfo.Arguments = $"-e -c \"{charmapPath}\" \"{tempTxt}\" \"{tempMsg}\"";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception("msgenc.exe failed to encode the text file.");
                }

                return File.ReadAllBytes(tempMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during encoding: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (File.Exists(tempTxt)) File.Delete(tempTxt);
                if (File.Exists(tempMsg)) File.Delete(tempMsg);
            }
        }

        public void SaveToFileDefaultDir(int IDtoReplace, bool showSuccessMessage = true)
        {
            string baseDir = gameDirs[DirNames.textArchives].unpackedDir;
            string expandedDir = Path.Combine(baseDir, "expanded");
            string path = Path.Combine(expandedDir, $"{IDtoReplace:D4}.txt");

            var utf8WithoutBom = new UTF8Encoding(false);
            File.WriteAllText(path, string.Join(Environment.NewLine, messages), utf8WithoutBom);
            bool success = TextEditor.CompressTextFile(IDtoReplace);
            if (showSuccessMessage && success)
            {
                MessageBox.Show("Saved successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void SaveToFileExplorePath(string suggestedFileName, bool showSuccessMessage = true)
        {
            SaveToFileExplorePath("Gen IV Text Archive", "msg", suggestedFileName, showSuccessMessage);
        }

        #endregion Methods (2)
    }
}