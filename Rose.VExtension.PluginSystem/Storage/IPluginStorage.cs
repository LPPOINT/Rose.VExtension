using System;
using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.Storage
{


    public interface IPluginStorage : IEnumerable<IPluginStorageItem>
    {

        event EventHandler<PluginStorageItemEventArgs> ItemAdded;
        event EventHandler<PluginStorageItemRemovedEventArgs> ItemRemoved;
        event EventHandler<PluginStorageItemUpdatedEventArgs> ItemUpdated;
        event EventHandler StorageCleared;

        bool IsEventsEnabled { get; set; }

        void AddItem(IPluginStorageItem item);
        IPluginStorageItem GetItem(string itemName);
        void RemoveItem(string itemName);
        bool ContainsItem(string itemName);

        void ClearStorage();

        IEnumerable<string> Names { get; }
        int Count { get; }

    }
}
