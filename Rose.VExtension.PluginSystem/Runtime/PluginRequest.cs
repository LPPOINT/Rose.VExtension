using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Rose.VExtension.PluginSystem.Runtime
{

    public interface IPluginRequest
    {
        string RequestString { get;  }
        IDictionary<string, string> Headers { get; }

    }

    public class PluginRequest : IPluginRequest
    {
        public string RequestString { get; set; }
        public IDictionary<string, string> Headers { get; set; }

    }
}
