using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BleemSync.Models;
using BleemSync.Services;

namespace BleemSync.Controllers
{
    public class HomeController : Controller
    {
        MenuService _menuService { get; set; }

        public HomeController(MenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            var items = _menuService.GetMenuItems();
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
