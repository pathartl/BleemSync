using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using BleemSync.UI.Extensions;

namespace BleemSync.UI
{
    [HtmlTargetElement("button")]
    public class ButtonHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpRequest _request;
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

        [HtmlAttributeName("raised")]
        public bool Raised { get; set; }

        [HtmlAttributeName("flat")]
        public bool Flat { get; set; }

        [HtmlAttributeName("outline")]
        public bool Outline { get; set; }

        [HtmlAttributeName("float")]
        public bool Floating { get; set; }

        [HtmlAttributeName("sm")]
        public bool IsSm { get; set; }

        [HtmlAttributeName("lg")]
        public bool IsLg { get; set; }

        [HtmlAttributeName("block")]
        public bool Block { get; set; }

        [HtmlAttributeName("default")]
        public bool Default { get; set; }

        [HtmlAttributeName("primary")]
        public bool Primary { get; set; }

        [HtmlAttributeName("success")]
        public bool Success { get; set; }

        [HtmlAttributeName("info")]
        public bool Info { get; set; }

        [HtmlAttributeName("warning")]
        public bool Warning { get; set; }

        [HtmlAttributeName("danger")]
        public bool Danger { get; set; }

        [HtmlAttributeName("link")]
        public bool Link { get; set; }

        [HtmlAttributeName("icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("text")]
        public string Text { get; set; }

        [HtmlAttributeName("type")]
        public string Type { get; set; }

        [HtmlAttributeName("onclick")]
        public string OnClick { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ButtonHelper(IHtmlGenerator generator, IUrlHelperFactory urlHelperFactory)
        {
            _generator = generator;
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            output.AddClass("btn");

            if (Raised) output.AddClass("pmd-btn-raised");
            if (Flat) output.AddClass("pmd-btn-flat");
            if (Outline) output.AddClass("pmd-btn-outline");
            if (Floating) output.AddClass("pmd-btn-fab");
            if (IsLg) output.AddClass("btn-lg");
            if (IsSm) output.AddClass("btn-sm");
            if (Block) output.AddClass("btn-block");
            if (Default) output.AddClass("btn-default");
            if (Primary) output.AddClass("btn-primary");
            if (Success) output.AddClass("btn-success");
            if (Info) output.AddClass("btn-info");
            if (Warning) output.AddClass("btn-warning");
            if (Danger) output.AddClass("btn-danger");
            if (Link) output.AddClass("btn-link");

            if (!Default && !Primary && !Success && !Info && !Warning && !Danger && !Link)
            {
                output.AddClass("btn-primary");
            }

            if (Icon != null)
            {
                output.AddClass("btn-icon");
                output.PreContent.AppendHtml($"<i class=\"material-icons pmd-sm\">{Icon}</i>");
            }

            if (Text == null)
            {
                output.AddClass("no-text");
            }

            if (Action != null || Controller != null)
            {
                if (Type == null || (Type != null && Type == "submit"))
                {
                    var url = _urlHelper.Action(Action, Controller, _routeValues);

                    output.Attributes.SetAttribute("href", url);
                    output.TagName = "a";
                }
            }
            else if (Type != null && Type == "link")
            {
                output.TagName = "a";
            }
            else if (OnClick != null)
            {
                output.SetAttribute("onclick", OnClick);
                output.TagName = "button";
            }
            else
            {
                Type = "submit";
                output.Attributes.SetAttribute("type", "submit");
                output.TagName = "button";
            }

            output.RemoveAttributes(
                new string[] {
                    "raised",
                    "flat",
                    "outline",
                    "floating",
                    "lg",
                    "sm",
                    "block",
                    "default",
                    "primary",
                    "success",
                    "info",
                    "warning",
                    "danger",
                    "link",
                    "icon",
                    "text"
                }
            );

            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(Text);
        }
    }
}
