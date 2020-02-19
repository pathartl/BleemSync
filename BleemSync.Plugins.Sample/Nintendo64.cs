using BleemSync.Fingerprinting;
using System;
using System.Collections.Generic;
using System.IO;

namespace BleemSync.Plugins.Sample
{
    public class Nintendo64 : IFingerprinter
    {
        IEnumerable<string> IFingerprinter.FileExtensions
        {
            get => new string[] {
                "z64",
                "n64"
            };

            set => throw new NotImplementedException();
        }

        public string GetFingerprint(FileStream fileStream)
        {
            return "";
        }
    }
}
