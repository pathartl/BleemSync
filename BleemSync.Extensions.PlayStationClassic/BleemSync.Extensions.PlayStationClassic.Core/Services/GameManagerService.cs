using BleemSync.Data;
using BleemSync.Data.Abstractions;
using BleemSync.Data.Entities;
using BleemSync.Data.Models;
using BleemSync.Services.Abstractions;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BleemSync.Extensions.PlayStationClassic.Core.Services
{
    public class GameManagerService : IGameManagerService
    {
        private MenuDatabaseContext _context { get; set; }
        private IStorage _storage { get; set; }
        private IGameManagerNodeRepository _gameManagerNodeRepository { get; set; }
        private IConfiguration _configuration { get; set; }
        private string _baseGamesDirectory { get; set; }

        public GameManagerService(MenuDatabaseContext context, IStorage storage, IConfiguration configuration)
        {
            _context = context;
            _gameManagerNodeRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _storage = storage;
            _configuration = configuration;

            _baseGamesDirectory = Path.Combine(configuration["PlayStationClassic:GamesDirectory"].Split('/'));
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

            // Move the files to the correct location and update the BleemSync database to reflect where the files are moved to
            var outputDirectory = Path.Combine(_baseGamesDirectory, game.Id.ToString(), "GameData");

            Directory.CreateDirectory(outputDirectory);

            foreach (var file in node.Files)
            {
                var source = file.Path;
                var destination = Path.Combine(outputDirectory, file.Name);

                File.Move(source, destination);
                file.Path = destination;
            }

            _storage.Save();
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
            var gameDirectory = Path.Combine(_baseGamesDirectory, node.Id.ToString());
            Directory.Delete(gameDirectory, true);

            var game = _context.Games.Find(node.Id);

            _context.Games.Remove(game);
            _context.SaveChanges();
        }
    }
}
