using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.ViewModels
{
    public class CentralDisc
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public virtual CentralGame Game { get; set; }
    }
}
