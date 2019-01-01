using System.IO;
using System.Linq;

namespace BleemSync.Validator.Validators
{
    public class GamesValidator : Validator
    {
        public override string Title => "Checking game folders";

        public GamesValidator(string path) : base(path) { }

        public override void Validate()
        {
            var games = Directory.GetDirectories(System.IO.Path.Join(DirPath, "Games"));
            
            foreach (var gamePath in games)
            {
                var validator = new GameValidator(gamePath);
                Validators.Add(validator);
            }

            var valid = Validators.All(v => v.Status == ValidationStatus.Pass);

            Messages.Add(valid 
                ? "Games validated successfully." 
                : "Games not validated successfully. Some or all of your games may not work.");

            Status = valid ? ValidationStatus.Pass : ValidationStatus.Warn;
        }
    }
}