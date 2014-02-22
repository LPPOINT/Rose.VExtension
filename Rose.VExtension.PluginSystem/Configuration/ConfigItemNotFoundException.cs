using System;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public class ConfigItemNotFoundException : Exception
    {
        public ConfigItemNotFoundException()
        {
        }

        public ConfigItemNotFoundException(string message, string path) : base(message)
        {
            Path = path;
        }

        public ConfigItemNotFoundException(string message, Exception innerException, string path) : base(message, innerException)
        {
            Path = path;
        }


        public ConfigItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Путь до узла или свойства конфигурации
        /// </summary>
        public string Path { get; private set; }
    }
}
