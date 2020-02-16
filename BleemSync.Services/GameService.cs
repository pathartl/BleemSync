using BleemSync.Data;
using BleemSync.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Services
{
    public class GameService : DataService
    {
        public Game Get(Guid id)
        {
            return DatabaseContext.Games.FirstOrDefault(g => g.Id == id);
        }

        public IQueryable<Game> Get()
        {
            return DatabaseContext.Games.AsQueryable();
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
    }
}
