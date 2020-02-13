using BleemSync.Data;
using BleemSync.Services.Extensions;
using BleemSync.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
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
        private readonly DbContext _context;

        public UsbService(DatabaseContext context, IWritableOptions<BleemSyncConfiguration> writableConfig, IConfiguration config)
        {
            _context = context;
            _writableConfig = writableConfig;
            _config = config;
        }

        public UsbService(IServiceProvider serviceProvider, IWritableOptions<BleemSyncConfiguration> writableConfig, IConfiguration config)
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

            _context.Database.Migrate();
        }
    }

    public static class DatabaseExtension
    {
        public static void ChangeDatabase(this DatabaseFacade database, string connectionString)
        {
            //database.
        }
    }
}
