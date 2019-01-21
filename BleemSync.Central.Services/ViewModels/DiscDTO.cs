using BleemSync.Central.Data.Models;

namespace BleemSync.Central.ViewModels
{
    public class DiscDTO
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }

        public virtual GameDTO Game { get; set; }

        public DiscDTO(Disc disc)
        {
            Id = disc.Id;
            SerialNumber = disc.SerialNumber;
        }
    }
}
