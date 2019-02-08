using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BleemSync.Data.Entities
{
    [Table("GameManagerFiles")]
    public class GameManagerFile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int NodeId { get; set; }
        public virtual GameManagerNode Node { get; set; }
    }
}
