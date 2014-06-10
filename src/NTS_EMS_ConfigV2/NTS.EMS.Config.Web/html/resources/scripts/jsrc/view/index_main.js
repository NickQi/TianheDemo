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
        this.setPadding(0);
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
            var _planNumber = $("#planNumber"),
                    _quotaId = $("#quotaId"),
					_startTime = $("#date1"),
     				_endTime = $("#date2"),
					_saveBtn = $("#btn_save"),
                _searchBtn = $("#btn_search");
            _loadBtn = $("#btn_load");

            _loadBtn.click(function () {
                _planNumber.val(0);
                _quotaId.val(0);
                _startTime.val("");
                _endTime.val("");
                $("#startTime").val("1900-01-01");
                $("#endTIme").val("1900-01-01");
                self.getQuotaInfo();
            });

            // 保存按钮
            _saveBtn.click(function () {
                var planNumber = $.trim(_planNumber.val());
                var quotaId = $.trim(_quotaId.val());

                var deviceDesc = $("#TreeObjectName").val();
                var deviceId = $("#TreeObjectId").val(),
                        itemCode = $("#item").val(),
                        plandDate = $("#Year").val(),
                        quotaType = 0;
                var radioVal = document.getElementsByName("quotaType");
                for (var i = 0; i < radioVal.length; i++) {
                    if (radioVal[i].checked) {
                        quotaType = radioVal[i].value;
                        break;
                    }
                }
                if (quotaType == 0) {
                    alert("请选择定额类型！");
                    return;
                }

                if (deviceId == "") {
                    alert("请选择定额对象！");
                    return;
                }
                if (quotaType == 1) {
                    plandDate += "-" + $("#Month").val() + "-01";
                } else if (quotaType == 2) {
                    plandDate += "-01" + "-01";
                }


                if (planNumber === "" || planNumber === 0.00) {
                    new LoadPage().showMsg("请输入定额值。");
                }
                else {
                    //输入json
                    var data = {
                        QuotaId: quotaId,
                        ObjectType: 32,
                        ObjectId: deviceId,
                        ObjectDesc: deviceDesc,
                        QuotaType: quotaType,
                        QuotaValue: planNumber,
                        Reserved: "",
                        ItemCode: itemCode,
                        QuotaDate: plandDate
                    };
                    data = JSON.stringify(data);
                    $.ajax({
                        url: window.loadPage.getAction().saveDataUrl,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { "Inputs": data },
                        success: function (json) {
                            if (json !== -1) {
                                _quotaId.val(json);
                                var startTime = $("#startTime").val();
                                var endTime = $("#endTime").val();

                                var quotaId = $.trim(_quotaId.val());
                                var data = {
                                    QuotaId: quotaId,
                                    StartTime: startTime,
                                    EndTime: endTime,
                                    PageCurrent: 1,
                                    PageSize: self.pageSize
                                };
                                data = JSON.stringify(data);
                                $.ajax({
                                    url: window.loadPage.getAction().getQuotaLogUrl,
                                    dataType: "json",
                                    type: "POST",
                                    data: { "Inputs": data },
                                    success: function (json) {
                                        loadPage.showQuotaLog(json);
                                        if (json.Page != null) {
                                            loadPage.setPadding(json.Page.Total);
                                        }
                                        else {
                                            loadPage.setPadding(0);
                                        }
                                    }
                                });
                                alert("保存成功。");
                            }
                        },
                        error: function (json) {
                            alert("服务器错误，请联系管理员！---2");
                        }
                    });
                }
            });


            _searchBtn.click(function () {
                var startTime = _startTime.val();
                var endTime = _endTime.val();
                if (startTime != "" && endTime != "") {
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

                var quotaId = $.trim(_quotaId.val());
                var data = {
                    QuotaId: quotaId,
                    StartTime: $("#startTime").val(),
                    EndTime: $("#endTime").val(),
                    PageCurrent: 1,
                    PageSize: self.pageSize
                };
                data = JSON.stringify(data);
                $.ajax({
                    url: window.loadPage.getAction().getQuotaLogUrl,
                    dataType: "json",
                    type: "POST",
                    data: { "Inputs": data },
                    success: function (json) {
                        loadPage.showQuotaLog(json);
                        if (json.Page != null) {
                            loadPage.setPadding(json.Page.Total);
                        }
                        else {
                            loadPage.setPadding(0);
                        }
                    },
                    error: function (json) {
                        alert("服务器错误，请联系管理员！");
                    }
                });
            });

        }
    },


    /** 
    * 获取json用户信息
    * author: ghj
    * time: 2013-05-18 23:08:35
    */
    getQuotaInfo: function () {
        var _planNumber = $("#planNumber");
        var _quotaId = $("#quotaId");

        var deviceId = $("#TreeObjectId").val(),
                itemCode = $("#item").val(),
                plandDate = $("#Year").val(),
                quotaType = 0;
        var radioVal = document.getElementsByName("quotaType");
        for (var i = 0; i < radioVal.length; i++) {
            if (radioVal[i].checked) {
                quotaType = radioVal[i].value;
                break;
            }
        }
        if (quotaType == 0) {
            alert("请选择定额类型！");
            return;
        }

        if (deviceId == "") {
            alert("请选择定额对象！");
            return;
        }
        if (quotaType == 1) {
            plandDate += "-" + $("#Month").val() + "-1";
        } else if (quotaType == 2) {
            plandDate += "-01" + "-01";
        }
        loadPage.setTitle();
        var data = {
            ObjectId: deviceId,
            Date: plandDate,
            QuotaType: quotaType,
            ItemCode: itemCode,
            PageSize: this.pageSize
        };

        data = JSON.stringify(data);
        $.ajax({
            url: this.getAction().loadPageUrl,
            dataType: "json",
            type: "POST",
            data: { "Inputs": data },
            success: function (json) {
                if (json != null && json.QuotaData != null) {
                    _planNumber.val(json.QuotaData.QuotaValue);
                    _quotaId.val(json.QuotaData.QuotaId);
                }
                loadPage.showQuotaLog(json);
                $("#btn_save").removeAttr("disabled");
                if (json.Page != null) {
                    loadPage.setPadding(json.Page.Total);
                }
                else {
                    loadPage.setPadding(0);
                }
            },
            error: function (msg) {
                alert("服务器错误，请联系管理员！--4");
            }
        });
    },

    showQuotaLog: function (json) {
        $(".table tbody").empty();
        var $list = $(".table tbody"),
				html = "";
        if (json.QuotaLogList != null && json.QuotaLogList.length > 0) {
            html = template.render("quotaLogItem", json);
            $list.html(html);

        } else {
            // self.common.loading($list, false);
        }
    },

    setPadding: function (total) {
        var countNum = Math.ceil(total / this.pageSize);
        var quotaId = $.trim($("#quotaId").val());
        var startTime = $("#startTime").val();
        var endTime = $("#endTime").val();
        $("#padding").ntspadding({
            recordnum: total,
            count: countNum,
            onChange: function (data) {
                var data1 = {
                    QuotaId: quotaId,
                    StartTime: startTime,
                    EndTime: endTime,
                    PageCurrent: data.page,
                    PageSize: data.pagesize
                };
                data = JSON.stringify(data1);
                $.ajax({
                    url: window.loadPage.getAction().getQuotaLogUrl,
                    dataType: "json",
                    type: "POST",
                    data: { "Inputs": data },
                    success: function (json) {
                        loadPage.showQuotaLog(json);
                    },
                    error: function (json) {
                        alert("服务器错误，请联系管理员！");
                    }
                });
            }
        });
    },

    setTitle: function () {
        $(".labUnit").empty();
        var unit = $("#item option:selected").attr("name");
        $(".labUnit").each(function () { $(this).text(unit); });
        var radioVal = document.getElementsByName("quotaType");
        var itemName = $("#item option:selected").text().replace("├└", "").replace("├", "");
        for (var i = 0; i < radioVal.length; i++) {
            if (radioVal[i].checked) {
                var value = radioVal[i].value;
                if (value == 1) {
                    var objectName = $("#TreeObjectName").val();
                    //$("#tempObjectDesc").val(objectName);
                    var date = $("#Year").val() + "年" + $("#Month").val() + "月";
                    $("#showTitle").html(objectName + " " + date + itemName + " 月定额配置");
                }
                else if (value == 2) {
                    var objectName = $("#TreeObjectName").val();
                    // $("#tempObjectDesc").val(objectName);
                    var date = $("#Year").val() + "年";
                    $("#showTitle").html(objectName + " " + date + itemName + " 年定额配置");
                }
                break;
            }
        }
    },
    /**
    * Ajax请求
    */
    getAction: function () {
        return {
            loadPageUrl: "action.ashx?action=GetQuotaInfo",
            saveDataUrl: "action.ashx?action=DealQuota",
            getQuotaLogUrl: "action.ashx?action=GetQuotaLogs"
        };
    }
};

