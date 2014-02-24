using System;
using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Configuration
{

    public class PluginConfiguration : IPluginConfiguration
    {
        public PluginConfiguration(IConfigurationItem rootItem)
        {
            RootItem = rootItem;
        }

        public PluginConfiguration()
        {
            
        }

        public IConfigurationItem RootItem { get;  set; }

        private IEnumerable<IConfigurationItem> SearchInItem(IConfigurationItem item, string uri)
        {
            if (item.Uri == uri)
                return new List<IConfigurationItem>() {item};
            return item.InnerItems.Where(configurationItem => configurationItem.Uri == uri);
        }
        public IConfigurationItem GetItem(string path)
        {
            try
            {
                return SearchInItem(RootItem, path).FirstOrDefault();
            }
            catch 
            {
                throw new ConfigItemNotFoundException();
            }
        }

        public IEnumerable<IConfigurationItem> GetItems(string path)
        {
            try
            {
                return SearchInItem(RootItem, path);
            }
            catch
            {
                throw new ConfigItemNotFoundException();
            }
        }

        public string GetItemValue(string path)
        {
            try
            {
                var segments = path.Split(new []{'/'}, StringSplitOptions.RemoveEmptyEntries);

                var pathes = (segments.Take(segments.Length - 1));
                var itemPath = string.Empty;

                foreach (var p in pathes)
                {
                    itemPath += p + "/";
                }

                var itemContentName = segments.Last();

                var item = GetItem(itemPath);

                if (item == null)
                    return string.Empty;

                var value = item.Content.FirstOrDefault(pair => pair.Key == itemContentName);
                return value.Value;
            }
            catch
            {
                throw new ConfigItemNotFoundException();
            }

        }
        public bool ContainsItem(string path)
        {
            return GetItem(path) != null;
        }
        public bool ContainsItemValue(string path)
        {
            return GetItemValue(path) != string.Empty;
        }

        public static bool IsValidItemName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && (!name.Any(char.IsControl) || !name.Any(char.IsSeparator));
        }
        public static bool IsValidItemValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && (!value.Any(char.IsControl) || !value.Any(char.IsSeparator));
        }


    }
}
