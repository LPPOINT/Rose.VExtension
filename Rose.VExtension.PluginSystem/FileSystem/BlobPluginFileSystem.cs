using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Rose.VExtension.PluginSystem.FileSystem
{
    public class BlobPluginFileSystem : IPluginFileSystem
    {

        public BlobPluginFileSystem(string blobUniquePrefix)
        {
            BlobUniquePrefix = blobUniquePrefix;
            var connection = CloudConfigurationManager.GetSetting(
                "UseDevelopmentStorage=true");
            var account = CloudStorageAccount.Parse(connection);
            CloudBlobClient client = account.CreateCloudBlobClient();
            fileSystemContainer = client.GetContainerReference("plugins");
        }

        public bool IsExist { get { return true; }}

        private readonly CloudBlobContainer fileSystemContainer;
        public string BlobUniquePrefix { get; private set; }
        private string GetFullBlobName(string itemName)
        {
            return BlobUniquePrefix + "_" + itemName;
        }
        public void AddItem(IPluginFileSystemItem item, Stream stream)
        {
            var blobName = GetFullBlobName(item.Uri);
            var blob = fileSystemContainer.GetBlockBlobReference(blobName);
            blob.UploadFromStream(stream);
        }
        public void RemoveItem(IPluginFileSystemItem item)
        {
            throw new System.NotImplementedException();
        }
        public Stream GetItemStream(IPluginFileSystemItem item)
        {
            var blobName = GetFullBlobName(item.Uri);
            var blob = fileSystemContainer.GetBlockBlobReference(blobName);
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            return stream;
        }
        public bool ContainsItem(IPluginFileSystemItem item)
        {
            throw new System.NotImplementedException();
        }
    }
}
