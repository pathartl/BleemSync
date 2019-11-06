using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BleemSync.Central.Data.Models.MegaDrive;
using BleemSync.Central.Services.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BleemSync.Central.Web.Areas.Systems.Controllers
{
    [Area("Systems")]
    [Route("[area]/[controller]/[action]")]
    public class MegaDriveController : Controller
    {
        public readonly MegaDriveService _service;

        public MegaDriveController(MegaDriveService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var game = _service.GetGame(id);

            return View(game);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var game = _service.GetGame(id);

            return View(game);
        }

        [HttpPost("{id}")]
        [Authorize]
        public IActionResult Edit(Game game)
        {
            _service.ReviseGameAsync(game);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Moderation()
        {
            var games = _service.GetGameRevisions(gr => gr.ApprovedBy == null && gr.SubmittedBy != null && gr.RejectedBy == null).ToList();

            return View(games);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Moderator")]
        public IActionResult ReviewChanges(int id)
        {
            var gameRevision = _service.GetGameRevisions(gr => gr.Id == id).FirstOrDefault();

            return View(gameRevision);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Moderator")]
        public IActionResult RejectChanges(int id)
        {
            _service.RejectGameRevision(id);

            return RedirectToAction("Moderation");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Moderator")]
        public IActionResult ApproveChanges(int id)
        {
            _service.ApproveGameRevision(id);

            return RedirectToAction("Moderation");
        }

        public IActionResult DataTable()
        {
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);

            var games = _service.GetGames(g => g.IsActive).OrderBy(g => g.Title).Skip(start).Take(length).ToList();

            int filteredCount = _service.GetGamesCount();

            List<string[]> records = new List<string[]>();

            foreach (var game in games)
            {
                records.Add(new string[]
                {
                    game.Fingerprint,
                    game.Title,
                    $"<div class=\"text-right\"><a href=\"{Url.Action("Details", new { Id = game.Id })}\" class=\"btn btn-primary\">More Info</a></div>"
                });
            }

            dynamic result = new
            {
                draw = Request.Form["draw"],
                recordsTotal = filteredCount,
                recordsFiltered = filteredCount,
                data = records
            };

            return Json(result);
        }
    }
}