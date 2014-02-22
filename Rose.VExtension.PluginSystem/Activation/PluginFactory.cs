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


        public Plugin ToRunnable(IPluginFileSystem fileSystem, IPluginPackage package = null, IPluginInitializationHandler initializationHandler = null)
        {

            Check.NotNull(fileSystem);

            using (var manifestFileStream = fileSystem.GetItemStream(FileSystemItem.GetXMLManifestFile()))
            {
                var manifest = new XMLManifest(XDocument.Load(manifestFileStream));
                var config = manifest.CreateConfiguration();
                return ToRunnable(fileSystem, config, package, initializationHandler);
            }
        }

        public Plugin ToRunnable(IPluginFileSystem fileSystem, IPluginConfiguration pluginPluginConfiguration, IPluginPackage package = null, IPluginInitializationHandler initializationHandler = null)
        {

            if(initializationHandler == null)
                initializationHandler = new PluginInitializationHandler();

            Check.NotNull(fileSystem);
            Check.NotNull(pluginPluginConfiguration);

            var plugin = new Plugin();

            InitializePluginCore(fileSystem, pluginPluginConfiguration, package, initializationHandler, plugin);

            if (initializationHandler.ContainsFatalExceptions()) // Если обнаружены фатальные ошибки на этапе инициализации ядра, то нет смысла продолжать инициализацию
                return plugin;

            Initializer.InitializePlugin(plugin, initializationHandler);

            return plugin;
        }

        /// <summary>
        /// Инициализиреут основные свойства плагина
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="pluginPluginConfiguration"></param>
        /// <param name="package"></param>
        /// <param name="initializationHandler"></param>
        /// <param name="plugin"></param>
        private static void InitializePluginCore(IPluginFileSystem fileSystem, IPluginConfiguration pluginPluginConfiguration,
            IPluginPackage package, IPluginInitializationHandler initializationHandler, Plugin plugin)
        {
            try
            {
                plugin.PluginConfiguration = pluginPluginConfiguration;
            }
            catch (Exception e)
            {
                initializationHandler.OnException(
                    new ActivationStepException("Фатальная ошибка инициализации конфигурации плагина", e,
                        ActivationStepName.ConfigurationActivation, true));
            }

            try
            {
                plugin.FileSystem = fileSystem;
            }
            catch (Exception e)
            {
                initializationHandler.OnException(
                    new ActivationStepException("Фатальная ошибка инициализации файловой системы плагина", e,
                        ActivationStepName.FileSystemActivation, true));
            }
            try
            {
                plugin.Package = package;
            }
            catch (Exception e)
            {
                initializationHandler.OnException(new ActivationStepException("Фатальная ошибка инициализации пакета плагина", e,
                    ActivationStepName.PackageActivation, true));
            }
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
