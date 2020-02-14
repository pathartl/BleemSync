using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Scrapers.ViewModels.Central
{
    public class Cover
    {
        public int Id { get; set; }
        public string File { get; set; }
        public float Rating { get; set; }
        public virtual Game Game { get; set; }
    }
}
