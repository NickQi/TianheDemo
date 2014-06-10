
function loadPage() {
    //this.init();
}

loadPage.prototype = {
    pagesize: 10,
    itemcodeid: 0,
    currpage: 1,
    deviceName: "",
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

        }
    },

    getinfo: function () {
        this.searchAction(1, false);
    },

    searchAction: function (currentPage, isPadding) {

        var data = {
            ItemCodeId: this.itemcodeid,
            DeviceName: this.deviceName,
            PageCurrent: currentPage,
            PageSize: this.pagesize
        };

        data = JSON.stringify(data);
        jQuery.ajax({
            url: this.getAction().GetDevicePropInfo,
            dataType: "json",
            type: "POST",
            data: { "Inputs": data },
            success: function (json) {
                if (json.ResultInfo.Success) {
                    deviceProp.showDataList(json);
                } else {
                    alert(json.ResultInfo.ExceptionMsg);
                }
                if (!isPadding) {
                    if (json.Page != null) {
                        deviceProp.setPadding(json.Page.Total);
                    }
                    else {
                        deviceProp.setPadding(0);
                    }
                }
                else {
                    deviceProp.resetPadding(json.Page.Total, json.Page.Current);
                }
            },
            error: function (msg) {
                alert("服务器错误，请联系管理员！--411");
            }
        });

    },

    showDataList: function (json) {
        jQuery(".table tbody").empty();
        var $list = jQuery(".table tbody"),
		    html = "";
        if (json.DevicePropList != null && json.DevicePropList.length > 0) {
            html = template.render("dataItem", json);
            $list.html(html);
        } else {
            // self.common.loading($list, false);
        }
    },

    setPadding: function (total) {
        var countNum = Math.ceil(total / this.pagesize);
        jQuery("#padding").ntspadding({
            recordnum: total,
            count: countNum,
            onChange: function (data) {
                deviceProp.currpage = data.page;
                deviceProp.searchAction(data.page, true);
            }
        });
    },

    resetPadding: function (total, page) {
        $.ReSetting({
            recordnum: total,
            count: Math.ceil(total / this.pagesize),
            page: page
        });
    },

    setInfo: function (json) {

    },



    /**
    * Ajax请求
    */
    getAction: function () {
        return {
            GetDevicePropInfo: "action.ashx?action=GetDevicePropInfo",
            SaveDeviceProp: "action.ashx?action=SaveDeviceProp"
        };
    }
};


var deviceProp = new loadPage();

$(function () {

    $("#btnSave").click(function () {
        var areaId = $("#TreeObjectId").val(),
                    areaType = $("#TreeInfo").val();
        if (areaType == 1) {
            areaType = 2;
        } else if (areaType == 2) {
            areaType = 1;
        }
        if (areaId == "") {
            alert("请选择对象!");
            return;
        }
        var arr = new Array();
        $(".cksimple:checked").each(function () {
            var id = $(this).parent().next().text();
            arr.push(id);
        });
        if (arr.length == 0) {
            alert("请选择设备！");
            return;
        }
        else {

            //输入json
            var data = {
                AreaId: areaId,
                AreaType: areaType,
                ItemCodeId: deviceProp.itemcodeid,
                DeviceIds: arr
            };
            data = JSON.stringify(data);
            $.ajax({
                url: deviceProp.getAction().SaveDeviceProp,
                dataType: "json",
                type: "POST",
                data: { "Inputs": data },
                success: function (json) {
                    if (json !== -1) {
                        deviceProp.searchAction(deviceProp.currpage, true);
                        alert("保存成功.");
                    }
                },
                error: function (json) {
                    alert("服务器错误，请联系管理员！---2");
                }
            });
        }
    });

    $("#btnQuery").click(function () {
        deviceProp.itemcodeid = $("#item").val();
        deviceProp.currpage = 1;
        deviceProp.deviceName = $("#deviceName").val();
        deviceProp.searchAction(1, false);
    });
    deviceProp.getinfo();
});

