using BleemSync.Central.Data.Models;

namespace BleemSync.Central.ViewModels
{
    public class CoverDTO
    {
        public int Id { get; set; }
        public string File { get; set; }
        public float Rating { get; set; }

        public virtual GameDTO Game { get; set; }

        public CoverDTO(Cover cover)
        {
            Id = cover.Id;
            File = cover.File;
            Rating = cover.Rating;
        }
    }
}
