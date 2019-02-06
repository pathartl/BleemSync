using BleemSync.Data.Abstractions;
using BleemSync.Data.Entities;
using ExtCore.Data.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Data.Repositories
{
    public class GameManagerNodeRepository : RepositoryBase<GameManagerNode>, IGameManagerNodeRepository
    {
        public IEnumerable<GameManagerNode> All()
        {
            return dbSet.OrderBy(n => n.SortName != "" ? n.SortName : n.Name).Include(n => n.Files);
        }

        public GameManagerNode Get(int id)
        {
            return dbSet.SingleOrDefault(n => n.Id == id);
        }

        public void Create(GameManagerNode node)
        {
            dbSet.Add(node);
        }

        public void Update(GameManagerNode node)
        {
            storageContext.Entry(node).State = EntityState.Modified;
        }

        public void Delete(GameManagerNode node)
        {
            storageContext.Database.ExecuteSqlCommand($"DELETE FROM GameManagerFiles WHERE NodeId = {node.Id};");
            dbSet.Remove(node);
        }
    }
}
