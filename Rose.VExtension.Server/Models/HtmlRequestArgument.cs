using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Helpers;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling;

namespace Rose.VExtension.Server.Models
{
    public class HtmlRequestArgument : RequestArgument
    {
        public HtmlRequestArgument(string name, HtmlDocument html)
            : base(name, html)
        {
        }
    }

    public class HtmlWriterArgument : RequestArgument
    {
        public HtmlWriterArgument(string name, IHtmlWriter writer)
            : base(name, writer)
        {

        }
    }


}