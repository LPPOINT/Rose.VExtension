using System;
using System.Collections.Generic;
using System.IO;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public interface IPluginFileSystem : IDisposable
    {
        bool IsExist { get; }

        void AddItem(IPluginFileSystemItem item, Stream stream);
        void RemoveItem(IPluginFileSystemItem item);
        Stream GetItemStream(IPluginFileSystemItem item);
        bool ContainsItem(IPluginFileSystemItem item);

        IEnumerable<IPluginFileSystemItem> EnumerateItems();
        IEnumerable<IPluginFileSystemItem> EnumerateItems(string path);


    }

    public static class PluginFileSystemExtensions
    {
        public static bool ContainsLogoFile(this IPluginFileSystem fileSystem)
        {
            return fileSystem.ContainsItem(FileSystemItem.GetLogoItem());
        }

        public static IPluginFileSystemItem GetDefaultLogoItem(this IPluginFileSystem fileSystem)
        {
            return FileSystemItem.GetLogoItem();
        }
    }

}
