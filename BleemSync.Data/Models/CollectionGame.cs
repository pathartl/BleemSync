using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Models
{
    public class CollectionGame
    {
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
