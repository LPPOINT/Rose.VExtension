using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rose.VExtension.PluginSystem.Activation;
using Rose.VExtension.PluginSystem.FileSystem;

namespace Rose.VExtension.PluginSystem.Transactions
{
    public interface IPluginTransactionNode
    {
        PluginStatus Status { get; }
        string PluginId { get; }
        IPluginFileSystem FileSystem { get; }
    }
}
