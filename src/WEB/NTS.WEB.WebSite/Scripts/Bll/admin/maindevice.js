
var dataModel = null;
var XPdataModel = null;
var mainchart = '';
function maindevice() { }

NTSMainDevice = maindevice.prototype = {
    ShowDataInfo: function () {

        setDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择查询的对象。');
            return;
        }
        ReportData.BaseData.objecttype = 1;
        $("#showbox_title").html($("#objectname").val() + $(".select").find("option:selected").text().replace('└', '').replace('├', '').trim());
        $("#querytime").html($("#starttime").val() + " - " + $("#endtime").val());
        $(".equ_title_big").html($("#objectname").val() + $(".select").find("option:selected").text().replace('└', '').replace('├', '').trim() + '<em style="color: #359bba">设备能耗查询</em>');
        $(".equ_title_small").html($("#starttime").val() + " - " + $("#endtime").val());

        /*分页数据,极值部分请求信息*/
        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.EneryQuery,
            data: ReportData.BaseData,
            pageSize: 20,
            callback: function (listId, result) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(result)) && ko.applyBindings(dataModel))) && ko.mapping.fromJS(result, dataModel);
                $(".dname").html($("#objectname").val());
            }
        });


        // 显示图表

        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.EneryQueryChart,
            data: ReportData.BaseData,
            charts: {
                type: "column",
                chartUnit: true,
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                itemUnit: true
            }
        });


        /*切换单位*/
        var itemcodeid = { id: $(".select").val() };
        NTS.NTSAjax(AjaxUrl.DeptNameByItemCode, itemcodeid, function (data) {
            // alert(data.DeptName);
            $("#dept").html(data.DeptName);
        });
    }
};



function ShowChartQueryReport() {
    NTSMainDevice.ShowDataInfo();
}

$(function () {

    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    //$("#itemcode").val('1');
    $("#unit").val('3');

    // 注册导出按钮事件
    $(".exportbtn").click(function () {
        $(this).FileExport({
            url: AjaxUrl.ExportMainDevice,
            data: ReportData.BaseData
        });
    });


});

