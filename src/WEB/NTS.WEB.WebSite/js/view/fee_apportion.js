/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗对比/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appFeeApportionPie', 'chartsmod/appFeeApportion'], function(charts, appFeeApportionPie, appFeeApportion) {

	function FeeApportion() {
		//global对象用来存储全局变量
		this.global = {};
		this.serverDate = $("#sys-time").html();
		this.init();
	}

    FeeApportion.prototype = {

		/**
		 * Initialize page
		 * author: ghj
		 * time: 2013-12-29 20:28:47
		 */
		init: function() {
			this.render();
		},

		/**
		 * Render page
		 * author: ghj
		 * time: 2013-12-29 20:28:47
		 */
		render: function() {
			this.bindEvent.treeSelect.call(this);
			this.bindEvent.initSelect.call(this);
			var $tree = this.getTree(2, $("select[name=type-diff]").eq(0).val());
			this.bindEvent.initTreeSelected.call(this);
			this.bindEvent.setDate.call(this);
			if($tree.tree('getRoot')) {
                this.initQuery();
            }else{
                $tiansu.common.info('show', {
                    timeout: 4000,
                    content: "请联系管理员设置您的权限。"
                });
            }
			this.bindEvent.search.call(this);
			this.bindEvent.slcChartTbl.call(this);
            this.bindEvent.exportList.call(this);
		},

		/**
		 * Bind Events
		 * author: ghj
		 * time: 2013-12-15 15:26:10
		 */
		bindEvent: {

			/** 
			 * 双树切换（业态、区域）
			 * Author: HF
			 * Time: 2014-3-22 15:17:20
			 */
			treeSelect: function() {
				var that = this;
				var $selector = $("#tree-tab");
				var $treeContainer = $("#tree-container"),
					$areaSelect = $("#area-select");
				if ($selector.size() > 0 || $treeContainer.size() > 0) {
					$("#tree-tab>li").click(function() {
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
			 * author: pl
			 * time: 2014-03-22 10:46:48
			 */
			setDate: function() {
				var $startDate = $('input[name=startDate]');
				var that = this;
				var monthDay = this.serverDate.slice(0,8)+"01";
				$startDate.val(monthDay);
				$startDate.datepicker({
					format: "yyyy-mm-dd",
					weekStart: 1,
					endDate: that.serverDate,
					autoclose: true,
					minViewMode: 1,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				});
			},

			/** 
			 * 初始化分类分项下拉框
			 * author: pl
			 * time: 2014-3-23 22:03:40
			 */
			/*initSelect: function() {
				var that = this;
				var $typeDiff = $("select[name=type-diff]");
				var param = {
					"ItemCode": "00000"
				};
				param = JSON.stringify(param);
				$.ajax({
					url: that.getAction().select,
					dataType: "json",
					type: "POST",
					data: {
						Inputs: param || {}
					},
					async: false,
					success: function(json) {
						if (json.ItemLst && json.ItemLst.length > 0) {
							$.each(json.ItemLst, function(i, item) {
								$typeDiff.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
							});							
						}
					},
					error: function(error) {
						console.log(error);
					}
				});
				$("#area-select select").select2({
					placeholder: '----------',
					minimumResultsForSearch: -1,
					formatNoMatches: function() {
						return "没有数据！";
					}
				}).on("change", function() {	
					$("#tree-tab .current").hasClass("first") ?	that.getTree(2, $(this).val()) : that.getTree(1, $(this).val());
				});
			},*/
			initSelect: function() {
				var that = this;
				var $typeDiff = $("select[name=type-diff]"),
					$itemDiff = $("select[name=item-diff]");
				var param = {
					"ItemCode": "00000"
				};
				param = JSON.stringify(param);
				$.ajax({
					url: that.getAction().select,
					dataType: "json",
					type: "POST",
					data: {
						Inputs: param || {}
					},
					async: false,
					success: function(json) {
						if (json.ItemLst && json.ItemLst.length > 0) {
							$.each(json.ItemLst, function(i, item) {
								$typeDiff.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
							});
							that.bindEvent.secondSelect.call(that, "select[name=type-diff]", 1);
						}
					},
					error: function(error) {
						console.log(error);
					}
				});
				$("#area-select select").select2({
					placeholder: '----------',
					minimumResultsForSearch: -1,
					formatNoMatches: function() {
						return "没有数据！";
					}
				}).on("change", function() {
					that.bindEvent.secondSelect.call(that, this);
				});
			},

			secondSelect: function(select, times) {
				var that = this;
				//$item 是级联下拉的第一级下拉
				var $item = $(select).siblings("select[name=item-diff]");
				if ($item.length != 0) {
					$("option", $item).remove();
					var param = {
						"ItemCode": $(select).val()
					};
					param = JSON.stringify(param);
					$.ajax({
						url: that.getAction().select,
						dataType: "json",
						type: "POST",
						data: {
							Inputs: param || {}
						},
						async: false,
						success: function(json) {
							$item.empty();
							$item.append('<option></option>');
							$.each(json.ItemLst, function(i, item) {
								$item.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
							});
							$item.append('<option value="">全部分项</option>');
							$("#area-select select[name=item-diff]").select2({
								placeholder: '---------',
								minimumResultsForSearch: -1,
								formatNoMatches: function() {
									return "没有数据！";
								}
							});
						},
						error: function(error) {
							console.log(error);
						}
					});
				}
			},

			/** 
			 * 初始化选中的树节点
			 * author: pl
			 * time: 2014-3-24 20:17:36
			 */
			initTreeSelected: function() {
				$("#tree-content ul").eq(1).children("li").children(".tree-node").eq(0).addClass("tree-node-selected");
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
			 slcChartTbl: function(){
				var that = this;	 
				var $tbl = $("#obj-apportion-tbl");
				var $chart = $("#obj-apportion-chart");
			 	$("#slcChartTbl>a").click(function(e){
					e.preventDefault();
					$parent = $(this).parent("#slcChartTbl");
					if($parent.hasClass("to-tbl")){
						$parent.removeClass("to-tbl").addClass("to-chart");
						$(this).text("切换为图表");
						$tbl.show();
						$chart.hide();
						$("#export-link").removeClass().addClass("export")
					}else if($parent.hasClass("to-chart")){
						$parent.removeClass("to-chart").addClass("to-tbl");
						$(this).text("切换为表格");
						$tbl.hide();
						
						$chart.show(0, function(){
							that.global.objApportionCharts.reflow();
						});
						
						$("#export-link").removeClass().addClass("export-disabled");
					}
				});	
			 },

			/**
			 * 点击查询按钮触发查询事件
			 * Author: pl
			 * Time: 2014-3-24 09:48:58
			 */
			search: function() {
				var that = this;
				var param = that.getParam();
				$("#queryBtn").click(function() {
					param = that.getParam();
					var flag = that.checkParam(param);
					if (flag == true) {
						that.search(param);
					}

				});
			},

            //导出按钮
            exportList:function(){
                var that= this;
                $("#export-link").click(function() {
                    if($(this).hasClass("export-disabled")){
                        return false;
                    }else{
                        var param = that.getParam();
                        var flag = that.checkParam(param);
                        var $a = $(".fn a.current");
                        if (flag) {
                            that.exportList(param);
                        } else {
                            return false;
                        }
                    }
                });
            }

		},



		/**
		 * 页面初始化查询
		 * Author: pl
		 * Time: 2014-3-24 20:43:09
		 */
		initQuery: function() {
			$("#multi-obj input[name='time-span']").attr("disabled", false).eq(0).click();
			var param = this.getParam();
			this.search(param);			
			
		},

		/**
		 * 设置目录树
		 * author: ghj
		 * time: 2014-01-17 00:51:14
		 */
		getTree: function(type, itemCode) {
			var that = this;
			var $tree = null;
			var $treeContent = null;
			var is_single = true;
			if (type === 2) {
				$tree = $('#tree-content ul:first-child');
				$treeContent = $('#tree-content');
			} else if (type === 1) {
				$tree = $('#areaTree-content ul:first-child');
				$treeContent = $('#areaTree-content');
			}
			//$tiansu.common.info("show",{container:"#tree-box"});
			//$tiansu.common.loading("#tree-box","show");
			var data = {"ClassId":type};
			var data = JSON.stringify(data);
			$tree.tree({
				method: 'post',
				url: this.getAction().tree,
				animate: true,
				data:{
					Inputs:data
				},
				selectchildren: true,
				single: is_single,
				onBeforeLoad: function() {
					$tiansu.common.loading('#tree-container', 'show');
				},
				onBeforeExpand:function(node,param){ 
					var data = {
							"ParentID":node.id,
							"ClassId":type
						}
					var data = JSON.stringify(data);
                    $tree.tree('options').data =
					{
						Inputs:data
					};
                },
				onLoadSuccess: function() {
					$tiansu.common.loading('#tree-container', 'hide');
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
			if (that.isObj == false) {
				$(".select-all").addClass("hide");
			}
			return $tree;
		},

		/**
		 * 获取查询参数
		 * author: pl
		 * time: 2014-3-23 21:14:02
		 */
		getParam: function() {
			var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');
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
					itemCode = $("#area-select").children().eq(1).find("select[name=item-diff]").val() || $("#area-select").children().eq(1).find("select[name=type-diff]").val();
			}
			var param = {};
			
			var nodeSelected = $tree.tree('getNode', $treeNode);
			if (nodeSelected != null) {
				param.ObjectId = nodeSelected.id;
			}
			//计算时间颗粒度
			param.Particle = $('input[name=time-span]:checked').val();
			param.StartTime = $startDate.val();			
			param.ObjType = objType;
			param.ItemCode = itemCode ? itemCode : "00000";
			return param;
		},

		/**
		 * 参数验证
		 * Author: pl
		 * Time: 2014-3-25 21:46:30
		 */
		checkParam: function(param) {
				if (!param.ObjectId) {
					$tiansu.common.info("show", {
						content: "请选择1个对象！",
						css: {
							centerX: false,
							centerY: false,
							top: '40%',
							left: '150px'
						},
						timeout: 2000
					});
					return false;
				}else {
					return true;
				}
		},

		/**
		 * 查询数据
		 * author: pl
		 * time: 2014-3-24 09:41:13
		 */
		search: function(param) {
			//console.log(param);
			var that = this;
			var objParam = param;
			var param = JSON.stringify(param);
			//var ctype = 1;

			$.ajax({
				url: this.getAction().dataUrl,
				dataType: 'json',
				type: 'POST',
				data: {
					Inputs: param || {}
				},
				beforeSend: function() {
					$tiansu.common.loading('#chart-fee', 'show');
					$tiansu.common.loading('#fee-analyses-info', 'show');
					$tiansu.common.loading('#chart-fee-pencents', 'show');
				},
				success: function(json) {
					
					
					if (json.ActionInfo.Success) {

						$("#apportion-pie").show();
                        $("#apportion-info").show();
                        $(".zeroMsg").remove();
                        //填充能耗分类和时间和面包屑
                        var info = that.getChartInfo(objParam);
                        $("#chartsApportion h3>.itemName").html(info.itemName);
                        $("#chartsApportion h3>.series-date").html(info.dateString);

                        var treeTypeVal = objParam.ObjType;
                        var treeElem = "#tree-content";
                        if(treeTypeVal == 1){
                            treeElem = "#areaTree-content";
                        }else{
                            treeElem = "#tree-content";
                        }
                        var breadcrumb = that.getTreeNodePath(treeElem);
                        $("#chartsApportion h3 span.areaName").html(breadcrumb);
						
                        //渲染右上侧信息
                        that.fillFeeInfo(json.FeeApportionVal);

                        //渲染饼图
                        that.renderFeeApportionPie(json.FeeApportionVal);

                        //填充分摊方式
                        that.setApportionType(json);

                        //分摊对象柱状图
                        that.global.objApportionCharts = that.renderApportionObj(json.FeeApportionCharts);

                        //渲染表格
                        that.renderTbl(json.FeeApportionTbl);


					
					} else {
						/*
						$tiansu.common.info('show', {
							timeout: 1000,
							content: json.ActionInfo.ExceptionMsg
						});
						*/
						$("#apportion-pie").hide();
                        $("#apportion-info").hide();
                        $(".zeroMsg").remove();
                        $("<div class='zeroMsg'>" + json.ActionInfo.ExceptionMsg + "</div>").appendTo("#chart-apportion");
                        //清除图表数据和表格数据
                        var zeroChartData = {
                            "series": [{data:[],name:"费用未分摊"}]
                        }
                        that.global.objApportionCharts = that.renderApportionObj(zeroChartData);

                        var zeroTblData = {
                            "FeeApportionList": []
                        };
                        that.renderTbl(zeroTblData);
					}
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败！'
					});
				},
				complete: function() {
					$tiansu.common.loading('#main', 'hide');
				}
			});
		},

        /**
         * 填充右上角费用信息
         * author: HF
         * time: 2014-4-19 11:15:32
         */
        fillFeeInfo: function(json){
            //分摊前费用：
            var feeBefore = json.BeforeVal;
            var feeBeforeLastMonth = json.BeforeValLastMonth;
            var feeBeforeCompare = parseFloat(json.BeforeValCompare);
            var feeBeforeIconClass = getIconClass(feeBeforeCompare);
            fillVal("#beforeApporInfo", feeBefore, feeBeforeLastMonth, feeBeforeCompare, feeBeforeIconClass);
            //分摊费用
            var feeApportion = json.ApportionVal;
            var feeApportionLastMonth = json.ApportionValLastMonth;
            var feeApportionCompare = parseFloat(json.ApportionValCompare);
            var feeApportionIconClass = getIconClass(feeApportionCompare);
            fillVal("#apporInfo", feeApportion, feeApportionLastMonth, feeApportionCompare, feeApportionIconClass);
            //总费用
            var feeTotal = json.TotalVal;
            var feeTotalLastMonth = json.TotalValLastMonth;
            var feeTotalCompare = parseFloat(json.TotalValCompare);
            var feeTotalIconClass = getIconClass(feeTotalCompare);
            fillVal("#totalApporInfo", feeTotal, feeTotalLastMonth, feeTotalCompare, feeTotalIconClass);

            //确定上下箭头的Class：
            function getIconClass(val){
                return (val < 0) ? "down" : "up";
            }
            //填充数据
            function fillVal(id, val, lastmonth, compare, iconClass){
                var $li = $(id);
                $li.find(".fee-val").html(val);
                var compareVal = Math.abs(compare);
                if(isNaN(compareVal)){
                    $li.find(".percent-val").html("--");
                }else {
                    $li.find(".percent-val").html(Math.abs(compare) + "%");
                }
                $li.find("i").removeClass().addClass(iconClass).attr("data-original-title", "上月费用：" + lastmonth + "元").tooltip();
            }
        },

        /**
         * 渲染饼图
         * author: HF
         * time: 2014-4-19 12:07:21
         */
        renderFeeApportionPie: function(json){
            var chartData = {
                series: [
                    {
                        data:[
                            {
                                "name": "分摊前费用",
                                "y": json.BeforeVal
                            },
                            {
                                "name": "分摊费用",
                                "y": json.ApportionVal
                            }
                        ]
                    }
                ]
            };
            return appFeeApportionPie.renderTo("apportion-pie", chartData);
        },

        /**
         * 填充分摊方式
         * author: HF
         * time: 2014-4-19 14:01:04
         */
        setApportionType: function(json){
            $("#tool-bar").find(".fn>li").eq(0).children("a").html(json.FeeApportionType);
        },

        /**
         * 分摊对象柱状图
         * author: HF
         * time: 2014-4-19 14:01:34
         */
        renderApportionObj: function(chartData){
            return appFeeApportion.renderTo("obj-apportion-chart", chartData);
        },


		getChartInfo: function(param) {
			//var $startDate = $('input[name=startDate]'),
			//	$endDate = $('input[name=endDate]');
			//var startDate = $startDate.val();
			//var endDate = $endDate.val();
			var startDate = new Date(param.StartTime.replace(/-/g, "/"));
			var year = startDate.getFullYear();
			var month = startDate.getMonth() + 1;
			//var particle = param.Particle;
			var dateString = "(" + year + "年" + month + "月)";

			var itemName = "";

			//根据树的显示隐藏获取对应参数
			if ($("#tree-content").is(":visible")) {
				itemName = $("#area-select").children().eq(0).find("select[name=type-diff]").find("option:selected").text();
                if(itemName === "总能耗"){
                    itemName = "总";
                }
			} else {
				itemName = $("#area-select").children().eq(1).find("select[name=item-diff]").find("option:selected").text() || $("#area-select").children().eq(1).find("select[name=type-diff]").find("option:selected").text();
                if(itemName === "总能耗"){
                    itemName = "总";
                }
			}

			return {
				dateString: dateString,
				itemName: itemName
			}

		},
		
		getTreeNodePath: function(elem){
            var pathInfo = {};
            //var $tree = $("#tree-content ul.tree");
            //var $treeNode = $("#tree-content .tree-node-selected");
            $tree = $(elem).find("ul:first-child");
            var $treeNode = $(elem).find(".tree-node-selected");
            var nodeSelected = $tree.tree('getNode',$treeNode);
            var pnode = getParentNode(nodeSelected);
            var pathArry = [];
            pathArry.push(nodeSelected.text);
            //while(pnode !== null){
            //	pathArry.push(pnode.text);
            //	pnode = getParentNode(pnode);
            //}
            if(pnode){
                pathArry.push(pnode.text);
            }
            //删掉数组里的最后一项，即第一级节点
            //pathArry.pop();
            var path = pathArry.reverse().join("　");
            pathInfo.fullPath = path;
            //保留16个字及最后一个节点
            if(path.length > 16){
                var theLastNodeTextIndex = path.lastIndexOf("　");
                var theLastNodeText = path.slice(theLastNodeTextIndex + 1);
                var theLastNodeTextLength = theLastNodeText.length;
                if(theLastNodeTextLength < 16) {
                    var theRestLength = 16 - theLastNodeTextLength;
                    var theRestString = path.substring(0, theRestLength);
                    path = theRestString + "..." + theLastNodeText;
                }else{
                    path = theLastNodeText.substring(0, 16) + "...";
                }
            }
            pathInfo.shortPath = path;
            return pathInfo;

            function getParentNode(node){
                return $tree.tree("getParent", node.target)
            }
        },

        /**
         * 渲染表格
         * author: HF
         * time: 2014-4-19 14:59:14
         */
        renderTbl: function(json){
            var $tbody = $('#obj-apportion-tbl tbody');
            var items = template("list-tpl", json);
            // 列表
            $tbody.html(items);
        },


        /**
         * 导出数据列表
         * author: pl
         * time: 2014-3-28 10:21:42
         */
        exportList: function(param){
            var paramString = JSON.stringify(param);
            $.ajax({
                url: this.getAction().exportUrl,
                dataType: 'json',
                type: 'POST',
                data: {
                    inputs: paramString || {}
                },
                async: false,
                beforeSend: function() {
                    this.isQuering = true;
                    $tiansu.common.loading('#chart-load-tbl', 'show');
                },
                success: function(json) {
                    if (json.status == "success") {
                        window.open(json.msg);
                    } else if (json.status == "error") {
                        $tiansu.common.info('show', {
                            timeout: 1000,
                            content: json.msg
                        });
                    }
                },
                error: function() {
                    $tiansu.common.info('show', {
                        timeout: 1000,
                        content: '数据请求失败！'
                    });
                },
                complete: function() {
                    $tiansu.common.loading('#chart-load-tbl', 'hide');
                    this.isQuering = false;
                }
            });
        },


		/**
		 * Ajax request
		 * author: ghj
		 * time: 2013-12-15 15:26:10
		 */
		getAction: function() {
			return {
			    tree: "action.ashx?action=objectItemTree",
			    select: 'action.ashx?action=indexItem',
			    dataUrl: 'action.ashx?action=GetFeeApportion',
                exportUrl:'action.ashx?action=ExportFeeApportion',
			};
		}
	};

	return FeeApportion;
});