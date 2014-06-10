function importdata(){}
importdata.prototype = {
    showdatalist: function () {
        var firstp = 1;
        var wherestr = " classid=" + $("#datatype").val();
        //alert(wherestr);
        /*图表初始化结束*/
        var data = {
            classid: wherestr
        }
        //alert('');
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.ImportData.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: data, async: false, cache: false }).responseText;
        maxpage = Math.ceil(maxpage / 20);
        // alert(maxpage);
        $("#cp").val(maxpage);
        $('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: firstp, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp").val()) { return; }
                new importdata().showpaddinglist(pages, data);
            }
        });
        new importdata().showpaddinglist(1, data); // 第一次加载时
    },
    updateimportdata: function () {
        var data = {
            starttime: $(".date-pick#editbefore").val(),
            endtime: $(".date-pick#editafter").val(),
            itemcode: $("#eitemcode").val(),
            fvalue: $("#efvalue").val(),
            id: $("#id").val()
        }

        if (!compareTime(data.starttime, data.endtime)) {
            return;
        }
        //alert(data.itemcode.length);
        if (data.itemcode.length == 0) {
            alert('分类分项代码不能为空。');
            return;
        } else {
            $.ajax({
                url: "action.ashx?method=NTS_BECM.BLL.ImportData.updateimportdata&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                type: 'Post',
                data: data,
                dataType: 'text',
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                timeout: 1000,
                success: function (data) {
                    //alert(data);
                    eval("data=" + data);
                    if (data.success) {
                        alert('人工导入修改成功。');
                        jQuery(".popupbox").hide();
                        jQuery("#popuprevise").hide();
                        window.location = "warming-import.aspx?id=" + new Date().getTime();
                    }
                }
            });

        }


    },
    showimportdatainfo: function (v) {
        // alert(v);
        var tm = '';
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.ImportData.showimportdata&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: v },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                tm = data[0].classid;

                $(".dd_buildingno#edit").val(GetBuildName(data[0].buildid));
                $(".dd_areano#edit").val(GetAreaName(data[0].areaid));
                $(".dd_equipno#edit").val(GetDeviceName(data[0].deviceid));
                $("#id").val(data[0].id);
                $("#eitemcode").val(data[0].itemcode);
                $("#efvalue").val(data[0].fvalue);
                $("#datatype").val(data[0].classid);
                $(".date-pick#editbefore").val(data[0].starttime);
                $(".date-pick#editafter").val(data[0].endtime);
                // var starttime = $(".date-pick#addbefore").val();
                //  var endtime = $(".date-pick#addafter").val();

                $("#popuprevise .date-pick#editbefore").show();
                $("#popuprevise .date-pick#editafter").show();
                $("#popuprevise .date-con#editbefore").hide();
                $("#popuprevise .date-con#editafter").hide();
                $("#popuprevise .minute#editbefore").hide();
                $("#popuprevise .minute#editafter").hide();
                $("#popuprevise .hours#editbefore").hide();
                $("#popuprevise .hours#editafter").hide();
                $(".jiange").hide();

            }
        });

        /*
        if (tm == "1") {
        $("#popuprevise .date-pick#editbefore").show();
        $("#popuprevise .date-pick#editafter").show();
        $("#popuprevise .date-con#editbefore").hide();
        $("#popuprevise .date-con#editafter").hide();
        $("#popuprevise .minute#editbefore").show();
        $("#popuprevise .minute#editafter").show();
        $("#popuprevise .hours#editbefore").hide();
        $("#popuprevise .hours#editafter").hide();
        $(".jiange").show();
        }
        if (tm == "2") {
        $("#popuprevise .date-pick#editbefore").show();
        $("#popuprevise .date-pick#editafter").show();
        $("#popuprevise .date-con#editbefore").hide();
        $("#popuprevise .date-con#editafter").hide();
        $("#popuprevise .minute#editbefore").hide();
        $("#popuprevise .minute#editafter").hide();
        $("#popuprevise .hours#editbefore").show();
        $("#popuprevise .hours#editafter").show();
        $(".jiange").show();
        }
        if (tm == "3") {
        $("#popuprevise .date-pick#editbefore").show();
        $("#popuprevise .date-pick#editafter").show();
        $("#popuprevise .date-con#editbefore").hide();
        $("#popuprevise .date-con#editafter").hide();
        $("#popuprevise .minute#editbefore").hide();
        $("#popuprevise .minute#editafter").hide();
        $("#popuprevise .hours#editbefore").hide();
        $("#popuprevise .hours#editafter").hide();
        $(".jiange").hide();
        }
        if (tm == "4") {
        $("#popuprevise .date-pick#editbefore").hide();
        $("#popuprevise .date-pick#editafter").hide();
        $("#popuprevise .date-con#editbefore").show();
        $("#popuprevise .date-con#editafter").show();
        $("#popuprevise .minute#editbefore").hide();
        $("#popuprevise .minute#editafter").hide();
        $("#popuprevise .hours#editbefore").hide();
        $("#popuprevise .hours#editafter").hide();
        $(".jiange").hide();
        }
        */
    },
    deleteimportdata: function (id) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.ImportData.deleteimportdata&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: id },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval("data=" + data);
                if (data.success) {
                    window.location = "warming-import.aspx?id=" + new Date().getTime();
                } else {
                    alert('出现异常的错误，删除失败');
                }

            }
        });
    },
    showpaddinglist: function (pages, data1) {
        $("#padding-main").html('');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.ImportData.GetPaddingList&dll=NTS_BECM.BLL&pagesize=20&pages=" + pages + "&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data1,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        //alert(data[i].DeviceStatus);
                        htmls += "<li>";
                        htmls += "<table width=\"696\" border=\"0\" cellpadding=\"0\" class=\"menu_tbl\" align='center' cellspacing=\"0\">";

                        htmls += "<tr  valign=\"middle\" align=\"left\">";

                        htmls += "<td width=\"111\" align='center'><span>" + data[i].buildid + "</span></td>";
                        htmls += "<td width=\"72\" align='center'><span>" + data[i].areaid + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span>" + data[i].deviceid + "</span></td>";
                        htmls += "<td width=\"73\" align='center'><span>" + data[i].itemcode + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span>" + data[i].starttime + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span>" + data[i].endtime + "</span></td>";
                        htmls += "<td width=\"70\" align='center'><span>" + data[i].fvalue + "</span></td>";
                        htmls += "<td width=\"100\" align='center'><span class=\"btnbg2\" style=\"background: url(images/menu/line04.jpg) no-repeat scroll center 4px;\"><input type=\"button\" class=\"button04\" onclick=\"DivRev(" + data[i].id + ")\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"DivDel(" + data[i].id + ")\" value=\"删除\"></span></td>";
                        htmls += " </tr>";
                        htmls += " </table>";
                        htmls += " </li>";
                    }
                    // alert(htmls);
                    $("#padding-main").html(htmls);
                } else {
                    $("#padding-main").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    },
    showarealist: function (buildid) {
        //alert(buildid);
        //清空下拉框
        $(".dd_areano#add").val('');
        $("#F_AreaID").val('');

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&[__DOTNET__]System.String=" + buildid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".sub_mid_areano#add").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $(".sub_mid_areano#add").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $(".sub_mid_areano#add").append(" <li config='" + data[i].BAreaID + "'>" + data[i].BAreaName + "</li>");
                    }

                    jQuery(".sub_mid_areano#add li").click(function () {
                        var wt = $(this).html();
                        $(".dd_areano#add").val(wt);
                        $("#F_AreaID").val($(this).attr('config'));
                        new importdata().showdevicelist($(this).attr('config'));
                    });

                    $(".dd_areano#add").val($(".sub_mid_areano#add li:first").html());
                    $("#F_AreaID").val($(".sub_mid_areano#add li:first").attr('config'));
                    // 显示默认的仪表
                    new importdata().showdevicelist($(".sub_mid_areano#add li:first").attr('config'));
                } else {
                    $(".sub_mid_areano#add").html('');
                }

            }
        });
    },
    showdevicelist: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceList&[__DOTNET__]System.String=" + v + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $(".dd_equipno#add").val('');
                $(".sub_mid_equipno#add").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#sub_mid_equipno").html('');
                    for (var i = 0; i < data.length; i++) {
                        $(".sub_mid_equipno#add").append(" <li config='" + data[i].F_MeterID + "'>" + data[i].F_MeterName + "</li>");
                    }
                    $(".dd_equipno#add").val($(".sub_mid_equipno#add li:first").html());
                    $("#deviceid").val($(".sub_mid_equipno#add li:first").attr('config'));
                    jQuery(".sub_mid_equipno#add li").click(function () {
                        var wt = $(this).html();
                        $(".dd_equipno#add").val(wt);
                        $("#deviceid").val($(this).attr('config'));
                    });
                } else {
                    $("#sub_mid_equipno").html('');
                }
            }
        });
    },
    addimportdata: function () {
        var buildid = $("#F_BuildID").val();
        var areaid = $("#F_AreaID").val();
        var deviceid = $("#deviceid").val();
        var itemcode = $("#itemcode").val();
        var starttime = $(".date-pick#addbefore").val();
        var endtime = $(".date-pick#addafter").val();
        var fvalue = $("#fvalue").val();
        var classid = $("#datatype").val();

        // alert(buildid);
        //  return false;
        if (deviceid == '') {
            alert('设备不能为空。');
            return false;
        }
        if (itemcode == '') {
            alert('分类分项不能为空。');
            return false;
        }
        if (fvalue == '') {
            alert('数值不能为空。');
            return false;
        }
        switch (classid) {
            case "1":
                starttime = starttime + " " + $("#minute15").val();
                endtime = endtime + " " + $("#aminute15").val();
                break;
            case "2":
                starttime = starttime + " " + $("#hour").val() + ":00:00";
                endtime = endtime + " " + $("#ahour").val() + ":00:00";
                break;
            case "3":
                starttime = starttime;
                endtime = endtime;
                break;
            default:
                starttime = $(".inputRev#addbefore_month").val() + "-1";
                endtime = $(".inputRev#addafter_month").val() + "-1";
                break;
        }
        var data = {
            buildid: buildid,
            areaid: areaid,
            deviceid: deviceid,
            itemcode: itemcode,
            starttime: starttime,
            endtime: endtime,
            fvalue: fvalue,
            classid: classid
        }
       // alert(compareTime(data.starttime, data.endtime))
      //  return;
        if (!compareTime(data.starttime, data.endtime)) {
            return;
        } else {
            //alert(starttime);
            //return;
            $.ajax({
                url: "action.ashx?method=NTS_BECM.BLL.ImportData.addimportdata&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                type: 'Post',
                data: data,
                dataType: 'text',
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                timeout: 1000,
                success: function (data) {
                    //alert(data);
                    eval("data=" + data);
                    if (data.success) {
                        alert('人工导入成功。');
                        jQuery(".popupbox").hide();
                        jQuery("#popupadd").hide();
                        window.location = "warming-import.aspx?id=" + new Date().getTime();
                    }
                }
            });
        }


    }
}

/*
function GetAreaName(areaid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.BArea.GetAreaName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + areaid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetDeviceName(deviceid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + deviceid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetBuildName(buildid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}
*/
$(function(){
    new importdata().showdatalist();
});