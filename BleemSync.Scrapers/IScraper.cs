using BleemSync.Data.Models;
using BleemSync.Scrapers.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Scrapers
{
    public interface IScraper : IDisposable
    {
        GameInfo GetGame(string fingerprint);
    }
}
