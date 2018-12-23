using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BleemSync.Models;
using BleemSync.Services;
using BleemSync.Data.Abstractions;
using ExtCore.Data.Abstractions;
using BleemSync.Data.Entities;

namespace BleemSync.Controllers
{
    public class HomeController : Controller
    {
        MenuService _menuService { get; set; }
        private IGameRepository _gameRepository { get; set; }
        private IStorage _storage { get; set; }

        public HomeController(MenuService menuService, IStorage storage)
        {
            _menuService = menuService;
            _gameRepository = storage.GetRepository<IGameRepository>();
            _storage = storage;
        }

        public IActionResult Index()
        {
            var items = _menuService.GetMenuItems();
            var games = _gameRepository.All();

            var game = new Game()
            {
                Title = "Tony Hawk's Pro Skater 2",
                SortTitle = "Tony Hawk 2",
                Developer = "Neversoft",
                Publisher = "Activision",
                Players = 2,
                ReleaseDate = DateTime.Now
            };

            _gameRepository.Add(game);
            _storage.Save();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
