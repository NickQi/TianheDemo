
var dataModel = null;
var XPdataModel = null;
var mainchart = '';
function mainquery() { }

NTSMainQuery = mainquery.prototype = {
    ShowDataInfo: function() {
        setDefault();
        ReportData.BaseData.type = '0';
        var msg = getCheckMsg(ReportData.BaseData, msgModel);
        if (msg != '') {
            alert(msg);
            return;
        }
        $("#showbox_title").html($("#objectname").val() + $("#Categorized").find("option:selected").text().replace('└', '').replace('├', '').trim());
        $("#querytime").html($("#starttime").val() + " - " + $("#endtime").val());
        $(".table_title").html($("#objectname").val() + $("#Categorized").find("option:selected").text().replace('└', '').replace('├', '').trim() + '<em style="color: #359bba">能耗查询</em>');
        $(".table_smalltt").html($("#starttime").val() + " - " + $("#endtime").val());


        /*分页数据,极值部分请求信息*/
        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.EneryQuery,
            data: ReportData.BaseData,
            pageSize: 20,
            callback: function(listId, result) {
                ((dataModel != null) || ((dataModel = ko.mapping.fromJS(result)) && ko.applyBindings(dataModel))) && ko.mapping.fromJS(result, dataModel);
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
        var itemcodeid = { id: $('#Categorized').val() };
        NTS.NTSAjax(AjaxUrl.DeptNameByItemCode, itemcodeid, function(data) {
            // alert(data.DeptName);
            $("#dept1,#dept2,#dept").html(data.DeptName);
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
        jQuery(".showbox_button").css({ 'left': '715px' });
        jQuery(".showbox_img img").css({ 'width': '718px' });
        jQuery(".showbox_img").css({ 'width': '918px' });
        jQuery(".showboxexport_title").css({ 'width': '957px' });
        jQuery(".tblbox").css({ 'width': '957px' });
        jQuery(".tblbox ul").css({ 'width': '957px' });
        jQuery(".tblbox ul li").css({ 'width': '957px' });
        jQuery("#up1").hide();
        jQuery("#up2").show();
        jQuery("#up3").hide();
        jQuery("#up4").show();
        mainchart.setSize(918, $("#mainchartdiv").height());
    });
    jQuery("#ibox1").click(function () {
        jQuery(".search_condition").show();
        jQuery(".indentbox").hide();
        jQuery(".search_show").css({ 'width': '718px' });
        jQuery(".top_showbox").css({ 'width': '718px', 'background': 'url(images/energy/top_side01.png) no-repeat 0px 0px' });
        jQuery(".showbox").css({ 'width': '716px' });
        jQuery(".bottom_showbox").css({ 'width': '718px', 'background': 'url(images/energy/bottom_side01.png) no-repeat 0px 0px' });
        jQuery(".showbox").css({ 'width': '716px' });
        jQuery(".showbox_chart").css({ 'width': '716px' });
        jQuery(".showbox_value").css({ 'width': '716px', 'padding-left': '0px' });
        jQuery(".showbox_export").css({ 'width': '716px' });
        jQuery(".showbox_chart_cn").css({ 'width': '692px' });
        jQuery(".showbox_title").css({ 'width': '702px' });
        jQuery(".showbox_smalltt").css({ 'width': '702px' });
        jQuery(".showbox_button").css({ 'left': '398px' });
        jQuery(".showbox_img").css({ 'width': '692px' });
        jQuery(".showboxexport_title").css({ 'width': '716px' });
        jQuery(".tblbox").css({ 'width': '716px' });
        jQuery(".tblbox ul").css({ 'width': '716px' });
        jQuery(".tblbox ul li").css({ 'width': '716px' });
        jQuery("#ibtn1").show();
        jQuery("#ibtn2").hide();
        jQuery("#up1").show();
        jQuery("#up2").hide();
        jQuery("#up3").show();
        jQuery("#up4").hide();
        mainchart.setSize(692, $("#mainchartdiv").height());
    });
});

$(function () {
    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#itemcode").val('1');
    $("#unit").val('3');

    $(".button_show_proportion").click(function () {
        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.EneryQueryPieChart,
            data: ReportData.BaseData,
            charts: {
                type: "pie",
                showLegend: true,
                legendPosition: 'right',
                itemUnit:true
            }
        });

    });
    $(".button_show_total").click(function () {
        mainchart = $("#mainchartdiv").AjaxHighCharts({
            url: AjaxUrl.EneryQueryChart,
            data: ReportData.BaseData,
            charts: {
                type: "column",
                chartUnit:true,
                showDataLabel: false,
                useTicker: true,
                xStep: 6,
                itemUnit: true
            }
        });

    });

    //导出Excel
    $(".exportbtn").click(function () {
        $(this).FileExport({
            url: AjaxUrl.ExportMainQuery,
            data: ReportData.BaseData
        });
    });
});

function ShowChartQueryReport() {
    NTSMainQuery.ShowDataInfo();
  
    showToolBarMenu();
}

function showToolBarMenu() {
    var obj = $(".checkbox[config='" + ReportData.BaseData.objectid + "']");
    var classname = obj.parent().parent().parent().attr("class");
    $(".button_show_proportion").show();
    if (ReportData.BaseData.unit == '1') {
        $(".button_show_today").hide();
    }
    if (classname == "nav_third") { $(".button_show_proportion").hide(); }
}