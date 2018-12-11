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
            var directoriesToIgnore = new string[]
            {
                "databases",
                "system",
                "geninfo",
                "preferences"
            };

            var gamesDir = GetGamesDirectory();
            var dirList = Directory.GetDirectories(gamesDir).ToList();
            var filteredDirList = dirList.Where(d => !directoriesToIgnore.Contains(new DirectoryInfo(d).Name)).ToArray();
            var gameIds = new List<int>();

            foreach (var dir in dirList)
            {
                Console.WriteLine(dir);
            }

            // We want to ignore the databases folder from the validation
            if (IsValidGamesDirectoryStructure(filteredDirList))
            {
                for (int i = 0; i < filteredDirList.Length; i++)
                {
                    gameIds.Add(i + 1);
                }
            }
            else
            {
                throw new Exception("Your games directory structure is invalid. Make sure that folders are named as a whole number (e.g. 1, 2, 3) and there are no gaps in the sequence");
            }

            return gameIds;
        }

        public static bool IsValidGamesDirectoryStructure(string[] structure)
        {
            var validDirectoryStructure = true;

            for (int i = 0; i < structure.Length; i++)
            {
                if (Common.StringIsInt(structure[i]))
                {
                    if (Convert.ToInt32(new DirectoryInfo(structure[i]).Name) != i + 1)
                    {
                        validDirectoryStructure = false;
                    }
                } else
                {
                    validDirectoryStructure = false;
                }
            }

            return validDirectoryStructure;
        }

        public static string GetGamesDirectory()
        {
            var currentPath = GetExecutingDirectory();
            return Path.Join(currentPath, "..", "Games");
        }
    }
}
