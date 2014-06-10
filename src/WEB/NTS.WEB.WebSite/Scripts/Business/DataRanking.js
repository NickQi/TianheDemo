var rankingChart = null;
var dataModel = null;
function dataranking() { };

NTSDataRanking = dataranking.prototype = {
    name: '能耗排名',
    showChartsList: function () {
        $("#qlabel,#gundongtxt").html($("#objectname").val().substring(1));
        $(".export_ttsmall").html($("#starttime").val() + " - " + $("#endtime").val());
        $(".export_tt").html("<font color='green'>" + $(".itemclass").find("option:selected").text().replace('└', '').replace('├', '').trim() + "分项</font><em style='color: #5d87c3'>能耗排名</em>");

        ReportRankingData.BaseData.starttime = $("#starttime").val();
        ReportRankingData.BaseData.endtime = $("#endtime").val();
        ReportRankingData.BaseData.objectid = $("#objectid").val();
        ReportRankingData.BaseData.unit = $("#unit").val();
        ReportRankingData.BaseData.itemcode = $("#itemcode").val();
        ReportRankingData.BaseData.type = $("#type").val();
        ReportRankingData.BaseData.order = $("#order").val();
        ReportRankingData.BaseData.topnum = $("#topnum").val();
        ReportRankingData.BaseData.unittype = $("#clickunit").val();

        if ($("#mainLayer").get(0).options[$("#mainLayer").get(0).selectedIndex].text == '设备') {
            ReportRankingData.BaseData.objecttype = 1;
            ReportRankingData.BaseData.type = 0;
            $(".unitusebox").hide();
        } else {
            $(".unitusebox").show();
        }

        rankingChart = $("#ranking_cnimg").AjaxHighCharts({
            url: AjaxUrl.ShowDataRanking,
            data: ReportRankingData.BaseData,
            charts: {
                type: 'bar',
                yTitle: '能耗排名',
                chartUnit: true,
                itemUnit: true,
                useTicker: false,
                showDataLabel: false,
                yShow: false
            }
        });

        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.ShowDataRankingList,
            data: ReportRankingData.BaseData,
            pageSize: 20,
            callback: function (listId, result) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(result)) && ko.applyBindings(dataModel, document.getElementById(listId)))) && ko.mapping.fromJS(result, dataModel);
            }
        });
    }
};


