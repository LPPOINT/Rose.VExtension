using System.Web.Mvc;
using Rose.VExtension.Server.Models;

namespace Rose.VExtension.Server.Controllers
{
    public class PluginsController : Controller
    {

        protected static readonly IPluginsRepository repository = new PluginsRepository();
        protected static readonly IPluginTransactor transactor = new PluginTransactor(repository);
        protected static readonly IPluginsCollection collection = new PluginsCollection();

        protected void AddAreaDataToken()
        {
            try
            {
               // RouteData.DataTokens.Add("area", "Plugins");
            }
            catch 
            {
                
            }
        }
    }
}