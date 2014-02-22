using System;
using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.Storage
{
    public sealed class PluginStorageItem : IPluginStorageItem
    {

        public PluginStorageItem(string name)
        {
            Name = name;
            Value = string.Empty;
            AccessFilter = new List<string>();
        }
        public PluginStorageItem(string name, string value)
        {
            Name = name;
            Value = value;
            AccessFilter = new List<string>();
        }
        public PluginStorageItem(string name, string value, IEnumerable<string> accessFilter)
        {
            Name = name;
            Value = value;
            AccessFilter = new List<string>(accessFilter);
        }

        public event EventHandler<PluginStorageItemUpdatedEventArgs> ValueChanged;
        private  void OnValueChanged(PluginStorageItemUpdatedEventArgs e)
        {
            var handler = ValueChanged;
            if (handler != null) handler(this, e);
        }

        public string Name { get; private set; }

        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                if (value != this.value)
                {
                    var old = this.value;
                    this.value = value;
                    OnValueChanged(new PluginStorageItemUpdatedEventArgs(this, old));
                }
            }
        }

        public IList<string> AccessFilter { get; set; }
    }
}
