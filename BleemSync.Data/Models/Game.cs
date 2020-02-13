using BleemSync.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Data.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Fingerprint { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        
        [Display(Name = "Date Released")]
        public DateTime DateReleased { get; set; }

        public Region Region { get; set; }

        [Display(Name = "Number of Players")]
        public string Players { get; set; }

        [Display(Name = "ESRB Rating")]
        public EsrbRating EsrbRating { get; set; }

        //[Display(Name = "ESRB Rating Descriptors")]
        //public virtual ICollection<EsrbRatingDescriptor> EsrbRatingDescriptors { get; set; }

        [Display(Name = "PEGI Rating")]
        public PegiRating PegiRating { get; set; }

        //[Display(Name = "PEGI Rating Descriptors")]
        //public virtual ICollection<PegiRatingDescriptor> PegiDescriptors { get; set; }

        [Display(Name = "Officially Licensed")]
        public bool OfficiallyLicensed { get; set; }

        public string Path { get; set; }

        // public Guid? GameConsoleId { get; set; }
        // public GameConsole GameConsole { get; set; }
    }
}
