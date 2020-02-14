using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Scrapers.ViewModels.Central
{
    public class Game
    {
        public int Id { get; set; }
        public string RelativePath { get; set; }
        public string Title { get; set; }
        public string CommonTitle { get; set; }
        public string Region { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public int Players { get; set; }
        public virtual ICollection<Disc> Discs { get; set; }
        public virtual ICollection<Cover> Covers { get; set; }
    }
}
