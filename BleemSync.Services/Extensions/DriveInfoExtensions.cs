using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BleemSync.Services.Extensions
{
    public static class DriveInfoExtensions
    {
        public static bool DriveIsFormattedProperly(this DriveInfo driveInfo)
        {
            var validFormats = new DriveFormat[]
            {
                DriveFormat.Fat32,
                DriveFormat.Ntfs,
                DriveFormat.Exfat,
                DriveFormat.Ext4
            };

            var displayNames = new List<String>();

            foreach (var format in validFormats)
            {
                var key = format.ToString();
                var displayName = key.ToUpper();

                try
                {
                    displayName = format.GetType().GetMember(key).First().GetCustomAttribute<DisplayAttribute>().Name;
                }
                catch { }

                displayNames.Add(displayName);
            }

            return displayNames.Contains(driveInfo.DriveFormat.ToUpper());
        }

        private static decimal ConvertToDataUnit(long bytes, DataUnit unit)
        {
            switch (unit)
            {
                case DataUnit.Bit:
                    return bytes * 8;

                case DataUnit.Byte:
                default:
                    return bytes;

                case DataUnit.Kilobyte:
                    return bytes / 1024M;

                case DataUnit.Megabyte:
                    return bytes / 1024M / 1024M;

                case DataUnit.Gigabyte:
                    return bytes / 1024M / 1024M / 1024M;

                case DataUnit.Terabyte:
                    return bytes / 1024M / 1024M / 1024M / 1024M;
            }
        }

        public static decimal GetFreeSpace(this DriveInfo driveInfo, DataUnit unit = DataUnit.Byte)
        {
            return ConvertToDataUnit(driveInfo.TotalFreeSpace, unit);
        }

        public static string GetHumanReadableFreeSpace(this DriveInfo driveInfo)
        {
            return GetHumanReadableSize(driveInfo.TotalFreeSpace);
        }

        public static string GetHumanReadableTotalSpace(this DriveInfo driveInfo)
        {
            return GetHumanReadableSize(driveInfo.TotalSize);
        }

        public static decimal GetTotalSpace(this DriveInfo driveInfo, DataUnit unit = DataUnit.Byte)
        {
            return ConvertToDataUnit(driveInfo.TotalSize, unit);
        }

        private static string GetHumanReadableSize(long size)
        {
            // Probably a better way to do all this.
            var humanSize = "";
            var unit = "bytes";
            var reduced = 0d;
            var outOfBounds = false;

            // Is bits
            if (size < 8)
            {
                unit = "bits";
                reduced = (double)size;
            }
            else if (size >= 8 && size < Math.Pow(1024, 1))
            {
                unit = "bytes";
                reduced = (double)size;
            }
            else if (size >= Math.Pow(1024, 1) && size < Math.Pow(1024, 2))
            {
                reduced = size / Math.Pow(1024, 1);
                unit = "KB";
            }
            else if (size >= Math.Pow(1024, 2) && size < Math.Pow(1024, 3))
            {
                reduced = size / Math.Pow(1024, 2);
                unit = "MB";
            }
            else if (size >= Math.Pow(1024, 3) && size < Math.Pow(1024, 4))
            {
                reduced = size / Math.Pow(1024, 3);
                unit = "GB";
            }
            else if (size >= Math.Pow(1024, 4) && size < Math.Pow(1024, 5))
            {
                reduced = size / Math.Pow(1024, 4);
                unit = "TB";
            }
            else
            {
                outOfBounds = true;
            }

            if (!outOfBounds)
            {
                return $"{reduced.ToString("0.0")} {unit}";
            }
            else
            {
                return "Whoah, slow down there!";
            }
        }
    }
}
