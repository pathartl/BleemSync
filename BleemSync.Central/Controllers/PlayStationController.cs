using BleemSync.Central.Data;
using BleemSync.Central.Services;
using BleemSync.Central.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Central.Controllers
{
    [Route("[controller]/[action]")]
    public class PlayStationController : Controller
    {
        private DatabaseContext _context { get; set; }
        private GameService _service { get; set; }

        public PlayStationController(DatabaseContext context)
        {
            _context = context;
            _service = new GameService(_context);
        }

        public IActionResult Index()
        {
            var games = _service.Get(0, 10);

            return View(games);
        }

        [HttpPost]
        public JsonResult DataTable()
        {
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);

            var games = _service.Get(start, length);

            int filteredCount = _service.GetTotal();

            List<string[]> records = new List<string[]>();

            foreach (var game in games)
            {
                records.Add(new string[]
                {
                    game.Title,
                    game.Region,
                    game.Genre,
                    game.Developer,
                    game.Publisher,
                    game.DateReleased.ToString("MM/dd/yyyy"),
                    game.Players.ToString(),
                    String.Join(", ", game.Discs.Select(d => d.SerialNumber))
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
