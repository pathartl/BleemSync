using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    public enum PegiRating
    {
        Unknown,
        Pegi3,
        Pegi7,
        Pegi12,
        Pegi16,
        Pegi18
    }

    public class PegiRatingDescriptor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
