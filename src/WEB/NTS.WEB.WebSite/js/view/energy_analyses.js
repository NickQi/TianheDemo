/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗排名/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appFenxi', 'chartsmod/appFenjie'],function(charts, appFenxi, appFenjie) {

	function Analyses() {
		this.serverDate = $("#serverDate").val();
		this.isQuering = false;
		
		this.chart1 = null;
		this.addedSeries = null;
		
		//恢复默认时所用到的变量副本 开始
		this.chartDataDefault = null;
		this.tblDataDefault = null;
		this.from = null;
		this.to = null;
		this.itemNameStringDefault = "";
		//恢复默认时所用到的变量副本 结束
		this.chartJson = null;
		
		this.init();
	}

	Analyses.prototype = {

		/**
		 * Initialize page
		 * author: ghj
		 * time: 2013-12-29 20:28:47
		 */
		init: function() {
			$("#NHfenxi-slc").select2({minimumResultsForSearch: -1});
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
			var $tree = this.getTree(2,$("select[name=type-diff]").eq(0).val());						
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
			this.bindEvent.slcChartTbl.call(this);
			this.bindEvent.changeChartType.call(this);
			this.bindEvent.slidePie.call(this);
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
						if(index == 1){
							var $tree = $('#areaTree-content ul:first-child');
							if($tree.html() == ""){
								that.getTree(1,$("select[name=type-diff]").eq(1).val());
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
			 initSelect: function(){
			 	var that = this;
			 	var $typeDiff = $("select[name=type-diff]"),
			 	$itemDiff = $("select[name=item-diff]");
			 	var param = {"ItemCode":"00000"};
			 	param = JSON.stringify(param);
			 	$.ajax({
					url: that.getAction().select,
					dataType: "json",
					type: 'POST',
					data: {Inputs:param || {}},
					async: false,
					success: function(json){
						if(json.ItemLst && json.ItemLst.length > 0){
							$.each(json.ItemLst,function(i,item){
								$typeDiff.append('<option value='+item.ItemCode+'>'+item.ItemName+'</option>');
							});	
							$typeDiff.append('<option value="00000">总能耗</option>');
							that.bindEvent.secondSelect.call(that,"select[name=type-diff]",1);
						}					
					},
					error: function( error){
						console.log(error);
					}
				});
			 	$("#area-select select").select2({
					placeholder: '----------',
					minimumResultsForSearch: -1,
					formatNoMatches: function () {
					 return "没有数据！"; 
					}
				}).on("change",function(){
					that.bindEvent.secondSelect.call(that,this);		
				});
			 },

			 secondSelect:function(select,times){
			 		var that = this;
			 	//$item 是级联下拉的第一级下拉
					var $item = $(select).siblings("select[name=item-diff]");
					if($item.length != 0){
						$("option",$item).remove();
						var param = {"ItemCode":$(select).val()};
			 			param = JSON.stringify(param);
						$.ajax({
							url: that.getAction().select,
							dataType: "json",
							type:"POST",
							data: {Inputs:param || {}},
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
			},

			/** 
			 * 初始化选中的树节点 
			 * author: pl
			 * time: 2014-3-24 20:17:36
			 */
			 initTreeSelected: function(){
			 	var $node = $("#tree-content ul li").eq(0).children(".tree-node");
			 		$node.addClass("tree-node-selected");
			 },
			
			/**
			* 点击查询按钮触发查询事件
			* Author: pl
			* Time: 2014-3-24 09:48:58
			*/
			search: function(){
				var that = this;
				var param = that.getParam();
				var treeNode = param.ObjectId;
				var queryTypes = {
					danwei: 2,
					renjun: 3,
					biaozhunmei:6,
					tanpfl:7,
					rmb:8
				};			
				$("#queryBtn a").click(function(e){
					$(".fn a, .fn2 a, .fn3 a").removeClass("current");
					if(that.isQuering == true){
						return false;
					}
						
					//显示同比环比功能
					//$("#tool-bar .fn2").show();
					//$("#tool-bar .fn").show();
					//回到能耗分解饼图
					$("#AnalyDeviceBlock").hide();
					$("#chartsAnalyPie").show();
					param = that.getParam();
					treeNode = param.ObjectId;
					var flag = that.checkParam(param);
					if(flag){
						
						//chart info:
						var info = that.getChartInfo();
						that.itemNameStringDefault = info.itemName;
						$("#chartsAnalyses h3 span.itemName").html(info.itemName);
						$("#chartsAnalyses h3 span.series-date").html(info.dateString);
						var treeTypeVal = param.ObjType;
						var treeElem = "#tree-content";
						if(treeTypeVal == 1){
							treeElem = "#areaTree-content";
						}else{
							treeElem = "#tree-content";
						}
						var breadcrumb = that.getTreeNodePath(treeElem);
						$("#chartsAnalyses h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
						
						//释放无用的同比环比series引用
						that.addedSeries = null;
						//modify by lyy==start
						var check = that.checkFirstDay(param);
						if (check) {
							that.ajaxRenderCharts(param, true);
							that.ajaxRenderPie(param);							
							//获得总能耗信息（最大最小平均等）
							that.getEnergyInfo(param);
						} else {
							that.handleFirstDay.call(this);
						}
						//modify by lyy==end		
					}else{
						return false;
					}
				});
				
				$(".fn a, .fn3 a").click(function(e){
					e.preventDefault();
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						$(".fn a, .fn2 a, .fn3 a").removeClass("current");
						$(this).addClass("current");
						var typesub = $(e.target).attr("id");
						var typeId = queryTypes[typesub];
						param.QueryType = typeId;
						
						//隐藏同比环比功能
						$("#tool-bar .fn2").hide();
						
						//释放无用的同比环比series引用
						that.addedSeries = null;
						//modify by lyy==start
						var check = that.checkFirstDay(param);
						if (check) {
							that.ajaxRenderCharts(param);
							//that.ajaxRenderPie(param);						
							//获得总能耗信息（最大最小平均等）
							that.getEnergyInfo(param);
						} else {
							that.handleFirstDay.call(this);
						}
						//modify by lyy==end
					}else{
						return false;
					}		
				});

				//恢复默认
				$("#default").click(function(e){
					$(".fn a, .fn2 a").removeClass("current");
					e.preventDefault();
					//显示同比环比功能
					$("#tool-bar .fn2").show();
					$("#tool-bar .fn").show();
					$("#tool-bar .fn3").show();
					$("#AnalyDeviceBlock").hide();
					$("#chartsAnalyPie").show();
					
					param.IsDevice = 0;
					param.QueryType = 1;
					param.ObjectId = treeNode;
					//释放无用的同比环比series引用
					that.addedSeries = null;
					//modify by lyy==start
					var check = that.checkFirstDay(param);
					if (check) {
						that.chartJson = that.chartDataDefault;
						that.renderCharts(that.chartDataDefault, that.from, that.to);
						//获得总能耗信息（最大最小平均等）
						that.getEnergyInfo(param);
						$("#chartsAnalyses h3 span.itemName").html(that.itemNameStringDefault);
					} else {
						that.handleFirstDay.call(this);
					}
					//modify by lyy==end
				});
				
				//增加同比值
				$("#tongbi").click(function(e){
					e.preventDefault();
					$(".fn a, .fn2 a").removeClass("current");
					$(this).addClass("current");
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						param.QueryType = 4;
						//modify by lyy==start
						var check = that.checkFirstDay(param);
						if (check) {
							var json = that.search(param);						
							if(json.ActionInfo.Success){						
								//先清除掉已添加的series，如果有
								if(that.addedSeries){
									that.addedSeries.remove();
								}
								that.addedSeries = appFenxi.addTBSeries(that.chart1, json);
							}else{
								$tiansu.common.info('show', {
									timeout: 1000,
									content: json.ActionInfo.ExceptionMsg
								});
							}
						} else {
							that.handleFirstDay.call(this);
						}
						//modify by lyy==end
					}else{
						return false;
					}		
					
				});
			
				//增加环比值
				$("#huanbi").click(function(e){
					e.preventDefault();
					$(".fn a, .fn2 a").removeClass("current");
					$(this).addClass("current");
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						param.QueryType = 5;
						//modify by lyy==start
						var check = that.checkFirstDay(param);
						if (check) {
							var json = that.search(param);						
							if(json.ActionInfo.Success){						
								//先清除掉已添加的series，如果有
								if(that.addedSeries){
									that.addedSeries.remove();
								}
								that.addedSeries = appFenxi.addHBSeries(that.chart1, json);
							}else{
								$tiansu.common.info('show', {
									timeout: 1000,
									content: json.ActionInfo.ExceptionMsg
								});
							}							
						} else {
							that.handleFirstDay.call(this);
						}
						//modify by lyy==end
					}else{
						return false;
					}	
				});

				//导出按钮
				$("#export-link").click(function() {
					if($(this).hasClass("export-disabled")){
						return false;
					}else{
						var flag = that.checkParam(param);
						//var $a = $(".fn a.current");
						var $a = $(".fn a.current, .fn3 a.current");					
						if (flag) {
							//modify by lyy==start
							var check = that.checkFirstDay(param);
							if (check) {
								if($a.length>0){
									$.each($a, function(i, item) {
										var typesub = $(item).attr("id");
										var typeId = queryTypes[typesub];
										param.QueryType = typeId;
									});
								}else{
									param.QueryType = 1;
								}
								that.exportList(param);
							} else {
								that.handleFirstDay.call(this);
							}
							//modify by lyy==end
						} else {
							return false;
						}
					}
				});
				
				//点击“设备能耗”，加载设备列表及第一项设备信息
				$("#device-btn").click(function(e){
					e.preventDefault();
					param.ObjectId = treeNode;
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						//modify by lyy==start
						var check = that.checkFirstDay(param);
						if (check) {
							//param.QueryType = 5;
							//IsDetail: 0-不需要详细信息，1-需要详细信息
							param.IsDetail = 0;												
							var callback = function(json){													
								$("#chartsAnalyses .itemName").html($("#ana-deviceList li.current a").text());
								if(json.DeviceUnitList.length>0){
									//切出设备信息
									$("#AnalyDeviceBlock").show();
									$("#chartsAnalyPie").hide();
									$("#tool-bar .fn").hide();
									$("#tool-bar .fn3").hide();
									$("#tool-bar .fn2").show();
									$(".fn a, .fn2 a, .fn3 a").removeClass("current");
									var DeviceID = $("#ana-deviceList li.current a").attr("data-deviceid");
									param.ObjectId = DeviceID;
									param.IsDevice = 1;	
									param.QueryType = 1;
									//加载第一项信息
									that.getDeviceInfo(param);
									//总能耗模块数据更新
									that.getEnergyInfo(param);
									//释放无用的同比环比series引用
									that.addedSeries = null;
									//显示该设备的柱状图
									that.ajaxRenderCharts(param);
								}else{
									$tiansu.common.info('show', {
										container: '#chart-analyses-pies',
										centerX: false,
										centerY: false,
										css: {
											top: $("#device-btn").offset().top+30,
											left: $("#device-btn").offset().left-200,
											position: 'absolute'
										},
										content: "该区域下没有设备",
										timeout: 2000
									});
								}							
							};
							that.getDeviceList(param, callback);		
						} else {
							that.handleFirstDay.call(this);
						}
						//modify by lyy==end	
					}else{
						return false;
					}	
				});
				
				//点击设备列表的名称，加载设备信息
				$(document).on("click", "#ana-deviceList>ul>li>a", function(e){
					e.preventDefault();
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						
						//改变当前样式
						$(this).parent("li").addClass("current").siblings().removeClass("current");
						
						//param.QueryType = 5;
						//设备ID
						var DeviceID = $(this).attr("data-DeviceID");
						param.ObjectId = DeviceID;
						param.IsDevice = 1;
						that.getDeviceInfo(param);
						//总能耗模块数据更新
						that.getEnergyInfo(param);
						//释放无用的同比环比series引用
						that.addedSeries = null;
						//显示该设备的柱状图
						that.ajaxRenderCharts(param);
						$("#chartsAnalyses .itemName").html($(this).text());
					}else{
						return false;
					}	
				});
				
				//点击切回饼图
				$("#toPie-btn").click(function(e){
					e.preventDefault();
					//$("#AnalyDeviceBlock").hide();
					//$("#chartsAnalyPie").show();
					//相当于恢复默认
					$("#default").click();
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
			 slcChartTbl: function(){
				var _this = this;	 
				var $tbl = $("#table-analyses");
				var $chart = $("#chart-analyses");
			 	$("#slcChartTbl>a").click(function(e){
					e.preventDefault();
					//modify by lyy==start
					var param = _this.getParam();
					var check = _this.checkFirstDay(param);
					if (check) {						
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
								_this.renderCharts(_this.chartJson, _this.from, _this.to);	
							});
							
							$("#export-link").removeClass().addClass("export-disabled");
						}
					} else {
						_this.handleFirstDay.call(this);
					}
					//modify by lyy==end
				});	
			 },
			 
			 changeChartType: function(){
				var _this = this;
			 	$("#NHfenxi-slc").val("1");
				$("#NHfenxi-slc").change(function(){
					//modify by lyy==start
					var param = _this.getParam();
					var check = _this.checkFirstDay(param);
					if (check) {
						var type = $(this).val();
						if(type == 0){
							charts.changeType(_this.chart1, "line", 0);
						}else{
							charts.changeType(_this.chart1, "column", 0);
						}
					} else {
						_this.handleFirstDay.call(this);
					}
					//modify by lyy==end
				});
			 },
			 
			 slidePie: function(){
			 	$tiansu.common.responseScroll("#chart-analyses-pies", "#chart-prev", "#chart-next", 1);
			 }
			 
			 
		},

		/**
			* 页面初始化查询
			* Author: pl
			* Time: 2014-3-24 20:43:09
			*/
			initQuery: function(){
				var _this = this;
				var param = this.getParam();
				
				//chart info:
				var info = this.getChartInfo();
				
				this.itemNameStringDefault = info.itemName;
				
				$("#chartsAnalyses h3 span.itemName").html(info.itemName);
				$("#chartsAnalyses h3 span.series-date").html(info.dateString);
				
				var breadcrumb = this.getTreeNodePath("#tree-content");
				$("#chartsAnalyses h3 span.areaName").html(breadcrumb);
				
				//modify by lyy==start
				var check = _this.checkFirstDay(param);
				if (check) {
					this.ajaxRenderCharts(param, true);
					this.ajaxRenderPie(param);					
					//获得总能耗信息（最大最小平均等）
					this.getEnergyInfo(param);	
				} else {
					that.handleFirstDay.call(this);
				}
				//modify by lyy==end
			},

		/**
		 * 设置目录树
		 * author: ghj
		 * time: 2014-01-17 00:51:14
		 */
		getTree: function(type,itemCode) {
			var that = this;
			var $tree = null;
			var $treeContent = null;
			var is_single = true;
			if(type === 2){
				$tree = $('#tree-content ul:first-child');
				$treeContent = $('#tree-content');
				is_single = true;
			}else if(type === 1){
				$tree = $('#areaTree-content ul:first-child');
				$treeContent = $('#areaTree-content');
				is_single = true;
			}
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
					$tiansu.common.loading('#tree-container','hide');
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
		 getParam: function(){
		 	var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');
			var secondItemCode = $("#area-select").children().eq(1).find("select[name=item-diff]").val();
			//根据树的显示隐藏获取对应参数
			if($("#tree-content").is(":visible")){
				var $tree = $("#tree-content ul"),
				    nodeSelected = $tree.tree('getNode',"#tree-content .tree-node-selected"),
				    objType = 2,
				    itemCode = $("#area-select").children().eq(0).find("select[name=type-diff]").val();
			}else{
				var $tree = $("#areaTree-content ul"),
					nodeSelected = $tree.tree('getNode',"#areaTree-content .tree-node-selected"),
					objType = 1,
					itemCode =  secondItemCode != "" ? secondItemCode : $("#area-select").children().eq(1).find("select[name=type-diff]").val();
			}
			var param = {},
				children = [];
			//计算时间颗粒度
			param.particle = $tiansu.common.getParticle($startDate.val(),$endDate.val());
			param.StartTime = $startDate.val();
			param.EndTime = $endDate.val();
			if(nodeSelected){
				param.ObjectId = nodeSelected.id;				
				var nodeChildren = nodeSelected.children;
				if(nodeChildren.length>0){
					$.each(nodeChildren,function(i,item){
						children.push(item.id);
					});
				}
			}
			param.ObjectChildren = children;
			param.ObjType = objType;
			param.ItemCode = itemCode?itemCode:"00000";
			param.QueryType = 1;
			param.IsDevice = 0;
			param.IsDetail = 0;
			return param;
		 },

		/**
		* 参数验证
		* Author: pl
		* Time: 2014-3-25 21:46:30
		*/
		checkParam: function(param){
			if(!param.ObjectId){
				$tiansu.common.info("show",{
					content:"请选择1个对象！",
					css:{
						centerX: false,
						centerY: false,
						top:'40%',
						left:'150px'
					},
					timeout:2000
				});
				return false;
			}else{
				return true;
			}
		},

		/** 
		 * 验证当天是否为每月的第一天
		 * true: 不是每月的第一天；false: 是每月的第一天
		 * author: luyy
		 * time: 2014-05-15 14:36:29
		 */		
		checkFirstDay: function(param){
			var fastDate = $(".fast-select a.current").attr("data-type");
			if(param.particle == 0 && (fastDate == 2 || fastDate == 3)){				
				return false;
			}else{
				return true;
			}
		},		
		/** 
		 * 每月第一天处理
		 * author: luyy
		 * time: 2014-05-15 15:25:18
		 */
		handleFirstDay: function(){
			$("#chart-analyses").empty();
			$("#table-analyses .text-center").empty();
			$("#chart-pie1").empty();
			$("#chart-pie2").empty();
			//$("#lastYearVal i").removeClass("down,up");
            $("#lastYearVal i").removeClass("down").removeClass("up");
			$("#lastYearVal span").html("--%");
			//$("#lastMonVal i").removeClass("down,up");
            $("#lastMonVal i").removeClass("down").removeClass("up");
			$("#lastMonVal span").html("--%");
			$(".info-values .num").empty();
			$(".info-values .unit").html("--");
			$tiansu.common.info('show', {
				timeout: 1000,
				content: '暂无数据信息'
			});
		},
		
		 /**
		 * 查询数据
		 * author: pl
		 * time: 2014-3-24 09:41:13
		 */
		 search: function(param){
		 	//console.log(param);
		 	var that = this;
			//var result = null;
			var param = JSON.stringify(param);
		 	$.ajax({
				url: this.getAction().chartDataUrl,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				async: false,
				beforeSend: function(){
					that.isQuering = true;
					$tiansu.common.loading('#chart-analyses','show');
				},
				success: function(json) {
					result = json;
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败！'
					});
				},
				complete: function(){
					$tiansu.common.loading('#main','hide');
					that.isQuering = false;
				}
			});
			return result;
		 },
		 
		 /** 
		 * 获取数据并渲染分析柱状图表
		 * author: HF
		 * time: 2014-3-28 15:20:20
		 */
		 ajaxRenderCharts: function(param, cache){
		 	var _this = this;
			var objParam = param;
			var param = JSON.stringify(param);
			$.ajax({
				url:this.getAction().chartDataUrl,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				beforeSend: function(){
					_this.isQuering = true;
					$tiansu.common.loading('#chart-analyses','show');
				},
				success: function(json) {
					
					if(json.ActionInfo.Success){
						/*如果是当日、本周或本月，则用0填充剩下的时间数据 开始*/
						var seriesData = json.series[0].data;
						var seriesDataLength = seriesData.length;
						var dataTimesTxt = $(".fast-select a.current").text();
						switch(dataTimesTxt){
							case "当日":
								var longLength = 24;
								break;
							case "本周":
								var longLength = 7;
								break;
							case "本月":
								//console.log(objParam);
								var monthStartDate = new Date(objParam.StartTime.replace(/-/g, "/"));
								var nowYear = monthStartDate.getFullYear();
								var myMonth = monthStartDate.getMonth();
								var theMonthDays = $tiansu.date.getMonthDays(nowYear,myMonth);
								var longLength = theMonthDays;
								break;
						}
						if(longLength){
							var diffLength = longLength - seriesDataLength;
							if(diffLength > 0){
								for(var i = 0; i < diffLength; i++){
									seriesData.push(0);
								}
							}
						}
						/*如果是当日、本周或本月，则用0填充剩下的时间数据 结束*/
						
						//是否缓存数据
						if(cache){
							_this.chartDataDefault = $.extend(true, {}, json);
							_this.tblDataDefault = $.extend(true, {}, json);
						}
						_this.chartJson = json;
						_this.from = new Date((objParam.StartTime).replace(/-/g, "/"));
						_this.to = new Date((objParam.EndTime).replace(/-/g, "/"));
						
						_this.renderCharts(json, _this.from, _this.to);
					}else{
						$tiansu.common.info('show', {
							timeout: 1000,
							content: json.ActionInfo.ExceptionMsg
						});
					}
					
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败'
					});
				},
				complete: function(){
					$tiansu.common.loading('#main','hide');
					_this.isQuering = false;
				}
			});
		 },
		 
		 /** 
		 * 渲染highcharts图表
		 * @param  {obj} json [列表json数据]
		 * author: HF
		 * time: 2014-3-24 18:00:21
		 */
		 renderCharts: function(data, from, to){
			 
			
			charts.resetTimezone();
			var seriesTypeVal = $("#NHfenxi-slc option:selected").val();
			var seriesType = "column";
			if(seriesTypeVal === "0"){
				seriesType = "line";
			}else{
				seriesType = "column";
			}
			appFenxi.setSeriesType(seriesType);
			this.chart1 = appFenxi.renderTo("chart-analyses", data, from, to);
			$("#chartsAnalyses .unit>span").text(data.Unit);
			//生成表格
			this.renderList(data);
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
		 

		/** 
		 * 渲染列表
		 * @param  {obj} json [列表json数据]
		 * author: PL & HF
		 * time: 2013-12-15 15:46:19
		 */
		renderList: function(json) {
			var $tbody = $('#table-analyses tbody');
			var items = template('list-tpl', json);
			// 列表
			$tbody.html(items);
		},
		
		/** 
		 * 获取数据并渲染饼图
		 * author: HF
		 * time: 2014-3-28 17:19:01
		 */
		ajaxRenderPie: function(param){
			var _this = this;
			var objParam = param;
			var param = JSON.stringify(param);
			$.ajax({
				url:this.getAction().pieDataUrl,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				beforeSend: function(){
					_this.isQuering = true;
					$tiansu.common.loading('#chart-analyses-pies','show');
				},
				success: function(json) {
					if(json.ActionInfo.Success){
					
						var layerPieData = json.LayerPie;
						var itemPieData = json.ItemCodePie;
						layerPieData.unit = json.Unit;
						layerPieData.titleText = "子区域汇总值";
						itemPieData.unit = json.Unit;
						var pie1 = appFenjie.renderTo("chart-pie1", layerPieData);
						var pie2 = appFenjie.renderTo("chart-pie2", itemPieData);
						
					}else{
						$tiansu.common.info('show', {
							timeout: 1000,
							content: json.ActionInfo.ExceptionMsg
						});
					}
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败'
					});
				},
				complete: function(){
					//$tiansu.common.loading('#main','hide');
					_this.isQuering = false;
				}
			});
		},
		
		/** 
		 * 获取总能耗信息
		 * author: HF
		 * time: 2014-3-28 22:11:32
		 */
		 getEnergyInfo: function(param){
		 	var _this = this;
			var objParam = param;
			var param = JSON.stringify(param);
			$.ajax({
				url:this.getAction().alanInfoUrl,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				beforeSend: function(){
					_this.isQuering = true;
					$tiansu.common.loading('#analyses-infos','show');
				},
				success: function(json) {
					//同比百分比
					var lastYearCompare = json.LastYearCompare;
					if(lastYearCompare < 0){
						$("#lastYearVal i").removeClass().addClass("down");
					}else{
						$("#lastYearVal i").removeClass().addClass("up");
					}
                    $("#lastYearVal i").attr("data-original-title", "去年同期：" + json.LastYearTotalValue + json.Unit).tooltip();
					$("#lastYearVal .num").html(Math.abs(lastYearCompare) + "%");
					//环比百分比
					var lastMonthCompare = json.LastMonthCompare;
					if(lastMonthCompare < 0){
						$("#lastMonVal i").removeClass().addClass("down");
					}else{
						$("#lastMonVal i").removeClass().addClass("up");
					}
                    $("#lastMonVal i").attr("data-original-title", "上月总能耗：" + json.LastMonthTotalValue + json.Unit).tooltip();
					$("#lastMonVal .num").html(Math.abs(lastMonthCompare) + "%");
					//最大值
					$("#maxVal .num").html(json.MaxValue);
					$("#maxVal .unit").html(json.Unit);
					//最小值
					$("#minVal .num").html(json.MinValue);
					$("#minVal .unit").html(json.Unit);
					//平均值
					$("#avgVal .num").html(json.AverageValue);
					$("#avgVal .unit").html(json.Unit);
					
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败'
					});
				},
				complete: function(){
					//$tiansu.common.loading('#main','hide');
					$tiansu.common.loading('#analyses-infos','hide');
					_this.isQuering = false;
				}
			});
		 },
		 
		 /** 
		 * 获取设备列表
		 * author: HF
		 * time: 2014-3-29 16:08:14
		 */
		 getDeviceList: function(param, callback){
		 	var _this = this;
			var objParam = param;
			var param = JSON.stringify(param);
			$.ajax({
				url:this.getAction().deviceListUrl,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				beforeSend: function(){
					_this.isQuering = true;
					$tiansu.common.loading('#ana-deviceList','show');
				},
				success: function(json) {
					
					if(json.ActionInfo.Success){
						var listArr = json.DeviceUnitList;
						var listArrLength = listArr.length;
						var listHtml = "<ul>";
						for(var i = 0; i < listArrLength; i++){
							if(i === 0){
								listHtml += "<li class='current'><a data-DeviceID='" + listArr[i].DeviceID + "' href=''>" + listArr[i].DeviceName + "</a></li>";
							}else{
								listHtml += "<li><a data-DeviceID='" + listArr[i].DeviceID + "' href=''>" + listArr[i].DeviceName + "</a></li>";
							}
						}
						listHtml += "</ul>";
						$("#ana-deviceList").html(listHtml);
						//设置上下滚动
						$tiansu.common.scrollUpDown("#ana-deviceList", "#ana-deviceList-prev", "#ana-deviceList-next", 8);
						
						callback(json);
					}else{
						$tiansu.common.info('show', {
							timeout: 1000,
							content: json.ActionInfo.ExceptionMsg
						});
					}
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败'
					});
				},
				complete: function(){
					$tiansu.common.loading('#ana-deviceList','hide');
					_this.isQuering = false;
				}
			});
		 },
		 
		 /** 
		 * 获取设备信息
		 * author: HF
		 * time: 2014-3-29 16:08:14
		 */
		 getDeviceInfo: function(param){
		 	var _this = this;
			var objParam = param;		
			var param = JSON.stringify(param);
			$.ajax({
				url:this.getAction().deviceInfoUrl,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				beforeSend: function(){
					_this.isQuering = true;
					$tiansu.common.loading('#ana-deviceInfo','show');
				},
				success: function(json) {
					if(json.ActionInfo.Success){
						var deviceInfo = json.Info;
						if(deviceInfo){
							//设备编号
							$("#Number>span").html(deviceInfo.Number);
							//设备型号
							$("#DevType>span").html(deviceInfo.DevType);
							//类别
							$("#Category>span").html(deviceInfo.Category);
							//性质
							$("#Nature>span").html(deviceInfo.Nature);
							//安装位置
							$("#Location>span").html(deviceInfo.Location);
							//额定功率
							$("#Rating>span").html(deviceInfo.Rating);
							//隶属机构
							$("#Affiliations>span").html(deviceInfo.Affiliations);
						}
					}else{
						$tiansu.common.info('show', {
							timeout: 1000,
							content: json.ActionInfo.ExceptionMsg
						});
					}
				},
				error: function() {
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '数据请求失败'
					});
				},
				complete: function(){
					$tiansu.common.loading('#ana-deviceInfo','hide');
					_this.isQuering = false;
				}
			});
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
					$tiansu.common.loading('#table-order', 'show');
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
					$tiansu.common.loading('#table-order', 'hide');
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
				chartDataUrl: 'action.ashx?action=GetEnergyAnalyseLineChart',
				pieDataUrl:'action.ashx?action=GetEnergyAnalysePie',
				alanInfoUrl: 'action.ashx?action=GetEnergyAnalyseCompare',
				deviceListUrl:  'action.ashx?action=indexDeviceList',
				deviceInfoUrl:  'action.ashx?action=realtime',
				exportUrl: 'action.ashx?action=ExportExcelEnergyAnalyse' 
			};
		}
	};

	return Analyses;
});