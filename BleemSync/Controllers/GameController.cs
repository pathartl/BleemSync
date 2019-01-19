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
        private IGameManagerFileRepository _gameManagerFileRepository { get; set; }
        private IStorage _storage { get; set; }
        private BleemSyncCentralService _bleemSyncCentral { get; set; }
        private IGameManagerService _gameManagerService { get; set; }

        public GamesController(IStorage storage, BleemSyncCentralService bleemSyncCentral, IGameManagerService gameManagerService)
        {
            _gameManagerNodeRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _gameManagerFileRepository = storage.GetRepository<IGameManagerFileRepository>();
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
                .OrderBy(n => n.Position)
                .ThenBy(n => n.SortName)
                .ThenBy(n => n.Name)
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

            var position = 0;
            foreach (var node in request.Nodes)
            {
                var dbNode = dbNodes.Single(n => n.Id == Convert.ToInt32(node.Id));

                dbNode.Position = position;

                if (node.Parent == "#")
                {
                    dbNode.ParentId = null;
                }
                else
                {
                    dbNode.ParentId = Convert.ToInt32(node.Parent);
                }

                position++;
            }

            _storage.Save();
            _gameManagerService.UpdateGames(dbNodes);
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

        [HttpGet("{id}")]
        public ActionResult GetLocalCoverByGameId(int id)
        {
            var coverFile = _gameManagerFileRepository.All().Where(f => f.NodeId == id && f.Name.EndsWith(".png")).First();

            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), coverFile.Path), "image/jpg");
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

            if (gameUpload.Cover != null && gameUpload.Cover != "")
            {
                var file = new GameManagerFile()
                {
                    Name = "cover.png",
                    Path = Path.GetTempFileName()
                };

                System.IO.File.WriteAllBytes(file.Path, Convert.FromBase64String(gameUpload.Cover.Split(",")[1]));

                temporaryFiles.Add(file);
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
        public async Task<ActionResult> ModifyGame(GameUpload gameUpload, string UpdateAction)
        {
            ActionResult result = Json("");

            switch (UpdateAction)
            {
                case "Delete":
                    result = DeleteGame(gameUpload);
                    break;
                case "Update":
                    result = await UpdateGame(gameUpload);
                    break;
            }

            return result;
        }

        private async Task<ActionResult> UpdateGame(GameUpload gameUpload)
        {
            var node = _gameManagerNodeRepository.Get(gameUpload.Id);

            node.Name = gameUpload.Name;
            node.SortName = gameUpload.SortName;
            node.Description = gameUpload.Description;
            node.ReleaseDate = gameUpload.ReleaseDate;
            node.Players = gameUpload.Players;
            node.Developer = gameUpload.Developer;
            node.Publisher = gameUpload.Publisher;
            node.Type = GameManagerNodeType.Game;

            if (gameUpload.Cover != null)
            {
                var coverFile = _gameManagerFileRepository.All().Where(f => f.NodeId == node.Id && f.Name.EndsWith(".png")).First();

                using (var stream = new FileStream(coverFile.Path, FileMode.Create))
                {
                    //await gameUpload.Cover.CopyToAsync(stream);
                }
            }

            _storage.Save();

            _gameManagerService.UpdateGame(node);

            return Json("Game updated!");
        }

        private ActionResult DeleteGame(GameUpload gameUpload)
        {
            var node = _gameManagerNodeRepository.Get(gameUpload.Id);
            _gameManagerNodeRepository.Delete(node);
            _storage.Save();
            _gameManagerService.DeleteGame(node);

            return Json("Game deleted!");
        }
    }
}
