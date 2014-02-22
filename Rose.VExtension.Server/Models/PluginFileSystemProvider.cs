using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.Server.Models
{

    public interface IPluginFileSystemProvider
    {
        IPluginFileSystem GetFileSystem(Plugin plugin);
    }

    public class PluginFileSystemProvider : IPluginFileSystemProvider
    {
        public IPluginFileSystem GetFileSystem(Plugin plugin)
        {
            return null;
        }

    }
}