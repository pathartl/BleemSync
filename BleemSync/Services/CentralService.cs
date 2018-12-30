using BleemSync.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.Services
{
    public class BleemSyncCentralService
    {
        readonly RestClient _client;

        public BleemSyncCentralService()
        {
            _client = new RestClient("https://central.bleemsync.app/api");
        }

        public RestRequest Request(string endpoint)
        {
            return new RestRequest(endpoint);
        }

        public CentralGame GetPlayStationGameBySerial(string serial)
        {
            var result = _client.Execute<CentralGame>(Request($"PlayStation/GetBySerial/{serial}"));
            var game = JsonConvert.DeserializeObject<CentralGame>(result.Content);

            return game;
        }
    }
}
