function hello() {
    log.Info("Начало выполнения скрипта плагина " + plugin.Id);
    var html = response.Html;
    var node = html.CreateElement("HelloWorld");
    html.DocumentNode.AppendChild(node);
}