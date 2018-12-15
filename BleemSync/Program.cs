using BleemSync.Data;
using BleemSync.Data.Models;
using BleemSync.Services;
using BleemSync.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BleemSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("BleemSync.Config.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();

            var relativeDatabaseLocation = Path.Combine(configuration["GamesDatabase"].Split('/'));
            var databaseLocation = Path.Combine(Filesystem.GetExecutingDirectory(), relativeDatabaseLocation);

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(databaseLocation)
            );

            var serviceProvider = services.BuildServiceProvider();
            var _context = serviceProvider.GetService<DatabaseContext>();

            SyncGames(_context);
        }

        static void SyncGames(DatabaseContext db)
        {
            try
            {
                var gameService = new GameService();

                var gameIds = Filesystem.GetGameIds();


                foreach (var existingGame in db.Games)
                {
                    db.Remove(existingGame);
                }

                foreach (var existingDisc in db.Discs)
                {
                    db.Remove(existingDisc);
                }

                db.SaveChanges();

                var infos = gameIds.Select(id => gameService.GetGameInfo(id));

                foreach (var info in infos)
                {
                    var game = new Game()
                    {
                        Id = info.Id,
                        Title = info.Title,
                        Publisher = info.Publisher,
                        Year = info.Year,
                        Players = info.Players
                    };

                    game.Discs = info.DiscIds.Select((discId, index) => new Disc() { GameId = info.Id, DiscNumber = index + 1, DiscBasename = discId }).ToList();

                    db.Add(game);

                    Console.WriteLine($"Added game [{game.Id}] {game.Title} to the database");
                    Console.WriteLine("");
                }

                db.SaveChanges();

                Console.WriteLine($"Successfully inserted {gameIds.Count} games");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                Console.ReadLine();
            }
        }
    }
}
