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
        public ActionResult GetByFingerprint(string system, string fingerprint)
        {
            Game game = new Game();
            switch (system)
            {
                case "PlayStation":
                    var bscGame = _bleemSyncCentral.GetPlayStationGameBySerial(fingerprint);
                    game = new Game(bscGame);
                    break;
            }

            return new JsonResult(game);
        }
    }
}
