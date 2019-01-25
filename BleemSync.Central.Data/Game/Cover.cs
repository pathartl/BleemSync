using System.ComponentModel.DataAnnotations;

namespace BleemSync.Central.Data.Models
{
    public class Cover
    {
        [Key]
        public int Id { get; set; }
        public string File { get; set; }
        public float Rating { get; set; }
        public virtual Game Game { get; set; }
    }
}
