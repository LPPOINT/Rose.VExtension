using System.IO;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public class PluginContentFile : IPluginFileSystemItem
    {
        private const string ContentFolder = "Content";

        public PluginContentFile(string name)
        {
            Name = name;
        }
        public PluginContentFile()
        {
            
        }

        public string Name { get; set; }

        public string Uri
        {
            get { return Path.Combine("Content", Name); }
        }
    }
}
