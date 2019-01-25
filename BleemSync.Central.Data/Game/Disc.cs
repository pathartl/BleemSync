using System.ComponentModel.DataAnnotations;

namespace BleemSync.Central.Data.Models
{
    public class Disc
    {
        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public virtual Game Game { get; set; }
    }
}
