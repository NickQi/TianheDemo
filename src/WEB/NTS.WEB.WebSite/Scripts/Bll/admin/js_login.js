function Sys_Login() { }
Sys_Login.prototype = {
    name: '系统登录处理js类',
    Login: function () {
        if ($("#userid").val() == "") {
            this.AlertMsg('请输入您的帐号信息');
            $("#userid").focus();
            return false;
        }
        if ($("#userpass").val() == "") {
            this.AlertMsg('请输入您的密码信息');
            $("#userpass").focus();
            return false;
        }

        if ($("#userid").val().length > 50) {
            this.AlertMsg('帐号信息长度不能超过50个字符');
            $("#userid").focus();
            return false;
        }

        if ($("#userpass").val().length > 50) {
            this.AlertMsg('密码信息长度不能超过50个字符');
            $("#userpass").focus();
            return false;
        }

        var issavepass = $("#savepass")[0].checked == true ? 1 : 0;
        //alert(issavepass);
        /*系统登录处理*/
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.LoginAuthen&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: { username: $("#userid").val(), password: $("#userpass").val(), issavepass: issavepass },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval('data=' + data);
                if (data.success) {
                    alert('xxxxx');
                    return;
                    fullscreen("system.aspx");

                    //window.location.href = "system.aspx";
                } else {

                    new Sys_Login().AlertMsg(data.msg);
                    return;
                }
            }
        });
    }, AlertMsg: function (message) {
        alert(message);
    }, RemeberMyPass: function () {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.RemeberMyPass&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            async: false,
            //data: { username: $("#userid").val(), password: $("#userpass").val(), issavepass: issavepass },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval('data=' + data);
                if (data.success) {
                    $("#userid").val(data.userid);
                    $("#userpass").val(data.userpass);
                    $("#savepass")[0].checked = true;
                }
            }
        });
    }
}
$(function () {
    //注册按钮的单击事件
    var obj = new Sys_Login();
    $("#btnLogin").click(function (e) { obj.Login(); });
    obj.RemeberMyPass();
    document.onkeydown = function (e) {
        var ev = document.all ? window.event : e;
        if (ev.keyCode == 13) {

            $("#btnLogin").click();

        }
    }


    //  $("#userid").blur(function (e) { obj.GetUserPass(); });
    /*
    $("#imagecode").bind("click", function () { $("#imagecode").attr("src", "codeimage.ashx?d=" + new Date()); });
    $("#imagecode").css("cursor", "pointer");
    */
});






