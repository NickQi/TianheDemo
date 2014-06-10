require.config({
    baseUrl: "/html/resources/scripts/jsrc/",
    urlArgs: "",
    paths: {
        jquery: "lib/jquery/jquery.min",
        blockui: "lib/blockUI/blockUI",
        artTemplate: "lib/artTemplate/template.min",
        common: "common/common"
    }
});

var timeList;

// 正浮点数验证
function RegDecimal(str) {
    var s = /^[0-9]+[.]?[0-9]*$/;
    return s.test(str);
}

// 正整数验证
function RegNumber(str) {
    var s = /^[0-9]+$/;
    return s.test(str);
}

function DeletePeriod(id, hifId) {
    if (confirm("是否确认删除数据？")) {
        if (id > 0) {
            var trObj = $("#tr" + id);
            var dId = $("#hifTimeRuleID" + hifId).val();
            if (dId == "" || dId == undefined) {
                trObj.remove();
            } else {
                $.ajax({
                    url: "action.ashx?action=DeletePeriodByID",
                    dataType: "text",
                    type: "POST",
                    data: { "Inputs": id },
                    success: function (json) {
                        var jsonData = eval("(" + json + ")");
                        if (jsonData.IsSucess) {
                            trObj.remove();
                        }
                    },
                    error: function () {
                    }
                });
            }
        }
    }
}

function DeleteStep(id, hifId) {
    if (confirm("是否确认删除数据？")) {
        if (id > 0) {
            var count = $("#divRate tr").size();
            var trObj = $("#trStep" + id);
            var trpre = trObj.prev();
            var dId = $("#hifID" + hifId).val();
            if (dId == "" || dId == undefined) {
                trObj.remove();
                if (trpre != null) {
                    var button = trpre.find("input[type='button']");
                    if (button != null) {
                        button.removeAttr("disabled");
                    }
                }
            } else {
                $.ajax({
                    url: "action.ashx?action=DeleteStepByID",
                    dataType: "text",
                    type: "POST",
                    data: { "Inputs": id },
                    success: function (json) {
                        var jsonData = eval("(" + json + ")");
                        if (jsonData.IsSucess) {
                            trObj.remove();
                            if (trpre != null) {
                                var button = trpre.find("input[type='button']");
                                if (button != null) {
                                    button.removeAttr("disabled");
                                }
                            }
                        }
                    },
                    error: function () {
                    }
                });
            }
        }
    }
}

function GetList() {
    InitDate();
    var obj = $("#sltEnergy");
    var energyType = obj.val();
    var info = $("#sltEnergy option[value='" + energyType + "']").text();
    $("#lblInfo").html(info);
    $.ajax({
        url: "action.ashx?action=GetRateInfoList",
        dataType: "text",
        type: "POST",
        data: { "Inputs": energyType },
        success: function (json) {
            var jsonData = eval("(" + json + ")");
            // 平价
            if (jsonData.ParValueModel != null) {
                $("#hifCommID").val(jsonData.ParValueModel.ID);
                $("#iptCommPrice").val(jsonData.ParValueModel.PRICE);
                $("#sltParDate option[value='" + jsonData.ParValueModel.DATE + "']").attr("selected", true);
            }
            else {
                $("#hifCommID").val("");
                $("#iptCommPrice").val("");
                $("#sltParDate").val("1");
            }
            // 阶梯
            var $listStep = $("#divRate tbody");
            if (jsonData.MultiStepList != null) {
                var html = "";
                if (jsonData.MultiStepList.length > 0) {
                    $("#sltStepDate option[value='" + jsonData.MultiStepList[0].DATE + "']").attr("selected", true);
                    $listStep.empty();
                    html = template.render("rateItem", jsonData);
                    $listStep.html(html);
                    $("#divRate input[type='button']").attr("disabled", "disabled");
                    $("#divRate input[type='button']:last").removeAttr("disabled");
                }
                else {
                    $("#sltStepDate").val("1");
                    $listStep.empty();
                }
            }
            else {
                $("#sltStepDate").val("1");
                $listStep.empty();
            }

            // 分时
            $("#timePrice1").val(jsonData.PriceJ);
            $("#timePrice2").val(jsonData.PriceF);
            $("#timePrice3").val(jsonData.PriceP);
            $("#timePrice4").val(jsonData.PriceG);
            var $listPeroid = $("#divTimeItemList tbody");
            if (jsonData.PeroidList != null) {
                var html = "";
                if (jsonData.PeroidList.length > 0) {
                    $("#sltPeriodDate option[value='" + jsonData.PeroidList[0].DATE + "']").attr("selected", true);
                    $listPeroid.empty();
                    html = template.render("timeRuleItem", jsonData);
                    $listPeroid.html(html);

                    // 初始化下拉框值 
                    for (var i = 1; i <= jsonData.PeroidList.length; i++) {
                        var sltType = eval("sltRateType" + i);
                        var $typeDiff = $(sltType);
                        $.each(jsonData.PeroidFlag, function (i, item) {
                            $typeDiff.append('<option value=' + item.ID + '>' + item.Name + '</option>');
                        });
                        var id = jsonData.PeroidList[(i - 1)].TYPE;
                        $("#sltRateType" + i + " option[value='" + id + "']").attr("selected", true);
                    }
                }
                else {
                    $("#sltPeriodDate").val("1");
                    $listPeroid.empty();
                }
            }
            else {
                $("#sltPeriodDate").val("1");
                $listPeroid.empty();
            }
        },
        error: function () {

        }
    });
}

