
/// Содержит вспомогательные методы
var Utils = function () { };

Utils.getUrlParameterValue = function (url, parameterName) {
    "use strict";

    var urlParameters = url.substr(url.indexOf("#") + 1),
        parameterValue = "",
        index,
        temp;

    urlParameters = urlParameters.split("&");

    for (index = 0; index < urlParameters.length; index += 1) {
        temp = urlParameters[index].split("=");

        if (temp[0] === parameterName) {
            return temp[1];
        }
    }

    return parameterValue;
};
Utils.getHtmlHttp = function () {
    var xmlhttp;
    try {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (E) {
            xmlhttp = false;
        }
    }
    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    }
    return xmlhttp;
};
Utils.isEmptyObject = function (obj) {
    for (var i in obj) {
        return false;
    }
    return true;
};
Utils.getUrlDomain = function (url) {
    var a = document.createElement('a');
    a.href = url;
    return a.hostname;
};
Utils.getIdByUrl = function(url) {

};

/// Содержит методы для взаимодействия с локальным хранилищем
var LocalStorage = function () { };


LocalStorage.getAccessToken = function (callback) {

    chrome.storage.sync.get("access_info", function (object) {
        debugger;
        if (object == undefined || Utils.isEmptyObject(object)) {
            var authUrl = "https://oauth.vk.com/authorize?client_id=4126850&scope=docs,offline&redirect_uri=http://oauth.vk.com/blank.html&display=page&response_type=token";
            chrome.tabs.create({ url: authUrl, selected: true }, function(tab) {
                var authTabId = tab.id;
                chrome.tabs.onUpdated.addListener(function(tabId, info) {
                    if (tabId == authTabId && info.url != undefined && info.status == "loading") {

                        var vkAccessToken = Utils.getUrlParameterValue(info.url, 'access_token');
                        var expiries = Utils.getUrlParameterValue(info.url, 'expires_in');
                        var userId = Utils.getUrlParameterValue(info.url, 'user_id');

                        var authSave = {};

                        authSave['access_token'] = vkAccessToken;
                        authSave['user_id'] = userId;
                        authSave['expiries_in'] = expiries;
                        authSave['access_info'] = {};

                        authSave['access_info']['access_token'] = vkAccessToken;
                        authSave['access_info']['user_id'] = userId;
                        authSave['access_info']['expiries_in'] = expiries;
                        chrome.storage.sync.set(authSave, function() {
                            chrome.tabs.remove(tabId);
                            console.groupEnd();

                            if (callback != undefined) {
                                callback(authSave);
                            }

                        });
                    }
                });
            });
        } else {
            callback(object.access_info);
        }
        return null;
    });
};

var VKPageType = function () { };

VKPageType.MyPage = 1;
VKPageType.Page = 2;
VKPageType.MyFrends = 3;
VKPageType.MyPhotos = 4;
VKPageType.MyVideos = 5;
VKPageType.MyAudios = 6;
VKPageType.Messages = 7;
VKPageType.MyGroups = 8;
VKPageType.News = 9;
VKPageType.Settings = 10;
VKPageType.Other = 11;
VKPageType.Undefined = undefined;

/// Содержит методы для разбора данных страницы ВКонтакте
var VKPage = function (tabId) {
    this.tabId = tabId;

    this.isVKPage = function (callback) {
        TabInteraction.getTabUrl(tabId, function (url) {
            var result = Utils.getUrlDomain(url) == "vk.com";
            callback(result);
        });
    };

};

var VKPageHandler = function () { };

VKPageHandler.onPageOpenned = function (callback) {
    chrome.tabs.onCreated.addListener(function (tab) {
        var page = new VKPage(tab.id);
        page.isVKPage(function (isVkPage) {
            if (isVkPage) {
                callback(tab.id, null);
            }
        });
    });
    chrome.tabs.onUpdated.addListener(function (tabId, changeInfo, tab) {
        if (changeInfo.status == "complete") {

            var page = new VKPage(tab.id);
            page.isVKPage(function(isVkPage) {
                if (isVkPage) {
                    callback(tab.id, null);
                }
            });
        }
    });
};
VKPageHandler.onPageClosed = function (callback) {

};
VKPageHandler.onPageUpdated = function (callback) {
    chrome.tabs.onUpdated.addListener(function (id, info, tab) {
        var page = new VKPage(id);
        if (page.isVKPage()) {
            callback(id, info, tab);
        }
    });
};

