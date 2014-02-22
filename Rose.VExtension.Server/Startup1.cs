using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Rose.VExtension.Server.Startup1))]

namespace Rose.VExtension.Server
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
