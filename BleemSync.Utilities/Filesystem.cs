using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BleemSync.Utilities
{
    public class Filesystem
    {
        public static string GetExecutingDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path);
        }

        public static List<int> GetGameIds()
        {
            var gamesDir = GetGamesDirectory();
            var dirList = Directory.GetDirectories(gamesDir).Select(path => new DirectoryInfo(path).Name);

            var gameIds = dirList.Select(directoryName =>
            {
                if (int.TryParse(directoryName, out var gameId)) {
                    return gameId;
                }

                throw new Exception($"Found game with invalid id: {directoryName}");
            }).ToList();

            gameIds.Sort();

            if (gameIds.First() != 1 && gameIds.Last() != gameIds.Count())
            {
                throw new Exception("Your games directory structure is invalid. Make sure that folders are named as a whole number (e.g. 1, 2, 3) and there are no gaps in the sequence");
            }

            return gameIds;
        }
        public static string GetGamesDirectory()
        {
            var currentPath = GetExecutingDirectory();
            return Path.Join(currentPath, "..", "Games");
        }
    }
}
