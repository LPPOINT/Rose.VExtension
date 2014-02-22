chrome.extension.onRequest.addListener(
    function (request, sender, sendResponse) {

        console.groupCollapsed("Обработка запроса от расширения");

        console.log(request);
        console.log(sender);
        console.log(sendResponse);


        if (request.method == "setHtml") {
            var html = request.html;
            document.getElementByTagName("html").outerHTML = html;
            console.groupEnd();
            sendResponse({ data: document.all[0].innerText, method: "getText" }); 
        }
        else if (request.method == "setNodeHtml") {
            var node = request.node;
            var nodeHtml = request.html;
            document.getElementById(node).innerHTML = nodeHtml;
            console.groupEnd();
            sendResponse({ data: document.all[0].innerText, method: "getText" }); 
        }
        else if (request.method == "getHtml") {
            console.log("Обнаружен запрос на получение HTML");
            console.groupEnd();
            sendResponse(document.getElementsByTagName("html").innerHTML);
        }
    }
);


chrome.extension.onRequest.addListener(
    function(request, sender, sendResponse) {
        if (request.method == "getText") {
            sendResponse({ data: document.all[0].outerHTML, method: "getText" }); 
        }
});