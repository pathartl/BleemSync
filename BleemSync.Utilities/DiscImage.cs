using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BleemSync.Utilities
{
    public class DiscImage
    {
        public static string GetSerialNumber(string path)
        {
            var serial = "";
            var foundSerial = false;

            var serialNumberPrefixes = new List<string>()
            {
                "CPCS",
                "ESPM",
                "HPS",
                "LPS",
                "LSP",
                "SCAJ",
                "SCED",
                "SCES",
                "SCPS",
                "SCUS",
                "SIPS",
                "SLES",
                "SLKA",
                "SLPM",
                "SLPS",
                "SLUS"
            };

            using (FileStream isoStream = new FileStream(path, FileMode.Open))
            {
                var length = (int)isoStream.Length;
                var bits = new byte[11];

                var triggerCharacters = new List<byte>()
                {
                    Convert.ToByte('C'),
                    Convert.ToByte('E'),
                    Convert.ToByte('H'),
                    Convert.ToByte('L'),
                    Convert.ToByte('S')
                };

                var triggerChar = Convert.ToByte('S');

                // Search the file 11 bytes at a time
                for (int pos = 0; pos < length; pos += bits.Length)
                {
                    isoStream.Seek(pos, SeekOrigin.Begin);
                    isoStream.Read(bits, 0, bits.Length);
                    
                    // Search the byte array of the current 11 bytes for our trigger character
                    for (int i = 0; i < bits.Length; i++)
                    {
                        if (bits[i] == triggerChar)
                        {
                            pos += i;
                            isoStream.Seek(pos, SeekOrigin.Begin);
                            isoStream.Read(bits, 0, bits.Length);

                            var possibleString = Encoding.UTF8.GetString(bits);

                            foreach (var prefix in serialNumberPrefixes)
                            {
                                if (possibleString.StartsWith($"{prefix}_"))
                                {
                                    foundSerial = true;
                                    serial = possibleString;
                                }
                            }
                        }

                        if (foundSerial) break;
                    }

                    if (foundSerial) break;
                }
            }

            if (!foundSerial)
            {
                throw new Exception($"PlayStation game disc serial not found for file {path}. Please verify this is a valid, uncorruped disc image.");
            }

            return serial.Replace(".", "").Replace("_", "-");
        }
    }
}
