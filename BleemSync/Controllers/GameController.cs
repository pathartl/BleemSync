using BleemSync.Extensions.Infrastructure.Attributes;
using BleemSync.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BleemSync.Data.Entities;
using System.Net;
using System.IO;
using BleemSync.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using BleemSync.Services;

namespace BleemSync.Controllers
{
    [MenuSection(Name = "Games", Icon = "gamepad", Position = 99999)]
    [Route("[controller]/[action]")]
    public class GamesController : Controller
    {
        private readonly GameManagerNodeService _gameManagerNodeService;
        private readonly GameManagerFileService _gameManagerFileService;
        private readonly IGameManagerService _gameManagerService;
        private readonly IConfiguration _configuration;

        public GamesController(GameManagerNodeService gameManagerNodeService, GameManagerFileService gameManagerFileService, IGameManagerService gameManagerService, IConfiguration configuration)
        {
            _gameManagerNodeService = gameManagerNodeService;
            _gameManagerFileService = gameManagerFileService;
            _gameManagerService = gameManagerService;
            _configuration = configuration;
        }

        [MenuItem(Name = "Manage")]
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult GetTree()
        {
            var tree = _gameManagerNodeService
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
        public ActionResult SaveTree(GameManagerNodeTreeSaveRequest request)
        {
            var dbNodes = _gameManagerNodeService.All();

            var position = 0;
            foreach (var node in request.Nodes)
            {
                int id;

                if (int.TryParse(node.Id, out id))
                {
                    var dbNode = dbNodes.Single(n => n.Id == Convert.ToInt32(node.Id));

                    dbNode.Position = position;
                    dbNode.Name = node.Text;

                    if (node.Parent == "#")
                    {
                        dbNode.ParentId = null;
                    }
                    else
                    {
                        dbNode.ParentId = Convert.ToInt32(node.Parent);
                    }

                    _gameManagerNodeService.Update(dbNode, false);
                }
                else
                {
                    _gameManagerNodeService.Create(node.ToGameManagerNode(), false);
                }

                position++;
            }

            _gameManagerNodeService.Save();
            _gameManagerService.UpdateGames(dbNodes);
            _gameManagerService.GenerateFolders();

            return Json("Tree updated!");
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var game = _gameManagerNodeService.Get(id);

            return new JsonResult(game);
        }

        [HttpGet("{id}")]
        public ActionResult GetLocalCoverByGameId(int id)
        {
            var coverFile = _gameManagerFileService.All().Where(f => f.NodeId == id && f.Name.EndsWith(".png")).FirstOrDefault();

            if (coverFile != null)
                return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), coverFile.Path), "image/jpg");
            else
                return NoContent();
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> AddGame()
        {
            var temporaryFiles = new List<GameManagerFile>();
            var formModel = await Request.StreamFile(
                Path.Combine(
                    _configuration["BleemSync:Destination"],
                    _configuration["BleemSync:TemporaryPath"]
                ), temporaryFiles);

            GameUpload gameUpload = new GameUpload();
            var bindingSuccessful = await TryUpdateModelAsync(gameUpload, prefix: "", valueProvider: formModel);
            if (!bindingSuccessful)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            if (gameUpload.Cover != null && gameUpload.Cover != "")
            {
                var file = new GameManagerFile()
                {
                    Name = "cover.png",
                    Path = Path.GetTempFileName()
                };

                var splitted = gameUpload.Cover.Split(",");
                if (splitted.Length > 1)
                {
                    System.IO.File.WriteAllBytes(file.Path, Convert.FromBase64String(splitted[1]));

                    temporaryFiles.Add(file);
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

            _gameManagerNodeService.Create(node);
            _gameManagerNodeService.Save();

            _gameManagerService.AddGame(node);
            _gameManagerService.GenerateFolders();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Game added!");
        }

        [HttpPost]
        public ActionResult ModifyGame(GameUpload gameUpload, string UpdateAction)
        {
            ActionResult result = Json("");

            switch (UpdateAction)
            {
                case "Delete":
                    result = DeleteGame(gameUpload);
                    break;
                case "Update":
                    result = UpdateGame(gameUpload);
                    break;
            }

            return result;
        }

        private ActionResult UpdateGame(GameUpload gameUpload)
        {
            var node = _gameManagerNodeService.Get(gameUpload.Id);

            node.Name = gameUpload.Name;
            node.SortName = gameUpload.SortName;
            node.Description = gameUpload.Description;
            node.ReleaseDate = gameUpload.ReleaseDate;
            node.Players = gameUpload.Players;
            node.Developer = gameUpload.Developer;
            node.Publisher = gameUpload.Publisher;
            node.Type = GameManagerNodeType.Game;

            _gameManagerNodeService.Update(node);

            if (gameUpload.Cover != null && gameUpload.Cover != "")
            {
                var coverFile = _gameManagerFileService.All().Where(f => f.NodeId == node.Id && f.Name.EndsWith(".png")).First();

                System.IO.File.WriteAllBytes(coverFile.Path, Convert.FromBase64String(gameUpload.Cover.Split(",")[1]));
            }

            _gameManagerService.UpdateGame(node);
            _gameManagerService.GenerateFolders();

            return Json("Game updated!");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteGame(int id)
        {
            var node = _gameManagerNodeService.Get(id);

            _gameManagerNodeService.Delete(node);
            _gameManagerService.DeleteGame(node);
            _gameManagerService.GenerateFolders();

            return Json("Node deleted!");
        }

        private ActionResult DeleteGame(GameUpload gameUpload)
        {
            return DeleteGame(gameUpload.Id);
        }
    }
}
