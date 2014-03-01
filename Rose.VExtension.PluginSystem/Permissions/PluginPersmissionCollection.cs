using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Permissions
{
    public class PluginPersmissionCollection : List<IPluginPermission>
    {
        public PluginPersmissionCollection()
        {
        }

        public IPluginPermission GetPermission(string permissionName)
        {
            return this.FirstOrDefault(permission => permission.Name == permissionName);
        }

        public bool ContainsPermission(string permissionName)
        {
            return this.Any(permission => permission.Name == permissionName);
        }

        public IConfigurationItem GetPermissionConfigurationItem(string permissionName)
        {
            if(!ContainsPermission(permissionName))
                return null;
            var permission = GetPermission(permissionName);
            if(permission == null)
                return null;
            return permission.Configuration;

        }

        public IDictionary<string, string> GetPermissionParameters(string permissionName)
        {
            var p = GetPermission(permissionName);
            if(p == null)
                return null;
            return p.Parameters;
        }

        public IPluginPermission this[string permissionName]
        {
            get { return GetPermission(permissionName); }
        }

        public PluginPersmissionCollection(IEnumerable<IPluginPermission> collection) : base(collection)
        {
        }

        public PluginPersmissionCollection(int capacity) : base(capacity)
        {
        }
    }
}
