using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models.PlayStation
{
    [Table("PlayStation_Games")]
    public class Game : BaseGame
    {
        [Display(Name = "Memory Card Blocks Used")]
        public int MemoryCardBlockCount { get; set; }

        [Display(Name = "Supports Multitap")]
        public bool MultitapCompatible { get; set; }

        [Display(Name = "Supports Link Cable")]
        public bool LinkCableCompatible { get; set; }

        [Display(Name = "Supports Vibration")]
        public bool VibrationCompatible { get; set; }

        [Display(Name = "Supports Analog Sticks")]
        public bool AnalogCompatible { get; set; }

        [Display(Name = "Supports Digital Input")]
        public bool DigitalCompatible { get; set; }

        [Display(Name = "Supports Light Guns")]
        public bool LightGunCompatible { get; set; }

        public virtual ICollection<Disc> Discs { get; set; }
        public virtual ICollection<Art> Art { get; set; }
        [InverseProperty("RevisedGame")]
        public virtual GameRevision Revision { get; set; }

        public Game() : base()
        {
            Discs = new List<Disc>();
            Art = new List<Art>();
        }
    }
}
