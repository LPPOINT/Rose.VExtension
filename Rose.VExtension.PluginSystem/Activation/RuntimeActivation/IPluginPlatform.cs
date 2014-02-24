using System;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Runtime;

namespace Rose.VExtension.PluginSystem.Activation.RuntimeActivation
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

        IConfigurationItem ConfigurationItem { get; }

        IPluginActivity CreateActivity();
    }

    public static class PluginPlatformExtensions
    {
        public static string GetActivityName(this IPluginPlatform platform)
        {
            try
            {
                return platform.ConfigurationItem.GetContentValue("Name");
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static bool HasActivityName(this IPluginPlatform platform)
        {
            return !string.IsNullOrWhiteSpace(GetActivityName(platform));
        }
    }

}
