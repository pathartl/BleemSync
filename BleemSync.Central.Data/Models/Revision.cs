using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    [NotMapped]
    public abstract class Revision
    {
        [Key]
        public int Id { get; set; }
        public virtual ApplicationUser SubmittedBy { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public virtual ApplicationUser ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public virtual ApplicationUser RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
    }
}
