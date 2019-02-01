using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    public class BaseArt
    {
        [Key]
        public int Id { get; set; }
        public string File { get; set; }
        public decimal Rating { get; set; }
        public int RatingVoteCount { get; set; }
    }
}
