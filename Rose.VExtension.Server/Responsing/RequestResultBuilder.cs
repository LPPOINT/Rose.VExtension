using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Rose.VExtension.PluginSystem.Runtime;
using Rose.VExtension.Server.Models;

namespace Rose.VExtension.Server.Responsing
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

        private ContentResult XmlResult(string xml)
        {
            return new ContentResult { Content = xml, ContentType = "text/xml" };
        }

        public ActionResult GetResult()
        {
            var service = new XmlResponseMessager();

            try
            {

                if (responses.Count == 0)
                    return XmlResult(service.CreateEmpty());

                var xml = service.Create(responses.First());
                return XmlResult(xml);
            }
            catch (Exception e)
            {
                return XmlResult(service.CreateException(e));
            }

        }

        public void AddMissedResponse(Exception missException)
        {
            
        }
    }
}