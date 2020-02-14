using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Scrapers.ViewModels.Central
{
    public class Disc
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public virtual Game Game { get; set; }
    }
}
