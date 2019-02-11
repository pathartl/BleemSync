using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    public enum EsrbRating
    {
        Unknown,
        Everyone,
        Everyone10Plus,
        Teen,
        Mature,
        AdultsOnly
    }

    public class EsrbRatingDescriptor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
