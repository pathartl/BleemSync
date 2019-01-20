using System.ComponentModel.DataAnnotations;

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
