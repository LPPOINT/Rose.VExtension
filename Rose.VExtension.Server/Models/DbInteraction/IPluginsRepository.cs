using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.Server.Models.DbInteraction.Automation;

namespace Rose.VExtension.Server.Models.DbInteraction
{
    public interface IPluginsRepository : IPluginsReservationRepository
    {
        RepositoryPluginController PluginContext { get; }
        RepositoryStorageController StorageContext { get; }
        RepositoryFileSystemController FileSystemContext { get; }
        RepositoryPackageController PackageContext { get; }
        RepositoryPluginComponentEntityController<ResourceToken, string> ResourceAccessTokenContext { get; }
        RepositoryUserController UserContext { get; }


    }
}
