namespace Rose.VExtension.PluginSystem.Packing
{

    /// <summary>
    /// Инкапсулирует операции получения пакета плагина
    /// </summary>
    public interface IPluginPackageProvider
    {

        /// <summary>
        /// При переопределении возвращает пакет плагина.
        /// </summary>
        /// <returns></returns>
        IPluginPackage GetPackage();
    }
}
