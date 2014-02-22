using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;

namespace Rose.VExtension.PluginSystem.Activation
{
    /// <summary>
    /// Представляет набор методов для создания экземпляра класса <see cref="Plugin"/>
    /// </summary>
    public interface IPluginFactory
    {
        /// <summary>
        /// Загружает плагин из заданной файловой системы плагина, используя конфигурацию, созданную из файла Manifest.xml
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <returns></returns>
        Plugin ToRunnable(IPluginFileSystem fileSystem, IPluginPackage package = null, IPluginInitializationHandler initializationHandler = null);
        /// <summary>
        /// Загружает плагин из заданной файловой системы плагина, используя заданную конфигурацию
        /// </summary>
        /// <param name="fileSystem">Файловая система плагина</param>
        /// <param name="pluginPluginConfiguration">Конфигурация плагина</param>
        /// <returns></returns>
        Plugin ToRunnable(IPluginFileSystem fileSystem, IPluginConfiguration pluginPluginConfiguration, IPluginPackage package = null, IPluginInitializationHandler initializationHandler = null);

        /// <summary>
        /// Распаковывает пакет с плагином в заданную файловую систему
        /// </summary>
        /// <param name="fileSystem">Файловая система, в которую будет распакован плагин</param>
        /// <param name="package">Пакет плагина</param>
        /// <returns></returns>
        IPluginConfiguration ToFileSystem(IPluginFileSystem fileSystem, IPluginPackage package);

    }
}
