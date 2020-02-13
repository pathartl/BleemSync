using BleemSync.Data;
using BleemSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Services
{
    public class GameService : IDisposable
    {
        private DatabaseContext DatabaseContext { get; set; }

        public GameService()
        {
            DatabaseContext = new DatabaseContext();
            DatabaseContext.Database.EnsureCreated();
            DatabaseContext.Database.Migrate();
        }

        public Game Add(Game game)
        {
            game = DatabaseContext.Add(game).Entity;

            DatabaseContext.SaveChanges();

            return game;
        }

        public void AddRange(IEnumerable<Game> games)
        {
            DatabaseContext.AddRange(games);
            DatabaseContext.SaveChanges();
        }

        public void Dispose()
        {
            DatabaseContext.Dispose();
        }
    }
}
