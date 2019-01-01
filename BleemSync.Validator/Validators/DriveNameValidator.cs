using System.IO;

namespace BleemSync.Validator.Validators
{
    public class DriveNameValidator : Validator
    {
        public override string Title => $"Checking '{DirPath}' is named 'SONY'";

        public DriveNameValidator(string path) : base(path) { }

        public override void Validate()
        {
            var driveInfo = new DriveInfo(DirPath);

            var valid = driveInfo.VolumeLabel.Equals("SONY");

            Messages.Add(valid
                ? $"Drive is named 'SONY'."
                : $"Drive must be named 'SONY'.");

            Status = valid ? ValidationStatus.Pass : ValidationStatus.Fail;
        }
    }
}