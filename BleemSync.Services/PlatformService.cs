using BleemSync.Data.Enums;
using BleemSync.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Services
{
    public class PlatformService : DataService
    {
        public void Initialize()
        {
            Add("Nintendo Entertainment System", "NES", "Nintendo", Region.NTSC_U, false);
            Add("Super Nintendo Entertainment System", "SNES", "Nintendo", Region.NTSC_U, false);
            Add("Nintendo 64", "N64", "Nintendo", Region.NTSC_U, false);
            Add("GameCube", "GameCube", "Nintendo", Region.NTSC_U, false);
            Add("Wii", "Wii", "Nintendo", Region.NTSC_U, false);
            Add("Wii U", "Wii U", "Nintendo", Region.NTSC_U, false);
            Add("Game & Watch", "Game & Watch", "Nintendo", Region.NTSC_U, false);
            Add("Game Boy", "Game Boy", "Nintendo", Region.NTSC_U, false);
            Add("Virtual Boy", "Virtual Boy", "Nintendo", Region.NTSC_U, false);
            Add("Game Boy Color", "GBC", "Nintendo", Region.NTSC_U, false);
            Add("Game Boy Advance", "GBA", "Nintendo", Region.NTSC_U, false);
            Add("Nintendo DS", "DS", "Nintendo", Region.NTSC_U, false);
            Add("Nintendo 3DS", "3DS", "Nintendo", Region.NTSC_U, false);
            Add("SG-1000", "SG-1000", "Sega", Region.NTSC_U, false);
            Add("Master System", "Master System", "Sega", Region.NTSC_U, false);
            Add("Genesis", "Genesis", "Sega", Region.NTSC_U, false);
            Add("Game Gear", "Game Gear", "Sega", Region.NTSC_U, false);
            Add("Sega CD", "Sega CD", "Sega", Region.NTSC_U, false);
            Add("Sega 32X", "32X", "Sega", Region.NTSC_U, false);
            Add("Sega Saturn", "Saturn", "Sega", Region.NTSC_U, false);
            Add("Dreamcast", "Dreamcast", "Sega", Region.NTSC_U, false);
            Add("PlayStation", "PS1", "Sony", Region.NTSC_U, false);
            Add("PlayStation 2", "PS2", "Sega", Region.NTSC_U, false);
            Add("PlayStation 3", "PS3", "Sega", Region.NTSC_U, false);

            DatabaseContext.SaveChanges();
        }

        public Platform Get(Guid id)
        {
            return DatabaseContext.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public Platform Get(string title)
        {
            return DatabaseContext.Platforms.FirstOrDefault(p => p.Title == title);
        }

        public void Add(string title, string shortTitle, string manufacturer, Region region, bool autoSave = true)
        {
            if (!DatabaseContext.Platforms.Any(p => p.Title == title))
            {
                DatabaseContext.Add(new Platform()
                {
                    Title = title,
                    ShortTitle = shortTitle,
                    Manufacturer = manufacturer,
                    Region = region
                });
            }

            if (autoSave) DatabaseContext.SaveChanges();
        }
    }
}
