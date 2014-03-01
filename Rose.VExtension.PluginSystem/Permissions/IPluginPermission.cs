using System.Collections.Generic;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Permissions
{
    public interface IPluginPermission
    {
        string Name { get;  }
        IConfigurationItem Configuration { get; }

        /// <summary>
        /// Возвращает конфигурацию в виде словаря значений
        /// </summary>
        IDictionary<string, string> Parameters { get; } 

    }
}
