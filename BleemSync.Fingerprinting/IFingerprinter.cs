using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BleemSync.Fingerprinting
{
    public interface IFingerprinter
    {
        IEnumerable<string> FileExtensions { get; set; }

        string GetFingerprint(FileStream fileStream);
    }
}
