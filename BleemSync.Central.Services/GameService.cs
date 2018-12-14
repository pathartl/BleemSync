using BleemSync.Central.Data;
using BleemSync.Central.Data.Models;
using BleemSync.Central.ViewModels;
using System;
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
