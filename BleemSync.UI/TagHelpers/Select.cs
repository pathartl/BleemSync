using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace BleemSync.UI
{
    [HtmlTargetElement("dropdown", TagStructure = TagStructure.WithoutEndTag)]
    public class DropdownHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        [HtmlAttributeName("disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("float")]
        public bool IsFloating { get; set; }

        [HtmlAttributeName("sm")]
        public bool IsSm { get; set; }

        [HtmlAttributeName("lg")]
        public bool IsLg { get; set; }

        [HtmlAttributeName("readonly")]
        public bool ReadOnly { get; set; }

        [HtmlAttributeName("required")]
        public bool Required { get; set; }

        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; }

        [HtmlAttributeName("has-warning")]
        public bool HasWarning { get; set; }

        [HtmlAttributeName("has-success")]
        public bool HasSuccess { get; set; }

        [HtmlAttributeName("has-error")]
        public bool HasError { get; set; }

        [HtmlAttributeName("icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("placeholder")]
        public string Placeholder { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public DropdownHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var placeholder = output.HasAttribute("placeholder") ? output.GetAttribute("placeholder") : "";

            output.AddClass("form-group pmd-textfield");

            var labelAttributes = new Dictionary<string, object>();
            var selectAttributes = new Dictionary<string, object>
            {
                { "class", "select-simple form-control pmd-select2" }
            };

            if (IsLg) output.AddClass("form-group-lg");
            if (IsSm) output.AddClass("form-group-sm");
            if (IsFloating) output.AddClass("pmd-textfield-floating-label");
            if (HasWarning) output.AddClass("has-warning");
            if (HasSuccess) output.AddClass("has-success");
            if (HasError) output.AddClass("has-error");

            if (Disabled) selectAttributes.Add("disabled", "disabled");
            if (ReadOnly) selectAttributes.Add("readonly", "readonly");
            if (Required) selectAttributes.Add("required", "required");

            var inputPre = "";
            var inputPost = "";
            var labelClass = "control-label";

            if (Icon != null)
            {
                inputPre = $"{inputPre}<div class=\"input-group\"><div class=\"input-group-addon\"><i class=\"material-icons md-dark pmd-sm\">{Icon}</i></div>";
                inputPost = $"{inputPost}</div>";
                labelClass = $"{labelClass} pmd-input-group-label";
            }

            labelAttributes.Add("class", labelClass);

            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;

            var modelType = For.ModelExplorer.ModelType;

            if (Nullable.GetUnderlyingType(For.ModelExplorer.ModelType) != null)
            {
                modelType = Nullable.GetUnderlyingType(For.ModelExplorer.ModelType);
            }

            if (modelType.IsEnum)
            {
                var selectListItems = new List<SelectListItem>();
                var enumNames = Enum.GetNames(modelType);
                var enumValues = Enum.GetValues(modelType);
                var attributes = modelType.GetCustomAttribute<DisplayAttribute>();

                int i = 0;
                foreach (var name in enumNames)
                {
                    var display = modelType.GetMember(name).First().GetCustomAttribute<DisplayAttribute>();
                    selectListItems.Add(new SelectListItem()
                    {
                        Text = display == null ? name : display.Name,
                        Value = enumValues.GetValue(i).ToString()
                    });

                    i++;
                }

                Items = selectListItems;
            }

            var select = _generator.GenerateSelect(ViewContext, modelExplorer, placeholder, For.Name, Items, Multiple, selectAttributes);
            var label = _generator.GenerateLabel(ViewContext, modelExplorer, For.Name, metadata.DisplayName, labelAttributes);
            var hidden = _generator.GenerateHidden(ViewContext, modelExplorer, For.Name, null, true, null);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            string selectOutput;

            using (var writer = new StringWriter())
            {
                label.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPre);
                select.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                writer.Write(inputPost);
                hidden.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                selectOutput = writer.ToString();
            }

            output.Content.SetHtmlContent(selectOutput);
        }
    }
}
