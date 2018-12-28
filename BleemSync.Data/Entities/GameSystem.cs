using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Entities
{
    public class GameSystem : IEntity
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }
}
