using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Configuration
{
    public class ConfigurationItemContent : List<KeyValuePair<string, string>>, IDictionary<string, string>
    {
        public bool ContainsKey(string key)
        {
            return this.Any(pair => pair.Key == key);
        }

        public void Add(string key, string value)
        {
            Add(new KeyValuePair<string, string>(key, value));
        }

        public bool Remove(string key)
        {
            var pair = this.FirstOrDefault(valuePair => valuePair.Key == key);
            return Remove(pair);
        }

        public bool TryGetValue(string key, out string value)
        {
            value = string.Empty;
            if(!ContainsKey(key))
                return false;
            value = this[key];
            return true;
        }

        public string this[string key]
        {
            get { return this.FirstOrDefault(pair => pair.Key == key).Value; }
            set
            {
                Add(key, value);
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                var query = from pair in this select pair.Key;
                return query.ToList();
            }
        }

        public ICollection<string> Values
        {
            get
            {
                var query = from pair in this select pair.Value;
                return query.ToList();
            }
        }

        public ConfigurationItemContent()
        {
        }

        public ConfigurationItemContent(int capacity) : base(capacity)
        {
        }

        public ConfigurationItemContent(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }
    }
}
