using System;
using System.Linq;
using Rose.VExtension.Server.Models.DbInteraction.Automation;

namespace Rose.VExtension.Server.Models.DbInteraction
{
    public class PluginsRepository : IPluginsRepository
    {


        private readonly PluginsContainer db;


        public RepositoryPluginController PluginContext { get; private set; }
        public RepositoryStorageController StorageContext { get; private set; }
        public RepositoryFileSystemController FileSystemContext { get; private set; }
        public RepositoryPackageController PackageContext { get; private set; }
        public RepositoryPluginComponentEntityController<ResourceToken, string> ResourceAccessTokenContext { get; private set; } 

        public PluginsRepository()
        {
            db = new PluginsContainer();
            PluginContext = new RepositoryPluginController(this, db, "Id");
            StorageContext = new RepositoryStorageController(this, db, "Id", "PluginId");
            ResourceAccessTokenContext = new RepositoryPluginComponentEntityController<ResourceToken, string>(this, db, "Id", "PluginId");
            FileSystemContext = new RepositoryFileSystemController(this, db);
            PackageContext = new RepositoryPackageController(this, db);
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

    }
}