﻿using BleemSync.Data;
using BleemSync.Data.Models;
using BleemSync.Services;
using BleemSync.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

            var databaseLocation = configuration["GamesDatabase"];

            Directory.CreateDirectory(new FileInfo(databaseLocation).Directory.FullName);

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite($"Data Source={databaseLocation}")
            );

            var serviceProvider = services.BuildServiceProvider();
            var _context = serviceProvider.GetService<DatabaseContext>();

            SyncGames(_context, configuration);
        }

        static void SyncGames(DatabaseContext db, IConfigurationRoot configuration)
        {
            try
            {
                var gameService = new GameService(configuration);

                var gamePaths = Filesystem.GetGamePaths(configuration["GamesPath"]);

                // Clean up old database
                foreach (var existingGame in db.Games)
                {
                    db.Remove(existingGame);
                }

                foreach (var existingDisc in db.Discs)
                {
                    db.Remove(existingDisc);
                }

                db.SaveChanges();

                var infos = new List<GameInfo>();
                var idx_count = 1;
                foreach (var path in gamePaths)
                {
                    try
                    {
                        GameInfo info = gameService.GetGameInfo(path);
                        info.Id = idx_count;
                        infos.Add(info);
                        Console.WriteLine("");
                        idx_count++;
                    }
                    catch
                    {

                    }
                }

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
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }

                Console.WriteLine($"Successfully inserted {gamePaths.Count} games");
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
