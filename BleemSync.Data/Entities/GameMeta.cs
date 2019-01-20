using ExtCore.Data.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BleemSync.Data.Entities
{
    public class GameMeta : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public virtual GameManagerNode Game { get; set; }
    }
}
