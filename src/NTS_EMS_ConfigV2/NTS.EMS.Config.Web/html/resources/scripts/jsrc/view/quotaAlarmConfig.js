/**
* ==========================================================
* Copyright (c) 2013, NTS-9000 All rights reserved.
* NTS项目用户登录JS
* Author: Jinsam
* Date: 2013-05-17 22:43:55 502000
* ==========================================================
*/

function Data() {
    this.percent = $.trim($("#percent").val());
    this.objectDesc = $("#TreeObjectName").val();
    this.objectId = $("#TreeObjectId").val();
    this.itemCode = $("#item").val();
    this.id = $("#id").val();
    this.objectType = $("#TreeInfo").val();
    this.alarmType = $("#alarmType").val();
    this.quotaType = (function () {
        var radioVal = document.getElementsByName("quotaType");
        for (var i = 0; i < radioVal.length; i++) {
            if (radioVal[i].checked) {
                return radioVal[i].value;
            }
        }
        return 0;
    })();

}


function LoadPage() {

}

LoadPage.prototype = {
    pagesize: 10,
    itemCodeS: "",
    objectNameS: "",
    alarmTypeS: 0,
    quotaTypeS: 0,
    pageCurrentS: 1,


    /**
    * 初始化页面
    * author: wyl
    */
    init: function () {
        this.render();
        this.search(1, false);
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
            var _saveBtn = $("#btn_save"),
                _deleteBtn = $("#btn_delete"),
                _searchBtn = $("#btn_search");
            var _treeOid = document.getElementById("TreeObjectId");

            // 保存按钮
            _saveBtn.click(self.saveData);
            //删除
            _deleteBtn.click(function () { var id = $("#id").val(); self.deleteData(id); });
            //查询
            _searchBtn.click(function () {
                self.itemCodeS = $("#itemS").val();
                self.objectNameS = $.trim($("#objectNameS").val());
                self.alarmTypeS = $("#alarmTypeS").val();
                self.quotaTypeS = $("#quotaTypeS").val();
                self.pageCurrentS = 1;
                self.search(1, false);
            });

            //change事件
            $(".changeEvent").change(self.loadData);
            //            if (!!(document.all && navigator.userAgent.indexOf('Opera') === -1)) {//ie
            //                _treeOid.onpropertychange = self.loadData;
            //            } else {
            //                _treeOid.addEventListener("input", function () { alert(111); }, false);
            //            }
            $("#btn_TreeClick").click(function () {
                var nameS = $("#TreeObjectName").val();
                $("#objectNameS").val(nameS);
                self.loadData();
            });
            $("input[name='quotaType']").click(self.loadData);

            //（表格）删除事件
            $('span.dialog-delete').live('click', function () {
                var quotaAlarmId = $(this).parent().next().text();
                if (quotaAlarmId != 0 && quotaAlarmId != "") {
                    self.deleteData(quotaAlarmId);
                }
            });

            //表格 修改事件
            $('span.dialog-edit').live('click', function () {
                var tr = $(this).parent();
                var tempId = tr.next().text();
                var tempAlarmType = tr.next().next().text();
                var tempQuotaType = tr.next().next().next().text();
                var tempItemCode = tr.next().next().next().next().text();
                var tempObjectId = tr.next().next().next().next().next().text();
                var tempObjectType = tr.next().next().next().next().next().next().text();
                var tempPercent = tr.prev().text();

                //移除change事件
                $('.changeEvent').unbind("change");
                $("input[name='quotaType']").unbind("change");

                //设置值
                $("#id").val(tempId);
                $("#alarmType").val(tempAlarmType);
                $("#item").val(tempItemCode);
                $("input:radio[value='" + tempQuotaType + "']").attr("checked", "true");
                var tree = "";
                if (tempObjectType == 31) {
                    $(".links li:first a").trigger("click");
                    tree = jQuery.fn.zTree.getZTreeObj("LeftTree");

                } else if (tempObjectType == 32) {
                    $(".links li:last a").trigger("click");
                    tree = jQuery.fn.zTree.getZTreeObj("LeftAreaTree");
                }
                var node = tree.getNodeByParam("id", tempObjectId);
                tree.selectNode(node, false);
                $("#TreeObjectName").val(tree.getSelectedNodes()[0].name);
                $("#TreeObjectId").val(tempObjectId);

                $("#percent").val(tempPercent);
                $("#btn_save").removeAttr("disabled");
                //在添加change事件
                $("input[name='quotaType']").click(self.loadData);
                $(".changeEvent").change(self.loadData);
            });
        }
    },

    /** 
    * 保存
    */
    saveData: function () {
        var inputData = new Data();
        if (inputData.objectId == "") {
            alert("请选择对象！");
            return;
        }
        if (inputData.alarmType == "") {
            alert("请选择告警类型！");
            return;
        }
        if (inputData.precent === "") {
            alert("请输入百分比！");
            return;
        }
        else {
            if (inputData.objectType == 1) {//业态
                inputData.objectType = 31;
            }
            else if (inputData.objectType == 2) {//区域
                inputData.objectType = 32;
            }
            inputData.percent = inputData.percent / 100;
            //输入json
            var data = {
                AlarmType: inputData.alarmType,
                QuotaType: inputData.quotaType,
                ItemCode: inputData.itemCode,
                ObjectId: inputData.objectId,
                ObjectType: inputData.objectType,
                ObjectDesc: inputData.objectDesc,
                Percent: inputData.percent,
                Id: inputData.id
            };
            data = JSON.stringify(data);
            $.ajax({
                url: window.loadPage.getAction().SaveQuotaAlarm,
                dataType: "json",
                type: "POST",
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                data: { "Inputs": data },
                success: function (json) {
                    if (json.Success) {
                        //刷新列表
                        var prevId = $("#id").val();
                        if (prevId == "" || prevId == 0) {
                            $("#itemS").val("");
                            $("#alarmTypeS").val("0");
                            $("#quotaTypeS").val("0");
                            loadPage.itemCodeS = "";
                            loadPage.objectNameS = "";
                            loadPage.alarmTypeS = 0;
                            loadPage.quotaTypeS = 0;
                            loadPage.pageCurrentS = 1;
                            loadPage.search(1, false);
                        }
                        else {
                            loadPage.search(loadPage.pageCurrentS, true);
                        }

                        alert("保存成功！");
                        $("#id").val(json.ExtendContent);
                    }
                },
                error: function (json) {
                    alert("服务器错误，请联系管理员！");
                }
            });
        }
    },

    /** 
    * 删除
    */
    deleteData: function (id) {
        //var id = $("#id").val();
        //只有id不为空时可删除
        if (id == "" || id == 0) {
            alert("无数据可删除！");
            return;
        }
        if (confirm("确认删除数据？")) {
            $.ajax({
                url: window.loadPage.getAction().DeleteQuotaAlarm,
                dataType: "json",
                type: "POST",
                data: { "Inputs": id },
                success: function (json) {
                    if (json.Success) {
                        loadPage.search(loadPage.pageCurrentS, false);
                        alert("删除成功！");
                        $("#id").val("");
                        $("#percent").val("");
                    }
                },
                error: function (json) {
                    alert("服务器错误，请联系管理员！");
                }
            });
        }
    },

    /** 
    * 加载
    */
    loadData: function () {
        var inputData = new Data();
        if (inputData.objectId == "") {
            // alert("请选择对象！");
            return;
        }
        if (inputData.alarmType == "") {
            alert("请选择告警类型！");
            return;
        }
        else {
            if (inputData.objectType == 1) {//业态
                inputData.objectType = 31;
            }
            else if (inputData.objectType == 2) {//区域
                inputData.objectType = 32;
            }
            //输入json
            var data = {
                AlarmType: inputData.alarmType,
                QuotaType: inputData.quotaType,
                ItemCode: inputData.itemCode,
                ObjectId: inputData.objectId,
                ObjectType: inputData.objectType
            };
            data = JSON.stringify(data);
            $.ajax({
                url: window.loadPage.getAction().GetQuotaAlarmInfo,
                dataType: "json",
                type: "POST",
                data: { "Inputs": data },
                success: function (json) {
                    if (json.ResultInfo.Success) {
                        $("#btn_save").removeAttr("disabled");
                        $("#id").val(json.QuotaAlarm.Id);
                        if (json.QuotaAlarm.Percent != 0) {
                            $("#percent").val(Number(json.QuotaAlarm.Percent * 100).toFixed(2));
                        } else {
                            $("#percent").val("");
                        }

                    }
                },
                error: function (json) {
                    alert("服务器错误，请联系管理员！");
                }
            });
        }
    },

    search: function (page, flag) {
        //flag ---是否是分页查询
        //page ---页码
        var data = {
            ObjectName: this.objectNameS,
            AlarmType: this.alarmTypeS,
            QuotaType: this.quotaTypeS,
            ItemCode: this.itemCodeS,
            PageCurrent: page,
            PageSize: this.pagesize
        };
        data = JSON.stringify(data);
        $.ajax({
            url: window.loadPage.getAction().GetQuotaAlarmList,
            dataType: "json",
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { "Inputs": data },
            success: function (json) {
                //是否查询成功
                if (json.ResultInfo.Success) {
                    loadPage.showDataList(json);
                } else {
                    alert(json.ResultInfo.ExceptionMsg);
                }

                //是否是分页查询（flag true 是)
                if (!flag) {
                    if (json.Page != null) {
                        loadPage.setPadding(json.Page.Total);
                    }
                    else {
                        loadPage.setPadding(0);
                    }
                }
                else {
                    loadPage.resetPadding(json.Page.Total, json.Page.Current);
                    loadPage.pageCurrentS = json.Page.Current;
                }
            },
            error: function (json) {
                alert("服务器错误，请联系管理员！");
            }
        });
    },

    /** 
    * 呈现列表
    */
    showDataList: function (json) {
        jQuery(".table tbody").empty();
        var $list = jQuery(".table tbody"),
		    html = "";
        if (json.QuotaAlarmList != null && json.QuotaAlarmList.length > 0) {
            html = template.render("dataItem", json);
            $list.html(html);
        } else {
            // self.common.loading($list, false);
        }
    },

    /** 
    * 设置分页
    */
    setPadding: function (total) {
        var countNum = Math.ceil(total / this.pagesize);
        jQuery("#padding").ntspadding({
            recordnum: total,
            count: countNum,
            onChange: function (data) {
                loadPage.search(data.page, true);
            }
        });
    },

    /** 
    * 重新设置分页总数（保持在page页)
    */
    resetPadding: function (total, page) {
        $.ReSetting({
            recordnum: total,
            count: Math.ceil(total / this.pagesize),
            page: page
        });
    },

    /**
    * Ajax请求
    */
    getAction: function () {
        return {
            GetQuotaAlarmList: "action.ashx?action=GetQuotaAlarmList",
            GetQuotaAlarmInfo: "action.ashx?action=GetQuotaAlarmInfo",
            SaveQuotaAlarm: "action.ashx?action=SaveQuotaAlarm",
            DeleteQuotaAlarm: "action.ashx?action=DeleteQuotaAlarm"
        };
    }
};

var loadPage = new LoadPage();
loadPage.init();


$(function () {
    $("#percent").live("keyup", function isNumber(e) {
        if ($(this).val() != '') {
            var temp = $(this).val();
            if ($(this).val().length > 1 && $(this).val().substring(0, 1) == "0" && $(this).val().indexOf('.') == -1) {
                //alert('输入的数值格式不正确！');
                $(this).val(temp.substring(0, temp.length - 1));
            }

            //小数后两位
            var post = temp.indexOf(".");
            if (post >= 0) {
                if (temp.substring((post + 1)).length > 2) {
                    $(this).val(temp.substring(0, post + 3));
                }
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