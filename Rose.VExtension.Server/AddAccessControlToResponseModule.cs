using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rose.VExtension.Server
{
    public class AddAccessControlToResponseModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
           context.BeginRequest += ContextOnBeginRequest;
        }

        private void ContextOnBeginRequest(object sender, EventArgs eventArgs)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            context.Response.Headers.Add("Access-Control-Allow-Origin", "chrome-extension://dbfhgfgiejjbeakgnbknjidhcnkokgof");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "content-type");
        }

        public void Dispose()
        {
            
        }
    }
}