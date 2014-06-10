var exportdata = function () { };
exportdata.prototype = {
    name: '数据导出',
    showorderlist: function (data) {

        if (jQuery(".datesort_fast_list ul li.data_select").html() == "本周") {
            $(".export_ttsmall").html(data.starttime.split(' ')[0] + ' - ' + data.endtime + ' ' + $('.val').html() + ' <em style="color: #5d87c3">数据导出</em>');
            //$("#querytime").html(data.starttime.split(' ')[0] + " - " + data.endtime);
        } else {
            $(".export_ttsmall").html(data.starttime + ' - ' + data.endtime + ' ' + $('.val').html() + ' <em style="color: #5d87c3">数据导出</em>');
        }
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.BllChart.chartexport.GetExportPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'Post', data: data, async: false, cache: false }).responseText;
        var pagesize = 16;
        // alert(maxpage);
        maxpage = Math.ceil(maxpage / pagesize);

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
                new exportdata().showpaddinglist(pages, pagesize, data);
            }
        });
        new exportdata().showpaddinglist(1, pagesize, data);
    },
    showpaddinglist: function (pages, pagesize, data) {
        //alert('123456');
        $("#padding-main").html('');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BllChart.chartexport.showpaddinglist&dll=NTS_BECM.BLL&pages=" + pages + "&pagesize=" + pagesize + "&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            async: false,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    $("#part1").html('');
                    $("#part2").html('');
                    for (var i = 0; i < data.length; i++) {
                        var html = "";
                        html += "<li>";
                        html += "<table width=100% border=0 cellpadding=0 cellspacing=0 class=tbl_exportcn>";
                        html += "<tr height=30 align=center valign=middle>";
                        html += "<td width=18%>";
                        html += "<span>" + (pagesize * (pages - 1) + i + 1) + "</span>";
                        html += "</td>";
                        html += "<td width=41% align=left>";
                        html += "<span>" + data[i].Oname + "</span>";
                        html += "</td>";
                        html += " <td width=26%>";
                        //alert(data[i].Fvalue);
                        html += "    <span>" + data[i].Fvalue + " </span>";

                        html += " </td>";
                        html += "<td width=15%>";
                        if ($("#tjclass").val() != '2') {
                            if ($("#isall").val() == '1') {
                                $("#y1").html('面积');
                                $("#y2").html('面积');
                                html += "    <span>" + data[i].mianji + "m<sup>2</sup></span>";
                            } else if ($("#isall").val() == '2') {
                                $("#y1").html('人数');
                                $("#y2").html('人数');
                                html += "    <span>" + data[i].mens + "人</span>";
                            } else {
                                $("#y1").html('');
                                $("#y2").html('');
                            }
                        }
                        html += "</td>";
                        html += " </tr>";
                        html += " </table>";
                        html += "</li>";
                        if (i < 8) {
                            $("#part1").append(html);
                        } else {
                            $("#part2").append(html);
                        }
                    }
                } else {
                    $("#part1").html('');
                    $("#part2").html('');
                }

            }
        });
    }
}

$(function () {
    var olists = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetQueryBuildList&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { flag: '0' }, async: false, cache: false }).responseText;
    olists = olists.length > 0 ? olists.substring(1) : olists;
    var data = {
        starttime: GetCuttMonth(),
        endtime: GetToday(),
        itemcode: $('.val').attr('config'),
        olist: olists,
        isall: 0,
        topnum: -1, // 多少个，-1表示不限
        order: 'asc', //前排序
        tjclass: '0'// 0，建筑1，区域，2设备
    }
    /*隐藏域赋值*/
    $("#starttime").val(data.starttime);
    $("#endtime").val(data.endtime);
    $("#olist").val(data.olist);
    $("#isall").val(data.isall);
    $("#topnum").val(data.topnum);
    $("#order").val(data.order);
    $("#tjclass").val(data.tjclass);

    var buildnamelist = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetQueryBuildList&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { flag: '1' }, async: false, cache: false }).responseText;
    $("#qlabel").html(buildnamelist.length > 0 ? buildnamelist.substring(1) : buildnamelist);
    $("#gundongtxt").html(buildnamelist.length > 0 ? buildnamelist.substring(1) : buildnamelist);
    new exportdata().showorderlist(data);
});

