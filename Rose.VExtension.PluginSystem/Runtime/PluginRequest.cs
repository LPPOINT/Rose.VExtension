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
        string Url { get; }
        IEnumerable<string> Attributes { get;  }
        IDictionary<string, string> Meta { get; }
        string UserId { get;  }
        string ExtensionId { get; }
    }

    public class PluginRequest : IPluginRequest
    {
        public string RequestString { get; set; }
        public HtmlDocument Html { get; set; }
        public string Url { get; set; }
        public IDictionary<string, string> Meta { get; set; }
        public string UserId { get; set; }
        public string ExtensionId { get; set; }
        public IEnumerable<string> Attributes { get; set; } 


    }
}
