using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BleemSync.Central.Data.Models.PlayStation;
using BleemSync.Central.Services.Systems;
using Microsoft.AspNetCore.Mvc;

namespace BleemSync.Central.Web.Areas.System.Controllers
{
    [Area("System")]
    [Route("[area]/[controller]/[action]")]
    public class PlayStationController : Controller
    {
        public readonly PlayStationService _service;

        public PlayStationController(PlayStationService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var games = _service.GetGames();

            return View(games);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var game = _service.GetGame(id);

            return View(game);
        }

        [HttpPost("{id}")]
        public IActionResult Edit(Game game)
        {
            _service.ReviseGame(game);

            return RedirectToAction("Index");
        }
    }
}