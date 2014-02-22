using System.Collections.Generic;
using System.IO;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public interface IPluginFileSystem
    {
        bool IsExist { get; }

        

        void AddItem(IPluginFileSystemItem item, Stream stream);
        void RemoveItem(IPluginFileSystemItem item);
        Stream GetItemStream(IPluginFileSystemItem item);
        bool ContainsItem(IPluginFileSystemItem item);

        IEnumerable<IPluginFileSystemItem> EnumerateItems();
        IEnumerable<IPluginFileSystemItem> EnumerateItems(string path);


    }
}
