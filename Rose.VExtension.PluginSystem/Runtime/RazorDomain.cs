using System;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Schema;
using NLog;
using RazorEngine;
using RazorEngine.Compilation;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using Rose.VExtension.PluginSystem.Activation.Platforms;

namespace Rose.VExtension.PluginSystem.Runtime
{


    public class RazorDomainModel
    {
        public RazorDomainModel(Plugin plugin, PluginRequest request, PluginResponse response, Logger log)
        {
            Log = log;
            Response = response;
            Request = request;
            Plugin = plugin;
        }

        public Plugin Plugin { get; private set; }
        public PluginRequest Request { get; private set; }
        public PluginResponse Response { get; private set; }
        public Logger Log { get; private set; }
    }

    public class RazorDomain : PluginDomain
    {
        private string GetPageForRequest(PluginRequest request)
        {
            foreach (var page in (Plugin.Platform as RazorPluginPlatform).Pages)
            {
                if (page.Key.IsValidRequest(request))
                    return page.Value;
            }
            return string.Empty;
        }

        public override PluginResponse Execute(PluginRequest request)
        {
            try
            {
                var page = GetPageForRequest(request);

                if(string.IsNullOrWhiteSpace(page))
                    return new PluginResponse();

                var response = new PluginResponse();

                var model = new RazorDomainModel(Plugin, request, response, LogManager.GetCurrentClassLogger());
                var responseHtml = Razor.Parse(page, model);

                LogManager.GetCurrentClassLogger().Info(responseHtml);

                return response;
            }
            catch (Exception e) 
            {
                LogManager.GetCurrentClassLogger().WarnException("Ошибка во время выполнения плагина", e);
                return new PluginResponse();
            }


        }
    }
}
