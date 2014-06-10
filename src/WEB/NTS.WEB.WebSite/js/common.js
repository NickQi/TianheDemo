/** 
* ==========================================================
* Copyright (c) 2013, nts.com All rights reserved.
* 公用组件JS
* Author: Jinsam
* Date: 2013-10-22 16:23:43 745000
* ==========================================================
*/
if (typeof $tiansu === 'undefined') {
    var $tiansu = {
        version: '1.0.0'
    };
}
// conslog.log兼容IE
if (typeof console === 'undefined') {
    var console = {};
    console.log = function (msg) {
        return false;
        /*$.blockUI({
        message: '<span style="position:relative;">' + msg + '</span>',
        showOverlay: false,
        fadeIn: 200,
        fadeOut: 400,
        timeout: 1000,
        css: {
        margin: '0 auto',
        padding: '15px 25px',
        font: 'normal 12px/100% Arial, Helvetica, sans-serif',
        color: '#fff',
        filter: 'alpha(opacity=80)',
        background: '#000',
        background: 'rgba(0,0,0,0.8)',
        display: 'inline',
        zoom: 1,
        display: 'inline-block',
        borderRadius: '2px',
        boxShadow: '0 0 2px rgba(0,0,0,0.5)',
        cursor: 'default',
        border: '0 none'
        }
        });*/
    };
}
$tiansu.common = {
    /*
    * 全屏显示
    * @param  {obj} element xml dom obj [<html>]
    * author: pl
    * time : 2014-3-20 16:38:34
    */
    launchFullscreen: function (element) {
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        } else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }
    },
    //退出全屏
    exitFullscreen: function () {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        } else if (typeof window.ActiveXObject !== "undefined") {
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }
    },

    fullScreenCtrl: function (selector) {
        var isFull = false;
        var _this = this;
        $(selector).click(function (e) {
            e.preventDefault();
            if (!isFull) {
                try {
                    _this.launchFullscreen(document.documentElement);
                    $(this).text("退出全屏");
                    isFull = true;
                } catch (err) {
                    $(".screen-tip").show(function () {
                        $(".screen-tip, .hd-fullscreen a").click(function (e) {
                            e.stopPropagation();
                        });
                        $(document).click(function () {
                            $(".screen-tip").hide();
                        });
                        return false;
                    });
                }
            } else {
                _this.exitFullscreen();
                $(this).text("全屏");
                isFull = false;
            }
        });
    },

    /** 
    * html5 placeholder
    * @param  {str/obj} element ['.classname', '#id', $element]
    * author: ghj
    * time: 2013-12-28 16:16:38
    */
    placeholder: function (element) {
        var $element = $(element),
			holder = $element.attr('placeholder'),
			support = 'placeholder' in document.createElement('input');
        if (support) {
            $element.focus(function () {
                if (this.value === this.defaultValue) {
                    this.value = '';
                }
            });
            $element.blur(function () {
                if (this.value === '') {
                    this.value = this.defaultValue;
                }
            });
        } else {
            var height = $element.outerHeight(),
				width = $element.width(),
				left = $element.offset().left + 10,
				top = $element.offset().top;
            var $span = $('<span></span>')
				.css({
				    position: 'absolute',
				    overflow: 'hidden',
				    fontSize: '14px',
				    color: '#2980b9',
				    width: width + 'px',
				    height: height + 'px',
				    lineHeight: height + 'px',
				    left: left + 'px',
				    top: top + 'px',
				    display: 'block',
				    cursor: 'text',
				    zIndex: 10
				})
				.text(holder);
            $('body').append($span);
            $span.click(function () {
                $element.focus();
            });
            $element.keyup(function () {
                if (this.value !== '') {
                    $span.hide();
                } else {
                    $span.show();
                }
            });
            $element.blur(function () {
                if (this.value !== '') {
                    $span.hide();
                } else {
                    $span.show();
                }
            });
        }
    },

    /** 
    * 获取URL参数
    * @param: {str} name [参数名称]
    * @return {str} [返回参数值]
    * author: ghj
    * time: 2013-10-22 15:12:26
    */
    getParam: function (name) {
        var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
        var r = window.location.search.substr(1).match(reg);
        if (r !== null) {
            return decodeURIComponent(r[2]);
        } else {
            return null;
        }
    },

    /*
    * 处理url
    * author: pl
    * time: 2014-3-23 12:07:17
    */
    parseURL: function (url) {
        var a = document.createElement('a');
        //创建一个链接
        a.href = url;
        return {
            source: url,
            protocol: a.protocol.replace(':', ''),
            host: a.hostname,
            port: a.port,
            query: a.search,
            params: (function () {
                var ret = {},
					seg = a.search.replace(/^\?/, '').split('&'),
					len = seg.length,
					i = 0,
					s;
                for (; i < len; i++) {
                    if (!seg[i]) {
                        continue;
                    }
                    s = seg[i].split('=');
                    ret[s[0]] = s[1];
                }
                return ret;
            })(),
            file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
            hash: a.hash.replace('#', ''),
            path: a.pathname.replace(/^([^\/])/, '/$1'),
            relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
            segments: a.pathname.replace(/^\//, '').split('/')
        };
    },

    /** 
    * info
    * @param: {str} display [显示/隐藏 show/hide]
    * @param: {obj} options [参数]
    * author: ghj
    * time: 2013-10-22 15:08:01
    */
    info: function (display, options) {
        if (display === 'show' || display === '') {
            var defaults = {
                // 新增参数
                content: '',
                html: '<div class="info"><div class="info-content"></div></div>',
                container: 'body',
                // blockui参数
                fadeIn: 200,
                fadeOut: 200,
                centerX: true,
                centerY: true,
                showOverlay: false,
                css: {
                    background: 'none',
                    border: 0,
                    cursor: 'default',
                    position: 'fixed'
                },
                overlayCSS: {
                    opacity: 0.4,
                    cursor: 'default'
                }
            };
            var config = $.extend(true, {}, defaults, options),
				$html = $(config.html),
				length = $html.find('.info-content').length;
            if (length === 1) {
                $html.find('.info-content').text(config.content === '' ? '正在加载...' : config.content);
                config.message = $html;
                $.blockUI(config);
            } else {
                console.log('[info] options.html中必须包含.info-content容器');
            }
        } else if (display === 'hide') {
            $.unblockUI();
        } else {
            console.log('[info] 请传入display参数');
        }
    },

    /** 
    * 数据loading加载效果
    * @param: {[string]}  container [加载loading的容器，如".box","#box"]
    * @param: {[string]} display   [show:显示loading，hide隐藏loading]
    * @param: {[number]}  top       [loading与容器顶部之间的距离]
    * author: ghj
    * time: 2013-10-25 17:18:39
    */
    loading: function (container, display, top, left) {
        var $node = $('<div class="loading f14"><span><img height="30px" src="/img/loading.gif" /><img class="ml10" src="/img/loading1.gif" /></span></div>'),
			$container = $(container),
			mTop = null;
        if (display === 'show') {
            // 如果指定高度，则显示loading与容器顶部之间的距离，否则在容器中间显示
            mTop = top ? top : $container.height() / 2 - 20;
            mLef = left ? left : $container.width() / 2 - 50;
            $node.css({
                "position": "absolute",
                "left": mLef,
                "top": mTop > 20 ? mTop : 5
            });
            if ($(".loading", $container).length === 0) {
                $container.append($node);
            }
        } else if (display === 'hide') {
            $(".loading", $container).remove();
        }
    },

    /** 
    * 弹出窗口
    * author: ghj
    * time: 2013-10-22 15:08:01
    */
    modal: function (options) {
        if (typeof options === 'object') {
            var defaults = {
                title: '',
                content: '',
                // 新增参数
                //modify by pl 2014-01-15 22:22:25 修改div和titil的class名
                html: '<div class="module-define f14">' +
					'<h4 class="title bg_green text-left"></h4>' +
					'<a href="javascript:void(0);" class="close">×</a>' +
					'<div class="modal-content text-left"></div>' +
					'<div class="modal-action"><button class="confirm btn btn-blue-small">确定</button><button class="cancel btn btn-blue-small ml20">取消</button></div>' +
					'</div>',
                // blockui参数
                fadeIn: 200,
                fadeOut: 200,
                centerX: true,
                centerY: true,
                showOverlay: true,
                css: {
                    background: 'none',
                    border: 0,
                    cursor: 'default',
                    position: 'fixed'
                },
                overlayCSS: {
                    opacity: 0.3,
                    cursor: 'default'
                },
                close: {
                    callback: null
                },
                cancel: {
                    text: '',
                    callback: null
                },
                confirm: {
                    text: '',
                    callback: null
                }
            };
            var config = $.extend(true, {}, defaults, options),
				$html = $(config.html),
				$close = $('.close', $html),
				$title = $('.title', $html),
				$content = $('.modal-content', $html),
				$action = $('.modal-action', $html),
				$cancel = $('.cancel', $html),
				$confirm = $('.confirm', $html);
            // 标题判断
            if (config.title === '') {
                $title.hide();
                $close.hide();
                $action.hide();
            } else {
                $title.html(config.title);
            }
            // 内容判断
            if (config.content === '') {
                console.log('[modal] 请传入options.content参数');
            } else {
                $content.html(config.content);
                config.message = $html;
                $.blockUI(config);
            }
            // 关闭符号
            if (config.close.text !== '') {
                $close.html(config.close.text);
            }
            // 取消文字
            if (config.cancel.text !== '') {
                $cancel.text(config.cancel.text);
            }
            // 确认文字
            if (config.confirm.text !== '') {
                $confirm.text(config.confirm.text);
            }
            // 关闭事件
            $close.click(function () {
                if (typeof config.close.callback === 'function') {
                    config.close.callback.call(this);
                }
                $.unblockUI();
            });
            // 取消事件
            $cancel.click(function () {
                if (typeof config.cancel.callback === 'function') {
                    config.cancel.callback.call(this, {
                        callback: function () {
                            $.unblockUI();
                        }
                    });
                } else {
                    $.unblockUI();
                }
            });
            // 确认事件
            $confirm.click(function () {
                if (typeof config.confirm.callback === 'function') {
                    config.confirm.callback.call(this, {
                        callback: function () {
                            $.unblockUI();
                        }
                    });
                } else {
                    $.unblockUI();
                }
            });
        } else {
            console.log('[modal] 请传入options参数');
        }
    },

    /** 
    * template模板界定符
    * author: ghj
    * time: 2013-11-11 18:26:34
    */
    templateTag: function () {
        template.openTag = "{";
        template.closeTag = "}";
    },

    /** 
    * 页面跳转前用 html5 本地存储将参数保存到window.localStorage中，用于页面间共享参数
    * author: pl
    * time: 2013-04-17 20:16:33
    */
    shareParam: function (param) {
        try {
            var storage = window.sessionStorage;
            storage.ItemCode1 = param.ItemCode1;
            storage.ItemCode2 = param.ItemCode2;
            storage.ObjType = param.ObjType;
            storage.AreaIdLst = param.AreaIdLst;
            storage.StartTime = param.StartTime;
            storage.EndTime = param.EndTime;
        } catch (err) {
            console.log("您的浏览器不支持本地存储！");
            return false;
        }
    },

    /** 
    * 左侧菜单事件
    * author: ghj
    * time: 2014-01-09 23:48:14
    */
    sidebar: function () {
        var $sidebar = $('#sidebar');
        if ($sidebar.length === 1) {
            //根据权限异步加载左侧菜单
            $.ajax({
                url: "action.ashx?action=GetMenuModule",
                dataType: 'json',
                type: 'GET',
                data: {},
                beforeSend: function () {
                    $tiansu.common.loading(".menu", "show", 200, 5);
                },
                success: function (json) {
                    //alert(json);
                    var cc = [];
                    $.each(json.data, function (i, item) {
                        cc.push("<li>");
                        if (item.state === "open") {
                            cc.push("<a class='current' href=" + item.href + "><i class=" + item.iconCls + "></i><span class='menu-name'>" + item.text + "</span>");
                        } else {
                            cc.push("<a href=" + item.href + "><i class=" + item.iconCls + "></i><span class='menu-name'>" + item.text + "</span>");
                        }
                        if (item.children.length > 0 && item.state === "open") {
                            cc.push("<i class='father contract ml5'></i>");
                        } else if (item.children.length > 0 && item.state != "open") {
                            cc.push("<i class='father ml5'></i>");
                        }
                        cc.push("</a>");
                        if (item.children.length > 0) {
                            cc.push("<ul");
                            if (item.state === "closed") {
                                cc.push("style='display:none' class='sub-menu'>");
                            } else {
                                cc.push("class='sub-menu'>");
                            }
                            for (var i = 0; i < item.children.length; i++) {
                                if (i == item.children.length - 1) {
                                    cc.push("<li class='last'>");
                                } else {
                                    cc.push("<li>");
                                }
                                cc.push("<a href=" + item.children[i].href + "><i class='icon'></i><span>" + item.children[i].text + "</span></a>");
                                cc.push("</li>");
                            }
                            cc.push("</ul>");
                        }
                        cc.push("</li>");
                    });
                    var html = cc.join(" ");
                    //按钮点击事件
                    $sidebar.find("ul").html(html);
                    var $menu = $('a', $sidebar);
                    /*$menu.click(function () {
                    var $subMenu = $(this).siblings('.sub-menu'),
                    num = $subMenu.length,
                    visible = $subMenu.is(':visible');
                    if (num === 1 && visible) {
                    $subMenu.slideUp(200);
                    } else if (num === 1 && !visible) {
                    $subMenu.slideDown(200);
                    } else {
                    return;
                    }
                    });*/

                    //添加选择样式
                    /*var url = $tiansu.common.parseURL(window.location);
                    $.each($menu, function (i, item) {
                    //console.log(item);
                    var href = $(item).attr("href");
                    if (url.file == href) {
                    $menu.removeClass("current");
                    $(item).addClass("current");
                    var isSub = $(item).parents(".sub-menu");
                    if (isSub) {
                    $(isSub).siblings("a").addClass("current");
                    $(isSub).css("display", "block");
                    }
                    }
                    });*/
                    //添加选择样式
                    var url = window.location.pathname;
                    $.each($menu,function(i,item){
                        var href = $(item).attr("href");
                        if(url.indexOf(href) >= 0){
                            $menu.removeClass("current");
                            $(item).addClass("current");
                            var isSub = $(item).parents(".sub-menu");
                            isSub.siblings().find("i.father").addClass("contract");
                            if(isSub){
                                $(isSub).siblings("a").addClass("current");
                                $(isSub).css("display","block");
                            }
                        }
                    }); 
                    $menu.click(function() {
                        var $subMenu = $(this).siblings('.sub-menu'),
                            num = $subMenu.length,
                            visible = $subMenu.is(':visible');
                        //revised by lyy 2014-04-25 14:31:56//start
                        if (num === 1 && !visible) {
                            $menu.removeClass("current");
                            $("i.father").removeClass("contract")
                            $(".sub-menu",$sidebar).slideUp(200);
                            $subMenu.slideDown(200).find("li:first a").addClass("current");
                            $(this).addClass("current");
                            $(this).find("i.father").addClass("contract");
                        }
                        //revised by lyy 2014-04-25 14:31:56//end
                    });
                    $("i.father").click(function(e){
                        e.stopPropagation();
                        if ($(this).parent().hasClass("current")) {
                            if ($(this).hasClass("contract")) {
                                $(this).removeClass("contract");
                                $(this).parent().siblings(".sub-menu").hide();
                            } else {
                                $(this).addClass("contract");
                                $(this).parent().siblings(".sub-menu").show();
                            }
                        }
                    });
                },
                complete: function () {
                    $tiansu.common.loading(".menu", "hide");
                },
                error: function () {
                    var top = $sidebar.offset().top; alert(top);
                    $tiansu.common.info('show', {
                        timeout: 2000,
                        content: '菜单数据请求失败！',
                        css: {
                            top: 200,
                            left: -160,
                            position: "absolute"
                        }
                    });
                }
            });
            var $menu = $('a', $sidebar);
            //添加选择样式
            /*
            var url = window.location.pathname;
            $.each($menu, function (i, item) {
            var href = $(item).attr("href");
            if (url.indexOf(href) >= 0) {
            $menu.removeClass("current");
            $(item).addClass("current");
            var isSub = $(item).parents(".sub-menu");
            if (isSub) {
            $(isSub).siblings("a").addClass("current");
            $(isSub).css("display", "block");
            }
            }
            });
            */
            /*$menu.click(function () {
            var $subMenu = $(this).siblings('.sub-menu'),
            num = $subMenu.length,
            visible = $subMenu.is(':visible');
            //revised by lyy 2014-04-25 14:31:56//start
            if (num === 1 && !visible) {
            $menu.removeClass("current");
            $(".sub-menu", $sidebar).slideUp(200);
            $subMenu.slideDown(200).find("li:first a").addClass("current");
            $(this).addClass("current");
            }
            //revised by lyy 2014-04-25 14:31:56//end
            });*/
        }
    },

    /** 
    * 安全退出
    * author: mdk
    * time: 2013-02-27 15:24:22
    */
    exit: function () {
        /*$.post("../../Process/Login/Index.ashx?type=Exit", {},
        function (data) {
        // data 可以是 xmlDoc, jsonObj, html, text, 等等.
        //this; // 这个Ajax请求的选项配置信息，请参考jQuery.get()说到的this
        if (data == "true") {
        window.location.href = "../../html/login.html";
        }
        }, "json");*/
    },

    /** 
    * 根据权限判断快捷链接是否可用
    * author: pl
    * time: 2014-4-29 20:23:51
    */
    quickLink: function () {
        $("ul.quick-links a").click(function () {
            var index = window.localStorage.menu.indexOf($(this).attr("href"));
            if (index < 0) {
                $tiansu.common.info("show", {
                    content: "对不起，你没有该页面的权限！",
                    css: {
                        centerX: false,
                        centerY: false,
                        top: $(this).offset().top - 50,
                        left: $(this).offset().left - 300
                    },
                    timeout: 3000
                });
                return false;
            }
        });
    },

    /** 
    * 动态设置树结构高度
    * author: ghj
    * time: 2014-01-15 22:50:21
    */
    setTreeHeight: function () {
        var $tree = $('#tree-content');
        var $tree2 = $('#areaTree-content');
        var contrastType_height = $("#contrast-type").outerHeight(true) || 0;
        var datebox_height = $(".date-block").outerHeight(true) || 0;
        var treeTab_height = $("#tree-tab").outerHeight() || 0;
        var areaSlc_height = $("#area-select").outerHeight(true) || 0;
        var step_height = $(".step1 h2").outerHeight(true) || 0;
        var remove_height = datebox_height + treeTab_height + areaSlc_height + contrastType_height;

        if ($tree.length === 1) {
            if (step_height > 0) {
                var h = 580;
                $tree.height(h);
                $tree2.height(h);
            } else {
                var h = $(document).height() - $('#header').height() - $('#footer').outerHeight() - remove_height - 20;
                $tree.height(h);
                $tree2.height(h);
                $(window).resize(function () {
                    var pageHeight = 0;
                    if ($(window).height() < 900) {
                        pageHeight = 900;
                    } else {
                        pageHeight = $(window).height();
                    }

                    h = pageHeight - $('#header').height() - $('#footer').outerHeight() - remove_height - 20;
                    $tree.height(h);
                    $tree2.height(h);
                });
            }
        }
    },

    /*******************************
    * 根据默认规则获取颗粒度
    * @params: {String} from, {String} to, 格式："2014-3-23"
    * return: 1：小时颗粒度； 2： 天颗粒度； 3：月颗粒度； 4：年颗粒度
    *******************************/
    getParticle: function (from, to) {
        var diff_day = this.diffDate(from, to);
        var tick_1 = 0; //一天内
        var tick_2 = 90;
        var tick_3 = 365 * 3;
        if (diff_day == tick_1) {
            return 0;
        } else if (diff_day <= tick_2 && diff_day > tick_1) {
            return 1;
        } else if (diff_day <= tick_3 && diff_day > tick_2) {
            return 2;
        } else {
            return 3;
        }
    },

    /*起止时间之间的天数*/
    diffDate: function (from, to) {
        var strFrom = from.replace(/-/g, "/");
        var strTo = to.replace(/-/g, "/");
        var dateFrom = new Date(strFrom);
        var dateTo = new Date(strTo);
        var diff = dateTo.valueOf() - dateFrom.valueOf();
        var diff_day = diff / (1000 * 60 * 60 * 24);
        return diff_day;
    },


    /** 
    * 显示分页
    * @param  {obj} json     [分页参数]
    * @return {fun} callback [分页回调参数]
    * author: ghj
    * time: 2013-06-14 23:29:11
    */
    page: function (json, callback) {
        var pages = [],
			page = 0,
			cls = "";
        if (json.current && json.total < 5) {
            pages = json.pages;
        } else if (json.current && json.current <= 3 && json.total >= 5) {
            pages = [1, 2, 3, 4, 5];
        } else if (json.current && json.current <= json.total - 2) {
            pages = [json.current - 2, json.current - 1, json.current, json.current + 1, json.current + 2];
        } else if (json.current && json.total > 5 && json.current > json.total - 2) {
            pages = [json.total - 4, json.total - 3, json.total - 2, json.total - 1, json.total];
        }
        var $page = $(".show-page"),
			$html = "",
			$prevPage = $(".prev-page", $page),
			$nextPage = $(".next-page", $page),
			$totalNum = $(".total-page em", $page),
			$toNum = $("input", $page),
			$goBtn = $("button", $page),
			$topage = $(".to-page", $page);
        //总页数小于1，隐藏跳转输入
        if (json.total <= 1) {
            $topage.hide();
        } else if (json.total >= 1) {
            $topage.show();
        }
        // 上一页
        if (parseInt(json.current) === 1) {
            $prevPage.hide();
        } else {
            $prevPage.show();
        }
        // 下一页
        if (parseInt(json.current) === json.total) {
            $nextPage.hide();
        } else {
            $nextPage.show();
        }
        // 总页数
        $totalNum.text(json.total);
        // 分页数
        for (var i = 0; i < pages.length; i++) {
            if (parseInt(json.current) === parseInt(pages[i], 10)) {
                cls = "current";
            }
            $html += ('<li class="' + cls + '" data-page="' + pages[i] + '"><a href="javascript:void(0);">' + pages[i] + '</a></li>');
            cls = "";
        }
        $prevPage.nextUntil($nextPage).remove();
        $prevPage.after($html);
        // 数字页
        $("li", $page).click(function () {
            page = $(this).attr("data-page");
            if (page && !$(this).hasClass("current") && typeof callback === 'function') {
                callback.call(this, page);
            }
        });
        // 上一页
        $prevPage.unbind().click(function () {
            if (!$(this).hasClass("current") && typeof callback === 'function') {
                page = parseInt(json.current) > 1 ? parseInt(json.current) - 1 : 1;
                callback.call(this, page);
            }
        });
        // 下一页
        $nextPage.unbind().click(function () {
            if (!$(this).hasClass("current") && typeof callback === 'function') {
                page = parseInt(json.current) + 1;
                callback.call(this, page);
            }
        });
        // 显示按钮
        $toNum.focus(function () {
            $goBtn.animate({
                marginLeft: 0
            }, 350);
        });
        // 隐藏按钮
        $toNum.blur(function () {
            $goBtn.animate({
                marginLeft: -35
            }, 350);
        });
        // 跳到n页
        $goBtn.unbind().click(function () {
            page = parseInt($toNum.val(), 10);
            if ($tiansu.regexp.isPositive(page) && page > 0) {
                if (typeof callback === 'function') {
                    callback.call(this, page);
                }
            } else {
                $tiansu.common.info('show', {
                    timeout: 1000,
                    content: '请填写大于0的数字'
                })
            }
        });
        return $page;
    },

    /** 
    * 查询按钮垂直居中
    * Author: HF
    * Time: 2014-3-22 15:43:49
    */
    queryBtnPos: function () {
        var $btn = $("#queryBtn");
        var btnHeight = $btn.height();
        var posTop = 0;
        var posLeft = parseInt($btn.css("left"));
        var visualHeight = $(window).height();
        posTop = (visualHeight - btnHeight) / 2 + 50;
        $btn.css("top", posTop).show();
        $(window).resize(function () {
            visualHeight = $(window).height();
            posTop = (visualHeight - btnHeight) / 2 + 50;
            $btn.css("top", posTop);
        });
        $(window).scroll(function () {
            var newPosLeft = posLeft - $(window).scrollLeft();
            //console.log(newPosLeft);
            $btn.css("left", newPosLeft);
        });
    },

    /** 
    * 自适应左右滑动
    * @param {string}container: 容器的选择符
    * @param {string}prevCtrl: prev的选择符
    * @param {string}nextCtrl: next的选择符
    * Author: HF
    * Time: 2014-3-28 18:19:56
    */
    responseScroll: function (container, prevCtrl, nextCtrl, showNum) {
        var showNum = showNum || 1;
        var $container = $(container);
        var $prev = $(prevCtrl);
        var $next = $(nextCtrl);
        var $wrapper = $container.children("ul");
        var $items = $wrapper.children("li");
        var itemSize = $items.size();
        var offsetNum = 0;
        //set width:
        var containerWidth = $container.width();
        var itemWidth = containerWidth * (1 / Math.min(showNum, itemSize));
        var wrapperWidth = itemWidth * itemSize;
        $items.css("width", itemWidth);
        $wrapper.css("width", wrapperWidth);

        $(window).resize(function () {
            containerWidth = $container.width();
            itemWidth = containerWidth * (1 / Math.min(showNum, itemSize));
            wrapperWidth = itemWidth * itemSize;
            $items.css("width", itemWidth);
            $wrapper.css("width", wrapperWidth);
        });

        $prev.hide();
        if (showNum >= itemSize) {
            $next.hide();
        }
        $next.click(function () {
            offsetNum--;
            $wrapper.stop(true, true).animate({ marginLeft: itemWidth * offsetNum });
            if (-offsetNum > 0) {
                $prev.show();
            }
            if (-offsetNum == (itemSize - showNum)) {
                $next.hide();
            }
        });
        $prev.click(function () {
            offsetNum++;
            $wrapper.stop(true, true).animate({ marginLeft: itemWidth * offsetNum });
            if (offsetNum == 0) {
                $prev.hide();
            }
            if (-offsetNum < (itemSize - showNum)) {
                $next.show();
            }
        });

    },

    /** 
    * 设备列表上下滚动
    * @param {string}container: 容器的选择符
    * @param {string}prevCtrl: prev的选择符
    * @param {string}nextCtrl: next的选择符
    * Author: HF
    * Time: 2014-3-29 18:05:10
    */
    scrollUpDown: function (container, prevCtrl, nextCtrl, showNum) {
        var showNum = showNum || 1;
        var $container = $(container);
        var $prev = $(prevCtrl);
        var $next = $(nextCtrl);
        var $wrapper = $container.children("ul");
        var $items = $wrapper.children("li");
        var itemSize = $items.size();
        var offsetNum = 0;
        var itemHeight = $items.height();

        $prev.addClass("disabled");
        if (showNum >= itemSize) {
            $next.addClass("disabled");
        } else {
            $next.removeClass("disabled");
        }
        $next.click(function () {
            if ($(this).hasClass("disabled")) {
                return false;
            } else {
                offsetNum--;
                $wrapper.stop(true, true).animate({ marginTop: itemHeight * offsetNum });
                if (-offsetNum > 0) {
                    $prev.removeClass("disabled");
                }
                if (-offsetNum == (itemSize - showNum)) {
                    $next.addClass("disabled");
                }
            }
        });
        $prev.click(function () {
            if ($(this).hasClass("disabled")) {
                return false;
            } else {
                offsetNum++;
                $wrapper.stop(true, true).animate({ marginTop: itemHeight * offsetNum });
                if (offsetNum == 0) {
                    $prev.addClass("disabled");
                }
                if (-offsetNum < (itemSize - showNum)) {
                    $next.removeClass("disabled");
                }
            }
        });
    },
    /** 
    * function image shake
    * author: luyy
    * time: 2014-04-14 11:23:33
    */
    shake: function (o, end) {
        var typ = ["marginTop", "marginLeft"],
			rangeN = 6,
			timeout = 20;
        var range = Math.floor(Math.random() * rangeN);
        var typN = Math.floor(Math.random() * typ.length);
        o["style"][typ[typN]] = "" + range + "px";
        var shakeTimer = setTimeout(function () {
            $tiansu.common.shake(o, end)
        }, timeout);
        o[end] = function () {
            clearTimeout(shakeTimer)
        };
    },
    /** 
    * change two decimal
    * author: luyy
    * time: 2014-04-29 16:17:16
    */
    changeTwoDecimal_f: function (x) {
        var f_x = parseFloat(x);
        if (isNaN(f_x)) {
            //alert('function:changeTwoDecimal->parameter error');  
            return "-";
        }
        var f_x = Math.round(x * 100) / 100;
        var s_x = f_x.toString();
        var pos_decimal = s_x.indexOf('.');
        if (pos_decimal < 0) {
            pos_decimal = s_x.length;
            s_x += '.';
        }
        while (s_x.length <= pos_decimal + 2) {
            s_x += '0';
        }
        return s_x;
    }
};
// 日期类
$tiansu.date = {
    /** 
    * 日期相加减
    * @param {obj} options [{data:'2013-11-11',number:'5',type:'day/month/year'}]
    * author: ghj
    * time: 2013-11-12 18:46:28
    */
    diff: function (options) {
        var date, str;
        if (options.type === 'day') {
            str = options.date;
            str = str.split('-');
            date = new Date(str[0], str[1] - 1, str[2]);
            date = date.valueOf();
            date = date + (options.number * 24 * 60 * 60 * 1000);
            date = new Date(date);
        } else if (options.type === 'month') {
            date = new Date(options.date);
            var month = (date.getMonth() + 1) + options.number;
            date = new Date(date.getFullYear() + '/' + month + '/' + date.getDate());
        } else if (options.type === 'year') {
            date = new Date(options.date);
            var year = date.getFullYear() + options.number;
            date = new Date(year + '/' + (date.getMonth() + 1) + '/' + date.getDate());
        }
        return date;
    },

    /** 
    * 格式化日期
    * @param {obj} options [{date:2013-11-11,connect:'/'}]
    * author: ghj
    * time: 2013-11-12 19:08:50
    */
    format: function (options) {
        var d = null;
        if (options.date) {
            d = new Date(options.date);
        } else {
            d = new Date();
        }
        var year = d.getFullYear(),
			month = d.getMonth() + 1;
        day = d.getDate();
        if (month < 10) {
            month = "0" + month;
        }
        if (day < 10) {
            day = "0" + day;
        }
        if (!options.connect) {
            options.connect = '-';
        }
        var dateString = year + options.connect + month + options.connect + day;
        return dateString;

    },
    /** 
    * 获取一个月的天数
    * author: pl
    * time: 2014-3-18 19:49:36
    */
    getMonthDays: function (nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    },
    /** 
    * 日、周、月、年 时间段选择
    * @param {obj} options [{type:1,date:'2013-03-18',connect:'-',sDate:$('input[name="startDate"]'),eDate:$('input[name="endDate"]')}]
    * author: pl
    * time: 2014-3-18 18:04:30
    */
    datePeriod: function (options) {
        options.date = options.date.replace(/-/g, "/");
        var now = new Date(options.date); //当前日期
        var nowDayOfWeek = now.getDay(); //今天本周的第几天
        var nowDay = now.getDate(); //当前日
        var nowMonth = now.getMonth(); //当前月
        var nowYear = now.getFullYear(); //当前年
        //nowYear += (nowYear < 2000) ? 1900 : 0; 
        var startDate, endDate;
        //1:本日,2：本周，3：本月，4：本年，5：明日，6：下周，7：下月
        switch (options.type) {
            case "1":
                startDate = new Date(nowYear, nowMonth, nowDay);
                endDate = new Date(nowYear, nowMonth, nowDay);
                break;
            case "2":
                if (nowDayOfWeek > 0) {
                    startDate = new Date(nowYear, nowMonth, nowDay - nowDayOfWeek + 1);
                } else {
                    startDate = new Date(nowYear, nowMonth, nowDay - nowDayOfWeek - 6);
                }
                endDate = new Date(nowYear, nowMonth, nowDay + (7 - nowDayOfWeek));
                break;
            case "3":
                startDate = new Date(nowYear, nowMonth, 1);
                endDate = new Date(nowYear, nowMonth, $tiansu.date.getMonthDays(nowYear, nowMonth));
                break;
            case "4":
                startDate = new Date(nowYear, 0, 1);
                endDate = new Date(nowYear, 12, 0);
                break;
            case "5":
                startDate = new Date(nowYear, nowMonth, nowDay + 1);
                endDate = new Date(nowYear, nowMonth, nowDay + 1);
                break;
            case "6":
                startDate = new Date(nowYear, nowMonth, nowDay + (7 - nowDayOfWeek) + 1);
                endDate = new Date(nowYear, nowMonth, nowDay + (7 - nowDayOfWeek) + 7);
                break;
            case "7":
                startDate = new Date(nowYear, nowMonth + 1, 1);
                endDate = new Date(nowYear, nowMonth + 1, $tiansu.date.getMonthDays(nowYear, nowMonth + 1));
                break;
        }
        //返回本周，月，年的起止时间	
        startDate = $tiansu.date.format({ date: startDate, connect: options.connect });
        endDate = $tiansu.date.format({ date: endDate, connect: options.connect });
        //参数如果传起或止时间则优先级较高
        if (options.startDate && options.startDate > startDate) {
            startDate = options.startDate;
        }
        if (options.endDate && options.endDate < endDate) {
            endDate = options.endDate;
        }
        options.sDate.val(startDate);
        options.eDate.val(endDate);
    }
};
// 正则表达式
$tiansu.regexp = {
    /** 
    * 校验是否是邮箱
    * author: ghj
    * time: 2013-11-13 14:11:30
    */
    isMail: function (s) {
        var reg = /^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/;
        return reg.test(s);
    },

    /** 
    * 校验是否是手机号码
    * author: ghj
    * time: 2013-11-13 14:25:16
    */
    isMobile: function (s) {
        var reg = /^(13[0-9]{9})|(18[0-9]{9})|(14[7][0-9]{8})|(15[0-9]{9})$/;
        return reg.test(s);
    },

    /** 
    * 校验是否是数字
    * author: pl
    * time: 2013-3-23
    */
    isNum: function (s) {
        var reg = /[-|+]?\d+(?=\.{0,1}\d+$|$)/;
        return reg.test(s);
    },

    /** 
    * 校验是否是正数(包括0)
    * author: ghj
    * time: 2013-12-29 22:48:24
    */
    isPositive: function (s) {
        var reg = /^\d+(?=\.{0,1}\d+$|$)/;
        return reg.test(s);
    },

    /** 
    * 校验身份证号码
    * author: ghj
    * time: 2013-12-29 22:48:24
    */
    isCardNo: function (s) {
        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        return reg.test(s);
    },

    /** 
    * 校验时分
    * author: pl
    * time: 2013-2-28 17:00:24
    */
    isTime: function (s) {
        var reg = /^([0-1]?[0-9]|2[0-3]):([0-5]?[0-9])$/;
        return reg.test(s);
    }

};

new $tiansu.common.templateTag();
new $tiansu.common.sidebar();
new $tiansu.common.quickLink();
new $tiansu.common.setTreeHeight();
//$tiansu.common.treeSelect();
$tiansu.common.queryBtnPos();
$tiansu.common.fullScreenCtrl(".hd-fullscreen>a");
