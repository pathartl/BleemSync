using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace BleemSync.UI
{
    [HtmlTargetElement("card")]
    public class CardTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var classAttr = output.Attributes.SingleOrDefault(a => a.Name == "class");
            var titleAttr = output.Attributes.SingleOrDefault(a => a.Name == "title");
            var secondaryTitleAttr = output.Attributes.SingleOrDefault(a => a.Name == "secondary-title");
            var isEditableTitle = output.Attributes.SingleOrDefault(a => a.Name == "editable-title") != null;

            var classString = classAttr != null ? classAttr.Value : "";
            var titleString = titleAttr != null ? titleAttr.Value : "";
            var secondaryTitleString = secondaryTitleAttr != null ? secondaryTitleAttr.Value : "";

            output.Attributes.RemoveAll("class");
            output.Attributes.RemoveAll("title");
            output.Attributes.RemoveAll("secondary-title");
            output.Attributes.RemoveAll("editable-title");

            output.TagName = "div";
            output.Attributes.SetAttribute("class", $"{classString} pmd-card pmd-card-default pmd-z-depth");

            var pre = "";
            var post = "";

            if (titleString.ToString() != "")
            {
                secondaryTitleString = secondaryTitleAttr != null ? $"<span class=\"pmd-card-subtitle-text\">{secondaryTitleString}</span>" : "";
                pre = $"<div class=\"pmd-card-title\"><h2 class=\"pmd-card-title-text\">{titleString}</h2>{secondaryTitleString}</div>{pre}";
            }

            if (isEditableTitle)
            {

            }

            output.PreContent.SetHtmlContent(pre);
            output.PostContent.SetHtmlContent(post);
        }
    }
}