/// Содержит методы для формирования запроса к серверу от страницы ВКонтакте
var ServerRequestBuilder = function (vkPage) {
    this.page = vkPage;
    var localPage = this.page;
    this.buildRequest = function (callback) {

        TabInteraction.getTabUrl(this.page.tabId, function (url) {
            debugger;
            TabInteraction.getTabHtml(localPage.tabId, function (html) {
                debugger;
                LocalStorage.getAccessToken(function (accessToken) {
                    debugger;
                    var buffer = "<request><Url>" + url + "</Url><Html></Html><UserId>" + accessToken.user_id + "</UserId><AccessToken>" +accessToken.access_token+ "</AccessToken></request>";
                    var parser = new DOMParser();
                    var xmlDoc = parser.parseFromString(buffer, "text/xml");
                    var htmlNode = xmlDoc.getElementsByTagName("Html")[0];
                    htmlNode.textContent = html;
                    callback(xmlDoc);
                });

            });
        });
    };

};

/// Содержит методы для взаимодействия с вкладками
var TabInteraction = function () { };

TabInteraction.getTab = function (tabId, callback) {
    chrome.tabs.get(tabId, callback);
};
TabInteraction.setTabHtml = function (tabId, html) {
    chrome.tabs.get(tabId, function (tab) {
        chrome.tabs.sendRequest(tab.id, { method: "setHtml", html: html }, function (response) {
        });
    });
};
TabInteraction.getTabHtml = function (tabId, callback) {
    chrome.tabs.get(tabId, function (tab) {
        chrome.tabs.sendRequest(tab.id, { method: "getText" }, function (response) {
            if (response == undefined || response.method == undefined) {
                callback("undefined html");
            } else if (response.method == "getText") {
                callback(response.data);
            }
        });
    });
};
TabInteraction.getTabUrl = function (tabId, callback) {
    chrome.tabs.get(tabId, function (tab) {
        callback(tab.url);
    });
};

/// Содержит методы для взаимодействия с сервером
var ServerInteraction = function (serverUrl) {
    this.serverUrl = serverUrl;
    this.timeout = 60 * 10;

    this.query = function (request, callback) {
        debugger;
        var text = request.childNodes[0].outerHTML;
        debugger;
        try {

            console.log("Идет отправка запроса на сервер");

            var xmlhttp = Utils.getHtmlHttp();
            xmlhttp.open('POST', 'http://localhost:19945/query', true);
            xmlhttp.setRequestHeader("Content-type", "text/html");
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    if (xmlhttp.status == 200) {
                        debugger;
                        var data = xmlhttp.responseXML;
                        callback(data);
                    }
                }
            };
            xmlhttp.send(text);
        } catch (e) {
            console.log(e);
            callback(undefined);
        }

    };

};

/// Содержит методы для внедрения ответа от сервера в страницу ВКонтакте
var ServerAnswerParser = function (tabId, answer) {
    this.tabId = tabId;
    this.answer = answer;

    this.isErrorAnswer = function () {
        return false;
    };
    this.isEmptyAnswer = function () {
        return false;
    };

    this.injectAnswer = function () {
        debugger;
        var codeNode = this.answer.getElementsByTagName("code")[0];
        debugger;
        if (codeNode.innerHTML == "1") { // Html
            debugger;
            var htmlNode = this.answer.getElementsByTagName("html")[0];
            debugger;
            var htmlNodeHtml = htmlNode.textContent;
            debugger;
            TabInteraction.setTabHtml(this.tabId, htmlNodeHtml);
            debugger;
        }
        debugger;
    };

};

/// Содержит методы управления клиентским расширением
var VExtensionClient = function () {

    this.UpdatePage = function (tabId) {
        var page = new VKPage(tabId);
        var requestBuilder = new ServerRequestBuilder(page);

        requestBuilder.buildRequest(function (request) {
            console.log("Сформирован запрос: " + request);
            var serverInteraction = new ServerInteraction("http://localhost:19945/");
            debugger;
            serverInteraction.query(request, function (response) {
                debugger;
                var parser = new ServerAnswerParser(tabId, response);
                parser.injectAnswer();


            });
        });

    };

};


VKPageHandler.onPageOpenned(function (tabId, info) {
    var client = new VExtensionClient();
    client.UpdatePage(tabId);
});