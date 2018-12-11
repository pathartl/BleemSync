using BleemSync.Data.Models;
using System;
using SharpConfig;
using System.Linq;
using System.IO;

namespace BleemSync.Services
{
    public class GameService
    {
        public GameInfo GetGameInfo(int gameId)
        {
            var gamesDirectory = Utilities.Filesystem.GetGamesDirectory();

            Configuration config = null;

            try
            {
                var path = Path.Combine(new [] { gamesDirectory, gameId.ToString(), "GameData", "Game.ini"});
                config = Configuration.LoadFromFile(path);
            } catch {
                throw new Exception($"Game.ini not found for game with id {gameId}");
            }

            var section = config["Game"];

            var game = new GameInfo()
            {
                Id = gameId,
                Title = section["Title"].StringValue,
                Publisher = section["Publisher"].StringValue,
                Year = section["Year"].IntValue,
                Players = section["Players"].IntValue,
                DiscIds = section["Discs"].StringValue.Split(',').ToList()
            };

            return game;
        }
    }
}
