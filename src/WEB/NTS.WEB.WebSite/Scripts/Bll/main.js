/*
-----------------------------------------------------------------------------------
主页的javascript类集合
-----------------------------------------------------------------------------------
*/
var dataModel = null;
var eneryModel = null;
var colMap = '';

function tsfn() { };
NTSIndex = tsfn.prototype = {
    ShowBaseReport: function () {
        setDefault();

        colMap = $("#columnimg").AjaxHighCharts({
            url: AjaxUrl.IndexReportChart,
            data: ReportData.BaseData,
            charts: {
                type: 'columnpie',
                itemUnit: true,
                pieLocation: [400, 120],
                piePoint: [365, 15]
            }
        });
        
        NTS.NTSAjax(AjaxUrl.IndexReport, ReportData.BaseData, function (json) {
            if (json.success) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(json)) && ko.applyBindings(dataModel, document.getElementById("listEneryValue")))) && ko.mapping.fromJS(json, dataModel);

                NTS.NTSAjax(AjaxUrl.IndexEneryChain, ReportData.BaseData, function (result) {
                    if (result.success) {
                        ((eneryModel != null) || ((eneryModel = ko.mapping.fromJS(result)) && ko.applyBindings(eneryModel, document.getElementById("eneryChainValue")))) && ko.mapping.fromJS(result, eneryModel);
                    }
                });
            }
        });
    }
};


$(document).ready(function () {
    $("#starttime").val("2012-10-08");
    $("#endtime").val(GetToday());
    $("#itemcode").val('1');
    $("#unit").val('5');
    $("#objectid").val('0');


    $('.top_value_sort span').click(function () {
        alert("ddd");
        $(this).addClass("top_select").siblings().removeClass("top_select");
    });


    $('.date_selectbox span').click(function () {
        $(this).addClass("selected").siblings().removeClass("selected");
        $(this).addClass("select").siblings().removeClass("select");
        var ss = $(this).html();
        $("em.date_now").html(ss);
        $("em.date_prev").html(ss.replace('本', '上').replace('今', '昨'));
        var pp = $(".date_selectbox span.selected").html();
        switch (pp) {
            case "今日":
                $("#starttime").val(GetToday());
                $("#endtime").val(GetNow());
                $("#unit").val('1');
                break;
            case "本周":
                $("#starttime").val(GetWeekFirstDay());
                $("#endtime").val(GetToday());
                $("#unit").val('2');
                break;
            case "本月":
                $("#starttime").val(GetCuttMonth());
                $("#endtime").val(GetToday());
                $("#unit").val('3');
                break;
            default:
                $("#starttime").val("2012-10-08");
                $("#endtime").val(GetToday());
                $("#unit").val('5');
                break;
        }
        $("#currentTime").text($("#starttime").val() + " 至 " + $("#endtime").val());
        NTSIndex.ShowBaseReport();
    });

    $('.date_selectbox span.selected').trigger("click");
});

//single tree comm interface
function ShowChartQueryReport() {
    NTSIndex.ShowBaseReport();
}