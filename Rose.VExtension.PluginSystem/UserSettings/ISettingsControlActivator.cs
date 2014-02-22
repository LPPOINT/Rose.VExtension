using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rose.VExtension.PluginSystem.UserSettings
{
    public interface ISettingsControlActivator
    {
        ISettingsControl Activate(Type control, params object[] args);
        ISettingsControl Activate(XElement xmlElement);
        ISettingsControl Activate(Type control, IDictionary<string, object> args);
    }

    public class SettingsControlActivator : ISettingsControlActivator
    {
        public ISettingsControl Activate(Type control, params object[] args)
        {
            return Activator.CreateInstance(control, args) as ISettingsControl;
        }

        private IEnumerable<Type> GetAllControlsTypes(IEnumerable<Assembly> assemblies)
        {
            var res = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.GetInterfaces().Contains(typeof(ISettingsControl)))
                        res.Add(type);
                }

            }

            return res;
        }

        private IEnumerable<Type> GetAllControlsTypes()
        {
            return GetAllControlsTypes(new List<Assembly> {Assembly.GetExecutingAssembly()});
        }

        public ISettingsControl Activate(XElement xmlElement)
        {

            var controls = GetAllControlsTypes();

            var controlType = (from control in controls
                where control.GetCustomAttribute<ControlNameAttribute>() != null
                      && control.GetCustomAttribute<ControlNameAttribute>().Name == xmlElement.Name.ToString()
                select control).FirstOrDefault()
                    ?? Type.GetType(xmlElement.Name.ToString());

            if(controlType == null)
                return null;

            xmlElement.Name = controlType.Name;

            var xmlDes = new XmlSerializer(controlType);

            return xmlDes.Deserialize(xmlElement.CreateReader()) as ISettingsControl;

        }

        public ISettingsControl Activate(Type controlType, IDictionary<string, object> args)
        {
            try
            {
                var control = Activator.CreateInstance(controlType) as ISettingsControl;

                var pramsQuery = from prop in controlType.GetProperties(BindingFlags.Public)
                    where args.ContainsKey(prop.Name)
                    select prop;


                var prams = pramsQuery.ToList();

                foreach (var propertyInfo in prams)
                {
                    propertyInfo.SetValue(control, args[propertyInfo.Name]);
                }

                return control;
            }
            catch 
            {
                return null;
            }
        }
    }

}
