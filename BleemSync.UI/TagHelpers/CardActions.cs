using Microsoft.AspNetCore.Razor.TagHelpers;

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
