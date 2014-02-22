using System.IO;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public interface IPluginFileSystemItem
    {
        string Name { get; set; }
        string Uri { get; }

    }

}
