function warmnote() { }
warmnote.prototype = {
    showmaxlist: function (data) {
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: data, async: false, cache: false }).responseText;
        //alert(maxpage);
        maxpage = Math.ceil(maxpage / data.pagesize);
        $("#cp").val(maxpage);
        $('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: 1, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp").val()) { return; }
                new warmnote().showpaddinglist(data, pages);
            }
        });
        new warmnote().showpaddinglist(data, 1);

    },
    showtxlist: function (data) {
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetTXPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: data, async: false, cache: false }).responseText;
        //alert(maxpage);
        maxpage = Math.ceil(maxpage / data.pagesize);
        $("#cp1").val(maxpage);
        $('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: 1, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp1").val()) { return; }
                new warmnote().showtxpaddinglist(data, pages);
            }
        });
        new warmnote().showtxpaddinglist(data, 1);

    },
    showtxpaddinglist: function (data1, pages) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetTXPaddingList&dll=NTS_BECM.BLL&page=" + pages + "&times=" + new Date().getTime(),
            type: 'Post',
            data: data1,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#sysnlist").html('');
               // alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += "<li>";
                        html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_data\">";
                        html += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        html += "<td width=\"24%\"><span><span class=\"imgclick\"></span>" + data[i].F_Time + "</span></td>";
                        html += "<td width=\"24%\"><span>" + data[i].F_Content + "</span></td>";
                        html += "<td width=\"20%\"><span>" + GetDeviceName(data[i].F_DeviceID, data[i].AlarmType) + "</span></td>";
                        html += "<td width=\"16%\"><span></span></td>";
                        html += "<td width=\"16%\"><span></span></td>";
                        html += "</tr>";
                        html += "</table>";
                        html += "</li>";
                        $("#sysnlist").append(html);
                    }
                } else {
                    $("#sysnlist").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    },
    showarealist: function (buildid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(),
            type: 'Post',
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".lf_list_cn ul[name=a]").html('');
                $("#sareaid").val('');
                // 清除设备的选中
                $('.lf_list_cn2 ul').html('');
                if (data != ']') {
                    eval('data=' + data);
                    for (var i = 0; i < data.length; i++) {
                        $(".lf_list_cn ul[name=a]").append("<li config='" + data[i].BAreaID + "'> " + data[i].BAreaName + "</li>");
                    }
                    jQuery(".lf_list_cn ul[name=a] li").click(function () {
                        var index = jQuery(".lf_list_cn ulul[name=a] li").index(this);
                        jQuery(this).addClass("clicksub").siblings().removeClass("clicksub");
                        $("#sareaid").val($(this).attr('config'));

                        new warmnote().showdevicelist($(this).attr('config'));
                    });
                } else {
                    $(".lf_list_cn ul[name=a]").html('');
                }
            }
        });
    },
    showdevicelist: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&areaid=" + v + "&times=" + new Date().getTime(),
            type: 'Post',
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".lf_list_cn2 ul").html('');
                if (data != ']') {
                    eval('data=' + data);
                    for (var i = 0; i < data.length; i++) {
                        $(".lf_list_cn2 ul").append("<li style='cursor:pointer' config='" + data[i].F_MeterID + "'><span>" + data[i].F_MeterName + "</span><em style=\"display:none\"></em></li>");
                    }

                    jQuery(".lf_list_cn2 ul li").click(function () {
                        if ($(this).hasClass("clicksub999")) {
                            $(this).removeClass("clicksub999");

                        } else {
                            $(this).addClass("clicksub999");
                        }
                    });

                } else {
                    $(".lf_list_cn2 ul").html('');
                }
            }
        });
    },
    showpaddinglist: function (data1, pages) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetPaddingList&dll=NTS_BECM.BLL&page=" + pages + "&times=" + new Date().getTime(),
            type: 'Post',
            data: data1,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#sysnlist").html('');
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += "<li>";
                        html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_data\">";
                        html += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        html += "<td width=\"24%\"><span><span class=\"imgclick\"></span>" + data[i].WarnDate + "</span></td>";
                        html += "<td width=\"24%\"><span>" + data[i].Content + "</span></td>";
                        html += "<td width=\"20%\"><span>" + GetName(data[i].Build_Zone_Dev_ID, data[i].AlarmType) + "</span></td>";
                        html += "<td width=\"16%\"><span>" + data[i].F_AbnormalValue + "</span></td>";
                        html += "<td width=\"16%\"><span></span>" + data[i].F_NormalValue + "</td>";
                        html += "</tr>";
                        html += "</table>";
                        html += "</li>";
                        $("#sysnlist").append(html);
                    }
                } else {
                    $("#sysnlist").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    },
    showfzlist: function () {
        var bb11 = jQuery("#date01").val();
        var bb12 = jQuery("#hour01").val();
        var bb21 = jQuery("#date02").val();
        var bb22 = jQuery("#hour02").val();
        var data = {
            starttime: bb11 + ' ' + bb12 + ':00:00',
            endtime: bb21 + ' ' + bb22 + ':00:00',
            pagesize: 20,
            buildgroupid: $("#F_BuildGroupID").val()
        }
        new warmnote().showmaxlist(data);
    },
    outmaxexcel: function (data) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.outmaxexcel&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: data,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                window.location = data;
            }
        });
    },
    outtxexcel: function (data) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.outtxexcel&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: data,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                window.location = data;
            }
        });
    }
}


