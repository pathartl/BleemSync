using BleemSync.Central.Data;
using BleemSync.Central.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BleemSync.Central.Services
{
    public class GameService
    {
        private DatabaseContext _context { get; set; }

        public GameService(DatabaseContext context)
        {
            _context = context;
        }

        public List<GameDTO> Get()
        {
            var games = _context.Games.ToList();
            var gameDTOs = new List<GameDTO>();

            foreach (var game in games)
            {
                gameDTOs.Add(new GameDTO(game));
            }

            return gameDTOs;
        }

        public List<GameDTO> Get(int start, int length)
        {
            var games = _context.Games.Skip(start).Take(length).ToList();

            var gameDTOs = new List<GameDTO>();

            foreach (var game in games)
            {
                gameDTOs.Add(new GameDTO(game));
            }

            return gameDTOs;
        }

        public int GetTotal()
        {
            return _context.Games.Count();
        }

        public GameDTO GetGame(int id)
        {
            var game = _context.Games.Where(g => g.Id == id).FirstOrDefault();

            return new GameDTO(game);
        }

        public GameDTO GetGameBySerialNumber(string serialNumber)
        {
            var sanitized = serialNumber.Trim();

            var game = _context.Games.Where(g => g.Discs.Any(d => d.SerialNumber == sanitized)).FirstOrDefault();

            return new GameDTO(game);
        }
    }
}
