using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.Extensions.PlayStationClassic.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Areas.PlayStationClassic.Controllers
{
    [Area("PlayStationClassic")]
    [MenuSection(Icon = "theaters", Name = "PlayStation Classic")]
    public class SettingsController : Controller
    {
        private IConfiguration _configuration { get; set; }

        public SettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [MenuItem(Name = "System Preferences")]
        [HttpGet]
        public ActionResult SystemPreferences()
        {
            SystemPreferences preferences;

            try
            {
                var preferencesString = System.IO.File.ReadAllText(_configuration["PlayStationClassic:SystemPreferencesPath"]);
                preferences = new SystemPreferences(preferencesString);
            } catch
            {
                preferences = new SystemPreferences();
            }

            return View(preferences);
        }

        [HttpPost]
        public ActionResult SystemPreferences(SystemPreferences preferences)
        {
            var path = _configuration["PlayStationClassic:SystemPreferencesPath"];

            System.IO.File.WriteAllText(path, preferences.ToString());

            return View(preferences);
        }

        [MenuItem(Name = "BleemSync Preferences")]
        [HttpGet]
        public ActionResult BleemSyncPreferences()
        {
            PayloadConfig payloadConfig;

            try
            {
                var configuration = Configuration.LoadFromFile(_configuration["PlayStationClassic:PayloadConfigPath"]);
                payloadConfig = new PayloadConfig(configuration);
            } catch
            {
                payloadConfig = new PayloadConfig();
            }

            return View(payloadConfig);
        }

        [HttpPost]
        public ActionResult BleemSyncPreferences(PayloadConfig payloadConfig)
        {
            var submittedConfiguration = payloadConfig.ToConfiguration();

            return View(payloadConfig);
        }
    }
}
