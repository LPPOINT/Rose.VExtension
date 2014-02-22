using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rose.VExtension.PluginSystem.Console
{

    internal class ConsoleCommandParsedNode<T>
    {
        public ConsoleCommandParsedNode(T parsedValue, int offset)
        {
            Offset = offset;
            ParsedValue = parsedValue;
        }

        public T ParsedValue { get; private set; }
        public int Offset { get; private set; }
    }

    internal class NameParsedNode : ConsoleCommandParsedNode<string>
    {
        public NameParsedNode(string parsedValue, int offset) : base(parsedValue, offset)
        {
        }
    }

    internal class ArgumentsParsedNode : ConsoleCommandParsedNode<ConsoleCommandArgumentsCollection>
    {
        public ArgumentsParsedNode(ConsoleCommandArgumentsCollection parsedValue, int offset) : base(parsedValue, offset)
        {
        }
    }

    internal class AttributesParsedNode : ConsoleCommandParsedNode<IList<string>>
    {
        public AttributesParsedNode(IList<string> parsedValue, int offset) : base(parsedValue, offset)
        {
        }
    }


    public class CommandParseException : Exception
    {
        public CommandParseException()
        {
        }

        public CommandParseException(string message) : base(message)
        {
        }

        public CommandParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class ConsoleCommandParser : IConsoleCommandParser
    {



        private NameParsedNode GetName(string str)
        {
            var res = string.Empty;
            for (var i = 0; i < str.Length; i++)
            {
                var ch = res[i];
                if (!char.IsLetterOrDigit(ch))
                {
                    return new NameParsedNode(res, i);
                }
                res += ch;
            }

            return new NameParsedNode(str, str.Length);

        }

        private ConsoleCommandArgument ParseArgument(string argumentString)
        {
            return null;
        }

        private ArgumentsParsedNode GetArguments(string str)
        {
            return null;
        }

        private AttributesParsedNode GetAttributes(string str)
        {
            str = str.Trim();

            if(str.First() != '[' && str.Last() != ']')
                throw new CommandParseException();

            var attribsStr = str.Substring(1, str.Length - 2);
            var attribsArray = attribsStr.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

            return new AttributesParsedNode(attribsArray, str.Length);


        }

        public ConsoleCommand Parse(string str)
        {

            str = str.Trim();

            var nameNode = GetName(str);
            //str = str.Remove(0, nameNode.Offset);
            //var args = GetArguments(str);
            //str = str.Remove(0, args.Offset);
            //var attribs = GetAttributes(str);

            return new ConsoleCommand(nameNode.ParsedValue, null, null);

        }
    }
}
