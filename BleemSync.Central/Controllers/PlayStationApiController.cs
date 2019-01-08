using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BleemSync.Central.Services;
using BleemSync.Central.Data;
using System.IO;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BleemSync.Central.Controllers
{
    [Route("api/PlayStation/[action]")]
    public class PlayStationApiController : Controller
    {
        private IConfiguration _configuration { get; set; }
        private GameService _service { get; set; }

        public PlayStationApiController(DatabaseContext context, IConfiguration configuration)
        {
            _service = new GameService(context);
            _configuration = configuration;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var game = _service.GetGame(id);

            return new JsonResult(game);
        }

        [HttpGet("{serial}")]
        public ActionResult GetBySerial(string serial)
        {
            var game = _service.GetGameBySerialNumber(serial);

            return new JsonResult(game);
        }

        [HttpGet("{serial}")]
        public ActionResult GetByFingerprint(string serial)
        {
            var game = _service.GetGameBySerialNumber(serial);

            return new JsonResult(game);
        }

        [HttpGet("{serial}")]
        public ActionResult GetCoverBySerial(string serial)
        {
            var game = _service.GetGameBySerialNumber(serial);
            var cover = game.Covers.First();

            var coverDirectory = _configuration["CoversPath"];

            return PhysicalFile(Path.Combine(coverDirectory, cover.File), "image/jpg");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
