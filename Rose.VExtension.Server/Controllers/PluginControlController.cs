using System.Web.Mvc;
using Rose.VExtension.Server.DbInteraction;
using Rose.VExtension.Server.Models;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Transactions;

namespace Rose.VExtension.Server.Controllers
{
    public class PluginControlController : Controller
    {

        private static IPluginsRepository repository = new PluginsRepository();
        private static IPluginTransactor transactor = new PluginTransactor(repository);
        private static IPluginsCollection collection = new PluginsCollection();

        //
        // GET: /PluginControl/
        public ActionResult Index(string id)
        {

            if (transactor.ShouldInspectCollection == false || transactor.Out == null)
            {
                transactor.Out = collection;
                transactor.ShouldInspectCollection = true;
            }

            var pluginEntity = repository.PluginContext.GetEntity(id);

            if (pluginEntity == null)
                return HttpNotFound();

            var connection = PluginConnection.GetConnection(id);
            var plugin = transactor.GetPlugin(id);

            var model = new PluginViewModel(plugin, pluginEntity, connection);

            return View(model);
        }
	}
}