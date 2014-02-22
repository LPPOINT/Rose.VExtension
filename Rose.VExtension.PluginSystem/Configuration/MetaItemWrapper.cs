using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public class MetaItemWrapper : IConfigurationItemWrapper
    {

        public MetaItemWrapper()
        {
            Meta = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Meta { get; private set; } 

        public void Wrap(IConfigurationItem item)
        {
            if (item.Name != "Meta")
                throw new UnexpectedConfigItemForWrappingException();

            Meta.Clear();

            var items = item.Content;

            foreach (var metaItem in items)
            {
                if (metaItem.Key.ToLower() == "add")
                {
                    Meta.Add(metaItem.Key, metaItem.Value);
                }
            }

        }

        public IConfigurationItem UnWrap()
        {
            throw new System.NotImplementedException();
        }
    }
}
