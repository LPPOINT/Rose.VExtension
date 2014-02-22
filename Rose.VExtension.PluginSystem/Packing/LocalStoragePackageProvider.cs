using Rose.VExtension.PluginSystem.Activation;

namespace Rose.VExtension.PluginSystem.Packing
{
    public class LocalStoragePackageProvider : IPluginPackageProvider
    {
        public LocalStoragePackageProvider(string packagePath)
        {
            PackagePath = packagePath;
        }

        public string PackagePath { get; private set; }
        public IPluginPackage GetPackage()
        {
            return new LocalStoragePluginPackage(PackagePath, PacckageType.ZipFile);
        }
    }
}
