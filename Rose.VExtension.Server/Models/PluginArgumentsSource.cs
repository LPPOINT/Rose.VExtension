using HtmlAgilityPack;
using Rose.VExtension.PluginSystem;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.PluginSystem.Runtime.RequestHandeling;

namespace Rose.VExtension.Server.Models
{
    public class PluginArgumentsSource : IRequestArgumentSource
    {
        public PluginArgumentsSource(Plugin plugin, PluginRequestModel requestModel, PluginRequest request)
        {
            Request = request;
            RequestModel = requestModel;
            Plugin = plugin;
            NameTable = new RequestArgumentNameTable();
        }

        public RequestArgumentNameTable NameTable { get; private set; }
        public Plugin Plugin { get; private set; }
        public PluginRequestModel RequestModel { get; private set; }
        public PluginRequest Request { get; private set; }

        public IRequestArgument GetRequestArgument(string definition)
        {

            if (NameTable[definition] == RequestArgumentNameTable.Values.Html)
            {
                var html = new HtmlDocument();
                html.LoadHtml(RequestModel.Html);
                return new RequestArgument(definition, html);
            }

            return new RequestArgument("hello", "world");
        }
    }
}