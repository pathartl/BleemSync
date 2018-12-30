using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.UI
{
    public class TagHelperAttribute : Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute
    {
        public TagHelperAttribute(string name) : base(name) { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
