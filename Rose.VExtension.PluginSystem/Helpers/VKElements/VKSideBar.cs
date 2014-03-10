using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Common;

namespace Rose.VExtension.PluginSystem.Helpers.VKElements
{
    public class VKSidebar : VKPageElement
    {
        public VKSidebar(HtmlNode node) : base(node)
        {
        }


        public IEnumerable<HtmlNode> ElementsNodes
        {
            get
            {
                try
                {
                    var sidebarContainer = Node.ChildNodes.FirstOrDefault(node => node.Name == "ol");
                    return sidebarContainer.ChildNodes.Where(node => node.Name == "li");
                }
                catch (Exception e)
                {
                    throw new VKPageElementReadException("Невозможно разобрать узлы навигационной панели.", e);
                }
            }
        }

        public IEnumerable<string> ElementsNames
        {
            get
            {
                try
                {
                    return from elementsNode in ElementsNodes
                        select
                            GetNodeName(elementsNode);
                }
                catch (Exception e)
                {
                    throw new VKPageElementReadException("Невозможно разобрать имена узлов навигационной панели.", e);
                }

            }
        }

        public HtmlNode GetNode(int index)
        {
            try
            {
                return ElementsNodes.ToList()[index];
            }
            catch 
            {
                return null;
            }
        }

        public string GetNodeName(HtmlNode node)
        {
            try
            {
                return
                    node.FirstChild.ChildNodes.FirstOrDefault(
                        htmlNode => htmlNode.GetAttributeValue("class", "none") == "left_label inl_bl").InnerText;
            }
            catch 
            {
                return string.Empty;
            }
        }

        public string GetNodeLink(HtmlNode node)
        {
            try
            {
                return node.FirstChild.GetAttributeValue("href", string.Empty);
            }
            catch 
            {
                return string.Empty;
            }
        }

        public HtmlNode GetNode(string nodeName)
        {
            try
            {
                return ElementsNodes.FirstOrDefault(node => GetNodeName(node) == nodeName);
            }
            catch
            {
                return null;
                
            }
        }

        public static HtmlNode CreateSidebarNode(string nodeName, string nodeRef)
        {
            try
            {
                var template = new VKSidebarTemplate(nodeName, nodeRef);
                return HtmlNode.CreateNode(template.TransformText());
            }
            catch (Exception e)
            {
                throw new VKPageElementException("Невозможно выполнить трансформацию html-кода навигационной панели в html-узел.Подробнее в InnerException", e);
            }
        }

        public void AppendNode(int index, string name, string uri)
        {
            try
            {
                var node = CreateSidebarNode(name, uri);
                var except = Node.ChildNodes.FindFirst("ol").ChildNodes.Count(htmlNode => htmlNode.Name != "li");
                var fixedIndex = index - except;
                if (fixedIndex > 1)
                {
                    var before = Node.ChildNodes.FindFirst("ol").ChildNodes[fixedIndex - 1];
                    Node.ChildNodes.FindFirst("ol").InsertAfter(before, node);
                }
                else
                {
                    Node.ChildNodes.FindFirst("ol").AppendChild(node);
                }
            }
            catch (Exception e)
            {
                throw new VKPageElementWriteException("Невозможно внедрить навигационный узел", e);
            }
        }

        public void AppendNodeToEnd(string name, string url)
        {
            AppendNode(ElementsNodes.Count() - 1, name, url);
        }

        public void AppendNodeToBegin(string name, string url)
        {
            AppendNode(0, name, url);
        }

    }
}
