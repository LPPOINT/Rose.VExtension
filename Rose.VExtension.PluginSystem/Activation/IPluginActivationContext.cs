using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;

namespace Rose.VExtension.PluginSystem.Activation
{
    /// <summary>
    /// Контекст, с помощью которого плагин может быть автивирован
    /// </summary>
    public interface IPluginActivationContext
    {

        /// <summary>
        /// Статус плагина
        /// </summary>
        PluginStatus Status { get;  }
    }


    /// <summary>
    /// Контекст плагина, загружаемого из файловой системы
    /// </summary>
    public class FileSystemPluginActivationContext : IPluginActivationContext
    {
        public FileSystemPluginActivationContext(IPluginFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            Status = PluginStatus.InFileSystem;
        }

        public PluginStatus Status { get; private set; }
        public IPluginFileSystem FileSystem { get; private set; }
    }

    public class PackagePluginActivationContext : IPluginActivationContext
    {
        public PackagePluginActivationContext(IPluginPackage package, IPluginFileSystem emptyFileSystem)
        {
            EmptyFileSystem = emptyFileSystem;
            Package = package;
            Status = PluginStatus.InPackage;
        }

        public PluginStatus Status { get; private set; }
        public IPluginPackage Package { get; private set; }
        public IPluginFileSystem EmptyFileSystem { get; private set; }
    }

}
