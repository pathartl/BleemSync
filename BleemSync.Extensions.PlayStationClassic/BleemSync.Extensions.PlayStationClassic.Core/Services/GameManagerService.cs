using BleemSync.Data;
using BleemSync.Data.Entities;
using BleemSync.Data.Models;
using BleemSync.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Services
{
    public class GameManagerService : IGameManagerService
    {
        private MenuDatabaseContext _context { get; set; }

        public GameManagerService(MenuDatabaseContext context)
        {
            _context = context;
        }

        public void UploadGame(GameManagerNode node, IEnumerable<string> filePaths)
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

            foreach (var filePath in filePaths)
            {
                File.Move(filePath, Path.Combine(outputDirectory, $"{game.Title}.bin"));
            }
        }
    }
}
