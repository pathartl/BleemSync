using BleemSync.Data;
using BleemSync.Data.Models;
using BleemSync.Services;
using BleemSync.Utilities;
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
            var gameService = new GameService();

            var gameIds = Filesystem.GetGameIds();

            using (var db = new DatabaseContext())
            {
                foreach (var existingGame in db.Games)
                {
                    db.Remove(existingGame);
                }

                foreach (var existingDisc in db.Discs)
                {
                    db.Remove(existingDisc);
                }

                db.SaveChanges();

                foreach (var id in gameIds)
                {
                    var gameInfo = gameService.GetGameInfo(id);

                    var game = new Game()
                    {
                        Id = id,
                        Title = gameInfo.Title,
                        Publisher = gameInfo.Publisher,
                        Year = gameInfo.Year,
                        Players = gameInfo.Players
                    };

                    var i = 1;

                    foreach (var discId in gameInfo.DiscIds)
                    {
                        var disc = new Disc()
                        {
                            GameId = id,
                            DiscNumber = i,
                            DiscBasename = discId
                        };

                        i++;

                        db.Add(disc);
                    }

                    db.Add(game);
                }

                db.SaveChanges();
            }
        }
    }
}
