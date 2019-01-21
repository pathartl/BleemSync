using BleemSync.Data;
using BleemSync.Data.Abstractions;
using BleemSync.Data.Entities;
using BleemSync.Data.Models;
using BleemSync.Services.Abstractions;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Services
{
    public class GameManagerService : IGameManagerService
    {
        private MenuDatabaseContext _context { get; set; }
        private IStorage _storage { get; set; }
        private IGameManagerNodeRepository _gameManagerNodeRepository { get; set; }
        private IGameManagerFileRepository _gameManagerFileRepository { get; set; }
        private IConfiguration _configuration { get; set; }
        private string _baseGamesDirectory { get; set; }

        public GameManagerService(MenuDatabaseContext context, IStorage storage, IConfiguration configuration)
        {
            _context = context;
            _gameManagerNodeRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _gameManagerFileRepository = storage.GetRepository<IGameManagerFileRepository>();
            _storage = storage;
            _configuration = configuration;

            _baseGamesDirectory = configuration["PlayStationClassic:GamesDirectory"];
        }

        public void AddGame(GameManagerNode node)
        {
            var game = new Game()
            {
                Id = node.Id,
                Title = node.Name,
                Publisher = node.Publisher,
                Year = node.ReleaseDate.HasValue ? node.ReleaseDate.Value.Year : 0,
                Players = node.Players.HasValue ? node.Players.Value : 0,
                Position = node.Position
            };

            _context.Games.Add(game);
            _context.SaveChanges();

            // Move the files to the correct location and update the BleemSync database to reflect where the files are moved to
            var outputDirectory = Path.Combine(_baseGamesDirectory, game.Id.ToString());

            Directory.CreateDirectory(outputDirectory);

            PostProcessGameFiles(node.Files, outputDirectory);

            _storage.Save();
        }

        private void PostProcessGameFiles(List<GameManagerFile> files, string outputDirectory)
        {
            var additionalFiles = new List<GameManagerFile>();

            foreach (var file in files)
            {
                var source = file.Path;
                var destination = Path.Combine(outputDirectory, file.Name);

                SystemMove(source, destination);
                file.Path = Path.GetFullPath(destination);

                var fileInfo = new FileInfo(destination);

                switch (fileInfo.Extension.ToLower())
                {
                    case ".pbp":
                        additionalFiles.Add(CreateCueSheet(fileInfo, file));
                        break;
                }
            }

            var cueFiles = files.Where(f => Path.GetExtension(f.Name).ToLower() == ".cue");
            var firstDiscFile = cueFiles.Select(f => f.Name).FirstOrDefault();
            if (firstDiscFile == null) firstDiscFile = files.First().Name;
            var baseName = Path.GetFileNameWithoutExtension(firstDiscFile);
            var coverFile = files.Where(f => f.Name == "cover.png").FirstOrDefault();
            if (coverFile != null)
            {
                var newCoverFileName = baseName + ".png";
                var newCoverFilePath = Path.Combine(outputDirectory, newCoverFileName);

                SystemMove(coverFile.Path, newCoverFilePath);
                coverFile.Path = Path.GetFullPath(newCoverFilePath);
                coverFile.Name = newCoverFileName;
            }

            files.AddRange(additionalFiles);

            var discNum = 1;
            foreach (var cueFile in cueFiles)
            {
                var disc = new Disc()
                {
                    DiscBasename = cueFile.Name.Replace(".cue", ""),
                    DiscNumber = discNum,
                    GameId = cueFile.NodeId,
                };

                _context.Discs.Add(disc);

                discNum++;
            }

            _context.SaveChanges();
        }

        static void SystemMove(string from, string to)
        {
            SystemMove(from, to, false);
        }

        // HACK: File.Move() seems to cause a copy. Try to mv with shell and see if it's faster.
        static void SystemMove(string from, string to, bool overwrite)
        {
            if (overwrite && File.Exists(to)) throw new IOException("Destination file exists.");
            from = from.Replace("\"", "\\\"");
            to = to.Replace("\"", "\\\"");
            var proc = System.Diagnostics.Process.Start("mv", $"{(overwrite ? "-f " : string.Empty)}\"{from}\" \"{to}\"");
            proc.WaitForExit();
            if (proc.ExitCode != 0) throw new IOException($"mv returned {proc.ExitCode}");
        }

        private GameManagerFile CreateCueSheet(FileInfo sourceFileInfo, GameManagerFile sourceFile)
        {
            var managerFile = new GameManagerFile();
            var sb = new StringBuilder();

            sb.AppendLine($"FILE \"{sourceFileInfo.Name}\" BINARY");
            sb.AppendLine("  TRACK 01 MODE2/2352");
            sb.AppendLine("    INDEX 01 00:00:00");

            var cueSheetFileName = sourceFile.Name.Replace(sourceFileInfo.Extension, "") + ".cue";

            File.WriteAllText(
                Path.Combine(
                    sourceFileInfo.Directory.FullName,
                    cueSheetFileName),
                sb.ToString());

            managerFile.Name = cueSheetFileName;
            managerFile.Path = sourceFile.Path;
            managerFile.NodeId = sourceFile.NodeId;

            _gameManagerFileRepository.Create(managerFile);
            _storage.Save();

            return managerFile;
        }

        public void UpdateGame(GameManagerNode node)
        {
            var game = _context.Games.Find(node.Id);

            game.Title = node.Name;
            game.Publisher = node.Publisher;
            game.Year = node.ReleaseDate.HasValue ? node.ReleaseDate.Value.Year : 0;
            game.Players = node.Players.HasValue ? node.Players.Value : 0;
            game.Position = node.Position;

            _context.Games.Update(game);
            _context.SaveChanges();
        }

        public void UpdateGames(IEnumerable<GameManagerNode> nodes)
        {
            foreach (var node in nodes)
            {
                var game = _context.Games.Find(node.Id);

                game.Title = node.Name;
                game.Publisher = node.Publisher;
                game.Year = node.ReleaseDate.HasValue ? node.ReleaseDate.Value.Year : 0;
                game.Players = node.Players.HasValue ? node.Players.Value : 0;
                game.Position = node.Position;

                _context.Games.Update(game);
            }

            _context.SaveChanges();
        }

        public void DeleteGame(GameManagerNode node)
        {
            // Delete files first
            var gameDirectory = Path.Combine(_baseGamesDirectory, node.Id.ToString());
            Directory.Delete(gameDirectory, true);

            var game = _context.Games.Find(node.Id);

            _context.Games.Remove(game);
            _context.SaveChanges();
        }

        public IEnumerable<GameManagerNode> GetGames()
        {
            List<GameManagerNode> nodes = new List<GameManagerNode>();
            foreach (var game in _context.Games)
            {
                var node = new GameManagerNode
                {
                    Id = game.Id,
                    Name = game.Title,
                    SortName = game.Title,
                    ReleaseDate = new DateTime(game.Year, 1, 1),
                    Players = game.Players,
                    Publisher = game.Publisher,
                    Type = GameManagerNodeType.Game
                };

                // For files, iterate what we have on disk instead of looking in the database
                foreach (var path in Directory.GetFiles(Path.Combine(_baseGamesDirectory, game.Id.ToString())))
                {
                    node.Files.Add(new GameManagerFile
                    {
                        Name = Path.GetFileName(path),
                        Path = Path.GetFullPath(path),
                        Node = node
                    });
                }

                nodes.Add(node);
            }

            return nodes;
        }

        public void Sync()
        {
            // Assuming everything's going OK, we can tell power_manage to reboot,
            // and any additional setup needed will be done on next boot
            File.WriteAllText("/dev/shm/power/control", "reboot");
        }
    }
}
