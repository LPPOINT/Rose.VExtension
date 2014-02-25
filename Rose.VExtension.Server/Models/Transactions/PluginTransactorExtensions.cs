using System;
using System.Linq;
using Rose.VExtension.PluginSystem.Transactions;
using Rose.VExtension.Server.Models.DbInteraction;
using Rose.VExtension.Server.Models.Middleware;

namespace Rose.VExtension.Server.Models.Transactions
{
    public static class PluginTransactorExtensions
    {

        /// <summary>
        /// Возвращает загруженный плагин
        /// </summary>
        /// <param name="transactor"></param>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public static PluginSystem.Plugin GetPlugin(this IPluginTransactor transactor, string pluginId)
        {
            var plugin = transactor.Repository.PluginContext.GetEntity(pluginId);
            if(plugin.Location == PluginLocation.Undefined)
                return null;
            if (plugin.Location == PluginLocation.InRam)
            {
                var res =  transactor.Out.FirstOrDefault(plugin1 => plugin1.Id == pluginId);
                if (res == null)
                {
                    transactor.Sync(new PluginsPhysicalStatusProvider(transactor.Repository, transactor.Out));
                    res = transactor.Out.FirstOrDefault(plugin1 => plugin1.Id == pluginId);
                }
                return res;
            }
            if (plugin.Location == PluginLocation.InFileSystem)
            {
                var middleware = new FileSystemMiddleware();
                var fileSystem = middleware.CreateBase(plugin.PluginFileSystem);
                var transaction = TransactionBuilder.FromFileSystemToRam(fileSystem, pluginId);
                var result = transactor.ExecuteTransaction(transaction);

                return result.Plugin;

            }
            if (plugin.Location == PluginLocation.InPackage)
            {
                var fMiddleware = new FileSystemMiddleware();
                var pMiddlerare = new PackageMiddleware();

                var fileSystem = fMiddleware.CreateBase(plugin.PluginFileSystem);
                var packge = pMiddlerare.CreateBase(plugin.PluginPackage);

                var tranasction = TransactionBuilder.FromPackageToRam(packge, fileSystem, pluginId);
                var result = transactor.ExecuteTransaction(tranasction);

                return result.Plugin;

            }

            throw new NotSupportedException("Загрузка плагина и данной локации не поддерживается");
        }
    }
}