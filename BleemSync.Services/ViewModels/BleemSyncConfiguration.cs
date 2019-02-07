using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Services.ViewModels
{
    public class BleemSyncConfiguration
    {
        public string Destination { get; set; }
        public string Path { get; set; }
        public string TemporaryPath { get; set; }
        public string DatabaseFile { get; set; }
        public PlayStationClassicConfiguration PlayStationClassic { get; set; }
        public BleemSyncCentralConfiguration Central { get; set; }
    }

    public class PlayStationClassicConfiguration
    {
        public string GamesDirectory { get; set; }
        public string SystemPreferencesPath { get; set; }
        public string PayloadConfigPath { get; set; }
        public string DatabaseFile { get; set; }
    }

    public class BleemSyncCentralConfiguration
    {
        public string BaseUrl { get; set; }
    }
}
