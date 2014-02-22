using System.Collections;
using System.Collections.Generic;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Activation
{
    /// <summary>
    /// Обертка элемента конфигурации для сборок
    /// </summary>
    public class AssembliesConfigItemWrapper : IConfigurationItemWrapper, IEnumerable<PluginAssembly>
    {
        private List<PluginAssembly> assemblies; 

        public void Wrap(IConfigurationItem item)
        {
            if(item.Name.ToLower() != "assemblies")
                throw new UnexpectedConfigItemForWrappingException();
            assemblies = new List<PluginAssembly>();

            var inner = item.InnerItems;

            foreach (var configurationItem in inner)
            {
                if (configurationItem.Name.ToLower() == "assembly")
                {
                    var name = configurationItem.Content["Name"];
                    var config = configurationItem.Content["String"];

                    assemblies.Add(new PluginAssembly(name, config));

                }
            }

        }

        public IConfigurationItem UnWrap()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<PluginAssembly> GetEnumerator()
        {
            return assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
