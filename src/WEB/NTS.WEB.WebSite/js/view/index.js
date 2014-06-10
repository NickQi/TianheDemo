/**
* ==========================================================
* Copyright (c) 2014, tiansu-china.com All rights reserved.
* 能耗排名/详情JS
* Author: Jinsam
* Date:2014-3-24 08:56:08
* ==========================================================
*/

define(['chartsmod/charts', 'chartsmod/appQushi', 'chartsmod/appZonglan'], function (charts, appQushi, appZonglan) {

    function Index() {
        this.init();
    }

    Index.prototype = {

        /**
        * Initialize page
        * author: ghj
        * time: 2013-12-29 20:28:47
        */
        init: function () {
            this.render();
        },

        /**
        * Render page
        * author: ghj
        * time: 2013-12-29 20:28:47
        */
        render: function () {
            this.getInfo();
            this.ajaxRenderChart();
            this.ajaxAlarm();
        },

        /**
        * Bind Events
        * author: ghj
        * time: 2013-12-15 15:26:10
        */
        bindEvent: {
            setInfo: function (json) {
                if (json.PeriodValues.length > 0) {
                    $.each(json.PeriodValues, function (i, item) {
                        var $dom = $("#" + item.PeriodType);
                        var $span = $("span", $dom);
                        var num = parseFloat(item.MonthCompare);
                        if (num > 0) {
                            $(".contrast-detail i", $dom).removeClass("down").addClass("up");
                        } else {
                            $(".contrast-detail i", $dom).removeClass("up").addClass("down");
                        }
                        if (isNaN(num)) {
                            $span.eq(0).html("— —");
                        }
                        else {
                            $span.eq(0).html(Math.abs(num) + "%");
                        }
                        $span.eq(1).html(item.Value1);
                        $span.eq(2).html(item.Value2);

                        if (item.PeriodType == 1) {
                            $dom.find("p").children("i").attr("data-original-title", "上月日均用电量：" + item.Value1 + "kwh");
                        } else if (item.PeriodType == 2) {
                            $dom.find("p").children("i").attr("data-original-title", "上月周均用电量：" + item.Value1 + "kwh");
                        } else if (item.PeriodType == 3) {
                            $dom.find("p").children("i").attr("data-original-title", "去年同月用电量：" + item.Value1 + "kwh");
                        } else {
                            $dom.find("p").children("i").attr("data-original-title", "去年同期用电：" + item.Value1 + "kwh");
                        }
                    });
                    $(".summery i.up").tooltip();
                    $(".summery i.down").tooltip();
                }
            }

        },

        /**
        * 获取页面数据
        * author: pl
        * time: 2014-3-30 14:51:42
        */
        getInfo: function () {
            var that = this;
            $.ajax({
                url: this.getAction().index,
                dataType: "json",
                type: "post",
                async: false,
                beforeSend: function () {
                    //$tiansu.common.loading('.zoushi','show');
                    $tiansu.common.loading('.overview', 'show');
                    $tiansu.common.loading('.overview-chart', 'show');
                    $tiansu.common.loading('.summery', 'show');
                },
                success: function (json) {
                    if (json.ActionInfo.Success) {
                        that.bindEvent.setInfo(json);
                        that.setOverviewList(json);
                        that.pieChartInfo(json);

                        //定义饼图所须的数据格式
                        var pieData = {
                            series: [{
                                data: []
                            }]
                        };
                        //pieDataArr处理前的数据
                        var pieDataArr = json.ItemValues;
                        if (json.IsOnlyElec) {
                            pieDataArr.shift();
                        }
                        var pieDataArrLength = pieDataArr.length;
                        for (var i = 0; i < pieDataArrLength; i++) {
                            var dataItem = {
                                name: pieDataArr[i].CName,
                                y: pieDataArr[i].EnergyValue2Coal,
                                x: pieDataArr[i].EneryValue + pieDataArr[i].Unit
                            }
                            pieData.series[0].data.push(dataItem);
                        }
                        that.renderPie(pieData);
                    } else {
                        $tiansu.common.info('show', {
                            content: json.ActionInfo.ExceptionMsg,
                            timeout: 2000
                        });
                    }
                },
                error: function () {
                    $tiansu.common.info('show', {
                        content: "数据请求失败，请联系管理员！",
                        timeout: 2000
                    });
                },
                complete: function () {
                    $tiansu.common.loading('#main', 'hide');
                }
            });
        },


        /**
        * 当日用电趋势
        * author: hf
        * time: 2014-3-30 21:20:20
        */
        ajaxRenderChart: function () {
            var that = this;
            $.ajax({
                url: this.getAction().indexChart,
                dataType: "json",
                type: "post",
                async: false,
                beforeSend: function () {
                    $tiansu.common.loading('#indexChart', 'show');
                },
                success: function (json) {
                    if (json.ActionInfo.Success) {
                        var tempData = json.series[0].data;
                        var dataLength = tempData.length;
                        for (var i = 0; i < 24 - dataLength; i++)
                            tempData.push(null);
                        that.renderChart(json);
                    } else {
                        $tiansu.common.info('show', {
                            content: '数据错误',
                            timeout: 2000
                        });
                    }
                },
                error: function () {
                    $tiansu.common.info('show', {
                        content: "数据请求失败，请联系管理员！",
                        timeout: 2000
                    });
                },
                complete: function () {
                    $tiansu.common.loading('#main', 'hide');
                }
            });
        },

        renderChart: function (json) {
            charts.resetTimezone();
            var strSysDate = $("#sys-time").text();
            var sysDate = new Date(strSysDate.replace(/-/g, "/"));
            var chart1 = appQushi.renderTo("indexChart", json, sysDate);
        },


        /**
        * 本月能耗总览
        * author: hf
        * time: 2014-3-30 22:02:48
        */
        setOverviewList: function (json) {
            var liHtml = "";
            var totalCname = "总能耗";
            var totalValue = json.Total;
            var totalUnit = "T";
            var totalCompare = json.TotalCompare;
            var totalCompareVal = parseFloat(totalCompare);
            var totalLastMonVal = json.TotalLastMon;
            var icon = ""
            if (totalCompareVal < 0) {
                icon = "down";
            } else {
                icon = "up";
            }

            //开始拼接HTML字符串
            var liTotalHtml = '';
            liTotalHtml += '<li>';
            liTotalHtml += '	<div class="total">';
            liTotalHtml += '  	<span class="cname">总能耗</span>';
            liTotalHtml += '     <span class="value">' + totalValue + '</span>';
            liTotalHtml += '     <span class="unit">T</span>';
            liTotalHtml += ' </div>';
            liTotalHtml += ' <div class="percent">';
            liTotalHtml += ' 	<p>环比上个月</p>';
            liTotalHtml += ' 	<i class="' + icon + '" data-original-title="上月能耗值：' + totalLastMonVal + 'T"></i>';
            if (isNaN(totalCompareVal)) {
                liTotalHtml += '     <span class="num">— —</span>';
            } else {
                liTotalHtml += '     <span class="num">' + Math.abs(totalCompareVal) + '%</span>';
            }
            liTotalHtml += ' </div>';
            liTotalHtml += '</li>';

            liHtml += liTotalHtml;

            //处理子项
            var childrenArray = json.ItemValues;
            var childrenArrayLength = childrenArray.length;
            for (var i = 0; i < childrenArrayLength; i++) {

                var itemCname = childrenArray[i].CName;
                var itemValue = childrenArray[i].EneryValue;
                var itemUnit = childrenArray[i].Unit;
                var itemCompare = childrenArray[i].MonthCompare;
                var itemCompareVal = parseFloat(itemCompare);
                var itemLastMonVal = childrenArray[i].EnergyLastMonth;
                var icon = ""
                if (itemCompareVal < 0) {
                    icon = "down";
                } else {
                    icon = "up";
                }

                var itemHtml = '';
                itemHtml += '<li>';
                itemHtml += '	<div class="item item' + i + ' item-style-' + i % 3 + '">';
                itemHtml += '  	<span class="cname">' + itemCname + '</span>';
                itemHtml += '     <span class="value">' + itemValue + '</span>';
                itemHtml += '     <span class="unit">' + itemUnit + '</span>';
                itemHtml += ' </div>';
                itemHtml += ' <div class="percent">';
                itemHtml += ' 	<p>环比上个月</p>';
                itemHtml += ' 	<i class="' + icon + '" data-original-title="上月能耗值：' + itemLastMonVal + itemUnit + '"></i>';
                if (isNaN(itemCompareVal)) {
                    itemHtml += '     <span class="num">— —</span>';
                } else {
                    itemHtml += '     <span class="num">' + Math.abs(itemCompareVal) + '%</span>';
                }
                itemHtml += ' </div>';
                itemHtml += '</li>';

                liHtml += itemHtml;
            }

            $("#overviewList ul").html(liHtml);
            $tiansu.common.responseScroll("#overviewList", "#overviewPrev", "#overviewNext", 3);
            $("#overviewList i").tooltip();
        },

        /**
        * 本月能耗占比图信息
        * author: hf
        * time: 2014-3-31 11:30:39
        */
        pieChartInfo: function (json) {
            var liHtml = "";
            var infoArray = [];
            if (!json.IsOnlyElec) {
                infoArray = json.ItemValues;
            } else {
                infoArray.push(json.ItemValues[0]);
            }
            var infoArrayLength = infoArray.length;
            for (var i = 0; i < infoArrayLength; i++) {
                liHtml += '<li><span class="cname">' + infoArray[i].CName + ':</span><span class="value">' + infoArray[i].EnergyValue2Coal + 'T</span></li>';
            }
            $("#energyListInfo ul").html(liHtml);
        },

        /**
        * 本月能耗占比图
        * author: hf
        * time: 2014-3-31 11:30:39
        */
        renderPie: function (chartData) {
            var pieChart = appZonglan.renderTo("indexPieChart", chartData);
        },


        /**
        * 当日告警
        * author: hf
        * time: 2014-4-22 16:50:26
        */
        ajaxAlarm: function () {
            var that = this;
            $.ajax({
                url: this.getAction().indexAlarm,
                dataType: "json",
                type: "post",
                async: false,
                beforeSend: function () {
                    $tiansu.common.loading('.alarm-block', 'show');
                },
                success: function (json) {
                    if (json.ActionInfo.Success) {
                        that.setAlarmInfo(json);
                    } else {
                        $tiansu.common.info('show', {
                            content: '数据错误',
                            timeout: 2000
                        });
                    }
                },
                error: function () {
                    $tiansu.common.info('show', {
                        content: "数据请求失败，请联系管理员！",
                        timeout: 2000
                    });
                },
                complete: function () {
                    $tiansu.common.loading('.alarm-block', 'hide');
                }
            });
        },

        setAlarmInfo: function (json) {
            var AllAlarmData = json.AllAlarm;
            var UndoAlarmData = json.UndoAlarm;
            var ProcessedAlarmData = json.ProcessedAlarm;
            //填入全部告警数据
            var $all_alarm = $("#allAlarmItem");
            //AllAlarmData.iconClass = parseFloat(AllAlarmData.CompareValue) > 0 ? "up" : "down";
            AllAlarmData.iconClass = setIconClass(parseFloat(AllAlarmData.CompareValue));
            AllAlarmData.percentTxt = isNaN(parseFloat(AllAlarmData.CompareValue)) ? "--" : Math.abs(parseFloat(AllAlarmData.CompareValue)) + "%";
            $all_alarm.find(".val").html(AllAlarmData.Value);
            $all_alarm.find(".percent-val").html(AllAlarmData.percentTxt);
            $all_alarm.find("i").addClass(AllAlarmData.iconClass).attr("data-original-title", "昨日" + AllAlarmData.YesterdayValue + "条").tooltip();
            //填入待处理告警数据
            var $undo_alarm = $("#undoAlarmItem");
            //UndoAlarmData.iconClass = parseFloat(UndoAlarmData.CompareValue) > 0 ? "up" : "down";
            UndoAlarmData.iconClass = setIconClass(parseFloat(UndoAlarmData.CompareValue));
            UndoAlarmData.percentTxt = isNaN(parseFloat(UndoAlarmData.CompareValue)) ? "--" : Math.abs(parseFloat(UndoAlarmData.CompareValue)) + "%";
            $undo_alarm.find(".val>a").html(UndoAlarmData.Value);
            $undo_alarm.find(".percent-val").html(UndoAlarmData.percentTxt);
            $undo_alarm.find("i").addClass(UndoAlarmData.iconClass).attr("data-original-title", "昨日" + UndoAlarmData.YesterdayValue + "条").tooltip();
            //给待处理加链接
            var strSysDate = $("#sys-time").text();
            var undoUrl = "alarm.html?datetype=1&sdate=" + strSysDate + "&edate=" + strSysDate + "&alarmstatus=0&allalarm=true";
            $undo_alarm.find(".val>a").attr("href", undoUrl);

            //填入待处理告警数据
            var $did_alarm = $("#didAlarmItem");
            //ProcessedAlarmData.iconClass = parseFloat(ProcessedAlarmData.CompareValue) > 0 ? "up" : "down";
            ProcessedAlarmData.iconClass = setIconClass(parseFloat(ProcessedAlarmData.CompareValue));
            ProcessedAlarmData.percentTxt = isNaN(parseFloat(ProcessedAlarmData.CompareValue)) ? "--" : Math.abs(parseFloat(ProcessedAlarmData.CompareValue)) + "%";
            $did_alarm.find(".val").html(ProcessedAlarmData.Value);
            $did_alarm.find(".percent-val").html(ProcessedAlarmData.percentTxt);
            $did_alarm.find("i").addClass(ProcessedAlarmData.iconClass).attr("data-original-title", "昨日" + ProcessedAlarmData.YesterdayValue + "条").tooltip();

            function setIconClass(CompareVal) {
                var iconClass = "";
                if (CompareVal > 0) {
                    iconClass = "up";
                } else if (CompareVal < 0) {
                    iconClass = "down";
                }
                return iconClass;
            }
        },


        /**
        * Ajax request
        * author: pl
        * time: 2014-3-29 10:08:30
        */
        getAction: function () {
            return {
                index: 'action.ashx?action=indexCompare',
                indexChart: 'action.ashx?action=indexElectricRealLineChart',
                indexAlarm: 'action.ashx?action=GetAlarmIndexCount'
            };
        }
    };

    return Index;
});