/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗排名/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appNHpm', 'chartsmod/appFenjie'], function (charts, appNHpm, appFenjie) {

    function Order() {
        this.serverDate = $("#serverDate").val();
        this.isQuering = false;

        this.chart1 = null;
        this.chart2 = null;

        //恢复默认时所用到的变量副本 开始
        this.chartDataDefault = null;
        this.chartPieDataDefault = null;
        this.tblDataDefault = null;
        this.dateStringDefault = "";
        this.itemNameDefault = "";
        //恢复默认时所用到的变量副本 结束

        this.chartData = null;
        this.chartPieData = null;
        this.tblData = null;
        this.chartReverse = false;
        this.init();
    }

    Order.prototype = {

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
            this.bindEvent.treeSelect.call(this);
            this.bindEvent.initSelect.call(this);
            var $tree = this.bindEvent.initTree.call(this);
            this.bindEvent.initTreeSelected.call(this);
            this.bindEvent.setDate.call(this);
            this.bindEvent.fastSelectDate.call(this);
            if ($tree.tree('getRoot')) {
                this.initQuery();
            } else {
                $tiansu.common.info('show', {
                    timeout: 4000,
                    content: "请联系管理员设置您的权限。"
                });
            }
            this.bindEvent.search.call(this);
            this.bindEvent.slcChartTbl.call(this);
        },

        /**
        * Bind Events
        * author: ghj
        * time: 2013-12-15 15:26:10
        */
        bindEvent: {

            /*
            * 根据sessionStorage中的值初始化参数
            * Author: pl
            * Time: 2014-4-18 13:51:46
            */
            initTree: function () {
                var storage = window.sessionStorage;
                if (storage.ObjType == 1) {
                    var $tree = $('#areaTree-content ul:first-child');
                    $('#areaTree-content').show().siblings("#tree-content").hide();
                    if ($tree.html() == "") {
                        if (storage.ItemCode2) {
                            this.getTree(1, storage.ItemCode2 || $("select[name=type-diff]").eq(1).val());
                        } else {
                            this.getTree(1, storage.ItemCode1 || $("select[name=type-diff]").eq(1).val());
                        }
                        $("#tree-tab>li.last").addClass("current").siblings(".first").removeClass("current");
                    }
                    $areaSelect = $("#area-select");
                    $areaSelect.children().hide().eq(1).show();

                } else {
                    var $tree = this.getTree(2, storage.ItemCode1 || $("select[name=type-diff]").eq(0).val());
                }
                return $tree;
            },

            /** 
            * 双树切换（业态、区域）
            * Author: HF
            * Time: 2014-3-22 15:17:20
            */
            treeSelect: function () {
                var that = this;
                var $selector = $("#tree-tab");
                var $treeContainer = $("#tree-container"),
					$areaSelect = $("#area-select");
                if ($selector.size() > 0 || $treeContainer.size() > 0) {
                    $("#tree-tab>li").click(function () {
                        var index = $(this).index();
                        $(this).addClass("current").siblings().removeClass("current");
                        $treeContainer.children("").hide().eq(index).show();
                        $treeContainer.children(".loading").show();
                        $areaSelect.children().hide().eq(index).show();
                        if (index == 1) {
                            var $tree = $('#areaTree-content ul:first-child');
                            if ($tree.html() == "") {
                                that.getTree(1, $("select[name=type-diff]").eq(1).val());
                            }
                        } else if (index == 0) {
                            var $tree = $('#tree-content ul:first-child');
                            if ($tree.html() == "") {
                                that.getTree(2, $("select[name=type-diff]").eq(1).val());
                            }
                        }
                    });
                }
            },

            /** 
            * 日历控件快捷方式 
            * author: pl
            * time: 2014-03-22 10:46:48 
            */
            fastSelectDate: function () {
                var $startDate = $('input[name=startDate]'),
					$endDate = $('input[name=endDate]');
                var today = this.serverDate;
                var $fastSelect = $(".fast-select a");
                $fastSelect.click(function () {
                    $(this).addClass("current").siblings().removeClass("current");
                    var type = $(this).attr("data-type");
                    if (type) {
                        $tiansu.date.datePeriod({
                            type: type,
                            connect: "-",
                            date: today,
                            endDate: today,
                            sDate: $startDate,
                            eDate: $endDate
                        });
                    }
                });

            },

            /** 
            * 初始化分类分项下拉框 
            * author: pl
            * time: 2014-3-23 22:03:40
            */
            initSelect: function () {
                var that = this;
                var storage = window.sessionStorage;
                var $typeDiff = $("select[name=type-diff]"),
			 	$itemDiff = $("select[name=item-diff]");
                var param = { "ItemCode": "00000" };
                param = JSON.stringify(param);
                $.ajax({
                    url: that.getAction().select,
                    dataType: "json",
                    type: 'POST',
                    data: { Inputs: param || {} },
                    async: false,
                    success: function (json) {
                        if (json.ItemLst && json.ItemLst.length > 0) {
                            $.each(json.ItemLst, function (i, item) {
                                if (item.ItemCode == storage.ItemCode1) {
                                    $typeDiff.append('<option value=' + item.ItemCode + ' selected="selected">' + item.ItemName + '</option>');
                                } else {
                                    $typeDiff.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
                                }

                            });
                            $typeDiff.append('<option value="00000">总能耗</option>');
                            that.bindEvent.secondSelect.call(that, "select[name=type-diff]", 1);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
                $("#area-select select").select2({
                    placeholder: '----------',
                    minimumResultsForSearch: -1,
                    formatNoMatches: function () {
                        return "没有数据！";
                    }
                }).on("change", function () {
                    that.bindEvent.secondSelect.call(that, this);
                });
            },

            secondSelect: function (select, times) {
                var that = this;
                var storage = window.sessionStorage;
                //$item 是级联下拉的第一级下拉
                var $item = $(select).siblings("select[name=item-diff]");
                if ($item.length != 0) {
                    $("option", $item).remove();
                    var param = { "ItemCode": $(select).val() };
                    param = JSON.stringify(param);
                    $.ajax({
                        url: that.getAction().select,
                        dataType: "json",
                        type: "POST",
                        data: { Inputs: param || {} },
                        async: false,
                        success: function (json) {
                            $item.empty();
                            $item.append('<option></option>');
                            $.each(json.ItemLst, function (i, item) {
                                if (item.ItemCode == storage.ItemCode2) {
                                    $item.append('<option value=' + item.ItemCode + ' selected="selected">' + item.ItemName + '</option>');
                                } else {
                                    $item.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
                                }
                            });
                            $item.append('<option value="">全部分项</option>');
                            $("#area-select select[name=item-diff]").select2({
                                placeholder: '---------',
                                minimumResultsForSearch: -1,
                                formatNoMatches: function () {
                                    return "没有数据！";
                                }
                            });
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            },


            /** 
            * 设置日期事件 
            * author: ghj
            * time: 2014-01-03 14:46:48 
            */
            setDate: function () {
                var that = this;
                var storage = window.sessionStorage;
                var $startDate = $('input[name=startDate]'),
					$endDate = $('input[name=endDate]');
                var serverDate = this.serverDate.replace(/-/g, "/");
                var edate, sdate;
                if (storage.StartTime && storage.EndTime) {
                    sdate = storage.StartTime;
                    edate = storage.EndTime;
                } else {
                    edate = $tiansu.date.format({
                        date: new Date(serverDate),
                        connect: '-'
                    });
                    sdate = edate;
                }
                if (edate != this.serverDate || sdate != this.serverDate) {
                    $(".fast-select a").removeClass("current");
                }
                $startDate.val(sdate);
                $endDate.val(edate);
                $startDate.datepicker({
                    format: "yyyy-mm-dd",
                    weekStart: 1,
                    autoclose: true,
                    minViewMode: 0,
                    serverDate: that.serverDate,
                    endDate: that.serverDate,
                    todayBtn: "linked",
                    todayHighlight: false,
                    keyboardNavigation: false
                }).on("changeDate", function () {
                    $(".fast-select a").removeClass("current");
                    $endDate.datepicker("remove");
                    $endDate.datepicker({
                        format: "yyyy-mm-dd",
                        weekStart: 1,
                        autoclose: true,
                        minViewMode: 0,
                        startDate: $startDate.val(),
                        endDate: that.serverDate,
                        todayBtn: "linked",
                        todayHighlight: false,
                        keyboardNavigation: false
                    });
                });
                $endDate.datepicker({
                    format: "yyyy-mm-dd",
                    weekStart: 1,
                    startDate: $startDate.val(),
                    endDate: that.serverDate,
                    serverDate: that.serverDate,
                    autoclose: true,
                    minViewMode: 0,
                    todayBtn: "linked",
                    todayHighlight: false,
                    keyboardNavigation: false
                }).on("changeDate", function () {
                    $(".fast-select a").removeClass("current");
                });
            },

            /** 
            * 初始化选中的树节点 
            * author: pl
            * time: 2014-3-24 20:17:36
            */
            initTreeSelected: function () {
                var storage = window.sessionStorage;
                if ($("#tree-content").is(":visible")) {
                    var $tree = $("#tree-content ul:first");
                } else {
                    var $tree = $("#areaTree-content ul:first");
                }
                if (storage.AreaIdLst) {
                    var node = storage.AreaIdLst.split(",");
                    $.each(node, function (i, item) {
                        var selectNode = $tree.tree("find", item);
                        selectNode && $(selectNode.target).addClass("tree-node-selected");
                        selectNode && $tree.tree("expandTo", selectNode.target);
                    })
                } else {
                    var $node = $("#tree-content ul").eq(1).children("li").children(".tree-node");
                    var length = $node.length > 20 ? 20 : $node.length;
                    for (var i = 0; i < length; i++) {
                        $($node[i]).addClass("tree-node-selected");
                    }
                }
            },

            /**
            * 点击查询按钮触发查询事件
            * Author: pl
            * Time: 2014-3-24 09:48:58
            */
            search: function () {
                var that = this;
                var param = that.getParam();
                var queryTypes = {
                    danwei: 2,
                    renjun: 3,
                    biaozhunmei: 6,
                    tanpfl: 7,
                    rmb: 8
                };
                $("#queryBtn a").click(function (e) {
                    param = that.getParam();
                    //匿名函数自我执行创建独立的作用域
                    (function () {
                        var param = that.getParam();
                        if ($("#tree-content").is(":visible")) {
                            param.ItemCode1 = $("#area-select").children().eq(0).find("select[name=type-diff]").val();
                        } else {
                            param.ItemCode1 = $("#area-select").children().eq(1).find("select[name=type-diff]").val();
                        }
                        if ($("select[name=item-diff]").is(":visible")) {
                            param.ItemCode2 = $("select[name=item-diff]").val();
                        }
                        //alert(param.ItemCode1);
                        $tiansu.common.shareParam(param);
                    })();
                    $(".fn a, .fn2 a, .fn3 a").removeClass("current");
                    if (that.isQuering == true) {
                        return false;
                    }
                    //升降序针对默认
                    $("#px-type").attr("data-target", "default");
                    var flag = that.checkParam(param);
                    if (flag) {
                        var json = that.search(param);
                        if (json.ActionInfo.Success) {
                            that.chartData = json.lineHighChart;
                            that.chartData.unit = json.Unit;
                            that.chartPieData = json.pieHighChart;
                            that.chartPieData.unit = json.Unit;
                            that.tblData = json;

                            //存储一份副本用作恢复默认 开始
                            that.chartDataDefault = $.extend(true, {}, that.chartData);
                            that.chartPieDataDefault = $.extend(true, {}, that.chartPieData);
                            that.tblDataDefault = $.extend(true, {}, that.tblData);

                            that.dateStringDefault = that.getChartInfo().dateString;
                            that.itemNameDefault = that.getChartInfo().itemName;
                            //存储一份副本用作恢复默认 结束

                            that.renderCharts(that.chartData, that.tblData, that.chartPieData);
                        } else {
                            $tiansu.common.info('show', {
                                timeout: 1000,
                                content: json.ActionInfo.ExceptionMsg
                            });
                        }
                    } else {
                        return false;
                    }
                });

                $(".fn a, .fn3 a").click(function (e) {
                    e.preventDefault();
                    if (that.isQuering == true) {
                        return false;
                    }
                    //升降序针对单位面积 优化有的代码不再分数据类别，以下这句已无作用 hf 2014-4-17 10:46:09
                    $("#px-type").attr("data-target", "danwei");
                    //var param = that.getParam();
                    var flag = that.checkParam(param);
                    if (flag) {
                        $(".fn a, .fn3 a").removeClass("current");
                        $(this).addClass("current");
                        var typesub = $(e.target).attr("id");
                        var typeId = queryTypes[typesub];
                        param.QueryType = typeId;
                        var json = that.search(param);
                        if (json.ActionInfo.Success) {
                            that.chartData = json.lineHighChart;
                            that.chartData.unit = json.Unit;
                            that.chartPieData = json.pieHighChart;
                            that.chartPieData.unit = json.Unit;
                            that.tblData = json;

                            that.renderCharts(that.chartData, that.tblData, that.chartPieData);
                        } else {
                            $tiansu.common.info('show', {
                                timeout: 1000,
                                content: json.ActionInfo.ExceptionMsg
                            });
                        }
                    } else {
                        return false;
                    }
                });

                //恢复默认
                $("#default").click(function (e) {
                    $(".fn a, .fn3 a").removeClass("current");
                    param.QueryType = 1;
                    e.preventDefault();
                    //升降序针对默认
                    $("#px-type").attr("data-target", "default");
                    that.chartData = $.extend(true, {}, that.chartDataDefault);
                    that.tblData = $.extend(true, {}, that.tblDataDefault);
                    that.chartPieData = $.extend(true, {}, that.chartPieDataDefault);
                    that.renderCharts(that.chartData, that.tblData, that.chartPieData, e);
                    //恢复树上的选择项以及param里的AreaIdLst.
                    //根据树的显示隐藏获取对应树对象
                    if ($("#tree-content").is(":visible")) {
                        var $tree = $("#tree-content ul");
                    } else {
                        var $tree = $("#areaTree-content ul");
                    }
                    //先清空掉AreaIdLst里的数据，重新添加
                    param.AreaIdLst = [];
                    $("#chart-order-panel li").each(function (index, element) {
                        var areaId = $(element).attr("data-treeid");
                        param.AreaIdLst.push(areaId);
                        var treenode = $tree.tree('find', areaId);
                        $(treenode.target).addClass("tree-node-selected");
                    });
                });
                //导出按钮
                $("#export-link").click(function () {
                    if ($(this).hasClass("export-disabled")) {
                        return false;
                    } else {
                        var flag = that.checkParam(param);
                        //var $a = $(".fn a.current");	
                        var $a = $(".fn a.current, .fn3 a.current");
                        if (flag) {
                            if ($a.length > 0) {
                                $.each($a, function (i, item) {
                                    /*
                                    switch ($(item).attr("id")) {
                                    case "danwei":
                                    param.QueryType = 2;
                                    break;
                                    case "renjun":
                                    param.QueryType = 3;
                                    break;
                                    }
                                    */
                                    var typesub = $(item).attr("id");
                                    var typeId = queryTypes[typesub];
                                    param.QueryType = typeId;
                                });
                            } else {
                                param.QueryType = 1;
                            }
                            that.exportList(param);
                        } else {
                            return false;
                        }
                    }
                });

                /*图例面板功能*/
                $(document).on("click", "#chart-order-panel ul>li>a", function (e) {
                    e.preventDefault();
                });
                $(document).on("click", "#chart-order-panel ul>li>a>.series-close", function (e) {
                    //var _this = this;
                    e.preventDefault();
                    //var target = $("#px-type").attr("data-target");
                    var $item = $(this).parents("li");
                    var treeId = $item.attr("data-treeid");
                    //根据树的显示隐藏获取对应树对象
                    if ($("#tree-content").is(":visible")) {
                        var $tree = $("#tree-content ul");
                    } else {
                        var $tree = $("#areaTree-content ul");
                    }

                    //将此值从param.AreaIdLst中删除
                    var areaIdLstLength = param.AreaIdLst.length;
                    for (var i = 0; i < areaIdLstLength; i++) {
                        if (param.AreaIdLst[i] == treeId) {
                            param.AreaIdLst.splice(i, 1);
                        }
                    }
                    //从树上删掉对应的选择
                    var treenode = $tree.tree('find', treeId);
                    $(treenode.target).removeClass("tree-node-selected");

                    //发请求
                    var flag = that.checkParam(param);
                    if (flag) {
                        var json = that.search(param);
                        if (json.ActionInfo.Success) {
                            that.chartData = json.lineHighChart;
                            that.chartData.unit = json.Unit;
                            that.chartPieData = json.pieHighChart;
                            that.chartPieData.unit = json.Unit;
                            that.tblData = json;

                            that.renderCharts(that.chartData, that.tblData, that.chartPieData);
                        } else {
                            $tiansu.common.info('show', {
                                timeout: 1000,
                                content: json.ActionInfo.ExceptionMsg
                            });
                        }
                    } else {
                        return false;
                    }


                    //_this.chartData.series[0].data.splice(index, 1);
                    //_this.renderCharts(_this.chartData, _this.tblData, _this.chartPieData);	

                });
                $(document).on("click", "#chart-order-panel .removeAll>a", function (e) {
                    e.preventDefault();
                    that.chartData.series[0].data = [];
                    that.tblData.OrderLst = [];
                    that.chartPieData.series[0].data = [];
                    that.renderCharts(that.chartData, that.tblData, that.chartPieData);
                    //$(this).parent(".removeAll").remove();
                    //清除树上的选择项
                    //根据树的显示隐藏获取对应树对象
                    if ($("#tree-content").is(":visible")) {
                        var $tree = $("#tree-content ul");
                    } else {
                        var $tree = $("#areaTree-content ul");
                    }
                    $tree.find(".tree-node-selected").removeClass("tree-node-selected");
                });
            },

            /**
            * 图表表格切换
            * Author: HF
            * Time: 2014-3-23 21:48:55
            * 备注：若图表容器隐藏式render图表，会造成图表大小不能自适应，须作为$chart.show()的回调函数执行，为此设置全局性的
            * this.chart1 = null;
            *	this.chartData = null;
            *	this.tblData = null;
            *	this.chartReverse = false;
            */
            slcChartTbl: function () {
                var _this = this;
                var $tbl = $("#table-order");
                var $chart = $("#chart-order");
                $("#slcChartTbl>a").click(function (e) {
                    e.preventDefault();
                    $parent = $(this).parent("#slcChartTbl");
                    if ($parent.hasClass("to-tbl")) {
                        $parent.removeClass("to-tbl").addClass("to-chart");
                        $(this).text("切换为图表");
                        $tbl.show();
                        $chart.hide();
                        $("#export-link").removeClass().addClass("export")
                    } else if ($parent.hasClass("to-chart")) {
                        $parent.removeClass("to-chart").addClass("to-tbl");
                        $(this).text("切换为表格");
                        $tbl.hide();
                        $chart.show(0, function () {
                            _this.renderCharts(_this.chartData, _this.tblData, _this.chartPieData);
                        });
                        $("#export-link").removeClass().addClass("export-disabled");
                    }
                });
            }
        },

        /**
        * 页面初始化查询
        * Author: pl
        * Time: 2014-3-24 20:43:09
        */
        initQuery: function () {
            var _this = this;
            var param = this.getParam();
            var json = this.search(param);
            if (json.ActionInfo.Success) {
                this.chartData = json.lineHighChart;
                this.chartData.unit = json.Unit;
                this.chartPieData = json.pieHighChart;
                this.chartPieData.unit = json.Unit;
                this.tblData = json;

                //存储一份副本用作恢复默认 开始
                this.chartDataDefault = $.extend(true, {}, _this.chartData);
                this.chartPieDataDefault = $.extend(true, {}, _this.chartPieData);
                this.tblDataDefault = $.extend(true, {}, _this.tblData);

                this.dateStringDefault = this.getChartInfo().dateString;
                this.itemNameDefault = this.getChartInfo().itemName;
                //存储一份副本用作恢复默认 结束

                this.renderCharts(this.chartData, this.tblData, this.chartPieData);
            } else {
                $tiansu.common.info('show', {
                    timeout: 1000,
                    content: json.ActionInfo.ExceptionMsg
                });
            }

            //转换排序
            $("#px-type").click(function (e) {
                e.preventDefault();
                //默认、单位面积、人均 优化后的通用代码不必区分target 以下这句不再起作用 hf 2014-4-17 10:54:39
                var target = $(this).attr("data-target");

                //reverse = !reverse;
                _this.chartReverse = !(_this.chartReverse);

                if (_this.chartReverse) {
                    $(this).text("降序排名").parent("span").removeClass("order-type-1").addClass("order-type-2");
                } else {
                    $(this).text("升序排名").parent("span").removeClass("order-type-2").addClass("order-type-1");
                }
                _this.renderCharts(_this.chartData, _this.tblData, _this.chartPieData);

            });
        },

        /**
        * 设置目录树
        * author: ghj
        * time: 2014-01-17 00:51:14
        */
        getTree: function (type, itemCode) {
            var that = this;
            var $tree = null;
            var $treeContent = null;
            var is_single = false;
            if (type === 2) {
                $tree = $('#tree-content ul:first-child');
                $treeContent = $('#tree-content');
            } else if (type === 1) {
                $tree = $('#areaTree-content ul:first-child');
                $treeContent = $('#areaTree-content');
            }
            var maxlen = {
                num: 20,
                detail: "能耗排名"
            };
            var data = { "ClassId": type };
            var data = JSON.stringify(data);
            $tree.tree({
                method: 'post',
                url: this.getAction().tree,
                animate: true,
                data: {
                    Inputs: data
                },
                selectchildren: true,
                maxlen: maxlen,
                single: is_single,
                onBeforeLoad: function () {
                    $tiansu.common.loading('#tree-container', 'show');
                },
                onBeforeExpand: function (node, param) {
                    var data = {
                        "ParentID": node.id,
                        "ClassId": type
                    }
                    var data = JSON.stringify(data);
                    $tree.tree('options').data =
					{
					    Inputs: data
					};
                },
                onBeforeSelect: function (node) {
                    var $treeVisible = $(".tree");
                    $.each($treeVisible, function (i, item) {
                        if ($(item).is(":visible")) {
                            selctedNode = $(item).find(".tree-node-selected");
                        }
                    });
                    var isSelected = $(node.target).hasClass("tree-node-selected");
                    if (selctedNode.length >= maxlen.num && !isSelected) {
                        $tiansu.common.info('show', {
                            container: '.tree',
                            centerX: false,
                            centerY: false,
                            css: {
                                top: '40%',
                                left: '50px'
                            },
                            content: maxlen.detail + "最多选择" + maxlen.num + "个对象！",
                            timeout: 2000
                        });
                        return false;
                    }
                },
                onSelect: function (node) {
                    // 节点被选择时触发
                },
                onLoadSuccess: function (node) {
                    if (node && node.children.length > 1) {
                        $(node.target).append('<span class="select-all pl5 pr5 ml5">all+</span>');
                    }

                    $tiansu.common.loading('#tree-container', 'hide');
                    //$('#tree-content').jScrollPane({
                    $treeContent.jScrollPane({
                        autoReinitialise: true,
                        mouseWheelSpeed: 20
                    });
                    $('.scroll-tbody').jScrollPane({
                        autoReinitialise: true,
                        mouseWheelSpeed: 20
                    });
                }
            });
            return $tree;
        },

        /**
        * 获取查询参数
        * author: pl
        * time: 2014-3-23 21:14:02
        */
        getParam: function () {
            var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');
            var secondItemCode = $("#area-select").children().eq(1).find("select[name=item-diff]").val();
            //根据树的显示隐藏获取对应参数
            if ($("#tree-content").is(":visible")) {
                var $tree = $("#tree-content ul"),
				    $treeNode = $("#tree-content .tree-node-selected"),
				    objType = 2,
				    itemCode = $("#area-select").children().eq(0).find("select[name=type-diff]").val();
            } else {
                var $tree = $("#areaTree-content ul"),
					$treeNode = $("#areaTree-content .tree-node-selected"),
					objType = 1,
					itemCode = secondItemCode != "" ? secondItemCode : $("#area-select").children().eq(1).find("select[name=type-diff]").val();
            }
            var param = {},
				nodeList = [];
            //遍历选中的节点得到节点id
            $.each($treeNode, function (i, item) {
                var nodeSelected = $tree.tree('getNode', item);
                nodeList.push(nodeSelected.id);
            });
            //计算时间颗粒度
            param.particle = $tiansu.common.getParticle($startDate.val(), $endDate.val());
            param.StartTime = $startDate.val();
            param.EndTime = $endDate.val();
            param.AreaIdLst = nodeList;
            param.ObjType = objType;
            param.ItemCode = itemCode ? itemCode : "00000";
            param.QueryType = 1;
            return param;
        },

        /**
        * 参数验证
        * Author: pl
        * Time: 2014-3-25 21:46:30
        */
        checkParam: function (param) {
            if (param.AreaIdLst.length < 2) {
                $tiansu.common.info("show", {
                    content: "请至少选择2个对象！",
                    css: {
                        centerX: false,
                        centerY: false,
                        top: '40%',
                        left: '150px'
                    },
                    timeout: 2000
                });
                return false;
            } else {
                return true;
            }
        },

        /**
        * 查询数据
        * author: pl
        * time: 2014-3-24 09:41:13
        */
        search: function (param) {
            //console.log(param);
            var that = this;
            var result = null;
            var param = JSON.stringify(param);
            $.ajax({
                url: this.getAction().dataUrl,
                dataType: 'json',
                type: 'POST',
                data: { inputs: param || {} },
                async: false,
                beforeSend: function () {
                    that.isQuering = true;
                    $tiansu.common.loading('#chart-order', 'show');
                    $tiansu.common.loading('#chart-order-panel', 'show');
                    $tiansu.common.loading('#chart-order-pie', 'show');
                },
                success: function (json) {
                    result = json;
                },
                error: function () {
                    $tiansu.common.info('show', {
                        timeout: 1000,
                        content: '数据请求失败！'
                    });
                },
                complete: function () {
                    $tiansu.common.loading('#main', 'hide');
                    that.isQuering = false;
                }
            });
            return result;
        },

        /**
        * 导出数据列表
        * author: pl
        * time: 2014-3-28 10:21:42
        */
        exportList: function (param) {
            var paramString = JSON.stringify(param);
            $.ajax({
                url: this.getAction().exportUrl,
                dataType: 'json',
                type: 'POST',
                data: {
                    inputs: paramString || {}
                },
                async: false,
                beforeSend: function () {
                    this.isQuering = true;
                    $tiansu.common.loading('#table-order', 'show');
                },
                success: function (json) {
                    if (json.status == "success") {
                        window.open(json.msg);
                    } else if (json.status == "error") {
                        $tiansu.common.info('show', {
                            timeout: 1000,
                            content: json.msg
                        });
                    }
                },
                error: function () {
                    $tiansu.common.info('show', {
                        timeout: 1000,
                        content: '数据请求失败！'
                    });
                },
                complete: function () {
                    $tiansu.common.loading('#table-order', 'hide');
                    this.isQuering = false;
                }
            });
        },

        /** 
        * 渲染highcharts图表
        * @param  {obj} json [列表json数据]
        * author: HF
        * time: 2014-3-24 18:00:21
        */
        renderCharts: function (chartData, tblData, chartPieData, e) {

            var newChartData = $.extend(true, {}, chartData);
            //var newTblData = $.extend(true, {}, tblData);
            var newChartPieData = $.extend(true, {}, chartPieData);

            charts.resetTimezone();

            //console.log(newTblData.OrderLst);

            if (this.chartReverse) {
                newChartData.series[0].data.reverse();
                //newTblData.OrderLst.reverse();
            }

            this.chart1 = appNHpm.renderTo("chart-order", newChartData, this.chartReverse);
            //生成表格
            this.renderList(tblData);

            $("#chartsOrder .unit>span").text(chartData.unit);
            $("#chartsOrder h3 span.itemName").html(this.itemNameDefault);
            $("#chartsOrder h3 span.series-date").html(this.dateStringDefault);

            this.chart2 = appFenjie.renderTo("chart-order-pie", newChartPieData);


            if ($("#chart-order-panel .jspPane").size() > 0) {
                //var panelContent = $("#chart-order-panel").data('jsp').getContentPane();
                appNHpm.setPanel(this.chart1, "#chart-order-panel .jspPane", true);
                //去除掉removeAll按钮，以免重复添加
                $("#chart-order-panel .jspPane .removeAll").remove();
                //$("#chart-order-panel .jspPane").jScrollPane({
                //	autoReinitialise: true,
                //	mouseWheelSpeed: 20
                //});
            } else {
                appNHpm.setPanel(this.chart1, "#chart-order-panel", true);
                $("#chart-order-panel").jScrollPane({
                    autoReinitialise: true,
                    mouseWheelSpeed: 20
                });
            }

            //将remove all按钮移到滚动内容之外，使之位置固定
            if ($("#chart-order-panel .jspPane").size() > 0) {
                $("#chart-order-panel .removeAll").appendTo($("#chart-order-panel"));
            }
        },

        getChartInfo: function () {
            var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');
            var startDate = $startDate.val();
            var endDate = $endDate.val();
            var dateString = "(" + startDate + " — " + endDate + ")";
            var itemName = "";

            //根据树的显示隐藏获取对应参数
            if ($("#tree-content").is(":visible")) {
                itemName = $("#area-select").children().eq(0).find("select[name=type-diff]").find("option:selected").text();
                if (itemName === "总能耗") {
                    itemName = "总";
                }
            } else {
                itemName = $("#area-select").children().eq(1).find("select[name=item-diff]").find("option:selected").text() || $("#area-select").children().eq(1).find("select[name=type-diff]").find("option:selected").text();
                if (itemName === "总能耗") {
                    itemName = "总";
                }
            }

            return {
                dateString: dateString,
                itemName: itemName
            }

        },


        /** 
        * 渲染列表
        * @param  {obj} json [列表json数据]
        * author: PL & HF
        * time: 2013-12-15 15:46:19
        */
        renderList: function (json) {

            var newJson = $.extend(true, {}, json);
            if (this.chartReverse) {
                newJson.OrderLst.reverse();
            }

            var $tbody = $('#table-order tbody');
            var items = template('list-tpl', newJson);
            // 列表
            $tbody.html(items);
        },



        /**
        * Ajax request
        * author: ghj
        * time: 2013-12-15 15:26:10
        */
        getAction: function () {
            return {
                tree: "action.ashx?action=objectItemTree",
                select: 'action.ashx?action=indexItem',
                dataUrl: 'action.ashx?action=shopordernew',
                exportUrl: 'action.ashx?action=exportExcelDataRankingNew'
            };
        }
    };

    return Order;
});
