using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Transactions
{
    public class FileSystemTransactionNode : IPluginTransactionNode
    {

        public FileSystemTransactionNode(string pluginId, IPluginFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            PluginId = pluginId;
            Status = PluginStatus.InFileSystem;
        }

        public PluginStatus Status { get; private set; }
        public string PluginId { get; private set; }
        public IPluginFileSystem FileSystem { get; private set; }
    }
}
