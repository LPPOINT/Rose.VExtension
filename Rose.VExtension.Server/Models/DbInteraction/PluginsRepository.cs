using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;

namespace Rose.VExtension.Server.Models.DbInteraction
{
    public class PluginsRepository : IPluginsRepository
    {


        private readonly PluginsContainer db;


        public PluginsRepository()
        {
            db = new PluginsContainer();
        }

        #region IPluginsRepository Implementation

        public void AssociatePluginString(string str, string id)
        {

            db.PluginAssociationSet.Add(new PluginAssociation
                                        {
                                            PluginId = id,
                                            Seed = str
                                        });
            db.SaveChanges();

        }

        public string GetIdForStringAssociation(string str)
        {

            var query = from ass in db.PluginAssociationSet where ass.Seed == str select ass.PluginId;
            return query.FirstOrDefault();

        }

        public bool IsAssociatedId(string id)
        {

            var query = from ass in db.PluginAssociationSet where ass.PluginId == id select ass;
            return query.Any();

        }

        private static readonly Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            return Guid.NewGuid().ToString("N").Substring(0, size);
        }

        public string GenerateNewId()
        {
            string randomStr;
            do
            {
                randomStr = RandomString(9);

            } while (IsAssociatedId(randomStr));

            return randomStr;

        }

        #endregion

        #region Collections

        public IEnumerable<Plugin> Plugins
        {
            get
            {

                return db.Plugins.ToList();

            }
        }
        public IEnumerable<ResourceToken> ResourceAccessTokens
        {
            get
            {

                return db.ResourceTokens.ToList();

            }
        }
        public IEnumerable<StorageItem> StorageItems
        {
            get
            {

                return db.StorageItems.ToList();

            }
        }

        #endregion

        #region Services

        public IEnumerable<ResourceToken> GetResourceAccessTokensForPlugin(string pluginId)
        {

            var query = from resourceToken in db.ResourceTokens
                        where resourceToken.PluginId == pluginId
                        select resourceToken;
            return query.ToList();

        }
        public IEnumerable<StorageItem> GetStorageItemsForPlugin(string pluginId)
        {

            var query = from storageItem in db.StorageItems
                        where storageItem.PluginId == pluginId
                        select storageItem;
            return query.ToList();

        }

        #endregion

        #region Plugin

        public Plugin GetPlugin(string pluginId)
        {

            return (from plugin in db.Plugins where plugin.Id == pluginId select plugin).FirstOrDefault();

        }

        public Plugin AddPlugin(Plugin plugin)
        {
            try
            {


                db.SaveChanges();

                db.Plugins.Add(plugin);
                db.SaveChanges();


                return plugin;
            }
            catch
            {
                return null;
            }

        }

        public void RemovePlugin(string pluginId)
        {

            var query = from p in db.Plugins where p.Id == pluginId select p;
            var plugin = query.FirstOrDefault();
            if (plugin != null)
            {
                db.Plugins.Remove(plugin);
                db.SaveChanges();
            }
        }

        public void SetPluginLocation(string pluginId, PluginLocation location)
        {

            var plugin = (from p in db.Plugins where p.Id == pluginId select p).FirstOrDefault();
            if (plugin != null)
            {
                plugin.Location = location;
                db.SaveChanges();
            }

        }

        #endregion

        #region Storage

        public void ClearPluginStorageItems()
        {

            var items = db.StorageItems;

            foreach (var storageItem in items)
            {
                db.StorageItems.Remove(storageItem);
            }

            db.SaveChanges();


        }

        public void ClearPluginStorageItems(string id)
        {

            var items = (from item in db.StorageItems where item.PluginId == id select item).ToList();

            foreach (var storageItem in items)
            {
                db.StorageItems.Remove(storageItem);
            }

            db.SaveChanges();

        }

        public void SetStorageItemValue(string id, string key, string val)
        {

            var item = (from i in db.StorageItems where i.PluginId == id && i.Name == key select i).FirstOrDefault();
            if (item != null)
            {
                item.Value = val;
                db.SaveChanges();
            }


        }

        public void RemoveStorageItem(string id, string name)
        {

            var item = (from i in db.StorageItems where i.PluginId == id && i.Name == name select i).FirstOrDefault();
            if (item != null)
            {
                db.StorageItems.Remove(item);
                db.SaveChanges();
            }

        }

        public void AddStorageItem(string id, string name, string value)
        {

            var item = new StorageItem { Name = name, PluginId = id, Value = value };
            db.StorageItems.Add(item);
            db.SaveChanges();

        }

        public void AddStorageItem(StorageItem storageItem)
        {

            db.StorageItems.Add(storageItem);
            db.SaveChanges();
        }

        #endregion


        public void AddResourceAccessToken(ResourceToken at)
        {

            db.ResourceTokens.Add(at);
            db.SaveChanges();

        }

        public ResourceToken GetResourceAccessToken(string token)
        {
            var t = db.ResourceTokens.FirstOrDefault(resourceToken => resourceToken.Id == token);
            return t;
        }

        public void AddPluginFileSystem(PluginFileSystem fileSystem)
        {
            var plugin = db.Plugins.FirstOrDefault(plugin1 => plugin1.Id == fileSystem.PluginId);

            if(plugin != null)
                plugin.PluginFileSystem = fileSystem;

            db.PluginFileSystems.Add(fileSystem);
            db.SaveChanges();
        }

        public PluginFileSystem GetPluginFileSystemByPluginId(string pluginId)
        {
            var quey = from fs in db.PluginFileSystems where fs.PluginId == pluginId select fs;
            return quey.FirstOrDefault();

        }

        public void RemovePluginFileSystemByPluginId(string id)
        {
            var fileSystem = (from fs in db.PluginFileSystems where fs.PluginId == id select fs).FirstOrDefault();
            db.PluginFileSystems.Remove(fileSystem);
            db.SaveChanges();
        }

        public void AddPluginPackage(PluginPackage package)
        {
            var plugin = db.Plugins.FirstOrDefault(plugin1 => plugin1.Id == package.PluginId);

            if (plugin != null)
                plugin.PluginPackage = package;


            db.PluginPackages.Add(package);
            db.SaveChanges();
        }

        public void RemovePluginPackageByPluginId(string pluginId)
        {
            var package = (from p in db.PluginPackages where p.PluginId == pluginId select p).FirstOrDefault();
            if (package != null)
            {
                db.PluginPackages.Remove(package);
                db.SaveChanges();
            }
        }

   

        public void ClearResourceAccessTokensForPlugin(string id)
        {
            var resourceAccessTokens =  (from rat in db.ResourceTokens where rat.PluginId == id select rat).ToList();

            foreach (var rat in resourceAccessTokens)
            {
                db.ResourceTokens.Remove(rat);
            }
            db.SaveChanges();

        }

        public void ClearStorageItemsForPlugin(string id)
        {
            var items = (from si in db.StorageItems where si.PluginId == id select si).ToList();

            foreach (var si in items)
            {
                db.StorageItems.Remove(si);
            }
            db.SaveChanges();
        }
    }
}