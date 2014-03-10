using System;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.Server.Middleware;
using Rose.VExtension.Server.Models;

namespace Rose.VExtension.Server.Controllers
{
    public class ResourcesController : PluginsController
    {

        private IPluginFileSystem GetPluginFileSystem(string pluginId)
        {
            var middleware = new FileSystemMiddleware();
            var fileSystemEntity = repository.FileSystemContext.GetEntitiesByPluginId(pluginId).FirstOrDefault();

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

            var accesToken = repository.ResourceAccessTokenContext.GetEntity(id);

            if(accesToken == null)
                return new HttpNotFoundResult();

            var plugin = accesToken.Plugin;

            if(plugin == null)
                return new HttpNotFoundResult();

            var fileSystem = GetPluginFileSystem(plugin.Id);

            if(fileSystem == null)
                return new HttpNotFoundResult();

            try
            {
                var stream = fileSystem.GetItemStream(FileSystemItem.GetResourceItem(accesToken.ResourcePath));

                return new FileStreamResult(stream, "image/jpg");
            }
            catch (ResourceAccessDeniedException e)
            {
                return HttpNotFound(String.Format("Access to resource '{0}' is denied", accesToken.ResourcePath));
            }

        }
	}
}