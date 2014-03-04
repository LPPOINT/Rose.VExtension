using System.Collections.Generic;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace Rose.VExtension.Server.Models.Responsing
{




    /// <summary>
    /// Представляет сведения о частичном редактировании html кода страницы
    /// <remarks>
    /// Многие плагины вносят небольшие изменения в html код. Поэтому, чтобы снизить размер отправляемого
    /// клиенту ответа, создается словарь, в котором отражены все изменения html-кода. 
    /// 
    /// Например, есть следующий html-код:
    /// <code>
    /// <html>
    ///     <head>
    ///     </head>
    ///     <body>
    ///         <div id="container">This is input text</div>
    ///     </body>
    /// </html>
    /// </code>
    /// 
    /// Плагин изменяет текст в одном узле:
    /// <code>
    /// <html>
    ///     <head>
    ///     </head>
    ///     <body>
    ///         <div id="container">Another text</div> <!--Текст изменен здесь-->
    ///     </body>
    /// </html>
    /// </code>
    /// 
    /// Таким образом, словарь изменений будет содержать следующий элемент
    /// Key: html/body[id=container]
    /// Value: Another text
    /// 
    /// Теперь, на основе исходного html-документа и словаря изменений можно составить результирующий html-документ
    /// 
    /// </remarks>
    /// </summary>
    public class HtmlPartEditing
    {

        /// <summary>
        /// Представляет словарь, в котором ключ - путь до html-узла, а значение - html-код данного узла
        /// </summary>
        public IDictionary<string, string> EditedNodes { get; private set; }

        public void SerializeToXml(XElement parent)
        {
            
        }

        /// <summary>
        /// Создает словарь изменений html-документа на основе html документа до изменений и после
        /// </summary>
        /// <param name="beforeEditing">Html документ, содержащий узлы до изменения</param>
        /// <param name="afterEditing">Html документ, содержащий узлы после изменения</param>
        /// <returns></returns>
        public static HtmlPartEditing Create(HtmlDocument beforeEditing, HtmlDocument afterEditing)
        {
            return null;
        }

    }
}