using BleemSync.Central.Data;
using BleemSync.Central.Data.Models;
using BleemSync.Central.Data.Models.PlayStation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace BleemSync.Central.Services.Systems
{
    public class PlayStationService
    {
        public readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayStationService(DatabaseContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Game> GetGames()
        {
            return GetGames(g => g.IsActive).ToList();
        }

        public IEnumerable<Game> GetGames(int start, int length)
        {
            return GetGames(g => g.IsActive).Skip(start).Take(length).ToList();
        }

        public IQueryable<Game> GetGames(Expression<Func<Game, bool>> predicate)
        {
            return _context.PlayStation_Games.Where(predicate);
        }

        public int GetGamesCount()
        {
            return GetGames(g => g.IsActive).Count();
        }

        public Game GetGame(int id)
        {
            return GetGames(g => g.IsActive).SingleOrDefault(g => g.Id == id);
        }

        public Game GetGameByFingerprint(string fingerprint)
        {
            var sanitizedFingerprint = fingerprint.Trim();

            var game = GetGames(g => g.Fingerprint == sanitizedFingerprint && g.IsActive).FirstOrDefault();

            if (game == null)
            {
                game = _context.PlayStation_Discs.Where(d => d.Fingerprint == sanitizedFingerprint && d.Game.IsActive).FirstOrDefault().Game;
            }

            return game;
        }

        public IEnumerable<GameRevision> GetGameRevisions()
        {
            return GetGameRevisions(gr => true).ToList();
        }

        public IQueryable<GameRevision> GetGameRevisions(Expression<Func<GameRevision, bool>> predicate)
        {
            return _context.PlayStation_GameRevisions.Where(predicate)
                .Include(gr => gr.Game)
                .Include(gr => gr.RevisedGame)
                .Include(gr => gr.SubmittedBy);
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

        public async void ReviseGameAsync(Game game)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var originalGameId = game.Id;

            game.Id = 0;

            _context.Add(game);
            _context.SaveChanges();

            var revision = new GameRevision()
            {
                GameId = originalGameId,
                SubmittedOn = DateTime.Now,
                SubmittedBy = user,
                RevisedGameId = game.Id
            };

            _context.Add(revision);
            _context.SaveChanges();
        }

        public async void RejectGameRevision(int id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var gameRevision = GetGameRevisions(gr => gr.Id == id).FirstOrDefault();

            gameRevision.RejectedOn = DateTime.Now;
            gameRevision.RejectedBy = user;
            gameRevision.RevisedGame.IsActive = false;

            _context.Update(gameRevision);
            _context.SaveChanges();
        }

        public async void ApproveGameRevision(int id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var gameRevision = GetGameRevisions(gr => gr.Id == id).FirstOrDefault();

            gameRevision.ApprovedOn = DateTime.Now;
            gameRevision.ApprovedBy = user;
            gameRevision.Game.IsActive = false;
            gameRevision.RevisedGame.IsActive = true;

            _context.Update(gameRevision);
            _context.SaveChanges();
        }
    }
}