$(function () {
    $("#btn").click(function () {
        var bb11 = jQuery("#date01").val();
        var bb12 = jQuery("#hour01").val();
        var bb21 = jQuery("#date02").val();
        var bb22 = jQuery("#hour02").val();
        $("#starttime").val(bb11 + ' ' + bb12 + ':00:00');
        $("#endtime").val(bb21 + ' ' + bb22 + ':00:00');
        //alert($(".sort_select span.val").attr('config'));
       // return;
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
    });
});


/*页面级别的js部分*/
$(function () {
/*
$('#date01').DatePicker({
    format: 'Y-m-d',
    date: $('#s').val(),
    current: $('#s').val(),
    starts: 1,
    position: 'r',
    onBeforeShow: function () {
        $('#date01').DatePickerSetDate($('#s').val(), true);
    },
    onChange: function (formated, dates) {
        $('#date01').val(formated);
        $('#date01').DatePickerHide();
    }
});

$('#date02').DatePicker({
    format: 'Y-m-d',
    date: $('#s').val(),
    current: $('#s').val(),
    starts: 1,
    position: 'r',
    onBeforeShow: function () {
        $('#date02').DatePickerSetDate($('#s').val(), true);
    },
    onChange: function (formated, dates) {
        $('#date02').val(formated);
        $('#date02').DatePickerHide();
    }
});
*/
});

jQuery(document).ready(function () {

    var clearTimer = null;
    var SleepTime = 3000;   //停留时长，单位毫秒
    jQuery(".rt_nav_list").jCarouselLite({
        btnPrev: "span.rt_topbtn",
        btnNext: "span.rt_btmbtn",
        visible: 4,
        scroll: 1,
        vertical: true,
        speed: 500 //滚动速度，单位毫秒
        
        //circular: true,
        //auto:5000,
        //mouseOver:true
    });

});


/*圆角*/
DD_roundies.addRule('.sub_box', '5px 5px 5px 5px', true);
DD_roundies.addRule('.subbox', '5px 5px 5px 5px', true);

/*返回主页*/
function ReturnShow(flag) {
    jQuery("#A" + flag).show();
    jQuery("#B" + flag).show();
    jQuery("#C" + flag).show();
    jQuery("#D" + flag).hide();
}
function ReturnHide(flag) {
    jQuery("#A" + flag).hide();
    jQuery("#B" + flag).hide();
    jQuery("#C" + flag).hide();
    jQuery("#D" + flag).show();
}
function ShowBox() {
    jQuery("#A" + flag).hide();
    jQuery("#B" + flag).hide();
    jQuery("#C" + flag).hide();
    jQuery("#D" + flag).show();
}

/*取消按钮*/
/**建筑**/
function DivClosed() {
    jQuery(".popupbox").hide();
    jQuery("#building_up").hide();
    jQuery("#building_up .selectedboxcn").html("" + "<em></em>");
    jQuery("#building_up").find("input").attr("checked", "");
}
/**区域**/
function DivClosed1() {
    jQuery(".popupbox").hide();
    jQuery("#area_up").hide();
    jQuery("#area_up .selectedboxcn").html("" + "<em></em>");
    jQuery("#area_up").find("input").attr("checked", "");
}
/**设备**/
function DivClosed2() {
    jQuery(".popupbox").hide();
    jQuery("#equip_up").hide();
    jQuery("#equip_up .selectedboxcn").html("" + "<em></em>");
    jQuery("#equip_up").find("input").attr("checked", "");
}
function DivClosed3() {
    $("#equip_up2").find("input[checked]").each(function (i, val) {
        var tt = $(this).parent().find("em").html();
        if (tt) {
            var ttc = tt + "、";
            var tm = $("#equip_up .selectedboxcn").html();
            var tm = tm.replace(ttc, "");
            $("#equip_up .selectedboxcn").html(tm);
        }
    });
    jQuery(".popupbox").hide();
    jQuery("#equip_up2").hide();
}



