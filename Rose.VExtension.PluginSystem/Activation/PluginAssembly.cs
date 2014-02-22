namespace Rose.VExtension.PluginSystem.Activation
{
    public class PluginAssembly
    {

        public PluginAssembly()
        {
            
        }

        public PluginAssembly(string assemblyFileName, string assemblyString)
        {
            AssemblyFileName = assemblyFileName;
            AssemblyString = assemblyString;
        }

        public string AssemblyFileName { get; set; }
        public string AssemblyString { get; set; }
    }
}
