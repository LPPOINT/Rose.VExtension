namespace Rose.VExtension.PluginSystem.Runtime
{
    public interface IPluginRequestFilter
    {
        bool IsValidRequest(PluginRequest request);
    }
}
