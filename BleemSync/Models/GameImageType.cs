using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Models
{
    public enum GameImageType
    {
        [FileExtensions(Extensions = "bin,cue")]
        BinCue = 1,

        [FileExtensions(Extensions = "pbp")]
        Eboot,

        [FileExtensions(Extensions = "iso")]
        Iso,

        [FileExtensions(Extensions = "chd")]
        Chd
    }
}
