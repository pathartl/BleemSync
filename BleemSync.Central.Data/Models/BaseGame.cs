using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BleemSync.Central.Data.Models
{
    [NotMapped]
    public class BaseGame
    {
        [Key]
        public int Id { get; set; }
        public string Fingerprint { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public virtual ICollection<GameGenre> Genres { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public GameRegion Region { get; set; }
        public string Players { get; set; }

        public EsrbRating EsrbRating { get; set; }
        public virtual ICollection<EsrbRatingDescriptor> EsrbRatingDescriptors { get; set; }

        public PegiRating PegiRating { get; set; }
        public virtual ICollection<PegiRatingDescriptor> PegiDescriptors { get; set; }
        public bool OfficiallyLicensed { get; set; }
        public bool IsActive { get; set; }

        public BaseGame()
        {
            Genres = new List<GameGenre>();
            EsrbRatingDescriptors = new List<EsrbRatingDescriptor>();
            PegiDescriptors = new List<PegiRatingDescriptor>();
        }
    }

    public enum GameRegion
    {
        RegionFree,
        NTSC_J,
        NTSC_U,
        PAL,
        NTSC_C
    }
}
