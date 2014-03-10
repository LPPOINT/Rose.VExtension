namespace Rose.VExtension.PluginSystem.Helpers.VKElements
{
    public partial class VKSidebarTemplate
    {

        public string Name { get; private set; }
        public string Reference { get; private set; }

        public VKSidebarTemplate(string name, string reference)
        {
            Name = name;
            Reference = reference;
        }
    }
}
