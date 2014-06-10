var deviceChart = null;
function devicecompare() { };

NTSDeviceCompare = devicecompare.prototype = {
    name: '设备对比',
    showChartsList: function () {
        $("#qlabel,#gundongtxt").html($("#objectname").val().substring(1));
        if (Number($("#issametime").val()) == 0) {
            $(".equ_title_small").html($("#starttime").val() + " - " + $("#endtime").val());
        } else {
            var d = new Date();
            if ($("#unit").val() == "5") {
                $(".equ_title_small").html($("#cyear").val() + "年" + " VS " + d.getFullYear() + "年");
            }
            else if ($("#unit").val() == "3") {
                $(".equ_title_small").html($("#cmonth").val() + " VS " + d.getFullYear() + "-" + (d.getMonth() + 1));
            } else {
                $(".equ_title_small").html($("#cdate").val() + " VS " + GetToday());
            }
        }

        $(".equ_title_big").html("<em style='color:#41a1be'>设备能耗</em><font color='green'>" + $(".select").find("option:selected").text().replace('└', '').replace('├', '').trim() + "分项</font><em style='color:#41a1be'>对比</em>");

        $("#itemcode").val($(".select :selected").val());

        setDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择查询的对象。');
            return;
        }
        ReportData.BaseData.objecttype = 1;

        ReportCompareData.BaseData.itemcode = $("#itemcode").val();
        ReportCompareData.BaseData.objectid = $("#objectid").val();
        ReportCompareData.BaseData.unit = $("#unit").val();
        ReportCompareData.BaseData.objecttype = 1;


        var url = Number($("#issametime").val()) == 0 ? AjaxUrl.DeviceCompare : AjaxUrl.DeviceDateCompare;
        var basedata = Number($("#issametime").val()) == 0 ? ReportData.BaseData : ReportCompareData.BaseData;

        deviceChart = $("#equ_img").AjaxHighCharts({
            url: url,
            data: basedata,
            charts: {
                type: 'muticolumn',
                chartUnit: true,
                itemUnit: true,
                useTicker: true,
                xStep: 6,
                showDataLabel: false,
                showLegend: true,
                yShow: true
            }
        });
    }
};


