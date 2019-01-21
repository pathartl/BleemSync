using ExtCore.Data.Entities.Abstractions;

namespace BleemSync.Data.Entities
{
    public class GameSystem : IEntity
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }
}
