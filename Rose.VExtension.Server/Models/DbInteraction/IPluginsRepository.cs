using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Rose.VExtension.PluginSystem.Reservation;

namespace Rose.VExtension.Server.Models.DbInteraction
{
    public interface IPluginsRepository : IPluginsReservationRepository
    {
        IEnumerable<Plugin> Plugins { get; }
        IEnumerable<ResourceToken> ResourceAccessTokens { get; }
        IEnumerable<StorageItem> StorageItems { get; }


        IEnumerable<ResourceToken> GetResourceAccessTokensForPlugin(string pluginId);
        IEnumerable<StorageItem> GetStorageItemsForPlugin(string pluginId);

        #region Plugin
        Plugin GetPlugin(string pluginId);
        Plugin AddPlugin(Plugin plugin);
        void RemovePlugin(string pluginId);
        void SetPluginLocation(string pluginId, PluginLocation location);

        #endregion

        #region Storage

        void ClearPluginStorageItems();
        void ClearPluginStorageItems(string id);
        void SetStorageItemValue(string id, string key, string val);
        void RemoveStorageItem(string id, string name);
        void AddStorageItem(string id, string name, string value);
        void AddStorageItem(StorageItem storageItem);
        void ClearStorageItemsForPlugin(string id);

        #endregion

        #region Resources
        void AddResourceAccessToken(ResourceToken at);
        ResourceToken GetResourceAccessToken(string token);
        void ClearResourceAccessTokensForPlugin(string id);

        #endregion

        #region FileSystem

        void AddPluginFileSystem(PluginFileSystem fileSystem);
        PluginFileSystem GetPluginFileSystemByPluginId(string pluginId);
        void RemovePluginFileSystemByPluginId(string id);

        #endregion

        void AddPluginPackage(PluginPackage package);
        void RemovePluginPackageByPluginId(string pluginId);


    }
}
