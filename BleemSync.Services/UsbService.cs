using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BleemSync.Services
{
    public class UsbService
    {
        public IEnumerable<DriveInfo> GetDrives()
        {
            var devices = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable);

            return devices;
        }

        public bool DriveIsFormattedProperly(DriveInfo driveInfo)
        {
            var validFormats = new DriveFormat[]
            {
                DriveFormat.Fat32,
                DriveFormat.Ntfs,
                DriveFormat.Exfat,
                DriveFormat.Ext4
            };

            var displayNames = validFormats.Select(df => df
                .GetType()
                .GetMember(df.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>().Name
                .ToUpper()
            );

            return displayNames.Contains(driveInfo.DriveFormat.ToUpper());
        }

        private decimal ConvertToDataUnit(long bytes, DataUnit unit)
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

        public decimal GetFreeSpace(DriveInfo driveInfo, DataUnit unit = DataUnit.Byte) {
            return ConvertToDataUnit(driveInfo.TotalFreeSpace, unit);
        }

        public string GetHumanReadableFreeSpace(DriveInfo driveInfo)
        {
            return GetHumanReadableSize(driveInfo.TotalFreeSpace);
        }

        public string GetHumanReadableTotalSpace(DriveInfo driveInfo)
        {
            return GetHumanReadableSize(driveInfo.TotalSize);
        }

        public decimal GetTotalSpace(DriveInfo driveInfo, DataUnit unit = DataUnit.Byte)
        {
            return ConvertToDataUnit(driveInfo.TotalSize, unit);
        }

        private string GetHumanReadableSize(long size)
        {
            // Probably a better way to do all this.
            var humanSize = "";
            var magicNumber = 1024;

            // Is bits
            if (size < 8)
            {
                return humanSize = $"{size} bits";
            }
            else if (size >= 8 && size < (magicNumber ^ 1))
            {
                return humanSize = $"{size} bytes";
            }
            else if (size >= (magicNumber ^ 1) && size < (magicNumber ^ 2))
            {
                return humanSize = $"{size / (magicNumber ^ 1)} KB";
            }
            else if (size >= (magicNumber ^ 2) && size < (magicNumber ^ 3))
            {
                return humanSize = $"{size / (magicNumber ^ 2)} MB";
            }
            else if (size >= (magicNumber ^ 3) && size < (magicNumber ^ 4))
            {
                return humanSize = $"{size / (magicNumber ^ 3)} GB";
            }
            else if (size >= (magicNumber ^ 4) && size < (magicNumber ^ 5))
            {
                return humanSize = $"{size / (magicNumber ^ 4)} TB";
            }
            else
            {
                return "Whoah, slow down there!";
            }
        }
    }

    public enum DriveFormat
    {
        Fat16,
        Fat32,
        Ntfs,
        Exfat,
        Ext3,
        Ext4,
        HfsPlus
    }

    public enum DataUnit
    {
        Bit,
        Byte,
        Kilobyte,
        Megabyte,
        Gigabyte,
        Terabyte
    }
}
