function devicemaxvalue() { }
devicemaxvalue.prototype = {
    name: '区域阀值js处理类',
    adddevicemaxvalue: function () {
        var DeviceID = $("#DeviceID").val();
        var F_EnergyItemCode = $("#F_EnergyItemCode").val();
        var MaxValue = $("#MaxValue").val();
        var TimeType = $("#TimeType").val();
        var MonthType = $("#MonthType").val();
        var data = {
            DeviceID: DeviceID,
            F_EnergyItemCode: F_EnergyItemCode,
            MaxValue: MaxValue,
            TimeType: TimeType,
            MonthType: MonthType
        }

        if (F_EnergyItemCode.length == 0) {
            alert('请选择分类分项的代码');
            return false;
        }

        if (MaxValue.length == 0) {
            alert('请输入阀值');
            return false;
        }
        if (MaxValue.length > 8) {
            alert('输入阀值的长度不能大于8位');
            return false;
        }
        var isdemail = MyCommValidate({ rule: "isdemail", value: MaxValue });
        if (isdemail != '') { alert("阀值的格式错误：" + isdemail); return false; }

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.adddevicemaxvalue&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('设备阀值信息添加成功。');
                    window.location = 'threshold-equipment.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showdevicemaxvalueinfo: function (v) {
        //alert(v);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.showdevicemaxvalueinfo&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { cuid: v },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#EAreaID").val(data[0].DeviceID);
                    $("#EF_EnergyItemCode").val(data[0].F_EnergyItemCode);
                    $("#EMaxValue").val(data[0].MaxValue);
                    $("#ETimeType").val(data[0].TimeType);
                    $("#EMonthType").val(data[0].MonthType);
                    $("#id").val(data[0].id);
                    // 设置初始化的下拉框信息
                    $("#equip_sort_edit").val(GetF_EnergyItemCodeName(data[0].F_EnergyItemCode));

                    /*
                    $(".sub_mid_areaname#edit li").each(function () {
                    //alert($(this).attr('config'));
                    if ($(this).attr('config') == data[0].BuildID) {
                    $(".dd_areaname#edit").val($(this).html());
                    //$("#EF_CollectionID").val($(this).attr('config'));
                    }
                    });
                    */

                    $(".sub_mid_cycle#edit li").each(function () {
                        //alert(data[0].F_BuildID);

                        if ($(this).attr('config') == data[0].TimeType) {
                            $(".dd_cycle#edit").val($(this).html());
                            //$("#EF_BuildID").val($(this).attr('config'));
                            //显示区域的列表
                            //  new collectoruse().showeditarealist($(this).attr('config'));
                        }
                    });

                    $(".sub_mid_season#edit li").each(function () {
                        //alert($(this).attr('config'));
                        // alert(data[0].F_AreaID);
                        if ($(this).attr('config') == data[0].MonthType) {
                            $(".dd_season#edit").val($(this).html());
                            // $("#EF_AreaID").val($(this).attr('config'));
                            //显示区域的列表
                            // new collectoruse().showeditarealist($(this).attr('config'));
                        }
                    });

                    new devicemaxvalue().showeditdevicelist(data[0].F_AreaID, data[0].DeviceID);

                }

            }
        });
    },
    updatedevicemaxvalue: function () {
        var DeviceID = $("#EDeviceID").val();
        var F_EnergyItemCode = $("#EF_EnergyItemCode").val();
        var MaxValue = $("#EMaxValue").val();
        var TimeType = $("#ETimeType").val();
        var MonthType = $("#EMonthType").val();
        var id = $("#id").val();

        if (F_EnergyItemCode.length == 0) {
            alert('请选择分类分项的代码');
            return false;
        }

        if (MaxValue.length == 0) {
            alert('请输入阀值');
            return false;
        }
        if (MaxValue.length > 8) {
            alert('输入阀值的长度不能大于8位');
            return false;
        }
        var isdemail = MyCommValidate({ rule: "isdemail", value: MaxValue });
        if (isdemail != '') { alert("阀值的格式错误：" + isdemail); return false; }

        // alert(id);
        // alert(F_EnergyItemCode);
        // return;
        var data = {
            id: id,
            DeviceID: DeviceID,
            F_EnergyItemCode: F_EnergyItemCode,
            MaxValue: MaxValue,
            TimeType: TimeType,
            MonthType: MonthType
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.updatedevicemaxvalue&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('设备阀值信息修改成功。');
                    window.location = 'threshold-equipment.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    deletedevicemaxvalue: function (did) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.deletedevicemaxvalue&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { did: did },
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    // alert('建筑阀值信息修改成功。');
                    window.location = 'threshold-equipment.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showsearcharealist: function (buildid) {
        // searcharea
        //alert(buildid);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&[__DOTNET__]System.String=" + buildid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            // data: { buildid: buildid },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $("#searcharea").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#searcharea").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $("#searcharea").append(" <em config='" + data[i].BAreaID + "'>" + data[i].BAreaName + "</em>");
                    }

                    jQuery("#searcharea em").click(function () {
                        //alert('');
                        var ba = jQuery(this).html();
                        jQuery("#dome_area").html(ba);
                        jQuery("#c_path2").html(ba);
                        var index = jQuery("#dome_1_3 em").index(this);
                        jQuery(this).css({ 'background-color': '#4b4b4b' }).siblings().css({ 'background-color': '' });
                        $("#sareaid").val($(this).attr('config'));
                        // 显示仪表数据信息
                        //alert('');
                        var firstp = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.BackfirstPage&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
                        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.GetPageCount&dll=NTS_BECM.BLL&[__DOTNET__]System.Int32=6&[__DOTNET__]System.String=" + $("#sbuild").val() + "&times=" + new Date().getTime(), type: 'GET', data: { areaid: $("#sareaid").val() }, async: false, cache: false }).responseText;
                        $("#cp").val(maxpage);
                        // alert(maxpage);
                        $('.pagination#all').css("display", "none");
                        $('.pagination#classid').css("display", "");
                        $('.pagination#classid').html('');
                        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
                        $('.pagination#classid').jqPagination({
                            link_string: '/?page={page_number}',
                            current_page: firstp, //设置当前页 默认为1
                            max_page: maxpage, //设置最大页 默认为1
                            page_string: '当前第{current_page}页,共{max_page}页',
                            paged: function (pages) {
                                if (pages > $("#cp").val()) { return; }
                                new devicemaxvalue().showpaddingcollectoruselist(pages);
                            }
                        });
                        new devicemaxvalue().showpaddingcollectoruselist(1); // 第一次加载时 

                    });
                } else {
                    $("#searcharea").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });
    },
    showdevicelist: function (areaid) {
        //alert(buildid);
        //清空下拉框
        $(".dd_areaname#add").val('');
        $("DeviceID").val('');

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceList&[__DOTNET__]System.String=" + areaid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".sub_mid_areaname#add").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $(".sub_mid_areaname#add").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $(".sub_mid_areaname#add").append(" <li style=\"width:145px;padding-left:5px;\" config='" + data[i].F_MeterID + "'>" + data[i].F_MeterName + "</li>");
                    }

                    jQuery(".sub_mid_areaname#add li").click(function () {
                        var wt = $(this).html();
                        $(".dd_areaname#add").val(wt);
                        $("#DeviceID").val($(this).attr('config'));
                    });


                    // 默认第一个
                    $(".dd_areaname#add").val($(".sub_mid_areaname#add li:first").html());
                    $("#DeviceID").val($(".sub_mid_areaname#add li:first").attr('config'));

                } else {
                    $(".sub_mid_areaname#add").html('');
                }

            }
        });
    },
    showeditdevicelist: function (areaid, deviceid) {
        $(".dd_areaname#edit").val('');
        $("#EDeviceID").val('');

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceList&[__DOTNET__]System.String=" + areaid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            async: false,
            success: function (data) {
                //alert(data);
                $(".sub_mid_areaname#edit").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $(".sub_mid_areaname#edit").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $(".sub_mid_areaname#edit").append(" <li style=\"width:145px;padding-left:5px;\" config='" + data[i].F_MeterID + "'>" + data[i].F_MeterName + "</li>");
                    }

                    jQuery(".sub_mid_areaname#edit li").click(function () {
                        var wt = $(this).html();
                        $(".dd_areaname#edit").val(wt);
                        $("#EDeviceID").val($(this).attr('config'));
                    });
                   // alert(deviceid);
                    $(".sub_mid_areaname#edit li").each(function () {
                       // alert($(this).attr('config'));
                        if ($(this).attr('config').replace(" ", "") == deviceid.replace(" ", "")) {
                            //alert('sb');
                            $(".dd_areaname#edit").val($(this).html());
                            $("#EDeviceID").val($(this).attr('config'));
                        }
                    });


                } else {
                    $(".sub_mid_areaname#edit").html('');
                }

            }
        });
    },
    showpaddingcollectoruselist: function (current_page) {
        var buildid = $("#sbuild").val();
        var areaid = $("#sareaid").val();
        //alert(buildid);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceMaxValue.showpaddingdevicemaxvaluelist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { buildid: buildid, current_page: current_page, pagesize: 6, areaid: areaid },
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#collectoruselist").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        htmls += "<li>";
                        htmls += "<table width=\"776\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"menu_tbl\">";
                        htmls += "<tr height=\"30\" valign=\"middle\">";
                        htmls += "<td width=\"151\" style=\"text-align:left\"><span><span class=\"imgclick\"></span> " + GetDeviceName(data[i].DeviceID) + "</span></td>";
                        htmls += "<td width=\"140\"><span style=\"float:left\">" + GetF_EnergyItemCodeName(data[i].F_EnergyItemCode) + "</span></td>";
                        htmls += "<td width=\"140\"><span style=\"float:left\">" + data[i].MaxValue + "</span></td>";
                        htmls += "<td width=\"140\"><span style=\"float:left\">" + GetTimeType(data[i].TimeType) + "</span></td>";
                        htmls += "<td width=\"57\" align=\"left\"><span style=\"float:left\">" + GetMonthType(data[i].MonthType) + "</span></td>";
                        htmls += "<td width=\"148\" valign=\"middle\"><span class=\"btnbg2\"><input type=\"button\" class=\"button04\" onclick=\"DivRevise4('" + data[i].id + "')\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"Del3('" + data[i].id + "')\" value=\"删除\"></span></td>";
                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";

                    }
                    $("#collectoruselist").append(htmls);
                } else {
                    $("#collectoruselist").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });
    }
}

function GetDeviceName(deviceid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + deviceid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}
function GetF_EnergyItemCodeName(code) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_DT_EnergyItemDict.GetF_EnergyItemCodeName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + code + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetTimeType(v) {
    switch (v) {
        case "1":
            return "年";
        case "2":
            return "月";
        case "3":
            return "季度";
        default:
            return "日";
    }
}

function GetMonthType(v) {
    switch (v) {
        case "1":
            return "春";
        case "2":
            return "夏";
        case "3":
            return "秋";
        default:
            return "冬";
    }
}