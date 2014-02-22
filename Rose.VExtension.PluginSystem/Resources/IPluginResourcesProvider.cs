using System.IO;

namespace Rose.VExtension.PluginSystem.Resources
{
    public interface IPluginResourcesProvider
    {
        Stream GetResourceStream(string resName);
        bool ContainsResource(string resName);
        int ResourcesStorageSize { get; }
        int MaxResourcesStorageSize { get; }
        PluginResourceAccessToken ConfirmAccessToken(PluginResourceAccessToken accessToken);

    }

    public class PluginResourcesProvider : IPluginResourcesProvider
    {

        public IPluginResourcesProvider Middleware { get; set; }

        public PluginResourcesProvider(IPluginResourcesProvider middleware)
        {
            Middleware = middleware;
        }

        public PluginResourcesProvider()
        {
            
        }

        public Stream GetResourceStream(string resName)
        {
            return Middleware.GetResourceStream(resName);
        }

        public bool ContainsResource(string resName)
        {
            return Middleware.ContainsResource(resName);
        }


        public int ResourcesStorageSize
        {
            get { return Middleware.ResourcesStorageSize; }
        }
        public int MaxResourcesStorageSize { get { return Middleware.MaxResourcesStorageSize; } }
        public PluginResourceAccessToken ConfirmAccessToken(PluginResourceAccessToken accessToken)
        {
            return Middleware.ConfirmAccessToken(accessToken);
        }
    }

}
