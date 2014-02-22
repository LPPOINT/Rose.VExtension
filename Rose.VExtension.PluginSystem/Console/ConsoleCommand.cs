using System.Collections.Generic;

namespace Rose.VExtension.PluginSystem.Console
{

    /// <summary>
    /// Представляет консольную команду.
    /// Пример WriteLine "Hello, world" size:12 [red, upper]
    /// </summary>
    public class ConsoleCommand
    {
        public ConsoleCommand(string commandName, IList<string> attributes, ConsoleCommandArgumentsCollection arguments)
        {
            Arguments = arguments;
            Attributes = attributes;
            CommandName = commandName;
        }


        public string CommandName { get; private set; }
        public IList<string> Attributes { get; private set; }
        public ConsoleCommandArgumentsCollection Arguments { get; private set; }

        public static ConsoleCommand Parse(string line)
        {
            var parser = new ConsoleCommandParser();
            return parser.Parse(line);
        }

    }
}
