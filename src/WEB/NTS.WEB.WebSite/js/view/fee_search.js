/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗对比/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appFeePie', 'chartsmod/appFee'], function(charts, appFeePie, appFee) {

	function feeSearch() {
		//global对象用来存储全局变量
		this.global = {};
		this.serverDate = $("#sys-time").html();
		this.init();
	}

	feeSearch.prototype = {

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
			initSelect: function() {
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
				var $tbl = $("#table-fee");
				var $chart = $("#chart-fee");
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
							//that.renderCharts(_this.chartJson, _this.from, _this.to);	
							that.global.chartFee.reflow();
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
					$tiansu.common.loading('#chart-fee', 'show');
					$tiansu.common.loading('#fee-analyses-info', 'show');
					$tiansu.common.loading('#chart-fee-pencents', 'show');
				},
				success: function(json) {
					
					
					if (json.ActionInfo.Success) {
						
						/* 费用查询开始 */
						var feeData = json.FeeQueryCharts;
						//objParam.particle = 1;
						//objParam.StartTime = "2014-03-01";
						
						var from = new Date(objParam.StartTime.replace(/-/g, "/"));
						if(objParam.Particle == 2){
							var year = from.getFullYear();
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
                            var year = from.getFullYear();
							var to = new Date(year, 11, 31);
						}
						var chartFee = that.renderFeeCharts("chart-fee", feeData, from, to);
						that.global.chartFee = chartFee;
						/* 费用查询结束 */
						
						//填充能耗分类和时间和面包屑
						var info = that.getChartInfo(objParam);
						$("#chartsFee h3>.itemName").html(info.itemName);
						$("#chartsFee h3>.series-date").html(info.dateString);
						
						var treeTypeVal = objParam.ObjType;
						var treeElem = "#tree-content";
						if(treeTypeVal == 1){
							treeElem = "#areaTree-content";
						}else{
							treeElem = "#tree-content";
						}
						var breadcrumb = that.getTreeNodePath(treeElem);
						$("#chartsFee h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);

                        //填充计费方式
                        that.setFeeType(json);
						
						/*费用占比图开始*/
						var pieFeeData = json.FeePie;
						var pieFee = that.renderFeePie('chart-fee-pencents', pieFeeData);
						/*费用占比图结束*/
						
						/*渲染列表开始*/
						var feeType = json.FeeType;
						var eneType = json.FeeTbl.EneType;
						var Unit = json.FeeTbl.Unit;
						var listJson = json.FeeTbl;
						that.renderList(feeType, eneType, Unit, listJson);
						/*渲染列表结束*/
						
						/*填充极值分析*/
						var feeAnalyseeData = json.FeeAnalyses;
						that.fillFeeAnalyses(feeAnalyseeData);
					
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
		 * 渲染highcharts费用查询图表
		 * author: HF
		 * time: 2014-4-16 15:48:01
		 */
		renderFeeCharts: function(container, chartData, from, to){
			charts.resetTimezone();
			return appFee.renderTo(container, chartData, from, to);
		},
		
		/** 
		 * 渲染highcharts费用占比图表
		 * author: HF
		 * time: 2014-4-16 16:16:38
		 */
		 renderFeePie: function(container, chartData){
		 	return appFeePie.renderTo(container, chartData);
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
		 * 渲染计费表格
		 * @param: type - "分时","平时"...
		 * @param: eneType - 电，水，气，暖...
		 * @param: Unit - 单位
		 * @param: json - 表格数据
		 * author: HF
		 * time: 2014-4-18 10:33:20
		 */
		renderList: function(type, eneType, Unit, json) {
			//var $tblHeaders = $("#table-fee .table-head");
			//var $fenshiHeader = $("#fenshi-tbl-header");
			//var $pingshiHeader = $("#pingshi-tbl-header");
            var $tblHeader = $("#fee-tbl-header");
			var templateId = "list-tpl";
			
			//首先根据type确定表格和表格模板
            /*
			switch(type){
				case "分时计费":
					$tblHeaders.hide();
					$fenshiHeader.show();
					templateId = "fenshi-list-tpl";
					break;
				case "平时计费":
					$tblHeaders.hide();
					$pingshiHeader.show();
					templateId = "pingshi-list-tpl";
					break;
				default:
					$tblHeaders.hide();
					templateId = "list-tpl";
			}
			*/

            //根据type确定表表头
             var theadArray = [];
             var theadHtml = "";
             switch(type){
                 case "分时计费":
                     theadArray = [
                         "时间",
                         eneType + "量总值<sub>(" + Unit + ")</sub>",
                         eneType + "费总值<sub>(元)</sub>",
                         "尖时" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "尖时" + eneType + "费<sub>(元)</sub>",
                         "峰时" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "峰时" + eneType + "费<sub>(元)</sub>",
                         "平时" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "平时" + eneType + "费<sub>(元)</sub>",
                         "谷时" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "谷时" + eneType + "费<sub>(元)</sub>"
                     ];
                 break;
                 case "平时计费":
                     theadArray = [
                         "时间",
                         eneType + "量总值<sub>(" + Unit + ")</sub>",
                         eneType + "费总值<sub>(元)</sub>"
                     ];
                 break;
                 case "阶梯计费":
                     theadArray = [
                         "时间",
                         eneType + "量总值<sub>(" + Unit + ")</sub>",
                         eneType + "费总值<sub>(元)</sub>",
                         "第一档" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "第一档" + eneType + "费<sub>(元)</sub>",
                         "第二档" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "第二档" + eneType + "费<sub>(元)</sub>",
                         "第三档" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "第三档" + eneType + "费<sub>(元)</sub>",
                         "第四档" + eneType + "量<sub>(" + Unit + ")</sub>",
                         "第四档" + eneType + "费<sub>(元)</sub>"
                     ];
                     break;
                 default :
                     return;
             }

            for(var i = 0; i < json.FeeList[0].length; i++){
                theadHtml += ("<td>" + theadArray[i] + "</td>");
            }
            $tblHeader.find("tr").html(theadHtml);
            $tblHeader.show();
			var $tbody = $('#table-fee tbody');
			var items = template(templateId, json);
			// 列表
			$tbody.html(items);
            //调整列表宽度
            var tdLength = json.FeeList[0].length + 1;
            var tdWidth = Math.ceil(100/tdLength) + "%";
            $tblHeader.find("td").width(tdWidth);
            $tbody.find("td").width(tdWidth);
		},

        /**
         * 填充计费方式
         * author: HF
         * time: 2014-4-19 16:08:42
         */
        setFeeType: function(json){
            $("#tool-bar").find(".fn>li").eq(0).children("a").html(json.FeeType);
        },
		
		/** 
		 * 填充及值分析
		 * author: HF
		 * time: 2014-4-18 13:31:42
		 */
		 fillFeeAnalyses: function(json){
		 	var compareVal = parseFloat(json.CompareLastMonth);
			var iconClass = (compareVal > 0) ? "up" : "down";
			var compareValTxt = isNaN(compareVal) ? "-" : Math.abs(compareVal) + "%";
			$("#fee-info-total").html(json.TotalVal);
			$("#fee-info-max").html(json.MaxVal);
			$("#fee-info-min").html(json.MinVal);
			$("#fee-info-avg").html(json.AvgVal);
			$("#fee-info-enertotal").html(json.TotalEnergy);
			$("#fee-info-enerUnit").html(json.EnergyUnit);
			$("#fee-info-enerpercent").html(compareValTxt);
			$("#energy").find("i").removeClass().addClass(iconClass).attr("data-original-title", "上月能耗值：" + json.EnergyLastMonth + json.EnergyUnit).tooltip();
			
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
                    $tiansu.common.loading('#table-fee', 'show');
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
                    $tiansu.common.loading('#table-fee', 'hide');
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
				tree: 'action.ashx?action=objectItemTree',
                areaTree: '/js/data/get_areaTree.js',
                select: 'action.ashx?action=indexItem',
                dataUrl: 'action.ashx?action=GetCostQuery',
                exportUrl: 'action.ashx?action=ExportExcelCostQuery'
			};
		}
	};

	return feeSearch;
});