using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Packing
{
    public interface IPluginUnpackingScheme
    {
        IDictionary<string, IPluginFileSystemItem> ItemsSourceScheme { get; }
        IPluginPackageFileSystem PluginPackageFileSystem { get; }
    }

    public class PluginUnpackingScheme : IPluginUnpackingScheme
    {

        private void Add(IPluginFileSystemItem item)
        {
            ItemsSourceScheme.Add(item.Uri, item);
        }

        public PluginUnpackingScheme(IPluginConfiguration pluginPluginConfiguration, IPluginPackageFileSystem pluginPackageFileSystem)
        {
            PluginPackageFileSystem = pluginPackageFileSystem;
            PluginPluginConfiguration = pluginPluginConfiguration;
            ItemsSourceScheme = new Dictionary<string, IPluginFileSystemItem>();
            
            Add(FileSystemItem.GetXMLManifestFile());

            var settingsNode = pluginPluginConfiguration.RootItem.Content.FirstOrDefault(pair => pair.Key == "Settings").Value;

            if (!string.IsNullOrWhiteSpace(settingsNode))
            {
                Add(FileSystemItem.GetSettingsFileItem(settingsNode));
            }

            foreach (var resName in pluginPackageFileSystem.GetFilesInFolder("Resources"))
            {
                var resOnlyName = Path.GetFileName(resName);
                Add(FileSystemItem.GetResourceItem(resOnlyName));
            }


            foreach (var resName in pluginPackageFileSystem.GetFilesInFolder("Scripts"))
            {
                var scriptOnlyName = Path.GetFileName(resName);
                Add(FileSystemItem.GetScriptItem(scriptOnlyName));
            }

            foreach (var resName in pluginPackageFileSystem.GetFilesInFolder("Pages"))
            {
                var scriptOnlyName = Path.GetFileName(resName);
                Add(FileSystemItem.GetRazorPageItem(scriptOnlyName));
            }


            foreach (var assemblyName in pluginPackageFileSystem.GetFilesInFolder("Assemblies"))
            {
                var assemblyOnlyName = Path.GetFileName(assemblyName);
                Add(FileSystemItem.GetAssemblyItem(assemblyOnlyName));
            }
        }

        public IPluginConfiguration PluginPluginConfiguration { get; private set; }

        public IDictionary<string, IPluginFileSystemItem> ItemsSourceScheme { get; private set; }
        public IPluginPackageFileSystem PluginPackageFileSystem { get; private set; }
    }

}
