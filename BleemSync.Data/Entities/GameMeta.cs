using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BleemSync.Data.Entities
{
    [Table("GameMeta")]
    public class GameMeta {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public virtual GameManagerNode Game { get; set; }
    }
}
