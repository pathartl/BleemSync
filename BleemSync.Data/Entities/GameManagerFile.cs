using ExtCore.Data.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BleemSync.Data.Entities
{
    public class GameManagerFile : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
