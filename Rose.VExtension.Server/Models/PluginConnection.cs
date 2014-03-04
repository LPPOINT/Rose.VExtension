using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Rose.VExtension.PluginSystem.Storage;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Middleware;

namespace Rose.VExtension.Server.Models
{

    public class PluginConnectionException : Exception
    {
        public PluginConnectionException()
        {
        }

        public PluginConnectionException(string message) : base(message)
        {
        }

        public PluginConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class PluginConnection : IPluginConnection
    {


        private static readonly List<PluginConnection> connections = new List<PluginConnection>();

        private static void RemoveConnection(string connectionId)
        {
            var connection =
                connections.FirstOrDefault(pluginConnection => pluginConnection.ConnectionId == connectionId);
            if (connection != null)
                connections.Remove(connection);
        }

        private PluginConnection(PluginSystem.Plugin controlPlugin,Plugin pluginEntity, IPluginsRepository repository)
        {
            Repository = repository;
            PluginEntity = pluginEntity;
            ControlPlugin = controlPlugin;

            ConnectionId = controlPlugin.Id;

        }

        public string ConnectionId { get; private set; }

        public PluginSystem.Plugin ControlPlugin { get; private set; }
        public Plugin PluginEntity { get; private set; }
        public IPluginsRepository Repository { get; private set; }

        public void Open()
        {

            try
            {
                ControlPlugin.Storage.ItemAdded += StorageOnItemAdded;
                ControlPlugin.Storage.ItemRemoved += StorageOnItemRemoved;
                ControlPlugin.Storage.ItemUpdated += StorageOnItemUpdated;
                ControlPlugin.Storage.StorageCleared += StorageOnStorageCleared;

                var provider = new PluginFileSystemResourcesProvider(ControlPlugin.FileSystem, Repository) { PluginId = ControlPlugin.Id };
                provider.InitializeInnoreListByDefault(ControlPlugin);

                ControlPlugin.ResourcesProvider = provider;



                Syncronize();

                LogManager.GetCurrentClassLogger().Info("Открыто соединение с плагином " + ControlPlugin.Id);
            }
            catch (Exception e)
            {
                throw new PluginConnectionException("Не удалось открыть соединение с плагином", e);
            }

        }

        private void StorageOnStorageCleared(object sender, EventArgs eventArgs)
        {
            Repository.StorageContext.RemoveEntitiesByPluginId(ControlPlugin.Id);
        }
        private void StorageOnItemUpdated(object sender, PluginStorageItemUpdatedEventArgs pluginStorageItemUpdatedEventArgs)
        {
            Repository.StorageContext.SetItemValue(ControlPlugin.Id, pluginStorageItemUpdatedEventArgs.Item.Name, pluginStorageItemUpdatedEventArgs.NewValue);
        }
        private void StorageOnItemRemoved(object sender, PluginStorageItemRemovedEventArgs pluginStorageItemRemovedEventArgs)
        {
            Repository.StorageContext.RemoveItemByName(ControlPlugin.Id, pluginStorageItemRemovedEventArgs.Item.Name);
        }
        private void StorageOnItemAdded(object sender, PluginStorageItemEventArgs pluginStorageItemEventArgs)
        {
            Repository.StorageContext.AddEntity(pluginStorageItemEventArgs.Item.Name, pluginStorageItemEventArgs.Item.Value, ControlPlugin.Id);
        }

        public void Close()
        {
            try
            {
                ControlPlugin.Storage.ItemAdded -= StorageOnItemAdded;
                ControlPlugin.Storage.ItemRemoved -= StorageOnItemRemoved;
                ControlPlugin.Storage.ItemUpdated -= StorageOnItemUpdated;
                ControlPlugin.Storage.StorageCleared -= StorageOnStorageCleared;

                ControlPlugin.ResourcesProvider = null;

                LogManager.GetCurrentClassLogger().Info("Закрыто соединение с плагином " + ControlPlugin.Id);
            }
            catch (Exception e)
            {
                throw new PluginConnectionException("Не удалось закрыть соединение с плагином", e);
            }
        }

        public void Drop()
        {
            Drop(ConnectionDropOptions.Default);

        }

        public void Drop(ConnectionDropOptions options)
        {
            DropConnection(ControlPlugin, Repository, options);
            LogManager.GetCurrentClassLogger().Info("Сброшено соединение с плагином " + ControlPlugin.Id);
        }

        public void Syncronize()
        {
            try
            {
                SyncronizeStorages();
            }
            catch (Exception e)
            {
                throw new PluginConnectionException("Ошибка синхронизации соединения", e);
            }
        }

        private void SyncronizeStorages()
        {
            var systemStorage = ControlPlugin.Storage;
            var entityStorage = Repository.StorageContext.GetEntitiesByPluginId(PluginEntity.Id);

            systemStorage.IsEventsEnabled = false;

            foreach (var item in systemStorage)
            {
                if (entityStorage.All(storageItem => storageItem.Name != item.Name))
                {
                    Repository.StorageContext.AddEntity(item.Name, item.Value, PluginEntity.Id);
                }
            }

            systemStorage.IsEventsEnabled = false;

            foreach (var item in entityStorage)
            {
                if (!systemStorage.ContainsItem(item.Name))
                {
                    systemStorage.AddItem(new PluginStorageItem(item.Value, item.Value));
                }
            }

            systemStorage.IsEventsEnabled = true;
        }

        /// <summary>
        /// Создает новое соединение.
        /// </summary>
        /// <param name="plugin">Плагин, который требуется соеденить с его серверным представлением</param>
        /// <param name="repository">Репозиторий серверных представлений</param>
        /// <returns></returns>
        public static IPluginConnection CreateConnection(PluginSystem.Plugin plugin, IPluginsRepository repository)
        {
            var entity = new Plugin();

            entity.Id = plugin.Id;
            entity.Name = plugin.Name;
            entity.Version = plugin.Version.ToString();

            repository.PluginContext.AddEntity(entity);

            var fsMiddleware = new FileSystemMiddleware();
            var fileSystem = fsMiddleware.CreateEntity(plugin.FileSystem);

            fileSystem.PluginId = plugin.Id;

            var pckMiddleware = new PackageMiddleware();
            var package = pckMiddleware.CreateEntity(plugin.Package);

            package.PluginId = plugin.Id;

            repository.FileSystemContext.AddEntity(fileSystem);
            repository.PackageContext.AddEntity(package);

            foreach (var item in plugin.Storage)
            {
                var itemEntity = new StorageItem
                                 {
                                     Name = item.Name,
                                     Value = item.Value,
                                     PluginId = plugin.Id
                                 };

               repository.StorageContext.AddEntity(itemEntity);
               entity.StorageItems.Add(itemEntity);

            }

            var connection =  new PluginConnection(plugin, entity, repository);
            connections.Add(connection);
            return connection;

        }

        public static void DropConnection(PluginSystem.Plugin plugin, IPluginsRepository repository, ConnectionDropOptions options)
        {
            DropConnection(plugin.Id, repository, options);

        }

        public static void DropConnection(Plugin entity, IPluginsRepository repository, ConnectionDropOptions options)
        {
            DropConnection(entity.Id, repository, options);
        }

        public static void DropConnection(string pluginId, IPluginsRepository repository, ConnectionDropOptions options)
        {
            try
            {

                if (options.DeleteFileSystem)
                {
                    try
                    {
                        var fsEntity = repository.FileSystemContext.GetEntitiesByPluginId(pluginId).FirstOrDefault();

                        if (fsEntity != null)
                        {
                            var fsm = new FileSystemMiddleware();
                            var fs = fsm.CreateBase(fsEntity);
                            fs.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                //TODO: Add options.DeletePackage handler

                if(options.DeleteReservation)
                    repository.RemoveReservationById(pluginId);

                repository.ResourceAccessTokenContext.RemoveEntitiesByPluginId(pluginId);
                repository.FileSystemContext.RemoveEntitiesByPluginId(pluginId);
                repository.PackageContext.RemoveEntitiesByPluginId(pluginId);

                if(options.DeleteStorageItems)
                    repository.StorageContext.RemoveEntitiesByPluginId(pluginId);

                repository.PluginContext.RemoveEntity(pluginId);

                RemoveConnection(pluginId);
            }
            catch (Exception e)
            {
                throw new PluginConnectionException("Не удалосьсь сбросить соединение с плагином", e);
            }
        }

        public static IPluginConnection CreateConnection(PluginSystem.Plugin systemPlugin, Plugin pluginEntity,
            IPluginsRepository repository)
        {
            var connection = new PluginConnection(systemPlugin, pluginEntity, repository);

            connections.Add(connection);

            return connection;
        }


        public static IPluginConnection GetConnection(string pluginId)
        {
            return connections.FirstOrDefault(connection => connection.PluginEntity.Id == pluginId);
        }


    }
}