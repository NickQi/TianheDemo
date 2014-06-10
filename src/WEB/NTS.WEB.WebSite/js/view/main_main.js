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
		common: "common"
	},
	shim: {
		common: ['base'],
		cookie: ['base'],
		main: ['base','common']
	}
});

require(["base", "cookie", "common"], function(base, cookie, Common) {
	function Main() {
		this.init();
	}

	Main.prototype = {

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
			this.initUrl();//页面初始化，电力URL
		},
		/** 
		 * 页面初始化，电力URL
		 * author: luyy
		 * time: 2014-05-28 10:19:01
		 */
		initUrl: function() {
			//var strStoreDate = window.localStorage? JSON.parse(localStorage.getItem("loginUserInfo")): "";
			var $enterElectLi = $(".enterThreeInOne ul li:first"),
				username = $.cookie("userid"),
				that = this,
				myDate = new Date();
			myDate = $tiansu.date.format({
				date: new Date(myDate),
				connect: '-'
			});
			$("#sys-time").html(myDate);//本地时间
			//用能计费URL
			$.ajax({
				url: that.getAction().getBillingUrl,
				dataType: "json",
				type: "GET",
				beforeSend:function(){
					$tiansu.common.loading('.enterThreeInOne','show',100);
				},
				success: function(json) {
					if (json.ActionInfo.Success) {
						$(".enterThreeInOne ul li:last a").attr('href',json.url);
					}
				},
				complete:function(){
					$tiansu.common.loading('.enterThreeInOne','hide');
				},
				error: function(json) {
					alert("服务器错误，请联系管理员！");
				}
			});
			//电力监控
			switch (username) {
				case 'operator':
					$("a",$enterElectLi).attr('href','javascript:void(0)');
					that.bindEvent.electMenu.call(this);
				break;
				case 'manager':
					$("a",$enterElectLi).attr('href','electrical-topology.htm');
				break;
			} 
		},		
		/**
		 * 页面事件绑定
		 * author: ghj
		 * time: 2013-05-17 22:48:07
		 */
		bindEvent: {
			/** 
			 * 电力监控菜单点击
			 * author: ghj
			 * time: 2013-05-17 23:48:38
			 */
			electMenu: function() {
				var $enterElectLi = $(".enterThreeInOne ul li:first"),
					that = this;
				$("a",$enterElectLi).click(function(){
					$.ajax({
						url: that.getAction().getElectUrl,
						dataType: "text",
						type: "GET",
						beforeSend:function(){
							$tiansu.common.loading('.enterThreeInOne','show',100);
						},
						success: function(json) {
							//code
						},
						complete:function(){
							$tiansu.common.loading('.enterThreeInOne','hide');
						},
						error: function(json) {
							alert("服务器错误，请联系管理员！");
						}
					});
				});
			}		
		},
		/**
		 * Ajax请求
		 * author: ghj
		 * time: 2013-05-17 22:48:07
		 */
		getAction: function() {
			return {
		        getElectUrl:"action.ashx?action=CallTSview",
		        getBillingUrl:"/js/data/billingUrl.js"
			};
		}
	};

	this.main = new Main();
});