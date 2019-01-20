using BleemSync.Data.Entities;
using ExtCore.Data.Abstractions;
using System.Collections.Generic;

namespace BleemSync.Data.Abstractions
{
    public interface IGameManagerNodeRepository : IRepository
    {
        IEnumerable<GameManagerNode> All();
        GameManagerNode Get(int id);
        void Create(GameManagerNode node);
        void Update(GameManagerNode node);
        void Delete(GameManagerNode node);
    }
}
