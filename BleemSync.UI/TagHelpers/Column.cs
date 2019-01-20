using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace BleemSync.UI
{
    [HtmlTargetElement("column")]
    public class ColumnTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
        }
    }

    [HtmlTargetElement(Attributes = "xs")]
    public class XsTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = output.Attributes.SingleOrDefault(a => a.Name == "xs");
            var hasClass = output.Attributes.SingleOrDefault(a => a.Name == "class") != null;

            if (attribute != null)
            {
                var width = attribute.Value.ToString();
                var classAttr = output.Attributes.SingleOrDefault(a => a.Name == "class");
                var cssClass = hasClass ? classAttr.Value : "";

                output.Attributes.RemoveAll("xs");
                output.Attributes.SetAttribute("class", $"{cssClass} col-xs-{width}");
            }
        }
    }

    [HtmlTargetElement(Attributes = "sm")]
    public class SmTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = output.Attributes.SingleOrDefault(a => a.Name == "sm");
            var hasClass = output.Attributes.SingleOrDefault(a => a.Name == "class") != null;

            if (attribute != null)
            {
                var width = attribute.Value.ToString();
                var classAttr = output.Attributes.SingleOrDefault(a => a.Name == "class");
                var cssClass = hasClass ? classAttr.Value : "";

                output.Attributes.RemoveAll("sm");
                output.Attributes.SetAttribute("class", $"{cssClass} col-sm-{width}");
            }
        }
    }

    [HtmlTargetElement(Attributes = "md")]
    public class MdTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = output.Attributes.SingleOrDefault(a => a.Name == "md");
            var hasClass = output.Attributes.SingleOrDefault(a => a.Name == "class") != null;

            if (attribute != null)
            {
                var width = attribute.Value.ToString();
                var classAttr = output.Attributes.SingleOrDefault(a => a.Name == "class");
                var cssClass = hasClass ? classAttr.Value : "";

                output.Attributes.RemoveAll("md");
                output.Attributes.SetAttribute("class", $"{cssClass} col-md-{width}");
            }
        }
    }

    [HtmlTargetElement(Attributes = "lg")]
    public class LgTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = output.Attributes.SingleOrDefault(a => a.Name == "lg");
            var hasClass = output.Attributes.SingleOrDefault(a => a.Name == "class") != null;

            if (attribute != null)
            {
                var width = attribute.Value.ToString();
                var classAttr = output.Attributes.SingleOrDefault(a => a.Name == "class");
                var cssClass = hasClass ? classAttr.Value : "";

                output.Attributes.RemoveAll("lg");
                output.Attributes.SetAttribute("class", $"{cssClass} col-lg-{width}");
            }
        }
    }
}
