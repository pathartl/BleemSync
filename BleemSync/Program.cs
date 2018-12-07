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
                        Title = gameInfo.Title,
                        Publisher = gameInfo.Publisher,
                        Year = gameInfo.Year,
                        Players = gameInfo.Players
                    };

                    var disc = new Disc()
                    {
                        Id = id,
                        DiscNumber = 1,
                        DiscId = gameInfo.DiscId
                    };

                    db.Add(game);
                    db.Add(disc);
                }

                db.SaveChanges();
            }
        }
    }
}
