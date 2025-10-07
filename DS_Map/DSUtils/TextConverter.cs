using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DSPRE
{
    internal class TextConverter
    {

        private static Dictionary<ushort, string> decodeMap;
        private static Dictionary<string, ushort> encodeMap;
        private static Dictionary<ushort, string> commandMap;

        private static bool mapsInitialized = false;

        public static Dictionary<ushort, string> GetDecodingMap()
        {
            if (!mapsInitialized)
            {
                InitializeCharMaps();
            }
            return decodeMap;
        }

        public static Dictionary<string, ushort> GetEncodingMap()
        {
            if (!mapsInitialized)
            {
                InitializeCharMaps();
            }
            return encodeMap;
        }

        private static void InitializeCharMaps()
        {
            decodeMap = new Dictionary<ushort, string>();
            encodeMap = new Dictionary<string, ushort>();
            commandMap = new Dictionary<ushort, string>();

            string charmapPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools", "charmap.xml");
            if (!File.Exists(charmapPath))
                throw new FileNotFoundException("Charmap XML file not found.", charmapPath);

            var xml = XDocument.Load(charmapPath, LoadOptions.PreserveWhitespace);

            foreach (var entry in xml.Descendants("entry"))
            {
                var code = entry.Attribute("code")?.Value;
                var kind = entry.Attribute("kind")?.Value;
                var value = entry.Value;

                if (code == null || kind == null || value == null)
                {
                    AppLogger.Error("Found charmap entry with null value.");
                    continue;
                }

                ushort codeValue;
                string codeKind = kind.ToLower();
                string chars = value.ToString();

                if (!ushort.TryParse(code, System.Globalization.NumberStyles.HexNumber, null, out codeValue))
                {
                    AppLogger.Error($"Invalid code value in charmap: {code}");
                    continue;
                }

                if (codeKind == "char" || codeKind == "escape")
                {
                    decodeMap[codeValue] = chars;
                    encodeMap[chars] = codeValue;
                }
                else if (codeKind == "alias")
                {
                    encodeMap[chars] = codeValue;
                }
                else if (codeKind == "command")
                {
                    commandMap[codeValue] = chars;
                }
                else
                {
                    AppLogger.Error($"Unknown kind '{kind}' in charmap entry.");
                }
            }

            mapsInitialized = true;
        }

        public static List<string> ReadMessageFromStream(Stream stream, out UInt16 key)
        {
            List<string> messages = new List<string>();
            key = 0;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                try
                {
                    UInt16 messageCount = reader.ReadUInt16();
                    key = reader.ReadUInt16();

                    var messageTable = new List<(int offset, int length)>(messageCount);

                    for (int i = 0; i < messageCount; i++)
                    {
                        int offset = reader.ReadInt32();
                        int length = reader.ReadInt32();

                        // Decrypt length and offset
                        int localKey = (765 * (i+1) * key) & 0xFFFF;
                        localKey |= (localKey << 16);
                        offset ^= localKey;
                        length ^= localKey;

                        messageTable.Add((offset, length));
                    }

                    int msgIndex = 1;

                    foreach (var (offset, length) in messageTable)
                    {
                        if (offset < 0 || length < 0 || offset + length * 2 > stream.Length)
                        {
                            AppLogger.Error($"Invalid message offset/length for message {msgIndex}: offset={offset}, length={length}");
                            msgIndex++;
                            break;
                        }

                        byte[] encryptedBytes = reader.ReadBytes(length * 2);
                        UInt16[] encryptedData = new UInt16[length];
                        for (int j = 0; j < length; j++)
                        {
                            encryptedData[j] = BitConverter.ToUInt16(encryptedBytes, j * 2);
                        }
                        string message = DecryptMessage(encryptedData, msgIndex);
                        messages.Add(message);
                        msgIndex++;
                    }

                }
                catch (EndOfStreamException)
                {
                    MessageBox.Show("Unexpected end of file while reading messages.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading messages: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return messages;

        }

        public static void WriteMessagesToStream(ref Stream stream, List<string> messages, UInt16 key)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                try
                {
                    UInt16 messageCount = (UInt16)messages.Count;
                    writer.Write(messageCount);
                    writer.Write(key);
                    long tableStartPos = writer.BaseStream.Position;
                    writer.Seek(messageCount * 8, SeekOrigin.Current); // Reserve space for the message table
                    var messageTable = new List<(int offset, int length)>(messageCount);
                    for (int i = 0; i < messageCount; i++)
                    {
                        UInt16[] encryptedMessage = EncryptMessage(messages[i], (i+1));
                        int offset = (int)writer.BaseStream.Position;
                        int length = encryptedMessage.Length;
                        // Write encrypted message
                        foreach (var code in encryptedMessage)
                        {
                            writer.Write(code);
                        }
                        // Encrypt length and offset for the table
                        int localKey = (765 * (i+1) * key) & 0xFFFF;
                        localKey |= (localKey << 16);
                        int encOffset = offset ^ localKey;
                        int encLength = length ^ localKey;
                        messageTable.Add((encOffset, encLength));
                    }
                    long endPos = writer.BaseStream.Position;
                    // Write the message table
                    writer.Seek((int)tableStartPos, SeekOrigin.Begin);
                    foreach (var (offset, length) in messageTable)
                    {
                        writer.Write(offset);
                        writer.Write(length);
                    }
                    writer.Seek((int)endPos, SeekOrigin.Begin); // Move back to the end
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error writing messages: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string DecryptMessage(UInt16[] message, int index)
        {
            ushort localKey = (ushort)(index * 596947);

            for (int i = 0; i < message.Length; i++)
            {
                message[i] ^= localKey;
                localKey = (ushort)((localKey + 18749) & 0xFFFF);
            }

            return DecodeMessage(message);

        }

        private static UInt16[] EncryptMessage(string message, int index)
        {
            UInt16[] encodedMessage = EncodeMessage(message);
            ushort localKey = (ushort)(index * 596947);
            for (int i = 0; i < encodedMessage.Length; i++)
            {
                encodedMessage[i] ^= localKey;
                localKey = (ushort)((localKey + 18749) & 0xFFFF);
            }
            return encodedMessage;
        }

        private static string DecodeMessage(UInt16[] message)
        {
            StringBuilder decodedMessage = new StringBuilder();

            for (int i = 0; i < message.Length; i++)
            {
                ushort code = message[i];
                // Regular characters and escape sequences
                if (GetDecodingMap().ContainsKey(code))
                {                    
                    decodedMessage.Append(GetDecodingMap()[code]);                   
                }
                // Commands
                else if (code == 0xFFFE)
                {
                    var (command, toSkip) = DecodeCommand(message, i);
                    decodedMessage.Append(command);
                    i += toSkip;
                }
                // String terminator
                else if (code == 0xFFFF)
                {
                    break;
                }
                // Hexadecimal representation for unknown codes
                else
                {
                    decodedMessage.Append(ToHex(code));
                }
            }
            return decodedMessage.ToString();
        }

        private static UInt16[] EncodeMessage(string message)
        {
            List<UInt16> encodedMessage = new List<UInt16>();

            int i = 0;

            while (i < message.Length)
            {
                // Regular characters
                if (GetEncodingMap().ContainsKey(message[i].ToString()))
                {
                    encodedMessage.Add(GetEncodingMap()[message[i].ToString()]);
                    i++;
                }
                // Escape sequences
                else if (message[i] == '\\')
                {
                    // Handle hex escape sequences like \x1234
                    if (i + 5 < message.Length && message[i + 1] == 'x')
                    {
                        string hexSeq = message.Substring(i + 2, 4);
                        if (ushort.TryParse(hexSeq, System.Globalization.NumberStyles.HexNumber, null, out ushort hexValue))
                        {
                            encodedMessage.Add(hexValue);
                            i += 6;
                            continue;
                        }
                    }
                    // Handle single character escape sequences
                    else if (i + 1 < message.Length)
                    {
                        string escapeSeq = message.Substring(i, 2);
                        if (GetEncodingMap().ContainsKey(escapeSeq))
                        {
                            encodedMessage.Add(GetEncodingMap()[escapeSeq]);
                            i += 2;
                            continue;
                        }
                    }
                    // No match add null char
                    else
                    {
                        encodedMessage.Add(0);
                        i++;
                    }
                }
                // Commands
                else if (message[i] == '{')
                {
                    int endIndex = message.IndexOf('}', i);
                    if (endIndex != -1)
                    {
                        string command = message.Substring(i, endIndex - i + 1);
                        encodedMessage.AddRange(EncodeCommand(command));
                        i = endIndex + 1;
                        continue;
                    }
                    // No match add null char
                    encodedMessage.Add(0);
                    i++;
                }
                // No match
                else
                {                    
                    encodedMessage.Add(0);
                    i++;
                }
            }

            // Add string terminator
            encodedMessage.Add(0xFFFF);

            return encodedMessage.ToArray();

        }

        private static (string command, int toSkip) DecodeCommand(UInt16[] message, int startIndex)
        {
            // We need at least two more codes (command ID and param count)
            if (startIndex + 1 >= message.Length)
            {
                return ("\\x0000", 0);
            }
            else if (startIndex + 2 >= message.Length)
            {
                return (ToHex(message[startIndex + 1]), 1);
            }

            // Number of UInt16 codes to skip not including the initial 0xFFFE
            // We always skip at least the command ID and param count
            int skip = 2;

            ushort commandID = message[startIndex + 1];
            ushort paramCount = message[startIndex + 2];

            // We need to make sure we have enough codes for the parameters
            if (startIndex + 2 + paramCount >= message.Length)
            {
                return (ToHex(commandID, paramCount), skip);
            }

            ushort[] parameters = new ushort[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                parameters[i] = message[startIndex + 3 + i];
                skip++;
            }

            // Special case for string buffer vars that have 1 byte command ids
            int? specialByte = null;
            
            if (!commandMap.ContainsKey(commandID) && commandMap.ContainsKey((ushort)(commandID & 0xFF00))) 
            {
                commandID = (ushort)(commandID & 0xFF00);
                specialByte = (ushort)(commandID & 0x00FF);            
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            if (commandMap.ContainsKey(commandID))
            {
                sb.Append($"{commandMap[commandID]}");
                if (specialByte.HasValue)
                {
                    sb.Append($", {specialByte.Value}");
                }
                for (int i = 0; i < parameters.Length; i++)
                {
                    sb.Append($", {parameters[i]}");
                }
            }
            else
            {
                // Unknown command, represent as raw number
                sb.Append($"0x{commandID:X4}");
                for (int i = 0; i < parameters.Length; i++)
                {
                    sb.Append($", {parameters[i]}");
                }
            }
            sb.Append("}");
            return (sb.ToString(), skip);
        }

        private static UInt16[] EncodeCommand(string command)
        {
            // Strip the braces
            if (command.StartsWith("{") && command.EndsWith("}"))
            {
                command = command.Substring(1, command.Length - 2);
            }
            else
            {
                AppLogger.Error($"Invalid text command format for command: {command} ");
                return new UInt16[] { 0 };
            }
            
            string[] parts = command.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                AppLogger.Error($"Empty text command: {command}");
                return new UInt16[] { 0 };
            }

            string commandName = parts[0].Trim();
            ushort parameterCount = (ushort)(parts.Length - 1);

            List<UInt16> encodedCommand = new List<UInt16>();

            encodedCommand.Add(0xFFFE); // Command start

            if (commandMap.ContainsValue(commandName)) 
            {
                encodedCommand.Add(commandMap.Reverse()[commandName]);
            }
            else if (ushort.TryParse(commandName, out ushort commandID))
            {
                encodedCommand.Add(commandID);
            }
            else
            {
                AppLogger.Error($"Unknown text command: {commandName}");
                return new UInt16[] { 0 };
            }

            encodedCommand.Add(parameterCount);
            for (int i = 1; i < parts.Length; i++)
            {
                string paramStr = parts[i].Trim();
                if (ushort.TryParse(paramStr, out ushort paramValue))
                {
                    encodedCommand.Add(paramValue);
                }
                else
                {
                    AppLogger.Error($"Invalid parameter '{paramStr}' in command: {command}. Replaced with value '0'");
                    encodedCommand.Add(0);
                }
            }

            return encodedCommand.ToArray();

        }

        private static string ToHex(params ushort[] codes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var code in codes)
            {
                sb.Append($"\\x{code:X4}");
            }
            return sb.ToString();
        }

    }
}
