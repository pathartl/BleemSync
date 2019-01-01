using System.IO;

namespace BleemSync.Validator.Validators
{
    public class BleemSystemValidator : Validator
    {
        public override string Title => $"Checking for Bleem artifacts in root of '{DirPath}'";

        public BleemSystemValidator(string path) : base(path) { }

        public override void Validate()
        {
            var valid = FolderExists("028c18a9-ec4b-4632-b2cf-d4e20f252e8f") &
                        FileExists("028c18a9-ec4b-4632-b2cf-d4e20f252e8f", "LUPDATA.BIN") &
                        FolderExists("lolhack") &
                        FileExists("lolhack", "20-joystick.rules") &
                        FileExists("lolhack", "boot.sh") &
                        FileExists("lolhack", "lolhack.sh") &
                        FolderExists("System") &
                        FolderExists("Games") &
                        FolderExists("System\\Databases") &
                        FileExists("System\\Databases", "regional.db");

            Messages.Add(valid
                ? "Required Bleem artifacts are present."
                : "Not all required Bleem artifacts are present.");

            Status = valid ? ValidationStatus.Pass : ValidationStatus.Fail;
        }

        private bool FolderExists(string name)
        {
            if (Directory.Exists(Path.Join(DirPath, name))) return true;
            Messages.Add($"Folder '{name}' does not exist in '{DirPath}'.");
            return false;
        }

        private bool FileExists(string path, string name)
        {
            var filePath = Path.Join(DirPath, path);

            if (File.Exists(Path.Join(filePath, name))) return true;
            Messages.Add($"File '{name}' does not exist in '{filePath}'.");
            return false;
        }
    }
}