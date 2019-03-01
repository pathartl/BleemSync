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

        [Display(Name = "Date Released")]
        public DateTime DateReleased { get; set; }

        public GameRegion Region { get; set; }

        [Display(Name = "Number of Players")]
        public string Players { get; set; }

        [Display(Name = "ESRB Rating")]
        public EsrbRating EsrbRating { get; set; }

        [Display(Name = "ESRB Rating Descriptors")]
        public virtual ICollection<EsrbRatingDescriptor> EsrbRatingDescriptors { get; set; }

        [Display(Name = "PEGI Rating")]
        public PegiRating PegiRating { get; set; }

        [Display(Name = "PEGI Rating Descriptors")]
        public virtual ICollection<PegiRatingDescriptor> PegiDescriptors { get; set; }

        [Display(Name = "Officially Licensed")]
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
        [Display(Name = "Region Free")]
        RegionFree,
        [Display(Name = "NTSC - Japan")]
        NTSC_J,
        [Display(Name = "NTSC - North America")]
        NTSC_U,
        [Display(Name = "PAL")]
        PAL,
        [Display(Name = "NTSC - China")]
        NTSC_C
    }
}
