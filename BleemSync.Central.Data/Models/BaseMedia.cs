using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    public class BaseMedia
    {
        [Key]
        public int Id { get; set; }
        public string Fingerprint { get; set; }
    }
}
