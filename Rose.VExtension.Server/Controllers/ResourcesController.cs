using System.Web.Mvc;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.Server.Models;

namespace Rose.VExtension.Server.Controllers
{
    public class ResourcesController : PluginsController
    {

        private IPluginFileSystem GetPluginFileSystem(string pluginId)
        {
            var middleware = new FileSystemMiddleware();
            var fileSystemEntity = repository.GetPluginFileSystemByPluginId(pluginId);

            if (fileSystemEntity != null)
            {
                var fileSystem = middleware.CreateBase(fileSystemEntity);
                return fileSystem;
            }

            return null;

        }

        public ActionResult Index(string id)
        {

            AddAreaDataToken();

            var accesToken = repository.GetResourceAccessToken(id);

            if(accesToken == null)
                return new HttpNotFoundResult();

            var plugin = accesToken.Plugin;

            if(plugin == null)
                return new HttpNotFoundResult();

            var fileSystem = GetPluginFileSystem(plugin.Id);

            if(fileSystem == null)
                return new HttpNotFoundResult();

            var stream = fileSystem.GetItemStream(FileSystemItem.GetResourceItem(accesToken.ResourceName));

            return new FileStreamResult(stream, "image/jpg");

        }
	}
}