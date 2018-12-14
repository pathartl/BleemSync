using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Central.ViewModels
{
    public class DiscDTO
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public virtual GameDTO Game { get; set; }
    }
}
