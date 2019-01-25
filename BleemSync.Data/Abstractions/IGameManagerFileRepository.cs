using BleemSync.Data.Entities;
using ExtCore.Data.Abstractions;
using System.Collections.Generic;

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
