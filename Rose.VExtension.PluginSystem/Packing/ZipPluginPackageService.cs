using System.IO;
using System.IO.Compression;
using Rose.VExtension.PluginSystem.Common;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Packing
{
    public class ZipPluginPackageService : IPluginPackageService
    {
        public Stream UnpackFile(string fileName, Stream archiveStream)
        {

            Check.NotNullOrWhiteSpace(fileName);
            Check.NotNull(archiveStream);

            var archive = new ZipArchive(archiveStream);
                var entry = archive.GetEntry(fileName);
                return entry.Open();
        }

        public void Unpack(Stream archiveStream, IPluginFileSystem fileSystem, IPluginUnpackingScheme unpackingScheme)
        {
            Check.NotNull(archiveStream);
            Check.NotNull(fileSystem);
            Check.NotNull(unpackingScheme);

            using (var zip = new ZipArchive(archiveStream))
            {
                foreach (var itemSource in unpackingScheme.ItemsSourceScheme)
                {
                    var e = zip.Entries;
                    var entry = zip.GetEntry(itemSource.Key.Replace("\\", "/"));
                    if (entry != null)
                    {
                        using (var stream = entry.Open())
                        {

                            fileSystem.AddItem(itemSource.Value, stream);
                        }
                    }
                }
            }
        }

        public IPluginPackageFileSystem GetFileSystem(Stream archive)
        {
            Check.NotNull(archive);
            return new ZipPluginPackageFileSystem(archive);
        }
    }
}
