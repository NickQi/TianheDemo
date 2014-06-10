/**
* ==========================================================
* Copyright (c) 2013, NTS-9000 All rights reserved.
* NTS项目用户登录JS
* Author: Jinsam
* Date: 2013-05-17 22:43:55 502000
* ==========================================================
*/

require.config({
    baseUrl: "/html/resources/scripts/jsrc/",
    urlArgs: "",
    paths: {
        jquery: "lib/jquery/jquery.min",
        blockui: "lib/blockUI/blockUI",
        md5: "lib/md5/md5.min",
        common: "common/common"
    }
});

require(["jquery", "blockui", "md5", "common"], function ($, blockui, md5, Common) {
    function Login() {
        //this.common = new Common();
        this.init();
    }

    Login.prototype = {

        /**
        * 初始化页面
        * author: ghj
        * time: 2013-05-17 22:58:29
        */
        init: function () {
            this.render();
        },

        /**
        * 渲染页面
        * author: ghj
        * time: 2013-05-17 22:58:32
        */
        render: function () {
            this.remberPass();
            this.bindEvent.loginPanel.call(this);
        },

        /**
        * 页面事件绑定
        * author: ghj
        * time: 2013-05-17 22:48:07
        */
        bindEvent: {

            /** 
            * 登录事件
            * author: ghj
            * time: 2013-05-17 23:48:38
            */
            loginPanel: function () {
                var self = this;
                var _username = $("#username"),
					_password = $("#password"),
     				_remember = $("#remember"),
					_loginBtn = $("#btn_login");
                var username = "",
					password = "",
					remember = false;
                // 登录按钮
                _loginBtn.click(function () {
                    username = $.trim(_username.val());
                    password = $.trim(_password.val());
                    if (_remember[0].checked) {
                        remember = true;
                    } else {
                        remember = false;
                    }
                    if (username === "" || username === _username[0].defaultValue) {
                        new Login().showMsg("请输入您的用户名。");

                        // $.blockUI();
                    } else if (password === "") {
                        new Login().showMsg("请输入您的密码。");
                        // $.blockUI();
                    } else {
                        $.unblockUI();
                        //输入json
                        var data = {
                            LoginUser: username,
                            LoginPass: password,
                            IsRemeberPass: remember
                        };
                        data = JSON.stringify(data);
                        $.ajax({
                            url: window.login.getAction().getUserInfoUrl,
                            dataType: "json",
                            type: "POST",
                            data: { "Inputs": data },
                            success: function (json) {
                                if (json.Success) {
                                    location.href = "home.html";
                                } else {
                                    //new Login().showMsg("您的用户名或密码不正确。");
                                    new Login().showMsg(json.Msg);
                                }
                            },
                            error: function (json) {
                                //  alert("服务器错误，请联系管理员！---2");
                            }
                        });
                    }
                });
            }
        },

        /** 
        * 判断用户是否记住密码
        * author: pl
        * time: 2013-11-18 23:08:35
        */
        remberPass: function () {
            var _username = $("#username"),
               _password = $("#password"),
               _remember = $("#remember");
            var username = "",
				password = "";
            $.ajax({
                url: this.getAction().remberPassUrl,
                dataType: "json",
                type: "POST",
                success: function (json) {
                    username = json.LoginUser;
                    password = json.LoginPass;
                    if (username) {
                        _username.val(username);
                    }
                    if (password) {
                        _password.val(password);
                        _remember[0].checked = true;
                    }
                },
                error: function (msg) {
                    alert(msg.responseText);
                    //alert("服务器错误，请联系管理员！---1");
                }
            });
        },
        showMsg: function (msg) {
            $(".messages").show();
            $(".text").show();
            $(".text span").html(msg);
        },
        /** 
        * 获取json用户信息
        * author: ghj
        * time: 2013-05-18 23:08:35
        */
        getUserInfo: function () {
            var _username = $("#username"),
		        _password = $("#password"),
		        _remember = $(".about-password");
            var username = "",
				password = "";
            $.ajax({
                url: this.getAction().getUserInfoUrl,
                dataType: "json",
                type: "POST",
                success: function (json) {
                    username = json.RememberPassword.LoginUser;
                    password = json.RememberPassword.LoginPass;
                    if (username) {
                        // _username.val(username);
                    } else {
                        _username.val("请输入用户名");
                    }
                    if (password) {
                        // _password.val(md5(password));
                        //  _remember.checked = true;
                    }
                },
                error: function (msg) {


                    alert("服务器错误，请联系管理员！---3");
                }
            });
        },
        /**
        * Ajax请求
        * author: ghj
        * time: 2013-05-17 22:48:07
        */
        getAction: function () {
            return {
                remberPassUrl: "action.ashx?action=RemeberMyPass",
                getUserInfoUrl: "action.ashx?action=UserLogin"
            };
        }
    };

    this.login = new Login();
});