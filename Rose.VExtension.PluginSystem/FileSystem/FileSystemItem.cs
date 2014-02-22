using System.IO;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public class FileSystemItem : IPluginFileSystemItem
    {
        public FileSystemItem(string name)
        {
            Name = name;
            Uri = name;
        }
        public FileSystemItem(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        public FileSystemItem(string name, params string[] folders)
        {
            for (var i = 0; i < folders.Length; i++)
            {
                folders[i] += "/";
            }
            Name = name;
            Uri = string.Concat(folders);
        }

        public string Name { get; set; }
        public string Uri { get; private set; }


        public static FileSystemItem GetRazorPageItem(string pageName)
        {
            return new FileSystemItem(pageName, Path.Combine("Pages", pageName));
        }

        public static FileSystemItem GetXMLManifestFile()
        {
            return new FileSystemItem("Manifest.xml");
        }
        public static FileSystemItem GetAssemblyItem(string assemblyName)
        {
            return new FileSystemItem(assemblyName, Path.Combine("Assemblies", assemblyName));
        }
        public static FileSystemItem GetScriptItem(string scriptName)
        {
            return new FileSystemItem(scriptName, Path.Combine("Scripts", scriptName));
        }
        public static FileSystemItem GetResourceItem(string resName)
        {
            return new FileSystemItem(resName, Path.Combine("Resources", resName));
        }

        public static FileSystemItem GetSettingsFileItem(string settings)
        {
            return new FileSystemItem(settings);
        }
    }
}
