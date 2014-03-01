using System.Web.Mvc;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Packing;
using Rose.VExtension.PluginSystem.Transactions;
using Rose.VExtension.Server.Models;
using LocalPluginFileSystem = Rose.VExtension.PluginSystem.FileSystem.LocalPluginFileSystem;
using LocalStoragePluginPackage = Rose.VExtension.PluginSystem.Packing.LocalStoragePluginPackage;

namespace Rose.VExtension.Server.Controllers
{
    public class PluginsDebugController : PluginsController
    {

        public ActionResult AddPlugin()
        {
            AddAreaDataToken();

            if (Request.Files.Count == 0)
                return RedirectToAction("Index");

            var stream = Request.Files[0].InputStream;

            var fileSystem =
                new LocalPluginFileSystem(
                    @"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\Content\PluginFolder");
            var package = new StreamPluginPackage(stream);

            var transaction = TransactionBuilder.FromPackageToRam(package, fileSystem);

            var p = transactor.ExecuteTransaction(transaction).Plugin;

            collection.Add(p);

            return RedirectToAction("Index");

        }

        public ActionResult RemovePlugin(string pluginId)
        {
            PluginConnection.DropConnection(pluginId, repository);
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            AddAreaDataToken();
            return View(repository.PluginContext.All);
        }

        public ActionResult Details(string pluginId)
        {
            var plugin = repository.PluginContext.GetEntity(pluginId);
            if (plugin == null)
                return HttpNotFound();

            var connection = PluginConnection.GetConnection(pluginId);
            if (connection == null)
            {
                transactor.Sync(new PluginsPhysicalStatusProvider(repository, collection));
                connection = PluginConnection.GetConnection(pluginId);
            }

            return View(new PluginViewModel(connection.ControlPlugin, connection.PluginEntity, connection));
        }

        public ActionResult AddStorageItem(string pluginId, string name, string value)
        {
            repository.StorageContext.AddEntity(name, value, pluginId);

            return RedirectToAction("Index");

        }

        public ActionResult DeleteStorageItem(string pluginId, string name)
        {
            repository.StorageContext.RemoveItemByName(pluginId, name);

            return RedirectToAction("Index");
        }

        public ActionResult Settings(string pluginId)
        {
            var connection = PluginConnection.GetConnection(pluginId);
            if (connection == null)
            {
                var fileSystem =
                     new LocalPluginFileSystem( // TODO: AD
                        @"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\Rose.VExtension.PluginSystem.ConsoleTest\PluginFolder");
                var package = new LocalStoragePluginPackage(@"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\Rose.VExtension.PluginSystem.ConsoleTest\plugin.zip", PacckageType.ZipFile);
                var transaction = TransactionBuilder.FromPackageToRam(package, fileSystem);
                transactor.ExecuteTransaction(transaction);
                connection = PluginConnection.GetConnection(pluginId);
            }

            var plugin = connection.ControlPlugin;

            return View(plugin);


        }


        [HttpPost]
        public ActionResult Sync()
        {
            var provider = new PluginsPhysicalStatusProvider(repository, collection);
            var result = transactor.Sync(provider);
            result.MakeLogReport();;
            return RedirectToAction("Index");
        }
    }
}