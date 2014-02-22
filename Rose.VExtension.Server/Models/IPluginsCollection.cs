using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rose.VExtension.Server.Models
{

    /// <summary>
    /// Представляет коллекцию для загруженных плагинов
    /// </summary>
    public interface IPluginsCollection : ICollection<PluginSystem.Plugin>
    {
    }

    public class PluginsCollection : IPluginsCollection
    {

        public PluginsCollection()
        {
            plugins = new Collection<PluginSystem.Plugin>();
        }

        public PluginsCollection(IEnumerable<PluginSystem.Plugin> plugins)
        {
            this.plugins = new Collection<PluginSystem.Plugin>(plugins.ToList());
        }

        private readonly ICollection<PluginSystem.Plugin> plugins;

        public IEnumerator<PluginSystem.Plugin> GetEnumerator()
        {
            return plugins.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(PluginSystem.Plugin item)
        {
            plugins.Add(item);
        }

        public void Clear()
        {
            plugins.Clear();
        }

        public bool Contains(PluginSystem.Plugin item)
        {
            return plugins.Contains(item);
        }

        public void CopyTo(PluginSystem.Plugin[] array, int arrayIndex)
        {
            plugins.CopyTo(array, arrayIndex);
        }

        public bool Remove(PluginSystem.Plugin item)
        {
            return plugins.Remove(item);
        }

        public int Count { get { return plugins.Count; }}
        public bool IsReadOnly { get { return plugins.IsReadOnly; } }
    }

}
