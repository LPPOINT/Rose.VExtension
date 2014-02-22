using System.Web.Mvc;
using Rose.VExtension.Server.Models;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Transactions;

namespace Rose.VExtension.Server.Controllers
{
    public class PluginsController : Controller
    {

        protected static readonly IPluginsRepository repository = new PluginsRepository();
        protected static readonly IPluginTransactor transactor = new PluginTransactor(repository);
        protected static readonly IPluginsCollection collection = new PluginsCollection();

        public PluginsController()
        {
            if (transactor.Out == null)
            {
                transactor.Out = collection;
                transactor.ShouldInspectCollection = true;
            }
        }

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