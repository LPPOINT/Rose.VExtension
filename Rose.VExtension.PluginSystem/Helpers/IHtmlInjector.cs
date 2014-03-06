using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Helpers
{

    public class HtmlInjectionException : Exception
    {
        public HtmlInjectionException()
        {
        }

        public HtmlInjectionException(string message) : base(message)
        {
        }

        public HtmlInjectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HtmlInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public interface IHtmlInjector
    {

        IHtmlTemplateBuilder TemplateBuilder { get; set; }

        void Inject(HtmlDocument html);
    }

    public class HtmlScriptInjector : IHtmlInjector
    {

        public HtmlScriptInjector()
        {
            Scripts = new List<string>();
            TemplateBuilder = new HtmlTemplateBuilder();
        }

        public IList<string> Scripts { get; private set; }

        public void Inject(string script)
        {
            Scripts.Add(script);
        }

        public void Inject(HtmlNode node, string script)
        {

            Check.NotNull(node);
            Check.NotNullOrWhiteSpace(script);

            var scriptNode = TemplateBuilder.GetScriptTemplate(script);
            node.AppendChild(scriptNode);
        }

        public IHtmlTemplateBuilder TemplateBuilder { get; set; }

        public void Inject(HtmlDocument html)
        {

            if(TemplateBuilder == null)
                TemplateBuilder = new HtmlTemplateBuilder();

            var head = html.DocumentNode.ChildNodes.FirstOrDefault(node => node.Name == "head");
            foreach (var script in Scripts)
            {
                Inject(head, script);
            }
        }
    }

}
