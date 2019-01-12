using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Central.ViewModels
{
    public class GameDTO
    {
        public int Id { get; set; }
	      public String RelativePath { get; set; }
        public string Title { get; set; }
        public string CommonTitle { get; set; }
        public string Region { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public int Players { get; set; }
        public virtual ICollection<DiscDTO> Discs { get; set; }
        public virtual ICollection<CoverDTO> Covers { get; set; }
    }
}
