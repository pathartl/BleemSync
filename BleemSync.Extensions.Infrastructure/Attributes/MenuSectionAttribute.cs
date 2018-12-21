using System;

namespace BleemSync.Extensions.Infrastructure.Attributes
{
    public class MenuSectionAttribute : Attribute
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Position { get; set; }
    }
}
