var mainchart = '';
var mainchart2 = '';
NTSMaxValue = function () { } .prototype = {
    ShowMaxReport: function () {
        setDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择查询的对象。');
            return;
        }
        $("#showbox_title").html($("#objectname").val() + $("#Categorized").find("option:selected").text().replace('└', '').replace('├', '').trim());
        $("#querytime").html($("#starttime").val() + " - " + $("#endtime").val());
        $(".table_title").html($("#objectname").val() + $("#Categorized").find("option:selected").text().replace('└', '').replace('├', '').trim() + '<em style="color: #359bba">能耗查询</em>');
        $(".table_smalltt").html($("#starttime").val() + " - " + $("#endtime").val());
        var maxunit = "maxunit";
        var maxname = "maxname";
        ReportData.BaseData[maxunit] = 0;
        ReportData.BaseData[maxname] = "定额线";
        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.MaxChart,
            data: ReportData.BaseData,
            charts: {
                type: "comline",
                chartUnit: true,
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                showLegend: true,
                itemUnit: true
            }
        });

        ReportData.BaseData[maxname] = "定额斜率线";
        mainchart2 = $("#mainchartdiv2").AjaxHighCharts({
            url: AjaxUrl.MaxChartGoing,
            data: ReportData.BaseData,
            charts: {
                type: "comline",
                chartUnit: true,
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                showLegend: true,
                itemUnit: true
            }
        });

        NTS.NTSAjax(AjaxUrl.GetRunResult, ReportData.BaseData, function (data) {
            if (data.success) {
                $("#nowenery,#Em1").html(data.nowenery + data.dept);
                $("#plant,#Em2").html(data.plantvalue + data.dept);
                $("#yue").html(data.yue + data.dept);
                $("#rtprecentvalue").html(data.rtprecentvalue * 100 + "%");
                $("#runstatus").html(data.subtotal);
                $("#Em3,#yuqi").html(data.yjye + data.dept);// 总计划值-当前计划值
                $("#Em4").html(data.subnowvalue + data.dept);
                $(".showpic_txt").show();
            } else {
                $("#nowenery,#plant,#runstatus,#Em1,#Em2,#Em3,#Em4").html('');
                $(".showpic_txt").hide();
            }
        });

    }
};


$(function () {
    // 显示自定义查询条件
    // new mainquery().showmainreportmap();
    $("#Categorized").change(function () {
        $("#itemcode").val($(this).val());
        ShowChartQueryReport();
    });
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday()); //GetMonthLast
    $("#itemcode").val('1');
    $("#unit").val('3');
});


function ShowChartQueryReport() {
    NTSMaxValue.ShowMaxReport();
}