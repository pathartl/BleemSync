using System;
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

            using (FileStream isoStream = new FileStream(path, FileMode.Open))
            {
                var length = (int)isoStream.Length;
                var bits = new byte[11];
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

                            if (possibleString.StartsWith("SCUS_") || possibleString.StartsWith("SLUS_") || possibleString.StartsWith("SP_"))
                            {
                                foundSerial = true;
                                serial = possibleString;
                            }
                        }

                        if (foundSerial) break;
                    }

                    if (foundSerial) break;
                }
            }

            return serial;
        }
    }
}
