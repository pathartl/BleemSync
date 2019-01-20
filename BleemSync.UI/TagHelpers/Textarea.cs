using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace BleemSync.UI
{
    [HtmlTargetElement("textarea", TagStructure = TagStructure.WithoutEndTag)]
    public class TextAreaHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public TextAreaHelper(IHtmlGenerator generator)
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
            var hasRows = output.Attributes.SingleOrDefault(a => a.Name == "rows") != null;
            var hasColumns = output.Attributes.SingleOrDefault(a => a.Name == "columns") != null;
            var hasWarning = output.Attributes.SingleOrDefault(a => a.Name == "has-warning") != null;
            var hasSuccess = output.Attributes.SingleOrDefault(a => a.Name == "has-success") != null;
            var hasError = output.Attributes.SingleOrDefault(a => a.Name == "has-error") != null;

            var icon = output.Attributes.SingleOrDefault(a => a.Name == "icon");
            var binding = output.Attributes.SingleOrDefault(a => a.Name == "for");

            var rowsAttr = output.Attributes.SingleOrDefault(a => a.Name == "rows");
            var columnsAttr = output.Attributes.SingleOrDefault(a => a.Name == "columns");

            output.Attributes.RemoveAll("lg");
            output.Attributes.RemoveAll("sm");
            output.Attributes.RemoveAll("float");
            output.Attributes.RemoveAll("disabled");
            output.Attributes.RemoveAll("readonly");
            output.Attributes.RemoveAll("required");
            output.Attributes.RemoveAll("rows");
            output.Attributes.RemoveAll("columns");
            output.Attributes.RemoveAll("has-warning");
            output.Attributes.RemoveAll("has-success");
            output.Attributes.RemoveAll("has-error");
            output.Attributes.RemoveAll("icon");
            output.Attributes.RemoveAll("for");

            var formGroupClass = "form-group pmd-textfield";

            var labelAttributes = new Dictionary<string, object>();
            var textareaAttributes = new Dictionary<string, object>
            {
                { "type", "text" },
                { "class", "form-control" }
            };

            if (isLg) formGroupClass = $"{formGroupClass} form-group-lg";
            if (isSm) formGroupClass = $"{formGroupClass} form-group-sm";
            if (isFloating) formGroupClass = $"{formGroupClass} pmd-textfield-floating-label";
            if (hasWarning) formGroupClass = $"{formGroupClass} has-warning";
            if (hasSuccess) formGroupClass = $"{formGroupClass} has-sucess";
            if (hasError) formGroupClass = $"{formGroupClass} has-error";

            if (isDisabled) textareaAttributes.Add("disabled", "disabled");
            if (isReadOnly) textareaAttributes.Add("readonly", "readonly");
            if (isRequired) textareaAttributes.Add("required", "required");

            var rows = 4;
            var columns = 50;

            if (hasRows) rows = Convert.ToInt32(rowsAttr.Value);
            if (hasColumns) rows = Convert.ToInt32(columnsAttr.Value);

            var inputPre = "";
            var inputPost = "";
            var labelClass = "control-label";

            if (icon != null)
            {
                inputPre = $"{inputPre}<div class=\"input-group\"><div class=\"input-group-addon\"><i class=\"material-icons md-dark pmd-sm\">{icon.Value}</i></div>";
                inputPost = $"{inputPost}</div>";
                labelClass = $"{labelClass} pmd-input-group-label";
            }

            labelAttributes.Add("class", labelClass);

            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;

            var input = _generator.GenerateTextArea(ViewContext, modelExplorer, For.Name, rows, columns, textareaAttributes);
            var label = _generator.GenerateLabel(ViewContext, modelExplorer, For.Name, metadata.DisplayName, labelAttributes);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", formGroupClass);

            string textboxOutput;

            using (var writer = new StringWriter())
            {
                label.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPre);
                input.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPost);
                textboxOutput = writer.ToString();
            }

            output.Content.SetHtmlContent(textboxOutput);
        }
    }
}
