using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.Server.Middleware;
using Rose.VExtension.Server.Models;

namespace Rose.VExtension.Server.Controllers
{
    public class JavascriptEditorController : PluginsController
    {

        public ActionResult Save(string pluginId, string jsFileName, string script, bool? updatePackage)
        {
            var plugin = repository.PluginContext.GetEntity(pluginId);
            if (plugin == null)
                return HttpNotFound("Plugin not found");
            var middleare = new FileSystemMiddleware();
            var fileSystem = middleare.CreateBase(plugin.PluginFileSystem);
            if(fileSystem == null)
                return HttpNotFound("Cant create fs");
            var item = FileSystemItem.GetScriptItem(jsFileName);

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.AutoFlush = true;
                    streamWriter.Write(script);
                    stream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(stream);
                    var text = reader.ReadToEnd();
                    stream.Seek(0, SeekOrigin.Begin);
                    fileSystem.AddItem(item, stream);
                }
            }

            return RedirectToAction("Index", "PluginsDebug");

        }

        public ActionResult Index(string pluginId, string jsFileName)
        {

            var plugin = repository.PluginContext.GetEntity(pluginId);
            if (plugin == null)
                return HttpNotFound("Plugin not found");
            var middleare = new FileSystemMiddleware();
            var fileSystem = middleare.CreateBase(plugin.PluginFileSystem);

            using (var scriptStream = fileSystem.GetItemStream(FileSystemItem.GetScriptItem(jsFileName)))
            {
                using (var streamReader = new StreamReader(scriptStream))
                {
                    var s = streamReader.ReadToEnd();
                    return View(new PluginScriptEditingModel(pluginId, jsFileName, s));
                }

            }
        }

        public ActionResult Edit(string text)
        {
            return View("Index", ((object)text));
        }

	}
}