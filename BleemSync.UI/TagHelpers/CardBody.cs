using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BleemSync.UI
{
    [HtmlTargetElement("card-body")]
    public class CardBodyTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class", "pmd-card-body");
            output.TagName = "div";
        }
    }
}
