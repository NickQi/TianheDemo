var dataModel = null;
var mainchart = '';
var chartpie = '';
function mainarea() { }

NTSMainArea = mainarea.prototype = {
    name: '区域能耗',
    ShowDataInfo: function () {
        setDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择查询的对象。');
            return;
        }
        $(".area_bigtitle").html($("#objectname").val() + '<em style="color: #359bba">能耗分布分析</em>');
        $(".area_smalltitle").html($("#starttime").val() + " - " + $("#endtime").val());

        // 显示图表

        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.EneryQueryAreaChart,
            data: ReportData.BaseData,
            charts: {
                type: 'stackcolumn',
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                showLegend: true,
                itemUnit: true,
                tipFormat: '<b>{0}</b>:{1} {2}  {3} '
            }
        });

        chartpie = $("#piechart").AjaxHighCharts({
            url: AjaxUrl.EneryQueryAreaPieChart,
            data: ReportData.BaseData,
            charts: {
                type: 'pie',
                yTitle: '耗电量',
                showLegend: true,
                legendPosition: 'bottom',
                tipFormat: '<b>{0}</b>:{1}% {2} '
            }
        });

        /*分页数据,极值部分请求信息*/
        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.EneryQueryArea,
            data: ReportData.BaseData,
            pageSize: 10,
            callback: function (listId, result) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(result)) && ko.applyBindings(dataModel))) && ko.mapping.fromJS(result, dataModel);
            }
        });

        /*切换单位*/
        var itemcodeid = { id: '1' };
        NTS.NTSAjax(AjaxUrl.DeptNameByItemCode, itemcodeid, function (data) {
            // alert(data.DeptName);
            $("#dept1").html(data.DeptName);
        });
    }
};

$(function () {
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#unit").val('3');
    $("#itemcode").val('1');

    //导出Excel
    $(".exportbtn").click(function () {
        $(this).FileExport({
            url: AjaxUrl.ExportMainArea,
            data: ReportData.BaseData
        });
    });
});


function ShowChartQueryReport() {
    NTSMainArea.ShowDataInfo();
}
