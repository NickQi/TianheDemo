var cpage = 0;
var ntsAjax = function () { } .prototype = {
    CustomAjax: function (serverUrl, serverData, callback) {
        $.ajax({
            url: serverUrl,
            dataType: "json",
            type: "POST",
            data: { Inputs: serverData },
            success: function (json) {
                callback(json);
            },
            error: function () {

            }
        });
    }
};
    function importPage() {
        this.init();
    }

    importPage.prototype = {

    /*
        ** 初始化页面
        */
    init: function() {

        this.initDevice();
        this.render();
    },

    /*
        ** Render page
        */
    render: function() {

        this.QueryHistory();

    },

    initDevice: function() {
        $("#ImportDeviceList,#deviceLabel").hide();
    },
    QueryHistory: function() {
        $("#btnQuery").click(function() {
            if ($("#TreeObjectId").val() == '') {
                alert("请选择导入的对象。");
                return;
            }
            // 加载设备列表
            imports.ShowDeviceList($("#TreeObjectId").val());

            // 显示历史的数值
            imports.ShowHistoryValue();
        });

        $("#ImportIsArea").change(function() {
            if ($(this).val() == '0') {
                $("#ImportDeviceList,#deviceLabel").show();
            } else {
                $("#ImportDeviceList,#deviceLabel").hide();
            }

        });

        $("#btnSave").click(function () {
            if ($("#TreeObjectId").val() == '') {
                alert("请选择导入的对象。");
                return;
            }
             imports.SaveEnery();
        });

        // 显示历史记录
        this.ShowHistoryList(1, true);
    },
    SaveEnery: function() {
        var datas = {
            ItemCode: $("#ItemCode").val(),
            IsArea: $("#ImportIsArea").val(),
            DeviceId: $("#ImportDeviceList").val(),
            DateUnit: $("#DateUnit").val(),
            ObjectId: $("#TreeObjectId").val(),
            IsAreaTree: $("#TreeInfo").val(),
            Starttime: this.getqueryTime($("#DateUnit").val()),
            ObjectValue: $("#ObjectValue").val()
        };
        datas = JSON.stringify(datas);
        ntsAjax.CustomAjax(this.getAction().saveObjectEnery, datas, function(json) {
            if (json) {
                alert(json.MessageContent);
                $("#ObjectValue").val('');
            }
        });
    },
    ShowHistoryValue: function() {
        var data = {
            ItemCode: $("#ItemCode").val(),
            IsArea: $("#ImportIsArea").val(),
            DeviceId: $("#ImportDeviceList").val(),
            DateUnit: $("#DateUnit").val(),
            ObjectId: $("#TreeObjectId").val(),
            IsAreaTree: $("#TreeInfo").val(),
            Starttime: this.getqueryTime($("#DateUnit").val())
        };

        data = JSON.stringify(data);
        ntsAjax.CustomAjax(this.getAction().getObjectEnery, data, function(json) {
            if (json) {
                $("#eneryValue").html(json.Total == 0 ? '暂无数据' : json.Total);
            }
        });
        //eneryValue
    },
    getqueryTime: function(unit) {
        switch (Number(unit)) {
        case 1:
            return $("#Year").val() + '-' + $("#Month").val() + '-1';
        case 2:
            return $("#AddDate").val();
        default:
            return $("#AddDate").val() + ' ' + $("#Times").val() + ':00:00';
        }
    },
    ShowHistoryList: function(page, flag) {

        var data = {
            ItemCode: $("#ItemCode").val(),
            IsArea: $("#ImportIsArea").val(),
            DeviceId: $("#ImportDeviceList").val(),
            DateUnit: $("#QueryDateUnit").val(),
            ObjectId: $("#TreeObjectId").val() == '' ? null : $("#TreeObjectId").val(),
            Starttime: $("#sdate").val(),
            Endtime: $("#edate").val(),
            PaddingInfo: { Page: page, PageSize: 10 }
        };
        data = JSON.stringify(data);
        var jsonpadding;
        ntsAjax.CustomAjax(this.getAction().getHistoryList, data, function(json) {
            if (json.Success) {
                jsonpadding = json.PageInfo;
                var $html = template.render("HistoryList", json);
                var $list = jQuery(".table tbody");
                $list.html($html);
                if (flag) {
                    imports.PagePadding(jsonpadding);
                }
                cpage = $("#pages").val();
            } else {
                alert(json.ErrorMsg);
            }

        });


    },
    PagePadding: function(pageInfo) {
        $("#pagination").ntspadding({
            recordnum: pageInfo.Total,
            count: pageInfo.Pages,
            onChange: function(data) {
                imports.ShowHistoryList(data.page, false);
            }
        });
    },
    ExcelImport: function() {

        //开始提交
        $("#upexcel").ajaxSubmit({
            success: function (data, textStatus) {
                if (data.Success) {
                    $("#uploadmsg").html(data.MsgContent);
                } else {
                    alert(data.MsgContent);
                }
            },
            error: function (data, status, e) {
                alert("上传失败，错误信息：" + e);
            },
           // url: '/import.ashx',
            url: this.getAction().excelImport,
            type: "post",
            dataType: "json",
            timeout: 600000
        });
        
    },
    ShowDeviceList: function(areaid) {
            var $thisImportDeviceList = $("#ImportDeviceList");
            var treeInfo = $("#TreeInfo").val();
            var itemCode = $("#ItemCode").val();
            var data = { treeInfo: treeInfo, areaid: areaid, itemCode: itemCode };
            data = JSON.stringify(data);
            $thisImportDeviceList.html('');
            $thisImportDeviceList.append("<option value='0'>请选择设备...</option>");
            ntsAjax.CustomAjax(this.getAction().getDeviceList, data, function (json) {
                if (json) {
                    for (var i = 0; i < json.length; i++) {
                        
                        $thisImportDeviceList.append("<option value=" + json[i].DeviceID + ">" + json[i].DeviceName + "</option>");
                    }
                }
            });
        },
        /**
        * Ajax request
        */
        getAction: function () {
            return {
                getDeviceList: 'action.ashx?action=GetDeviceListByAreaId',
                getObjectEnery: 'action.ashx?action=getObjectEnery',
                saveObjectEnery: 'action.ashx?action=saveObjectEnery',
                getHistoryList: 'action.ashx?action=getHistoryList',
                excelImport: 'action.ashx?action=excelImport'
            };
        }
    };


    var imports = new importPage();



    $(function() {
        $("#ImportDeviceList").change(function() {
            if ($(this).val() > 0) {
                imports.ShowHistoryValue();
            }
        });

        $("#ExcelBtn").click(function () {
            imports.ExcelImport();
        });

        $("#btnQueryLogs").click(function () {
            var startTime = $("#sdate").val();
            var endTime = $("#edate").val();
            if (startTime != '' && endTime != '') {
                if (endTime < startTime) {
                    alert("日期选择有误（结束日期小于开始日期)!");
                    return;
                }
            }
            imports.ShowHistoryList(1, true); 
        });
    });


