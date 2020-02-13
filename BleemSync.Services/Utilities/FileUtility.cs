using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BleemSync.Services.Utilities
{
    public static class FileUtility
    {
        public static void Move(string src, string dest)
        {
            var destinationPath = new FileInfo(dest).DirectoryName;

            Directory.CreateDirectory(destinationPath);

            src = src.Replace("\\", "\\\\");
            dest = dest.Replace("\\", "\\\\");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c move \"{src}\" \"{dest}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "mv",
                        Arguments = $"\"{src}\" \"{dest}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();
            }
        }
    }
}
