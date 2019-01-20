using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace BleemSync.UI
{
    [HtmlTargetElement("hidden", TagStructure = TagStructure.WithoutEndTag)]
    public class HiddenHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public HiddenHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;

            var input = _generator.GenerateHidden(ViewContext, modelExplorer, For.Name, modelExplorer.Model, false, null);

            string outputString;

            using (var writer = new StringWriter())
            {
                input.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                outputString = writer.ToString();
            }

            output.Content.SetHtmlContent(outputString);
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "hidden");
        }
    }
}
