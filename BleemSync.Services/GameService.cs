using BleemSync.Data.Models;
using System;
using SharpConfig;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using BleemSync.Utilities;
using RestSharp;
using BleemSync.Central.ViewModels;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace BleemSync.Services
{
    public class GameService
    {
        IConfigurationRoot _configuration { get; set; }

        public GameService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public GameInfo GetGameInfo(String gamePath)
        {
            GameInfo game;

            try
            {
                game = GetGameInfoFromFile(gamePath);
            }
            catch (Exception e) {
                Console.WriteLine("Game.ini doesn't exist, grabbing from BleemSync Central");

                game = GetGameInfoFromCentral(gamePath);
            }

            return game;
        }

        public GameInfo GetGameInfoFromFile(String gamePath)
        {
            var gamesDirectory = Filesystem.GetGamesDirectory(_configuration["GamesPath"]);

            Configuration config = null;

            var path = Path.Combine(new [] { gamesDirectory, gamePath, "Game.ini"});
            config = Configuration.LoadFromFile(path);

            var section = config["Game"];

            var game = new GameInfo()
            {
                RelativePath = gamePath,
                Title = section["Title"].StringValue,
                Publisher = section["Publisher"].StringValue,
                Year = section["Year"].IntValue,
                Players = section["Players"].IntValue,
                DiscIds = section["Discs"].StringValue.Split(',').ToList()
            };

            return game;
        }

        public void WriteGameInfoToFile(GameInfo gameInfo, string path)
        {
            var config = new Configuration();

            config["Game"]["Title"].StringValue = gameInfo.Title.TrimStart('"').TrimEnd('"');
            config["Game"]["Publisher"].StringValue = gameInfo.Publisher.TrimStart('"').TrimEnd('"');
            config["Game"]["Year"].StringValue = gameInfo.Year.ToString();
            config["Game"]["Players"].StringValue = gameInfo.Players.ToString();
            config["Game"]["Discs"].StringValue = String.Join(',', gameInfo.DiscIds);

            config.SaveToFile(Path.Combine(path, "Game.ini"));
        }

        public GameInfo GetGameInfoFromCentral(String gamePath)
        {
            var gamesDirectory = Filesystem.GetGamesDirectory(_configuration["GamesPath"]);

            var files = Directory.GetFiles(Path.Combine(gamesDirectory, gamePath));
            var discMap = new Dictionary<string, string>();
            var gameInfo = new GameInfo();

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                if (fileInfo.Extension == ".bin" || fileInfo.Extension == ".iso")
                {
                    try
                    {
                        var serial = DiscImage.GetSerialNumber(Path.Combine(gamesDirectory, gamePath, file));
                        
                        discMap.Add(serial, fileInfo.Name.Replace(fileInfo.Extension, ""));

                        Console.WriteLine($"Found serial {serial} from disc image file {file}");
                    }
                    catch {
                        Console.WriteLine($"Could not find a valid serial from the disc image {file}");
                    }
                }
            }

            try
            {
                var client = new RestClient(_configuration["BleemSyncCentralUrl"]);
                var request = new RestRequest($"api/PlayStation/GetBySerial/{discMap.Keys.First()}");
                var result = client.Execute<GameDTO>(request);

                var game = JsonConvert.DeserializeObject<GameDTO>(result.Content);

                gameInfo.RelativePath = gamePath;
                gameInfo.Title = game.Title;
                gameInfo.Year = game.DateReleased.Year;
                gameInfo.Publisher = game.Publisher;
                gameInfo.Players = game.Players;

                foreach (var serial in game.Discs.Select(d => d.SerialNumber))
                {
                    gameInfo.DiscIds.Add(discMap[serial]);
                }

                WriteGameInfoToFile(gameInfo, Path.Combine(gamesDirectory, gamePath));

                var coverFileName = gameInfo.DiscIds.First() + ".png";

                // Download the cover
                if (!File.Exists(Path.Combine(gamesDirectory, gamePath, coverFileName)))
                {
                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFile(
                                new Uri(_configuration["BleemSyncCentralUrl"] + "/api/PlayStation/GetCoverBySerial/" + discMap.Keys.First()),
                                Path.Combine(gamesDirectory, gamePath, coverFileName)
                            );
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Could not download the cover from BleemSync Central");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not grab game info for serial {discMap.Keys.First()}");
                throw;
            }

            return gameInfo;
        }
    }
}
