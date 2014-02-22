using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rose.VExtension.PluginSystem.Console
{
    public class ConsoleCommandArgumentsCollection : List<ConsoleCommandArgument>
    {
        public ConsoleCommandArgumentsCollection()
        {
        }

        public ConsoleCommandArgumentsCollection(int capacity) : base(capacity)
        {
        }

        public ConsoleCommandArgumentsCollection(IEnumerable<ConsoleCommandArgument> collection) : base(collection)
        {
        }

        public IEnumerable<ConsoleCommandArgument> NamedArguments
        {
            get { return this.Where(argument => argument.IsNamed); }
        }
        public IEnumerable<ConsoleCommandArgument> UnnamedArguments
        {
            get
            {
                return this.Where(argument => !argument.IsNamed);
            }
        }

        public ConsoleCommandArgument GetNamedArgument(string name)
        {
            return this.FirstOrDefault(argument => argument.Name == name);
        }

        public ConsoleCommandArgument this[string name]
        {
            get { return GetNamedArgument(name); }
        }


    }
}
