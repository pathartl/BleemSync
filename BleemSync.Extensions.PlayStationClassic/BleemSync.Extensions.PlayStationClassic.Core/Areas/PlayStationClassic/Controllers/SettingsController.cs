using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.Extensions.PlayStationClassic.Core.Models;
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
            //var config = Configuration.LoadFromFile("regional.pre");
            var configString = System.IO.File.ReadAllText("regional.pre");

            var readPreferences = new SystemPreferences(configString);

            var preferences = new SystemPreferences()
            {
                RegionId = 1,
                LanguageId = 2,
                ReverseEnterKey = true,
                AutoPowerOff = 1
            };

            var prefstring = preferences.ToString();
            return View();
        }
    }
}
