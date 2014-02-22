using System;
using System.IO;
using System.IO.Compression;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public class LocalPluginFileSystem : IPluginFileSystem
    {
        public LocalPluginFileSystem(string rootFolder)
        {
            RootFolder = rootFolder;
        }

        public bool IsExist
        {
            get { return Directory.Exists(RootFolder); }
        }
        public string RootFolder { get; private set; }

        public void AddItem(IPluginFileSystemItem item, Stream stream)
        {
            WriteFile(stream, Path.Combine(RootFolder, item.Uri));
        }

        public void RemoveItem(IPluginFileSystemItem item)
        {
            File.Delete(item.Uri);
        }

        public Stream GetItemStream(IPluginFileSystemItem item)
        {
            return new FileStream(Path.Combine(RootFolder, item.Uri), FileMode.Open);
        }

        public bool ContainsItem(IPluginFileSystemItem item)
        {
            return GetItemStream(item) != null;
        }

        private Stream ReadFile(string uri)
        {
            return new FileStream(uri, FileMode.Open);
        }

        public void SaveStreamToFile(string filename, Stream stream)
        {

                using (var fileStream = File.Create(filename))
                {

                    if (stream is DeflateStream)
                    {
                        stream.CopyTo(fileStream);
                    }
                    else
                    {
                        var data = new byte[stream.Length];

                        stream.Read(data, 0, data.Length);
                        fileStream.Write(data, 0, data.Length);
                    }
                }
        }

        private void WriteFile(Stream fileStream, string fileUri)
        {
            var dn = Path.GetDirectoryName(fileUri);
            if(!Directory.Exists(dn))
                Directory.CreateDirectory(dn);
            SaveStreamToFile(fileUri, fileStream);
        }
    }
}
