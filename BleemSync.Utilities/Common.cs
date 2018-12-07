using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BleemSync.Utilities
{
    public static class Common
    {
        public static bool StringIsInt(string input)
        {
            var name = new DirectoryInfo(input).Name;
            var isInt = false;

            try
            {
                int num = Convert.ToInt32(name);
                isInt = true;
            }
            catch { }

            return isInt;
        }
    }
}
