using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rose.VExtension.PluginSystem.Activation
{
    public enum ActivationStepName
    {
        NameActivation,
        VersionActivation,
        ControllerActivation,
        PermissionsActivation,
        MetaActivation,
        StorageActivation,
        ReservationActivation,
        SettingsActivation,
        ConfigurationActivation,
        FileSystemActivation,
        PackageActivation
    }
}
