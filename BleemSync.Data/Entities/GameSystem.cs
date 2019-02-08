using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BleemSync.Data.Entities
{
    [Table("GameSystem")]
    public class GameSystem
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
    }
}
