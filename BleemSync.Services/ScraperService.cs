using BleemSync.Data.Models;
using BleemSync.Scrapers;
using BleemSync.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Services
{
    public class ScraperService
    {
        public Game ScrapeGame(string fingerprint)
        {
            Game game = null;

            var scraperDefinitions = Reflection.TypesImplementingInterface(typeof(IScraper)).Where(t => !t.IsInterface);

            foreach (var scraperDefinition in scraperDefinitions)
            {
                IScraper scraper = Activator.CreateInstance(scraperDefinition) as IScraper;

                if (scraper != null && game == null)
                {
                    var gameInfo = scraper.GetGame(fingerprint);

                    if (gameInfo != null)
                    {
                        game = new Game();

                        game.Fingerprint = fingerprint;
                        game.Title = gameInfo.Title;
                        game.Developer = gameInfo.Developer;
                        game.Publisher = gameInfo.Publisher;
                        game.DateReleased = gameInfo.DateReleased;
                        game.Players = gameInfo.Players;
                    }
                }

                scraper.Dispose();
            }

            return game;
        }
    }
}
