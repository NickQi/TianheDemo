
var dataModel = null;
var XPdataModel = null;
var mainchart = '';

NTSELEPrice = function() {
}.prototype = {
    ShowDataInfo: function() {
        setDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择查询的对象。');
            return;
        }
        $("#showbox_title").html($("#objectname").val());
        $("#querytime").html($("#starttime").val() + " - " + $("#endtime").val());
        $(".table_title").html($("#objectname").val() + '<em style="color: #359bba">电费查询</em>');
        $(".table_smalltt").html($("#starttime").val() + " - " + $("#endtime").val());

        /*分页数据,极值部分请求信息*/
        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.ElePriceQuery,
            data: ReportData.BaseData,
            pageSize: 20,
            callback: function(listId, result) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(result)) && ko.applyBindings(dataModel))) && ko.mapping.fromJS(result, dataModel);
            }
        });
        // 显示图表

        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.ElePriceChartQuery,
            data: ReportData.BaseData,
            charts: {
                type: "stackcolumn",
                showDataLabel: false,
                yTitle: '电费消耗图',
                useTicker: true,
                xStep: 6,
                showLegend: true,
                itemUnit: true,
                tipFormat: '<b>{0}</b>:{1} {2}   '
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

    /*缩进*/
    jQuery("#ibtn1").click(function () {
        jQuery(".search_condition").hide();
        jQuery(".indentbox").show();
        jQuery(".search_show").css({ 'width': '959px' });
        jQuery(".top_showbox").css({ 'width': '959px', 'background': 'url(images/energy/top_side02.png) no-repeat 0px 0px' });
        jQuery(".showbox").css({ 'width': '957px' });
        jQuery(".bottom_showbox").css({ 'width': '959px', 'background': 'url(images/energy/bottom_side02.png) no-repeat 0px 0px' });
        jQuery(".showbox_chart").css({ 'width': '957px' });
        jQuery(".showbox_value").css({ 'width': '882px', 'padding-left': '75px' });
        jQuery(".showbox_export").css({ 'width': '957px' });
        jQuery(".showbox_chart_cn").css({ 'width': '918px' });
        jQuery(".showbox_title").css({ 'width': '943px' });
        jQuery(".showbox_smalltt").css({ 'width': '943px' });
        jQuery(".showbox_img").css({ 'width': '918px' });
        jQuery(".linepic").css({ 'left': '52px' });
        jQuery(".showboxexport_title").css({ 'width': '957px' });
        jQuery(".tblbox").css({ 'width': '957px' });
        jQuery(".tblbox ul").css({ 'width': '957px' });
        jQuery(".tblbox ul li").css({ 'width': '957px' });
        jQuery(".table_jifei").css({ 'width': '957px' });
        jQuery(".table_jifei2").css({ 'width': '957px' });
        jQuery("#up1").hide();
        jQuery("#up2").show();
        mainchart.setSize(918, $("#mainchartdiv").height());
    });
    jQuery("#ibox1").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery(".search_show").css({ 'width': '718px' });
        jQuery(".top_showbox").css({ 'width': '718px', 'background': 'url(images/energy/top_side01.png) no-repeat 0px 0px' });
        jQuery(".showbox").css({ 'width': '717px' });
        jQuery(".bottom_showbox").css({ 'width': '718px', 'background': 'url(images/energy/bottom_side01.png) no-repeat 0px 0px' });
        jQuery(".showbox_chart").css({ 'width': '716px' });
        jQuery(".showbox_value").css({ 'width': '716px', 'padding-left': '0px' });
        jQuery(".showbox_export").css({ 'width': '716px' });
        jQuery(".showbox_chart_cn").css({ 'width': '692px' });
        jQuery(".showbox_title").css({ 'width': '702px' });
        jQuery(".showbox_smalltt").css({ 'width': '702px' });
        jQuery(".showbox_img").css({ 'width': '692px' });
        jQuery(".linepic").css({ 'left': '37px' });
        jQuery(".showboxexport_title").css({ 'width': '716px' });
        jQuery(".tblbox").css({ 'width': '716px' });
        jQuery(".tblbox ul").css({ 'width': '716px' });
        jQuery(".tblbox ul li").css({ 'width': '716px' });
        jQuery(".table_jifei").css({ 'width': '716px' });
        jQuery(".table_jifei2").css({ 'width': '716px' });
        jQuery("#ibtn1").show();
        jQuery("#ibtn2").hide();
        jQuery("#up1").show();
        jQuery("#up2").hide();
        mainchart.setSize(692, $("#mainchartdiv").height());
    });

    $(".exportbtn").click(function () {
        $(this).FileExport({
            url: AjaxUrl.ExportElePriceQuery,
            data: ReportData.BaseData
        });
    });

});

$(function () {
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#itemcode").val('1');
    $("#unit").val('3');
});

function ShowChartQueryReport() {
    NTSELEPrice.ShowDataInfo();
}

