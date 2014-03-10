using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.Server.DbInteraction.Automation;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.DbInteraction
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
