using System;
using System.Runtime.Serialization;

namespace Rose.VExtension.PluginSystem.Permissions
{
    public class PermissionException : Exception
    {

        public string PermissionName { get; private set; }

        public PermissionException(string permissionName)
        {
            PermissionName = permissionName;
        }

        public PermissionException(string message, string permissionName) : base(message)
        {
            PermissionName = permissionName;
        }

        public PermissionException(string message, Exception innerException, string permissionName) : base(message, innerException)
        {
            PermissionName = permissionName;
        }

        protected PermissionException(SerializationInfo info, StreamingContext context, string permissionName) : base(info, context)
        {
            PermissionName = permissionName;
        }
    }

}
