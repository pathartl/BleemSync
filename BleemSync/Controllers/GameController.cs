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
using System.IO;
using BleemSync.Services.Abstractions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace BleemSync.Controllers
{
    [MenuSection(Name = "Games", Icon = "gamepad", Position = 99999)]
    [Route("[controller]/[action]")]
    public class GamesController : Controller
    {
        private IGameManagerNodeRepository _gameManagerNodeRepository { get; set; }
        private IStorage _storage { get; set; }
        private BleemSyncCentralService _bleemSyncCentral { get; set; }
        private IGameManagerService _gameManagerService { get; set; }

        public GamesController(IStorage storage, BleemSyncCentralService bleemSyncCentral, IGameManagerService gameManagerService)
        {
            _gameManagerNodeRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _storage = storage;
            _bleemSyncCentral = bleemSyncCentral;
            _gameManagerService = gameManagerService;
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

        [HttpGet("{serial}")]
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

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var game = _gameManagerNodeRepository.Get(id);

            return new JsonResult(game);
        }

        [HttpPost]
        public async Task<ActionResult> AddGame(GameUpload gameUpload)
        {
            var temporaryFiles = new List<GameManagerFile>();

            foreach (var file in gameUpload.Files)
            {
                long size = file.Length;
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    temporaryFiles.Add(new GameManagerFile()
                    {
                        Name = file.FileName,
                        Path = filePath
                    });
                }
            }

            // Add game to the database
            var node = new GameManagerNode()
            {
                Name = gameUpload.Name,
                SortName = gameUpload.SortName,
                Description = gameUpload.Description,
                ReleaseDate = gameUpload.ReleaseDate,
                Players = gameUpload.Players,
                Developer = gameUpload.Developer,
                Publisher = gameUpload.Publisher,
                Type = GameManagerNodeType.Game,
                Files = temporaryFiles
            };

            _gameManagerNodeRepository.Create(node);
            _storage.Save();

            _gameManagerService.AddGame(node);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Game added!");
        }

        [HttpPost]
        public ActionResult UpdateGame(GameManagerNode game)
        {
            game.Type = GameManagerNodeType.Game;
            _gameManagerNodeRepository.Update(game);

            _storage.Save();

            _gameManagerService.UpdateGame(game);

            return Json("Game updated!");
        }

        [HttpPost]
        public ActionResult DeleteGame(GameManagerNode game)
        {
            _gameManagerNodeRepository.Delete(game);
            _storage.Save();
            _gameManagerService.DeleteGame(game);

            return Json("Game deleted!");
        }
    }
}
