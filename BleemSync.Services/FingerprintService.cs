using BleemSync.Fingerprinting;
using BleemSync.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BleemSync.Services
{
    public class FingerprintService
    {
        public string GetFingerprint(string path)
        {
            string fingerprint = null;
            var fingerprinterDefinitions = Reflection.TypesImplementingInterface(typeof(IFingerprinter)).Where(t => !t.IsInterface);

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                foreach (var fingerprinterDefinition in fingerprinterDefinitions)
                {
                    IFingerprinter fingerprinter = Activator.CreateInstance(fingerprinterDefinition) as IFingerprinter;

                    if (fingerprinter != null && fingerprint == null)
                    {
                        fingerprint = fingerprinter.GetFingerprint(fileStream);
                    }
                }
            }

            return fingerprint;
        }


    }
}