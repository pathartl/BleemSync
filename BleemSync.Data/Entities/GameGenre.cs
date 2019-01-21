using ExtCore.Data.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace BleemSync.Data.Entities
{
    public class GameGenre : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
