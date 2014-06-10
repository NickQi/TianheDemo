/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗对比/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appNHdb'], function(charts, appNHdb) {

	function Contrast() {
		this.serverDate = $("#serverDate").val();
		this.isObj = true;

		this.chart1 = null;

		//恢复默认时所用到的变量副本 开始
		this.chartDataDefault = null;
		this.tblDataDefault = null;
		this.tblObjListDefault = null;
		this.dateStringDefault = "";
		this.itemNameDefault = "";
		//恢复默认时所用到的变量副本 结束

		this.chartData = null;
		this.tblData = null;
/*
		this.danweiChartData = null;
		this.danweiTblData = null;

		this.renjunChartData = null;
		this.renjunTblData = null;
*/
		this.startTime = null;
		this.endTime = null;

		//多时间段对比时，需要一个全局的变量来存储已选时间段，并保存一个副本用于恢复
		this.selectedDates = [];
		this.selectedDatesCopy = [];

		this.init();
	}

	Contrast.prototype = {

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
			var storage = window.sessionStorage;
			this.bindEvent.treeSelect.call(this);
			this.bindEvent.initSelect.call(this);
			var $tree = this.bindEvent.initTree.call(this);
			this.bindEvent.initTreeSelected.call(this);
			this.bindEvent.typeSwitch.call(this);
			this.bindEvent.fastSelectDate.call(this);
			this.bindEvent.timeSpanSelect.call(this);
			this.bindEvent.addTime.call(this);
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
			initTree:function(){
				var storage = window.sessionStorage;
				if(storage.ObjType == 1){
					var $tree = $('#areaTree-content ul:first-child');
					$('#areaTree-content').show().siblings("#tree-content").hide();
					if($tree.html() == ""){
						if(storage.ItemCode2){
							this.getTree(1,storage.ItemCode2 || $("select[name=type-diff]").eq(1).val());
						}else{
							this.getTree(1,storage.ItemCode1 || $("select[name=type-diff]").eq(1).val());
						}						
						$("#tree-tab>li.last").addClass("current").siblings(".first").removeClass("current");
					}
					$areaSelect = $("#area-select");
					$areaSelect.children().hide().eq(1).show();

				}else{
					var $tree = this.getTree(2, storage.ItemCode1 || $("select[name=type-diff]").eq(0).val());
				}
                return $tree;
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
						$treeContainer.children("").hide().eq(index).show();
						$treeContainer.children(".loading").show();
						$areaSelect.children().hide().eq(index).show();
						if(index == 1){
							var $tree = $('#areaTree-content ul:first-child');
							if($tree.html() == ""){
								that.getTree(1,$("select[name=type-diff]").eq(1).val());
							}
						}else if(index == 0){
							var $tree = $('#tree-content ul:first-child');
							if($tree.html() == ""){
								that.getTree(2,$("select[name=type-diff]").eq(1).val());
							}	
						}
					});
				}
			},

			/** 
			 * 多时间、多对象切换
			 * author: pl
			 * time: 2014-03-22 10:46:48
			 */
			typeSwitch: function() {
				var that = this;
				var $contrastType = $("#contrast-type");
				$("#contrast-type a").click(function() {
					if ($(this).index() == 0) {
						$contrastType.removeClass("multime");
						$(this).css("color", "#fff");
						$(this).siblings().css("color", "#000");
					} else {
						$contrastType.addClass("multime");
						$(this).css("color", "#fff");
						$(this).siblings().css("color", "#000");
					}
					var type = $(this).attr("data-type");
					if (type == 1) {
						that.isObj = true;
						$("#multi-time").hide();
						$("#multi-obj").show();
						$(".select-all").removeClass("hide");
						$(".tree-node.tree-node-selected").removeClass("tree-node-selected");
					} else {
						that.isObj = false;
						$("#multi-obj").hide();
						$("#multi-time").show();
						$(".select-all").addClass("hide");
						$(".tree-node.tree-node-selected").removeClass("tree-node-selected");
					}

					//alert(that.isObj);
				});
			},

			/** 
			 * 日历控件快捷方式
			 * author: pl
			 * time: 2014-03-22 10:46:48
			 */
			fastSelectDate: function() {
				var storage = window.sessionStorage;
				var $startDate = $('#multi-obj input[name=startDate]'),
					$endDate = $('#multi-obj input[name=endDate]');
				var that = this;
				var serverDate = this.serverDate.replace(/-/g, "/");
				var edate,sdate;
				if(storage.StartTime && storage.EndTime){					
					sdate = storage.StartTime;
					edate = storage.EndTime;				
				}else{
					edate = $tiansu.date.format({
						date: new Date(serverDate),
						connect: '-'
					});	
					sdate = edate;
				}
				if(edate != this.serverDate || sdate != this.serverDate){
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
					endDate: that.serverDate,
					serverDate: that.serverDate,
					autoclose: true,
					minViewMode: 0,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				}).on("changeDate", function() {
					$(".fast-select a").removeClass("current");
				});
				var today = this.serverDate;
				var $fastSelect = $(".fast-select a");
				$fastSelect.click(function() {
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
			 * 日历时间跨度选择
			 * author: pl
			 * time: 2014-3-25 20:05:45
			 */
			timeSpanSelect: function() {
				var that = this;
				var $startDate = $('#multi-time input[name=startDate]'),
					$endDate = $('#multi-time input[name=endDate]');
				var that = this;
				var serverDate = this.serverDate.replace(/-/g, "/");
				var edate = $tiansu.date.format({
					date: new Date(serverDate),
					connect: '-'
				}),
					sdate = edate;
				$startDate.val(sdate);
				$endDate.val(edate);
				$endDate.attr("disabled", "disabled");
				$startDate.datepicker("remove");
				$startDate.datepicker({
					format: "yyyy-mm-dd",
					weekStart: 1,
					endDate: that.serverDate,
					autoclose: true,
					minViewMode: 0,
					todayBtn: "linked",
					todayHighlight: false,
					keyboardNavigation: false
				});
				$startDate.change(function() {
					$endDate.val($startDate.val());
				});
				var $timeSpan = $("input[name=time-span]");
				$.each($timeSpan, function(i, item) {
					$(item).selected = false;
				});
				$timeSpan.eq(0).attr("checked", "checked");
				$timeSpan.click(function() {
					var type = $(this).val();
					switch (type) {
						case "1":
							$tiansu.date.datePeriod({
								type: "1",
								connect: "-",
								date: that.serverDate,
								endDate: that.serverDate,
								sDate: $startDate,
								eDate: $endDate
							});
							$startDate.datepicker("remove");
							$startDate.datepicker({
								format: "yyyy-mm-dd",
								weekStart: 1,
								endDate: that.serverDate,
								serverDate: that.serverDate,
								autoclose: true,
								minViewMode: 0,
								todayBtn: "linked",
								todayHighlight: false,
								keyboardNavigation: false
							});
							$startDate.unbind("change").change(function() {
								$endDate.val($startDate.val());
							});
							$endDate.attr("disabled", "disabled");
							break;
						case "2":
							$tiansu.date.datePeriod({
								type: "2",
								connect: "-",
								date: that.serverDate,
								endDate: that.serverDate,
								sDate: $startDate,
								eDate: $endDate
							});
							$startDate.datepicker("remove");
							$startDate.datepicker({
								format: "yyyy-mm-dd",
								weekStart: 1,
								endDate: that.serverDate,
								serverDate: that.serverDate,
								autoclose: true,
								minViewMode: 0,
								todayBtn: "linked",
								todayHighlight: false,
								keyboardNavigation: false
							});
							$startDate.unbind("change").change(function() {
								$tiansu.date.datePeriod({
									type: "2",
									connect: "-",
									date: $startDate.val(),
									endDate: that.serverDate,
									sDate: $startDate,
									eDate: $endDate
								});
							});
							$endDate.attr("disabled", "disabled");
							break;
						case "3":
							$tiansu.date.datePeriod({
								type: "3",
								connect: "-",
								date: that.serverDate,
								endDate: that.serverDate,
								sDate: $startDate,
								eDate: $endDate
							});
							$startDate.datepicker("remove");
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
							$startDate.unbind("change").change(function() {
								$tiansu.date.datePeriod({
									type: "3",
									connect: "-",
									date: $startDate.val(),
									endDate: that.serverDate,
									sDate: $startDate,
									eDate: $endDate
								});
							});
							$endDate.attr("disabled", "disabled");
							break;
						case "4":
							$tiansu.date.datePeriod({
								type: "4",
								connect: "-",
								date: that.serverDate,
								endDate: that.serverDate,
								sDate: $startDate,
								eDate: $endDate
							});
							$startDate.datepicker("remove");
							$startDate.datepicker({
								format: "yyyy-mm-dd",
								weekStart: 1,
								endDate: that.serverDate,
								autoclose: true,
								minViewMode: 2,
								todayBtn: "linked",
								todayHighlight: false,
								keyboardNavigation: false
							});
							$startDate.unbind("change").change(function() {
								$tiansu.date.datePeriod({
									type: "4",
									connect: "-",
									date: $startDate.val(),
									endDate: that.serverDate,
									sDate: $startDate,
									eDate: $endDate
								});
							});
							$endDate.attr("disabled", "disabled");
							break;
						case "0":
							$startDate.datepicker("remove");
							$startDate.datepicker({
								format: "yyyy-mm-dd",
								weekStart: 1,
								endDate: that.serverDate,
								autoclose: true,
								minViewMode: 0,
								todayBtn: "linked",
								todayHighlight: false,
								keyboardNavigation: false
							}).on("changeDate", function() {
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
							$startDate.unbind("change").change(function() {
								$endDate.val($startDate.val());
							});
							$endDate.removeAttr("disabled");
							$endDate.datepicker("remove");
							$endDate.datepicker({
								format: "yyyy-mm-dd",
								weekStart: 1,
								startDate: $startDate.val(),
								endDate: that.serverDate,
								autoclose: true,
								minViewMode: 0,
								todayBtn: "linked",
								todayHighlight: false,
								keyboardNavigation: false
							});
					}
				});

			},
			/** 
			 * 初始化分类分项下拉框
			 * author: pl
			 * time: 2014-3-23 22:03:40
			 */
			initSelect: function() {
				var that = this;
				var storage = window.sessionStorage;
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
								if(item.ItemCode == storage.ItemCode1){
									$typeDiff.append('<option value='+item.ItemCode+' selected="selected">'+item.ItemName+'</option>');
								}else{
									$typeDiff.append('<option value='+item.ItemCode+'>'+item.ItemName+'</option>');
								}
							});
							$typeDiff.append('<option value="00000">总能耗</option>');
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
				var storage = window.sessionStorage;
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
								if(item.ItemCode == storage.ItemCode2){
									$item.append('<option value='+item.ItemCode+' selected="selected">'+item.ItemName+'</option>');
								}else{
									$item.append('<option value=' + item.ItemCode + '>' + item.ItemName + '</option>');
								}								
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
			 initTreeSelected: function(){
			 	var storage = window.sessionStorage;
				if($("#tree-content").is(":visible")){
					var $tree = $("#tree-content ul:first");
				}else{
					var $tree = $("#areaTree-content ul:first");
				}
			 	if(storage.AreaIdLst){
			 		var node = storage.AreaIdLst.split(",");
			 		$.each(node,function(i,item){
			 			var selectNode = $tree.tree("find",item);
			 			selectNode && $(selectNode.target).addClass("tree-node-selected");
			 			selectNode && $tree.tree("expandTo",selectNode.target);
			 		})
			 	}else{
					var $node = $("#tree-content ul").eq(1).children("li").children(".tree-node");
					var length = $node.length>20?20:$node.length;
					for(var i=0;i<length;i++){
						$($node[i]).addClass("tree-node-selected");
					}
			 	}
			 },

			/**
			 * 点击查询按钮触发查询事件
			 * Author: pl
			 * Time: 2014-3-24 09:48:58
			 */
			search: function() {
				var that = this;
				var param = that.getParam();
				var queryTypes = {
					danwei: 2,
					renjun: 3,
					biaozhunmei:6,
					tanpfl:7,
					rmb:8
				};	
				$("#queryBtn").click(function(e) {
					$(".fn a, .fn3 a").removeClass("current");
					$("#chart-contrast").attr("data-target", "default");
					param = that.getParam(e.target);
					if(that.isObj == true){
						//匿名函数自我执行创建独立的作用域
						(function(){
							var param = that.getParam();
							if($("#tree-content").is(":visible")){
								param.ItemCode1 = $("#area-select").children().eq(0).find("select[name=type-diff]").val();
							}else{							
								param.ItemCode1 =  $("#area-select").children().eq(1).find("select[name=type-diff]").val();
							}
							if($("select[name=item-diff]").is(":visible")){
								param.ItemCode2 = $("select[name=item-diff]").val();
							}						
							//alert(param.ItemCode1);
							$tiansu.common.shareParam(param);
						})();
					}

                    var check = that.checkFirstDay(param);
                    if(check) {

                        var flag = that.checkParam(param);
                        if (flag == true) {
                            var json = that.search(param);
                            if (json.ActionInfo.Success) {

                                /*如果是当日、本周或本月，则用0填充剩下的时间数据 开始*/
                                var lineSeries = json.lineJson.series;
                                var lineSeriesLength = lineSeries.length;
                                for (var i = 0; i < lineSeriesLength; i++) {

                                    var seriesData = json.lineJson.series[i].data;
                                    var seriesDataLength = seriesData.length;
                                    var dataTimesTxt = $(".fast-select a.current").text();
                                    //只有当 当日，本周，本月按钮可见，即是在多对象时才执行
                                    if ($("#multi-obj:visible").length > 0) {
                                        switch (dataTimesTxt) {
                                            case "当日":
                                                var longLength = 24;
                                                break;
                                            case "本周":
                                                var longLength = 7;
                                                break;
                                            case "本月":
                                                //console.log(objParam);
                                                var monthStartDate = new Date(param.StartTime.replace(/-/g, "/"));
                                                var nowYear = monthStartDate.getFullYear();
                                                var myMonth = monthStartDate.getMonth();
                                                var theMonthDays = $tiansu.date.getMonthDays(nowYear, myMonth);
                                                var longLength = theMonthDays;
                                                break;
                                        }
                                    }
                                    if (longLength) {
                                        var diffLength = longLength - seriesDataLength;
                                        if (diffLength > 0) {
                                            for (var j = 0; j < diffLength; j++) {
                                                seriesData.push(0);
                                            }
                                        }
                                    }
                                }
                                /*如果是当日、本周或本月，则用0填充剩下的时间数据 结束*/

                                //清空已选数组副本
                                that.selectedDatesCopy = [];

                                that.chartData = json.lineJson;
                                that.chartData.unit = json.Unit;
                                that.tblData = json;
                                if (that.isObj == true) {
                                    that.startTime = new Date(param.StartTime.replace(/-/g, "/"));
                                    that.endTime = new Date(param.EndTime.replace(/-/g, "/"));
                                }

                                //存储一份副本用作恢复默认 开始
                                that.chartDataDefault = $.extend(true, {}, that.chartData);
                                that.tblDataDefault = $.extend(true, {}, that.tblData);
                                if (that.isObj) {
                                    that.dateStringDefault = that.getChartInfo().dateString;
                                } else {
                                    that.dateStringDefault = "";
                                }
                                that.itemNameDefault = that.getChartInfo().itemName;
                                //存储一份副本用作恢复默认 结束


                                //渲染图表之前首先切换对应的图例面板样式，并且，若是多对象对比时，清除掉已选择多时间段数组里的值
                                if (that.isObj) {
                                    $("#chart-contrast-panel").attr("data-type", "obj");
                                    $("#chart-contrast-panel").removeClass("time-chart-panel");
                                    that.selectedDates = [];
                                    //解锁时间跨度选项
                                    that.resetTimeSlc();
                                } else {
                                    $("#chart-contrast-panel").attr("data-type", "time");
                                    $("#chart-contrast-panel").addClass("time-chart-panel");
                                }
                                that.renderCharts(that.chartData, that.tblData);

                                //填充标题 hf 2014-4-25 10:32:30
                                if (!that.isObj) {
                                    var treeTypeVal = param.ObjType;
                                    var treeElem = "#tree-content";
                                    if (treeTypeVal == 1) {
                                        treeElem = "#areaTree-content";
                                    } else {
                                        treeElem = "#tree-content";
                                    }
                                    var breadcrumb = that.getTreeNodePath(treeElem);
                                    $("#chartsContrast h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
                                } else {
                                    $("#chartsContrast h3 span.areaName").html("");
                                }
                                //填充标题结束 hf 2014-4-25 10:32:30

                            } else {
                                $tiansu.common.info('show', {
                                    timeout: 1000,
                                    content: json.ActionInfo.ExceptionMsg
                                });
                            }
                        }
                    }else{
                        that.handleFirstDay();
                    }

				});
				
				$(".fn a, .fn3 a").click(function(e) {
					e.preventDefault();
					//优化有的代码不再分数据类别，以下这句已无作用 hf 2014-4-17 18:01:11
					$("#chart-contrast").attr("data-target", "danwei");
					//var param = that.getParam();
                    var check = that.checkFirstDay(param);
                    if(check) {
                        var flag = that.checkParam(param);
                        if (flag == true) {
                            $(".fn a, .fn3 a").removeClass("current");
                            $(this).addClass("current");
                            //param.QueryType = 2;
                            var typesub = $(e.target).attr("id");
                            var typeId = queryTypes[typesub];
                            param.QueryType = typeId;
                            var json = that.search(param);
                            if (json.ActionInfo.Success) {
                                that.chartData = json.lineJson;
                                that.chartData.unit = json.Unit;
                                that.tblData = json;
                                if (this.isObj == true) {
                                    that.startTime = new Date(param.StartTime.replace(/-/g, "/"));
                                    that.endTime = new Date(param.EndTime.replace(/-/g, "/"));
                                }

                                //渲染图表之前首先切换对应的图例面板样式
                                if (that.isObj) {
                                    $("#chart-contrast-panel").removeClass("time-chart-panel");
                                    that.selectedDates = [];
                                    //解锁时间跨度选项
                                    that.resetTimeSlc();
                                } else {
                                    $("#chart-contrast-panel").addClass("time-chart-panel");
                                }
                                that.renderCharts(that.chartData, that.tblData);
                            } else {
                                $tiansu.common.info('show', {
                                    timeout: 1000,
                                    content: json.ActionInfo.ExceptionMsg
                                });
                            }
                        }
                    }else{
                        that.handleFirstDay();
                    }
				});

				//恢复默认
				$("#default").click(function(e) {
					$(".fn a, .fn3 a").removeClass("current");
					param.QueryType = 1;
					e.preventDefault();
					$("#chart-contrast").attr("data-target", "default");
                    var check = that.checkFirstDay(param);
                    if(check) {
                        that.chartData = $.extend(true, {}, that.chartDataDefault);
                        that.tblData = $.extend(true, {}, that.tblDataDefault);
                        //恢复已选择数组
                        that.selectedDates = that.selectedDatesCopy.concat();

                        //渲染图表之前首先切换对应的图例面板样式
                        that.renderCharts(that.chartData, that.tblData);
                        that.renderObjList(that.tblObjListDefault);

                        //恢复树上的选择项以及param里的AreaIdLst.
                        //根据树的显示隐藏获取对应树对象
                        if ($("#tree-content").is(":visible")) {
                            var $tree = $("#tree-content ul");
                        } else {
                            var $tree = $("#areaTree-content ul");
                        }
                        //先清空掉AreaIdLst里的数据，重新添加
                        param.AreaIdLst = [];
                        $("#chart-contrast-panel li").each(function (index, element) {
                            var areaId = $(element).attr("data-treeid");
                            param.AreaIdLst.push(areaId);
                            var treenode = $tree.tree('find', areaId);
                            if (treenode) {
                                $(treenode.target).addClass("tree-node-selected");
                            }
                        });
                    }else{
                        that.handleFirstDay();
                    }
				});
				//导出按钮
				$("#export-link").click(function() {
					if ($(this).hasClass("export-disabled")) {
						return false;
					} else {
						var flag = that.checkParam(param);
						//var $a = $(".fn a.current");
						var $a = $(".fn a.current, .fn3 a.current");
						if (flag) {
                            var check = that.checkFirstDay(param);
                            if(check) {
                                if ($a.length > 0) {
                                    $.each($a, function (i, item) {
                                        var typesub = $(item).attr("id");
                                        var typeId = queryTypes[typesub];
                                        param.QueryType = typeId;
                                    });
                                } else {
                                    param.QueryType = 1;
                                }
                                that.exportList(param);
                            }else{
                                that.handleFirstDay();
                            }
						} else {
							return false;
						}
					}
				});
				
				/*图例面板功能*/
				$(document).on("click", "#chart-contrast-panel ul>li>a", function(e) {
					e.preventDefault();
					//若图例为生成图表之后生成的，则可以控制图表样式
					if ($(this).attr("data-type") !== "before-render") {
						var $item = $(this).parent("li");
						var index = $item.index();
						//alert(index);
						var seriesArray = that.chart1.series;
						
						//图例增加当前样式
						$(this).addClass("current").parent("li").siblings().children("a").removeClass("current");
	
						//首先将所有series恢复默认样式
	
						for (var i = 0; i < seriesArray.length; i++) {
							seriesArray[i].update({
								marker: {
									enabled: false
								},
								lineWidth: 1
							})
						}
	
						seriesArray[index].update({
							marker: {
								enabled: true,
								radius: 3
							},
							lineWidth: 4
						});
					}
				});
				$(document).on("click", "#chart-contrast-panel ul>li>a>.series-close", function(e) {
					e.preventDefault();
					e.stopPropagation();
					
					var $item = $(this).parents("li");
					var index = $item.index();
					//多时间段时，删除掉对应的已选择时间段数组中的项
					//else多对象时，删掉对应的树节点选择状态
					if (that.selectedDates.length > 0) {
						//首先备份一次，仅当备份数组为空时备份，也就是只在第一次时备份
						if (that.selectedDatesCopy.length == 0) {
							that.selectedDatesCopy = that.selectedDates.concat();
						}
						that.selectedDates.splice(index, 1);
					}else{
						var treeId = $item.attr("data-treeid");
						//根据树的显示隐藏获取对应树对象
						if($("#tree-content").is(":visible")){
							var $tree = $("#tree-content ul");
						}else{
							var $tree = $("#areaTree-content ul");
						}
						
						//将此值从param.AreaIdLst中删除
						var areaIdLstLength = param.AreaIdLst.length;
						for(var i = 0; i < areaIdLstLength; i++){
							if(param.AreaIdLst[i] == treeId){
								param.AreaIdLst.splice(i,1);
							}
						}
						//从树上删掉对应的选择
						var treenode = $tree.tree('find', treeId);
                        if(treenode) {
                            $(treenode.target).removeClass("tree-node-selected");
                        }
					}
	
					//当已选时间段数组被清空时，解锁时间跨度选项
					if (that.selectedDates.length == 0) {
						//解锁时间跨度选项
						that.resetTimeSlc();
	
						if ($("#multi-time input[name='time-span']:checked").val() === "0") {
							$("#multi-time input[name='endDate']").attr("disabled", false);
						}
					}
						
					//若图例为生成图表之后生成的，则可以同时删除图表曲线，否则只删除自身
					//为图例面板连动改成了发起请求，则：
					//若图例为生成图表之后生成的，则发起新请求，否则只删除自身 2014-4-17 19:58:58 hf
					if ($(this).parent("a").attr("data-type") !== "before-render") {
                        //删除元素时，不再限制最少选择的个数
						//var flag = that.checkParam(param);
                        var flag = true;
						if (flag == true) {
							//$(".fn a, .fn3 a").removeClass("current");
							//$(this).addClass("current");
							//param.QueryType = 2;
							//var typesub = $(e.target).attr("id");
							//var typeId = queryTypes[typesub];
							//param.QueryType = typeId;
							var json = that.search(param);
							if (json.ActionInfo.Success) {
								that.chartData = json.lineJson;
								that.chartData.unit = json.Unit;
								that.tblData = json;
								if (this.isObj == true) {
									that.startTime = new Date(param.StartTime.replace(/-/g, "/"));
									that.endTime = new Date(param.EndTime.replace(/-/g, "/"));
								}
	
								//渲染图表之前首先切换对应的图例面板样式
								if (that.isObj) {
									$("#chart-contrast-panel").removeClass("time-chart-panel");
									that.selectedDates = [];
									//解锁时间跨度选项
									that.resetTimeSlc();
								} else {
									$("#chart-contrast-panel").addClass("time-chart-panel");
								}
								that.renderCharts(that.chartData, that.tblData);
							} else {
								$tiansu.common.info('show', {
									timeout: 1000,
									content: json.ActionInfo.ExceptionMsg
								});
							}
						}
					} else {
						$(this).parent("a").parent("li").remove();
					}
	
					//console.log(_this.selectedDates);
	
				});
				$(document).on("click", "#chart-contrast-panel .removeAll>a", function(e) {
					e.preventDefault();
	
					//若图例为生成图表之后生成的，则可以同时删除图表曲线，否则只清空图例，因为“多时间、生成前”的关系，需保留清除按钮
					//判断是绘图后生成的图例还是绘图前添加的，只要有一个图例是绘图后生成的，就算是绘图后生成的
					var isBefore = true;
					$("#chart-contrast-panel ul>li>a").each(function(index, element) {
						if ($(element).attr("data-type") !== "before-render") {
							isBefore = false;
						}
						return false;
					});
	
					if (!isBefore) {						
						that.chartData.series = [];
						that.tblData.ContrastLst = [];
						that.renderCharts(that.chartData, that.tblData);
						//清除对象列表内容
						$("#constrastObj-list tbody").html("");
						
						//$(this).parent(".removeAll").remove();
					} else {
						$("#chart-contrast-panel ul>li").remove();
					}
	
					//多时间段时，清空已选择时间段数组中的项
					if (that.selectedDates.length > 0) {
						//首先备份一次，仅当备份数组为空时备份，也就是只在第一次时备份
						if (that.selectedDatesCopy.length == 0) {
							that.selectedDatesCopy = that.selectedDates.concat();
						}
						that.selectedDates = [];
						//解锁时间跨度选项
						that.resetTimeSlc();
						if ($("#multi-time input[name='time-span']:checked").val() === "0") {
							$("#multi-time input[name='endDate']").attr("disabled", false);
						}
					}
					
					//清除树上的选择项
					//根据树的显示隐藏获取对应树对象
					if(that.isObj){
						if($("#tree-content").is(":visible")){
							var $tree = $("#tree-content ul");
						}else{
							var $tree = $("#areaTree-content ul");
						}
						$tree.find(".tree-node-selected").removeClass("tree-node-selected");
					}
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
			slcChartTbl: function() {
				var _this = this;
				var $tbl = $("#table-contrast");
				var $chart = $("#chart-contrast");
				$("#slcChartTbl>a").click(function(e) {
					e.preventDefault();
                    var param = _this.getParam();
                    var check = _this.checkFirstDay(param);
                    if(check) {
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
                                _this.renderCharts(_this.chartData, _this.tblData);
                            });
                            $("#export-link").removeClass().addClass("export-disabled");
                        }
                    }else{
                        _this.handleFirstDay();
                    }
				});
			},

			/**
			 * 多时间段对比，点击加号增加时间段
			 * Author: HF
			 * Time: 2014-3-27 10:42:44
			 */
			addTime: function() {
				var _this = this;
				//var selectedDates = [];
				$("#multi-time .date-add").click(function() {
					_this.timeAddTo("#chart-contrast-panel", _this.selectedDates);

					$startTime = $("#multi-time input[name='startDate']");
					$endTime = $("#multi-time input[name='endDate']");
					var startTime = $startTime.val();
					var endTime = $endTime.val();
					var objStart = new Date(startTime.replace(/-/g, "/"));
					var objEnd = new Date(endTime.replace(/-/g, "/"));
					var num = (objEnd.valueOf() - objStart.valueOf()) / (1000 * 3600 * 24);
					if ($("#multi-time input[name='time-span']:checked").val() === "0") {
						$endTime.attr("disabled", "disabled");
						$startTime.unbind("change").change(function() {
							var startDate = $startTime.val();
							if ($endTime.is(":disabled")) {
								var endDateObj = $tiansu.date.diff({
									date: startDate,
									type: "day",
									number: num
								});

								var year = endDateObj.getFullYear();
								var month = endDateObj.getMonth() + 1;
								var day = endDateObj.getDate();
								if (month < 10) {
									month = "0" + month;
								}
								if (day < 10) {
									day = "0" + day;
								}
								endDateString = year + "-" + month + "-" + day;
								$endTime.val(endDateString);
							}
						});
					}
				});
			}
		},


		/**
		 * 将选中的时间段添加到图例面板
		 * @param: elem为图例容器的选择符："#chart-contrast-panel"
		 * @param:
		 * Author: HF
		 * Time: 2014-3-27 10:49:48
		 */
		timeAddTo: function(elem, slcArr) {
			var $elem = $(elem);
			var $startTimeInput = $("#multi-time input[name='startDate']");
			var $endTimeInput = $("#multi-time input[name='endDate']");
			var startTime = $startTimeInput.val();
			var endTime = $endTimeInput.val();
			var timeString = "";

			//判断是否选择重复的时间段和个数
			var slcLength = slcArr.length;
			if(slcLength >= 20){
				$tiansu.common.info('show', {
						container:'#multi-time',
						centerX: false,
						centerY: false,
						css: {
							top:'200px',
							left:'80px'
						},
						content: "添加最多不得超过20个时间段！",
						timeout:2000
				});
				return false;
			}
			for(var i = 0; i < slcLength; i++){
				if(startTime === slcArr[i].StartTime && endTime === slcArr[i].EndTime){
					$tiansu.common.info('show', {
						container: '#multi-time',
						centerX: false,
						centerY: false,
						css: {
							top: '200px',
							left: '80px'
						},
						content: "此时间段已添加过！",
						timeout: 2000
					});
					return false;
				}
			}
			//var aTimes = [startTime, endTime];
			var aTimes = {
				"StartTime": startTime,
				"EndTime": endTime
			}
			slcArr.push(aTimes);
			//console.log(this.selectedDates);

			if (startTime === endTime) {
				timeString = '<li><a data-type="before-render" href=""><span class="color"></span><span class="series-name">' + startTime + '</span><span class="series-close"> X</span></a></li>';
			} else {
				timeString = '<li><a data-type="before-render" href=""><span class="color"></span><span class="series-name">' + startTime + '—' + endTime + '</span><span class="series-close"> X</span></a></li>';
			}

			//禁用时间跨度单选框，以免用户选择不同跨度的时间段对比
			$("#multi-time input[name='time-span']").attr("disabled", "disabled");

			//若之前有属于多对象对比的图例，清空
			if ($elem.attr("data-type") === "obj") {
				$elem.find("ul").children("li").remove();
			}
			//将图例面板的类型改为"time"，并增加相应的class
			$elem.attr("data-type", "time").addClass("time-chart-panel");

			$elem.find("ul").append(timeString);
			this._setColors();
			//提示添加成功
			$tiansu.common.info('show', {
				container: '#multi-time',
				centerX: false,
				centerY: false,
				css: {
					top: '200px',
					left: '80px'
				},
				content: "添加成功！",
				timeout: 400
			});


			$(elem).jScrollPane({
				autoReinitialise: true,
				mouseWheelSpeed: 20
			});
		},

		/**
		 * 给选中的时间段添加色块
		 * Author: HF
		 * Time: 2014-3-27 16:08:10
		 */
		_setColors: function() {
			var colors = [
				'#1ABC9C',
				'#81ABA9',
				'#A2D7DD',
				'#84A2D4',
				'#69B076',
				'#ABCF53',
				'#DCCB19',
				'#8F8667',
				'#C7B26F',
				'#E27D37',
				'#946142',
				'#CC8C5C',
				'#E0C28C',
				'#E4DC8B',
				'#B88884',
				'#E5D2D8',
				'#C3A2BD',
				'#94859C',
				'#2CA9E1',
				'#3E62AE'
			];
			$(".time-chart-panel li").each(function(index, element) {
				$(element).find("span.color").css("background", colors[index]);
			});
		},

		/**
		 * 解锁时间跨度选项（当所选时间被清空时调用）
		 * Author: HF
		 * Time: 2014-3-27 19:40:40
		 */
		resetTimeSlc: function() {
			$("#multi-time input[type='radio']").attr("disabled", false);
		},



		/**
		 * 页面初始化查询
		 * Author: pl
		 * Time: 2014-3-24 20:43:09
		 */
		initQuery: function() {
			var _this = this;

			//恢复时间跨度选择radio
			//$("#multi-time input[name='time-span']").attr("disabled", false).attr("checked", false).eq(0).attr("checked","checked");
			$("#multi-time input[name='time-span']").attr("disabled", false).eq(0).click();

			var param = this.getParam();
            var check = _this.checkFirstDay(param);
            if(check) {
                var json = this.search(param);
                if (json.ActionInfo.Success) {

                    /*如果是当日、本周或本月，则用0填充剩下的时间数据 开始*/
                    var lineSeries = json.lineJson.series;
                    var lineSeriesLength = lineSeries.length;
                    for (var i = 0; i < lineSeriesLength; i++) {

                        var seriesData = json.lineJson.series[i].data;
                        var seriesDataLength = seriesData.length;
                        var dataTimesTxt = $(".fast-select a.current").text();
                        switch (dataTimesTxt) {
                            case "当日":
                                var longLength = 24;
                                break;
                            case "本周":
                                var longLength = 7;
                                break;
                            case "本月":
                                //console.log(objParam);
                                var monthStartDate = new Date(param.StartTime.replace(/-/g, "/"));
                                var nowYear = monthStartDate.getFullYear();
                                var myMonth = monthStartDate.getMonth();
                                var theMonthDays = $tiansu.date.getMonthDays(nowYear, myMonth);
                                var longLength = theMonthDays;
                                break;
                        }
                        if (longLength) {
                            var diffLength = longLength - seriesDataLength;
                            if (diffLength > 0) {
                                for (var j = 0; j < diffLength; j++) {
                                    seriesData.push(0);
                                }
                            }
                        }
                    }
                    /*如果是当日、本周或本月，则用0填充剩下的时间数据 结束*/

                    this.chartData = json.lineJson;
                    this.chartData.unit = json.Unit;
                    this.tblData = json;

                    this.startTime = new Date(param.StartTime.replace(/-/g, "/"));
                    this.endTime = new Date(param.EndTime.replace(/-/g, "/"));

                    //存储一份副本用作恢复默认 开始
                    this.chartDataDefault = $.extend(true, {}, this.chartData);
                    this.tblDataDefault = $.extend(true, {}, this.tblData);
                    this.dateStringDefault = this.getChartInfo().dateString;
                    this.itemNameDefault = this.getChartInfo().itemName;
                    //存储一份副本用作恢复默认 结束

                    this.renderCharts(this.chartData, this.tblData);

                    //填充标题 hf 2014-4-25 10:32:30
                    if (!this.isObj) {
                        var treeTypeVal = param.ObjType;
                        var treeElem = "#tree-content";
                        if (treeTypeVal == 1) {
                            treeElem = "#areaTree-content";
                        } else {
                            treeElem = "#tree-content";
                        }
                        var breadcrumb = that.getTreeNodePath(treeElem);
                        $("#chartsContrast h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
                    } else {
                        $("#chartsContrast h3 span.areaName").html("");
                    }
                    //填充标题结束 hf 2014-4-25 10:32:30
                } else {
                    $tiansu.common.info('show', {
                        timeout: 1000,
                        content: json.ActionInfo.ExceptionMsg
                    });
                }
                ;
            }else{
                _this.handleFirstDay();
            }
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
			var is_single = false;
			if (type === 2) {
				$tree = $('#tree-content ul:first-child');
				$treeContent = $('#tree-content');
				is_single = false;
			} else if (type === 1) {
				$tree = $('#areaTree-content ul:first-child');
				$treeContent = $('#areaTree-content');
				is_single = false;
			}
			//$tiansu.common.info("show",{container:"#tree-box"});
			//$tiansu.common.loading("#tree-box","show");
			var maxlen = {
				num: 20,
				detail: "能耗对比"
			};
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
				maxlen: maxlen,
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
				onBeforeSelect: function(node) {
					if (that.isObj) {
						var $treeVisible = $(".tree");
						$.each($treeVisible, function(i, item) {
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
									top: '200px',
									left: '50px'
								},
								content: maxlen.detail + "最多选择" + maxlen.num + "个对象！",
								timeout: 2000
							});
							return false;
						}
					} else {
						$(".tree-node.tree-node-selected").removeClass("tree-node-selected");
					}
				},
				onSelect: function(node) {
					// 节点被选择时触发
					if (!node.children.length > 0 && node.state === 'open') {
						var param = {
							roomId: node.id
						};
					}
				},
				onLoadSuccess: function(node) {
					if(node && node.children.length>1){
						$(node.target).append('<span class="select-all pl5 pr5 ml5">all+</span>');
					}
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
		getParam: function(target) {
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
			//判断是多对象还是多时间，组织不同的参数对象
			if (this.isObj == true) {
				var nodeList = [];
				//遍历选中的节点得到节点id
				$.each($treeNode, function(i, item) {
					var nodeSelected = $tree.tree('getNode', item);
					nodeList.push(nodeSelected.id);
				});
				//计算时间颗粒度
				param.particle = $tiansu.common.getParticle($startDate.val(), $endDate.val());
				param.StartTime = $startDate.val();
				param.EndTime = $endDate.val();
				param.AreaIdLst = nodeList;
			} else if (this.isObj == false) {
				var nodeSelected = $tree.tree('getNode', $treeNode);
				if (nodeSelected != null) {
					param.AreaId = nodeSelected.id;
				}
				if (this.selectedDates.length > 0) {
					param.particle = $tiansu.common.getParticle(this.selectedDates[0].StartTime, this.selectedDates[0].EndTime);
				}
				//console.log(target);
				//var periodLst = $.extend(true, {}, this.selectedDates);
				param.PeriodLst = this.selectedDates;
				//param.PeriodLst = periodLst;
			}
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
		checkParam: function(param) {
			if (this.isObj == true) {
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
			} else {
				if (!param.AreaId) {
					$tiansu.common.info("show", {
						content: "请至少选择1个对象！",
						css: {
							centerX: false,
							centerY: false,
							top: '40%',
							left: '150px'
						},
						timeout: 2000
					});
					return false;
				} else if (param.PeriodLst.length < 2) {
					$tiansu.common.info("show", {
						content: "请至少选择2个时间段！",
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
			}
		},

        /**
         * 验证当天是否为每月的第一天,且在多对象对比时判断
         * true: 不是每月的第一天；false: 是每月的第一天
         * author: luyy
         * time: 2014-05-15 14:36:29
         */
        checkFirstDay: function(param){
            var fastDate = $(".fast-select a.current").attr("data-type");
            if(param.particle == 0 && (fastDate == 2 || fastDate == 3) && this.isObj){
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
            $("#chart-contrast").empty();
            $("#table-contrast .text-center").empty();
            $("#constrastObj-list .text-center").empty();
            $("#chart-contrast-panel ul").remove();
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
		search: function(param) {
			//console.log(param);
			var _this = this;
			var result = null;
			var objParam = param;
			var param = JSON.stringify(param);
			var ctype = _this.isObj ? 1 : 2;

			//处理对象列表
			$.ajax({
				url: this.getAction().objListUrl,
				dataType: 'json',
				type: 'POST',
				data: {
					cType: ctype,
					Inputs: param || {}
				},
				beforeSend: function() {
					$tiansu.common.loading('#constrastObj-list', 'show');
				},
				success: function(json) {
					if(objParam.QueryType == 1){
						_this.tblObjListDefault = $.extend(true, {}, json);
					}
					_this.renderObjList(json);
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

			$.ajax({
				url: this.getAction().dataUrl,
				dataType: 'json',
				type: 'POST',
				data: {
					cType: ctype,
					Inputs: param || {}
				},
				async: false,
				beforeSend: function() {
					$tiansu.common.loading('#chart-contrast', 'show');
					$tiansu.common.loading('#chart-contrast-panel', 'show');
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
				complete: function() {
					$tiansu.common.loading('#main', 'hide');
				}
			});



			return result;
		},

		/**
		 * 导出数据列表
		 * author: pl
		 * time: 2014-3-28 10:21:42
		 */
		exportList: function(param) {
			var ctype = this.isObj ? 1 : 2;
			var paramString = JSON.stringify(param);
			$.ajax({
				url: this.getAction().exportUrl,
				dataType: 'json',
				type: 'POST',
				data: {
					cType: ctype,
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
		 * 渲染highcharts图表
		 * @param  {obj} json [列表json数据]
		 * author: HF
		 * time: 2014-3-24 18:00:21
		 */
		renderCharts: function(chartData, tblData) {

			charts.resetTimezone();
			
			var callback = function(){
				if(this.series.length > 0){
					this.series[0].update({
						marker: {
							enabled: true,
							radius: 3
						},
						lineWidth: 4
					});
				}
			};
			
			this.chart1 = appNHdb.renderTo("chart-contrast", chartData, this.startTime, this.endTime, callback);

			//生成表格
			this.renderList(tblData);

			//chart info:
			$("#chartsContrast .unit>span").text(chartData.unit);
			$("#chartsContrast h3 span.itemName").html(this.itemNameDefault);
			$("#chartsContrast h3 span.series-date").html(this.dateStringDefault);


			//多对象比较时，截取图例面板多余字符,true为截取，false不截取
			var clip = false;
			var panelType = $("#chart-contrast-panel").attr("data-type");
			if (panelType === "obj") {
				clip = true;
			} else {
				clip = false;
			}

			if ($("#chart-contrast-panel .jspPane").size() > 0) {
				appNHdb.setPanel(this.chart1, "#chart-contrast-panel .jspPane", clip);
				//去除掉removeAll按钮，以免重复添加
				$("#chart-contrast-panel .jspPane .removeAll").remove();
				//$("#chart-contrast-panel .jspPane").jScrollPane({
				//	autoReinitialise: true,
				//	mouseWheelSpeed: 20
				//});
			} else {
				appNHdb.setPanel(this.chart1, "#chart-contrast-panel", clip);
				$("#chart-contrast-panel").jScrollPane({
					autoReinitialise: true,
					mouseWheelSpeed: 20
				});
			}

			//将remove all按钮移到滚动内容之外，使之位置固定
			if ($("#chart-contrast-panel .jspPane").size() > 0) {
				$("#chart-contrast-panel .removeAll").appendTo($("#chart-contrast-panel"));
			}
		},

		getChartInfo: function() {
			var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');
			var startDate = $startDate.val();
			var endDate = $endDate.val();
			var dateString = "(" + startDate + " — " + endDate + ")";
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
		 * 渲染列表
		 * @param  {obj} json [列表json数据]
		 * author: PL & HF
		 * time: 2013-12-15 15:46:19
		 */
		renderList: function(json) {
			var $tbody = $('#table-contrast tbody');
			var items = template('list-tpl', json);
			// 列表
			$tbody.html(items);
		},

		/** 
		 * 渲染对比对象列表
		 * @param  {obj} json [列表json数据]
		 * author: HF
		 * time: 2014-3-26 15:33:02
		 */
		renderObjList: function(json) {
			var $tbody = $('#constrastObj-list tbody');
			var $unitContainer = $("#contrastObj span.unit>span");
			var items = template('objList-tpl', json);
			// 列表
			$unitContainer.html(json.Unit);
			$tbody.html(items);
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
				dataUrl: 'action.ashx?action=IndexContrastChart',
				objListUrl: 'action.ashx?action=IndexContrastLst',
				exportUrl: 'action.ashx?action=ExportContrast'
			};
		}
	};

	return Contrast;
});