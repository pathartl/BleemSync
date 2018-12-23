using BleemSync.Extensions.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Controllers
{
    [MenuSection(Name = "Games", Icon = "settings", Position = 99999)]
    [Route("[controller]/[action]")]
    public class GameController : Controller
    {
        [MenuItem(Name = "Manage")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
