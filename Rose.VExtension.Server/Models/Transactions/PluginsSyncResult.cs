using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;

namespace Rose.VExtension.Server.Models.Transactions
{

    /// <summary>
    /// Представляет информацию о синхронизации плагинов
    /// </summary>
    public class PluginsSyncResult : IEnumerable<PluginTransactionResult>
    {

        public PluginsSyncResult(IEnumerable<PluginTransactionResult> transactions, int missed)
        {
            Transactions = new ReadOnlyCollection<PluginTransactionResult>(transactions.ToList());
            TotalSyncronized = Transactions.Count;
            Missed = missed;
        }

        /// <summary>
        /// Транзакции, выполненные для совершения синхронизации
        /// </summary>
        public ReadOnlyCollection<PluginTransactionResult> Transactions { get; private set; }

        /// <summary>
        /// Количество плагинов, для синхронизации которых были выполнены транзакции
        /// </summary>
        public int TotalSyncronized { get; private set; }

        /// <summary>
        /// Количество плагинов, не требующих транзакий при синхронизации
        /// </summary>
        public int Missed { get; private set; }

        public IEnumerator<PluginTransactionResult> GetEnumerator()
        {
            return Transactions.GetEnumerator();
        }

        public void MakeLogReport()
        {
            LogManager.GetCurrentClassLogger().Info("Выполнена синхронизация плагинов. \n   Всего синхронизировано: " + TotalSyncronized + "\n  Пропущено: " + Missed);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}