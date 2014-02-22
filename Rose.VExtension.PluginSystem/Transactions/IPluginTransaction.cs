namespace Rose.VExtension.PluginSystem.Transactions
{
    public interface IPluginTransaction
    {
        IPluginTransactionNode From { get;  }
        IPluginTransactionNode To { get; }
    }

    public class PluginTransaction : IPluginTransaction
    {
        public PluginTransaction(IPluginTransactionNode @from, IPluginTransactionNode to)
        {
            To = to;
            From = @from;
        }

        public IPluginTransactionNode From { get; private set; }
        public IPluginTransactionNode To { get; private set; }
    }

}
