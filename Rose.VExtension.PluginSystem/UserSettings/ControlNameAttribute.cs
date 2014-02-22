using System;

namespace Rose.VExtension.PluginSystem.UserSettings
{

    /// <summary>
    /// Задает имя контролла в xml-разметке
    /// </summary>
    public class ControlNameAttribute : Attribute
    {
        public ControlNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
