using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.MegaDrive
{
    [Table("MegaDrive_Games")]
    public class Game : BaseGame
    {
        public virtual ICollection<Cartridge> Cartridges { get; set; }
        public virtual ICollection<Art> Art { get; set; }
        [InverseProperty("RevisedGame")]
        public virtual GameRevision Revision { get; set; }

        public Game() : base()
        {
            Cartridges = new List<Cartridge>();
            Art = new List<Art>();
        }
    }
}
