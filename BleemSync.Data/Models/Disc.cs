using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Data.Models
{
    [Table("DISC")]
    public class Disc
    {
        [Column("GAME_ID")]
        public int Id { get; set; }

        [Column("DISC_NUMBER")]
        public int DiscNumber { get; set; }

        [Column("BASENAME")]
        public string DiscId { get; set; }
    }
}
