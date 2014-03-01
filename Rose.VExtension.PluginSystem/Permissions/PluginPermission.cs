using System;
using System.Collections.Generic;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Permissions
{
    public class PluginPermission : IPluginPermission, IConfigurationItemWrapper
    {
        public PluginPermission(string name, IConfigurationItem configuration)
        {
            Configuration = configuration;
            Name = name;
            Parameters = configuration != null ? CreateParamsFrom(configuration, string.Empty) : new Dictionary<string, string>();

        }

        public PluginPermission(string name) : this(name, null)
        {
            
        }

        public PluginPermission()
        {
            
        }

        private IConfigurationItem configuration;

        public string Name { get;  set; }

        public IConfigurationItem Configuration
        {
            get { return configuration; }
            set { configuration = value;
                Parameters = CreateParamsFrom(configuration, string.Empty);
            }
        }

        private IDictionary<string, string> CreateParamsFrom(IConfigurationItem item, string path)
        {

            if(item == null)
                return new Dictionary<string, string>();

            if (path != string.Empty && !path.EndsWith("."))
                path = path += ".";

            var res = new Dictionary<string, string>();

            foreach (var c in item.Content)
            {
                res.Add(path + c.Key, c.Value);
            }

            foreach (var inner in item.InnerItems)
            {
                var innerPath = path + inner.Name;
                var innerRes = CreateParamsFrom(inner, innerPath);
                foreach (var innerItem in innerRes)
                {
                    res.Add(innerItem.Key, innerItem.Value);
                }
            }

            return res;
        }

        public IDictionary<string, string> Parameters { get; private set; }

        public void Wrap(IConfigurationItem item)
        {
            if(item.Name != "Permission")
                throw new UnexpectedConfigItemForWrappingException("Узел конфигурации типа 'Разрешение' должен иметь имя Permission");
            Configuration = item;
            try
            {
                Name = item.GetContentValue("name");
            }
            catch (Exception e)
            {
                throw new UnexpectedConfigItemForWrappingException("У узла конфигурации типа 'Разрешение' отсутствует обязательное поле Name", e);
            }

            try
            {
                Parameters = CreateParamsFrom(Configuration, string.Empty);
            }
            catch (Exception e)
            {
                throw new UnexpectedConfigItemForWrappingException("Невозможно составить таблицу параметров на основе заданной конфигурации. Подробнее в InnerException", e);
            }
        }

        public IConfigurationItem UnWrap()
        {
            throw new System.NotImplementedException();
        }
    }
}