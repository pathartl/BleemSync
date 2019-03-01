using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Collections;

namespace BleemSync.UI
{
    [HtmlTargetElement("tags", TagStructure = TagStructure.WithoutEndTag)]
    public class TagsHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public TagsHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isLg = output.Attributes.SingleOrDefault(a => a.Name == "lg") != null;
            var isSm = output.Attributes.SingleOrDefault(a => a.Name == "sm") != null;
            var isFloating = output.Attributes.SingleOrDefault(a => a.Name == "float") != null;
            var isDisabled = output.Attributes.SingleOrDefault(a => a.Name == "disabled") != null;
            var isReadOnly = output.Attributes.SingleOrDefault(a => a.Name == "readonly") != null;
            var isRequired = output.Attributes.SingleOrDefault(a => a.Name == "required") != null;
            var hasWarning = output.Attributes.SingleOrDefault(a => a.Name == "has-warning") != null;
            var hasSuccess = output.Attributes.SingleOrDefault(a => a.Name == "has-success") != null;
            var hasError = output.Attributes.SingleOrDefault(a => a.Name == "has-error") != null;

            var icon = output.Attributes.SingleOrDefault(a => a.Name == "icon");
            var placeholder = output.Attributes.SingleOrDefault(a => a.Name == "placeholder");

            var binding = output.Attributes.SingleOrDefault(a => a.Name == "for");

            output.Attributes.RemoveAll("lg");
            output.Attributes.RemoveAll("sm");
            output.Attributes.RemoveAll("float");
            output.Attributes.RemoveAll("disabled");
            output.Attributes.RemoveAll("readonly");
            output.Attributes.RemoveAll("required");
            output.Attributes.RemoveAll("has-warning");
            output.Attributes.RemoveAll("has-success");
            output.Attributes.RemoveAll("has-error");
            output.Attributes.RemoveAll("icon");
            output.Attributes.RemoveAll("placeholder");
            output.Attributes.RemoveAll("for");

            var formGroupClass = "form-group pmd-textfield";

            var labelAttributes = new Dictionary<string, object>();
            var selectAttributes = new Dictionary<string, object>
            {
                { "class", "form-control pmd-select2-tags tags" }
            };

            if (isLg) formGroupClass = $"{formGroupClass} form-group-lg";
            if (isSm) formGroupClass = $"{formGroupClass} form-group-sm";
            if (isFloating) formGroupClass = $"{formGroupClass} pmd-textfield-floating-label";
            if (hasWarning) formGroupClass = $"{formGroupClass} has-warning";
            if (hasSuccess) formGroupClass = $"{formGroupClass} has-sucess";
            if (hasError) formGroupClass = $"{formGroupClass} has-error";

            if (isDisabled) selectAttributes.Add("disabled", "disabled");
            if (isReadOnly) selectAttributes.Add("readonly", "readonly");
            if (isRequired) selectAttributes.Add("required", "required");

            var inputPre = "";
            var inputPost = "";
            var labelClass = "control-label";
            string placeholderString = placeholder != null ? placeholder.Value.ToString() : "";

            if (icon != null)
            {
                inputPre = $"{inputPre}<div class=\"input-group\"><div class=\"input-group-addon\"><i class=\"material-icons md-dark pmd-sm\">{icon.Value}</i></div>";
                inputPost = $"{inputPost}</div>";
                labelClass = $"{labelClass} pmd-input-group-label";
            }

            labelAttributes.Add("class", labelClass);

            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;

            var items = new List<SelectListItem>();

            if (modelExplorer.Model != null && modelExplorer.ModelType is IEnumerable && ((IList)modelExplorer.Model).Count > 0)
            {
                foreach (var item in (string[])modelExplorer.Model)
                {
                    items.Add(new SelectListItem() { Text = item, Value = item });
                }
            }

            var select = _generator.GenerateSelect(ViewContext, modelExplorer, placeholderString, For.Name, items, true, selectAttributes);
            var label = _generator.GenerateLabel(ViewContext, modelExplorer, For.Name, metadata.DisplayName, labelAttributes);
            var hidden = _generator.GenerateHidden(ViewContext, modelExplorer, For.Name, null, true, null);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", formGroupClass);

            string selectOutput;

            using (var writer = new StringWriter())
            {
                label.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPre);
                select.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPost);
                //hidden.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                selectOutput = writer.ToString();
            }

            output.Content.SetHtmlContent(selectOutput);
        }
    }
}
