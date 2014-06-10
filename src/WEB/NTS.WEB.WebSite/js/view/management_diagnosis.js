/** 
 * ========================================================== 
 * Copyright (c) 2014, nts All rights reserved.
 * management diagnosis
 * Author: luyy
 * Date: 2014-04-15 21:17:03 316000 
 * ========================================================== 
 */
define(function() {
	function Diagnosis() {
		this.serverDate = $("#sys-time").html();
		this.init();
	}
	Diagnosis.prototype = {
		/** 
		 * Initialize page
		 * author: luyy
		 * time: 2014-04-15 21:25:13
		 */
		init: function() {
			this.render();
		},
		/** 
		 * Render page
		 * author: luyy
		 * time: 2014-04-15 21:26:52
		 */
		render: function() {			
			this.bindEvent.setDate.call(this);
			this.bindEvent.fastSelectDate.call(this);
			this.bindEvent.treeSelect.call(this);
			var $tree = this.getTree(2);
			this.bindEvent.initTreeSelected.call(this);
			this.bindEvent.search.call(this);
			this.setScroll();
			if($tree.tree('getRoot')) {
                this.initQuery();
            }else{
                $tiansu.common.info('show', {
                    timeout: 4000,
                    content: "请联系管理员设置您的权限。"
                });
            }
		},
		/** 
		 * Bind Events
		 * author: luyy
		 * time: 2014-04-15 21:49:06
		 */
		bindEvent: {
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
				$tiansu.date.datePeriod({
					type: "3",
					connect: "-",
					date: that.serverDate,
					endDate: that.serverDate,
					sDate: $startDate,
					eDate: $endDate
				});
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
						$treeContainer.children().hide().eq(index).show();
						$treeContainer.children(".loading").show();
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
			 * 初始化选中的树节点 
			 * author: pl
			 * time: 2014-3-24 20:17:36
			 */
			initTreeSelected: function(){
			 	//var $node = $("#tree-content ul li").eq(0).find("ul li").eq(0).find("ul li").eq(0).children(".tree-node");
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
				var param;
				$("#queryBtn a").click(function(e){
					param = that.getParam();
					var flag = that.checkParam(param);
					if(flag){
						$("#chartsAnalyses h3 span.series-date").html(param.dateString);
						var treeTypeVal = param.ObjType;
						var treeElem = "#tree-content";
						if(treeTypeVal == 1){
							treeElem = "#areaTree-content";
						}else{
							treeElem = "#tree-content";
						}
						var breadcrumb = that.getTreeNodePath(treeElem);
						$("#chartsAnalyses h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
						//分类分项异常渲染
						that.getDiagnosisTable(param);
					}else{
						return false;
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
			return $tree;
		},
		/** 
		 * 目录树的节点路径
		 * author: luyy
		 * time: 2014-04-16 13:33:44
		 */
         /*
		getTreeNodePath: function(elem){
			$tree = $(elem).find("ul:first-child");
			var $treeNode = $(elem).find(".tree-node-selected");
			var nodeSelected = $tree.tree('getNode',$treeNode);
			var pnode = getParentNode(nodeSelected);
			var pathArry = [];
			pathArry.push(nodeSelected.text);
			while(pnode !== null){
				pathArry.push(pnode.text);
				pnode = getParentNode(pnode);
			}
			var path = pathArry.reverse().join("　");
			return path;
			
			function getParentNode(node){
				return $tree.tree("getParent", node.target)
			}
		},
		*/
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
            //保留30个字及最后一个节点
            if(path.length > 30){
                var theLastNodeTextIndex = path.lastIndexOf("　");
                var theLastNodeText = path.slice(theLastNodeTextIndex + 1);
                var theLastNodeTextLength = theLastNodeText.length;
                if(theLastNodeTextLength < 30) {
                    var theRestLength = 30 - theLastNodeTextLength;
                    var theRestString = path.substring(0, theRestLength);
                    path = theRestString + "..." + theLastNodeText;
                }else{
                    path = theLastNodeText.substring(0, 30) + "...";
                }
            }
            pathInfo.shortPath = path;
            return pathInfo;

            function getParentNode(node){
                return $tree.tree("getParent", node.target)
            }
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
			if($("#tree-content").is(":visible")){
				var $tree = $("#tree-content ul"),
				    nodeSelected = $tree.tree('getNode',"#tree-content .tree-node-selected"),
				    objType = 2;//业态功能
			}else{
				var $tree = $("#areaTree-content ul"),
					nodeSelected = $tree.tree('getNode',"#areaTree-content .tree-node-selected"),
					objType = 1;//区域位置
			}
			var param = {},
				children = [];
			//计算时间颗粒度
			param.particle = $tiansu.common.getParticle($startDate.val(),$endDate.val());
			param.StartTime = $startDate.val();
			param.EndTime = $endDate.val();
			param.dateString = "(" + param.StartTime + " — " + param.EndTime + ")";
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
			//param.QueryType = 1;
			//param.IsDevice = 0;
			//param.IsDetail = 0;
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
         * 设置滚动条
         * author: ghj
         * time: 2013-06-10 22:48:58
         */
        setScroll: function() {
            var $tree = $(".itemAbnormal");
            $tree.jScrollPane({
                autoReinitialise: true,
                mouseWheelSpeed: 20
            });
        },
        /** 
         * 页面初始化查询
         * author: luyy
         * time: 2014-04-16 22:11:05
         */
 		initQuery: function() {
			var _this = this;
			var param = this.getParam();
			$("#chartsAnalyses h3 span.series-date").html(param.dateString);
			var breadcrumb = this.getTreeNodePath("#tree-content");
			$("#chartsAnalyses h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
			//分类分项异常渲染
			_this.getDiagnosisTable(param);			
       	},
        /** 
    	 * diagnosis abnormal table
    	 * author: luyy
    	 * time: 2014-04-16 14:34:27
    	 */
		getDiagnosisTable: function(param) {
		 	var _this = this;
			var objParam = param;			
			var param = JSON.stringify(param),
				$electAbnormal = $("#electAbnormal"),
				$waterAbnormal = $("#waterAbnormal"),
				$airConditionerAbnormal = $("#airConditionerAbnormal"),
				$gasAbnormal = $("#gasAbnormal"),
				html = "";
			$.ajax({
				url:this.getAction().diagnosis,
				dataType: 'json',
				type: 'POST',
				data: {inputs:param || {}},
				beforeSend: function(){
					$tiansu.common.loading('#electAbnormal','show');
					$tiansu.common.loading('#waterAbnormal','show');
					$tiansu.common.loading('#airConditionerAbnormal','show');
					$tiansu.common.loading('#gasAbnormal','show');
				},
				success: function(json) {
					$electAbnormal.html('');
					$waterAbnormal.html('');
					$airConditionerAbnormal.html('');
					$gasAbnormal.html('');
					$(".totalValue").html("");
					if(json.ActionInfo.Success){
						if(json.Rows.length > 0){
							//分项异常							
							_this.anormalRender(1,6,$electAbnormal,json);
							_this.anormalRender(6,8,$waterAbnormal,json);
							_this.anormalRender(8,9,$airConditionerAbnormal,json);
							_this.anormalRender(9,10,$gasAbnormal,json);
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
					$tiansu.common.loading('#electAbnormal','hide');
					$tiansu.common.loading('#waterAbnormal','hide');
					$tiansu.common.loading('#airConditionerAbnormal','hide');
					$tiansu.common.loading('#gasAbnormal','hide');
				}
			});
		},
		/** 
		 * trim array null
		 * author: luyy
		 * time: 2014-04-16 20:46:15
		 */
		arrayTrim: function(array) {
			for(var i = 0 ;i<array.length;i++)
			 {
	             if(array[i] == "" || typeof(array[i]) == "undefined")
	             {
	                      array.splice(i,1);
	                      i= i-1;
	             }
			 }
		},
		/** 
		 * anormal template render
		 * author: luyy
		 * time: 2014-04-16 21:45:13
		 */
		anormalRender: function(a,b,flag,json) {
			var abnormalItem = [{
		               "Value":"1001",
		               "Text":"电能耗突增"
		          },
		          {
		               "Value":"1002",
		               "Text":"电平衡异常"
		          },
		          {
		               "Value":"1003",
		               "Text":"过负荷"
		          },
		          {
		               "Value":"1004",
		               "Text":"电压/电流不平衡度"
		          },
		          {
		               "Value":"1005",
		               "Text":"电压/电流畸变"
		          },
		          {
		               "Value":"1006",
		               "Text":"水能耗突增"
		          },
		          {
		               "Value":"1007",
		               "Text":"水平衡异常"
		          },
		          {
		               "Value":"1008",
		               "Text":"空调能耗突增"
		          },
		          {
		               "Value":"1009",
		               "Text":"气能耗突增"
		          }
				];
			var getAbnormalRender = {
				"Row": []
			}
			var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]'),
				dateType = 0,
				sdate = $startDate.val(),
				edate = $endDate.val(),
				objType = "",
				objCode = this.getParam().ObjectId;
			var totalValue = 0;
			if ($(".fast-select a").hasClass("current")) {
				dateType = 	$(".fast-select a.current").index()+1;			
			}
			if ($("#tree-content").is(":visible")) {
				objType = 2;
			} else {
				objType = 1;
			}
			//objCode = $(".tree-title",$(".tree-node-selected")).attr("conf");
			for (var i = a-1; i < b-1; i ++) {
				var anomaly = "",
					abnormalValue = "";
					getAbnormalRender.Row[i] = {};
				getAbnormalRender.Row[i].Item = abnormalItem[i].Value;
				getAbnormalRender.Row[i].Text = abnormalItem[i].Text;
				for (var j = 0; j < json.Rows.length; j ++) {
					if(abnormalItem[i].Value == json.Rows[j].Item){
						anomaly = json.Rows[j].Anomaly;
						abnormalValue = json.Rows[j].AbnormalValue;
						totalValue += parseInt(abnormalValue);
					}
				}
				if(anomaly != null && abnormalValue != null){
				getAbnormalRender.Row[i].Anomaly = anomaly;
				getAbnormalRender.Row[i].AbnormalValue = abnormalValue;
				}
				if(anomaly == ""){
					getAbnormalRender.Row[i].Anomaly = "0";
				}
				if(abnormalValue == ""){
					getAbnormalRender.Row[i].AbnormalValue = "0";
				}
				getAbnormalRender.Row[i].DateType = dateType;
				getAbnormalRender.Row[i].Sdate = sdate;
				getAbnormalRender.Row[i].Edate = edate;
				getAbnormalRender.Row[i].ObjType = objType;
				getAbnormalRender.Row[i].ObjCode = objCode;
			}
			this.arrayTrim(getAbnormalRender.Row);
			html = template("abnormalRender", getAbnormalRender);
			flag.html(html);
			flag.children().fadeIn(400);
			flag.parent().parent().parent().find(".totalValue").html(totalValue);
		},
		/** 
		 * Ajax request
		 * author: luyy
		 * time: 2014-04-16 09:15:46
		 */		
		getAction: function() {
			return {
				tree: 'action.ashx?action=objectItemTree',
			    diagnosis: 'action.ashx?action=GetAlarmDiagnose'
			};
		}		
	}
	return Diagnosis;
});