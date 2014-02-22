using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;

namespace Rose.VExtension.PluginSystem.Transactions
{
    public class PackageTransactionNode : IPluginTransactionNode
    {
        public PackageTransactionNode(IPluginPackage package, IPluginFileSystem fileSystem, string pluginId)
        {
            PluginId = pluginId;
            FileSystem = fileSystem;
            Package = package;
            Status = PluginStatus.InPackage;
        }

        public IPluginPackage Package { get; private set; }
        public IPluginFileSystem FileSystem { get; private set; }
        public PluginStatus Status { get; private set; }
        public string PluginId { get; private set; }
    }
}
