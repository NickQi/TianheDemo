/*********************************************
*
*Author:chenlei
*DateTime:2012-12-14
*Descritpion:Kill IE 6
*
**********************************************/
(function ($) {
    $.extend({
        killie6: function () {
            var firefoxLink = "http://download.firefox.com.cn/releases/webins3.0/official/zh-CN/Firefox-latest.exe";
            var chromeLink = "http://dlc2.pconline.com.cn/filedown_51614_6750119/F09ttWy3/CHRZ_Chrome_non_defaultV5.exe";
            var operaLink = "http://www.opera.com/download/get.pl?id=35318&amp;location=381&nothanks=yes&sub=marine";
            var safariLink = "http://dlc2.pconline.com.cn/filedown_44487_6712048/tNzpSqke/SafariSetup.exe";
            var ie8Link = "http://download.microsoft.com/download/7/5/F/75F4B95A-4F42-4782-BD91-6DE0F7342F5F/IE8-WindowsXP-KB2618444-x86-CHS.exe";

            var ie = (function () {
                var undef = 3, v = 3,
                    div = document.createElement('div'),
                    all = div.getElementsByTagName('i');
                while (div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->',
                all[0]);
                return v > 4 ? v : undef;
            } ());
            if ($.browser.msie && ie < 8) {
                var killwarning = '<style type="text/css">' +
                '#killie6warning{width: 100%;height: 100px;position: absolute;top: 0;left: 0;}' +
                '#killie6warning .killcontent{border: solid 1px #E5C365;background-color: #FCF2CB;height: 108px;}' +
                '#killie6warning .killcontent .killtitle{height: 48px;line-height: 26px;font-size: 13px;font-family: 微软雅黑;color: Green;}' +
                '#killie6warning .killcontent .killtitle ul li b{color: Red;}' +
                '#killie6warning .killcontent .killbrowsers{padding: 5px 0;height: 60px;text-align: center;width: 360px;margin: 0 auto;}' +
                '#killie6warning .killcontent .killbrowsers ul{text-align: center;}' +
                '#killie6warning .killcontent .killbrowsers ul li{float: left;padding: 3px 10px;}' +
                '#killie6warning .killclose{width: 36px;height: 20px;right: 5px;top: 5px;position: absolute;}' +
                '#killie6warning .killclose a{text-decoration: none;cursor:pointer;}' +
                '</style>' +
                '<div id="killie6warning">' +
                '<div class="killcontent">' +
                '<div class="killtitle">' +
                '<ul><li>您正在使用 Internet Explorer 6 或 7 浏览网页，这些陈旧过时的浏览器会导致页面在浏览过程中出现不可预知的问题。</li>' +
                '<li>推荐您使用以下<b>安全</b>、<b>高效</b>、<b>快速</b>的高级浏览器，它可以使您获得更好的浏览体验。</li></ul>' +
                '</div>' +
                '<div class="killbrowsers">' +
                '<ul>' +
                '<li><a id="browserFirefox" target="_blank" title="Firefox"><img alt="火狐浏览器" src="images/browser/firefox.png" /></a></li>' +
                '<li><a id="browserChrome" target="_blank" title="Chrome"><img alt="Chrome浏览器" src="images/browser/chrome.png" /></a></li>' +
                '<li><a id="browserOpera" target="_blank" title="Opera"><img alt="Opera浏览器" src="images/browser/opera.png" /></a></li>' +
                '<li><a id="browserSafari" target="_blank" title="Safari"><img alt="Safari浏览器" src="images/browser/safari.png" /></a></li>' +
                '<li><a id="browserIE8" target="_blank" title="IE8"><img alt="IE8浏览器" src="images/browser/ie8.png" /></a></li>' +
                '</ul>' +
                '</div>' +
                '</div>' +
                '<div class="killclose"><a>[关闭]</a></div>' +
                '</div>';

                $('body').append(killwarning);
                $(".killclose a").bind("click", function () { $("#killie6warning").fadeOut(); });
                $("#browserFirefox").attr("href", firefoxLink);
                $("#browserChrome").attr("href", chromeLink);
                $("#browserOpera").attr("href", operaLink);
                $("#browserSafari").attr("href", safariLink);
                $("#browserIE8").attr("href", ie8Link);
            }
        }
    });
})(jQuery);