using System;

namespace BleemSync.Extensions.Infrastructure.Attributes
{
    public class MenuItemAttribute : Attribute
    {
        public string Name { get; set; }
        public int Position { get; set; }
    }
}
