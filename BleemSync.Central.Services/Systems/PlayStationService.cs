using BleemSync.Central.Data;
using BleemSync.Central.Data.Models;
using BleemSync.Central.Data.Models.PlayStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Central.Services.Systems
{
    public class PlayStationService
    {
        public readonly DatabaseContext _context;

        public PlayStationService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Game> GetGames()
        {
            return _context.PlayStation_Games.Where(g => g.IsActive).ToList();
        }

        public IEnumerable<Game> GetGames(int start, int length)
        {
            return _context.PlayStation_Games.Where(g => g.IsActive).Skip(start).Take(length).ToList();
        }

        public int GetGamesCount()
        {
            return _context.PlayStation_Games.Where(g => g.IsActive).Count();
        }

        public Game GetGame(int id)
        {
            return _context.PlayStation_Games.Where(g => g.IsActive).SingleOrDefault(g => g.Id == id);
        }

        public Game GetGameByFingerprint(string fingerprint)
        {
            var sanitizedFingerprint = fingerprint.Trim();

            var game = _context.PlayStation_Games.Where(g => g.Fingerprint == sanitizedFingerprint && g.IsActive).FirstOrDefault();

            if (game == null)
            {
                game = _context.PlayStation_Discs.Where(d => d.Fingerprint == sanitizedFingerprint && d.Game.IsActive).FirstOrDefault().Game;
            }

            return game;
        }

        public void AddGame(Game game)
        {
            _context.Add(game);
            _context.SaveChanges();
        }

        public void UpdateGame(Game game)
        {
            _context.Update(game);
            _context.SaveChanges();
        }

        public void ReviseGame(Game game)
        {
            var originalGameId = game.Id;

            game.Id = 0;

            _context.Add(game);
            _context.SaveChanges();

            var revision = new GameRevision()
            {
                GameId = originalGameId,
                SubmittedOn = DateTime.Now,
                // SubmittedBy
                RevisedGameId = game.Id
            };

            _context.Add(revision);
            _context.SaveChanges();
        }
    }
}
