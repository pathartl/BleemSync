using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BleemSync.Data.Entities
{
    [Table("GameGenre")]
    public class GameGenre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
