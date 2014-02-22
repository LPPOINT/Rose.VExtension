using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Middleware;

namespace Rose.VExtension.Server.Models
{

    /// <summary>
    /// Содержит набор значений, определяющих, на основе какой стороны необходимо выполнить синхронизацию плагинов
    /// </summary>
    public enum SyncPriority
    {
        /// <summary>
        /// Синхронизация по физическому состоянию плагина
        /// </summary>
        ToPhysical,
        /// <summary>
        /// Синхронизация по состоянию плагина на сервере
        /// </summary>
        ToServer
    }

    /// <summary>
    /// Представляет набор методов для получения статусов плагинов на основе их физического размещения
    /// </summary>
    public interface IPluginsPhysicalStatusProvider
    {

        IPluginsRepository Repository { get; }

        PluginLocation GetPhysicalLocation(string pluginId);
    }

    public class PluginsPhysicalStatusProvider : IPluginsPhysicalStatusProvider
    {
        public PluginsPhysicalStatusProvider(IPluginsRepository repository, ICollection<PluginSystem.Plugin> loaded)
        {
            Repository = repository;
            LoadedPlugins = loaded ?? new Collection<PluginSystem.Plugin>();
        }

        public ICollection<PluginSystem.Plugin> LoadedPlugins { get; private set; }  
        public IPluginsRepository Repository { get; private set; }

        public PluginLocation GetPhysicalLocation(string pluginId)
        {
            try
            {
                if(LoadedPlugins.Any(plugin => plugin.Id == pluginId))
                    return PluginLocation.InRam;
                var entity = Repository.GetPlugin(pluginId);

                if(entity == null)
                    return PluginLocation.Undefined;

                var fsMiddleware = new FileSystemMiddleware();
                var fs = fsMiddleware.CreateBase(entity.PluginFileSystem);

                if(fs.IsExist)
                    return PluginLocation.InFileSystem;

                var pckMiddleware = new PackageMiddleware();
                var pck = pckMiddleware.CreateBase(entity.PluginPackage);

                using (var stream = pck.GetStream())
                {
                    if(stream != null)
                        return PluginLocation.InPackage;
                }

                return PluginLocation.Undefined;
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().WarnException("Обнаружен неопределенный физический статус плагина", e);
                return PluginLocation.Undefined;
            }

        }
    }

}
