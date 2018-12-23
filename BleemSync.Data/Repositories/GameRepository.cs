using BleemSync.Data.Abstractions;
using BleemSync.Data.Entities;
using ExtCore.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Data.Repositories
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public IEnumerable<Game> All()
        {
            return dbSet.OrderBy(g => g.SortTitle != "" ? g.SortTitle : g.Title);
        }

        public Game Add(Game game)
        {
            return dbSet.Add(game).Entity;
        }
    }
}
