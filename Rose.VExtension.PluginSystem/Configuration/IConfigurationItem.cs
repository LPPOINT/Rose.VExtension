using System.Collections.Generic;
using System.Linq;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public interface IConfigurationItem
    {
        IPluginConfiguration PluginConfiguration { get; set; }
        IConfigurationItem Parent { get; set; }
        ICollection<IConfigurationItem> InnerItems { get; set; }
        ConfigurationItemContent Content { get; set; }
        string Name { get; set; }
        string Uri { get;  }
    }

    public static class ConfigurationItemExtensions
    {
        public static IConfigurationItem GetFirstChild(this IConfigurationItem item)
        {
            return item.InnerItems.FirstOrDefault();
        }
        public static IConfigurationItem GetLastChild(this IConfigurationItem item)
        {
            return item.InnerItems.LastOrDefault();
        }
        public static int GetChildsCount(this IConfigurationItem item)
        {
            return item.InnerItems.Count();
        }

        public static T WrapTo<T>(this IConfigurationItem item) where T : IConfigurationItemWrapper, new()
        {
            var wrapper = new T();
            wrapper.Wrap(item);
            return wrapper;
        }

        public static KeyValuePair<string, string> GetContentPair(this IConfigurationItem item, string name)
        {
            var val =  item.Content.FirstOrDefault(pair => pair.Key == name);
            if(val.Value == null)
                throw new ConfigItemNotFoundException();
            return val;
        }
        public static string GetContentValue(this IConfigurationItem item, string name)
        {
            var pair =  GetContentPair(item, name);

            if(pair.Value == null)
                throw new ConfigItemNotFoundException();

            return pair.Value;
        }

    }

}
