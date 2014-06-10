var dataModel = null;
var deviceModel = null;
var mainchart = '';
function RealTimeData() { };
NTSRealTimeData = RealTimeData.prototype = {
    ShowDataInfo: function (object, type) {
        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.RealTimeData,
            data: { type: type, objectID: object, debug: "debug" },
            pageSize: 13,
            callback: function (listId, result) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(result)) && ko.applyBindings(dataModel, document.getElementById(listId)))) && ko.mapping.fromJS(result, dataModel);
            }
        });
        setDefault();
        ReportData.BaseData.objecttype = 1;

        // 显示图表
        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.EneryQueryChart,
            data: ReportData.BaseData,
            charts: {
                type: "line",
                title: '设备今日能耗走势图',
                chartUnit: true,
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                itemUnit: true
            }
        });


        $.ajax({
            url: "action.ashx?" + AjaxUrl.DeviceRealData + "&times=" + new Date().getTime(),
            async: false,
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: ReportData.BaseData,
            timeout: 3000,
            success: function (result) {
                if (result != ']' && result != '') {
                    $("#device_infomation tfoot").css("display", "");
                    eval("results=" + result);
                    ((deviceModel != null) || ((deviceModel = ko.mapping.fromJS(results)) && ko.applyBindings(deviceModel, document.getElementById("device_infomation")))) && ko.mapping.fromJS(results, deviceModel);
                } else {

                }
            }
        });
    }
};

function ShowChartQueryReport() {
    $("#starttime").val(GetToday());
    $("#endtime").val(GetNow());
    $("#unit").val('1');
    //        $("#starttime").val("2012-11-15");
    //        $("#endtime").val(GetToday());
    //        $("#unit").val('3');

    var type = 3; //表示是设备
    var object = $("li.select_d div span.checkbox").attr('config');
    $("#device_name").text($("li.select_d div span.checkbox").attr('nconfig'));
    NTSRealTimeData.ShowDataInfo(object, type);
}

$(document).ready(function () {

    /*缩进*/
    jQuery("#ibtn3").click(function () {
        jQuery(".search_condition").hide();
        jQuery(".indentbox").show();
        jQuery("#show5").css({ 'width': '959px' });
        jQuery(".topside").css({ 'width': '959px', 'background': 'url(images/energy/top_side02.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '957px' });
        jQuery(".btmside").css({ 'width': '959px', 'background': 'url(images/energy/bottom_side02.png) no-repeat 0px 0px' });
        jQuery(".table_data").css({ 'width': '698px' });
        jQuery(".page").css({ 'width': '931px' });
        jQuery(".page_left").css({ 'padding-left': '359px' });
        jQuery(".table_bigtt").css({ 'width': '931px' });
        mainchart.setSize(935, $("#mainchartdiv").height());
    });
    jQuery("#ibox3").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery("#show5").css({ 'width': '718px' });
        jQuery(".topside").css({ 'width': '718px', 'background': 'url(images/energy/top_side01.png) no-repeat 0px 0px' });
        jQuery(".mid").css({ 'width': '717px' });
        jQuery(".btmside").css({ 'width': '718px', 'background': 'url(images/energy/bottom_side01.png) no-repeat 0px 0px' });
        jQuery(".table_data").css({ 'width': '458px' });
        jQuery(".table_data_tt").css({ 'width': '681px' });
        jQuery(".page").css({ 'width': '690px' });
        jQuery(".page_left").css({ 'padding-left': '249px' });
        jQuery(".table_bigtt").css({ 'width': '690px' });
        mainchart.setSize(694, $("#mainchartdiv").height());
    });
});