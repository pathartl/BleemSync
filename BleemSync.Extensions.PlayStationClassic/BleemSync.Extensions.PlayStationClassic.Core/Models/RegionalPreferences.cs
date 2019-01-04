using System;
using System.Collections.Generic;
using System.Text;
using BleemSync.Extensions.PlayStationClassic.Core.Attributes;

namespace BleemSync.Extensions.PlayStationClassic.Core.Models
{
    public class RegionalPreferences : Preference
    {
        public RegionalPreferences() { }
        public RegionalPreferences(string configString) : base(configString) { }

        [PreferenceProperty(Name = "iUiSystemSettingRegionId")]
        public int RegionId { get; set; }

        [PreferenceProperty(Name = "iUiSystemSettingDefaultLanguageId")]
        public int LanguageId { get; set; }

        [PreferenceProperty(Name = "bUiSystemSettingReverseEnterKey")]
        public bool ReverseEnterKey { get; set; }

        [PreferenceProperty(Name = "iUiUserSettingAutoPowerOff")]
        public int AutoPowerOff { get; set; }
    }
}
