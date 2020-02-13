using BleemSync.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Data.Models
{
    public class Platform
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Manufacturer { get; set; }
        public Region Region { get; set; }

        public List<Game> Games { get; set; }
    }
}
