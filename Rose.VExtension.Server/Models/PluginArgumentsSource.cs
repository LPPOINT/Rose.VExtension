using HtmlAgilityPack;
using Rose.VExtension.PluginSystem;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.PluginSystem.Runtime.Popups;
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
            switch (NameTable[definition])
            {
                case RequestArgumentNameTable.Values.Html:
                {
                    var html = new HtmlDocument();
                    html.LoadHtml(RequestModel.Html);
                    return new RequestArgument(definition, html);
                }
                case RequestArgumentNameTable.Values.NotificationPopup:
                    return new RequestArgument(definition, new NotificationPopup());
                case RequestArgumentNameTable.Values.WindowPopup:
                    return new RequestArgument(definition, new WindowPopup());
            }
            return new RequestArgument("hello", "world");
        }

        public object GetRequestArgumentValue(string definition)
        {
            return GetRequestArgument(definition).ArgumentValue;
        }
    }
}