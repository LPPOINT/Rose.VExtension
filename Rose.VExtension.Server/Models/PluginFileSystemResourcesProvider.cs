using System;
using System.IO;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Resources;

namespace Rose.VExtension.Server.Models
{
    public class PluginFileSystemResourcesProvider : IPluginResourcesProvider
    {
        public PluginFileSystemResourcesProvider(IPluginFileSystem fileSystem, IPluginsRepository repository)
        {
            Repository = repository;
            FileSystem = fileSystem;
        }

        public IPluginFileSystem FileSystem { get; private set; }
        public IPluginsRepository Repository { get; private set; }

        public string PluginId { get; set; }

        public Stream GetResourceStream(string resName)
        {
            return FileSystem.GetItemStream(FileSystemItem.GetResourceItem(resName));
        }

        public bool ContainsResource(string resName)
        {
            return FileSystem.ContainsItem(FileSystemItem.GetResourceItem(resName));
        }

        public int ResourcesStorageSize { get { throw new NotImplementedException(); } }
        public int MaxResourcesStorageSize { get { throw new NotImplementedException(); } }

        public PluginResourceAccessToken ConfirmAccessToken(PluginResourceAccessToken accessToken)
        {

            var at = new ResourceToken
            {
                Lifetime = accessToken.ValidBefore,
                Id = accessToken.Token,
                PluginId = PluginId,
                ResourceName = accessToken.ResourceName
            };

            Repository.AddResourceAccessToken(at);

            accessToken.Url = "/Resources/" + at.Id;

            return accessToken;
        }
    }
}