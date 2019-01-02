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
using BleemSync.Extensions.Infrastructure.Attributes;

namespace BleemSync.Controllers
{
    [MenuSection(Name = "Home", Icon = "home", Position = 999999)]
    public class HomeController : Controller
    {
        private IGameManagerNodeRepository _gameRepository { get; set; }
        private IStorage _storage { get; set; }

        public HomeController(IStorage storage)
        {
            _gameRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _storage = storage;
        }

        public IActionResult Index()
        {
            return View();
        }

        [MenuItem(Name = "About")]
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
