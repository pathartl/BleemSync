using BleemSync.Central.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Central.ViewModels
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CommonTitle { get; set; }
        public string Region { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public int Players { get; set; }
        public virtual ICollection<DiscDTO> Discs { get; set; }

        public GameDTO(Game game)
        {
            Id = game.Id;
            Title = game.Title;
            CommonTitle = game.CommonTitle;
            Region = game.Region;
            Genre = game.Genre;
            Developer = game.Developer;
            Publisher = game.Publisher;
            DateReleased = game.DateReleased;
            Players = game.Players;
            Discs = new List<DiscDTO>();

            foreach (var disc in game.Discs)
            {
                Discs.Add(new DiscDTO(disc));
            }
        }
    }
}
