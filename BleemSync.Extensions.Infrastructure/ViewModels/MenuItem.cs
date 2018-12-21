using System;
using System.Collections.Generic;
using System.Text;

namespace BleemSync.Extensions.Infrastructure.ViewModels
{
    public class MenuItem
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Icon { get; set; }
        public List<MenuItem> Children { get; set; }

        public MenuItem()
        {
            Children = new List<MenuItem>();
        }
    }
}
