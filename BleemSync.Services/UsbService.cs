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

    }

    public enum DriveFormat
    {
        [Display(Name = "FAT16")]
        Fat16,

        [Display(Name = "FAT")]
        Fat32,

        [Display(Name = "NTFS")]
        Ntfs,

        [Display(Name = "exFAT")]
        Exfat,

        [Display(Name = "ext3")]
        Ext3,

        [Display(Name = "ext4")]
        Ext4,

        [Display(Name = "HFS+")]
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