/*确定按钮*/
/**排序导出按钮**/
function ConfirmDiv0() {
    jQuery(".popupbox").hide();
    jQuery("#paixubox").hide();
    // 排序事件
    var topnum = $("#prevtxt").val();
    var isint = MyCommValidate({ rule: "number", value: topnum });
    if (isint != '') { alert("数据格式错误：" + isint); return false; }
    $("#topnum").val(topnum);
    var data = {
        starttime: $("#starttime").val(),
        endtime: $("#endtime").val(),
        itemcode: $('.val').attr('config'),
        olist: $("#olist").val(),
        isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
        topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
        order: $("#order").val(),
        tjclass: $("#tjclass").val()
    }
    new exportdata().showorderlist(data);
}

function ConfirmDivs0() {
    jQuery(".popupbox").hide();
    jQuery("#paixubox").hide();
    // 排序事件
    var topnum = $("#nexttxt").val();
    var isint = MyCommValidate({ rule: "number", value: topnum });
    if (isint != '') { alert("数据格式错误：" + isint); return false; }
    $("#topnum").val(topnum);
    var data = {
        starttime: $("#starttime").val(),
        endtime: $("#endtime").val(),
        itemcode: $('.val').attr('config'),
        olist: $("#olist").val(),
        isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
        topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
        order: $("#order").val(),
        tjclass: $("#tjclass").val()
    }
    new exportdata().showorderlist(data);
    
}
/**建筑**/
function ConfirmDiv() {
    var chooselist = '';
    var choosetxtlist = '';
    $("#building_up table input").each(function () {
        if ($(this).attr('name').indexOf('ch') > -1) {
            if ($(this).attr('checked')) {
                chooselist += "," + $(this).val();
                var tt = $(this).parent().find("em").html();
                choosetxtlist += tt + "、";
            }
        }
    });

    if (chooselist == '') {
        alert("请选择需要排序的建筑。");
        return false;
    } else {
        $("#qlabel").html(choosetxtlist);
        $("#gundongtxt").html(choosetxtlist);
        $("#olist").val(chooselist.substring(1));
    }
   DivClosed();
}


/**区域**/
function ConfirmDiv1() {

    var chooselist = '';
    var choosetxtlist = '';
    $("#area_up table input").each(function () {
        if ($(this).attr('name').indexOf('ch') > -1) {
            if ($(this).attr('checked')) {
                chooselist += "," + $(this).val();
                var tt = $(this).parent().find("em").html();
                choosetxtlist += tt + "、";
            }
        }
    });

    if (chooselist == '') {
        alert("请选择需要排序的建筑区域。");
        return false;
    } else {
        $("#qlabel").html(choosetxtlist);
        $("#gundongtxt").html(choosetxtlist);
        $("#olist").val(chooselist.substring(1));
    }
    DivClosed1();
}
/**设备**/
function ConfirmDiv2() {
    var chooselist = '';
    var choosetxtlist = '';
    $("#equip_up .selectedboxcn em:first span").each(function () {
        chooselist += "," + $(this).attr('config');
        var tt = $(this).html();
        choosetxtlist += tt ;
    });
    if (chooselist == '') {
        alert("请选择需要排序的设备信息。");
        return false;
    } else {
        $("#qlabel").html(choosetxtlist);
        $("#gundongtxt").html(choosetxtlist);
        $("#olist").val(chooselist.substring(1));
    }
    DivClosed2();
}
function ConfirmDiv3() {
    jQuery(".popupbox").hide();
    jQuery("#equip_up2").hide();
}







