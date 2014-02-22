using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Storage
{

    public class PluginStorageException : Exception
    {
        public PluginStorageException()
        {
        }

        public PluginStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PluginStorageException(string message) : base(message)
        {
        }
    }

    public class PluginStorage : IPluginStorage
    {

        public PluginStorage()
        {
            items = new List<IPluginStorageItem>();
            IsEventsEnabled = true;
        }

        private readonly IList<IPluginStorageItem> items;

        #region События

        public event EventHandler<PluginStorageItemEventArgs> ItemAdded;

        protected virtual void OnItemAdded(PluginStorageItemEventArgs e)
        {
            var handler = ItemAdded;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<PluginStorageItemRemovedEventArgs> ItemRemoved;

        protected virtual void OnItemRemoved(PluginStorageItemRemovedEventArgs e)
        {
            var handler = ItemRemoved;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<PluginStorageItemUpdatedEventArgs> ItemUpdated;

        protected virtual void OnItemUpdated(PluginStorageItemUpdatedEventArgs e)
        {
            var handler = ItemUpdated;
            if (handler != null) handler(this, e);
        }

        public event EventHandler StorageCleared;
        public bool IsEventsEnabled { get; set; }

        protected virtual void OnStorageCleared()
        {
            var handler = StorageCleared;
            if(IsEventsEnabled)
                if (handler != null) handler(this, EventArgs.Empty);

        }

        #endregion

        public void AddItem(IPluginStorageItem item)
        {
            if (!ContainsItem(item.Name))
            {
                items.Add(item);
                item.ValueChanged += (sender, args) => OnItemUpdated(args);
                if(IsEventsEnabled)
                    OnItemAdded(new PluginStorageItemEventArgs(item, PluginStorageItemEventArgs.ActionType.Add));
            }
        }

        public IPluginStorageItem GetItem(string itemName)
        {
            return items.FirstOrDefault(item => item.Name == itemName);
        }

        public void RemoveItem(string itemName)
        {
            if (ContainsItem(itemName))
            {
                var item = items.First(i => i.Name == itemName);
                var index = items.IndexOf(item);
                items.Remove(item);
                if(IsEventsEnabled)
                    OnItemRemoved(new PluginStorageItemRemovedEventArgs(item, index));
            }
        }

        public bool ContainsItem(string itemName)
        {
            return items.Any(item => item.Name == itemName);
        }

        public void ClearStorage()
        {
            items.Clear();
            if(IsEventsEnabled)
                OnStorageCleared();
        }

        public IEnumerable<string> Names
        {
            get
            {
                var query = from pluginStorageItem in items select pluginStorageItem.Name;
                return query.ToList();
            }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public IEnumerator<IPluginStorageItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