function GetCuttMonth() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() + 1;
    if (month < 10) {
        month = "0" + month;
    }
    var firstDay = year + "-" + month + "-" + "01";
    return firstDay;
}

function changeTwoDecimal(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        //alert('function:changeTwoDecimal->parameter error');
        //return false;
        return "";
    }
    var f_x = Math.round(x * 100) / 100;

    return f_x;
}

function GetToday() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() + 1;
    var date = myDate.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    return year + "-" + month + "-" + date;
}

$(function () {
    new warmnote().showfzlist();
    $("#querybtn").click(function () {
        // 注册按钮的事件
        var sclassid = $("#selectclass").val();
        if (sclassid == "0") {
            // 阀值
            var bb11 = jQuery("#date01").val();
            var bb12 = jQuery("#hour01").val();
            var bb21 = jQuery("#date02").val();
            var bb22 = jQuery("#hour02").val();
            $("#starttime").val(bb11 + ' ' + bb12 + ':00:00');
            $("#endtime").val(bb21 + ' ' + bb22 + ':00:00');
            var devicelist = '';
            $("#fazhi ul.nav_third li").each(function () {
                if ($(this).hasClass("select_c")) {
                    devicelist += ",'" + $(this).attr('config') + "'";
                }
            });
            //alert(devicelist);
            var data = {
                starttime: bb11 + ' ' + bb12 + ':00:00',
                endtime: bb21 + ' ' + bb22 + ':00:00',
                pagesize: 20,
                buildgroupid: $("#F_BuildGroupID").val(),
                buildid: $("#sbuild").val(),
                areaid: $("#sareaid").val(),
                deviceid: devicelist.length > 0 ? devicelist.substring(1) : devicelist
            }
            new warmnote().showmaxlist(data);
        } else {
            var bb11 = jQuery("#date01").val();
            var bb12 = jQuery("#hour01").val();
            var bb21 = jQuery("#date02").val();
            var bb22 = jQuery("#hour02").val();
            $("#starttime").val(bb11 + ' ' + bb12 + ':00:00');
            $("#endtime").val(bb21 + ' ' + bb22 + ':00:00');
            var data = {
                starttime: bb11 + ' ' + bb12 + ':00:00',
                endtime: bb21 + ' ' + bb22 + ':00:00',
                pagesize: 20,
                buildgroupid: $("#F_BuildGroupID").val(),
                buildid: $("#sbuild").val(),
                areaid: $("#sareaid").val(),
                deviceid: ''
            }
            //alert('xsaxsacwqcwq');
            new warmnote().showtxlist(data);
        }
    });

    $("#exportbtn").click(function () {
        var sclassid = $("#selectclass").val();
        if (sclassid == "0") {
            // 阀值
            var devicelist = '';
            $("#fazhi ul.nav_third li").each(function () {
                if ($(this).hasClass("select_c")) {
                    devicelist += ",'" + $(this).attr('config') + "'";
                }
            });
            var data = {
                starttime: $("#starttime").val(),
                endtime: $("#endtime").val(),
                pagesize: 20,
                buildgroupid: $("#F_BuildGroupID").val(),
                buildid: $("#sbuild").val(),
                areaid: $("#sareaid").val(),
                deviceid: devicelist.length > 0 ? devicelist.substring(1) : devicelist
            }
            new warmnote().outmaxexcel(data);
        } else {
            var data = {
                starttime: $("#starttime").val(),
                endtime: $("#endtime").val(),
                pagesize: 20,
                buildgroupid: $("#F_BuildGroupID").val(),
                buildid: $("#sbuild").val(),
                areaid: $("#sareaid").val(),
                deviceid: ''
            }
            new warmnote().outtxexcel(data);
        }

    });
});

function GetDeviceName(deviceid, deviceclass) {
    deviceclass = deviceclass == "2" ? "0" : deviceclass;
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.getdevicename&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { deviceid: deviceid, DeviceClass: deviceclass }, async: false, cache: false }).responseText;
}

function GetName(deviceid, deviceclass) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.getname&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { deviceid: deviceid, DeviceClass: deviceclass }, async: false, cache: false }).responseText;
}