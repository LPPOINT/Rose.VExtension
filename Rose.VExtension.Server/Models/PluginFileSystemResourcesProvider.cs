using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.Optimization;
using Rose.VExtension.PluginSystem.FileSystem;
using Rose.VExtension.PluginSystem.Resources;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Models
{

    public class ResourceAccessDeniedException : Exception
    {

        public ResourceAccessDeniedException(string message, string reourcePath) : base(message)
        {
            ReourcePath = reourcePath;
        }

        public ResourceAccessDeniedException(string message, Exception innerException, string reourcePath) : base(message, innerException)
        {
            ReourcePath = reourcePath;
        }

        protected ResourceAccessDeniedException(SerializationInfo info, StreamingContext context, string reourcePath) : base(info, context)
        {
            ReourcePath = reourcePath;
        }

        public string ReourcePath { get; private set; }

        public ResourceAccessDeniedException() : base("")
        {
        }

        public ResourceAccessDeniedException(string resourcePath) : base(String.Format("Доступ к ресурсу по пути '{0}' запрещен", resourcePath))
        {
            ReourcePath = resourcePath;
        }

        public ResourceAccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResourceAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class PluginFileSystemResourcesProvider : IPluginResourcesProvider
    {
        public PluginFileSystemResourcesProvider(IPluginFileSystem fileSystem, IPluginsRepository repository) : this(fileSystem, repository, null)
        {

        }

        public PluginFileSystemResourcesProvider(IPluginFileSystem fileSystem, IPluginsRepository repository, IList<string> ignoreList)
        {
            Repository = repository;
            FileSystem = fileSystem;
            IgnoreList = ignoreList != null ? new Collection<string>(ignoreList) : new Collection<string>();
        }


        public IPluginFileSystem FileSystem { get; private set; }
        public IPluginsRepository Repository { get; private set; }

        public ICollection<string> IgnoreList { get; private set; }

        public string PluginId { get; set; }

        public Stream GetResourceStream(string resName)
        {

            ValidateResourcePath(resName);

            return FileSystem.GetItemStream(FileSystemItem.GetResourceItem(resName));
        }

        public void ValidateResourcePath(string resourcePath)
        {
            if (IgnoreList.Any(item => Regex.IsMatch(resourcePath, item)))
            {
                throw new ResourceAccessDeniedException(resourcePath);
            }
        }

        public void InitializeInnoreListByDefault(PluginSystem.Plugin plugin)
        {
            IgnoreList.Add("Manifest.xml");
            IgnoreList.Add("Assemblies/*");
            IgnoreList.Add("Settings.xml");
        }

        public bool ContainsResource(string resName)
        {
            return FileSystem.ContainsItem(FileSystemItem.GetResourceItem(resName));
        }

        public int ResourcesStorageSize { get { throw new NotImplementedException(); } }
        public int MaxResourcesStorageSize { get { throw new NotImplementedException(); } }

        public PluginResourceAccessToken ConfirmAccessToken(PluginResourceAccessToken accessToken)
        {
            ValidateResourcePath(accessToken.ResourcePath);

            var at = new ResourceToken
            {
                Lifetime = accessToken.ValidBefore,
                Id = accessToken.Token,
                PluginId = PluginId,
                ResourcePath = accessToken.ResourcePath
            };

            Repository.ResourceAccessTokenContext.AddEntity(at);

            accessToken.Url = "/Resources/" + at.Id;

            return accessToken;
        }
    }
}