using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.Extensions.PlayStationClassic.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharpConfig;

namespace BleemSync.Extensions.PlayStationClassic.Core.Areas.PlayStationClassic.Controllers
{
    [Area("PlayStationClassic")]
    [MenuSection(Icon = "tv", Name = "PlayStation Classic")]
    [Route("[area]/[controller]/[action]")]
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
                var preferencesString = System.IO.File.ReadAllText(_configuration["BleemSync:PlayStationClassic:SystemPreferencesPath"]);
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
            var path = _configuration["BleemSync:PlayStationClassic:SystemPreferencesPath"];

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
                var configuration = Configuration.LoadFromFile(_configuration["BleemSync:PlayStationClassic:PayloadConfigPath"]);
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

            submittedConfiguration.SaveToFile(_configuration["BleemSync:PlayStationClassic:PayloadConfigPath"]);

            return View(payloadConfig);
        }
    }
}