function InitDate() {
    var parDate = $("#sltParDate");
    var stepDate = $("#sltStepDate");
    var periodDate = $("#sltPeriodDate");
    parDate.empty();
    stepDate.empty();
    periodDate.empty();
    for (var i = 1; i <= 28; i++) {
        parDate.append('<option value=' + i + '>' + i + '</option>');
        stepDate.append('<option value=' + i + '>' + i + '</option>');
        periodDate.append('<option value=' + i + '>' + i + '</option>');
    }
}

require(["jquery", "artTemplate", "blockui", "common"], function ($, ArtTemplate, blockui, md5, Common) {
    function Rate() {
        this.init();
    }
    Rate.prototype = {

        /*
        ** 初始化页面
        */
        init: function () {
            this.initDate();
            this.initSelect();
            this.render();
        },

        render: function () {
            this.SelectChange();
            this.AddStepTime();
            this.AddRate();
            this.BtnCommSave();
            this.BtnStepTime();
            this.BtnRateSave();
        },

        initSelect: function () {
            var energyType = $("#sltEnergy");
            $.ajax({
                url: "action.ashx?action=indexItem",
                dataType: "text",
                type: "POST",
                success: function (json) {
                    var jsonData = eval("(" + json + ")");
                    if (jsonData.ItemLst != null) {
                        $.each(jsonData.ItemLst, function (i, item) {
                            energyType.append('<option value=' + item.ItemCodeNumber + '>' + item.ItemCodeName + '</option>');
                        });
                        // 查询方法
                        GetList();
                    }
                },
                error: function () {
                }
            });
        },

        /*
        ** 初始化结算日
        */
        initDate: function () {
            var parDate = $("#sltParDate");
            var stepDate = $("#sltStepDate");
            var periodDate = $("#sltPeriodDate");
            for (var i = 1; i <= 28; i++) {
                parDate.append('<option value=' + i + '>' + i + '</option>');
                stepDate.append('<option value=' + i + '>' + i + '</option>');
                periodDate.append('<option value=' + i + '>' + i + '</option>');
            }
        },

        SelectChange: function () {
            $("#sltEnergy").change(function () {
                // 如果用户选择能源类型为“电”,则有分时模式，其他能源类型都没有分时模式
                var energy = $("#sltEnergy").val();
                if (energy == "01000") {
                    $("#divPeriod").show();
                }
                else {
                    $("#divPeriod").hide();
                }
                GetList();
            });
        },

        /*
        ** 平价模式保存
        */
        BtnCommSave: function () {
            $("#btnCommSave").click(function () {
                var energyType = $("#sltEnergy").val();
                var date = $("#sltParDate").val();
                var name = $("#sltEnergy option:selected").text();
                var price = $("#iptCommPrice").val();
                var id = $("#hifCommID").val();
                var jsonStr;
                if (price == "" || price == undefined) {
                    alert("请先输入平价价格！");
                    return false;
                }
                else {
                    price = parseFloat(price);
                    if (!RegDecimal(price)) {
                        alert("价格只能是正数！");
                        return false;
                    }
                }
                if (id == "" || id == undefined) {
                    jsonStr = "{'TYPEID':'" + energyType + "','PRICE':'" + price + "','DATE':" + date + ",'CNAME':'" + name + "平价'}";
                }
                else {
                    jsonStr = "{'TYPEID':'" + energyType + "','PRICE':'" + price + "','DATE':" + date + ",'ID':" + id + "}";
                }
                $.ajax({
                    url: "action.ashx?action=SaveCommPrice",
                    dataType: "text",
                    type: "POST",
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    data: { "Inputs": jsonStr },
                    success: function (json) {
                        var jsonData = eval("(" + json + ")");
                        if (jsonData.IsSucess) {
                            alert("平价模式保存成功！");
                            GetList();
                        }
                    },
                    error: function () {
                    }
                });
            });
        },

        /*
        ** 添加阶梯 页面效果
        */
        AddStepTime: function () {
            $("#btnStepAdd").click(function () {
                var count = $("#divRate input.iptText").size();
                var i = (count / 3) + 1;

                if (i > 4) {
                    alert("最多只能添加4档阶梯数据！");
                    return false;
                }
                var str = '<tr id="trStep' + i + '" class="trStyle"><td width="50" align="center">第' + i + '档</td><td width="250" ><input type="text" id="iptMinValue' + i + '"  class="iptText"  maxlength="10" />'
                        + ' ~ <input type="text" id="iptMaxValue' + i + '"  class="iptText" maxlength="10" /></td><td width="150" ><input type="text" id="iptPrice' + i + '" class="iptText" maxlength="10" /> 元'
                            + '<input type="hidden" id="hifID' + i + '" /></td><td>'
							+ '<input type="button" value="删除" width="30px" id="btnDeleteStep' + i + '" onclick="DeleteStep(' + i + ',' + i + ');">'
						+ '</td></tr>';
                if (count == 0) {
                    $("#divRate tbody").html(str);
                    $("#iptMinValue" + i).val("0");
                }
                else {
                    var maxValue = $("#iptMaxValue" + (i - 1)).val();
                    var minValue = $("#iptMinValue" + (i - 1)).val();

                    if (maxValue == "" || maxValue == undefined) {
                        alert("必须输入阶梯的最大值！");
                    } else {
                        maxValue = parseFloat(maxValue);
                        minValue = parseFloat(minValue);
                        if (!RegNumber(maxValue)) {
                            alert("最大值只能是正整数！");
                            return false;
                        }
                        if (eval(minValue) >= eval(maxValue)) {
                            alert("阶梯的最大值只能大于最小值！");
                        } else {
                            $("#divRate input[type='button']").attr("disabled", "disabled");
                            $(".trStyle:last").after(str);
                            $("#iptMinValue" + i).val(maxValue * 1 + 1);
                            if (i == 4) {
                                $("#iptMaxValue" + i).attr("readonly", "readonly");

                            }
                        }
                    }
                }
            });
        },

        /*
        ** 添加费率时段 页面效果
        */
        AddRate: function () {
            $("#btnRateAdd").click(function () {
                var count = $("#divTimeItemList select").size();
                var i = count + 1;
                var str = '<tr id="tr' + i + '" class="trTimeStyle">'
                        + '<td width="100">'
                            + '<select id="sltRateType' + i + '">'
                            + '</select>'
                            + ' <input type="hidden" id="hifTimeRuleID' + i + '" /> '
                        + '</td>'
                        + '<td width="350" style="height:40px;">'
                            + '<input type="text" class="iptText" id="iptStartTime' + i + '" maxlength="2" style="width:50px !important;" readonly="readonly" /> 时 '
                            + '<input type="text" class="iptText" id="iptStartMinute' + i + '" maxlength="2" style="width:50px !important;" readonly="readonly" /> 分'
                            + ' ~ '
                            + '<input type="text" class="iptText" id="iptEndTime' + i + '" maxlength="2" style="width:50px !important;" /> 时 '
                            + '<input type="text" class="iptText" id="iptEndMinute' + i + '" maxlength="2" style="width:50px !important;" readonly="readonly" value="59" /> 分'
                        + '</td>'
                        + '<td>'
							+ '<input type="button" value="删除" width="30px" id="btnDelete' + i + '" onclick="DeletePeriod(' + i + ',' + i + ');">'
						+ '</td>'
                        + '</tr>';

                // 初始化下拉框值 
                if (count == 0) {
                    $("#divTimeItemList tbody").html(str);
                    $("#iptStartTime" + i).val("0");
                    $("#iptStartMinute" + i).val("0");
                    $.ajax({
                        url: "action.ashx?action=GetPeriodEnum",
                        dataType: "text",
                        type: "POST",
                        success: function (json) {
                            var jsonData = eval("(" + json + ")");
                            var sltType = eval("sltRateType" + i);
                            var $typeDiff = $(sltType);
                            $.each(jsonData, function (i, item) {
                                $typeDiff.append('<option value=' + item.ID + '>' + item.Name + '</option>');
                            });
                        },
                        error: function () {
                        }
                    });
                } else {
                    var endHour = $("#iptEndTime" + (i - 1)).val();
                    var endMinute = $("#iptEndMinute" + (i - 1)).val();

                    var startHour = $("#iptStartTime" + (i - 1)).val();
                    var startMinute = $("#iptStartMinute" + (i - 1)).val();

                    if (endHour == "" || endHour == undefined || endMinute == "" || endMinute == undefined) {
                        alert("必须输入分时的最大时间段！");
                    } else {
                        endHour = parseFloat(endHour);
                        endMinute = parseFloat(endMinute);
                        startHour = parseFloat(startHour);
                        startMinute = parseFloat(startMinute);
                        if (endHour == "23" && endMinute == "59") {
                            alert("分时配置已经完成，若要继续添加数据，请修改！");
                        } else {

                            if (eval(startHour) > eval(endHour)) {
                                alert("结束时间(小时)不能小于开始时间(小时)！");
                            }
                            else {
                                if (eval(startHour) == eval(endHour)) {
                                    if (eval(startMinute) >= eval(endMinute)) {
                                        alert("结束时间(分钟)只能大于开始时间(分钟)！");
                                    }
                                    else {
                                        $(".trTimeStyle:last").after(str);
                                        if (endMinute == "59") {
                                            $("#iptStartTime" + i).val(endHour * 1 + 1);
                                            $("#iptStartMinute" + i).val("0");
                                        } else {
                                            $("#iptStartTime" + i).val(endHour);
                                            $("#iptStartMinute" + i).val(endMinute * 1 + 1);
                                        }

                                        $.ajax({
                                            url: "action.ashx?action=GetPeriodEnum",
                                            dataType: "text",
                                            type: "POST",
                                            success: function (json) {
                                                var jsonData = eval("(" + json + ")");
                                                var sltType = eval("sltRateType" + i);
                                                var $typeDiff = $(sltType);
                                                $.each(jsonData, function (i, item) {
                                                    $typeDiff.append('<option value=' + item.ID + '>' + item.Name + '</option>');
                                                });
                                            },
                                            error: function () {
                                            }
                                        });
                                    }
                                } else {
                                    $(".trTimeStyle:last").after(str);
                                    if (endMinute == "59") {
                                        $("#iptStartTime" + i).val(endHour * 1 + 1);
                                        $("#iptStartMinute" + i).val("0");
                                    } else {
                                        $("#iptStartTime" + i).val(endHour);
                                        $("#iptStartMinute" + i).val(endMinute * 1 + 1);
                                    }

                                    $.ajax({
                                        url: "action.ashx?action=GetPeriodEnum",
                                        dataType: "text",
                                        type: "POST",
                                        success: function (json) {
                                            var jsonData = eval("(" + json + ")");
                                            var sltType = eval("sltRateType" + i);
                                            var $typeDiff = $(sltType);
                                            $.each(jsonData, function (i, item) {
                                                $typeDiff.append('<option value=' + item.ID + '>' + item.Name + '</option>');
                                            });
                                        },
                                        error: function () {
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            });
        },
        /*
        ** 阶梯 保存
        */
        BtnStepTime: function () {
            $("#btnStepSave").click(function () {
                var date = $("#sltStepDate").val();
                var count = $("#divRate input.iptText").size();
                var energyType = $("#sltEnergy").val();
                var name = $("#sltEnergy option:selected").text();
                if (count > 0) {

                    /*
                    * add by wangyuliang ---begin--
                    */
                    var temp = count - 2;
                    var tempvalue = $("#divRate input.iptText:eq(" + temp + ")").val();
                    if (tempvalue != "") {
                        alert("最后一档的最大值不可输入数据！");
                        return false;
                    }
                    /*
                    * add by wangyuliang ---end
                    */

                    var jsonStr = "[";
                    for (var i = 1; i <= count / 3; i++) {
                        var maxId = eval("iptMaxValue" + i),
                            minId = eval("iptMinValue" + i),
                            priceId = eval("iptPrice" + i),
                            id = eval("hifID" + i);
                        var maxValue = $(maxId).val(),
                            minValue = $(minId).val(),
                            priceValue = $(priceId).val(),
                            idValue = $(id).val();

                        /*
                        ** 输入框验证 Start
                        */
                        if (i != count / 3) {
                            if (minValue == "" || minValue == undefined) {
                                alert("请输入第" + i + "条数据的最小值！");
                                return false;
                            }
                            else {
                                minValue = parseFloat(minValue);
                                if (!RegNumber(minValue)) {
                                    alert("第" + i + "条数据的最小值只能是正整数！");
                                    return false;
                                }
                            }

                            if (maxValue == "" || maxValue == undefined) {
                                alert("请输入第" + i + "条数据的最大值！");
                                return false;
                            }
                            else {
                                maxValue = parseFloat(maxValue);
                                if (!RegNumber(maxValue)) {
                                    alert("第" + i + "条数据的最大值只能是正整数！");
                                    return false;
                                }
                            }

                            if (eval(minValue) >= eval(maxValue)) {
                                alert("第" + i + "条数据的最小值不能大于或等于最大值！");
                                return false;
                            }
                        }
                        else {
                            maxValue = -1;
                        }

                        if (priceValue == "" || priceValue == undefined) {
                            alert("请输入第" + i + "条数据的价格！");
                            return false;
                        }
                        else {
                            priceValue = parseFloat(priceValue);
                            if (!RegDecimal(priceValue)) {
                                alert("第" + i + "条数据的价格只能是正数！");
                                return false;
                            }
                        }

                        if (i != 1) {
                            var prevPriceId = eval("iptPrice" + (i - 1));
                            var prevPriceValue = $(prevPriceId).val();
                            prevPriceValue = parseFloat(prevPriceValue);
                            if (prevPriceValue >= priceValue) {
                                alert("第" + i + "条数据的价格只能大于上一条价格！");
                                return false;
                            }

                            var prevMaxId = eval("iptMaxValue" + (i - 1));
                            var prevMaxValue = $(prevMaxId).val();
                            if ((prevMaxValue * 1 + 1) != minValue) {
                                alert("第" + i + "条数据的最小值和上一条数据的最大值不相连！");
                                return false;
                            }
                        }

                        /*
                        ** 输入框验证 End
                        */

                        var cname = name + "阶梯" + i;

                        if (idValue != undefined && idValue != "") {
                            jsonStr += '{"ID":' + idValue + ',"END_GEARS_VALUE":' + maxValue + ',"START_GEARS_VALUE":' + minValue + ',"PRICE":"' + priceValue + '","DATE":' + date + '},';
                        }
                        else {
                            jsonStr += '{"TYPEID":"' + energyType + '","GEARSID":' + i + ',"GEARNAME":"' + cname + '","END_GEARS_VALUE":' + maxValue + ',"START_GEARS_VALUE":' + minValue + ',"PRICE":"' + priceValue + '","DATE":' + date + '},';
                        }

                    }
                    jsonStr = jsonStr.substring(0, jsonStr.lastIndexOf(","));
                    jsonStr += "]";

                    $.ajax({
                        url: "action.ashx?action=SaveRatePrice",
                        dataType: "text",
                        type: "POST",
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { "Inputs": jsonStr },
                        success: function (json) {
                            var jsonData = eval("(" + json + ")");
                            if (jsonData.IsSucess) {
                                alert("阶梯模式保存成功！");
                                GetList();
                            }
                        },
                        error: function () {
                        }
                    });
                }
            });
        },

        /*
        ** 分时 模式 保存
        */
        BtnRateSave: function () {
            $("#btnRateSave").click(function () {
                var energyType = $("#sltEnergy").val();
                var name = $("#sltEnergy option:selected").text();
                var jsonStr, jsonRuleItem = "";

                // 4种分时价格
                var price = 0;
                var price1 = $("#timePrice1").val();
                var price2 = $("#timePrice2").val();
                var price3 = $("#timePrice3").val();
                var price4 = $("#timePrice4").val();
                if (price1 == "" || price1 == undefined || price2 == "" || price2 == undefined || price3 == "" || price3 == undefined || price4 == "" || price4 == undefined) {
                    alert("【费率时段】的4种类型，价格都不能为空！");
                    return false;
                }

                // 结算日
                var periodDate = $("#sltPeriodDate").val();
                // 分时 规则
                var countRule = $("#divTimeItemList select").size();

                if (countRule > 0) {
                    for (var i = 1; i <= countRule; i++) {
                        var startTimeId = eval("iptStartTime" + i),
                                startMinuteId = eval("iptStartMinute" + i),
                                endTimeId = eval("iptEndTime" + i),
                                endMinuteId = eval("iptEndMinute" + i),
                                selectId = eval("sltRateType" + i),
                                hifTimeRuleId = eval("hifTimeRuleID" + i);
                        var startTimeValue = $(startTimeId).val(),
                                endTimeValue = $(endTimeId).val(),
                                selectValue = $(selectId).val(),
                                startMinuteValue = $(startMinuteId).val(),
                                endMinuteValue = $(endMinuteId).val(),
                                hifTimeRuleValue = $(hifTimeRuleId).val();

                        var pValue = $("#sltRateType" + i + " option:selected").text();

                        if (selectValue == "1") {
                            price = parseFloat(price1);
                        }
                        else if (selectValue == "2") {
                            price = parseFloat(price2);
                        }
                        else if (selectValue == "3") {
                            price = parseFloat(price3);
                        }
                        else if (selectValue == "4") {
                            price = parseFloat(price4);
                        }

                        // 表单验证
                        if (startTimeValue == "" || startTimeValue == undefined) {
                            alert("请输入第" + i + "条数据的开始时间(小时)！");
                            return false;
                        }
                        else {
                            startTimeValue = parseFloat(startTimeValue);
                            if (!RegNumber(startTimeValue)) {
                                alert("第" + i + "条数据的开始时间(小时)只能是正整数！");
                                return false;
                            }
                            if (eval(startTimeValue) < 0 || eval(startTimeValue) > 23) {
                                alert("第" + i + "条数据的开始时间(小时)只能在0时~24时之间！");
                                return false;
                            }
                        }

                        if (startMinuteValue == "" || startMinuteValue == undefined) {
                            alert("请输入第" + i + "条数据的开始时间(分钟)！");
                            return false;
                        }
                        else {
                            startMinuteValue = parseFloat(startMinuteValue);
                            if (!RegNumber(startMinuteValue)) {
                                alert("第" + i + "条数据的开始时间(分钟)只能是正整数！");
                                return false;
                            }
                            if (eval(startMinuteValue) < 0 || eval(startMinuteValue) > 59) {
                                alert("第" + i + "条数据的开始时间(分钟)只能在0分~60分之间！");
                                return false;
                            }
                        }

                        if (endTimeValue == "" || endTimeValue == undefined) {
                            alert("请输入第" + i + "条数据的结束时间(小时)！");
                            return false;
                        }
                        else {
                            endTimeValue = parseFloat(endTimeValue);
                            if (!RegNumber(endTimeValue)) {
                                alert("第" + i + "条数据的结束时间(小时)只能是正整数！");
                                return false;
                            }
                            if (eval(endTimeValue) < 0 || eval(endTimeValue) > 23) {
                                alert("第" + i + "条数据的结束时间(小时)只能在0时~24时之间！");
                                return false;
                            }
                        }

                        if (endMinuteValue == "" || endMinuteValue == undefined) {
                            alert("请输入第" + i + "条数据的结束时间(分钟)！");
                            return false;
                        }
                        else {
                            endMinuteValue = parseFloat(endMinuteValue);
                            if (!RegNumber(endMinuteValue)) {
                                alert("第" + i + "条数据的结束时间(分钟)只能是正整数！");
                                return false;
                            }
                            if (eval(endMinuteValue) < 0 || eval(endMinuteValue) > 59) {
                                alert("第" + i + "条数据的结束时间(分钟)只能在0分~60分之间！");
                                return false;
                            }
                        }

                        if (eval(startTimeValue) > eval(endTimeValue)) {
                            alert("第" + i + "条数据的开始时间(小时)不能大于结束时间(小时)！");
                            return false;
                        }
                        else {
                            if (eval(startTimeValue) == eval(endTimeValue)) {
                                if (eval(startMinuteValue) >= eval(endMinuteValue)) {
                                    alert("第" + i + "条数据的开始时间(分钟)不能大于或等于结束时间(分钟)！");
                                    return false;
                                }
                            }
                        }

                        // 判断最后一条数据的时间是否是  24时0分
                        if (i == countRule) {
                            if (endTimeValue != "23" || endMinuteValue != "59") {
                                alert("最后一个分时阶段的时间只能是23时59分！");
                                return false;
                            }
                        }

                        if (i != 1) {
                            var prevHourId = eval("iptEndTime" + (i - 1)),
                                prevMinuteId = eval("iptEndMinute" + (i - 1));
                            var prevHourTemp, prevMinuteTemp;

                            var prevHour = $(prevHourId).val(),
                            prevMinute = $(prevMinuteId).val();

                            if (prevMinute == "59") {
                                prevMinuteTemp = "0";
                                prevHourTemp = prevHour * 1 + 1;
                            } else {
                                prevMinuteTemp = prevMinute * 1 + 1;
                                prevHourTemp = prevHour;
                            }

                            if (prevMinuteTemp != startMinuteValue || prevHourTemp != startTimeValue) {
                                alert("第" + i + "条数据的开始时间、分钟与上一条数据的开始时间、分钟不相连！");
                                return false;
                            }
                        }

                        // 表单验证 End
                        var value = name + "_费率时段_" + pValue + "_(" + startTimeValue + "/" + endTimeValue + ")";

                        if (hifTimeRuleValue != 0) {
                            jsonRuleItem += '{"ID":' + hifTimeRuleValue + ',"CNAME":"' + value + '","TYPE":' + selectValue + ',"STARTHOUR":' + startTimeValue + ',"STARTMINUTE":' + startMinuteValue + ',"ENDHOUR":' + endTimeValue + ',"ENDMINUTE":' + endMinuteValue + ',"DATE":' + periodDate + ',"PRICE":"' + price + '"},';
                        } else {
                            jsonRuleItem += '{"TYPEID":"' + energyType + '","CNAME":"' + value + '","TYPE":' + selectValue + ',"STARTHOUR":' + startTimeValue + ',"STARTMINUTE":' + startMinuteValue + ',"ENDHOUR":' + endTimeValue + ',"ENDMINUTE":' + endMinuteValue + ',"PRICE":"' + price + '","DATE":' + periodDate + '},';
                        }
                    }
                }

                if (jsonRuleItem.length > 0) {
                    jsonRuleItem = "[" + jsonRuleItem.substring(0, jsonRuleItem.lastIndexOf(",")) + "]";
                }
                else {
                    jsonRuleItem = null;
                }

                jsonStr = jsonRuleItem;

                $.ajax({
                    url: "action.ashx?action=SaveTimePrice",
                    dataType: "text",
                    type: "POST",
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    data: { "Inputs": jsonStr },
                    success: function (json) {
                        var jsonData = eval("(" + json + ")");
                        if (jsonData.IsSucess) {
                            alert("分时模式保存成功！");
                            GetList();
                        }
                    },
                    error: function () {
                    }
                });
            });
        },

        /**
        * Ajax request
        */
        getAction: function () {
            return {
                saveCommPrice: "action.ashx?action=SaveCommPrice",
                saveTimePrice: "action.ashx?action=SaveTimePrice",
                GetList: "action.ashx?action=GetRateInfoList"
            };
        }
    };
    this.Rate = new Rate();
});


$(function () {
    $(".iptText").live("keyup", function isNumber(e) {
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

    $(".iptText").live("keydown", function isNumber(e) {
        var obj = $(this);
        var value = obj.val();
        if (!e) {
            e = window.event;
        }
        if (e.which) {
            key = e.which;

        } else {
            key = e.keyCode;
        }
        var validNumber = false;
       
        if (!e.shiftKey) {

            //only check shift is not pressed
            if (value.length > 0 && (key == 190 || key == 110) && value.indexOf(".") == -1) {
                validNumber = true;
            }
            //0 
            if ((key == 48 || key == 96) && (value.length == 0 || (value.length > 0 && value > 0))) { // 0
                validNumber = true;
            }
            //1-9
            if ((key >= 49 && key <= 57) || (key >= 97 && key <= 105)) {
                validNumber = true;
            }
            // if (key >= 49 && key <= 57 && !(value == 0 && value.length == 1)) { // 1~9同时不能有099这种形式
            //  validNumber = true;
            //  }
        }
        //back delete ->  <- home end 
        if (key == 8 || key == 46 || key >= 35 && key <= 37 || key == 39) {
            validNumber = true;
        }
        if (!validNumber) {
            if (e.preventDefault) {
                e.preventDefault();
            } else {
                e.returnValue = false;
            }
        }
    });
});