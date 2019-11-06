using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.MegaDrive
{
    [Table("MegaDrive_Art")]
    public class Art : BaseArt
    {
        public ArtType Type { get; set; }
        public virtual Cartridge Cartridge { get; set; }
        public virtual Game Game { get; set; }
    }

    public enum ArtType
    {
        Other,
        Front,
        Back,
        Spine,
        Full,
        Cartridge
    }
}
