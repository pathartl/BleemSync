using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using BleemSync.Scrapers.PSXDataCenterScraper.Data;
using BleemSync.Scrapers.PSXDataCenterScraper.Data.Models;

namespace BleemSync.Scrapers.PSXDataCenterScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            ScrapeMainList("https://psxdatacenter.com/jlist.html");
        }

        static void ScrapeMainList(string url)
        {
            HtmlWeb web = new HtmlWeb();

            var html = web.Load(url);
            var dom = html.DocumentNode;

            var links = dom.QuerySelectorAll("[href]");

            var games = new List<Game>();

            using (var db = new DatabaseContext())
            {
                foreach (var link in links)
                {
                    var game = GetGame("https://psxdatacenter.com/" + link.GetAttributeValue("href", ""));

                    db.Add(game);

                    db.SaveChanges();
                }
            }
        }

        static Game GetGame(string url)
        {
            HtmlWeb web = new HtmlWeb();

            var html = web.Load(url.Replace("https", "http"));
            var dom = html.DocumentNode;

            var metaTable = dom.QuerySelector("#table4");
            var featuresTable = dom.QuerySelector("#table19");
            var discsTable = dom.QuerySelector("#table7");

            var game = new Game()
            {
                Title = GetContent(metaTable.QuerySelector("tr:nth-child(1) td:nth-child(2)")),
                CommonTitle = GetContent(metaTable.QuerySelector("tr:nth-child(2) td:nth-child(2)")),
                Region = GetContent(metaTable.QuerySelector("tr:nth-child(4) td:nth-child(2)")),
                Genre = GetContent(metaTable.QuerySelector("tr:nth-child(5) td:nth-child(2)")),
                Developer = GetContent(metaTable.QuerySelector("tr:nth-child(6) td:nth-child(2)")).TrimEnd('.'),
                Publisher = GetContent(metaTable.QuerySelector("tr:nth-child(7) td:nth-child(2)")).TrimEnd('.'),
                Players = GetPlayerCount(GetContent(featuresTable.QuerySelector("tr:nth-child(1) td:nth-child(2)"))),
                Discs = new List<Disc>()
            };

            var serialNumbers = new List<string>();

            for (int i = 2; i <= 7; i++)
            {
                var cell = GetContent(discsTable.QuerySelector($"tr:nth-child(2) td:nth-child({i})"));

                if (cell != "")
                {
                    serialNumbers.Add(cell);
                }
            }

            foreach (var serialNumber in serialNumbers)
            {
                var disc = new Disc()
                {
                    SerialNumber = serialNumber,
                    Game = game,
                };

                game.Discs.Add(disc);
            }

            var dateString = GetContent(metaTable.QuerySelector("tr:nth-child(8) td:nth-child(2)"));

            if (DateTime.TryParse(dateString, out var dateReleased))
            {
                game.DateReleased = dateReleased;
            }

            Console.WriteLine($"Grabbed info for [{game.Title}]");

            return game;
        }

        static string GetContent(HtmlNode node)
        {
            var content = node.InnerText;

            return content
                .Replace("\t", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("&nbsp;", "");
        }

        static int GetPlayerCount(string input)
        {
            var numberString = new string(input.Where(c => char.IsDigit(c)).ToArray());

            int count = 0;

            if (int.TryParse(numberString, out var number))
            {
                switch (number)
                {
                    case 12:
                    case 2:
                        count = 2;
                        break;

                    case 13:
                    case 23:
                    case 3:
                        count = 4;
                        break;

                    case 14:
                    case 24:
                    case 34:
                    case 4:
                        count = 4;
                        break;

                    default:
                        count = 1;
                        break;
                }
            }

            return count;
        }
    }
}
