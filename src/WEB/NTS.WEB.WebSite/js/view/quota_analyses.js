/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗对比/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appDinge', 'chartsmod/appDiff', 'chartsmod/appTrend'], function(charts, appDinge, appDiff, appTrend) {

	function Quota() {
		this.serverDate = $("#sys-time").html();
		this.init();
	}

	Quota.prototype = {

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
			this.bindEvent.fastSelectDate.call(this);
			this.bindEvent.timeSpanSelect.call(this);
			if($tree.tree('getRoot')) {
                this.initQuery();
            }else{
                $tiansu.common.info('show', {
                    timeout: 4000,
                    content: "请联系管理员设置您的权限。"
                });
            }
			this.bindEvent.search.call(this);
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

			month: function() {
				var $startDate = $('#multi-obj input[name=startDate]');
				$startDate.datepicker("remove");
				$startDate.datepicker({
					format: "yyyy-mm",
					weekStart: 1,
					endDate: this.serverDate,
					autoclose: true,
					minViewMode: 1,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				}).on("changeDate", function() {
					$(".fast-select a").removeClass("current");
				});
			},

			year: function() {
				var $startDate = $('#multi-obj input[name=startDate]');
				$startDate.datepicker("remove");
				$startDate.datepicker({
					format: "yyyy",
					weekStart: 1,
					endDate: this.serverDate,
					autoclose: true,
					minViewMode: 2,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				}).on("changeDate", function() {
					$(".fast-select a").removeClass("current");
				});
			}, 
			/** 
			 * 日历控件快捷方式
			 * author: pl
			 * time: 2014-03-22 10:46:48
			 */
			fastSelectDate: function() {
				var $startDate = $('#multi-obj input[name=startDate]'),
					$endDate = $('#multi-obj input[name=endDate]');
				var that = this;
				var today = this.serverDate;   
				var $fastSelect = $(".fast-select a");
				var $timeSpan = $("input[name=time-span]");
				$startDate.val(that.serverDate.slice(0,7));
				$fastSelect.click(function() {
					$(this).addClass("current").siblings().removeClass("current");
					$("input[name=time-span]").eq($(this).index()).click();
					if($(this).index() == 0){
						var mm = that.serverDate.slice(0,7);
						$startDate.val(mm);											
					}else{
						var yyyy = that.serverDate.slice(0,4);
						$startDate.val(yyyy);
					}
					$(this).addClass("current")
				});

			},

			/** 
			 * 日历时间跨度选择
			 * author: pl
			 * time: 2014-3-25 20:05:45
			 */
			timeSpanSelect: function() {
				var that = this;
				var $startDate = $('#multi-obj input[name=startDate]'),
					$endDate = $('#multi-obj input[name=endDate]');
				var that = this;
				$startDate.datepicker({
					format: "yyyy-mm",
					weekStart: 1,
					endDate: that.serverDate,
					autoclose: true,
					minViewMode: 1,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				});
				var $timeSpan = $("input[name=time-span]");
				$.each($timeSpan, function(i, item) {
					$(item).selected = false;
				});
				$timeSpan.click(function() {
					var type = $(this).val();
					$(".fast-select a").removeClass("current");
					switch (type) {						
						case "2":
							var mm = $startDate.val().slice(0,4)+"-01";
							$startDate.val(mm);
							that.bindEvent.month.call(that);
							break;
						case "3":
							var yyyy = $startDate.val().slice(0,4);
							$startDate.val(yyyy);
							that.bindEvent.year.call(that);
							break;
					}
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
			/** 
			 * 初始化分类分项下拉框
			 * author: pl
			 * time: 2014-3-23 22:03:40
			 */
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
			}

		},



		/**
		 * 页面初始化查询
		 * Author: pl
		 * Time: 2014-3-24 20:43:09
		 */
		initQuery: function() {
			$("#multi-obj input[name='time-span']").attr("disabled", false).eq(0).click();
			$(".fast-select a").eq(0).click();
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
			param.StartTime = $startDate.val().length > 5 ? $startDate.val() + "-01" : $startDate.val() + "-01-01";		
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
			var ctype = 1;

			$.ajax({
				url: this.getAction().dataUrl,
				dataType: 'json',
				type: 'POST',
				data: {
					Inputs: param || {}
				},
				beforeSend: function() {
					$tiansu.common.loading('#chart-quota-gauge', 'show');
					//$tiansu.common.loading('#quota-info', 'show');
					$tiansu.common.loading('#chart-diff', 'show');
					$tiansu.common.loading('#chart-trend', 'show');
				},
				success: function(json) {
					
					
					if (json.ActionInfo.Success) {
					
						//定额值
						var QuotaValue = json.Pie.QuotaValue;
						//实际消耗
						var ActualValue = json.Pie.ActualValue;
						//剩余值
						var OverPlusValue = json.Pie.OverPlusValue;
						//单位
						var Unit = json.Unit;
						//使用百分比
						var ActualPercent = parseFloat(((ActualValue/QuotaValue)*100).toFixed(2));
						//剩余百分比
						var OverPlusPercent = $tiansu.common.changeTwoDecimal_f(parseFloat(json.Pie.OverPlusPercent));

						/*if (OverPlusPercent.indexOf("-") == 0) {
							OverPlusPercent = (OverPlusPercent < -9999.99) ? (-9999.99+"%") : (OverPlusPercent+"%");
						} else {
							OverPlusPercent = (OverPlusPercent > 99999.99) ? (99999.99+"%") : (OverPlusPercent+"%");
						}*/
                        if (!isNaN(OverPlusPercent) && OverPlusPercent < 0) {
                            OverPlusPercent = (OverPlusPercent < -9999.99) ? (-9999.99+"%") : (OverPlusPercent+"%");
                        } else if(!isNaN(OverPlusPercent) && OverPlusPercent >= 0){
                            OverPlusPercent = (OverPlusPercent > 99999.99) ? (99999.99+"%") : (OverPlusPercent+"%");
                        }

						//历史节能率
						var LastYearSavingPercent = $tiansu.common.changeTwoDecimal_f(parseFloat(json.Pie.LastYearSavingPercent));
						/*if (LastYearSavingPercent.indexOf("-") == 0) {
							LastYearSavingPercent = (LastYearSavingPercent < -9999.99) ? (-9999.99+"%") : (LastYearSavingPercent+"%");
						} else {
							LastYearSavingPercent = (LastYearSavingPercent > 99999.99) ? (99999.99+"%") : (LastYearSavingPercent+"%");
						}*/
                        if(!isNaN(LastYearSavingPercent) && LastYearSavingPercent < 0){
                            LastYearSavingPercent = (LastYearSavingPercent < -9999.99) ? (-9999.99+"%") : (LastYearSavingPercent+"%");
                        } else if(!isNaN(LastYearSavingPercent) && LastYearSavingPercent >= 0){
                            LastYearSavingPercent = (LastYearSavingPercent > 99999.99) ? (99999.99+"%") : (LastYearSavingPercent+"%");
                        }
						//预测节能率
						var ForecastSavingPercent = $tiansu.common.changeTwoDecimal_f(parseFloat(json.Pie.ForecastSavingPercent));
						/*if (ForecastSavingPercent.indexOf("-") == 0) {
							ForecastSavingPercent = (ForecastSavingPercent < -9999.99) ? (-9999.99+"%") : (ForecastSavingPercent+"%");
						} else {
							ForecastSavingPercent = (ForecastSavingPercent > 99999.99) ? (99999.99+"%") : (ForecastSavingPercent+"%");
						}*/
                        if(!isNaN(ForecastSavingPercent) && ForecastSavingPercent < 0){
                            ForecastSavingPercent = (ForecastSavingPercent < -9999.99) ? (-9999.99+"%") : (ForecastSavingPercent+"%");
                        } else if(!isNaN(ForecastSavingPercent) && ForecastSavingPercent >= 0){
                            ForecastSavingPercent = (ForecastSavingPercent > 99999.99) ? (99999.99+"%") : (ForecastSavingPercent+"%");
                        }
						//显示在仪表盘上的百分比，最大不超过180
						var VisualPercent = (ActualPercent > 180) ? 180 : ActualPercent;
						//拼接charts
						var gaugeData = {
							series:[
								{
									data:[VisualPercent]
								}
							],
							ActualPercent:ActualPercent,
							ActualValue:ActualValue
						};
						var chartGauge = that.renderGauge("chart-quota-gauge", gaugeData);
						
						//填充右侧上部分的值
						that.fillQuotaInfo(QuotaValue, ActualValue, OverPlusValue, Unit);
						//填充各百分比
						that.fillPercentVal(OverPlusPercent, LastYearSavingPercent, ForecastSavingPercent);
						
						
						/* 差额分析开始 */
						var diffData = json.BalanceHighChart;
						//objParam.particle = 2;
						//objParam.StartTime = "2014-03-01";
						
						var from = new Date(objParam.StartTime.replace(/-/g, "/"));
                        var year = from.getFullYear();
						if(objParam.Particle == 2){
							//var year = from.getFullYear();
							var nextMonth = from.getMonth() + 1;
							if(nextMonth < 12){
								var toNextDay = new Date(year, nextMonth ,1);
								var toNextDayVal = toNextDay.valueOf();
								var toVal = toNextDayVal - (24*60*60*1000);
								var to = new Date(toVal);
							}else{
								var to = new Date(year, 11, 31);
							}
						}else if(objParam.Particle == 3){
                           // var year = from.getFullYear();
							var to = new Date(year, 11, 31);
						}
						var chartDiff = that.renderDiffCharts("chart-diff", diffData, from, to);
						/* 差额分析结束 */
						
						/* 趋势分析开始 */
						var trendData = json.TrendHighChart;
						var chartTrend = that.renderTrendCharts("chart-trend", trendData, from, to);
						/*趋势分析结束*/
						
						//填充单位
						$("#chartsDiff span.unit>span").html(Unit);
						$("#chartsTrend span.unit>span").html(Unit);
						
						//填充能耗分类和时间和面包屑
						var info = that.getChartInfo(objParam);
						$("#chartsQuota h3>.itemName").html(info.itemName);
						$("#chartsQuota h3>.series-date").html(info.dateString);
						
						var treeTypeVal = objParam.ObjType;
						var treeElem = "#tree-content";
						if(treeTypeVal == 1){
							treeElem = "#areaTree-content";
						}else{
							treeElem = "#tree-content";
						}
						var breadcrumb = that.getTreeNodePath(treeElem);
						$("#chartsQuota h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
					
					} else {
						$tiansu.common.info('show', {
							timeout: 1000,
							content: json.ActionInfo.ExceptionMsg
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
					$tiansu.common.loading('#main', 'hide');
				}
			});
		},


		/** 
		 * 渲染highcharts图表
		 * @param  {obj} json [列表json数据]
		 * author: HF
		 * time: 2014-3-24 18:00:21
		 */
		
		renderGauge: function(container, chartData) {

			charts.resetTimezone();
			return appDinge.renderTo(container, chartData);
			
		},
		
		/** 
		 * 填充月定额值、实际消耗值、剩余值
		 * author: HF
		 * time: 2014-4-14 17:15:38
		 */
		fillQuotaInfo: function(QuotaValue, ActualValue, OverPlusValue, Unit){
			//填充月定额值
			$("#QuotaValue").html(QuotaValue + Unit);
			//实际消耗值
			$("#ActualValue").html(ActualValue + Unit);
			//剩余值
			$("#OverPlusValue").html(OverPlusValue + Unit);
		},
		
		/** 
		 * 填充剩余百分比、历史节能率、预测节能率
		 * author: HF
		 * time: 2014-4-14 17:29:14
		 */
		fillPercentVal: function(OverPlusPercent, LastYearSavingPercent, ForecastSavingPercent){
			//剩余百分比
			$("#OverPlusPercent").html(OverPlusPercent);
			//历史节能率
			$("#LastYearSavingPercent").html(LastYearSavingPercent);
			//预测节能率
			$("#ForecastSavingPercent").html(ForecastSavingPercent);
		},
		
		/** 
		 * 渲染highcharts差额分析图表
		 * author: HF
		 * time: 2014-4-15 08:51:02
		 */
		renderDiffCharts: function(container, chartData, from, to){
			charts.resetTimezone();
			return appDiff.renderTo(container, chartData, from, to);
		},
		
		/** 
		 * 渲染highcharts趋势分析图表
		 * author: HF
		 * time: 2014-4-15 10:56:47
		 */
		renderTrendCharts: function(container, chartData, from, to){
			charts.resetTimezone();
			return appTrend.renderTo(container, chartData, from, to);
		},

		getChartInfo: function(param) {
			//var $startDate = $('input[name=startDate]'),
			//	$endDate = $('input[name=endDate]');
			//var startDate = $startDate.val();
			//var endDate = $endDate.val();
			var startDate = new Date(param.StartTime.replace(/-/g, "/"));
			var year = startDate.getFullYear();
			var month = startDate.getMonth() + 1;
			var particle = param.Particle;
			if(particle == 2){
				var dateString = "(" + year + "年" + month + "月)";
			}else if(particle == 3){
				var dateString = "(" + year + "年)";
			}else{
				var dateString = "";
			}
			
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
		 * Ajax request
		 * author: ghj
		 * time: 2013-12-15 15:26:10
		 */
		getAction: function() {
			return {
				tree: 'action.ashx?action=objectItemTree',
			    select: 'action.ashx?action=indexItem',
			    dataUrl: 'action.ashx?action=GetQuotaAnalyseChart'
			};
		}
	};

	return Quota;
});