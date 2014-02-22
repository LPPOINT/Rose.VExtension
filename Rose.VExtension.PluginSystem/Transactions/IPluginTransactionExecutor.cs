namespace Rose.VExtension.PluginSystem.Transactions
{
    public interface IPluginTransactionExecutor
    {
        void ExecuteTransaction(IPluginTransaction transaction);
    }
}