var loadPage = new LoadPage();
loadPage.init();
$(function () {
    $("#planNumber").live("keyup", function isNumber(e) {
        if ($(this).val() != '') {
            var temp = $(this).val();
            if ($(this).val().length > 1 && $(this).val().substring(0, 1) == "0" && $(this).val().indexOf('.') == -1) {
                $(this).val(temp.substring(0, temp.length - 1));
            }
        }
    });

});


function isNumber(e, obj, withFraction) {
    if (!e) {
        e = window.event;
    }
    if (e.which) {
        key = e.which;
    } else {
        key = e.keyCode;
    }
    var validNumber = false;
    if (key == 8 || key == 46 || key >= 35 && key <= 37 || key == 39) {
        validNumber = true;
    }
    if (!e.shiftKey) {
        //only check shift is not pressed
        if (withFraction && obj.value.length > 0 && (key == 190 || key == 110) && obj.value.indexOf(".") == -1) {
            validNumber = true;
        }
        if ((key == 48 || key == 96) && (obj.value.length == 0 || (obj.value.length > 0 && obj.value > 0))) { // 0
            validNumber = true;
        }
        if ((key >= 49 && key <= 57) || (key >= 97 && key <= 105)) { // 1~9
            validNumber = true;
        }
    }
    if (!validNumber) {
        if (e.preventDefault) {
            e.preventDefault();
        } else {
            e.returnValue = false;
        }
    }
}