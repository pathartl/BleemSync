using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BleemSync.UI
{
    [HtmlTargetElement("card-actions")]
    public class CardActionsTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class", "pmd-card-actions");
            output.TagName = "div";
        }
    }
}
