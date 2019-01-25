using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace BleemSync.UI
{
    [HtmlTargetElement("checkbox", TagStructure = TagStructure.WithoutEndTag)]
    public class CheckBoxHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public CheckBoxHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var text = output.Attributes.SingleOrDefault(a => a.Name == "text");
            var isDisabled = output.Attributes.SingleOrDefault(a => a.Name == "disabled") != null;
            var isReadOnly = output.Attributes.SingleOrDefault(a => a.Name == "readonly") != null;
            var isRequired = output.Attributes.SingleOrDefault(a => a.Name == "required") != null;

            output.Attributes.RemoveAll("disabled");
            output.Attributes.RemoveAll("readonly");
            output.Attributes.RemoveAll("required");

            var labelAttributes = new Dictionary<string, object>
            {
                { "class", "pmd-checkbox checkbox-pmd-ripple-effect" }
            };

            var checkboxAttributes = new Dictionary<string, object>
            {
                { "type", "checkbox" }
            };

            if (isDisabled) checkboxAttributes.Add("disabled", "disabled");
            if (isReadOnly) checkboxAttributes.Add("readonly", "readonly");
            if (isRequired) checkboxAttributes.Add("required", "required");

            var inputPre = "";
            var inputPost = "";

            var value = For.ModelExplorer.Model == null ? false : (bool)For.ModelExplorer.Model;

            var labelText = text == null ? For.Metadata.DisplayName : text.Value.ToString();

            var input = _generator.GenerateCheckBox(ViewContext, For.ModelExplorer, For.Name, value, checkboxAttributes);
            var label = _generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, "", labelAttributes);
            var hidden = _generator.GenerateHiddenForCheckbox(ViewContext, For.ModelExplorer, For.Name);

            // Strip end tag from label
            var labelStart = "";
            using (var writer = new StringWriter())
            {
                label.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                labelStart = writer.ToString();
                labelStart = labelStart.Replace("</label>", "");
            }

            inputPre = $"{labelStart}{inputPre}";
            inputPost = $"{inputPost}<span class=\"pmd-checkbox-label\">&nbsp;</span><span class=\"pmd-checkbox\">{labelText}</span></label>";

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "checkbox pmd-default-theme");

            string textboxOutput;

            using (var writer = new StringWriter())
            {
                writer.Write(inputPre);
                input.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                hidden.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPost);
                textboxOutput = writer.ToString();
            }

            output.Content.SetHtmlContent(textboxOutput);
        }
    }
}