$(document).ready(function () {
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#unit").val('3');
    $("#issametime").val('0');

    /*周 年 月查询*/
    $(".search_button span").click(function () {
        $("#issametime").val('0');

        GetSelectedObject();

        var ss = $(this).html();
        $(this).addClass("select").siblings().removeClass("select");
        switch (ss) {
            case "本月":
                $("#starttime").val(GetCuttMonth());
                $("#endtime").val(GetToday());
                $("#unit").val('3');
                break;
            case "本周":
                $("#starttime").val(GetWeekFirstDay());
                $("#endtime").val(GetToday());
                $("#unit").val('2');
                break;
            default:
                $("#starttime").val(GetToday());
                $("#endtime").val(GetNow());
                $("#unit").val('1');
        }
        ShowChartQueryReport();
    });

    /*日期自定义查询*/
    $(".sidecolumn_middle .title span.query_senior").click(function () {
        $("#issametime").val('0');

        $("#objectid").val('');
        $("#objectname").val('');
        var chooseobject = '';
        var chooseobjectname = '';

        $("li.select_d div span.checkbox").each(function () {
            chooseobject += ',' + $(this).attr('config');
            chooseobjectname += ',' + $(this).attr('nconfig');
        });

        $("#objectid").val(chooseobject);
        $("#objectname").val(chooseobjectname);

        $(".sidecolumn_middle .title span.query").removeClass("pitch_on");
        $(this).addClass("pitch_on");

        $("body").append('<div class="custom_box"><table width="240" style="color:#5D5D5D;float:left"><tr height="18"><td colspan="2"></td></tr><tr height="35"><td width="85" align="right">开始时间：</td><td width="155" align="left"><input class="alarm_datetime" id="stime" type="text" Readonly value="" onclick="return SelectDate(this,\'yyyy-MM-dd\')" style="width:120px;padding-left:8px"></td></tr><tr height="35"><td width="85" align="right">结束时间：</td><td width="155" align="left"><input Readonly class="alarm_datetime" type="text" id="etime" value="" onclick="return SelectDate(this,\'yyyy-MM-dd\')" style="width:120px;padding-left:8px"></td></tr><tr height="40" align="center"><td width="85"></td><td width="155" align="left"><span class="custom_submit">确定</span><span class="custom_cancel">取消</span></td></tr></table></div>');

        $("input.alarm_datetime").focus(function () {
            $(this).css('border', '1px solid #167fc5');
        });
        $("input.alarm_datetime").blur(function () {
            $(this).css('border', '1px solid #cecece');
        });

        $("span.custom_submit").click(function () {
            if ($("#stime").val() == '' || $("#etime").val() == '') {
                alert('请输入统计的起始和结束时间。');
                return;
            }
            else {
                var stime = $("#stime").val();
                var etime = $("#etime").val();
                if ($.compareTime(stime, etime)) {
                    $(".popupbox").hide();
                    $(".custom_box").remove();
                    $("#unit").val('5');
                    $("#starttime").val(stime);
                    $("#endtime").val(etime);
                    $(".search_button span").removeClass("select");
                    ShowChartQueryReport();
                }
            }
        });

        $("span.custom_cancel").click(function () {
            $(".popupbox").hide();
            $(".custom_box").remove();
        });
    });


    var nianss = "2012";
    var yuess = "1";
    /*同期对比按钮*/
    $("#contrast_title").click(function () {
        $("em#build_area").html("同期对比");
    });
    $(".contrast").mouseover(function () {
        $(".contrastsub").show();
    });
    $(".contrast").mouseout(function () {
        $(".contrastsub").hide();
    });
    /*同期对比下拉鼠标移上去变色*/
    $(".contrastsub span").hover(
        function () {
            $(this).addClass("white");
        },
        function () {
            $(this).removeClass("white");
        }
    );
    /*日历按钮*/
    $("#nian_cont").click(function () {
        $("#niandiv").show();
        $("#monthdiv").hide();
        $("#daydiv").hide();
    });
    $("#month_cont").click(function () {
        $("#niandiv").hide();
        $("#monthdiv").show();
        $("#daydiv").hide();
    });
    $("#day_cont").click(function () {
        $("#niandiv").hide();
        $("#monthdiv").hide();
        $("#daydiv").show();
    });
    /*年份下拉*/
    $("#nians").mouseover(function () {
        $(".niansbox").show();
    });
    $("#nians").mouseout(function () {
        $(".niansbox").hide();
    });
    $(".niansbox div em").click(function () {
        var tt = $(this).attr("config");
        $("#nians input").val(tt);
    });
    /*年月日历*/
    $("#monthes input.contrastbtn").click(function () {
        $(".yuessbox").show();
    });
    $(".yuessbox span.yearbox").mouseover(function () {
        $(".yuessbox .yearlist").show();
    });
    $(".yuessbox span.yearbox").mouseout(function () {
        $(".yuessbox .yearlist").hide();
    });
    $(".yuessbox .yearlist ul li").click(function () {
        var tt = $(this).html();
        $(".yuessbox .yearbox em").html(tt);
        nianss = $(".yuessbox .yearbox em").html();
        $("#monthes input.contrastbtn").val(nianss + "-" + yuess);
    });
    $(".yuessbox table.tbl_yue span").click(function () {
        yuess = $(this).html().replace("月", "");
        $("#monthes input.contrastbtn").val(nianss + "-" + yuess);
    });
    $(".yuessbox .confirmbtn em").click(function () {
        $(".yuessbox").hide();
    });

    /*缩进*/
    jQuery("#ibtn3").click(function () {
        jQuery(".search_condition").hide();
        jQuery(".indentbox").show();
        jQuery("#show3").css({ 'width': '959px' });
        jQuery(".topside").css({ 'width': '959px', 'background': 'url(images/energy/top_side02.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '957px' });
        jQuery(".btmside").css({ 'width': '959px', 'background': 'url(images/energy/bottom_side02.png) no-repeat 0px 0px' });
        jQuery(".equ_title").css({ 'width': '957px' });
        jQuery(".btnsbox").css({ 'left': '689px' });
        jQuery(".contrast").css({ 'left': '865px' });
        jQuery(".equ_imgbox").css({ 'width': '935px' });
        jQuery(".equipmentbox").css({ 'width': '902px' });
        jQuery(".equ_title_big").css({ 'width': '957px' });
        jQuery(".equ_title_small").css({ 'width': '957px' });
        jQuery(".equ_img").css({ 'width': '935px' });
        jQuery(".equ_img img").css({ 'width': '935px' });
        jQuery(".equipment_list").css({ 'width': '848px' });
        $("#ismax").val('max');
        deviceChart.setSize(935, $("#equ_img").height());
    });
    jQuery("#ibox3").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery("#show3").css({ 'width': '718px' });
        jQuery(".topside").css({ 'width': '718px', 'background': 'url(images/energy/top_side01.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '717px' });
        jQuery(".btmside").css({ 'width': '718px', 'background': 'url(images/energy/bottom_side01.png) no-repeat 0px 0px' });
        jQuery(".equ_title").css({ 'width': '716px' });
        jQuery(".btnsbox").css({ 'left': '448px' });
        jQuery(".contrast").css({ 'left': '624px' });
        jQuery(".equ_imgbox").css({ 'width': '694px' });
        jQuery(".equipmentbox").css({ 'width': '661px' });
        jQuery(".equ_title_big").css({ 'width': '716px' });
        jQuery(".equ_title_small").css({ 'width': '716px' });
        jQuery(".equ_img").css({ 'width': '694px' });
        jQuery(".equ_img img").css({ 'width': '694px' });
        jQuery(".equipment_list").css({ 'width': '607px' });
        jQuery("#ibtn3").show();
        $("#ismax").val('');
        deviceChart.setSize(694, $("#equ_img").height());
    });


});

function ShowChartQueryReport() {
    NTSDeviceCompare.showChartsList();
}

function NianCloseDiv() {
    jQuery(".niandiv").hide();
}
/**日历按钮**/
function NianConfirmDiv(v) {
    $("#issametime").val('1');
    switch (v) {
        case 1:
            $("#mycdate").val($("#cyear").val());
            /*
            ReportCompareData.BaseData.starttime1 = $("#cyear").val() + "-1-1";
            ReportCompareData.BaseData.endtime1 = $("#cyear").val() + "-12-31";
            ReportCompareData.BaseData.starttime2 = GetCuttYear() + "-1-1";
            ReportCompareData.BaseData.endtime2 = GetCuttYear() + "-12-31";
            */
            ReportCompareData.BaseData.starttime1 = $("#cyear").val() + "-1-1";
            ReportCompareData.BaseData.endtime1 = GetLastYearMyDate($("#cyear").val());
            ReportCompareData.BaseData.starttime2 = GetCuttYear() + "-1-1";
            ReportCompareData.BaseData.endtime2 = GetToday(); ;

            $("#unit").val(5);
            //alert(ReportCompareData.BaseData.endtime2);
            //  return;
            break;
        case 2:
            $("#mycdate").val($("#cmonth").val());
            ReportCompareData.BaseData.starttime1 = $("#cmonth").val() + "-1";
            ReportCompareData.BaseData.endtime1 = GetMyMonth($("#cmonth").val());
            ReportCompareData.BaseData.starttime2 = GetCuttMonth();
            ReportCompareData.BaseData.endtime2 = GetMonthLast();
            $("#unit").val(3);
            break;
        case 3:
            $("#mycdate").val($("#cdate").val());
            ReportCompareData.BaseData.starttime1 = $("#cdate").val();
            ReportCompareData.BaseData.endtime1 = GetMyDate($("#cdate").val());
            ReportCompareData.BaseData.starttime2 = GetToday();
            ReportCompareData.BaseData.endtime2 = GetNow();
            $("#unit").val(1);
            break;
    }
    setCompareDefault();
    jQuery(".niandiv").hide();
    GetSelectedObject();
    ShowChartQueryReport();
}

function GetSelectedObject() {
    $("#objectid").val('');
    $("#objectname").val('');
    var chooseobject = '';
    var chooseobjectname = '';

    $("li.select_d div span.checkbox").each(function () {
        chooseobject += ',' + $(this).attr('config');
        chooseobjectname += ',' + $(this).attr('nconfig');
    });

    $("#objectid").val(chooseobject);
    $("#objectname").val(chooseobjectname);
}