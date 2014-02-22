using Rose.VExtension.PluginSystem.Configuration;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class PlatformConfigWrapper : IConfigurationItemWrapper
    {


        public void Wrap(IConfigurationItem item)
        {
            if (item.Name != "Platform")
                throw new UnexpectedConfigItemForWrappingException();



        }

        public IConfigurationItem UnWrap()
        {
            throw new System.NotImplementedException();
        }
    }
}
