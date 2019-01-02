using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BleemSync.ViewModels
{
    public class GameUpload : Game
    {
        [Display(Name = "Upload Game")]
        public IEnumerable<IFormFile> Files { get; set; }

        public GameUpload()
        {

        }
    }
}
