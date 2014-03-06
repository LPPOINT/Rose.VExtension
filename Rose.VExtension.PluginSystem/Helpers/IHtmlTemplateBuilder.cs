using HtmlAgilityPack;

namespace Rose.VExtension.PluginSystem.Helpers
{
    public interface IHtmlTemplateBuilder
    {
        HtmlNode GetScriptTemplate();
        HtmlNode GetScriptTemplate(string script);

        HtmlNode GetReferenceTemplate(string href);
        HtmlNode GetReferenceTemplate(string href, string content);
        HtmlNode GetReferenceTemplate();

    }

    public class HtmlTemplateBuilder : IHtmlTemplateBuilder
    {

        private static HtmlNode CreateEmptyNode()
        {
            var node = HtmlNode.CreateNode(string.Empty);
            return node;
        }

        private static HtmlNode CreateEmptyNode(string name)
        {
            return HtmlNode.CreateNode(string.Format("<{0}></{0}>", name));
        }

        public HtmlNode GetScriptTemplate()
        {
            return GetScriptTemplate(string.Empty);
        }

        public HtmlNode GetScriptTemplate(string script)
        {
            var node = CreateEmptyNode("script");
            node.InnerHtml = script;
            node.SetAttributeValue("type", "javascript");
            return node;
        }

        public HtmlNode GetReferenceTemplate(string href)
        {
            return GetReferenceTemplate(href, href);
        }

        public HtmlNode GetReferenceTemplate(string href, string content)
        {
            var node = CreateEmptyNode("a");
            node.InnerHtml = content;
            node.SetAttributeValue("href", href);
            return node;
        }

        public HtmlNode GetReferenceTemplate()
        {
            return GetReferenceTemplate(string.Empty, string.Empty);
        }
    }
}
