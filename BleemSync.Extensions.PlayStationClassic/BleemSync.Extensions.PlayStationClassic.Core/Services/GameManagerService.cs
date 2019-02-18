using BleemSync.Data;
using BleemSync.Data.Entities;
using BleemSync.Data.Models;
using BleemSync.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
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
        private DatabaseContext _bleemsyncContext { get; set; }
        private IConfiguration _configuration { get; set; }
        private string _baseGamesDirectory { get; set; }

        public GameManagerService(MenuDatabaseContext context, DatabaseContext bleemsyncContext, IConfiguration configuration)
        {
            _context = context;
            _bleemsyncContext = bleemsyncContext;
            _configuration = configuration;

            _baseGamesDirectory = Path.Combine(
                configuration["BleemSync:Destination"],
                configuration["BleemSync:PlayStationClassic:GamesDirectory"]
            );
        }

        private void AddDiscs(GameManagerNode gameNode)
        {
            #region Populate Disc Entries
            // Playlist files / cue sheets should always be first in this list
            var launchableGameFileExtensions = new string[]
            {
                ".cue",
                ".m3u",
                ".bin",
                ".iso",
                ".pbp",
                ".img",
                ".mdf",
                ".toc",
                ".cbn"
            };

            IEnumerable<GameManagerFile> launchableGameFiles = new List<GameManagerFile>();

            // Search uploaded files for the first launchable files out of our file extension list.
            // We only want to grab the first here as people shouldn't be mixing and matching different
            // game image file formats.
            foreach (var launchableGameFileExtension in launchableGameFileExtensions)
            {
                launchableGameFiles = gameNode.Files.Where(f => Path.GetExtension(f.Path) == launchableGameFileExtension);

                if (launchableGameFiles.Count() > 0)
                {
                    break;
                }
            }

            var discNum = 1;

            foreach (var launchableGameFile in launchableGameFiles)
            {
                var disc = new Disc()
                {
                    DiscBasename = Path.GetFileNameWithoutExtension(launchableGameFile.Path),
                    DiscNumber = discNum,
                    GameId = gameNode.Id,
                };

                _context.Discs.Add(disc);

                discNum++;
            }
            #endregion
        }

        private void MoveGame(GameManagerNode gameNode)
        {
            if (gameNode.Type != GameManagerNodeType.Game)
            {
                throw new InvalidDataException("The node provided was not a game.");
            }

            var gameDirectory = Path.Combine(_baseGamesDirectory, gameNode.Id.ToString());

            // Ensure directory exists before we try to move the files
            Directory.CreateDirectory(gameDirectory);

            foreach (var file in gameNode.Files)
            {
                var source = file.Path;
                var destination = Path.Combine(gameDirectory, file.Name);
                var extension = Path.GetExtension(file.Name).ToLower();

                // Lowercase/normalize extension
                Path.ChangeExtension(destination, extension);

                File.Move(source, destination);

                // Update the path in the database
                file.Path = Path.GetFullPath(destination);
            }

            #region Move Cover File
            // Rename the cover file to the basename of the game's image or cue sheet
            string basename = "";
            var firstCueSheet = gameNode.Files.Where(f => Path.GetExtension(f.Path) == ".cue").FirstOrDefault();

            if (firstCueSheet != null)
            {
                basename = Path.GetFileNameWithoutExtension(firstCueSheet.Path);
            }
            else
            {
                basename = Path.GetFileNameWithoutExtension(gameNode.Files.First().Path);
            }

            var cover = gameNode.Files.Where(f => f.Name == "cover.png").FirstOrDefault();

            if (cover != null)
            {
                File.Move(cover.Path, Path.Combine(gameDirectory, $"{basename}.png"));
            }
            #endregion

            _bleemsyncContext.SaveChanges();
        }

        public void AddGame(GameManagerNode node)
        {
            AddGame(node, _context);
        }

        private void AddGame(GameManagerNode node, MenuDatabaseContext context)
        {
            UpdateGame(node, context);
            MoveGame(node);
        }

        public void UpdateGame(GameManagerNode node)
        {
            UpdateGame(node, _context);
        }

        private void UpdateGame(GameManagerNode node, MenuDatabaseContext context)
        {
            var exists = true;
            var game = _context.Games.Find(node.Id);

            // If game doesn't exist in database, create it
            if (game == null)
            {
                game = new Game()
                {
                    Id = node.Id
                };

                exists = false;
            }

            game.Title = node.Name;
            game.Publisher = node.Publisher;
            game.Year = node.ReleaseDate.HasValue ? node.ReleaseDate.Value.Year : 0;
            game.Players = node.Players.HasValue ? node.Players.Value : 0;
            game.Position = node.Position;

            if (exists)
            {
                context.Games.Update(game);
            }
            else
            {
                context.Games.Add(game);
            }

            context.SaveChanges();
        }

        public void UpdateGames(IEnumerable<GameManagerNode> nodes)
        {
            UpdateGames(nodes, _context);
        }

        private void UpdateGames(IEnumerable<GameManagerNode> nodes, MenuDatabaseContext context)
        {
            foreach (var node in nodes)
            {
                UpdateGame(node, context);
            }
        }

        private GameManagerFile CreateCueSheet(FileInfo sourceFileInfo, GameManagerFile sourceFile)
        {
            var managerFile = new GameManagerFile();
            var sb = new StringBuilder();

            sb.AppendLine($"FILE \"{sourceFileInfo.Name}\" BINARY");
            sb.AppendLine("  TRACK 01 MODE2/2352");
            sb.AppendLine("    INDEX 01 00:00:00");

            var cueSheetFileName = Path.ChangeExtension(sourceFile.Name, ".cue");

            File.WriteAllText(
                Path.Combine(
                    sourceFileInfo.Directory.FullName,
                    cueSheetFileName),
                sb.ToString());

            managerFile.Name = cueSheetFileName;
            managerFile.Path = sourceFile.Path;
            managerFile.NodeId = sourceFile.NodeId;

            return managerFile;
        }

        public void DeleteGame(GameManagerNode node)
        {
            // Delete files first
            var gameDirectory = Path.Combine(_baseGamesDirectory, node.Id.ToString());
            if (Directory.Exists(gameDirectory)) Directory.Delete(gameDirectory, true);

            var game = _context.Games.Find(node.Id);

            _context.Games.Remove(game);
            _context.SaveChanges();
        }

        public void RebuildDatabase(IEnumerable<GameManagerNode> nodes)
        {
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();

            foreach (var node in nodes)
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

                if (node.Files.Count > 0)
                {
                    var cueFiles = node.Files.Where(f => Path.GetExtension(f.Name).ToLower() == ".cue");

                    var discNum = 1;

                    foreach (var cueFile in cueFiles)
                    {
                        var disc = new Disc()
                        {
                            DiscBasename = Path.ChangeExtension(cueFile.Name, null),
                            DiscNumber = discNum,
                            GameId = cueFile.NodeId,
                        };

                        _context.Discs.Add(disc);

                        discNum++;
                    }
                }

                _context.SaveChanges();
            }
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

                string gameDir = Path.Combine(_baseGamesDirectory, game.Id.ToString());
                // If user for some reason doesn't have the game files, don't return game
                if (Directory.Exists(gameDir))
                {
                    // For files, iterate what we have on disk instead of looking in the database
                    foreach (var path in Directory.GetFiles(gameDir))
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
            }

            return nodes;
        }

        public void GenerateFolders()
        {
            var folderIds = _bleemsyncContext.Nodes.Where(n => n.Type == GameManagerNodeType.Folder).Select(n => n.Id).ToList();

            foreach (var folderId in folderIds)
            {
                var folderGameDir = Path.Combine(
                    _configuration["BleemSync:Destination"],
                    _configuration["BleemSync:PlayStationClassic:GamesDirectory"],
                    folderId.ToString()
                );

                var optionsBuilder = new DbContextOptionsBuilder<MenuDatabaseContext>();
                optionsBuilder.UseSqlite("Data Source=" + Path.Combine(
                    folderGameDir,
                    "folder.db"
                ));

                Directory.CreateDirectory(folderGameDir);

                using (var folderContext = new MenuDatabaseContext(optionsBuilder.Options, _configuration))
                {
                    var nodesForFolder = _bleemsyncContext.Nodes.Where(n => n.ParentId == folderId).ToList();

                    foreach (var node in nodesForFolder)
                    {
                        AddGame(node, folderContext);
                    }
                }
            }
        }
    }
}
