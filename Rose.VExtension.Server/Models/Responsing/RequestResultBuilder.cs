using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.Server.Models.Responsing
{
    public class RequestResultBuilder : IRequestResutBuilder
    {
        public RequestResultBuilder(PluginRequestModel pluginRequest)
        {
            PluginRequest = pluginRequest;
            responses = new List<PluginResponse>();
        }

        public PluginRequestModel PluginRequest { get; private set; }

        private readonly List<PluginResponse> responses; 

        public void AddPluginResponse(PluginResponse response)
        {
            responses.Add(response);
        }

        public ActionResult GetResult()
        {
            var service = new PluginResponseXml();
            var xml = service.Create(responses.First());
            return new ContentResult(){Content = xml.ToString()};
        }

        public void AddMissedResponse(Exception missException)
        {
            
        }
    }
}