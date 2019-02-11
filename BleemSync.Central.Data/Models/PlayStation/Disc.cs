using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.PlayStation
{
    [Table("PlayStation_Discs")]
    public class Disc : BaseMedia
    {
        public int DiscNumber { get; set; }
        public virtual Game Game { get; set; }
        public int TrackCount { get; set; }
        public virtual ICollection<Art> Art { get; set; }

        public Disc() : base()
        {
            Art = new List<Art>();
        }
    }
}
