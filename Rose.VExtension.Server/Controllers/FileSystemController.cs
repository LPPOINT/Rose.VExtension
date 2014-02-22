using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.Server.Models;
using Rose.VExtension.Server.Models.Transactions;

namespace Rose.VExtension.Server.Controllers
{
    public class FileSystemController : PluginsController
    {

        public ActionResult Index(string pluginId)
        {

            if (string.IsNullOrWhiteSpace(pluginId))
                return HttpNotFound();

            var plugin = transactor.GetPlugin(pluginId);

            return View(plugin);
        }

        public ActionResult OpenFile(string filePath, string pluginId)
        {
            var plugin = transactor.GetPlugin(pluginId);
            var name = Path.GetFileName(filePath);
            var item = new FileSystemItem(name, filePath);
            var fileSystem = plugin.FileSystem;

            if (!fileSystem.ContainsItem(item))
                return HttpNotFound("File " + filePath + " not found");

            var fileStream = fileSystem.GetItemStream(item);

            if (fileStream == null)
                return HttpNotFound("Cant provide access to file  " + filePath);

            var itemExtension = Path.GetExtension(filePath);

            if (itemExtension == ".txt" || itemExtension == ".cs")
            {
                return new FileStreamResult(fileStream, "text/plain");
            }
            if (itemExtension == ".jpg")
            {
                return new FileStreamResult(fileStream, "image/jpg");
            }
            if (itemExtension == ".xml")
            {
                return new FileStreamResult(fileStream, "text/xml");
            }

            return HttpNotFound("Undefined file format '" + itemExtension + "'");
        }
    }
}