using BleemSync.Services;
using BleemSync.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class GameInfoController : ControllerBase
    {
        private BleemSyncCentralService _bleemSyncCentral { get; set; }

        public GameInfoController(BleemSyncCentralService bleemSyncCentral)
        {
            _bleemSyncCentral = bleemSyncCentral;
        }

        [HttpGet("{system}/{fingerprint}")]
        public ActionResult GetCoverByFingerprint(string system, string fingerprint)
        {
            var coverUrl = "";

            switch (system)
            {
                case "PlayStation":
                    coverUrl = _bleemSyncCentral.GetPlayStationCoverBySerial(fingerprint);
                    break;
            }

            return Redirect(coverUrl);
        }
    }
}
