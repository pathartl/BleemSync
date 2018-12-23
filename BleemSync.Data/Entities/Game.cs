using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Data.Entities
{
    public class Game : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string SortTitle { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Players { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public virtual List<GameGenre> Genres { get; set; }
        public virtual List<GameMeta> Meta { get; set; }
    }
}
