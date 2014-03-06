using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NLog;
using RazorEngine;
using Encoding = System.Text.Encoding;

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
            using (var stream = GetItemStream(item))
            {
                if (stream != null)
                    return true;
                return false;
            }
        }

        public IEnumerable<IPluginFileSystemItem> EnumerateItems()
        {
            return EnumerateItems(string.Empty);
        }

        public IEnumerable<IPluginFileSystemItem> EnumerateItems(string path)
        {
            var files = Directory.EnumerateFiles(RootFolder);

            var res = (from file in files where path == string.Empty || file.StartsWith(path) select new FileSystemItem(file)).Cast<IPluginFileSystemItem>().ToList();

            var dirs = Directory.GetDirectories(RootFolder);
            res.AddRange((from dir in dirs from s in Directory.GetFiles(dir) where path == string.Empty || s.StartsWith(path) select new FileSystemItem(s)));

            return res;
        }

        private Stream ReadFile(string uri)
        {
            return new FileStream(uri, FileMode.Open);
        }

        public void SaveStreamToFile(string filename, Stream stream)
        {

            try
            {

                if(File.Exists(filename))
                    File.Delete(filename);

                using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate))
                {

                    if (stream is DeflateStream)
                    {
                        stream.CopyTo(fileStream);
                    }
                    else
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        var reader = new StreamReader(stream);
                        var text = reader.ReadToEnd();

                        var data = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(data, 0, data.Length);
                        stream.Seek(0, SeekOrigin.Begin);
                        fileStream.Write(data, 0, data.Length);
                        stream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        private void WriteFile(Stream fileStream, string fileUri)
        {
            var dn = Path.GetDirectoryName(fileUri);
            if(!Directory.Exists(dn))
                Directory.CreateDirectory(dn);
            SaveStreamToFile(fileUri, fileStream);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(RootFolder, true);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().ErrorException("Не удалось удалить папку плагина по пути " + RootFolder, e);
            }
        }
    }
}
