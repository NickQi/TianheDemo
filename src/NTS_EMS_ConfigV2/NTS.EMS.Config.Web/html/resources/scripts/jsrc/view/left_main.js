require.config({
    baseUrl: "/html/resources/scripts/jsrc/",
    urlArgs: "",
    paths: {
        //        jquery: "lib/jquery/jquery.min",
        //        artTemplate: "lib/artTemplate/template.min",
        //        blockui: "lib/blockUI/blockUI",
        //        datepicker: "lib/datepicker/datepicker",
        //        treeview: "lib/treeview/treeview",
        //        treeviewEdit: "lib/treeview/treeview_edit",
        //        treeviewAsync: "lib/treeview/treeview_async",
        //        jscrollpane: "lib/jscrollpane/jscrollpane.min",
        //        mousewheel: "lib/jscrollpane/mousewheel",
        //        common: "common/common"


       // base: 'base',
        jquery: "lib/jquery/jquery.min",
        blockui: 'lib/blockUI/blockUI',
        jscrollpane: "lib/jscrollpane/jscrollpane.min",
        mousewheel: "lib/jscrollpane/mousewheel",
        datepicker: 'lib/datepicker/datepicker',
        //highcharts: 'lib/highcharts/highcharts.src',
        treeview: "lib/treeview/treeview",
        treeviewEdit: "lib/treeview/treeview_edit",
        treeviewAsync: "lib/treeview/treeview_async",
        // drilldown: 'lib/highcharts/modules/drilldown',
        //  chartsmod: 'chartsmod',
        common: 'common'

    }
});
//require(["jquery", "artTemplate", "blockui", "datepicker", "treeview", "treeviewAsync", "treeviewEdit", "jscrollpane", "mousewheel", "common"], function ($, ArtTemplate, BlockUI, datepicker, TreeView, TreeViewAsync, TreeViewEdit, JscrollPane, MouseWheel, Common) {
require(['jquery', 'blockui', 'jscrollpane', 'mousewheel', 'datepicker', "treeview", "treeviewAsync", "treeviewEdit", 'common'], function ($, blockui, jscrollpane, mousewheel, datepicker, TreeView, TreeViewAsync, TreeViewEdit, Common) {
    function Left() {
        this.init();
    }

    Left.prototype = {

        /**
        * Initialize page
        */
        init: function () {
            this.render();
        },

        /**
        * Render page
        */
        render: function () {
            this.bindEvent.treeSelect.call(this);
            this.bindEvent.initSelect.call(this);
             this.getTree(2, "01000");
            //			this.bindEvent.initTreeSelected.call(this);
         			this.bindEvent.setDate.call(this);
         			this.bindEvent.fastSelectDate.call(this);
            //			this.initQuery();
            //			this.bindEvent.search.call(this);
            //			this.bindEvent.slcChartTbl.call(this);
            //			this.bindEvent.changeChartType.call(this);
            //			this.bindEvent.slidePie.call(this);
        },

        bindEvent: {

            /** 
            * 双树切换（业态、区域）
            */
            treeSelect: function () {
                var that = this;
                var $selector = $("#tree-tab");
                var $treeContainer = $("#tree-container"),
					$areaSelect = $("#area-select");

                $areaSelect.children().hide().eq(0).show();

                if ($selector.size() > 0 || $treeContainer.size() > 0) {
                    $("#tree-tab>li").click(function () {
                        var index = $(this).index();
                        $(this).addClass("current").siblings().removeClass("current");
                        $treeContainer.children().hide().eq(index).show();
                        $treeContainer.children(".loading").show();
                        $areaSelect.children().hide().eq(index).show();
                        if (index == 1) {
                            var $tree = $('#areaTree-content ul:first-child');
                            if ($tree.html() == "") {
                                that.getTree(1, $("select[name=type-diff]").eq(1).val());
                            }
                        }
                    });
                }

            },

            /** 
            * 日历控件快捷方式 
            */
            fastSelectDate: function () {
            alert("32");
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
            */
            initSelect: function () {
                var that = this;
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
                                $typeDiff.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
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
                                $item.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
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
                    if (times != 1) {
                        $("#areaTree-content ul").removeClass("tree");
                        var secondItemCode = $(select).siblings("select[name=item-diff]").val();
                        var itemCode = secondItemCode != "" ? secondItemCode : $(select).val();
                        that.getTree(1, itemCode);
                    }
                } else {
                    if ($(select).attr("name") == "type-diff") {
                        that.getTree(2, $(select).val());
                    }
                    if ($(select).attr("name") == "item-diff") {
                        var secondItemCode = $(select).val(),
								ItemCode = $(select).siblings("select[name=type-diff]").val();
                        if (secondItemCode == "") {
                            secondItemCode = ItemCode;
                        }
                        that.getTree(1, secondItemCode);
                    }
                }
            },

            /** 
			 * 设置日期事件 
			 */
            setDate: function () {
                alert("323");
				var that = this;
				var $startDate = $('input[name=startDate]'),
					$endDate = $('input[name=endDate]');				
				var serverDate = this.serverDate.replace(/-/g, "/");
				var edate = $tiansu.date.format({
						date: new Date(serverDate),
						connect: '-'
					}),	
					sdate = edate;				
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
				}).on("changeDate", function() {
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
					startDate:$startDate.val(),
					endDate: that.serverDate,
					serverDate: that.serverDate,
					autoclose: true,
					minViewMode: 0,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				}).on("changeDate",function(){
					$(".fast-select a").removeClass("current");
				});				
			}
        },

        /**
        * 设置目录树
        */
        getTree: function (type, itemCode) {
        alert("tree");
            var that = this;
            var $tree = null;
            var $treeContent = null;
            var is_single = true;
            if (type === 2) {
                $tree = $('#tree-content ul:first-child');
                $treeContent = $('#tree-content');
                is_single = true;
            } else if (type === 1) {
                $tree = $('#areaTree-content ul:first-child');
                $treeContent = $('#areaTree-content');
                is_single = true;
            }
            $tree.tree({
                method: "post",
                url: "action.ashx?action=objectItemTree",
                animate: true,
                data: {
                    ItemCode: itemCode,
                    ClassId: type
                },
                selectchildren: false,
                single: is_single,
                onBeforeLoad: function () {
                    alert("onBeforeLoad");
                    $tiansu.common.loading('#tree-container', 'show');
                },
                onBeforeSelect: function (node) {
                    alert("onBeforeSelect");
                    var selctedNode = $(".tree-node-selected");
                },
                onSelect: function (node) {
                    alert("onSelect");
                    // 节点被选择时触发
                    if (!node.children.length > 0 && node.state === 'open') {
                        var param = {
                            roomId: node.id
                        };
                    }
                },
                onLoadSuccess: function () {
                    alert("Success");
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
        },

        /**
        * Ajax request
        * author: ghj
        * time: 2013-12-15 15:26:10
        */
        getAction: function () {
            return {
                tree: "action.ashx?action=objectItemTree",
                select: 'action.ashx?action=indexItem'
            };
        }
    };

    this.Login = new Left();
});