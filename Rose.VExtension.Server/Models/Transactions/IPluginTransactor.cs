using System.Threading.Tasks;
using Rose.VExtension.PluginSystem.Transactions;
using Rose.VExtension.Server.Models.DbInteraction;

namespace Rose.VExtension.Server.Models.Transactions
{
    public interface IPluginTransactor
    {


        /// <summary>
        /// Коллекция, которой управляет текущий экземпляр транзактора при занчении <code>true</code> флага <see cref="InspectCollection"/>
        /// </summary>
        IPluginsCollection Out { get; set; }

        /// <summary>
        /// Указывает, следует ли текущему экземпляру транзактора управлять коллекцией плагинов
        /// </summary>
        bool ShouldInspectCollection { get; set; }

        IPluginsRepository Repository { get; }

        /// <summary>
        /// При перепределении выполняет заданную транзакцию
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        PluginTransactionResult ExecuteTransaction(IPluginTransaction transaction);

        /// <summary>
        /// Синхронизирует представления плагинов на сервере с физическими представляениями
        /// </summary>
        PluginsSyncResult Sync(IPluginsPhysicalStatusProvider statusProvider, SyncPriority priority = SyncPriority.ToServer);

        Task<PluginsSyncResult> SyncAsync(IPluginsPhysicalStatusProvider statusProvider,
            SyncPriority priority = SyncPriority.ToServer);

    }
}
