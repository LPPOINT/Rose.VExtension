using System;
using System.Collections.Generic;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public class ConfigurationItem : IConfigurationItem
    {
        public ConfigurationItem(IConfigurationItem parent, ICollection<IConfigurationItem> innerItems, ConfigurationItemContent content, string name, IPluginConfiguration pluginConfiguration)
        {
            PluginConfiguration = pluginConfiguration;
            Name = name;
            Content = content;
            InnerItems = innerItems;
            Parent = parent;
        }

        public ConfigurationItem(string name, ConfigurationItemContent content, params IConfigurationItem[] innerItems) 
            : this(null, innerItems, content, name, null)
        {
            
        }

        public ConfigurationItem(string name)
        {
            PluginConfiguration = null;
            Name = name;
            Content = new ConfigurationItemContent();
            InnerItems = new List<IConfigurationItem>();
            Parent = null;
        }

        public static IConfigurationItem FromWrapper(IConfigurationItemWrapper wrapper)
        {
            return wrapper.UnWrap();
        }

        public IPluginConfiguration PluginConfiguration { get;  set; }
        public IConfigurationItem Parent { get;  set; }
        public ICollection<IConfigurationItem> InnerItems { get;  set; }
        public ConfigurationItemContent Content { get; set; }
        public string Name { get;  set; }

        public string Uri
        {
            get
            {
                try
                {
                    if (PluginConfiguration == null)
                        return Name + "/";
                    var segments = new List<string>();

                    IConfigurationItem currentItem = this;

                    while (currentItem != null)
                    {
                        segments.Add(currentItem.Name);
                        currentItem = currentItem.Parent;
                    }

                    segments.Reverse();

                    for (var i = 0; i < segments.Count; i++)
                    {
                        segments[i] += "/";
                    }

                    return String.Concat(segments);
                }
                catch (Exception e)
                {
                    return string.Empty;
                }
                
            }
        }
    }
}
