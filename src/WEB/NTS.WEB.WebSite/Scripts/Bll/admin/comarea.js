var mainchart = '';
var comarea = function () { };
NTSAreaCompare = comarea.prototype = {
    name: '区域对比',
    showChartImg: function () {

        //return;
        setDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择查询的对象。');
            return;
        }
        var clickunit = "clickunit";
        ReportData.BaseData[clickunit] = $("#isall").val();
        ReportCompareData.BaseData[clickunit] = $("#isall").val();
        var serverUrl = '';
        var ReportDatas = '';

        $("#qlabel,#gundongtxt").html($("#objectname").val().substring(1));
        if (Number($("#issametime").val()) == 0) {
            $(".titles").html($("#starttime").val() + " - " + $("#endtime").val());
        } else {
            var d = new Date();
            if ($("#unit").val() == "5") {
                $(".titles").html($("#cyear").val() + "年" + " VS " + d.getFullYear() + "年");
            }
            else if ($("#unit").val() == "3") {
                $(".titles").html($("#cmonth").val() + " VS " + d.getFullYear() + "-" + (d.getMonth() + 1));
            } else {
                $(".titles").html($("#cdate").val() + " VS " + GetToday());
            }
        }

        $(".ranking_ttside").html("<em style='color:#41a1be'>区域能耗</em><font color='green'>" + $(".itemclass").find("option:selected").text().replace('└', '').replace('├', '').trim() + "分项</font><em style='color:#41a1be'>对比</em>");
        // 显示基本的对比图
        serverUrl = Number($("#issametime").val()) == 0 ? AjaxUrl.EneryCompareQueryChart : AjaxUrl.SameEneryCompareQueryChart;
        ReportDatas = Number($("#issametime").val()) == 0 ? ReportData.BaseData : ReportCompareData.BaseData;
        mainchart = $("#compare_bigpic").AjaxHighCharts({
            url: serverUrl,
            data: ReportDatas,
            charts: {
                type: 'muticolumn',
                chartUnit: true,
                showLegend: true,
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                itemUnit: true
            }
        });
    }
};


$(function () {
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#itemcode").val('1');
    $("#unit").val('3');
    $("#issametime").val('0');
    $("#AreaType").change(function () {
        window.location = "?AreaType=" + $(this).val();
    });

});

function ShowChartQueryReport() {

    NTSAreaCompare.showChartImg();
}

function ConfirmDiv6() {

    jQuery("#bg03").show();
    jQuery("#add_up03").show();

}

function DivClosed6() {
    jQuery(".popupbox").hide();
    jQuery("#area_up3").hide();
    jQuery("#area_up3 .selectedboxcn").html("");
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
            ReportCompareData.BaseData.starttime1 = $("#cyear").val() + "-1-1";
            ReportCompareData.BaseData.endtime1 = GetLastYearMyDate($("#cyear").val());
            ReportCompareData.BaseData.starttime2 = GetCuttYear() + "-1-1";
            ReportCompareData.BaseData.endtime2 = GetToday(); ;
            $("#unit").val(5);
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
    ShowChartQueryReport();
}

