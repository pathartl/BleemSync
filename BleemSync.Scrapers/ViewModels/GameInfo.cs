using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Scrapers.ViewModels
{
    public class GameInfo
    {
        public string Fingerprint { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }

        public DateTime DateReleased { get; set; }

        public string Region { get; set; }

        public string Players { get; set; }

        public string EsrbRating { get; set; }

        //[Display(Name = "ESRB Rating Descriptors")]
        //public virtual ICollection<EsrbRatingDescriptor> EsrbRatingDescriptors { get; set; }

        public string PegiRating { get; set; }

        //[Display(Name = "PEGI Rating Descriptors")]
        //public virtual ICollection<PegiRatingDescriptor> PegiDescriptors { get; set; }

        public bool OfficiallyLicensed { get; set; }

        // public Guid? GameConsoleId { get; set; }
        // public GameConsole GameConsole { get; set; }
    }
}
