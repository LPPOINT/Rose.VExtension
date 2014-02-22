using System;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Configuration
{

    public class ManifestParseException : Exception
    {
        public ManifestParseException()
        {
        }

        public ManifestParseException(string message) : base(message)
        {
        }

        public ManifestParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Представляет конфигурацию, описанную на определенном языке 
    /// </summary>
    public interface IManifest
    {

        /// <summary>
        /// Создает независимую от языка модель конфигурации
        /// </summary>
        /// <returns></returns>
        IPluginConfiguration CreateConfiguration();
    }
}
