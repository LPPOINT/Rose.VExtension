using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Packing
{

    public interface IPluginPackageFileSystem
    {
        IEnumerable<string> Files { get; } 
    }

    public class ZipPluginPackageFileSystem : IPluginPackageFileSystem
    {

        public ZipPluginPackageFileSystem(ZipArchive archive)
        {
            var list = new List<string>();

            foreach (var entry in archive.Entries)
            {
                list.Add(entry.FullName);
            }
            Files = list.ToArray();
        }

        public ZipPluginPackageFileSystem(Stream archiveStream) 
            : this(new ZipArchive(archiveStream))
        {
            
        }

        public IEnumerable<string> Files { get; private set; }
    }

    public static class PluginPackageFileSystemExtension
    {
        public static IEnumerable<string> GetFilesInFolder(this IPluginPackageFileSystem fs, string folderName)
        {
            return fs.Files.Where(s => s.StartsWith(folderName) && s != folderName + "/");
        }
    }

}