using BleemSync.Data.Entities;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Repositories
{
    public interface IGameRepository : IRepository
    {
        IEnumerable<Game> All();
    }
}