$(function () {
    /*新闻播报*/
    jQuery(".ranking_ttbig span.rank_btn").click(function () {
        jQuery(".ranking_ttbig .gundongbox").show(1000);
    });
    jQuery(".ranking_ttbig .gundongbox span").click(function () {
        jQuery(".ranking_ttbig .gundongbox").hide(1000);
    });
    // alert($("#gundongtxt").html());
    /*排序 导出*/
    jQuery("#arrow01").click(function () {
        jQuery("#rankbox01").hide();
        jQuery("#rankbox02").show();
        $("#order").val('desc');
    });
    jQuery("#arrow02").click(function () {
        jQuery("#rankbox01").show();
        jQuery("#rankbox02").hide();
        $("#order").val('asc');
    });

    /*单位用量*/
    jQuery(".unitusebox").mouseover(function () {
        jQuery(".unitusebox ul").show();
    });
    jQuery(".unitusebox").mouseout(function () {
        jQuery(".unitusebox ul").hide();
    });
    /*主页排序按钮*/
    jQuery("#paixubtn").click(function () {
        $(".popupbox").show();
        $("#paixubox").show();
        $("#prevtxt").val('100');
    });

    /*导出表格按钮*/
    jQuery("#exportbtn").click(function () {

        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        $.ajax({
            type: "post",
            url: "action.ashx?method=NTS_BECM.BLL.BllChart.chartexport.OutExcel&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "text",
            data: data,
            success: function (data) {
                window.location = data;
            },
            complete: function (XMLHttpRequest, textStatus) {
                //HideLoading();
            },
            error: function () {
                //请求出错处理
            }
        });
    });

    $(".unitusebox li").click(function () {
        $("#chooselabel").html($(this).html());
        $("#isall").val($(this).attr('config'));
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
    });






    /*建筑 区域 设备 查询*/
    jQuery("#rank01").click(function () {
        $(".unitusebox").css("display", "block");
        $("#tjclass").val('0');
        jQuery(this).removeClass("rank01");
        jQuery("#rank02").removeClass("rank02");
        jQuery("#rank03").removeClass("rank03");
        jQuery(this).addClass("rank01");
        /*建筑 弹出框--选中部分*/
        jQuery(".popupbox").show();
        jQuery("#building_up").show();
        jQuery("#building_up").find("input").attr("checked", "checked");

        $("#building_up").find("input[checked]").each(function (i, val) {
            var tt = $(this).parent().find("em").html();
            if (tt) {
                $("#building_up .selectedboxcn em").before(tt + "、");

            }
        });

    });

    jQuery("#rank02").click(function () {
        $("#tjclass").val('1');
        $(".unitusebox").css("display", "block");
        jQuery("#rank01").removeClass("rank01");
        jQuery(this).removeClass("rank02");
        jQuery("#rank03").removeClass("rank03");
        jQuery(this).addClass("rank02");
        /*区域 弹出框--选中部分*/
        jQuery(".popupbox").show();
        jQuery("#area_up").show();
        jQuery("#area_up").find("input").attr("checked", "checked");
        $("#area_up").find("input[checked]").each(function (i, val) {
            var tt = $(this).parent().find("em").html();
            if (tt) {
                $("#area_up .selectedboxcn em").before(tt + "、");
            }
        });
    });

    jQuery("#rank03").click(function () {
        $("#tjclass").val('2');
        $("#isall").val('0');
        $(".unitusebox").css("display", "none");
        jQuery("#rank01").removeClass("rank01");
        jQuery("#rank02").removeClass("rank02");
        jQuery(this).removeClass("rank03");
        jQuery(this).addClass("rank03");
        /*设备 弹出框--选中部分*/
        jQuery(".popupbox").show();
        jQuery("#equip_up").show();
        jQuery("#equip_up").find("input").attr("checked", "checked");
        jQuery("#equip_up2").find("input").attr("checked", "checked");

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAllDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var htmlt = $("#equip_up .selectedboxcn em:first").html();
                        if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                            $("#equip_up .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                        }
                    }
                }
            }
        });
    });

    /*建筑复选框处理*/
    $("#building_up input").each(function () {
        $(this).click(function () {
            var ts = $("#building_up .selectedboxcn").html();
            if ($(this).attr("checked")) { // 复选框选中状态
                if ($(this).attr('name').indexOf('p') > -1) { // 父类的文本框
                    var chname = 'ch_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        $(this).attr("checked", pobj.attr('checked'));
                        var tt = $(this).parent().find("em").html();
                        if (ts.indexOf(tt) < 0) {
                            if (tt) {
                                $("#building_up .selectedboxcn em").before(tt + "、");
                            }
                        }
                    });
                } else {
                    var cpname = 'p' + $(this).attr('name').replace("ch", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", chobj.attr('checked'))
                    });
                    var tt = $(this).parent().find("em").html();
                    if (ts.indexOf(tt) < 0) {
                        if (tt) {
                            $("#building_up .selectedboxcn em").before(tt + "、");
                        }
                    }
                }
            }
            else { // 复选框未选中状态
                if ($(this).attr('name').indexOf('p') > -1) { // 父类的文本框
                    var chname = 'ch_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        //alert(pobj.attr('checked'));
                        $(this).attr("checked", false);
                        var tt = $(this).parent().find("em").html();
                        var tslist = $("#building_up .selectedboxcn").html();
                        var tm = tslist.replace(tt + "、", "");
                        $("#building_up .selectedboxcn").html(tm);
                    });
                } else {
                    var cpname = 'p' + $(this).attr('name').replace("ch", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false)
                    });
                    var tslist = $("#building_up .selectedboxcn").html();
                    var tt = $(this).parent().find("em").html();
                    var tm = tslist.replace(tt + "、", "");
                    $("#building_up .selectedboxcn").html(tm);
                }
            }
        });
    });

    /*区域 弹出框--不选中取消*/

    $("#area_up input").each(function () {
        $(this).click(function () {
            var ts = $("#area_up .selectedboxcn").html();
            if ($(this).attr("checked")) { // 复选框选中状态
                if ($(this).attr('name').indexOf('bp') > -1) { // 父类的文本框
                    var chname = 'bch_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        $(this).attr("checked", pobj.attr('checked'));
                        var tt = $(this).parent().find("em").html();
                        if (ts.indexOf(tt) < 0) {
                            if (tt) {
                                $("#area_up .selectedboxcn em").before(tt + "、");
                            }
                        }
                    });
                } else {
                    var cpname = 'bp' + $(this).attr('name').replace("bch", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", chobj.attr('checked'))
                    });
                    var tt = $(this).parent().find("em").html();
                    if (ts.indexOf(tt) < 0) {
                        if (tt) {
                            $("#area_up .selectedboxcn em").before(tt + "、");
                        }
                    }
                }
            }
            else { // 复选框未选中状态
                if ($(this).attr('name').indexOf('bp') > -1) { // 父类的文本框
                    var chname = 'bch_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        //alert(pobj.attr('checked'));
                        $(this).attr("checked", false);
                        var tt = $(this).parent().find("em").html();
                        var tslist = $("#area_up .selectedboxcn").html();
                        var tm = tslist.replace(tt + "、", "");
                        $("#area_up .selectedboxcn").html(tm);
                    });
                } else {
                    var cpname = 'bp' + $(this).attr('name').replace("bch", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false)
                    });
                    var tslist = $("#area_up .selectedboxcn").html();
                    var tt = $(this).parent().find("em").html();
                    var tm = tslist.replace(tt + "、", "");
                    $("#area_up .selectedboxcn").html(tm);
                }
            }
        });
    });


    /*设备 弹出框--不选中取消*/

    $("#equip_up input").each(function () {
        $(this).click(function () {
            //alert('');
            var ts = $("#equip_up .selectedboxcn").html();
            if ($(this).attr("checked")) { // 复选框选中状态
                if ($(this).attr('name').indexOf('dbp') > -1) { // 父类的文本框
                    var chname = 'dcha_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {

                        $(this).attr("checked", pobj.attr('checked'));
                        // alert($(this).val());
                        // 建筑范围设备全选
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                            type: 'Post',
                            contentType: "application/x-www-form-urlencoded; charset=utf-8",
                            data: { areaid: $(this).val() },
                            timeout: 1000,
                            success: function (data) {
                                //alert(data);
                                if (data != ']') {
                                    eval("data=" + data);
                                    for (var i = 0; i < data.length; i++) {
                                        var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                        if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                                            $("#equip_up .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                                        }
                                    }
                                    /*var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                    if ($("#equip_up .selectedboxcn em:first span[config=" + $(this).val() + "]")) {
                                    $("#equip_up .selectedboxcn em:first span").remove("span[config=" + $(this).val() + "]");

                                    }*/
                                }
                            }
                        });
                    });
                } else {
                    var cpname = 'dbp' + $(this).attr('name').replace("dcha", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", chobj.attr('checked'))
                    });
                    $.ajax({
                        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                        type: 'Post',
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { areaid: $(this).val() },
                        timeout: 1000,
                        success: function (data) {
                            //alert(data);
                            if (data != ']') {
                                eval("data=" + data);
                                for (var i = 0; i < data.length; i++) {
                                    var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                    if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                                        $("#equip_up .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else { // 复选框未选中状态
                if ($(this).attr('name').indexOf('dbp') > -1) { // 父类的文本框
                    var chname = 'dcha_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        $(this).attr("checked", false);
                        // 建筑范围设备全取消
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                            type: 'Post',
                            contentType: "application/x-www-form-urlencoded; charset=utf-8",
                            data: { areaid: $(this).val() },
                            timeout: 1000,
                            success: function (data) {
                                //alert(data);
                                if (data != ']') {
                                    eval("data=" + data);
                                    for (var i = 0; i < data.length; i++) {
                                        if ($("#equip_up .selectedboxcn em:first span[config=" + data[i].F_MeterID + "]")) {
                                            $("#equip_up .selectedboxcn em:first span").remove("span[config=" + data[i].F_MeterID + "]");

                                        }
                                    }
                                }
                            }
                        });
                    });
                } else {
                    var cpname = 'dbp' + $(this).attr('name').replace("dcha", "");
                    var chobj = $(this);
                    $.ajax({
                        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                        type: 'Post',
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { areaid: $(this).val() },
                        timeout: 1000,
                        success: function (data) {
                            // alert(data);
                            if (data != ']') {
                                eval("data=" + data);
                                for (var i = 0; i < data.length; i++) {
                                    if ($("#equip_up .selectedboxcn em:first span[config=" + data[i].F_MeterID + "]")) {
                                        $("#equip_up .selectedboxcn em:first span").remove("span[config=" + data[i].F_MeterID + "]");

                                    }
                                }
                            }
                        }
                    });
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false)
                    });
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false);
                    });
                }
            }
        });
    });




    /*重置按钮*/
    jQuery("#building_up #chongzhi").click(function () {
        $("#building_up .selectedboxcn").html("" + "<em></em>");
    });
    jQuery("#area_up #chongzhi").click(function () {
        $("#area_up .selectedboxcn").html("" + "<em></em>");
    });
    jQuery("#equip_up #chongzhi").click(function () {
        $("#equip_up .selectedboxcn").html("" + "<em></em>");
    });
    jQuery("#equip_up2 #chongzhi").click(function () {
        $("#equip_up2 table").find("input").each(function (i, val) {
            var tt = $(this).parent().find("em").html();
            if (tt) {
                //alert("0");
                $("#equip_up .selectedboxcn em").remove();
                var tts = $("#equip_up .selectedboxcn").html();
                var ttc = tt + "、";
                if (tts.indexOf(ttc) > 0) {
                    //alert("3");
                    var tts = tts.replace(ttc, "");
                    $("#equip_up .selectedboxcn").html(tts + "<em></em>");
                }
            } else {
                //alert("无效");
            }
        });
    });




    /*区域-设备弹出框按钮*/
    $("#choice img").click(function () {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { areaid: $(this).attr('config') },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    $("#dtext").html('');
                    for (var i = 0; i < data.length; i++) {
                        var html = "";
                        html += "<span>";
                        html += "<input config='" + data[i].F_MeterName + "' name=dch_" + data[i].F_AreaID + " type=\"checkbox\" value=\"" + data[i].F_MeterID + "\" />";
                        html += " <em>" + data[i].F_MeterName + "</em>";
                        html += "</span>";
                        $("#dtext").append(html);
                    }
                    /*编写弹出设备的选择和取消事件*/
                    $("#dtext input").each(function () {
                        $(this).click(function () {
                            if ($(this).attr('checked')) {
                                var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                if (htmlt.indexOf($(this).attr('config')) == -1) {
                                    $("#equip_up .selectedboxcn em:first").append('<span config="' + $(this).val() + '">' + $(this).attr('config') + '、</span>');
                                }
                            } else {
                                var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                if ($("#equip_up .selectedboxcn em:first span[config=" + $(this).val() + "]")) {
                                    $("#equip_up .selectedboxcn em:first span").remove("span[config=" + $(this).val() + "]");

                                }
                            }
                        });

                    });

                    $("#equip_up2").css("display", "block");
                    $(this).attr("src", "images/energy-ranking/arrow_btn_down.gif");
                    $("#bg").show();
                    $("#equip_up2").show();
                } else {
                    $("#dtext").html('');
                    alert('该区域下暂无设备信息。');
                }

            }
        });
    });






    /*缩进*/
    jQuery("#ibtn9").click(function () {
        jQuery(".search_condition").hide();
        jQuery(".indentbox").show();
        jQuery("#show9").css({ 'width': '861px' });
        jQuery(".topside").css({ 'width': '861px', 'background': 'url(images/energy/topside2.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '859px' });
        jQuery(".btmside").css({ 'width': '861px', 'background': 'url(images/energy/btmside2.png) no-repeat 0px 0px' });
        jQuery(".midbox3").css({ 'width': '841px' });
        jQuery(".export_title").css({ 'width': '819px' });
        jQuery(".export_con").css({ 'width': '841px' });
        jQuery(".page").css({ 'width': '841px' });
        jQuery(".ranking_ttbig").css({ 'width': '802px' });
        //jQuery(".rank_ttside").css({'width':'765px'});
        jQuery(".rankhide").css({ 'width': '765px' });
        jQuery(".export_ttsmall").css({ 'width': '819px' });
        jQuery(".export_list").css({ 'width': '417px' });
        jQuery(".table.tbl_exporth").css({ 'width': '417px' });
        jQuery(".table.tbl_exportcn").css({ 'width': '396px' });
        jQuery(".export_list ul").css({ 'width': '396px' });
        jQuery(".export_list ul li").css({ 'width': '396px' });
    });
    jQuery("#ibox9").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery("#show9").css({ 'width': '620px' });
        jQuery(".topside").css({ 'width': '620px', 'background': 'url(images/energy/topside.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '618px' });
        jQuery(".btmside").css({ 'width': '620px', 'background': 'url(images/energy/btmside.png) no-repeat 0px 0px' });
        jQuery(".midbox3").css({ 'width': '600px' });
        jQuery(".export_title").css({ 'width': '578px' });
        jQuery(".export_con").css({ 'width': '600px' });
        jQuery(".page").css({ 'width': '600px' });
        jQuery(".ranking_ttbig").css({ 'width': '561px' });
        //jQuery(".rank_ttside").css({'width':'524px'});
        jQuery(".rankhide").css({ 'width': '524px' });
        jQuery(".export_ttsmall").css({ 'width': '578px' });
        jQuery(".export_list").css({ 'width': '297px' });
        jQuery(".table.tbl_exporth").css({ 'width': '297px' });
        jQuery(".table.tbl_exportcn").css({ 'width': '276px' });
        jQuery(".export_list ul").css({ 'width': '276px' });
        jQuery(".export_list ul li").css({ 'width': '276px' });
    });


    /*功能区*/
    jQuery(".datesort_fast_list ul li").click(function () {
        var ss = jQuery(this).html();
        jQuery(".datesort_fast_list ul li.data_select").html(ss);
        switch (ss) {
            case "本月":
                $("#starttime").val(GetCuttMonth());
                $("#endtime").val(GetToday());
                break;
            case "本周":
                var time = new Date();
               // time.setDate(time.getDate() - time.getDay() + 1);
                //alert(time.toLocaleDateString());
                $("#starttime").val(GetWeekFirstDay());
                $("#endtime").val(GetToday());
                break;
            default:
                /*
                var bb11 = jQuery("#date01").val();
                var bb12 = jQuery("#hour01").val();
                var bb21 = jQuery("#date02").val();
                var bb22 = jQuery("#hour02").val();
                */
                $("#starttime").val(GetToday());
                $("#endtime").val(GetNow());
        }

        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
    });

    /*自定义查询*/
    jQuery("span.datesort_nav em").click(function () {
        jQuery("#custombox").show();
    });
    jQuery(".datesort_custom span").click(function () {
        jQuery("#custombox").hide();
    });




    /*选择分类*/
    jQuery(".sort_select em").click(function () {
        jQuery("#sortss").show();
        jQuery("#sortst").show();
    });
    jQuery(".electric").click(function () {
        var s1 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s1);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    jQuery(".light_tt").click(function () {
        var s2 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s2);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    jQuery(".light_cn").click(function () {
        var s3 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s3);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    jQuery(".light_tts").click(function () {
        var s4 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s4);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    jQuery(".light_cns").click(function () {
        var s5 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s5);
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
        $(".sort_select span.val").attr("config", $(this).attr('config'));
    });
    jQuery(".light_cns2").click(function () {
        var s6 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s6);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    jQuery(".light_cns3").click(function () {
        var s7 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s7);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    jQuery(".light_cns4").click(function () {
        var s8 = jQuery(this).html();
        jQuery(".sort_select span.val").html(s8);
        $(".sort_select span.val").attr("config", $(this).attr('config'));
        jQuery("#sortss").hide();
        jQuery("#sortst").hide();
    });
    /*分类分项展开按钮*/
    /*分类分项展开按钮*/
    jQuery(".arrowbtns").click(function () {
        var index = jQuery(".arrowbtns").index(this);
        jQuery(this).addClass("arrowbtnsbg").siblings().removeClass("arrowbtnsbg");
        jQuery(this).parent().parent().parent().parent().find("#H").show();
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

                    jQuery(".light_cns2").click(function () {
                        var s6 = jQuery(this).html();
                        jQuery(".sort_select span.val").html(s6);
                        jQuery("#sortss").hide();
                        jQuery("#sortst").hide();
                        $(".sort_select span.val").attr("config", $(this).attr('config'));

                        /* jQuery("input#equip_sort_edit").val(s6);
                        jQuery("#sortss").hide();
                        jQuery("#sortst_edit").hide();
                        $("#EF_EnergyItemCode").val($(this).attr('config'));*/
                    });

                } else {
                    //$("#collectoruselist").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });
    });

    /*日历*/
    jQuery("#hours01").mouseover(function () {
        jQuery("#hh01").show();
    });
    jQuery("#hours01").mouseout(function () {
        jQuery("#hh01").hide();
    });
    jQuery("#hh01 table td").click(function () {
        var sd = jQuery(this).attr('config');
        $("#hour01").val(sd);
    });
    jQuery("#hours02").mouseover(function () {
        jQuery("#hh02").show();
    });
    jQuery("#hours02").mouseout(function () {
        jQuery("#hh02").hide();
    });
    jQuery("#hh02 table td").click(function () {
        var sd = jQuery(this).attr('config');
        jQuery("#hour02").val(sd);
    });



    /*排序弹出框按钮注册区*/
    $("#pr1").css("cursor", "pointer");
    $("#pr2").css("cursor", "pointer");
    $("#pr3").css("cursor", "pointer");
    $("#pr4").css("cursor", "pointer");
    $("#pr1").click(function () {
        $("#topnum").val('10');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });
    $("#pr2").click(function () {
        $("#topnum").val('20');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });

    $("#pr3").click(function () {
        $("#topnum").val('50');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });

    $("#pr4").click(function () {
        $("#topnum").val('100');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });

    $("#next1").css("cursor", "pointer");
    $("#next2").css("cursor", "pointer");
    $("#next3").css("cursor", "pointer");
    $("#next4").css("cursor", "pointer");
    $("#next1").click(function () {
        $("#topnum").val('10');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });
    $("#next2").click(function () {
        $("#topnum").val('20');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });

    $("#next3").click(function () {
        $("#topnum").val('50');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });

    $("#next4").click(function () {
        $("#topnum").val('100');
        var data = {
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            itemcode: $('.val').attr('config'),
            olist: $("#olist").val(),
            isall: $("#isall").val() == '' ? '0' : $("#isall").val(),
            topnum: $("#topnum").val() == '' ? '-1' : $("#topnum").val(),
            order: $("#order").val(),
            tjclass: $("#tjclass").val()
        }
        new exportdata().showorderlist(data);
        jQuery(".popupbox").hide();
        jQuery("#paixubox").hide();
    });
});



