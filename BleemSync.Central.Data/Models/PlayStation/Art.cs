using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.PlayStation
{
    [Table("PlayStation_Art")]
    public class Art : BaseArt
    {
        public ArtType Type { get; set; }
        public bool IsGreatestHits { get; set; }
        public virtual Disc Disc { get; set; }
        public virtual Game Game { get; set; }
    }

    public enum ArtType
    {
        Other,
        Front,
        Back,
        Inside,
        Inlay,
        Disc,
        GreatestHitsFront,
        GreatestHitsBack,
        GreatestHitsInside,
        GreatestHitsInlay,
        GreatestHitsDisc
    }
}
