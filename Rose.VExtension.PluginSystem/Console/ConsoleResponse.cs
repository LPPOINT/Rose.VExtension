using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Rose.VExtension.PluginSystem.Console
{
    public class ConsoleResponse
    {
        public ConsoleResponse(Logger logger)
        {
            Logger = logger;
        }

        public void Test2()
        {
            
        }

        public Logger Logger { get; private set; }
    }
}
