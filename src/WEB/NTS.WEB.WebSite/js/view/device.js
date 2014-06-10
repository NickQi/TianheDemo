/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗排名/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appQushi'],function(charts, appQushi) {

	function Device() {
		this.serverDate = $("#serverDate").val();
		this.isQuering = false;
		
		this.chart1 = null;
		this.addedSeries = null;
		
		this.init();
	}

	Device.prototype = {

		/**
		 * Initialize page
		 * author: ghj
		 * time: 2013-12-29 20:28:47
		 */
		init: function() {
			$("#NHfenxi-slc").find("option").eq(0).attr("selected","selected");
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
			this.bindEvent.deviceSlc();
			var $tree = this.getTree(2,$("select[name=type-diff]").eq(0).val());						
			this.bindEvent.initTreeSelected.call(this);
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
			
			deviceSlc: function(){
				var $chartOrder = $("#NHfenxi-slc");
				$chartOrder.change(function() {				
					//根据下拉选项，显示隐藏相关表单
					if($chartOrder.select2('data').id == "0"){
						$("#equiplistanalog").hide();
						$("#equiplistpulse").hide();
						$("#equiplistswitch").show();
					}else if($chartOrder.select2('data').id == "1"){
						$("#equiplistanalog").show();
						$("#equiplistpulse").hide();
						$("#equiplistswitch").hide();
					}else if($chartOrder.select2('data').id == "2"){
						$("#equiplistanalog").hide();
						$("#equiplistpulse").show();
						$("#equiplistswitch").hide();
					}
				});
			},

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
							//$typeDiff.append('<option value="00000">总能耗</option>');
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
					},
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
									},
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
				$("#queryBtn a").click(function(e){
					$(".fn a").removeClass("current");
					if(that.isQuering == true){
						return false;
					}
					param = that.getParam();
					var flag = that.checkParam(param);
					if(flag){
						
						//获取设别列表
						param.IsDetail = 1;
						var callback = function(json){
							if(json.DeviceUnitList.length>0){
								//加载第一项信息
								that.getDeviceInfo(param);
								//加载设备详细信息
								//that.equipValue(param);
								//显示该设备的走势图
								that.ajaxRenderCharts(param);
							}							
						};
						that.getDeviceList(param, callback);
						
					}else{
						return false;
					}
				});				

				//恢复默认
				$("#default").click(function(e){
					$(".fn a").removeClass("current");
					e.preventDefault();					
					//先清除掉已添加的series，如果有
					if(that.addedSeries){
						that.addedSeries.remove();
					}
					that.addedSeries = null;
				});
				
				//增加同比值
				$("#tongbi").click(function(e){
					e.preventDefault();
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						param.IsDevice = 1;
						param.QueryType = 4;
						var json = that.search(param);
						//先清除掉已添加的series，如果有
						if(that.addedSeries){
							that.addedSeries.remove();
						}
						that.addedSeries = appQushi.addTBSeries(that.chart1, json);
						
						
					}else{
						return false;
					}		
					
				});
			
				//增加环比值
				$("#huanbi").click(function(e){					
					e.preventDefault();
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						param.IsDevice = 1;
						param.QueryType = 5;
						var json = that.search(param);
						
						//先清除掉已添加的series，如果有
						if(that.addedSeries){
							that.addedSeries.remove();
						}
						that.addedSeries = appQushi.addHBSeries(that.chart1, json);
						
						
					}else{
						return false;
					}	
				});
				
				//点击“设备能耗”，加载设备列表及第一项设备信息
				$("#device-btn").click(function(e){					
					e.preventDefault();
					if(that.isQuering == true){
						return false;
					}
					var flag = that.checkParam(param);
					if(flag){
						//param.QueryType = 5;
						//IsDetail: 0-不需要详细信息，1-需要详细信息
						param.IsDetail = 1;
						var callback = function(json){
							$("#chartsAnalyses .itemName").html($("#ana-deviceList li.current a").text());
							if(json.DeviceUnitList.length>0){
								//加载第一项信息
								that.getDeviceInfo(param);
								//显示该设备的柱状图
								that.ajaxRenderCharts(param);
							}							
						};
						that.getDeviceList(param, callback);				
						
						
						
						//切出设备信息
						$("#AnalyDeviceBlock").show();
						$("#chartsAnalyPie").hide();
						
						$("#tool-bar .fn").hide();
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
						
						//显示该设备的柱状图
						that.ajaxRenderCharts(param);
						
					}else{
						return false;
					}	
				});
			}			 
			 
		},

		/**
		* 页面初始化查询
		* Author: pl
		* Time: 2014-3-24 20:43:09
		*/
		initQuery: function(){
			var that = this;
			var param = this.getParam();
			//that.search(param);
			//获取设别列表
			param.IsDetail = 1;
			var callback = function(json){
				if(json.DeviceUnitList.length>0){
					//加载第一项信息
					that.getDeviceInfo(param);
					//加载设备详细信息
					//that.equipValue(param);
					//显示该设备的走势图
					that.ajaxRenderCharts(param);
				}							
			};
			that.getDeviceList(param, callback);
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
				selectchildren: false,
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
				onSelect: function(node) {
					// 节点被选择时触发
					if(!node.children.length > 0 && node.state === 'open') {
						var param = {
							roomId: node.id
						};
					}
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
			var param = {};
			//计算时间颗粒度
			if(nodeSelected){
				param.ObjectId = nodeSelected.id;				
			}
			param.ObjType = objType;
			param.ItemCode = itemCode?itemCode:"00000";
			param.QueryType = 1;
			param.IsDetail = 1;
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
		 	var DeviceID = $("#ana-deviceList li.current a").attr("data-DeviceID");
			param.ObjectId = DeviceID;
			param.IsDevice = 1;
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
							//设备通讯是否正常
							if(deviceInfo.Status == 1){
								$("#device-status").removeClass().addClass("status-1");
							}else if(deviceInfo.Status == 0){
								$("#device-status").removeClass().addClass("status-0");
							}else{
								$("#device-status").removeClass().addClass("status-2");
							}
							//设备编号
							//$("#Number>span").html(deviceInfo.Number);
							//演示版本的设备编号改成设备ID
							$("#Number>span").html(DeviceID);
							//类别
							$("#Category>span").html(deviceInfo.Category);
							//性质
							$("#Nature>span").html(deviceInfo.Nature);
							//隶属机构
							$("#Affiliations>span").html(deviceInfo.Affiliations);
							
							//设备详细信息
							_this.equipValue(json);
						}
					}else{
						$("#device-status").removeClass().addClass("status-disconnected");
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
		 
		 /**
		 * 当日用电趋势
		 * author: hf
		 * time: 2014-3-30 21:20:20
		 */
		 ajaxRenderCharts: function(param){
		 	var that = this;
			var objParam = param;		
			var param = JSON.stringify(param);
		 	$.ajax({
				url: this.getAction().chartDataUrl,
				dataType: "json",
				type: "post",
				data: {inputs:param || {}},
				beforeSend: function(){
					$tiansu.common.loading('#chart-analyses','show');
				},
				success: function(json) {
					if (json.ActionInfo.Success) {						
						that.renderChart(json);
						//设置单位：
						$("#chartsAnalyses .func span.unit span").html(json.Unit);
					} else {
						$tiansu.common.info('show', {
						content: '数据错误',
						timeout: 2000
					});
					}
				},
				error: function() {
					$tiansu.common.info('show', {
						content: "数据请求失败，请联系管理员！",
						css: {/*
							position: "absolute",
							top: $(this).offset().top + 120,
							left: $(this).offset().left + 48*/
						},
						timeout: 2000
					});
				},
				complete: function(){
					$tiansu.common.loading('#main','hide');
				}
			});
		 },
		 
		 renderChart: function(json){
		 	charts.resetTimezone();
			var strSysDate = $("#sys-time").text();
			var sysDate = new Date(strSysDate.replace(/-/g, "/"));
			this.chart1 = appQushi.renderTo("chart-analyses", json, sysDate);
		 },
		 
		 /** 
		 * 设备数值
		 * @param  {obj} data [发给后台的默认信息接口]
		 * author: ghj
		 * time: 2013-07-14 21:55:07
		 */
		equipValue: function(json) {


						var $eauiplistanalog = $("tbody#equiplistanalog"),
							$eauiplistpulse = $("tbody#equiplistpulse"),
							$eauiplistswitch = $("tbody#equiplistswitch"),
							html = "";
					
						if (json.Data.Analog.length > 0) {
							//html = template.render("equipValueAnalog", json.Data);
                            html = template.render("equipValuePulse", json.Data);
							$eauiplistanalog.html(html);
							$eauiplistanalog.children().fadeIn(400);
						} else {
							$eauiplistanalog.html("没有数据!");
						}
						if (json.Data.Pulse.length > 0) {
							//html = template.render("equipValuePulse", json.Data);
                            html = template.render("equipValueSwitch", json.Data);
							$eauiplistpulse.html(html);
							$eauiplistpulse.children().fadeIn(400);
						} else {
							$eauiplistpulse.html("没有数据!");
						}
						if (json.Data.Switch.length > 0) {
							//html = template.render("equipValueSwitch", json.Data);
                            html = template.render("equipValueAnalog", json.Data);
							$eauiplistswitch.html(html);
							$eauiplistswitch.children().fadeIn(400);
						} else {
							$eauiplistswitch.html("没有数据!");
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
				select:'action.ashx?action=indexItem',
				chartDataUrl: 'action.ashx?action=deviceRealChart',
				deviceListUrl: 'action.ashx?action=indexDeviceList',
				deviceInfoUrl: 'action.ashx?action=realtime',
			};
		}
	};

	return Device;
});