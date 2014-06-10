/*********************************************
*
*Author:chenlei
*DateTime:2012-10-23
*Descritpion:常用功能Jquery自定义插件封装
*
**********************************************/
(function ($) {
    /***************分页导航**************************
    *描述：对分页进行封装，只需要传入相关参数即可生成分页效果,并Knockout.js使用
    *方法options参数说明（*号为必填项）：
    *           listId: 分页列表ID（*）；
    *           url: 获取分页数据的地址（*）；
    *           data: 查询条件参数，传入后台使用；
    *           pageSize: 设置每页页大小，默认为25；
    *           page: 当前页面值，不需要填写；
    *           timeOut: 设置Ajax请求超时时间；
    *           callback: 回调函数，页面数据列表处理完成之后调用Konckoutjs实现数据与HTML绑定
    ***********************************************/
    $.fn.AjaxPaging = function (options) {
        var element = $(this);
        var dialogId = "pagingdialog_" + element.attr("id");

        var dialog = $.dialog({
            id: dialogId,
            width: 300,
            height: 100,
            title: "数据加载中",
            content: "数据加载中......",
            fixed: true,
            esc: false,
            lock: true
        }).show();

        var defaults = {
            listId: "",
            url: "",
            data: null,
            pageSize: 25,
            page: 1,
            timeOut: 1000,
            callback: null
        };
        var opts = $.extend(defaults, options);
        opts.data = $.extend(opts.data, { page: opts.page, pageSize: opts.pageSize });

        $.ajax({
            //url: opts.url,
            url: "action.ashx?" + opts.url + "&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            async: false,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: opts.data,
            timeout: opts.timeOut,
            success: function (result) {
                if (result == "" || result == "]") {
                    result = '{"total":"0","result":[]}';
                }
                eval("results=" + result);

                PagingShow(element, opts, results);
                dialog.close();
            },
            error: function (msg) {
                dialog.close();
               // $.dialog.alert(msg);
            }
        });
        return this;
    };

    function PagingShow(element, opts, results) {
        var count = results.total;
        if (count > 0) {
            $("#" + opts.listId).next().hide();
            element.show();
        } else {
            $("#" + opts.listId).next().show();
            element.hide();
        }
        var maxPage = Math.ceil(count / opts.pageSize);
        $("body").append('<input type="hidden" id="cp">');
        $("#cp").val(maxPage);

        element.empty();
        element.html('<div class="pagination"></div>');
        var pagination = element.children();
        $(pagination).html('<a href="#" class="first" data-action="first">&laquo;</a>' +
            '<a href="#" class="previous" data-action="previous">&lsaquo;</a>' +
            '<input type="text" readonly="readonly" data-max-page="40" />' +
            '<a href="#" class="next" data-action="next">&rsaquo;</a>' +
            '<a href="#" class="last" data-action="last">&raquo;</a>');
        $(pagination).jqPagination({
            link_string: '/?page={page_number}',
            current_page: opts.page,
            max_page: maxPage,
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (page) {
                if (page > $("#cp").val()) {
                    return;
                }
                opts.page = page;
                $(element).AjaxPaging(opts);
            }
        });
        opts.callback == null || opts.callback(opts.listId, results);
    };

    /***************文件导出功能**************************
    *描述：对文件导出功能进行封装，增加友好的进度条提示功能
    *方法options参数说明：
    *           url: 获取列表内容（*）；
    *           data: 查询条件参数，传入后台使用；
    *           timeOut: 设置超时时间；
    *           callback: 回调函数，页面加载完成操作
    ***********************************************/
    $.fn.FileExport = function (options) {
        var element = $(this);
        var defaults = {
            url: "",
            data: null,
            timeOut: 3000,
            callback: function () { }
        };
        var opts = $.extend(defaults, options);

        var dialogId = "exportdialog_" + element.attr("id");
        var dialog = $.dialog({
            id: dialogId,
            width: 300,
            height: 100,
            title: "数据处理中",
            content: "数据导出处理中......",
            fixed: true,
            esc: false,
            lock: true
        }).show();

        $.ajax({
            // url: "action.ashx?" + opts.url + "&times=" + new Date().getTime(),
            url:opts.url,
            async: false,
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: opts.data,
            timeout: opts.timeOut,
            success: function (result) {
                if (result != ']' && result != '') {
                    eval("result=" + result);
                    dialog.close();
                    if (result.status == 'success') {
                        window.open(result.msg);
                    }
                    else {
                        $.dialog.alert(result.msg);
                    }
                    opts.callback(result);
                } else {

                }
            }
        });

        return this;
    };

    /***************Charts查看功能**************************
    *描述：查看数据图表
    *Charts必须是针对一个Div对象的。
    *方法options参数说明：
    *           url: 获取列表内容（*）；
    *           data: 查询条件参数，传入后台使用；
    *           show: 报表显示方式，默认为在页面中显示，若要在对话框中显示，需要设置show:dialog
    *           charts: 为报表的一组参数，具体见代码或实例
    *           timeOut: 设置超时时间；
    *           callback: 回调函数，页面加载完成操作
    ***********************************************/
    $.fn.AjaxHighCharts = function (options) {
        var element = $(this);
        var elementId = element.attr("id") + "_charts";
        element.html("<div id='" + elementId + "' style='height: " + (element.height() - 5) + "px;margin: 0 auto'></div>");

        var defaults = {
            url: "",
            data: null,
            charts: {
                type: 'line',
                title: '',
                subtitle: '',
                yTitle: '',
                xShow: true,
                yShow: true,
                showLegend: false,
                showDataLabel: true,
                legendPosition: 'bottom',
                legend: {},
                useTicker: false,
                xStep: 10,
                tickInterval: null,
                width: null,
                itemColor: null,
                itemPadding: 0.05,
                itemMargin: 0.05,
                itemUnit: false,
                chartUnit: false,
                pieSize: 150,
                pieLocation: [200, 100],
                piePoint: [180, 20],
                tipFormat: '<b>{0}</b>:{1} {2}',
                callback: null
            },
            show: 'default',
            loading: true,
            timeOut: 1000,
            callback: function () { }
        };

        var opts = $.extend(true, defaults, options);

        if (opts.loading) {
            var dialogId = "chartsdialog_" + elementId;
            var dialog = $.dialog({
                id: dialogId,
                width: 300,
                height: 100,
                title: "图表加载中",
                content: "图表加载中......",
                fixed: true,
                esc: false,
                lock: true
            }).show();
        }

        switch (opts.charts.legendPosition) {
            case 'bottom':
                opts.charts.legend = { align: 'center', verticalAlign: 'bottom' };
                break;
            case 'right':
                opts.charts.legend = { layout: 'vertical', align: 'right', verticalAlign: 'middle' };
                break;
            case 'left':
                opts.charts.legend = { layout: 'vertical', align: 'left', verticalAlign: 'middle' };
                break;
            case 'top':
                opts.charts.legend = { align: 'center', verticalAlign: 'top' };
                break;
            default:
                opts.charts.legend = { align: 'center', verticalAlign: 'bottom' };
                break;
        }

        Highcharts.setOptions({
            colors: ['#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92',
                    '#9ACD32', '#00FFFF', '#7FFFD4', '#FFE4C4', '#8A2BE2', '#A52A2A', '#5F9EA0', '#7FFF00', '#D2691E',
                    '#FF7F50', '#6495ED', '#DC143C', '#00008B', '#008B8B', '#006400', '#8B008B', '#FF8C00', '#9932CC',
                    '#00CED1', '#483D8B', '#FF1493', '#B22222', '#FF00FF', '#DAA520', '#4B0082', '#7CFC00', '#F08080',
                    '#90EE90', '#20B2AA', '#00FF00', '#32CD32', '#FF00FF', '#800000', '#0000CD', '#EE82EE', '#40E0D0']
        });

        var chart;
        $.ajax({
            //  url: "action.ashx?" + opts.url + "&times=" + new Date().getTime(),
            url: opts.url,
            async: false,
            type: 'POST',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: opts.data,
            timeout: opts.timeOut,
            success: function (result) {
                if (result != ']' && result != '') {
                    eval("result=" + result);
                    if (opts.loading) {
                        dialog.close();
                    }
                    if (result.status == 'success') {
                        var step = opts.charts.xStep;
                        if (opts.charts.useTicker) {
                            if ((result.Charts.xCategory.length / step) >= 2) {
                                opts.charts.tickInterval = parseInt(result.Charts.xCategory.length / step);
                            } else if ((result.Charts.xCategory.length / step) > 1 && (result.Charts.xCategory.length / step) < 2) {
                                opts.charts.tickInterval = 2;
                            } else {
                                opts.charts.tickInterval = 1;
                            }
                        }
                        // alert(opts.charts.tickInterval);
                        switch (opts.charts.type) {
                            case 'line':
                                chart = lineCharts(elementId, result, opts);
                                break;
                            case 'comline':
                                chart = lineCombineCharts(elementId, result, opts);
                                break;
                            case 'columnpie':
                                chart = columnPieCharts(elementId, result, opts);
                                break;
                            case 'column':
                                chart = columnCharts(elementId, result, opts);
                                break;
                            case 'bar':
                                chart = barCharts(elementId, result, opts);
                                break;
                            case 'pie':
                                chart = pieCharts(elementId, result, opts);
                                break;
                            case 'muticolumn':
                                chart = columnCompareCharts(elementId, result, opts);
                                break;
                            case 'stackcolumn':
                                chart = columnStackCharts(elementId, result, opts);
                                break;
                            default:
                                chart = lineCharts(elementId, result, opts);
                                break;
                        }


                        if (opts.show == "dialog") {
                            element.show();
                            $.dialog({
                                title: opts.charts.title,
                                content: document.getElementById(elementId),
                                lock: true,
                                padding: '5px 5px',
                                close: function () {
                                    element.hide();
                                }
                            });
                        }
                    }
                    else {
                       // $.dialog.alert(result.msg);
                    }
                    opts.callback(result);
                } else {
                    dialog.close();
                }
            }
        });
        return chart;
    };


  

    function lineCharts(elementId, result, opts) {
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId,
                type: 'line'
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                }
            },
            tooltip: {
                formatter: function () {
                    if (opts.charts.itemUnit) {
                        var unit;
                        if (result.Charts.ItemUnit.length == result.Charts.xCategory.length) {
                            unit = result.Charts.ItemUnit[$.inArray(this.x, result.Charts.xCategory)];
                        } else {
                            unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                        }
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), unit);
                    } else {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '');
                    }
                }
            },
          
            series: result.Charts.DataValue
        });
        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };

    function lineCombineCharts(elementId, result, opts) {
        var seriesData = [];
        for (var i = 0; i < result.Charts.DataValue.length; i++) {
            if (i == 0) {
                seriesData[i] = {
                    type: 'line',
                    name: result.Charts.DataValue[i].name,
                    data: result.Charts.DataValue[i].data,
                    marker: {
                        enabled: false
                    },
                    states: {
                        hover: {
                            lineWidth: 0
                        }
                    },
                    dashStyle: 'shortdot',
                    enableMouseTracking: false
                };
            } else {
                seriesData[i] = {
                    name: result.Charts.DataValue[i].name,
                    data: result.Charts.DataValue[i].data
                };
            }
        }
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                }
            },
            tooltip: {
                formatter: function () {
                    if (opts.charts.itemUnit) {
                        var unit;
                        if (result.Charts.ItemUnit.length == result.Charts.xCategory.length) {
                            unit = result.Charts.ItemUnit[$.inArray(this.x, result.Charts.xCategory)];
                        } else {
                            unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                        }
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), unit);
                    } else {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '');
                    }
                }
            },
            series: seriesData
        });

        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    }

    function pieCharts(elementId, result, opts) {
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            tooltip: {
                formatter: function () {
                    var index = -1;
                    for (var i = 0; i < result.Charts.DataValue.length; i++) {
                        if (result.Charts.DataValue[i].name == this.point.name) {
                            index = i;
                        }
                    }
                    if (opts.charts.itemUnit) {
                        opts.charts.tipFormat = '<b>{0}</b>:{1} % <br/> 能耗值：{2} {3}';
                        return $.format(opts.charts.tipFormat, this.point.name, Highcharts.numberFormat(this.percentage, 2), this.y, result.Charts.ItemUnit[index]);
                    } else {
                        opts.charts.tipFormat = '<b>{0}</b>:{1} % ';
                        return $.format(opts.charts.tipFormat, this.point.name, Highcharts.numberFormat(this.percentage, 2), '');
                    }
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: opts.charts.showLegend
                }
            },
            legend: opts.charts.legend,
            series: [{
                type: 'pie',
                data: result.Charts.DataValue
            }]
        });
        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };

    function columnCharts(elementId, result, opts) {
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId,
                type: 'column'
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                }
            },
            tooltip: {
                formatter: function () {
                    if (opts.charts.itemUnit) {
                        var unit;
                        if (result.Charts.ItemUnit.length == result.Charts.xCategory.length) {
                            unit = result.Charts.ItemUnit[$.inArray(this.x, result.Charts.xCategory)];
                        } else {
                            unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                        }
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), unit);
                    } else {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '');
                    }
                }
            },
            plotOptions: {
                series: {
                    dataLabels: {
                        enabled: opts.charts.showDataLabel
                    },
                    color: opts.charts.itemColor,
                    pointPadding: opts.charts.itemPadding,
                    groupPadding: opts.charts.itemMargin,
                    pointWidth: result.Charts.xCategory.length > 9 ? null : 32
                }
            },
            series: result.Charts.DataValue
        });
        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };

    function columnPieCharts(elementId, result, opts) {
        var seriesData = [];
        var pieName = "";
        for (var i = 0; i < result.Charts.DataValue.length; i++) {
            if (i == 0) {
                pieName = result.Charts.DataValue[i].name;
                seriesData[i] = {
                    type: 'pie',
                    name: result.Charts.DataValue[i].name,
                    data: result.Charts.DataValue[i].data,
                    center: opts.charts.pieLocation,
                    size: opts.charts.pieSize,
                    showInLegend: opts.charts.showLegend,
                    dataLabels: {
                        enabled: false
                    }
                };
            } else {
                seriesData[i] = {
                    type: 'column',
                    name: result.Charts.DataValue[i].name,
                    data: result.Charts.DataValue[i].data
                };
            }
        }
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                }
            },
            labels: {
                items: [{
                    html: pieName,
                    style: {
                        left: opts.charts.piePoint[0],
                        top: opts.charts.piePoint[1]
                    }
                }]
            },
            tooltip: {
                formatter: function () {
                    if (this.point.name) {
                        //饼图提示
                        opts.charts.tipFormat = '<b>{0}</b>:{1} %<br/> 能耗值：{2} {3}';
                        var _unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                        return $.format(opts.charts.tipFormat, this.point.name, Highcharts.numberFormat(this.percentage, 2), this.y, _unit);
                    } else {
                        opts.charts.tipFormat = '<b>{0}</b>:{1} {2}';
                        if (opts.charts.itemUnit) {
                            var unit;
                            if (result.Charts.ItemUnit.length == result.Charts.xCategory.length) {
                                unit = result.Charts.ItemUnit[$.inArray(this.x, result.Charts.xCategory)];
                            } else {
                                unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                            }
                            return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), unit);
                        } else {
                            return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '');
                        }
                    }
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer'
                },
                series: {
                    dataLabels: {
                        enabled: opts.charts.showDataLabel
                    },
                    color: opts.charts.itemColor,
                    pointPadding: opts.charts.itemPadding,
                    groupPadding: opts.charts.itemMargin,
                    pointWidth: result.Charts.xCategory.length > 9 ? null : 36
                }
            },
            series: seriesData
        });
        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };

    function barCharts(elementId, result, opts) {
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId,
                type: 'bar'
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                }
            },
            tooltip: {
                formatter: function () {
                    if (opts.charts.itemUnit) {
                        var unit;
                        if (result.Charts.ItemUnit.length == 1) {
                            unit = result.Charts.ItemUnit[0];
                        }
                        else if (result.Charts.ItemUnit.length == result.Charts.xCategory.length) {
                            unit = result.Charts.ItemUnit[$.inArray(this.x, result.Charts.xCategory)];
                        } else {
                            unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                        }
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), unit);
                    } else {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '');
                    }
                }
            },
            plotOptions: {
                series: {
                    dataLabels: {
                        enabled: opts.charts.showDataLabel
                    },
                    color: opts.charts.itemColor,
                    pointPadding: opts.charts.itemPadding,
                    groupPadding: opts.charts.itemMargin,
                    pointWidth: result.Charts.xCategory.length > 9 ? null : 32
                }
            },
            series: result.Charts.DataValue
        });
        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };

    function columnCompareCharts(elementId, result, opts) {
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId,
                type: 'column',
                width: opts.charts.useTicker ? null : result.Charts.xCategory.length * result.Charts.DataValue.length * 25
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval,
                tickLength: 10
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                }
            },
            tooltip: {
                formatter: function () {
                    if (opts.charts.itemUnit) {
                        var unit;
                        if (result.Charts.ItemUnit.length == result.Charts.xCategory.length) {
                            unit = result.Charts.ItemUnit[$.inArray(this.x, result.Charts.xCategory)];
                        } else {
                            unit = result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')];
                        }
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), unit);
                    } else {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '');
                    }
                }
            },
            plotOptions: {
                series: {
                    dataLabels: {
                        enabled: opts.charts.showDataLabel
                    },
                    pointPadding: opts.charts.itemPadding,
                    groupPadding: opts.charts.itemMargin,
                    pointWidth: result.Charts.xCategory.length * result.Charts.DataValue.length > 18 ? null : 32
                }
            },
            series: result.Charts.DataValue
        });

        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };

    function columnStackCharts(elementId, result, opts) {
        var chart = new Highcharts.Chart({
            chart: {
                renderTo: elementId,
                type: 'column'
            },
            title: {
                text: opts.charts.title
            },
            subtitle: {
                text: opts.charts.subtitle,
                style: { color: 'red' },
                align: 'right',
                y: 15
            },
            credits: {
                enabled: false
            },
            legend: {
                enabled: opts.charts.showLegend
            },
            exporting: {
                enabled: false
            },
            xAxis: {
                labels: {
                    enabled: opts.charts.xShow
                },
                categories: result.Charts.xCategory,
                tickPosition: 'inside',
                tickInterval: opts.charts.tickInterval
            },
            yAxis: {
                title: {
                    text: opts.charts.yTitle
                },
                labels: {
                    enabled: opts.charts.yShow
                },
                stackLabels: {
                    enabled: opts.charts.showDataLabel,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            tooltip: {
                formatter: function () {
                    if (opts.charts.itemUnit) {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), result.Charts.ItemUnit[$.inJson(this.series.name, result.Charts.DataValue, 'name')], this.series.name);
                    } else {
                        return $.format(opts.charts.tipFormat, this.x, Highcharts.numberFormat(this.y, 2), '', this.series.name);
                    }
                }
            },
            plotOptions: {
                series: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: false
                    },
                    pointPadding: opts.charts.itemPadding,
                    groupPadding: opts.charts.itemMargin,
                    pointWidth: result.Charts.xCategory.length > 9 ? null : 32
                }
            },
            series: result.Charts.DataValue
        });

        if (opts.charts.chartUnit && result.Charts.ItemUnit.length > 0) {
            chart.setTitle(null, { text: '单位:(' + result.Charts.ItemUnit[0] + ')' });
        }
        return chart;
    };
})(jQuery);


/*********************************************
*
*Author:chenlei
*DateTime:2012-10-23
*Descritpion:扩展Jquery内置函数
*
**********************************************/
(function ($) {
    $.extend({
        format: function (source, params) {
            if (arguments.length == 1)
                return function () {
                    var args = $.makeArray(arguments);
                    args.unshift(source);
                    return $.format.apply(this, args);
                };
            if (arguments.length > 2 && params.constructor != Array) {
                params = $.makeArray(arguments).slice(1);
            }
            if (params.constructor != Array) {
                params = [params];
            }
            $.each(params, function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
            });
            return source;
        },
        inJson: function (source, target, key) {
            var index = -1;
            if (target.length < 0) {
                index = -1;
            }
            for (var i = 0; i < target.length; i++) {
                if (target[i][key] == source) {
                    index = i;
                }
            }
            return index;
        },
        compareTime: function (startTime, endTime) {
            if (Date.parse(startTime.replace(/-/g, '/ ')) > Date.parse(endTime.replace(/-/g, '/ '))) {
                alert("请起止时间小于终止时间");
                return false;
            }

            return true;
        }
    });
})(jQuery);