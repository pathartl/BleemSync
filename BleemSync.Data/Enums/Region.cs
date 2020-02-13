using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BleemSync.Data.Enums
{
    public enum Region
    {
        [Display(Name = "Region Free")]
        RegionFree,
        [Display(Name = "NTSC - Japan")]
        NTSC_J,
        [Display(Name = "NTSC - North America")]
        NTSC_U,
        [Display(Name = "PAL")]
        PAL,
        [Display(Name = "NTSC - China")]
        NTSC_C
    }
}
