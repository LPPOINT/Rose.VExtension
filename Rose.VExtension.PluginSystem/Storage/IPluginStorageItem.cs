using System;
using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.Storage
{
    public interface IPluginStorageItem
    {

        event EventHandler<PluginStorageItemUpdatedEventArgs> ValueChanged; 

        string Name { get; }
        string Value { get; }
        IList<string> AccessFilter { get; set; }
    }
}
