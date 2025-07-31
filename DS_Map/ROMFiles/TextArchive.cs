using DSPRE.MessageEnc;
using DSPRE.Resources;
using System;
using System.Collections.Generic;
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

            if (!File.Exists(expandedPath))
            {
                throw new FileNotFoundException($"Expanded text file not found: {expandedPath}");
            }

            // Read lines and keep only every other line (e.g., even indices: 0, 2, 4, ...)
            messages = File.ReadLines(expandedPath)
                          .Where((line, index) => index % 2 == 0)
                          .ToList();

            foreach (string line in messages)
            {
                Console.WriteLine(line);
            }
        }

        #endregion Constructors (1)

        #region Methods (2)

        public int[] EncodeString(string currentMessage, int stringIndex, int stringSize)
        { // Converts string to hex characters
            ResourceManager GetByte = new ResourceManager("DSPRE.Resources.WriteText", Assembly.GetExecutingAssembly());

            int[] pokemonMessage = new int[stringSize - 1];
            var charArray = currentMessage.ToCharArray();
            int count = 0;
            try
            {
                for (int i = 0; i < currentMessage.Length; i++)
                {
                    if (charArray[i] == '\\')
                    {
                        if (charArray[i + 1] == 'r')
                        {
                            pokemonMessage[count] = 0x25BC;
                            i++;
                        }
                        else
                        {
                            if (charArray[i + 1] == 'n')
                            {
                                pokemonMessage[count] = 0xE000;
                                i++;
                            }
                            else
                            {
                                if (charArray[i + 1] == 'f')
                                {
                                    pokemonMessage[count] = 0x25BD;
                                    i++;
                                }
                                else
                                {
                                    if (charArray[i + 1] == 'v')
                                    {
                                        pokemonMessage[count] = 0xFFFE;
                                        count++;
                                        string characterID = ((char)charArray[i + 2]).ToString() + ((char)charArray[i + 3]).ToString() + ((char)charArray[i + 4]).ToString() + ((char)charArray[i + 5]).ToString();
                                        pokemonMessage[count] = (int)Convert.ToUInt32(characterID, 16);
                                        i += 5;
                                    }
                                    else
                                    {
                                        //This looks like it can be optimized
                                        if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '0')
                                        {
                                            pokemonMessage[count] = 0x0000;
                                            i += 5;
                                        }
                                        else
                                        {
                                            if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '1')
                                            {
                                                pokemonMessage[count] = 0x0001;
                                                i += 5;
                                            }
                                            else
                                            {
                                                string characterID = ((char)charArray[i + 2]).ToString() + ((char)charArray[i + 3]).ToString() + ((char)charArray[i + 4]).ToString() + ((char)charArray[i + 5]).ToString();
                                                pokemonMessage[count] = (int)Convert.ToUInt32(characterID, 16);
                                                i += 5;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (charArray[i] == '[')
                        {
                            if (charArray[i + 1] == 'P')
                            {
                                pokemonMessage[count] = 0x01E0;
                                i += 3;
                            }
                            if (charArray[i + 1] == 'M')
                            {
                                pokemonMessage[count] = 0x01E1;
                                i += 3;
                            }
                        }
                        else
                        {
                            pokemonMessage[count] = (int)Convert.ToUInt32(GetByte.GetString(((int)charArray[i]).ToString()), 16);
                        }
                    }
                    count++;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Format exception. Assembled so far: " + Environment.NewLine + pokemonMessage);
            }
            return pokemonMessage;
        }

        public int GetStringLength(string currentMessage)
        { // Calculates string length
            int count = 0;
            var charArray = currentMessage.ToCharArray();
            for (int i = 0; i < currentMessage.Length; i++)
            {
                if (charArray[i] == '\\')
                {
                    if (charArray[i + 1] == 'r')
                    {
                        count++;
                        i++;
                    }
                    else
                    {
                        if (charArray[i + 1] == 'n')
                        {
                            count++;
                            i++;
                        }
                        else
                        {
                            if (charArray[i + 1] == 'f')
                            {
                                count++;
                                i++;
                            }
                            else
                            {
                                if (charArray[i + 1] == 'v')
                                {
                                    count += 2;
                                    i += 5;
                                }
                                else
                                {
                                    if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '0')
                                    {
                                        count++;
                                        i += 5;
                                    }
                                    else
                                    {
                                        if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '1')
                                        {
                                            count++;
                                            i += 5;
                                        }
                                        else
                                        {
                                            count++;
                                            i += 5;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (charArray[i] == '[')
                    {
                        if (charArray[i + 1] == 'P')
                        {
                            count++;
                            i += 3;
                        }
                        if (charArray[i + 1] == 'M')
                        {
                            count++;
                            i += 3;
                        }
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            count++;
            return count;
        }

        private byte[] ToByteArray(List<string> msgSource)
        {
            MemoryStream newData = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(newData))
            {
                writer.Write((ushort)msgSource.Count);
                writer.Write((ushort)initialKey);

                int key = (initialKey * 0x2FD) & 0xFFFF;
                int key2 = 0;
                int realKey = 0;
                int offset = 0x4 + (msgSource.Count * 8);
                int[] stringSize = new int[msgSource.Count];

                for (int i = 0; i < msgSource.Count; i++)
                { // Reads and stores string offsets and sizes
                    key2 = (key * (i + 1) & 0xFFFF);
                    realKey = key2 | (key2 << 16);
                    writer.Write(offset ^ realKey);
                    int length = GetStringLength(msgSource[i]);
                    stringSize[i] = length;
                    writer.Write(length ^ realKey);
                    offset += length * 2;
                }

                for (int i = 0; i < msgSource.Count; i++)
                { // Encodes strings and writes them to file
                    key = (0x91BD3 * (i + 1)) & 0xFFFF;
                    int[] currentString = EncodeString(msgSource[i], i, stringSize[i]);

                    for (int j = 0; j < stringSize[i] - 1; j++)
                    {
                        writer.Write((ushort)(currentString[j] ^ key));
                        key += 0x493D;
                        key &= 0xFFFF;
                    }
                    writer.Write((ushort)(0xFFFF ^ key));
                }
            }
            return newData.ToArray();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, messages);
        }

        public override byte[] ToByteArray()
        {
            return this.ToByteArray(messages);
        }

        public void SaveToFileDefaultDir(int IDtoReplace, bool showSuccessMessage = true)
        {
            bool success = EncryptText.WriteMessageArchive(IDtoReplace, messages, IDtoReplace == trainerNamesMessageNumber);
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