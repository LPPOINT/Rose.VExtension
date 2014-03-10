namespace Rose.VExtension.Server.Transactions
{
    public class PluginTransactionResult
    {

        public PluginTransactionResult()
        {
            
        }

        public PluginTransactionResult(PluginSystem.Plugin plugin)
        {
            Plugin = plugin;
        }
        public PluginSystem.Plugin Plugin { get; private set; }
    }
}