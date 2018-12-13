using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Scrapers.PSXDataCenterScraper.Data.Models
{
    public class Disc
    {
        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public virtual Game Game { get; set; }
    }
}
