using System.IO;

namespace BleemSync.Validator.Validators
{
    public class DriveFormatValidator : Validator
    {
        public override string Title => $"Checking drive format of '{DirPath}'";
        
        public DriveFormatValidator(string path) : base(path) { }

        public override void Validate()
        {
            var driveInfo = new DriveInfo(DirPath);

            var valid = driveInfo.DriveFormat.Equals("FAT32") ||
                        driveInfo.DriveFormat.Equals("exFAT");

            Messages.Add(valid
                ? $"Drive format '{driveInfo.DriveFormat}' is valid."
                : $"Drive format '{driveInfo.DriveFormat}' is invalid. Valid formats 'FAT32', 'exFAT'");

            Status = valid ? ValidationStatus.Pass : ValidationStatus.Fail;
        }
    }
}