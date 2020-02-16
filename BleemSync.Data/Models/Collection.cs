using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Models
{
    public class Collection
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }

        public ICollection<CollectionGame> CollectionGames { get; set; }
    }
}
