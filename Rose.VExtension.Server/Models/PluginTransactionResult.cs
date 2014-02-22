namespace Rose.VExtension.Server.Models
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