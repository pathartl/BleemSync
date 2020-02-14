using BleemSync.Scrapers.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BleemSync.Scrapers
{
    public class CentralScraper : IScraper
    {
        private const string BaseUrl = "https://central.bleemsync.app/api";
        private WebClient Client { get; set; }

        public CentralScraper()
        {
            Client = new WebClient();
            Client.Headers["User-Agent"] = "BleemSync.Scraper/1.0.0";
        }

        public GameInfo GetGame(string fingerprint)
        {
            var gameInfo = new GameInfo();

            var centralGame = Request<ViewModels.Central.Game>($"PlayStation/GetBySerial/{fingerprint}");

            gameInfo.Fingerprint = fingerprint;
            gameInfo.Title = centralGame.Title;
            gameInfo.Region = centralGame.Region;
            gameInfo.Developer = centralGame.Developer;
            gameInfo.Publisher = centralGame.Publisher;
            gameInfo.DateReleased = centralGame.DateReleased;
            gameInfo.Players = centralGame.Players.ToString();

            return gameInfo;
        }

        private T Request<T>(string endpoint)
        {
            var json = Client.DownloadString($"{BaseUrl}/{endpoint}");

            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
