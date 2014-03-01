using System.IO;
using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Packing
{
    public interface IPluginPackageService
    {
        Stream UnpackFile(string fileName, Stream archiveStream);
        void Unpack(Stream archiveStream, IPluginFileSystem fileSystem, IPluginUnpackingScheme unpackingScheme);
        IPluginPackageFileSystem GetFileSystem(Stream archive);

    }

    public static class PluginPackageServiceExtensions
    {
        public static Stream UnpackXMLManifest(this IPluginPackageService service, Stream archiveStream)
        {

            Check.NotNull(service);
            Check.NotNull(archiveStream);

            return service.UnpackFile("Manifest.xml", archiveStream);
        }
    }

}
