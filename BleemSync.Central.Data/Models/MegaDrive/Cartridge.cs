using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.MegaDrive
{
    [Table("MegaDrive_Cartridges")]
    public class Cartridge : BaseMedia
    {
        public virtual Game Game { get; set; }
        public virtual ICollection<Art> Art { get; set; }

        public Cartridge() : base()
        {
            Art = new List<Art>();
        }
    }
}
