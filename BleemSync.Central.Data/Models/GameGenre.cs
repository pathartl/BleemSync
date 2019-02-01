using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    public class GameGenre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
