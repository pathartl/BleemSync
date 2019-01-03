using BleemSync.Data;
using BleemSync.Data.Entities;
using BleemSync.Data.Models;
using BleemSync.Services.Abstractions;
using System.IO;

namespace BleemSync.Extensions.PlayStationClassic.Core.Services
{
    public class GameManagerService : IGameManagerService
    {
        private MenuDatabaseContext _context { get; set; }

        public GameManagerService(MenuDatabaseContext context)
        {
            _context = context;
        }

        public void AddGame(GameManagerNode node)
        {
            var game = new Game()
            {
                Id = node.Id,
                Title = node.Name,
                Publisher = node.Publisher,
                Year = node.ReleaseDate.HasValue ? node.ReleaseDate.Value.Year : 0,
                Players = node.Players.HasValue ? node.Players.Value : 0
            };

            _context.Games.Add(game);
            _context.SaveChanges();

            var outputDirectory = Path.Combine("Games", game.Id.ToString(), "GameData");

            Directory.CreateDirectory(outputDirectory);

            foreach (var file in node.Files)
            {
                File.Move(file.Path, Path.Combine(outputDirectory, file.Name));
            }
        }

        public void UpdateGame(GameManagerNode node)
        {
            var game = _context.Games.Find(node.Id);

            game.Title = node.Name;
            game.Publisher = node.Publisher;
            game.Year = node.ReleaseDate.HasValue ? node.ReleaseDate.Value.Year : 0;
            game.Players = node.Players.HasValue ? node.Players.Value : 0;

            _context.Games.Update(game);
            _context.SaveChanges();
        }

        public void DeleteGame(GameManagerNode node)
        {
            // Delete files first
            var gameDirectory = Path.Combine("Games", node.Id.ToString());
            Directory.Delete(gameDirectory, true);

            var game = _context.Games.Find(node.Id);

            _context.Games.Remove(game);
            _context.SaveChanges();
        }
    }
}
