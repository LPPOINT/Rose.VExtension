using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.Transactions;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Middleware;

namespace Rose.VExtension.Server.Models.Transactions
{

    [Serializable]
    public class PluginTransactionException : Exception
    {


        public PluginTransactionException()
        {
        }

        public PluginTransactionException(string message)
            : base(message)
        {
        }

        public PluginTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PluginTransactionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class PluginSyncException : PluginTransactionException
    {
        public PluginSyncException()
        {
        }

        public PluginSyncException(string message) : base(message)
        {
        }

        public PluginSyncException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class PluginTransactor : IPluginTransactor
    {
        public PluginTransactor(IPluginsRepository repository)
        {
            Repository = repository;
        }

        public IPluginsCollection Out { get; set; }
        public bool ShouldInspectCollection { get; set; }

        public bool ShouldInspectCollectionInternal
        {
            get { return ShouldInspectCollection && Out != null; }
        }

        public IPluginsRepository Repository { get; private set; }

        public PluginTransactionResult ExecuteTransaction(IPluginTransaction transaction)
        {
            var factory = new PluginFactory(new PluginInitializer(Repository));

            try
            {
                if (transaction.From.Status == PluginStatus.InPackage)
                {
                    var packageTransaction = (transaction.From as PackageTransactionNode);

                    if (transaction.To.Status == PluginStatus.InRAM)
                    {
                        try
                        {
                            var config = factory.ToFileSystem(packageTransaction.FileSystem, packageTransaction.Package);
                            var plugin = factory.ToRunnable(transaction.To.FileSystem, config,
                                packageTransaction.Package);

                            Repository.SetPluginLocation(plugin.Id, PluginLocation.InRam);

                            var pluginEntity = Repository.GetPlugin(plugin.Id);

                            if (pluginEntity != null)
                            {
                                var connection = PluginConnection.CreateConnection(plugin, pluginEntity, Repository);
                                connection.Open();

                            }
                            else
                            {
                                var connection = PluginConnection.CreateConnection(plugin, Repository);
                                connection.Open();

                            }

                            AddPluginIfNecessary(plugin);

                            return new PluginTransactionResult(plugin);
                        }
                        catch (Exception e)
                        {
                            throw new PluginTransactionException(
                                "Ошибка транзакции плагина из пакета в управляемую память", e);
                        }
                    }
                    if (transaction.To.Status == PluginStatus.InFileSystem)
                    {
                        factory.ToFileSystem(packageTransaction.FileSystem, packageTransaction.Package);
                        Repository.SetPluginLocation(transaction.To.PluginId, PluginLocation.InFileSystem);

                        var id = transaction.To.PluginId;
                        RemovePluginIfNecessary(id);

                    }
                }
                if (transaction.From.Status == PluginStatus.InFileSystem)
                {
                    if (transaction.To.Status == PluginStatus.InRAM)
                    {
                        try
                        {


                            if(string.IsNullOrWhiteSpace(transaction.From.PluginId))
                                throw new PluginTransactionException("При транзакции плагина из файловой системы в управляемую память ожидается, что плагин был уже зарегестрирован");

                            var pluginEntity = Repository.GetPlugin(transaction.From.PluginId);
                            var fs = transaction.From as FileSystemTransactionNode;
                            var packageMiddleware = new PackageMiddleware();
                            var package = packageMiddleware.CreateBase(pluginEntity.PluginPackage);
                            var plugin = factory.ToRunnable(fs.FileSystem, package);

                            Repository.SetPluginLocation(plugin.Id, PluginLocation.InRam);

                            if (pluginEntity != null)
                            {
                                var connection = PluginConnection.CreateConnection(plugin, pluginEntity, Repository);
                                connection.Open();

                            }

                            AddPluginIfNecessary(plugin);

                            return new PluginTransactionResult(plugin);
                        }
                        catch (Exception e)
                        {
                            throw new PluginTransactionException(
                                "Ошибка транзакции плагина из пакета в управляемую память", e);
                        }
                    }
                }

                return new PluginTransactionResult();
            }
            catch (PluginTransactionException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new PluginTransactionException("Непредвиденная ошибка во время выполнения транзакции", e);
            }
        }

        private void AddPluginIfNecessary(PluginSystem.Plugin plugin)
        {
            if (ShouldInspectCollectionInternal)
                Out.Add(plugin);
        }

        private void RemovePluginIfNecessary(string id)
        {
            if (ShouldInspectCollectionInternal)
            {
                var p = (from plugin in Out where plugin.Id == id select plugin).FirstOrDefault();
                if (p != null)
                    Out.Remove(p);
            }
        }

        public PluginsSyncResult Sync(IPluginsPhysicalStatusProvider statusProvider, SyncPriority priority = SyncPriority.ToServer)
        {
            try
            {
                var transactionResults = new List<PluginTransactionResult>();
                var plugins = Repository.Plugins;
                var missed = 0;

                foreach (var pluginEntity in plugins)
                {
                    var serverStatus = pluginEntity.Location;
                    var physicalStatus = statusProvider.GetPhysicalLocation(pluginEntity.Id);

                    if (physicalStatus == PluginLocation.Undefined)
                    {
                        missed++;
                        continue;
                    }

                    if (serverStatus != physicalStatus)
                    {
                        if (priority == SyncPriority.ToPhysical)
                        {
                            SyncronizePluginByPhysical(pluginEntity, physicalStatus);
                        }
                        else if (priority == SyncPriority.ToServer)
                        {
                            SyncronizePluginByServer(pluginEntity, physicalStatus, serverStatus, transactionResults);
                        }
                    }
                    else
                    {
                        missed++;
                    }

                }
                return new PluginsSyncResult(transactionResults, missed);
            }
            catch (PluginSyncException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new PluginSyncException("Во время синхронизации плагинов возникла ошибка", e);
            }

        }

        public async Task<PluginsSyncResult> SyncAsync(IPluginsPhysicalStatusProvider statusProvider, SyncPriority priority = SyncPriority.ToServer)
        {
            return await Task.Run(() => Sync(statusProvider, priority));
        }

        private void SyncronizePluginByServer(Plugin pluginEntity, PluginLocation physicalStatus, PluginLocation serverStatus,
            ICollection<PluginTransactionResult> transactionResults)
        {
            try
            {
                var transaction = new PluginTransaction(GetTransactionNode(pluginEntity, physicalStatus),
                    GetTransactionNode(pluginEntity, serverStatus));

                var transactionResult = ExecuteTransaction(transaction);

                //if(transactionResult.Plugin != null)
                //    AddPluginIfNecessary(transactionResult.Plugin);
                //if((transaction.To is RAMTransactionNode) == false)
                //    RemovePluginIfNecessary(transaction.To.PluginId);

                transactionResults.Add(transactionResult);
            }
            catch (Exception e)
            {
                throw new PluginSyncException("Ошибка синхронизации плагина на основе сервера", e);
            }
        }

        private IPluginTransactionNode GetTransactionNode(Plugin plugin, PluginLocation location)
        {
            if (location == PluginLocation.InPackage)
            {
                var packageMiddleware = new PackageMiddleware();
                var package = packageMiddleware.CreateBase(plugin.PluginPackage);

                var fileSystemMiddleware = new FileSystemMiddleware();
                var fileSystem = fileSystemMiddleware.CreateBase(plugin.PluginFileSystem);

                return new PackageTransactionNode(package, fileSystem, plugin.Id);
            }
            if (location == PluginLocation.InFileSystem)
            {
                var fileSystemMiddleware = new FileSystemMiddleware();
                var fileSystem = fileSystemMiddleware.CreateBase(plugin.PluginFileSystem);
                return new FileSystemTransactionNode(plugin.Id, fileSystem);
            }
            if (location == PluginLocation.InRam)
            {
                var fileSystemMiddleware = new FileSystemMiddleware();
                var fileSystem = fileSystemMiddleware.CreateBase(plugin.PluginFileSystem);
                return new RAMTransactionNode(plugin.Id, fileSystem);
            }

            throw new ArgumentException("Недопустипый статус плагина", "location");

        }
        private void SyncronizePluginByPhysical(Plugin pluginEntity, PluginLocation physicalStatus)
        {
            Repository.SetPluginLocation(pluginEntity.Id, physicalStatus);
        }
    }
}