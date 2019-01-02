using BleemSync.Extensions.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Controllers
{
    [MenuSection(Icon = "theaters", Name = "PlayStation Classic")]
    public class PlayStationClassicController : Controller
    {
        [MenuItem(Name = "Settings")]
        public ActionResult Index()
        {
            return Json("Hello World!");
        }
    }
}
