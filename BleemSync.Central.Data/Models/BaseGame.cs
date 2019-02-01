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
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public List<GameGenre> Genres { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public GameRegion Region { get; set; }
        public string Players { get; set; }
        public EsrbRating EsrbRating { get; set; }
        public List<string> EsrbRatingDescriptors { get; set; }
        public PegiRating PegiRating { get; set; }
        public List<string> PegiDescriptors { get; set; }
        public bool OfficiallyLicensed { get; set; }
    }

    public enum GameRegion
    {
        RegionFree,
        NTSC_J,
        NTSC_U,
        PAL,
        NTSC_C
    }

    public enum EsrbRating
    {
        Unknown,
        Everyone,
        Everyone10Plus,
        Teen,
        Mature,
        AdultsOnly
    }

    public enum PegiRating
    {
        Unknown,
        Pegi3,
        Pegi7,
        Pegi12,
        Pegi16,
        Pegi18
    }
}
