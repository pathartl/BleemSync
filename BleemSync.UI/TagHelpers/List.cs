using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BleemSync.UI
{
    [HtmlTargetElement("list", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ListHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("class", "list-group pmd-list pmd-card-list");
            output.TagName = "ul";
        }
    }

    [HtmlTargetElement("list-item", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ListItemHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;
        private IDictionary<string, string> _routeValues;
        private IUrlHelperFactory _urlHelperFactory;
        private IUrlHelper _urlHelper;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                    _routeValues = (IDictionary<string, string>)new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ListItemHelper(IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory)
        {
            _generator = generator;
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            var icon = output.Attributes.SingleOrDefault(a => a.Name == "icon");
            var text = output.Attributes.SingleOrDefault(a => a.Name == "text");

            output.Attributes.RemoveAll("icon");
            output.Attributes.RemoveAll("text");

            output.Content.SetContent(text.Value.ToString());
            output.Attributes.SetAttribute("class", "list-group-item");
            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;

            if (Action != null || Controller != null)
            {
                var url = _urlHelper.Action(Action, Controller, _routeValues);

                output.Attributes.SetAttribute("href", url);
                output.TagName = "a";
            }

            if (icon != null)
            {
                output.PreContent.SetHtmlContent($"<i class=\"material-icons media-left pmd-sm\">{icon.Value.ToString()}</i><span class=\"media-body\">");
                output.PostContent.SetHtmlContent("</span>");
            }
        }
    }
}
