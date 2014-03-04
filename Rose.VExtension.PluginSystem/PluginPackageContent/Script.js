
//Входня функция. Её имя должно быть объявлено в узле конфигурации активити в поле EntryFunction
function hello() {
    // args - позволяет получать объекты, определенные в узле Args кофигурации выполняемого обработчика
    // Аргумент 'Html' метода GetRequestArgument обозначает, что требуется получить html-документ открытой пользователем страницы
    var htmlDocument = args.GetRequestArgumentValue("Html");

    var titleHeader = htmlDocument.GetElementbyId("title");
    titleHeader.InnerHtml = "VExtension";

    // Чтобы изменения вступили в силу, их необходимо зарегестрировать в ответе
    response.Html(htmlDocument);
}