/**
 * ==========================================================
 * Copyright (c) 2013, NTS-9000 All rights reserved.
 * NTS项目用户登录JS
 * Author: Jinsam
 * Date: 2013-05-17 22:43:55 502000
 * ==========================================================
 */

require.config({
	baseUrl: "/js/",
	urlArgs: "",
	paths: {
		base: 'base',
		cookie: 'lib/cookie/jquery.cookie',
		blockui: 'lib/blockui/blockui.min',
		md5: "lib/md5/md5.min",
		common: "common"
	},
	shim: {
		common: ['base'],
		cookie: ['base'],
		blockui: ['base'],
		login: ['base','common']
	}
});

require(["base", "blockui", "md5", "common"], function(base, blockui, md5, Common) {
	function Login() {
		this.init();
	}

	Login.prototype = {

		/**
		 * 初始化页面
		 * author: ghj
		 * time: 2013-05-17 22:58:29
		 */
		init: function() {
			this.render();
		},

		/**
		 * 渲染页面
		 * author: ghj
		 * time: 2013-05-17 22:58:32
		 */
		render: function() {
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
			loginPanel: function() {
				var self = this;
				var $username = $("#username"),
					$password = $("#password"),
					$remove = $(".remove"),
					$remember = $(".about-password"),
					$checkbox = $("i", $remember),
					$loginBtn = $(".btn-login");
				var username = "",
					password = "",
					remember = false;
				$tiansu.common.placeholder($username);
				$tiansu.common.placeholder($password);
				// 得到焦点
				$username.focus(function() {
					username = $.trim($(this).val());
					if (username === this.defaultValue) {
						$(this).val("");
					}
				});
				// 失去焦点
				$username.blur(function() {
					username = $.trim($(this).val());
					if (username === "") {
						$(this).val(this.defaultValue);
						$remove.fadeOut(200);
					}
				});
				// 显示清除按钮
				$username.keyup(function() {
					username = $.trim($(this).val());
					if (username !== "" && username !== this.defaultValue) {
						$remove.fadeIn(200);
					}
					if (username === "") {
						$remove.fadeOut(200);
					}
				});
				// 清空用户名
				$remove.click(function() {
					$username.val("").focus();
					$(this).fadeOut(200);
				});
				// 记住密码
				$remember.click(function() {
					if ($checkbox.hasClass("remember-password")) {
						$checkbox.removeClass("remember-password").addClass("remembered-password");
						remember = true;
					} else {
						$checkbox.removeClass("remembered-password").addClass("remember-password");
						remember = false;
					}
				});
				// 登录按钮
				$loginBtn.click(function() {
					self.doLogin();
				});
				//回车触发登录
				//add by pl 2013-12-23
				$username.keyup(function(event) {
					if (event.keyCode == 13) {
						self.doLogin();
					}
				});
				$password.keyup(function(event) {
					if (event.keyCode == 13) {
						self.doLogin();
					}
				});
			},			
		},
      
       /** 
		 * 判断用户是否记住密码
		 * author: pl
		 * time: 2013-11-18 23:08:35
		 */
       remberPass:function(){
       var $username = $("#username"),
				$password = $("#password"),
				$remember = $(".about-password"),
				$checkbox = $("i", $remember);
			var username = "",
				password = "";
         $.ajax({
				url: this.getAction().remberPassUrl,
				dataType: "json",
				type: "POST",
				success: function(json) {
					username = json.LoginUser;
					password = json.LoginPass;
					if (username) {
						$username.val(username);
					}
					if (password) {
						$password.val(password);
						$checkbox.removeClass("remember-password").addClass("remembered-password");
					}
				},
				error: function() {
					console.log("记住密码功能请求失败！");
				}
			});	
       },

       doLogin:function(){  
       		var $username = $("#username"),
				$password = $("#password"),
				$remove = $(".remove"),
				$remember = $(".about-password"),
				$checkbox = $("i", $remember),     		
				username = $.trim($username.val()),
				password = $.trim($password.val());
			var reg = /[A-Za-z0-9_]/gi;
			//var loginInfo = {'username':''};//html5,记录用户名时使用
			if ($checkbox.hasClass("remembered-password")) {
				remember = true;
			} else {
				remember = false;
			}
			if (username === "" || username === $username[0].defaultValue) {
				$tiansu.common.info("show", {
					timeout: 1000,
					content: '请输入用户名',
					css: {
						position: "absolute",
						top: $username.offset().top - 30,
						left: $username.offset().left
					},
					timeout: 2000,
				});
			}else if (!reg.test(username)) {
				$tiansu.common.info("show", {
					timeout: 1000,
					content: '用户名只能是数字字母或下划线',
					css: {
						position: "absolute",
						top: $password.offset().top - 50,
						left: $password.offset().left
					},
					timeout: 2000,
				});
			} else if (password === "") {
				$tiansu.common.info("show", {
					timeout: 1000,
					content: '请输入密码',
					css: {
						position: "absolute",
						top: $password.offset().top - 50,
						left: $password.offset().left
					},
					timeout: 2000,
				});
			} else if (username === "admin") {
				location.href = "http://127.0.0.1:9000/adminpanel/html/index.htm";//这里填写网址“驾驶舱”
			/*} else if (username === "operator") {
				//html5本地存储,记录用户名 start
				if (window.localStorage) {
					loginInfo.username = "operator";
					localStorage.setItem("loginUserInfo", JSON.stringify(loginInfo));
				}
				//html5本地存储,记录用户名 end
				location.href = "html/main.htm";*/
			} else {
				$.unblockUI();
				//输入json
				var data = {
					LoginUser: username,
					LoginPass: password,
					IsRemeberPass: remember
				}
				data = JSON.stringify(data);
				$.ajax({
					url: window.login.getAction().getUserInfoUrl,
					dataType: "json",
					type: "POST",
					data: {
						"Inputs": data
					},
					beforeSend:function(){
						$tiansu.common.loading('.login-wrap','show',100);
					},
					success: function(json) {
						if (json.Success == true) {
							/*//html5本地存储,记录用户名 start
							if (window.localStorage) {
								loginInfo.username = "manager";
								localStorage.setItem("loginUserInfo", JSON.stringify(loginInfo));
							}
							//html5本地存储,记录用户名 end*/
							if(json.RedirectUrl != ""){
								location.href = json.RedirectUrl;
							}else{
								$tiansu.common.info("show", {
									timeout: 1000,
									content: '尚未配置菜单权限或者菜单无跳转页面',
									timeout: 4000,
								});
							}							
							window.localStorage.menu = json.Menus;
						} else {
							$tiansu.common.info("show", {
								position: "absolute",
								content:json.Msg,
								timeout: 3000,
								top: $password.offset().top - 50,
								left: $password.offset().left - 200
							});
						}
					},
					complete:function(){
						$tiansu.common.loading('.login-wrap','hide');
					},
					error: function(json) {
						alert("服务器错误，请联系管理员！");
					}
				});
			}
       },

		/** 
		 * 获取json用户信息
		 * author: ghj
		 * time: 2013-05-18 23:08:35
		 */
		getUserInfo: function() {
			var $username = $("#username"),
				$password = $("#password"),
				$remember = $(".about-password"),
				$checkbox = $("i", $remember);
			var username = "",
				password = "";
			$.ajax({
				url: this.getAction().getUserInfoUrl,
				dataType: "json",
				type: "POST",
				success: function(json) {
					username = json.RememberPassword.LoginUser;
					password = json.RememberPassword.LoginPass;
					if (username) {
						$username.val(username);
					} else {
						$username.val("请输入用户名");
					}
					if (password) {
						$password.val(md5(password));
						$checkbox.removeClass("remember-password").addClass("remembered-password");
					}
				},
				error: function() {
					alert("服务器错误，请联系管理员！");
				}
			});				
		},
		/**
		 * Ajax请求
		 * author: ghj
		 * time: 2013-05-17 22:48:07
		 */
		getAction: function() {
			return {
				remberPassUrl:"action.ashx?action=RemeberMyPass",
				getUserInfoUrl: "action.ashx?action=UserLogin"
			};
		}
	};

	this.login = new Login();
});