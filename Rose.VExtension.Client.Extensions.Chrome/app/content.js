chrome.extension.onRequest.addListener(
    function(request, sender, sendResponse) {
        if (request.method == "getText") {
            sendResponse({ data: document.all[0].outerHTML, method: "getText" }); 
        }
        else if (request.method == "setHtml") {
            var html = request.html;
            document.getElementsByTagName("html")[0].innerHTML = html;
            //sendResponse({ data: document.all[0].innerText, method: "setHtml" });
        }
        else if (request.method == "setNodeHtml") {
            var node = request.node;
            var nodeHtml = request.html;
            document.getElementById(node).innerHTML = nodeHtml;
            console.groupEnd();
            sendResponse({ data: document.all[0].innerText, method: "getText" });
        }
        else if (request.method == "getHtml") {
            console.groupEnd();
            sendResponse(document.getElementsByTagName("html").innerHTML);
        }
});