$(function () {

    jQuery(".search_button span").click(function () {
        var ss = jQuery(this).html();
        $(this).addClass("select").siblings().removeClass("select");
        switch (ss) {
            case "本月":
                $("#starttime").val(GetCuttMonth());
                $("#endtime").val(GetToday());
                $("#unit").val('3');
                $("#issametime").val('0');
                break;
            case "本周":
                $("#starttime").val(GetWeekFirstDay());
                $("#endtime").val(GetToday());
                $("#unit").val('2');
                $("#issametime").val('0');
                break;
            default:
                $("#starttime").val(GetToday());
                $("#endtime").val(GetNow());
                $("#unit").val('1');
                $("#issametime").val('0');
        }
        ShowChartQueryReport();
    });

    jQuery(".sidecolumn_middle .title span.query_senior").click(function () {
        jQuery(".sidecolumn_middle .title span.query").removeClass("pitch_on");
        jQuery(this).addClass("pitch_on");
        // jQuery(".search_button").hide();
        // jQuery(".date_button").show();
        //jQuery("#custombox").hide();
        $("body").append('<div class="custom_box"><table width="240" style="color:#5D5D5D;float:left"><tr height="18"><td colspan="2"></td></tr><tr height="35"><td width="85" align="right">开始时间：</td><td width="155" align="left"><input class="alarm_datetime" id="stime" type="text" Readonly value="" onclick="return SelectDate(this,\'yyyy-MM-dd\')" style="width:120px;padding-left:8px"></td></tr><tr height="35"><td width="85" align="right">结束时间：</td><td width="155" align="left"><input Readonly class="alarm_datetime" type="text" id="etime" value="" onclick="return SelectDate(this,\'yyyy-MM-dd\')" style="width:120px;padding-left:8px"></td></tr><tr height="40" align="center"><td width="85"></td><td width="155" align="left"><span class="custom_submit">确定</span><span class="custom_cancel">取消</span></td></tr></table></div>');
        //input box change	   
        $("input.alarm_datetime").focus(function () {
            $(this).css('border', '1px solid #167fc5');
        });
        $("input.alarm_datetime").blur(function () {
            $(this).css('border', '1px solid #cecece');
        });
        //submit
        $("span.custom_submit").click(function () {
            if ($("#stime").val() == '' || $("#etime").val() == '') {
                alert('请输入统计的起始和结束时间。');
                return;
            } else {
                var stime = $("#stime").val();
                var etime = $("#etime").val();
                if (compareTime(stime, etime)) {
                    $("#issametime").val('0');
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
        //cancel
        $("span.custom_cancel").click(function () {
            $(".popupbox").hide();
            $(".custom_box").remove();
        });
    });


    /*缩进*/
    jQuery("#ibtn7").click(function () {
        jQuery(".search_condition").hide();
        jQuery(".indentbox").show();
        jQuery("#show7").css({ 'width': '959px' });
        jQuery(".topside").css({ 'width': '959px', 'background': 'url(images/energy/top_side02.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '957px' });
        jQuery(".btmside").css({ 'width': '959px', 'background': 'url(images/energy/bottom_side02.png) no-repeat 0px 0px' });
        jQuery(".midbox2").css({ 'width': '927px' });
        jQuery(".midcn").css({ 'width': '927px' });
        jQuery(".ranking_ttbig").css({ 'width': '900px' });
        jQuery(".midcnrt").css({ 'width': '922px' });
        jQuery(".smalltitle").css({ 'width': '927px' });
        jQuery(".smalltitle span.titles").css({ 'width': '922px' });
        jQuery(".biaogan_box").css({ 'left': '722px' });
        jQuery(".contrast").css({ 'left': '857px' });
        jQuery(".compare_bigpic").css({ 'width': '927px' });
        jQuery(".bigshowbox").css({ 'width': '628px' });
        jQuery(".bigshowbox img").css({ 'width': '628px' });
        jQuery(".midcn_smalltitle").css({ 'width': '380px' });
        jQuery(".midcn_smallpic").css({ 'width': '380px' });
        jQuery(".midcn_smallpic img").css({ 'width': '380px' });
        jQuery(".midcn_xuxian").css({ 'width': '380px' });
        jQuery(".midcn_sg").css({ 'width': '380px' });
        jQuery(".midcn_sgbtn").css({ 'width': '380px' });
        mainchart.setSize(927, $(".compare_bigpic").height());
    });
    jQuery("#ibox7").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery("#show7").css({ 'width': '718px' });
        jQuery(".topside").css({ 'width': '718px', 'background': 'url(images/energy/top_side01.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '717px' });
        jQuery(".btmside").css({ 'width': '718px', 'background': 'url(images/energy/bottom_side01.png) no-repeat 0px 0px' });
        jQuery(".midbox2").css({ 'width': '686px' });
        jQuery(".midcn").css({ 'width': '686px' });
        jQuery(".midcnlf").css({ 'width': '686px' });
        jQuery(".midcnrt").css({ 'width': '681px' });
        jQuery(".smalltitle").css({ 'width': '686px' });
        jQuery(".smalltitle span.titles").css({ 'width': '681px' });
        jQuery(".biaogan_box").css({ 'left': '481px' });
        jQuery(".contrast").css({ 'left': '616px' });
        jQuery(".compare_bigpic").css({ 'width': '698px' });
        jQuery(".bigshowbox").css({ 'width': '508px' });
        jQuery(".bigshowbox img").css({ 'width': '508px' });
        jQuery(".midcn_smalltitle").css({ 'width': '259px' });
        jQuery(".midcn_smallpic").css({ 'width': '259px' });
        jQuery(".midcn_smallpic img").css({ 'width': '259px' });
        jQuery(".midcn_xuxian").css({ 'width': '259px' });
        jQuery(".midcn_sg").css({ 'width': '259px' });
        jQuery(".midcn_sgbtn").css({ 'width': '259px' });
        mainchart.setSize(698, $(".compare_bigpic").height());
    });
});



jQuery(document).ready(function () {

    var clearTimer = null;
    var SleepTime = 3000;   //停留时长，单位毫秒
    jQuery(".rt_nav_list").jCarouselLite({
        btnPrev: "span.rt_topbtn",
        btnNext: "span.rt_btmbtn",
        visible: 7,
        scroll: 1,
        vertical: true,
        speed: 500
    });

});


$(function () {
    //公共变量
    var nianss = "2012";
    var yuess = "1";

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

    $(".unitusebox ul li").click(function () {
        $(".unitusebox em").html($(this).html());
        $("#isall").val($(this).attr('config'));
        ShowChartQueryReport();
    });

    /*同期对比按钮*/
    jQuery("#contrast_title").click(function () {
        jQuery("em#build_area").html("同期对比");
    });
    jQuery(".contrast").mouseover(function () {
        jQuery(".contrastsub").show();
    });
    jQuery(".contrast").mouseout(function () {
        jQuery(".contrastsub").hide();
    });
    /*同期对比下拉鼠标移上去变色*/
    jQuery(".contrastsub span").hover(
        function () {
            jQuery(this).addClass("white");
        },
        function () {
            jQuery(this).removeClass("white");
        }
    );
    /*日历按钮*/
    jQuery("#nian_cont").click(function () {
        jQuery("#niandiv").show();
        jQuery("#monthdiv").hide();
        jQuery("#daydiv").hide();
    });
    jQuery("#month_cont").click(function () {
        jQuery("#niandiv").hide();
        jQuery("#monthdiv").show();
        jQuery("#daydiv").hide();
    });
    jQuery("#day_cont").click(function () {
        jQuery("#niandiv").hide();
        jQuery("#monthdiv").hide();
        jQuery("#daydiv").show();
    });
    /*年份下拉*/
    jQuery("#nians").mouseover(function () {
        jQuery(".niansbox").show();
    });
    jQuery("#nians").mouseout(function () {
        jQuery(".niansbox").hide();
    });
    jQuery(".niansbox div em").click(function () {
        var tt = jQuery(this).html();
        jQuery("#nians input").val(tt);
    });
    /*年月日历*/
    jQuery("#monthes input.contrastbtn").click(function () {
        jQuery(".yuessbox").show();
    });
    jQuery(".yuessbox span.yearbox").mouseover(function () {
        jQuery(".yuessbox .yearlist").show();
    });
    jQuery(".yuessbox span.yearbox").mouseout(function () {
        jQuery(".yuessbox .yearlist").hide();
    });
    jQuery(".yuessbox .yearlist ul li").click(function () {
        var tt = jQuery(this).html();
        jQuery(".yuessbox .yearbox em").html(tt);
        nianss = jQuery(".yuessbox .yearbox em").html();
        jQuery("#monthes input.contrastbtn").val(nianss + "-" + yuess);
    });
    jQuery(".yuessbox table.tbl_yue span").click(function () {
        yuess = jQuery(this).html().replace("月", "");
        jQuery("#monthes input.contrastbtn").val(nianss + "-" + yuess);
    });
    jQuery(".yuessbox .confirmbtn em").click(function () {
        //jQuery(".jidussbox").hide();
        jQuery(".yuessbox").hide();
    });
});