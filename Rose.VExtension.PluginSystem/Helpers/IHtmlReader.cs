using HtmlAgilityPack;
using Rose.VExtension.PluginSystem.Helpers.VKElements;

namespace Rose.VExtension.PluginSystem.Helpers
{


    public interface IHtmlReader
    {
        HtmlDocument Html { get; }

        VKSidebar ReadSidebar();

    }

    public class HtmlReader : IHtmlReader
    {
        public HtmlReader(HtmlDocument html)
        {
            Html = html;
        }

        public HtmlDocument Html { get; private set; }

        public VKSidebar ReadSidebar()
        {
            var node = Html.GetElementbyId("side_bar");
            if(node == null)
                throw new VKPageElementReadException("Невозможно разобрать навигационную панель: в заданном html-документе отсутствует элемент с идентификатором 'side_bar'");
            return new VKSidebar(node);
        }
    }
}
