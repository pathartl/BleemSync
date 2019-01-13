using BleemSync.Data.Entities;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Abstractions
{
    public interface IGameManagerFileRepository : IRepository
    {
        IEnumerable<GameManagerFile> All();
        GameManagerFile Get(int id);
        void Create(GameManagerFile file);
        void Update(GameManagerFile file);
        void Delete(GameManagerFile file);
    }
}
