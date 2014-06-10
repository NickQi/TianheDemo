function buildmaxvalue() { }
buildmaxvalue.prototype = {
    name: '建筑阀值js处理类',
    addbuildmaxvalue: function () {
        var BuildID = $("#BuildID").val();
        var F_EnergyItemCode = $("#F_EnergyItemCode").val();
        var MaxValue = $("#MaxValue").val();
        var TimeType = $("#TimeType").val();
        var MonthType = $("#MonthType").val();
        var data = {
            BuildID: BuildID,
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

        // alert(F_EnergyItemCode);
        //  return;
        //  alert(TimeType);

        //  alert(F_EnergyItemCode);
        // return false;
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BuildMaxValue.addbuildmaxvalue&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('建筑阀值信息添加成功。');
                    window.location = 'threshold-building.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    updatebuildmaxvalue: function () {
        var BuildID = $("#EBuildID").val();
        var F_EnergyItemCode = $("#EF_EnergyItemCode").val();
        var MaxValue = $("#EMaxValue").val();
        var TimeType = $("#ETimeType").val();
        var MonthType = $("#EMonthType").val();
        var id = $("#id").val();
        // alert(F_EnergyItemCode);
        // return;
        var data = {
            id: id,
            BuildID: BuildID,
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
            url: "action.ashx?method=NTS_BECM.BLL.BuildMaxValue.updatebuildmaxvalue&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('建筑阀值信息修改成功。');
                    window.location = 'threshold-building.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    deletebuildmaxvalue: function (did) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BuildMaxValue.deletebuildmaxvalue&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
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
                    window.location = 'threshold-building.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showbuildmaxvalueinfo: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BuildMaxValue.showbuildmaxvalueinfo&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { cuid: v },
            timeout: 1000,
            success: function (data) {
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#EBuildID").val(data[0].BuildID);
                    $("#EF_EnergyItemCode").val(data[0].F_EnergyItemCode);
                    $("#EMaxValue").val(data[0].MaxValue);
                    $("#ETimeType").val(data[0].TimeType);
                    $("#EMonthType").val(data[0].MonthType);
                    $("#id").val(data[0].id);
                    // 设置初始化的下拉框信息
                    $("#equip_sort_edit").val(GetF_EnergyItemCodeName(data[0].F_EnergyItemCode));
                    $(".sub_mid_areaname#edit li").each(function () {
                        //alert($(this).attr('config'));
                        if ($(this).attr('config') == data[0].BuildID) {
                            $(".dd_areaname#edit").val($(this).html());
                            //$("#EF_CollectionID").val($(this).attr('config'));
                        }
                    });

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


                }

            }
        });
    },
    showpaddingbuildmaxvaluelist: function (current_page) {
        var buildid = $("#sbuild").val();
        // alert(buildid);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BuildMaxValue.showpaddingbuildmaxvaluelist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { buildid: buildid, current_page: current_page, pagesize: 17 },
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
                        htmls += "<td width=\"151\" style=\"text-align:left\"><span><span class=\"imgclick\"></span> " + GetBuildName(data[i].BuildID) + "</span></td>";
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
function GetBuildName(buildid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}
function GetF_EnergyItemCodeName(code) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_DT_EnergyItemDict.GetF_EnergyItemCodeName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + code + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetTimeType(v)
{
    switch(v)
    {
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

function GetMonthType(v)
{
    switch(v)
    {
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

$(function () {
    /*下拉菜单设备16*/
    $(".equip_dd16").mouseover(function () { $(".dd_areaname_sub#edit").show(); });
    $(".equip_dd16").mouseout(function () { $(".dd_areaname_sub#edit").hide(); });
    $("ul.sub_mid_areaname#edit li").click(function () {
        var wt = $(this).html();
        $(".dd_areaname#edit").val(wt);
        $("#EBuildID").val($(this).attr('config'));
    });
    $(".equip_dd16").mouseover(function () { $(".dd_areaname_sub#add").show(); });
    $(".equip_dd16").mouseout(function () { $(".dd_areaname_sub#add").hide(); });
    $("ul.sub_mid_areaname#add li").click(function () {
        var wt = $(this).html();
        $(".dd_areaname#add").val(wt);
        // 给建筑下拉框赋值
        $("#BuildID").val($(this).attr('config'));
    });

    /*下拉菜单设备18*/
    $(".equip_dd18").mouseover(function () { $(".dd_cycle_sub#edit").show(); });
    $(".equip_dd18").mouseout(function () { $(".dd_cycle_sub#edit").hide(); });
    $("ul.sub_mid_cycle#edit li").click(function () {
        var wt = $(this).html();
        $(".dd_cycle#edit").val(wt);
        $("#ETimeType").val($(this).attr('config'));
    });
    $(".equip_dd18").mouseover(function () { $(".dd_cycle_sub#add").show(); });
    $(".equip_dd18").mouseout(function () { $(".dd_cycle_sub#add").hide(); });
    $("ul.sub_mid_cycle#add li").click(function () {
        var wt = $(this).html();
        $(".dd_cycle#add").val(wt);
        $("#TimeType").val($(this).attr('config'));

    });
    /*下拉菜单设备19*/
    $(".equip_dd19").mouseover(function () { $(".dd_season_sub#edit").show(); });
    $(".equip_dd19").mouseout(function () { $(".dd_season_sub#edit").hide(); });
    $("ul.sub_mid_season#edit li").click(function () {
        var wt = $(this).html();
        $(".dd_season#edit").val(wt);
        $("#EMonthType").val($(this).attr('config'));
    });
    $(".equip_dd19").mouseover(function () { $(".dd_season_sub#add").show(); });
    $(".equip_dd19").mouseout(function () { $(".dd_season_sub#add").hide(); });
    $("ul.sub_mid_season#add li").click(function () {
        var wt = $(this).html();
        $(".dd_season#add").val(wt);
        $("#MonthType").val($(this).attr('config'));
        //   alert($(this).attr('config'));
    });


    /*分类分项弹出修改*/
    jQuery("#equip_sort_edit").click(function () {
        jQuery("#sortss").show();
        jQuery("#sortst_edit").show();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .electric").click(function () {
        var s1 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s1);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .light_tt").click(function () {
        var s2 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s2);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .light_cn").click(function () {
        var s3 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s3);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .light_tts").click(function () {
        var s4 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s4);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .light_cns").click(function () {
        var s5 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s5);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .light_cns2").click(function () {
        var s6 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s6);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
    });
    jQuery("#sortst_edit .light_cns3").click(function () {
        var s7 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s7);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_edit .light_cns4").click(function () {
        var s8 = jQuery(this).html();
        jQuery("input#equip_sort_edit").val(s8);
        jQuery("#sortss").hide();
        jQuery("#sortst_edit").hide();
        $("#EF_EnergyItemCode").val($(this).attr('config'));
    });

    /*分类分项弹出增加*/
    jQuery("#equip_sort_add").click(function () {
        jQuery("#sortss").show();
        jQuery("#sortst_add").show();
    });
    jQuery("#sortst_add .electric").click(function () {
        var s1 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s1);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_tt").click(function () {
        var s2 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s2);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_cn").click(function () {
        var s3 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s3);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_tts").click(function () {
        var s4 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s4);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_cns").click(function () {
        var s5 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s5);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_cns2").click(function () {
        var s6 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s6);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_cns3").click(function () {
        var s7 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s7);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });
    jQuery("#sortst_add .light_cns4").click(function () {
        var s8 = jQuery(this).html();
        jQuery("input#equip_sort_add").val(s8);
        jQuery("#sortss").hide();
        jQuery("#sortst_add").hide();
        $("#F_EnergyItemCode").val($(this).attr('config'));
    });

    /*分类分项展开按钮*/
    jQuery(".arrowbtns").click(function () {
        var index = jQuery(".arrowbtns").index(this);
        jQuery(this).addClass("arrowbtnsbg").siblings().removeClass("arrowbtnsbg");
        jQuery(this).parent().parent().parent().parent().find("#H").show();
        // alert($(this).attr('config'));
        //  alert(jQuery(this).parent().parent().parent().parent().find("#H span").attr('config'));
        // 显示4级分类的信息列表
        var obj = jQuery(this).parent().parent().parent().parent().find("#H span");
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_DT_EnergyItemDict.showchildlist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { parentid: $(this).attr('config') },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    data = eval("data=" + data);
                    obj.html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        htmls += "<div config='" + data[i].F_EnergyItemCode + "' class=light_cns2>";
                        htmls += data[i].F_EnergyItemName;
                        htmls += "</div>";

                    }
                    obj.append(htmls);
                    jQuery("#sortst_add .light_cns2").click(function () {
                        var s6 = jQuery(this).html();
                        jQuery("input#equip_sort_add").val(s6);
                        jQuery("#sortss").hide();
                        jQuery("#sortst_add").hide();
                        $("#F_EnergyItemCode").val($(this).attr('config'));
                    });
                    jQuery("#sortst_edit .light_cns2").click(function () {
                        var s6 = jQuery(this).html();
                        jQuery("input#equip_sort_edit").val(s6);
                        jQuery("#sortss").hide();
                        jQuery("#sortst_edit").hide();
                        $("#EF_EnergyItemCode").val($(this).attr('config'));
                    });
                    
                } else {
                    //$("#collectoruselist").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });
    });


});