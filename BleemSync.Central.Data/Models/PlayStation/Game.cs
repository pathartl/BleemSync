using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.PlayStation
{
    [Table("PlayStation_Games")]
    public class Game : BaseGame
    {
        public int MemoryCardBlockCount { get; set; }
        public bool MultitapCompatible { get; set; }
        public bool LinkCableCompatible { get; set; }
        public bool VibrationCompatible { get; set; }
        public bool AnalogCompatible { get; set; }
        public bool DigitalCompatible { get; set; }
        public virtual ICollection<Disc> Discs { get; set; }
    }
}
