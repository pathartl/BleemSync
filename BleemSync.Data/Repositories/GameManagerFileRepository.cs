using BleemSync.Data.Abstractions;
using BleemSync.Data.Entities;
using ExtCore.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BleemSync.Data.Repositories
{
    public class GameManagerFileRepository : RepositoryBase<GameManagerFile>, IGameManagerFileRepository
    {
        public IEnumerable<GameManagerFile> All()
        {
            return dbSet.AsEnumerable();
        }

        public GameManagerFile Get(int id)
        {
            return dbSet.SingleOrDefault(n => n.Id == id);
        }

        public void Create(GameManagerFile file)
        {
            dbSet.Add(file);
        }

        public void Update(GameManagerFile file)
        {
            storageContext.Entry(file).State = EntityState.Modified;
        }

        public void Delete(GameManagerFile file)
        {
            dbSet.Remove(file);
        }
    }
}
