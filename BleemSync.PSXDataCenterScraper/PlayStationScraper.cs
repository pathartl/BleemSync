using BleemSync.Central.Data;
using BleemSync.Central.Data.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BleemSync.PSXDataCenterScraper
{
    public class PlayStationScraper
    {
        private DatabaseContext _context { get; set; }

        public PlayStationScraper(DatabaseContext context)
        {
            _context = context;
        }

        public void ScrapeMainList(string url)
        {
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;

            var html = web.Load(url);
            var dom = html.DocumentNode;

            var links = dom.QuerySelectorAll("[href]");

            var games = new List<BaseGame>();

            Directory.CreateDirectory("covers");

            foreach (var link in links)
            {
                //try
                //{
                    var game = GetGame("http://10.0.1.12/psxdatacenter/psxdatacenter.com/" + link.GetAttributeValue("href", ""));

                    _context.Add(game);
                    _context.SaveChanges();
                //}
                //catch (Exception e) {
                //    Console.WriteLine($"Could not scrape {link}");
                //}
            }

            Console.WriteLine("Done! Press any key to continue.");
            Console.ReadLine();
        }

        private Central.Data.Models.PlayStation.Game GetGame(string url)
        {
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.GetEncoding("iso-8859-1");

            var html = web.Load(url.Replace("https", "http"));
            var dom = html.DocumentNode;

            var metaTable = dom.QuerySelector("#table4");
            var featuresTable = dom.QuerySelector("#table19");
            var discsTable = dom.QuerySelector("#table7");

            var blockCount = 0;
            var blockCountText = GetContent(dom.QuerySelector("#table19 tr:nth-child(2) td:nth-child(2)"));
            var multitap = GetContent(dom.QuerySelector("#table19 tr:nth-child(8) td:nth-child(2)")).ToLower() == "yes";
            var linkCable = GetContent(dom.QuerySelector("#table19 tr:nth-child(9) td:nth-child(2)")).ToLower() == "yes";
            var vibration = GetContent(dom.QuerySelector("#table19 tr:nth-child(7) td:nth-child(2)")).ToLower() == "yes";
            var analog = GetContent(dom.QuerySelector("#table19 tr:nth-child(3) td:nth-child(2)")).ToLower().Contains("analog");
            var digital = GetContent(dom.QuerySelector("#table19 tr:nth-child(3) td:nth-child(2)")).ToLower().Contains("standard");
            var lightGun = !GetContent(dom.QuerySelector("#table19 tr:nth-child(4) td:nth-child(2)")).ToLower().Contains("none");

            Match match = Regex.Match(blockCountText, @"(?:\d\-|)(\d)", RegexOptions.IgnoreCase);
            
            if (match.Success)
            {
                blockCount = Convert.ToInt32(match.Groups[1].Value);
            }

            var serial = GetContent(dom.QuerySelector("#table4 tr:nth-child(3) td:nth-child(2)")).Split(" / ").First();

            var game = new Central.Data.Models.PlayStation.Game();
            game.Title = GetContent(metaTable.QuerySelector("tr:nth-child(1) td:nth-child(2)"));
            game.Description = GetContent(dom.QuerySelector("#table16 tr:nth-child(1) td:nth-child(1)"));
            game.Version = "";
            game.Genres = new List<GameGenre>();
            game.Developer = GetContent(metaTable.QuerySelector("tr:nth-child(6) td:nth-child(2)")).TrimEnd('.');
            game.Publisher = GetContent(metaTable.QuerySelector("tr:nth-child(7) td:nth-child(2)")).TrimEnd('.');
            game.Players = GetContent(featuresTable.QuerySelector("tr:nth-child(1) td:nth-child(2)"));
            game.OfficiallyLicensed = true;
            game.MemoryCardBlockCount = blockCount;
            game.MultitapCompatible = multitap;
            game.LinkCableCompatible = linkCable;
            game.VibrationCompatible = vibration;
            game.AnalogCompatible = analog;
            game.DigitalCompatible = digital;
            game.LightGunCompatible = lightGun;
            game.Fingerprint = serial;

            // Get Date
            var dateString = GetContent(metaTable.QuerySelector("tr:nth-child(8) td:nth-child(2)"));

            if (DateTime.TryParse(dateString, out var dateReleased))
            {
                game.DateReleased = dateReleased;
            }

            #region Region
            // Get Region
            var regionString = GetContent(metaTable.QuerySelector("tr:nth-child(4) td:nth-child(2)"));

            switch (regionString)
            {
                case "NTSC-U":
                    game.Region = GameRegion.NTSC_U;
                    break;

                case "PAL":
                    game.Region = GameRegion.PAL;
                    break;

                case "NTSC-J":
                    game.Region = GameRegion.NTSC_J;
                    break;

                default:
                    game.Region = GameRegion.RegionFree;
                    break;
            }
            #endregion

            #region Rating
            // Get rating
            var ratingImage = metaTable.QuerySelector("[src*=\"rating/esrb\"]");

            if (ratingImage != null)
            {
                var imageFileInfo = new FileInfo(ratingImage.Attributes.Where(a => a.Name == "src").First().Value);

                switch (imageFileInfo.Name)
                {
                    case "esrb-e.gif":
                        game.EsrbRating = EsrbRating.Everyone;
                        break;

                    case "esrb-e10.gif":
                        game.EsrbRating = EsrbRating.Everyone10Plus;
                        break;

                    case "esrb-ao.gif":
                        game.EsrbRating = EsrbRating.AdultsOnly;
                        break;

                    case "esrb-t.gif":
                        game.EsrbRating = EsrbRating.Teen;
                        break;

                    case "esrb-m.gif":
                        game.EsrbRating = EsrbRating.Mature;
                        break;

                    default:
                        game.EsrbRating = EsrbRating.Unknown;
                        break;
                }
            }
            #endregion

            #region Genres
            var genres = new List<GameGenre>();
            var genreStrings = GetContent(metaTable.QuerySelector("tr:nth-child(5) td:nth-child(2)")).Split(" / ");

            var genresInDb = _context.Genres.Where(g => genreStrings.Contains(g.Name)).ToList();
            var genreStringsNotInDb = genreStrings.Where(gs => !genresInDb.Select(g => g.Name).Contains(gs)).ToList();

            genres.AddRange(genresInDb);

            foreach (var genreString in genreStringsNotInDb)
            {
                var genre = new GameGenre()
                {
                    Name = genreString
                };

                genres.Add(genre);
            }
            #endregion

            #region Art
            // Jewel covers first
            for (int i = 1; i <= 6; i++)
            {
                var jewelCoverType = GetContent(dom.QuerySelector($"#table28 tr:nth-child(2) td:nth-child({i})"));
                var jewelCoverDescription = GetContent(dom.QuerySelector($"#table28 tr:nth-child(4) td:nth-child({i})"));

                if (jewelCoverType.Trim() != "" && !jewelCoverDescription.Trim().ToUpper().Contains("MISSING"))
                {
                    var art = new Central.Data.Models.PlayStation.Art() {
                        Id = Guid.NewGuid()
                    };

                    var imgNode = dom.QuerySelector($"#table28 tr:nth-child(3) td:nth-child({i}) img");

                    if (imgNode != null)
                    {
                        var imgSrc = imgNode.GetAttributeValue("src", "");
                        var imgHiResPath = imgSrc.Replace("thumbs", "hires");
                        var imgFile = new FileInfo(imgHiResPath).Name;
                        var imgExtension = new FileInfo(imgHiResPath).Extension;

                        art.File = art.Id.ToString() + imgExtension;

                        switch (jewelCoverType.Trim().ToUpper())
                        {
                            case "FRONT":
                                art.Type = Central.Data.Models.PlayStation.ArtType.Front;
                                break;

                            case "BACK":
                                art.Type = Central.Data.Models.PlayStation.ArtType.Back;
                                break;

                            case "INLAY":
                                art.Type = Central.Data.Models.PlayStation.ArtType.Inlay;
                                break;

                            case "INSIDE":
                                art.Type = Central.Data.Models.PlayStation.ArtType.Inside;
                                break;

                            case "DISC":
                                art.Type = Central.Data.Models.PlayStation.ArtType.Disc;
                                break;

                            default:
                                art.Type = Central.Data.Models.PlayStation.ArtType.Other;
                                break;
                        }

                        var outputArtDir = Path.Combine("art", "playstation", game.Fingerprint, Enum.GetName(typeof(Central.Data.Models.PlayStation.ArtType), art.Type));

                        Directory.CreateDirectory(outputArtDir);

                        try
                        {
                            using (WebClient wc = new WebClient())
                            {
                                var currentPage = new FileInfo(url).Name;
                                var pageLessPath = url.Replace(currentPage, "");

                                wc.DownloadFile(
                                    new Uri(pageLessPath + imgHiResPath),
                                    Path.Combine(outputArtDir, art.File)
                                );
                            }
                        }
                        catch { }

                        game.Art.Add(art);
                    }
                }
            }
            #endregion

            #region Discs
            for (int i = 1; i <= 6; i++)
            {
                var node = dom.QuerySelector($"#table7 tr:nth-child(1) td:nth-child({i + 1})");
                
                if (GetContent(node) != "")
                {
                    var discSerial = GetContentLines(node).Last();
                    var trackCount = GetContentLines(dom.QuerySelector($"#table7 tr:nth-child(4) td:nth-child({i + 1})")).First();

                    var disc = new Central.Data.Models.PlayStation.Disc()
                    {
                        DiscNumber = i,
                        Fingerprint = discSerial,
                        TrackCount = Convert.ToInt32(trackCount)
                    };

                    game.Discs.Add(disc);
                }
            }
            #endregion

            Console.WriteLine($"Grabbed info for [{game.Title}]");

            return game;
        }

        private IEnumerable<string> GetContentLines(HtmlNode node)
        {
            try
            {
                var content = node.InnerText
                    .Replace("\r\n", "\n")
                    .Replace("\t", "")
                    .Replace("&nbsp;", "")
                    .Trim()
                    .Split("\n");

                return content;
            }
            catch
            {
                return new List<string>();
            }
        }

        private string GetContent(HtmlNode node)
        {
            try
            {
                var content = node.InnerText
                    .Replace("\t", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("&nbsp;", "")
                    .Trim();

                return content;
            } catch
            {
                return "";
            }
        }

        private int GetPlayerCount(string input)
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
