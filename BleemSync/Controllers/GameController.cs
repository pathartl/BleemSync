using BleemSync.Data.Abstractions;
using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.ViewModels;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BleemSync.Services;
using BleemSync.Data.Entities;
using System.Net;
using System.Net.Mime;

namespace BleemSync.Controllers
{
    [MenuSection(Name = "Games", Icon = "gamepad", Position = 99999)]
    [Route("[controller]/[action]")]
    public class GamesController : Controller
    {
        private IGameManagerNodeRepository _gameManagerNodeRepository { get; set; }
        private IStorage _storage { get; set; }
        private BleemSyncCentralService _bleemSyncCentral { get; set; }

        public GamesController(IStorage storage, BleemSyncCentralService bleemSyncCentral)
        {
            _gameManagerNodeRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _storage = storage;
            _bleemSyncCentral = bleemSyncCentral;
        }

        [MenuItem(Name = "Manage")]
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult GetTree()
        {
            var tree = _gameManagerNodeRepository
                .All()
                .Select(n => new GameManagerNodeTreeItem(n));

            return new JsonResult(tree, new JsonSerializerSettings() {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });
        }

        [HttpPost]
        public void SaveTree(GameManagerNodeTreeSaveRequest request)
        {
            var dbNodes = _gameManagerNodeRepository.All();

            foreach (var node in request.Nodes)
            {
                var dbNode = dbNodes.Single(n => n.Id == Convert.ToInt32(node.Id));

                if (node.Parent == "#")
                {
                    dbNode.ParentId = null;
                }
                else
                {
                    dbNode.ParentId = Convert.ToInt32(node.Parent);
                }
            }

            _storage.Save();
        }

        [HttpGet]
        public ActionResult GetBySerial(string serial)
        {
            Game game;
            try
            {
                var bscGame = _bleemSyncCentral.GetPlayStationGameBySerial(serial);
                game = new Game(bscGame);
            }
            catch
            {
                game = new Game();
            }

            return new JsonResult(game);
        }

        [HttpGet]
        public ActionResult AddGame(string serial)
        {
            Game game;
            try
            {
                var bscGame = _bleemSyncCentral.GetPlayStationGameBySerial(serial);
                game = new Game(bscGame);
            }
            catch
            {
                game = new Game();
            }

            return View(game);
        }

        [HttpPost]
        public ActionResult AddGame(string serial, Game game)
        {
            var node = new GameManagerNode()
            {
                Name = game.Name,
                SortName = game.SortName,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate,
                Players = game.Players,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Type = GameManagerNodeType.Game
            };

            _gameManagerNodeRepository.Create(node);
            _storage.Save();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Game added!");
        }
    }
}
