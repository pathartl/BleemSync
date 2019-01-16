using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Attributes
{
    public class PreferencePropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
