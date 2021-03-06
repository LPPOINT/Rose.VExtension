﻿
var authTabId = undefined;
var isAuthTabDidOpened = false;
var activeVkTabId = undefined;


function VKTabDetected(tab) {

    chrome.storage.sync.get('access_info', function (result) {

        console.groupCollapsed("Получение данных из хранилища");
        console.log("Полученные данные по запросу 'access_info':");
        console.log(result);
        console.groupEnd();

        if (result == {} || result == [] || result == undefined || isEmptyObject(result)) {
            //if (isAuthTabDidOpened)
            //    return;
            //isAuthTabDidOpened = true;
            getAccessToken(function (accessInfo) {

                var accessInfoResult = accessInfo.access_info;

                getTabHtml(tab.id, function (html) {
                    sendRequest(accessInfoResult['access_token'], accessInfoResult['user_id'], "HomePage", html);
                });
            });

        } else {

            getTabHtml(tab.id, function (html) {
                sendRequest(result.access_info['access_token'], result.access_info['user_id'], "HomePage", html);
            });
        }
    });
}

chrome.tabs.onUpdated.addListener(function (id, info) {
    if (info.url == undefined || info.url.indexOf("vk.com") == -1 || info.url.indexOf("oauth") != -1)
        return;

    activeVkTabId = id;
    chrome.tabs.get(id, function (tab) {
        VKTabDetected(tab);
    });

});

chrome.tabs.onCreated.addListener(function (tab) {
    if (tab.url == undefined || tab.url.indexOf("vk.com") == -1 || tab.url.indexOf("oauth") != -1)
        return;
    activeVkTabId = tab.id;
    VKTabDetected(tab);
});

chrome.tabs.onReplaced.addListener(function (newTabId, oldTabId) {
    chrome.tabs.get(newTabId, function (tab) {
        if (tab.url == undefined || tab.url.indexOf("vk.com") == -1 || tab.url.indexOf("oauth") != -1)
            return;
        activeVkTabId = tab.id;
        VKTabDetected(tab);
    });
});

chrome.tabs.onActivated.addListener(function (info) {
});

function getTabHtml(tabId, callback) {
    chrome.tabs.getSelected(null, function (tab) {
        chrome.tabs.sendRequest(tab.id, { method: "getText" }, function (response) {
            if (response == undefined || response.method == undefined) {
                callback("undefined html");
            }
            else if (response.method == "getText") {
                callback(response.data);
            }
        });
    });
}

function getAccessInfo(callback) {
    chrome.storage.sync.get("access_info", function (result) {
        callback(result);
    });
}

function getUrlParameterValue(url, parameterName) {

}

function getAccessToken(callback) {

}

function writeAccessToken() {
    chrome.storage.sync.get('access_token', function (token) {
        console.log(token);
    });
}

function setTabHtml(tabId, html) {

}

function createRequestUri(currentTab, accessTinfo) {

}

function setTabHtmlNode(tabId, nodeId, html) {
    chrome.tabs.get(activeVkTabId, function (tab) {
        chrome.tabs.sendRequest(tab.id, { method: "setNodeHtml", html: html, node: nodeId }, function (response) {
        });
    });
}

function parseRespose(response) {

    console.groupCollapsed("Получен ответ от сервера");
    console.log(response);
    console.groupEnd();

    setTabHtmlNode(activeVkTabId, "wrap1", response);
}

function sendRequest(access_token, id, requestString, html, exp) {

    console.groupCollapsed("Посылка запроса");
    console.log("access_token: " + access_token);
    console.log("id: " + id);
    console.log("requestString: " + requestString);
    console.log("html: " + html);
    console.log("exp: " + exp);
    console.groupEnd();

    var xmlhttp = getXmlHttp();
    var params = "UserToken=" + access_token + "&RequestString=" + requestString + "&UserId" + id + "&Html" + html;
    console.log(params);
    xmlhttp.open('POST', 'http://localhost:19945/query', true);
    xmlhttp.setRequestHeader("Content-type", "text/html");
    //xmlhttp.setRequestHeader("Content-length", params.length);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            if (xmlhttp.status == 200) {;
                var data = xmlhttp.responseXML;
                console.log(data);
                parseRespose(data);
            }
        }
    };
    xmlhttp.send(params);

}