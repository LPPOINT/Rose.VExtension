using System;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.Server.Models
{


    public class FileSystemMiddleware : IPluginComponentMiddleware<IPluginFileSystem, PluginFileSystem>
    {
        public PluginFileSystem CreateEntity(IPluginFileSystem pluginSystem)
        {
            if (pluginSystem is PluginSystem.FileSystem.LocalPluginFileSystem)
            {
                var localFileSystem = pluginSystem as PluginSystem.FileSystem.LocalPluginFileSystem;
                return new LocalPluginFileSystem
                       {
                           RootFolder = localFileSystem.RootFolder
                       };
            }

            throw new NotSupportedException("Заданная файловая ситема не поддерживает преобразование в сущность");

        }

        public IPluginFileSystem CreateBase(PluginFileSystem entity)
        {
            if (entity is LocalPluginFileSystem)
            {
                var localFileSystem = entity as LocalPluginFileSystem;
                return new PluginSystem.FileSystem.LocalPluginFileSystem(localFileSystem.RootFolder);
            }

            throw new NotSupportedException("Заданная сущность ситема не поддерживает преобразование в файловую систему");
        }

    }

}
