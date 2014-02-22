using System;
using System.Xml.Linq;
using NLog;
using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;

namespace Rose.VExtension.PluginSystem.Activation
{

    public enum PacckageType
    {
        ZipFile
    }

    public class PluginFactory : IPluginFactory
    {
        public PluginFactory(IPluginInitializer initializer)
        {

            Check.NotNull(initializer);

            Initializer = initializer;
        }

        public IPluginInitializer Initializer { get; private set; }


        public Plugin ToRunnable(IPluginFileSystem fileSystem, IPluginPackage package = null)
        {

            Check.NotNull(fileSystem);

            using (var manifestFileStream = fileSystem.GetItemStream(FileSystemItem.GetXMLManifestFile()))
            {
                var manifest = new XMLManifest(XDocument.Load(manifestFileStream));
                var config = manifest.CreateConfiguration();
                manifestFileStream.Dispose();
                return ToRunnable(fileSystem, config, package);
            }
        }

        public Plugin ToRunnable(IPluginFileSystem fileSystem, IPluginConfiguration pluginPluginConfiguration, IPluginPackage package = null)
        {

            Check.NotNull(fileSystem);
            Check.NotNull(pluginPluginConfiguration);

            var plugin = new Plugin();

            plugin.PluginConfiguration = pluginPluginConfiguration;
            plugin.FileSystem = fileSystem;
            plugin.Package = package;

            LogManager.GetCurrentClassLogger().Info("Начало инициализации плагина");

            Initializer.InitializePlugin(plugin);

            LogManager.GetCurrentClassLogger().Info("Плагин инициализирован");

            return plugin;
        }

        public IPluginConfiguration ToFileSystem(IPluginFileSystem fileSystem, IPluginPackage package)
        {
            try
            {

                Check.NotNull(fileSystem);
                Check.NotNull(package);

                var service = new ZipPluginPackageService();
                using (var archiveStream = package.GetStream())
                {
                    using (var manifestFile = service.UnpackXMLManifest(archiveStream))
                    {
                        var packageFileSystem = service.GetFileSystem(archiveStream);
                        var manifest = new XMLManifest(XDocument.Load(manifestFile));
                        var configuration = manifest.CreateConfiguration();
                        var scheme = new PluginUnpackingScheme(configuration, packageFileSystem);

                        service.Unpack(archiveStream, fileSystem, scheme);

                        return configuration;
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка извлечения плагина из пакета в файловую систему");
            }
        }

    }
}
