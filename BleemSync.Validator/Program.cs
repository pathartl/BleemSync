using BleemSync.Validator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BleemSync.Validator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var drives = DriveInfo
                .GetDrives()
                .Where(d => d.DriveType == DriveType.Removable)
                .ToList();

            if (!drives.Any())
            {
                Console.WriteLine("No USB drives attached.");
                Console.Read();
                return;
            }

            var selectedDrive = SelectDrive(drives);

            try
            {
                Console.WriteLine($"==== Validating '{selectedDrive}' ====");

                var validator = new ValidationService(selectedDrive);
                validator.Validate();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("==== Validation complete ====");
                Console.Read();
            }
        }

        private static string SelectDrive(IReadOnlyCollection<DriveInfo> drives)
        {
            if (drives.Count == 1) return drives.First().Name;

            Console.WriteLine($"{drives.Count} USB drives found, please type the letter of the drive to validate.");
            foreach (var drive in drives)
            {
                Console.WriteLine(drive.Name);
            }

            var selectedDrive = "";

            do
            {
                selectedDrive = Console.ReadKey(true).Key.ToString();
            } while (!drives.Any(d =>
                d.Name.StartsWith(selectedDrive, StringComparison.InvariantCultureIgnoreCase)));

            return selectedDrive.ToUpper() + ":\\";
        }
    }
}