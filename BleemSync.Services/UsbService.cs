using BleemSync.Services.Extensions;
using BleemSync.Services.ViewModels;
using Microsoft.Extensions.Configuration;
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
        private readonly IWritableOptions<BleemSyncConfiguration> _writableConfig;
        private readonly IConfiguration _config;

        public UsbService(IWritableOptions<BleemSyncConfiguration> writableConfig, IConfiguration config)
        {
            _writableConfig = writableConfig;
            _config = config;
        }

        public DriveInfo GetDrive(string driveName)
        {
            return GetDrives().Where(d => d.Name == driveName).FirstOrDefault();
        }

        public IEnumerable<DriveInfo> GetDrives()
        {
            var devices = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable);

            return devices;
        }

        public DriveInfo GetCurrentDrive()
        {
            var currentDrivePath = _config["BleemSync:Destination"];

            return GetDrives().SingleOrDefault(d => d.RootDirectory.FullName == currentDrivePath);
        }

        public void SetCurrentDrive(string driveName)
        {
            SetCurrentDrive(GetDrive(driveName));
        }

        public void SetCurrentDrive(DriveInfo drive)
        {
            _writableConfig.Update(config =>
            {
                config.Destination = drive.RootDirectory.FullName;
            });
        }
    }

    public enum DriveFormat
    {
        [Display(Name = "FAT")]
        Fat16,

        [Display(Name = "FAT32")]
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
