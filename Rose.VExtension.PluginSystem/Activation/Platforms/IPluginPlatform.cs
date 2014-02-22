namespace Rose.VExtension.PluginSystem.Activation.Platforms
{

    /// <summary>
    /// Содержит типы доступных платформ 
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// Плагформа на языеке CSharp
        /// </summary>
        CSharp,
        /// <summary>
        /// Платформа на языке Javascript
        /// </summary>
        Javascript,
        /// <summary>
        /// Платформа на языке Razor
        /// </summary>
        Razor
    }

    /// <summary>
    /// Представляет платформу плагина, содержащую сведения об активации контроллера
    /// </summary>
    public interface IPluginPlatform
    {
        Plugin Plugin { get; }
        PlatformType Type { get; }

        void BuildDomain();

    }
}
