using System.Security.Cryptography.X509Certificates;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public interface IConfigurationSyntax
    {
        /// <summary>
        /// Путь до корневого узла конфигурации
        /// </summary>
        string RootName { get;  }

        /// <summary>
        /// Путь до свойтва имени плагина
        /// </summary>
        string NamePath { get; }

        /// <summary>
        /// Путь до узла мета-информации о плагине
        /// </summary>
        string MetaPath { get; }

        /// <summary>
        /// Путь до свойства версии плагина
        /// </summary>
        string VersionPath { get; }

        /// <summary>
        /// Путь до узла платформы плагина
        /// </summary>
        string PlatformItem { get; }

        /// <summary>
        /// Путь до узла Javascript-файлов плагина на платформе <see cref="Activation.Platforms.JSPluginPlatform"/>
        /// </summary>
        string ScriptsItem { get;  }

        /// <summary>
        /// Путь до свойства входной javascript-функции плагина на платформе <see cref="Activation.Platforms.JSPluginPlatform"/>
        /// </summary>
        string JSEntryFunction { get; }

        /// <summary>
        /// Путь до свойства версии манифеста плагина
        /// </summary>
        string ManifestVersionItem { get; }

        /// <summary>
        /// Путь до узла фильтров страниц плагина на платформе <see cref="Activation.Platforms.RazorPluginPlatform"/>
        /// </summary>
        string RazorPagesFilters { get; }
    }

    public class ConfigurationSyntax : IConfigurationSyntax
    {

        public string RootName
        {
            get { return "Manifest"; }
        }

        public string NamePath
        {
            get { return string.Format("{0}/Name", RootName); }
        }

        public string MetaPath
        {
            get { return string.Format("{0}/Meta/", RootName); }
        }

        public string VersionPath
        {
            get { return string.Format("{0}/Version", RootName); }
        }

        public string PlatformItem { get { return string.Format("{0}/Platform/", RootName); } }

        public string ScriptsItem
        {
            get { return PlatformItem + "Scripts/"; }
        }

        public string JSEntryFunction { get { return PlatformItem + "EntryFunction"; } }
        public string ManifestVersionItem { get { return "Manifest/ManifestVersion"; }}
        public string RazorPagesFilters { get { return PlatformItem + "Filters/"; } }
    }

}
