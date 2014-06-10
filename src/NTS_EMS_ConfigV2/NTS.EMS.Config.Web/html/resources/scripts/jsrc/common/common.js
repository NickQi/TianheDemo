/**
 * ==========================================================
 * Copyright (c) 2013, NTS-9000 All rights reserved.
 * NTS项目公用JS文件
 * Author: Jinsam
 * Date:2013-03-31 22:37:37 459000
 * ==========================================================
 */

define(function() {

	function Common() {
	}

	Common.prototype = {

		/** 
		 * 获取URL参数值
		 * @param: {string} name [参数名称]
		 * @return {string}      [参数值]
		 * author: ghj
		 * time: 2013-03-31 23:17:32
		 */
		getParam: function(name) {
			var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
			var arr = window.location.search.substr(1).match(reg);
			if (arr !== null) {
				return unescape(arr[2]);
			} else {
				return null;
			}
		},

		/** 
		 * 内容loading效果
		 * @param: {string}  container [加载loading的容器，如".box","#box"]
		 * @param: {boolean} flag      [true:显示成功loading，false显示失败loading]
		 * @param: {number}  top       [loading与容器顶部之间的距离，可缺省]
		 * @param: {string}  text      [loading文字，如"正在加载…"]
		 * author: ghj
		 * time: 2013-06-01 15:59:31
		 */
		loading: function(container, flag, top, text) {
			text = text ? text : "数据加载中…";
			var $nodesuccess = $('<div class="loading-module-success">' + text + '</div>'),
				$nodefalse = $('<div class="loading-module-false">数据加载失败！</div>'),
				$container = $(container),
				mTop = null;
				// 如果指定高度，则显示loading与容器顶部之间的距离，否则在容器中间显示
				mTop = top ? top : $container.height() / 2 - 20;
			if (flag) {				
				$nodesuccess.css({
					"margin-top": mTop > 20 ? mTop : 5
				});
				$container.append($nodesuccess);
			} else {
				$(".loading-module-success", $container).remove();
				$nodefalse.css({
					"margin-top": mTop > 20 ? mTop : 5
				});
				$container.append($nodefalse);
			}
		},

		/** 
		 * 弹窗提示信息
		 * @param  {obj} response [{status:1/0, msg:"", html: "html标签"}]
		 * @param  {obj} css      [覆盖默认css]
		 * @param  {obj} options  [覆盖默认配置]
		 * author: ghj
		 * time: 2013-06-11 23:05:10
		 */
		showInfo: function(response, css, options) {
			var className = "",
				$icon = '';
			if (response.status === 1) {
				className = "success";
				$icon = '<i class="icon-warning mr10">!</i>';
			} else if (response.status === 0) {
				className = "error";
				$icon = '<i class="icon-warning mr10">!</i>';
			} else {
				className = "tips";
				$icon = "";
			}
			var $html = $('<div class="show-info '+ className +'">' + $icon + '<div class="infos">' + response.msg + '</div></div>');
			this.css = $.extend({
				textAlign: "left",
				border: "0 none",
				cursor: "default",
				left: "50%",
				zIndex: 10000				
			}, css);
			var defaults = {
				message: response.html ? response.html : $html,
				fadeIn: 500,
				fadeOut: 500,
				timeout: 1000,
				showOverlay: false,
				centerX: false,
				centerY: false,
				css: this.css
			};
			this.param = $.extend(defaults, options);
			$.blockUI(this.param);
		},

		/** 
		 * 登录弹窗提示信息
		 * @param  {obj} response [{status:true/false, html: "html标签"}]
		 * @param  {obj} css      [覆盖默认css]
		 * @param  {obj} options  [覆盖默认配置]
		 * author: ghj
		 * time: 2013-06-11 23:05:10
		 */
		loadingShowInfo: function(response, css, options) {
			var className = "",
				$icon = '';
			if (response.status === true) {
				className = "success";
				$icon = '数据加载中…';
			} else if (response.status === false) {
				className = "false";
				$icon = '数据加载失败！';
			} else {
				className = "tips";
				$icon = "";
			}
			var $html = $('<div class="loading-module-'+ className +'">' + $icon + '</div>');
			this.css = $.extend({
				textAlign: "left",
				border: "0 none",
				cursor: "default",
				left: "50%",
				position: "absolute",
				zIndex: 10000				
			}, css);
			var defaults = {
				message: response.html ? response.html : $html,
				fadeIn: 500,
				fadeOut: 500,
				timeout: 1000,
				showOverlay: false,
				centerX: true,
				centerY: false,
				css: this.css
			};
			this.param = $.extend(defaults, options);
			$.blockUI(this.param);
		},
		/** 
		 * 弹窗确认信息
		 * @param  {obj} response [{callback:fn(), data:[], msg:""}]
		 * @param  {obj} options  [覆盖blockui配置参数]
		 * author: ghj
		 * time: 2013-06-01 16:53:12
		 */
		showConfirm: function(response, css, options) {
			var self = this;
			if (!options) {
				var options = {
					"cancel": "取消",
					"confirm": "确认"
				}
			}
			var $html = $('<div class="confirm-wrap">' +
								'<div class="confirm-inner">' +
									'<div class="confirm-content">' + response.msg + '</div>' +
									'<div class="confirm-btn"><a href="javascript:void(0);" class="cancel">' + options.cancel + '</a> / <a href="javascript:void(0);" class="confirm">' + options.confirm + '</a></div>' +
									'<i class="icon close"></i>' +
								'</div>' +
							'</div>');
			if (!css) {
				var css = {
					"confirm": {},
					"overlay": {}
				}
			}
			this.css = $.extend({
				left: "50%",
				marginLeft: "-120px",
				width: 240,
				textAlign: "left",
				backgroundColor: "none",
				border: "0 none",
				cursor: "default"
			}, css.confirm);
			this.overlayCss = $.extend({
				backgroundColor: '#000', 
        		opacity: 0.5, 
			}, css.overlay);
			var defaults = {
				message:response.html ? response.html : $html,
				fadeIn: 500,
				fadeOut: 500,
				showOverlay: false,
				css: this.css,
				overlayCSS: this.overlayCss
			};
			this.param = $.extend(defaults, response);
			$.blockUI(this.param);
			var $cancel = $(".cancel", $(this.param.message)),
				$confirm = $(".confirm", $(this.param.message)),
				$close = $(".close", $(this.param.message));
			$confirm.click(function() {
				if (typeof(response.callbacks.confirm) === "function") {
					response.callbacks.confirm.call(self);
				}
				$.unblockUI();
			});
			$cancel.click(function() {
				if (typeof(response.callbacks.cancel) === "function") {					
					response.callbacks.cancel.call(self);
				}
				$.unblockUI();
			});
			$close.click(function() {				
				if (response.callbacks && typeof(response.callbacks.close) === "function") {					
					response.callbacks.close.call(self);
				}
				$.unblockUI();
			});
		},

				/** 
		 * 显示分页
		 * @param  {obj} json     [分页参数]
		 * @return {fun} callback [分页回调参数]
		 * author: ghj
		 * time: 2013-06-14 23:29:11
		 */
		showPage: function(json, callback) {
			var pages = [],
				 page = 0,
				 cls = "";
            if(json.Current && json.Total<5){
                for(var i=1;i<=json.Total;i++){
                    pages.push(i);
                }
            }else if(json.Current && json.Current<=3 && json.Total>=5){
                pages = [1,2,3,4,5];
            }else if(json.Current && json.Current<=json.Total-2){
                pages = [json.Current-2,json.Current-1,json.Current,json.Current+1,json.Current+2];
            }else if(json.Current && json.Total>5 && json.Current>json.Total-2){
                pages = [json.Total-4,json.Total-3,json.Total-2,json.Total-1,json.Total];
            }
			  var $page = $(".show-page"),
				$html = "",
				$prevPage = $(".prev-page", $page),
				$nextPage = $(".next-page", $page),
				$totalNum = $(".total-page em", $page),
				$toNum = $("input", $page),
				$goBtn = $("button", $page);				
			// 上一页
			if (json.Current === 1) {
				$prevPage.hide();
			} else {
				$prevPage.show();
			}
			// 下一页
			if (json.Current === json.Total) {
				$nextPage.hide();
			} else {
				$nextPage.show();
			}
			// 总页数
			$totalNum.text(json.Total);
			// 分页数
			for (var i = 0; i < pages.length; i++) {
				if (json.Current === parseInt(pages[i], 10)) {
					cls = "current";
				}
				$html += ('<li class="' + cls + '" data-page="'+pages[i]+'"><a href="javascript:void(0);">' + pages[i] + '</a></li>');
				cls = "";
			}
			$prevPage.nextUntil($nextPage).remove();
			$prevPage.after($html);
			// 数字页
			$("li", $page).click(function() {
				page = $(this).attr("data-page");
				if (page && !$(this).hasClass("current")) {
					callback.call(this, page);
				}
			});
			// 上一页
			$prevPage.unbind().click(function() {
				if (!$(this).hasClass("current")) {
					page = json.Current > 1 ? json.Current - 1 : 1;
					callback.call(this, page);
				}
			});
			// 下一页
			$nextPage.unbind().click(function() {
				if (!$(this).hasClass("current")) {
					page = json.Current + 1;
					callback.call(this, page);
				}
			});
			// 显示按钮
			$toNum.focus(function() {
				$goBtn.animate({
					marginLeft: -3
				}, 350);
			});
			// 隐藏按钮
			$toNum.blur(function() {
				$goBtn.animate({
					marginLeft: -35
				}, 350);
			});
			// 跳到n页
			$goBtn.unbind().click(function() {
				page = parseInt($toNum.val(), 10);
				callback.call(this, page);
			});

			return $page;
		},

		/** 
		 * 随机颜色
		 * author: ghj
		 * time: 2013-06-13 13:35:41
		 */
		randomColor: function() {
			return '#' +
				(function(color) {
				return (color += '0123456789abcdef' [Math.floor(Math.random() * 16)]) && (color.length == 6) ? color : arguments.callee(color);
			})('');
		},

		/** 
		 * 异步加载CSS，适用于不直接参与渲染页面
		 * @param  {string} url [css文件路径]
		 * author: ghj
		 * time: 2013-04-15 20:38:13
		 */
		loadCss: function(url) {
			var link = document.createElement("link");
			link.type = "text/css";
			link.rel = "stylesheet";
			link.href = url;
			document.getElementsByTagName("head")[0].appendChild(link);
		},
      /** 
		 * 计算起止时间之间的差值
		 * @param  dateStart [开始日期]
		 * @param  dateEnd [结束日期]
		 * author: pl
		 * time: 2013-11-24 20:38:13
		 */
       diffDate:function(str1, str2) {
          str1 = str1.replace(/-/g, "/");
          str2 = str2.replace(/-/g, "/");
          var d1;
          var d2;
          var diffday = 0;
          if (str1 == "") {
            d1 = new Date();
          } else {
            d1 = new Date(str1);
          }
          if (str2 == "") {
            d2 = new Date();
          } else {
            d2 = new Date(str2);
          }
          diffday = Date.parse(d2) - Date.parse(d1);
          diffday = diffday.toFixed(2) / 86400000 ;
          return diffday;
        }

	};

	return Common;
});