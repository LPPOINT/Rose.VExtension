using System;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Middleware
{


    public class FileSystemMiddleware : IPluginComponentMiddleware<IPluginFileSystem, PluginFileSystem>
    {
        public PluginFileSystem CreateEntity(IPluginFileSystem pluginSystem)
        {
            if (pluginSystem is PluginSystem.FileSystem.LocalPluginFileSystem)
            {
                var localFileSystem = pluginSystem as PluginSystem.FileSystem.LocalPluginFileSystem;
                return new Models.DbInteraction.LocalPluginFileSystem
                       {
                           RootFolder = localFileSystem.RootFolder
                       };
            }

            throw new NotSupportedException("Заданная файловая ситема не поддерживает преобразование в сущность");

        }

        public IPluginFileSystem CreateBase(PluginFileSystem entity)
        {
            if (entity is Models.DbInteraction.LocalPluginFileSystem)
            {
                var localFileSystem = entity as Models.DbInteraction.LocalPluginFileSystem;
                return new PluginSystem.FileSystem.LocalPluginFileSystem(localFileSystem.RootFolder);
            }

            throw new NotSupportedException("Заданная сущность ситема не поддерживает преобразование в файловую систему");
        }

    }

}
