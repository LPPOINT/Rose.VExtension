using System;

namespace Rose.VExtension.PluginSystem.Storage
{
    public class PluginStorageItemEventArgs : EventArgs
    {

        public enum ActionType
        {
            Add,
            Update,
            Remove
        }

        public PluginStorageItemEventArgs(IPluginStorageItem item, ActionType action)
        {
            Action = action;
            Item = item;
        }

        public ActionType Action { get; private set; }
        public IPluginStorageItem Item { get; private set; }
    }

    public class PluginStorageItemUpdatedEventArgs : PluginStorageItemEventArgs
    {

        public string OldValue { get; private set; }
        public string NewValue { get; private set; }

        public PluginStorageItemUpdatedEventArgs(IPluginStorageItem item, string oldValue)
            : base(item, ActionType.Update)
        {
            OldValue = oldValue;
            NewValue = item.Value;
        }
    }

    public class PluginStorageItemRemovedEventArgs : PluginStorageItemEventArgs
    {
        public PluginStorageItemRemovedEventArgs(IPluginStorageItem item, int oldIndex)
            : base(item, ActionType.Remove)
        {
            OldIndex = oldIndex;
        }

        public int OldIndex { get; private set; }
    }

}
