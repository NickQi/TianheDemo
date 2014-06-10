var cpage = 0;
var pageSize = 10;
function importPage() {
    this.init();
}

importPage.prototype = {

    /*
    ** 初始化页面
    */
    init: function () {
        this.render();
    },

    /*
    ** Render page
    */
    render: function () {

        this.ShowHistoryList(1, true);
        this.BtnSearch();
    },
    ShowHistoryList: function (page, flag) {
        var startTime = $("#date1").val();
        var endTime = $("#date2").val();
        var areaId = $("#TreeObjectId").val();

        if (startTime != "" && endTime != "") {
            if (endTime < startTime) {
                alert("日期选择有误（结束日期小于开始日期)!");
                return;
            }
        }

        var data = {
            PageCurrent: page,
            PageSize: pageSize
        };

        data = JSON.stringify(data);
        var jsonpadding;

        $.ajax({
            url: "action.ashx?action=GetConfigLog",
            dataType: "json",
            type: "POST",
            data: { "Inputs": data, "StartTime": startTime, "EndTime": endTime, "AreaID": areaId },
            success: function (json) {
                jsonpadding = json.Page;

                $("#divLog tbody").empty();
                var $list = $("#divLog tbody"),
				html = "";
                if (json.LogList != null && json.LogList.length > 0) {
                    html = template.render("configLogItem", json);
                    $list.html(html);
                }
                if (flag) {
                    imports.PagePadding(json.Page.Total);
                }
                cpage = $("#pages").val();
            }
        });

    },
    PagePadding: function (total) {
        var countNum = Math.ceil(total / pageSize);
        $("#pagination").ntspadding({
            recordnum: total,
            count: countNum,
            onChange: function (data) {
                imports.ShowHistoryList(data.page, false);
            }
        });
    },
    BtnSearch: function () {
        $("#btn_search").click(function () {
            imports.ShowHistoryList(1, true);
        });
    }
};


var imports = new importPage();



// 浮点数验证
function RegNumber(str) {
    var s = /^[0-9]+[.]?[0-9]*$/;
    return s.test(str);
}

/*
** 点击加载按钮
*/
function GetTreeObj() {
    var parentObjId = $("#TreeObjectId").val(),
                info = $("#TreeInfo").val(),
                ftType = $("#ftType").val(),
                energyCode = $("#item").val(),
                date = $("#iptDate").val();

    $("#divTemp").show();
    $("#divSave").show();

    if (parentObjId == "" || parentObjId == undefined) {
        alert("请选择节点树！");
        return false;
    }

    if (date == "" || date == undefined) {
        alert("请选择日期！");
        return false;
    }
    $("#lblMonth").html(date.substring(0,7));

    var data = {
        ParentObjID: parentObjId,
        EnergyID: energyCode,
        TreeInfo: info,
        SelectDate: date
    };

    var unit = $("#item option:selected").attr("name");
    $(".labUnit").each(function () { $(this).text(unit); });

    data = JSON.stringify(data);
    $.ajax({
        url: "action.ashx?action=GetTreeObj",
        dataType: "json",
        type: "POST",
        data: { "Inputs": data },
        success: function (json) {
            var $list = $("#tblSetting tbody");
            $list.empty();
            if (json != null) {
                $("#iptMoney").val(json.SJFTMoney);
                $("#lblMoney").html(json.TotalFTMoney);
                if (json.ListTreeObjList != null) {
                    if (json.ListTreeObjList.length > 0) {
                        var html = "";
                        html = template.render("treeItem", json);
                        $list.html(html);

                        // 全部
                        var tdNH = $("#tblSetting .tdNH"),
                            tdMJ = $("#tblSetting .tdMJ");
                        if (tdNH != undefined || tdMJ != undefined) {
                            // 全部
                            if (ftType == "0") {
                                tdMJ.show();
                                tdNH.show();
                            }
                            // 能耗
                            else if (ftType == "1") {
                                tdMJ.hide();
                                tdNH.show();
                            }
                            // 面积
                            else if (ftType == "2") {
                                tdMJ.show();
                                tdNH.hide();
                            }
                        }
                    } else {
                        alert("该节点下面没有数据，无法加载！");
                    }
                }
            }
        },
        error: function (msg) {
            alert("服务器错误，请联系管理员！--4");
        }
    });

    // 查询日志
    imports.ShowHistoryList(1, true);
}

