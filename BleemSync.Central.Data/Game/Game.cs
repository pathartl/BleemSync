using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BleemSync.Central.Data.Models
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string CommonTitle { get; set; }
        public string Region { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public int Players { get; set; }
        public virtual ICollection<Disc> Discs { get; set; }
        public virtual ICollection<Cover> Covers { get; set; }
    }
}
