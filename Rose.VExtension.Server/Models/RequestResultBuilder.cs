using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.Server.Models
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
            var result =  new JsonResult
                   {
                       Data = responses
                   };

#if DEBUG
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

#endif
            return result;

        }

        public void AddMissedResponse(Exception missException)
        {
            
        }
    }
}