function fn() { }
nts.web9000.JsLibrary.login = fn.prototype = {
    name: '系统登录处理js类',
    Login: function () {
        var v1 = nts.web9000.JsLibrary.validator.binds("", { id: "userid", rule: "empty|lenrage", leninfo: "0,50" });
        var v2 = nts.web9000.JsLibrary.validator.binds("", { id: "userpass", rule: "empty|lenrage", leninfo: "0,50" });
        if (v1 && v2) {
            var issavepass = $("#savepass")[0].checked == true ? 1 : 0;
            NTS.NTSAjax(AjaxUrl.LoginServer, { username: $("#userid").val(), password: $("#userpass").val(), issavepass: issavepass }, function (data) {
                if (data.success) {
                    window.opener = null;
                    var targeturl = "system.aspx";
                    window.open('', '_self', '');
                    window.close();
                    var newwin = window.open("", "", "scrollbars");
                    if (document.all) {
                    newwin.moveTo(0, 0);
                    newwin.resizeTo(screen.width, screen.height);
                    }
                    newwin.location = targeturl;
                } else {
                    alert(data.msg);
                    return;
                }
            });
        }
    }, RemeberMyPass: function () {
        NTS.NTSAjax(AjaxUrl.RemeberPass, "", function (data) {
            if (data.success) {
                $("#userid").val(data.userid);
                $("#userpass").val(data.userpass);
                $("#savepass")[0].checked = true;
            }
        });
    }
}
$(function () {
    //注册按钮的单击事件
    var obj = nts.web9000.JsLibrary.login;
    $("#btnLogin").click(function (e) { obj.Login(); });
    obj.RemeberMyPass();
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {
            $("#btnLogin").click();
        }
    }
});

function Fkey() {
    var WsShell = new ActiveXObject('WScript.Shell')
    WsShell.SendKeys('{F11}');
}

function fullscreen(targeturl) {
    //var targeturl = ""
    newwin = window.open("", "_self", "scrollbars");
    if (document.all) {
        newwin.moveTo(0, 0)
        newwin.resizeTo(screen.width, screen.height)
    }
    newwin.location = targeturl;
}

function closeme() {
    var browserName = navigator.appName;
    if (browserName == "Netscape") {
        window.open('', '_self', '');
        window.close();
    }
    else {
        if (browserName == "Microsoft Internet Explorer") {
            window.opener = "whocares";
            window.opener = null;
            window.open('', '_top');
            window.close();
        }
    } 
} 




