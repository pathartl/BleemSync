using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Data.Models
{
    [Table("DISC")]
    public class Disc
    {
        [Key, Column("DISC_ID")]
        public int DiscId { get; set; }

        [Column("GAME_ID")]
        public int GameId { get; set; }

        [Column("DISC_NUMBER")]
        public int DiscNumber { get; set; }

        [Column("BASENAME")]
        public string DiscBasename { get; set; }
        
        public Game Game { get; set; }
    }
}
