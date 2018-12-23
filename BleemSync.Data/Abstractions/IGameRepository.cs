using BleemSync.Data.Entities;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Abstractions
{
    public interface IGameRepository : IRepository
    {
        IEnumerable<Game> All();
        Game Add(Game game);
    }
}
