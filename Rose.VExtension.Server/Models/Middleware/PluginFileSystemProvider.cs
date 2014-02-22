using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Models.Middleware
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