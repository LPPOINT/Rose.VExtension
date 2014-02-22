namespace Rose.VExtension.PluginSystem.Console
{
    public class ConsoleCommandArgument
    {
        public ConsoleCommandArgument(string name, string value)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }

        public bool IsNamed
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }
    }
}
