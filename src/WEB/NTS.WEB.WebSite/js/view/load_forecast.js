/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗对比/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appLoadForecast'], function(charts, appLoadForecast) {

	function Loadforecast() {
		this.serverDate = $("#sys-time").html();
		this.init();
	}

	Loadforecast.prototype = {

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
			this.bindEvent.fastSelectDate.call(this);
			if($tree.tree('getRoot')) {
                this.initQuery();
            }else{
                $tiansu.common.info('show', {
                    timeout: 4000,
                    content: "请联系管理员设置您的权限。"
                });
            }
			this.bindEvent.search.call(this);
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
			fastSelectDate: function(){
				var $startDate = $('input[name=startDate]'),
					$endDate = $('input[name=endDate]');
				var today = this.serverDate;
				var $fastSelect = $(".fast-select a");
				$fastSelect.click(function(){
					$(this).addClass("current").siblings().removeClass("current");
					var type = $(this).attr("data-type");
					if(type){
						$tiansu.date.datePeriod({
							type: type,
							connect: "-",
							date: today,
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
			 * 设置日期事件 
			 * author: ghj
			 * time: 2014-01-03 14:46:48 
			 */
			setDate: function() {
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
				var forecastStartDate = $tiansu.date.diff({
					date:this.serverDate,
					number:"-31",
					type:"day"
				});	
				var forecastEndDate = $tiansu.date.diff({
					date:this.serverDate,
					number:"62",
					type:"day"
				});
				$tiansu.date.datePeriod({
					type: "7",
					connect: "-",
					date: that.serverDate,
					sDate: $startDate,
					eDate: $endDate
				});
				$startDate.datepicker({
					format: "yyyy-mm-dd",
					weekStart: 1,
					autoclose: true,
					minViewMode: 0,
					serverDate: that.serverDate,
					startDate:forecastStartDate,
					endDate: forecastEndDate,
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
						endDate: forecastEndDate,
						todayBtn: "linked",
						todayHighlight: false,
						keyboardNavigation: false
					});
				});
				$endDate.datepicker({
					format: "yyyy-mm-dd",
					weekStart: 1,
					startDate:$startDate.val(),
					endDate: forecastEndDate,
					serverDate: that.serverDate,
					autoclose: true,
					minViewMode: 0,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				}).on("changeDate",function(){
					$(".fast-select a").removeClass("current");
				});				
			},

			/** 
			 * 初始化选中的树节点
			 * author: pl
			 * time: 2014-3-24 20:17:36
			 */
			initTreeSelected: function() {
				$("#tree-content ul").eq(0).children("li").children(".tree-node").eq(0).addClass("tree-node-selected");
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
			//this.ajaxLoadCharts(param);
			//this.ajaxRenderList(param);
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
			var unit = $(".fast-select a.current").attr("data-type")-4;
			DateUnit = unit || 0;
			var nodeSelected = $tree.tree('getNode', $treeNode);
			if (nodeSelected != null) {
				param.ObjectId = nodeSelected.id;
			}
			//计算时间颗粒度
			param.particle = $tiansu.common.getParticle($startDate.val(),$endDate.val());
			param.StartTime = $startDate.val();
			param.EndTime = $endDate.val();
			param.ObjType = objType;
			param.ItemCode = itemCode ? itemCode : "00000";
			param.DateUnit = DateUnit;
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
					$tiansu.common.loading('#chart-load', 'show');
					$tiansu.common.loading('#chart-load-tbl .scroll-tbody', 'show');
				},
				success: function(json) {
					
					
					if (json.ActionInfo.Success) {
					
						//图表部分
						var chartData = json;
						var from = new Date(objParam.StartTime.replace(/-/g, "/"));
						var to = new Date(objParam.EndTime.replace(/-/g, "/"));
						that.renderLoad("chart-load", chartData, from, to);
						
						//填充单位
						var Unit = json.Unit;
						$("#chartsLoad span.unit>span").html(Unit);
						
						//填充能耗分类和时间和面包屑
						var info = that.getChartInfo(objParam);
						$("#chartsLoad h3>.itemName").html(info.itemName);
						$("#chartsLoad h3>.series-date").html(info.dateString);
						
						var treeTypeVal = objParam.ObjType;
						var treeElem = "#tree-content";
						if(treeTypeVal == 1){
							treeElem = "#areaTree-content";
						}else{
							treeElem = "#tree-content";
						}
						var breadcrumb = that.getTreeNodePath(treeElem);
						$("#chartsLoad h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
						
						//列表部分
						that.renderList(json);
						
						//总值部分
						var forecastVal = json.ForeCastTotal;
						var historyVal = json.HistoryTotal;
						var Unit = json.Unit;
						that.renderTotalVal(forecastVal, historyVal, Unit);
					
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
		 * ajax负荷预测图
		 * author: hf
		 * time: 2014-4-15 19:57:23
		 */
		/*因接口合并已作废
		ajaxLoadCharts: function(param) {
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
					$tiansu.common.loading('#chart-load', 'show');
				},
				success: function(json) {
					
					
					if (json.ActionInfo.Success) {
					
						var chartData = json;
						var from = new Date(objParam.StartTime.replace(/-/g, "/"));
						var to = new Date(objParam.EndTime.replace(/-/g, "/"));
						that.renderLoad("chart-load", chartData, from, to);
						
						//填充单位
						var Unit = json.Unit;
						$("#chartsLoad span.unit>span").html(Unit);
						
						//填充能耗分类和时间和面包屑
						var info = that.getChartInfo(objParam);
						$("#chartsLoad h3>.itemName").html(info.itemName);
						$("#chartsLoad h3>.series-date").html(info.dateString);
						
						var treeTypeVal = objParam.ObjType;
						var treeElem = "#tree-content";
						if(treeTypeVal == 1){
							treeElem = "#areaTree-content";
						}else{
							treeElem = "#tree-content";
						}
						var breadcrumb = that.getTreeNodePath(treeElem);
						$("#chartsLoad h3 span.areaName").html(breadcrumb);
					
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
*/

		/** 
		 * 渲染highcharts图表
		 * @param  {obj} json [列表json数据]
		 * author: HF
		 * time: 2014-3-24 18:00:21
		 */
		
		renderLoad: function(container, chartData, from, to) {

			charts.resetTimezone();
			return appLoadForecast.renderTo(container, chartData, from, to);
			
		},
		
		

		getChartInfo: function(){
			var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');
			var startDate = $startDate.val();
			var endDate = $endDate.val();
			var dateString = "(" + startDate + " — " + endDate + ")";
			var itemName = ""; 
			
			//根据树的显示隐藏获取对应参数
			if($("#tree-content").is(":visible")){
				itemName = $("#area-select").children().eq(0).find("select[name=type-diff]").find("option:selected").text();
                if(itemName === "总能耗"){
                    itemName = "总";
                }
			}else{
				itemName =  $("#area-select").children().eq(1).find("select[name=item-diff]").find("option:selected").text() || $("#area-select").children().eq(1).find("select[name=type-diff]").find("option:selected").text();
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
		 * 渲染列表
		 * @param  {obj} json [列表json数据]
		 * author: HF
		 * time: 2014-4-16 09:47:55
		 */
		 /*因接口合并已作废
		ajaxRenderList: function(param){
			var that = this;
			var objParam = param;
			var param = JSON.stringify(param);
			var ctype = 1;

			$.ajax({
				url: this.getAction().tblUrl,
				dataType: 'json',
				type: 'POST',
				data: {
					Inputs: param || {}
				},
				beforeSend: function() {
					$tiansu.common.loading('#chart-load-tbl .scroll-tbody', 'show');
				},
				success: function(json) {
					
					
					if (json.ActionInfo.Success) {
					
						that.renderList(json);
					
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
		*/
		renderList: function(json) {
			var $tbody = $('#chart-load-tbl tbody');
			var items = template('list-tpl', json);
			// 列表
			$tbody.html(items);
		},
		
		/*预测能耗总值与历史能耗总值*/
		renderTotalVal: function(forecastVal, historyVal, Unit){
			//填充数值
			$("#forecastVal>.val").html(forecastVal + Unit);
			$("#historyTotalVal>.val").html(historyVal + Unit);
			if(forecastVal>historyVal){
				var forecastWidth = "100%";
				var historyWidth = parseInt((historyVal/forecastVal)*100) + "%";
			}else{
				var forecastWidth = parseInt((forecastVal/historyVal)*100) + "%";
				var historyWidth = "100%";
			}
			$("#historyVal").stop(true,true).animate({width:historyWidth},"slow", function(){
				$(this).children(".icon2").show();
			});
			$("#forecastVal").stop(true,true).animate({width:forecastWidth},"slow", function(){
				$(this).children(".col").show();
			});
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
				select:'action.ashx?action=indexItem',
				dataUrl: 'action.ashx?action=GetLoadForecastChart',
				exportUrl: 'action.ashx?action=ExportLoadForecast'
				//tblUrl: '/js/data/load_forecast_tbldata.js'
			};
		}
	};

	return Loadforecast;
});