$(document).ready(function () {
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#unit").val('3');
    $("#type").val('0');
    $("#order").val('asc');
    $("#topnum").val('100');

    $("#AreaType").change(function () {
        window.location = "?AreaType=" + $(this).val() + "&type=" + $(".itemclass :selected").val() + "&rnd=" + new Date().getTime();
    });


    /*周 年 月查询*/
    $(".search_button span").click(function () {
        $("#objectid").val('');
        $("#objectname").val('');
        var type = $("#clickunit").val();
        var chooseobject = '';
        var chooseobjectname = '';

        switch (type) {
            case "1":
                $("li.select_a div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
            case "2":
                $("li.select_b div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
            case "3":
                $("li.select_c div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
            case "4":
                $("li.select_d div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
        }

        $("#objectid").val(chooseobject);
        $("#objectname").val(chooseobjectname);
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

        $("#objectid").val('');
        $("#objectname").val('');
        var type = $("#clickunit").val();
        var chooseobject = '';
        var chooseobjectname = '';

        switch (type) {
            case "1":
                $("li.select_a div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
            case "2":
                $("li.select_b div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
            case "3":
                $("li.select_c div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
            case "4":
                $("li.select_d div span.checkbox").each(function () {
                    chooseobject += ',' + $(this).attr('config');
                    chooseobjectname += ',' + $(this).attr('nconfig');
                });
                break;
        }

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


    /*排序*/
    $("#paixubtn").click(function () {
        $(".popupbox").show();
        $("#paixubox").show();
        $("#prevtxt").val('100');
    });

    $("#arrow01").click(function () {
        $("#rankbox01").hide();
        $("#rankbox02").show();
        $("#order").val('desc');
    });
    $("#arrow02").click(function () {
        $("#rankbox01").show();
        $("#rankbox02").hide();
        $("#order").val('asc');
    });

    /*排序弹出框按钮注册区*/
    $("#pr1").css("cursor", "pointer");
    $("#pr2").css("cursor", "pointer");
    $("#pr3").css("cursor", "pointer");
    $("#pr4").css("cursor", "pointer");
    $("#pr1").click(function () {
        $("#topnum").val('10');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#pr2").click(function () {
        $("#topnum").val('20');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#pr3").click(function () {
        $("#topnum").val('50');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#pr4").click(function () {
        $("#topnum").val('100');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });

    $("#next1").css("cursor", "pointer");
    $("#next2").css("cursor", "pointer");
    $("#next3").css("cursor", "pointer");
    $("#next4").css("cursor", "pointer");
    $("#next1").click(function () {
        $("#topnum").val('10');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#next2").click(function () {
        $("#topnum").val('20');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#next3").click(function () {
        $("#topnum").val('50');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#next4").click(function () {
        $("#topnum").val('100');
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });

    $("#prevsort").click(function () {
        $("#topnum").val($("#prevtxt").val());
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });
    $("#nextsort").click(function () {
        $("#topnum").val($("#nexttxt").val());
        $(".popupbox").hide();
        $("#paixubox").hide();
        ShowChartQueryReport();
    });

    /*单位用量*/
    $(".unitusebox").mouseover(function () {
        $(".unitusebox ul").show();
    });

    $(".unitusebox").mouseout(function () {
        $(".unitusebox ul").hide();
    });

    $(".unitusebox li").click(function () {
        $("#chooselabel").html($(this).html());
        $("#type").val($(this).attr('config'));
        ShowChartQueryReport();
    });
    $("#allcheck_build").trigger("click");




    /*缩进*/
    jQuery("#ibtn9").click(function () {
        jQuery(".search_condition").hide();
        jQuery(".indentbox").show();
        jQuery(".search_show2").css({ 'width': '959px' });
        jQuery(".topside").css({ 'width': '959px', 'background': 'url(images/energy/top_side02.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '957px' });
        jQuery(".btmside").css({ 'width': '959px', 'background': 'url(images/energy/bottom_side02.png) no-repeat 0px 0px' });
        jQuery(".midbox").css({ 'width': '917px' });
        jQuery(".ranking_title").css({ 'width': '917px' });
        jQuery(".export_title").css({ 'width': '917px' });
        jQuery(".export_ttsmall").css({ 'width': '917px' });
        jQuery(".unitusebox").css({ 'left': '845px' });
        jQuery(".ranking_cn").css({ 'width': '917px' });
        jQuery(".ranking_ttsmall").css({ 'width': '917px' });
        jQuery(".ranking_cnimg").css({ 'width': '430px' });
        jQuery(".ranking_list").css({ 'width': '467px' });
        jQuery(".rank_name").css({ 'width': '467px' });
        jQuery(".tbl_rank").css({ 'width': '467px' });
        jQuery(".page").css({ 'width': '467px' });
        jQuery(".gundongbox").css({ 'width': '915px' });
        $("#ismax").val("max");
        rankingChart.setSize(488, $("#ranking_cnimg").height());
    });
    jQuery("#ibox9").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery(".search_show2").css({ 'width': '718px' });
        jQuery(".topside").css({ 'width': '718px', 'background': 'url(images/energy/top_side01.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '716px' });
        jQuery(".btmside").css({ 'width': '718px', 'background': 'url(images/energy/bottom_side01.png) no-repeat 0px 0px' });
        jQuery(".midbox").css({ 'width': '676px' });
        jQuery(".ranking_title").css({ 'width': '676px' });
        jQuery(".export_title").css({ 'width': '676px' });
        jQuery(".export_ttsmall").css({ 'width': '676px' });
        jQuery(".unitusebox").css({ 'left': '604px' });
        jQuery(".ranking_cn").css({ 'width': '676px' });
        jQuery(".ranking_ttsmall").css({ 'width': '676px' });
        jQuery(".ranking_cnimg").css({ 'width': '359px' });
        jQuery(".ranking_list").css({ 'width': '297px' });
        jQuery(".rank_name").css({ 'width': '297px' });
        jQuery(".tbl_rank").css({ 'width': '297px' });
        jQuery(".page").css({ 'width': '297px' });
        jQuery(".gundongbox").css({ 'width': '674px' });
        $("#ismax").val("small");
        rankingChart.setSize(363, $("#ranking_cnimg").height());
    });


    //导出Excel
    $("#exportbtn").click(function () {
        $(this).FileExport({
            url: AjaxUrl.ExportDataRankingList,
            data: ReportRankingData.BaseData
        });
    });
    //排序
    $('.paixu_asc img').click(function () {
        $('.paixu_asc').hide();
        $('.paixu_desc').show();
        $("#order").val('desc');
    });
    $('.paixu_desc img').click(function () {
        $('.paixu_desc').hide();
        $('.paixu_asc').show();
        $("#order").val('asc');
    });
    $('.rank_name a').click(function () {
        $(this).addClass("selected").siblings().removeClass("selected");
    });
});

function ShowChartQueryReport() {
    NTSDataRanking.showChartsList();
}

