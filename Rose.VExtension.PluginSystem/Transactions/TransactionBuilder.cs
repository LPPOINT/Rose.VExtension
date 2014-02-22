using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Packing;

namespace Rose.VExtension.PluginSystem.Transactions
{
    public static class TransactionBuilder
    {
        public static IPluginTransaction FromRamToFileSystem(Plugin plugin)
        {
            var from = new RAMTransactionNode(plugin.Id, plugin.FileSystem);
            var to = new FileSystemTransactionNode(plugin.Id, plugin.FileSystem);
            return new PluginTransaction(from, to);
        }

        public static IPluginTransaction FromRamToPackage(Plugin plugin, IPluginPackage package)
        {
            var from = new RAMTransactionNode(plugin.Id, plugin.FileSystem);
            var to = new PackageTransactionNode(package, plugin.FileSystem, plugin.Id);
            return new PluginTransaction(from, to);
        }

        public static IPluginTransaction FromPackageToFileSystem(IPluginPackage package, IPluginFileSystem fileSystem, string pluginId)
        {
            var from = new PackageTransactionNode(package, fileSystem, pluginId);
            var to = new FileSystemTransactionNode(pluginId, fileSystem);
            return new PluginTransaction(from, to);
        }

        public static IPluginTransaction FromPackageToFileSystem(IPluginPackage package, IPluginFileSystem fileSystem)
        {
            return FromPackageToFileSystem(package, fileSystem, string.Empty);
        }

        public static IPluginTransaction FromPackageToRam(IPluginPackage package, IPluginFileSystem fileSystem,
            string pluginId)
        {
            var from = new PackageTransactionNode(package, fileSystem, pluginId);
            var to = new RAMTransactionNode(pluginId, fileSystem);
            return new PluginTransaction(from, to);
        }

        public static IPluginTransaction FromPackageToRam(IPluginPackage package, IPluginFileSystem fileSystem)
        {
            return FromPackageToRam(package, fileSystem, string.Empty);
        }

        public static IPluginTransaction FromFileSystemToRam(IPluginFileSystem fileSystem, string pluginId)
        {
            var from = new FileSystemTransactionNode(pluginId, fileSystem);
            var to = new RAMTransactionNode(pluginId, fileSystem);
            return new PluginTransaction(from, to);
        }
        public static IPluginTransaction FromFileSystemToRam(IPluginFileSystem fileSystem)
        {
            return FromFileSystemToRam(fileSystem, string.Empty);
        }

        public static IPluginTransaction FromFileSystemToPackage(IPluginFileSystem fileSystem, IPluginPackage package,
            string pluginId)
        {
            var from = new FileSystemTransactionNode(pluginId, fileSystem);
            var to = new PackageTransactionNode(package, fileSystem, pluginId);
            return new PluginTransaction(from, to);
        }

        public static IPluginTransaction FromFileSystemToPackage(IPluginFileSystem fileSystem, IPluginPackage package)
        {
            return FromFileSystemToPackage(fileSystem, package, string.Empty);
        }

    }
}
