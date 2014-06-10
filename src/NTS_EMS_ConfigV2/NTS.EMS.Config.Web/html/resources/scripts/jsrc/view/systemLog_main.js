/**
* ==========================================================
* Copyright (c) 2013, NTS-9000 All rights reserved.
* NTS项目用户登录JS
* Author: Jinsam
* Date: 2013-05-17 22:43:55 502000
* ==========================================================
*/
function LoadPage() {

}

LoadPage.prototype = {
    pageSize: 10,
    /**
    * 初始化页面
    * author: wyl
    */
    init: function () {
        this.render();
    },

    /**
    * 渲染页面
    */
    render: function () {
        this.bindEvent.loadPagePanel.call(this);
    },

    /**
    * 页面事件绑定
    */
    bindEvent: {

        /** 
        * 保存、查询事件
        */
        loadPagePanel: function () {

            var self = this;
            _startTime = $("#date1"),
     				_endTime = $("#date2"),
                _searchBtn = $("#btn_search");
            _expprtBtn = $("#btn_Export");

            _searchBtn.click(function () {
                var startTime = _startTime.val();
                var endTime = _endTime.val();
                if (startTime != "1900-01-01" && endTime != "1900-01-01") {
                    if (endTime < startTime) {
                        alert("日期选择有误（结束日期小于开始日期)!");
                        return;
                    }
                }
                if (startTime != "") {
                    $("#startTime").val(_startTime.val());
                }
                if (endTime != "") {
                    $("#endTime").val(endTime);
                }
                self.searchAction(1, false);

            });

            _expprtBtn.click(function () {
                self.exporthAction();
            });
        }
    },

    getinfo: function () {
        this.searchAction(1, false);
    },

    searchAction: function (currentPage, isPadding) {
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        if (startTime != "1900-01-01" && endTime != "1900-01-01") {
            if (endTime < startTime) {
                alert("日期选择有误（结束日期小于开始日期)!");
                return;
            }
        }
        var operator = $("#operator").val(),
                model = $("#model").val();
        var data = {
            StartTime: startTime,
            EndTime: endTime,
            OperatorName: operator,
            ModelName: model,
            PageCurrent: currentPage,
            PageSize: this.pageSize
        };

        data = JSON.stringify(data);
        $.ajax({
            url: this.getAction().getSystemLogUrl,
            dataType: "json",
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { "Inputs": data },
            success: function (json) {
                loadPage.showSystemLog(json);
                // $("#totalNum").val(json.Page.Total);
                if (!isPadding) {
                    if (json.Page != null) {
                        loadPage.setPadding(json.Page.Total);
                    }
                    else {
                        loadPage.setPadding(0);
                    }
                }
                else {
                    loadPage.resetPadding(json.Page.Total, json.Page.Current);
                }
            },
            error: function (msg) {
                alert("服务器错误，请联系管理员！--4");
            }
        });

    },

    exporthAction: function () {
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        if (startTime != "1900-01-01" && endTime != "1900-01-01") {
            if (endTime < startTime) {
                alert("日期选择有误（结束日期小于开始日期)!");
                return;
            }
        }
        var operator = $("#operator").val(),
                model = $("#model").val();
        var data = {
            StartTime: startTime,
            EndTime: endTime,
            OperatorName: operator,
            ModelName: model,
            PageCurrent: 1,
            PageSize: this.pageSize
        };

        data = JSON.stringify(data);
        $.ajax({
            url: this.getAction().exportSysLogUrl,
            dataType: "json",
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { "Inputs": data },
            success: function (json) {
                if (json.status == "success") {
                    var end = document.location.href.indexOf(document.location.pathname);
                    var root = document.location.href.substring(0, end);
                    var path = root + json.msg;
                    //var execel = window.open(path);
                    //document.execCommand("SaveAs", true, path);
                    window.location.href = path;
                    //execel.close();
                }
                else {
                    alert(json.msg);
                }
            },
            error: function (msg) {
                alert("服务器错误，请联系管理员！--4");
            }
        });

    },

    showSystemLog: function (json) {
        $(".table tbody").empty();
        var $list = $(".table tbody"),
				html = "";
        if (json.SysLogList != null && json.SysLogList.length > 0) {

            html = template.render("systemLogItem", json);
            $list.html(html);
        } else {
            // self.common.loading($list, false);
        }
    },

    setPadding: function (total) {
        var countNum = Math.ceil(total / this.pageSize);
        $("#padding").ntspadding({
            recordnum: total,
            count: countNum,
            onChange: function (data) {
                loadPage.searchAction(data.page, true);
            }
        });
    },

    resetPadding: function (total, page) {
        $.ReSetting({
            recordnum: total,
            count: Math.ceil(total / loadPage.pageSize),
            page: page
        });
    },
    /**
    * Ajax请求
    */
    getAction: function () {
        return {
            exportSysLogUrl: "action.ashx?action=ExportSysLogExcel",
            getSystemLogUrl: "action.ashx?action=GetSystemLogs"
        };
    }
};

var loadPage = new LoadPage();
loadPage.init();
loadPage.getinfo();