using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.UserSettings
{


    public class UserSettingsCollection : List<ISettingsControl>
    {
        public ISettingsControl GetControlById(string id)
        {
            return (from control in this where control.Id == id select control).FirstOrDefault();

        }

        public T GetControlById<T>(string id) where T : class, ISettingsControl
        {
            var control = GetControlById(id);
            if (control == null)
                return default(T);
            return control as T;
        }

        public T GetControlValue<T>(string controlId) where T : class
        {
            var control = GetControlById(controlId);
            if (control == null && !(control is IValueableSettingsControl<T>))
                return default(T);

            var valueable = control as IValueableSettingsControl<T>;

            return valueable.Value;
        }

        public string GetControlTitle(string controlId)
        {
            var control = GetControlById(controlId);
            if (control == null && !(control is ITitleableSettingsControl))
                return string.Empty;

            var titleable = control as ITitleableSettingsControl;

            return titleable.Title;
        }

        public ISettingsControl this[string id]
        {
            get { return GetControlById(id); }
        }

    }
}
