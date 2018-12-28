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

namespace BleemSync.Controllers
{
    [MenuSection(Name = "Games", Icon = "gamepad", Position = 99999)]
    [Route("[controller]/[action]")]
    public class GamesController : Controller
    {
        private IGameManagerNodeRepository _gameManagerNodeRepository { get; set; }
        private IStorage _storage { get; set; }

        public GamesController(IStorage storage)
        {
            _gameManagerNodeRepository = storage.GetRepository<IGameManagerNodeRepository>();
            _storage = storage;
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
    }
}
