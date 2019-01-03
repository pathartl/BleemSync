using BleemSync.Extensions.Infrastructure.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Areas.PlayStationClassic.Controllers
{
    [Area("PlayStationClassic")]
    [MenuSection(Icon = "theaters", Name = "PlayStation Classic")]
    public class SettingsController : Controller
    {
        [MenuItem(Name = "Settings")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
