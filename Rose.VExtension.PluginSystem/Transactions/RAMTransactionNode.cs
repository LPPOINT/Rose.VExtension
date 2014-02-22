using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Transactions
{
    public class RAMTransactionNode : IPluginTransactionNode
    {

        public RAMTransactionNode(string pluginId, IPluginFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            PluginId = pluginId;
            Status = PluginStatus.InRAM;
        }

        public PluginStatus Status { get; private set; }
        public string PluginId { get; private set; }
        public IPluginFileSystem FileSystem { get; private set; }
    }
}
