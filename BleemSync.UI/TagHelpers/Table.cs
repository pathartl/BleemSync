using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace BleemSync.UI
{
    [HtmlTargetElement("table", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TableHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isBordered = output.Attributes.SingleOrDefault(a => a.Name == "border") != null;
            var isHover = output.Attributes.SingleOrDefault(a => a.Name == "hover") != null;
            var isStriped = output.Attributes.SingleOrDefault(a => a.Name == "striped") != null;
            var isSm = output.Attributes.SingleOrDefault(a => a.Name == "sm") != null;

            output.Attributes.RemoveAll("border");
            output.Attributes.RemoveAll("hover");
            output.Attributes.RemoveAll("striped");
            output.Attributes.RemoveAll("sm");

            var classAttribute = output.Attributes.SingleOrDefault(a => a.Name == "class");

            var tableClass = "pmd-table table";

            if (classAttribute != null) tableClass = $"{classAttribute.Value.ToString()} {tableClass}";

            if (isBordered) tableClass = $"{tableClass} table-bordered";
            if (isHover) tableClass = $"{tableClass} table-hover";
            if (isStriped) tableClass = $"{tableClass} table-striped";
            if (isSm) tableClass = $"{tableClass} table-sm";

            output.Attributes.SetAttribute("class", tableClass);
        }
    }
}
