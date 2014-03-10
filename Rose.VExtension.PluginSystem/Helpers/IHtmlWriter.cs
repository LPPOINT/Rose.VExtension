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

    public interface IHtmlWriter
    {
        IHtmlTemplateBuilder TemplateBuilder { get; }
        HtmlDocument Html { get; }



    }

    public class HtmlWriter : IHtmlWriter
    {
        public HtmlWriter(HtmlDocument html)
        {
            Html = html;
            TemplateBuilder = new HtmlTemplateBuilder();
        }

        public IHtmlTemplateBuilder TemplateBuilder { get; private set; }
        public HtmlDocument Html { get; private set; }
    }
}
