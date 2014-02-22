namespace Rose.VExtension.PluginSystem.Runtime
{
    public abstract class PluginDomain
    {
        protected PluginDomain()
        {
        }

        public Plugin Plugin { get;  set; }

        public abstract PluginResponse Execute(PluginRequest request);

    }
}
