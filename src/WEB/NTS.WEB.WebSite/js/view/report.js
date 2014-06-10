/**
 * ==========================================================
 * Copyright (c) 2014, tiansu-china.com All rights reserved.
 * 能耗排名/详情JS
 * Author: Jinsam
 * Date:2014-3-24 08:56:08
 * ==========================================================
 */

define(['chartsmod/charts', 'chartsmod/appNHpm', 'chartsmod/appFenjie'],function(charts, appNHpm, appFenjie) {

	function Report() {
		this.serverDate = $("#serverDate").val();
		this.isQuering = false;
		this.init();
	}

	Report.prototype = {

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
			this.bindEvent.setType.call(this);
			this.bindEvent.setDate.call(this);
			this.setDate();
			this.getTree(2);						
			this.reportType.call(this);
			this.bindEvent.preview.call(this);
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
				var $treeContainer = $("#tree-container");
				if ($selector.size() > 0 || $treeContainer.size() > 0) {
					$("#tree-tab>li").click(function() {
						var index = $(this).index();
						$(this).addClass("current").siblings().removeClass("current");
						$treeContainer.children(".loading").show();
						$treeContainer.children().hide().eq(index).show();
						if(index == 1){
							var $tree = $('#areaTree-content ul:first-child');
							if($tree.html() == ""){
								that.getTree(1);
							}
						}
					});
				}
			},
		/** 
		 * 设置报表类型滚动条
		 * author: ghj
		 * time: 2013-07-18 01:00:12
		 */
		setReportSortScroll: function() {
			$("#step-type").jScrollPane({
				autoReinitialise: true,
				mouseWheelSpeed: 20
			});
		},
			/** 
			 * 设置时间事件
			 * author: ghj
			 * time: 2013-08-11 21:25:53
			 */
			setDate: function() {
				var $stepDate = $(".step-date"),
					$input = $("input", $stepDate);
				// hover
				$stepDate.hover(function() {
					$(this).addClass("step-date-hover").siblings().removeClass("step-date-hover");
				}, function() {
					$(this).removeClass("step-date-hover");
				});
				// click
				$stepDate.click(function() {
					$(this).addClass("current").siblings().removeClass("current");
				});
				// blur
				$input.blur(function() {
					var val = $.trim($(this).val());
					if (val !== "") {
						$(this).parents("dl").siblings().find("input").val("");
					}
				});
			},
			/** 
			 * 选择报表类型事件
			 * author: ghj
			 * time: 2013-08-11 22:49:03
			 */
			setType: function() {
				var $li = $(".step-type li");

				$li.click(function() {
					if ($(this).hasClass("current")) {
						$(this).removeClass("current");
					} else {
						$(this).addClass("current").siblings().removeClass("current");
					}
				});
			},

			/** 
			 * 导出事件
			 * author: pl
			 * time: 2014-3-29 10:09:29
			 */
			preview: function() {
				var self = this;
				var $btn = $(".export-excel"),
					data = {},
					date = "",
					typeId = "";
				$btn.click(function() {				
					// 报表对象id
					var secondItemCode = $("#area-select").children().eq(1).find("select[name=item-diff]").val();
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
					var nodeSelected = $tree.tree('getNode', $treeNode);
					if (nodeSelected != null) {
						var AreaId = nodeSelected.id;
					}

					 // 报表时间
                var $inputs = $(".step2 input");
                var date = "";
                var attrName = "";
                for (var j = 0; j < $inputs.length; j++) {
                    var val = $.trim($($inputs[j]).val());
                    if (val !== "") {
                        date = val;
                        attrName = $($inputs[j]).attr("name");
                        break;
                    }
                }
                switch (attrName) {
                    case "date-day":
                        Starttime = date;
                        Endtime = date;
                        Timeunit = 1;
                        break;
                    case "date-month":
                        Starttime = date.substring(0, date.indexOf("~"));
                        Endtime = date.substring(date.indexOf("~") + 1);
                        Timeunit = 2;
                        break;
					case "date-quarter":
						Starttime = date.substring(0, date.indexOf("~"));
						Endtime = date.substring(date.indexOf("~") + 1);
						Timeunit = 2;
						break;
                    case "date-year":
                        Starttime = date.substring(0, date.indexOf("~"));
                        Endtime = date.substring(date.indexOf("~") + 1);
                        Timeunit = 3;
                        break;
                    default:
                        break;
                }

					// 报表类型
					var $type = $(".step-type .current");

					Counttype = $type.attr("data-id");
					Reporttype = $type.attr("report-id");

					// 判断
					if ($treeNode.length === 0) {
						$tiansu.common.info('show', {
							content: "请选择1个报表对象",
							css: {
								position: "absolute",
								top: $(this).offset().top - 50,
								left: $(this).offset().left
							},
							timeout: 2000
						});
					} else if (date === "") {
						$tiansu.common.info('show', {
							content: "请填写报表时间",
							css: {
								position: "absolute",
								top: $(this).offset().top - 50,
								left: $(this).offset().left
							},
							timeout: 2000
						});
					} else if ($type.length === 0) {
						$tiansu.common.info('show', {
								content: "请至少选择1个报表类型",
								css: {
									position: "absolute",
									top: $(this).offset().top - 50,
									left: $(this).offset().left
								},
								timeout: 2000
						});						
					} else {
						param = {
							"AreaId": AreaId,
							"StartTime": Starttime,
							"EndTime": Endtime,
							"TimeUnit": Timeunit,
							"ReportType": Reporttype,
							"itemCode": Counttype
						};
						self.exportexcel(param);
					}
				});
			}
		},		
			

		/**
		 * 设置目录树
		 * author: ghj
		 * time: 2014-01-17 00:51:14
		 */
		getTree: function(type) {
			var that = this;
			var $tree = null;
			var $treeContent = null;
			var is_single = true;
			if(type === 2){
				$tree = $('#tree-content ul:first-child');
				$treeContent = $('#tree-content');
			}else if(type === 1){
				$tree = $('#areaTree-content ul:first-child');
				$treeContent = $('#areaTree-content');
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
		},	
		/** 
		 * 设置报表时间
		 * author: pl
		 * time: 2014-3-29 10:08:15
		 */
		setDate: function() {
			var edate = $("#sys-time").html();
			var $day = $("input[name='date-day']"),
				$month = $("input[name='date-month']"),
				$quarter = $("input[name='date-quarter']"),
				$year = $("input[name='date-year']");

			$day.datepicker({
				format: "yyyy-mm-dd",
				weekStart: 1,
				endDate: edate,
				autoclose: true,
				minViewMode: 0,
				todayBtn: "linked",
				todayHighlight: false,
				keyboardNavigation: false
			}).on("changeDate", function() {
				$day.parents("dl").siblings().find("input").val("");
			});

			$month.datepicker({
				format: "yyyy-mm-dd",
				weekStart: 1,
				endDate: edate,
				autoclose: true,
				minViewMode: 0,
				todayBtn: "linked",
				todayHighlight: false,
				keyboardNavigation: false,
				stepDate: {
					type: "month",
					callback: function(startDate, endDate) {
						$month.datepicker('hide');
						$month.val(startDate + ' ~ ' + endDate);
						$month.parents("dl").siblings().find("input").val("");
					}
				}
			});

			$quarter.datepicker({
				format: "yyyy-mm-dd",
				weekStart: 1,
				endDate: edate,
				autoclose: true,
				minViewMode: 0,
				todayBtn: "linked",
				todayHighlight: false,
				keyboardNavigation: false,
				stepDate: {
					type: "quarter",
					callback: function(startDate, endDate) {
						$quarter.datepicker('hide');
						$quarter.val(startDate + ' ~ ' + endDate);
						$quarter.parents("dl").siblings().find("input").val("");
					}
				}
			});
			$year.datepicker({
				format: "yyyy-mm-dd",
				weekStart: 1,
				endDate: edate,
				autoclose: true,
				minViewMode: 2,
				todayBtn: "linked",
				todayHighlight: false,
				keyboardNavigation: false,
			});/*.on("changeDate",function(){
				start = $year.val();alert(start);
				end = start.replace("01-01",("12-31"));	alert(end);			
				$year.parents("dl").siblings().find("input").val("");
			});
			alert(end);
			$year.val(start + ' ~ ' + end);alert($year.val());*/
			$year.change(function(){
				var start = $year.val();
				var end = start.replace("01-01",("12-31"));	
				$year.val(start + ' ~ ' + end);		
				$year.parents("dl").siblings().find("input").val("");
			});
		},	

		/** 
		 * 加载报表类型
		 * @param  {obj} data [所选参数]
		 * author: ghj
		 * time: 2013-08-11 22:54:05
		 */
		reportType: function() {
			var $list = $("ul.step-type"),
				html = "";console.log($list);
			$.ajax({
				url: this.getAction().getReportTypeUrl,
				dataType: "json",
				type: "post",
				//data: data,
				beforeSend: function(){
					$tiansu.common.loading('.step3 step-body','show');
				},
				success: function(json) {
					if (json.ActionInfo[0].Success) {						
						$list.empty();
						for (var i=0; i<json.Data.length; i++) {
                            html += "<li report-id=" + json.Data[i].Reporttype + " data-id=" + json.Data[i].Counttype + ">" + json.Data[i].Reportname + "</li>";
						}
						$list.html(html);
						window.report.bindEvent.setType.call(this);
						window.report.bindEvent.setReportSortScroll();
					} else {
						$list.html(json.Msg);
					}
				},
				error: function() {
					$tiansu.common.info('show', {
						content: "数据请求失败，请联系管理员！",
						css: {
							position: "absolute",
							top: $(this).offset().top + 120,
							left: $(this).offset().left+ 48
						},
						timeout: 2000
					});
				},
				complete: function(){
					$tiansu.common.loading('.step3 step-body','hide');
				}
			});
		},
		
        /**
        * 导出报表
        * @param  {obj} data [所选参数]
        * author: PL
        * time: 2014-3-29 11:31:42
        */
        exportexcel: function (data) {
            $.ajax({
                url: this.getAction().exportUrl,
                dataType: "json",
                type: "POST",
                data: data,
                async: false,
                beforeSend: function(){
					$tiansu.common.loading('#main','show');
				},
                success: function (json) {
                    if (json.status == 'success') {
                        window.open(json.msg);
                    } else {
                        $tiansu.common.info('show', {
						content: "没有数据！",
						timeout: 2000
					});
                    }
                },
                error: function (json) {
                       $tiansu.common.info('show', {
						content: "数据请求失败，请联系管理员！",
						css: {
							position: "absolute",
							top: $(this).offset().top + 120,
							left: $(this).offset().left+ 48
						},
						timeout: 2000
					});
                },
                complete: function(){
					$tiansu.common.loading('#main','hide');
				}
            });
        },


/**
        * Ajax request
        * author: pl
        * time: 2014-3-29 10:08:30
        */
        getAction: function () {
            return {
                tree: "action.ashx?action=objectItemTree",
                select: 'action.ashx?action=indexItem',
                getReportTypeUrl: "action.ashx?action=Complex_GetReportType",
                exportUrl: "action.ashx?action=ReportExcel"
            };
        }
	};

	return Report;
});