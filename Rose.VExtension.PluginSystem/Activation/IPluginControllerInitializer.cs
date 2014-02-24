namespace Rose.VExtension.PluginSystem.Activation
{

    /// <summary>
    /// Представляет методы для инициализации контроллера плагина
    /// </summary>
    public interface IPluginControllerInitializer
    {
        /// <summary>
        /// При переопределении инициализирует свойство <see cref="Plugin.Activity"/> заданного экземпляря класса <see cref="Plugin"/>
        /// </summary>
        /// <param name="plugin">Плагин, для которого требуется инициализировать контроллер</param>
        void InitializeController(Plugin plugin, ActivationInfo info);
    }
}