function SaveData() {
    var sum = 0;
    var count = $("#tblSetting input").size();
    if (count > 0) {
        // 父区域ID
        var parentAreaId = $("#TreeObjectId").val(),
            parentAreaName = $("#TreeObjectName").val(),
               ftMoney = $("#iptMoney").val(),
               endTime = $("#iptDate").val(),
               energy = $("#item").val(),
               date = $("#iptDate").val();

        var jsonStr = "", tempStr = "";

        var ftType = $("#ftType").val();

        if (ftType == "0") {
            alert("请选择一种分摊方式保存！");
            return false;
        }
        if (date == "" || date == undefined) {
            alert("请选择日期！");
            return false;
        }

        if (ftMoney == "" || ftMoney == undefined) {
            alert("请输入实际分摊费用！");
            return false;
        }
        else {
            if (!RegNumber(ftMoney)) {
                alert("实际分摊费用格式不正确！");
                return false;
            }
        }

        for (var i = 1; i <= count / 3; i++) {
            var areaId = $("#hifAreaId" + i).val(),
                ftsjbl = $("#iptSJBL" + i).val(),
                id = $("#hifID" + i).val();

            if (ftsjbl == "" || ftsjbl == undefined) {
                alert("分摊实际比例不能为空！");
                return false;
            }
            else {
                if (!RegNumber(ftsjbl)) {
                    alert("分摊实际比例格式不正确！");
                    return false;
                }
            }

            sum = sum * 1 + ftsjbl * 1;

            if (id == "" || id == undefined) {
                tempStr += '{"ParentAREAID":' + parentAreaId + ',"AREAID":' + areaId + ',"ALLOCTION_FEE":' + ftMoney + ',"CFGPERCENT":' + ftsjbl + ',"ALLOCTION_EndDate":"' + endTime + '","PAYTYPE":"' + energy + '","PAYClass":"' + ftType + '"},';
            } else {
                tempStr += '{"ID":' + id + ',"ParentAREAID":' + parentAreaId + ',"AREAID":' + areaId + ',"ALLOCTION_FEE":' + ftMoney + ',"CFGPERCENT":' + ftsjbl + ',"ALLOCTION_EndDate":"' + endTime + '","PAYTYPE":"' + energy + '","PAYClass":"' + ftType + '"},';
            }
        }

        if (sum != "100") {
            alert("“分摊实际比例”之和只能是100%");
            return false;
        }
        else {
            for (var i = 1; i <= count / 3; i++) {
                var ftsjbl = $("#iptSJBL" + i).val();
                //处理详情
                var memoValue = $("#divBL" + i);
                memoValue.html(" ");
                memoValue.html(ftsjbl + " %");
            }
        }

        tempStr = tempStr.substring(0, tempStr.lastIndexOf(","));

        jsonStr = '{"ParentName":"' + parentAreaName + '","ListConfig":[' + tempStr + ']}';

        var memo = $("#divConfig").html();

        var tempId = $("#hifID1").val();
        if (tempId != "" && tempId != undefined && tempId != "0") {
            if (confirm("“" + parentAreaName + "”对象在" + date.substring(0,7) + " 中，分摊数据已存在，是否覆盖？")) {
                SaveMethod(jsonStr, memo);
            }
        }
        else {
            SaveMethod(jsonStr,memo);
        }
    }
}

function SaveMethod(jsonStr,memo) {
    $.ajax({
        url: "action.ashx?action=SaveAlloctionAndLog",
        dataType: "text",
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        data: { "Inputs": jsonStr, "Memo": memo },
        success: function (json) {
            var jsonData = eval("(" + json + ")");
            if (jsonData.IsSucess) {
                alert("分摊配置保存成功！");
                GetTreeObj();
                imports.ShowHistoryList(1, true);
            }
        },
        error: function () {
            self.common.loading($content, false);
        }
    });
}

/*
** 查看详情
*/
function ShowDetail(id) {
    $.ajax({
        url: "action.ashx?action=GetConfigLogDetail",
        dataType: "json",
        type: "POST",
        data: { "Inputs": id },
        success: function (json) {
            $("#divTemp").hide();
            $("#divConfig").empty();
            $("#divSave").hide();
            $("#divConfig").html(json);
        },
        error: function (msg) {
            alert("服务器错误，请联系管理员！--4");
        }
    });
}
