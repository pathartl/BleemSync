using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BleemSync.Validator.Validators
{
    public class GameValidator : Validator
    {
        private readonly IEnumerable<FileInfo> _gameFiles;
        private readonly IEnumerable<FileInfo> _binFiles;
        private readonly IEnumerable<FileInfo> _cueFiles;

        public override string Title => $"Validating folder structure for '{DirPath}'";

        public GameValidator(string path) : base(path)
        {
            _gameFiles = new DirectoryInfo(Path.Join(DirPath, "GameData"))
                .GetFiles();

            _binFiles = _gameFiles
                .Where(f => f.Extension.Equals(".bin", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            _cueFiles = _gameFiles
                .Where(f => f.Extension.Equals(".cue", StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public override void Validate()
        {
            if (!Directory.Exists(Path.Join(DirPath, "GameData")))
            {
                Messages.Add("Could not find 'GameData' folder.");
                Status = ValidationStatus.Fail;
                return;
            }
            
            var gameChecks = new List<ValidationStatus>();
            gameChecks.AddRange(ExtensionsExist());
            gameChecks.AddRange(FilesExist());
            gameChecks.Add(BinFileNamesValid());
            gameChecks.AddRange(MatchBinsWithCues());
            gameChecks.AddRange(CueContentsValid());
            gameChecks.Add(GameIniContentsValid());

            Status = GetValidationStatus(gameChecks);

            if(Status == ValidationStatus.Pass) Messages.Add("Game validated successfully.");
        }

        private IEnumerable<ValidationStatus> ExtensionsExist()
        {
            if (!_gameFiles.Any())
            {
                yield return ValidationStatus.Fail;
                yield break;
            }

            var extensionChecks = new Dictionary<string, ValidationStatus>
            {
                {".bin", ValidationStatus.Fail},
                {".cue", ValidationStatus.Fail},
                {".png", ValidationStatus.Warn}
            };

            foreach (var (extension, status) in extensionChecks)
            {
                if (_gameFiles.Any(f => f.Extension.Equals(extension, StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return ValidationStatus.Pass;
                    continue;
                };

                Messages.Add($"Could not find any '{extension}' file(s).");
                yield return status;
            }
        }

        private IEnumerable<ValidationStatus> FilesExist()
        {
            if (!_gameFiles.Any())
            {
                yield return ValidationStatus.Fail;
                yield break;
            }

            var fileChecks = new Dictionary<string, ValidationStatus>
            {
                {"Game.ini", ValidationStatus.Fail},
                {"pcsx.cfg", ValidationStatus.Fail}
            };

            foreach (var (fileName, status) in fileChecks)
            {
                if (_gameFiles.Any(f => f.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return ValidationStatus.Pass;
                    continue;
                }

                Messages.Add($"Could not find file '{fileName}'.");
                yield return status;
            }
        }

        private ValidationStatus BinFileNamesValid()
        {
            if (!_binFiles.Any()) return ValidationStatus.Fail;

            var binNames = _binFiles
                .Select(n => Path.GetFileNameWithoutExtension(n.Name))
                .ToList();
            
            if (!binNames.Any(b => b.Contains(',') || b.Contains('.'))) return ValidationStatus.Pass;

            Messages.Add("'.bin' files must not contain ',' or '.' in name.");
            return ValidationStatus.Fail;
        }

        private IEnumerable<ValidationStatus> MatchBinsWithCues()
        {
            if (!_binFiles.Any() || !_cueFiles.Any())
            {
                yield return ValidationStatus.Fail;
                yield break;
            }

            var binNames = _binFiles
                .Select(n => Path.GetFileNameWithoutExtension(n.Name))
                .ToList();

            var cueNames = _cueFiles
                .Select(n => Path.GetFileNameWithoutExtension(n.Name))
                .ToList();

            foreach (var name in binNames)
            {
                if (cueNames.Any(c => c.Equals(name)))
                {
                    yield return ValidationStatus.Pass;
                    continue;
                };

                Messages.Add($"The '.bin' file '{name}' does not have a matching '.cue' file.");
                yield return ValidationStatus.Fail;
            }
        }

        private IEnumerable<ValidationStatus> CueContentsValid()
        {
            if (!_cueFiles.Any())
            {
                yield return ValidationStatus.Fail;
                yield break;
            }

            foreach (var cue in _cueFiles)
            {
                var cueName = Path.GetFileNameWithoutExtension(cue.Name);
                var cueFileLine = File
                    .ReadAllLines(cue.FullName)
                    .FirstOrDefault(line => line.StartsWith("FILE"));

                if (cueFileLine?.Contains($"\"{cueName}\"") == true)
                {
                    yield return ValidationStatus.Pass;
                    continue;
                };

                Messages.Add(
                    $"The contents of the '.cue' file '{cue.Name}' does not start with 'FILE \"{cueName}\" BINARY'.");
                yield return ValidationStatus.Fail;
            }
        }

        private ValidationStatus GameIniContentsValid()
        {
            if (!_gameFiles.Any() || !_cueFiles.Any()) return ValidationStatus.Fail;

            var cueNames = _cueFiles
                .Select(n => Path.GetFileNameWithoutExtension(n.Name))
                .ToList();

            var iniFile = _gameFiles
                .FirstOrDefault(f => f.Name.Equals("Game.ini", StringComparison.InvariantCultureIgnoreCase));

            if (!cueNames.Any() || iniFile == null) return ValidationStatus.Fail;

            var iniDiscLine = File
                .ReadAllLines(iniFile.FullName)
                .FirstOrDefault(line => line.StartsWith("Discs"));
            
            if (iniDiscLine == null) return ValidationStatus.Fail;

            if (cueNames.All(c => iniDiscLine.Contains(c))) return ValidationStatus.Pass;

            Messages.Add(
                $"The contents of the 'Game.ini' file is incorrect. Discs line does not list all '.cue' files in the game directory. Try modifying to 'Discs={string.Join(",", cueNames)}'");
            return ValidationStatus.Fail;
        }

        private static ValidationStatus GetValidationStatus(IList<ValidationStatus> statuses)
        {
            if (statuses.Any(s => s == ValidationStatus.Fail)) return ValidationStatus.Fail;
            return statuses.Any(s => s == ValidationStatus.Warn) ? ValidationStatus.Warn : ValidationStatus.Pass;
        }
    }
}