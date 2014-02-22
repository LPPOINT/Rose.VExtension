namespace Rose.VExtension.PluginSystem.Activation
{

    /// <summary>
    /// Представляет методы для инициализации плагина
    /// </summary>
    public interface IPluginInitializer
    {

        /// <summary>
        /// При переопределении выполняет полную инициализацию плагина
        /// </summary>
        /// <param name="plugin">Плагин, который будет инициализирован</param>
        void InitializePlugin(Plugin plugin, IPluginInitializationHandler handler);
    }
}
