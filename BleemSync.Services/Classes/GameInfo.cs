using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Data.Models
{
    public class GameInfo
    {
        public int Id { get; set; }
	      public String RelativePath { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public int Players { get; set; }
        public List<string> DiscIds { get; set; }

        public GameInfo()
        {
            DiscIds = new List<string>();
        }
    }
}
