using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    [NotMapped]
    public class BaseArt
    {
        [Key]
        public Guid Id { get; set; }
        public string File { get; set; }
        public decimal Rating { get; set; }
        public int RatingVoteCount { get; set; }
    }
}
