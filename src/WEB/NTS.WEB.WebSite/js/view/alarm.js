/** 
 * ========================================================== 
 * Copyright (c) 2014, nts All rights reserved.
 * alarm management table
 * Author: luyy
 * Date: 2014-04-17 11:44:46 692000 
 * ========================================================== 
 */
 define(function() {
	function Alarm() {
		this.serverDate = $("#sys-time").html();//from 服务器的时间
		this.selectedLevel = "";//下拉框选择值
		this.selectedType = "";//下拉框选择值
		this.selectedStatus = "";//下拉框选择值
		//获取URL参数开始
		var $url = $tiansu.common.parseURL(window.location.href).params;
		this.urlItem = $url.item;
		this.urlDateType = $url.datetype;
		this.urlSdate = $url.sdate;
		this.urlEdate = $url.edate;
		this.urlObjType = $url.objtype;
		this.urlObjCode = $url.objcode;
        //Start: added by hf 2014-4-23 10:24:30
        this.urlAllAlarm = $url.allalarm;
        //End: added by hf 2014-4-23 10:25:04
            //获取URL参数结束
		this.init();
	}
	Alarm.prototype = {
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
			this.loadSelect();//加载下拉框
			if ($tiansu.common.parseURL(window.location.href).query.length > 0) {//URL有传值
				if (this.urlDateType) {//日期类型
					$(".fast-select a").removeClass("current").eq(this.urlDateType-1).addClass("current");
				} 
				this.setDate(this.urlDateType);
				$('input[name=startDate]').val(this.urlSdate);
				$('input[name=endDate]').val(this.urlEdate);
				switch (this.urlObjType){//双树TAB
					case 1:
					$("#tree-tab li").removeClass("current").eq(1).addClass("current");
					break;
					case 2:
					$("#tree-tab li").removeClass("current").eq(0).addClass("current");
					break;
				}
			} else {//URL没有传值
				this.setDate("3");
			}
			this.bindEvent.fastSelectDate.call(this);//日期快捷键				
			this.bindEvent.treeSelect.call(this);
			var $tree = this.getTree(2);
			this.bindEvent.initTreeSelected.call(this);//树的默认选项
			this.bindEvent.search.call(this);//查询按钮
			//this.initQuery();//页面初始化
			if($tree.tree('getRoot')) {
                this.initQuery();
            }else{
                $tiansu.common.info('show', {
                    timeout: 4000,
                    content: "请联系管理员设置您的权限。"
                });
            }
			this.bindEvent.exportClick.call(this);//导出
		},
		/** 
		 * Bind Events
		 * author: luyy
		 * time: 2014-04-15 21:49:06
		 */
		bindEvent: {
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
						var $tree = $treeContainer.children().eq(index).find("ul:first-child");
						if ($tree.html() == "") {
							switch (index){
								case 0:
								that.getTree(2);
									break;
								case 1:
								that.getTree(1);
									break;

							}
						}
						/*if(index == 1){
							var $tree = $('#areaTree-content ul:first-child');
							if($tree.html() == ""){
								that.getTree(1);
							}
						}*/
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
			 	var $tree = "",
			 		treeNodeId = "",
			 		selectedNode = "";
			 	if ($tiansu.common.parseURL(window.location.href).query.length > 0) {
			 		if (this.urlObjType) {
						switch (this.urlObjType){//双树TAB
							case "1":
								$tree = $('#areaTree-content ul:first-child');
							break;
							case "2":
								$tree = $('#tree-content ul:first-child');
							break;
						}
				 		treeNodeId = $tree.tree("find",this.urlObjCode);
				 		if (treeNodeId) {
				 			$(treeNodeId.target).addClass("tree-node-selected");
				 		}
				 		//查询父节点，展开
						/*function getParent(node){
							return $tree.tree("getParent", node.target)
						}
						var $treeNode = $tree.find(".tree-node-selected");
						var nodeSelected = $tree.tree('getNode',$treeNode);
						var pnode = getParent(nodeSelected);
						if (pnode) {
							$tree.tree('expand',pnode.target);
						}
						while(pnode !== null){
							pnode = getParent(pnode);
							if(pnode){
								$tree.tree('expand',pnode.target);
							}					
						}*/
			 		}
			 	} else {
			 		$node.addClass("tree-node-selected");			 		
			 	}
			},
			/** 
			 * export click
			 * author: luyy
			 * time: 2014-04-18 16:33:03
			 */
			exportClick: function(){
				var that = this,
					param = that.getParam();
				var flag = that.checkParam(param);
				$("#export-link a,#export-link i").on("click",function(){
					if (flag) {
						that.exportList(param);
					} else {
						return false;
					}
				});
			},				
			/**
			* 点击查询按钮触发查询事件
			* Author: pl
			* Time: 2014-3-24 09:48:58
			*/
			search: function(){
				var _this = this;
				$("#queryBtn a").click(function(e){
					_this.searchRender();
				});
			}
		},
		/** 
		 * 设置日期事件 
		 * author: ghj
		 * time: 2014-01-03 14:46:48 
		 */
		setDate: function(params) {
			var that = this;
			var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]');				
			var serverDate = this.serverDate.replace(/-/g, "/");
			$tiansu.date.datePeriod({
				type: params,
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
		 * search render table
		 * author: luyy
		 * time: 2014-04-18 14:59:41
		 */
		searchRender: function(){
			var that = this;
			var param;
			param = that.getParam();
			var flag = that.checkParam(param);
			if (that.selectedLevel) {
				param.AlarmLevel = that.selectedLevel;
			}
			if (that.selectedType) {
				param.AlarmType = that.selectedType;
			}
			if (that.selectedStatus) {
				param.AlarmStatus = that.selectedStatus;
			}
			if(flag){
				$("#alarmManagement h3 span.series-date").html(param.dateString);
				var treeTypeVal = param.ObjType;
				var treeElem = "#tree-content";
				if(treeTypeVal == 1){
					treeElem = "#areaTree-content";
				}else{
					treeElem = "#tree-content";
				}
				var breadcrumb = that.getTreeNodePath(treeElem);
				$("#alarmManagement h3 span.areaName").html(breadcrumb.shortPath).attr("title", breadcrumb.fullPath);
				//告警查询表单渲染
				that.getAlarmNote(param);
			}else{
				return false;
			}			
		},
		/** 
		 * 页面初始化查询
		 * author: luyy
		 * time: 2014-04-18 15:12:08
		 */
 		initQuery: function() {
			var _this = this;
			var param = this.getParam();
			$("#alarmManagement h3 span.series-date").html(param.dateString);
			var breadcrumb = this.getTreeNodePath("#tree-content");
			$("#alarmManagement h3 span.areaName").html(breadcrumb.shortPath).attr(breadcrumb.fullPath);
			//告警查询表单渲染
			_this.getAlarmNote(param);			
       	},
		/** 
		 * 告警类型,载入下拉项
		 * author: luyy
		 * time: 2014-04-17 16:20:46
		 */
		loadSelect: function(){
			var that = this,
			 	$selectClassDiv = $(".alarm-select-class"),
			 	$selectItemDiv = $(".alarm-select-item"),
			 	$selectStatusDiv = $(".alarm-select-status"),
		 		$selectItem = "",
		 		$selectClass = "",
		 		$selectStatus = "",
		 		$alarmSelect = "",
		 		selectClass = [{"Value":"0001","Name":"1"
		 		},{"Value":"0002","Name":"2"
				},{"Value":"0003","Name":"3"
				},{"Value":"0004","Name":"4"
				},{"Value":"0005","Name":"5"
				},{"Value":"0006","Name":"6"
				},{"Value":"0007","Name":"7"
				},{"Value":"0008","Name":"8"
				},{"Value":"0009","Name":"9"
				},{"Value":"0010","Name":"10"
		 		}],
		 		selectStatus = [{"Value":"0","Name":"正在告警"
		 		},{"Value":"2","Name":"已恢复"
				},{"Value":"3","Name":"已处理"
				},{"Value":"4","Name":"已取消"
		 		}];
		 		//加载等级状态下拉
		 		$selectClassDiv.append('<select name="type-class" class="w100"></select>');
				$selectItemDiv.append('<select name="type-item" class="w162"></select>');
		 		$selectStatusDiv.append('<select name="type-status" class="w100"></select>');
				$selectClass = $("select[name=type-class]");
		 		$selectStatus = $("select[name=type-status]");
				$selectItem = $("select[name=type-item]");
				$alarmSelect = $("select[name=type-class], select[name=type-item], select[name=type-status]");
		 		if (selectClass.length > 0) {
		 			$selectClass.html('<option></option>');
		 			$.each(selectClass,function(i,item){
		 				$selectClass.append('<option value='+item.Value+'>'+item.Name+'</option>');
		 			});
		 			$selectClass.append('<option value="">全部等级</option>');
		 		}
		 		if (selectStatus.length > 0) {
		 			$selectStatus.html('<option></option>');
		 			$.each(selectStatus,function(i,item){
		 				$selectStatus.append('<option value='+item.Value+'>'+item.Name+'</option>');
		 			});
		 			$selectStatus.append('<option value="">全部状态</option>');
		 		}
			 	$.ajax({
					url: that.getAction().select,
					dataType: "json",
					type: 'GET',
					async: false,
					success: function(json){
						//加载类型下拉
						if(json.ItemLst && json.ItemLst.length > 0){
							$selectItem.html('<option></option>')
							$.each(json.ItemLst,function(i,item){
								$selectItem.append('<option value='+item.ItemCode+'>'+item.ItemName+'</option>');
							});
							$selectItem.append('<option value="">全部类型</option>');						
						}
					},
					error: function( error){
						console.log(error);
					}
				});
				$selectClass.select2({
					placeholder: '等级',
					minimumResultsForSearch: -1,
					formatNoMatches: function () {
					 return "没有数据！"; 
					}
				});
				if (that.urlItem) {
					$(".alarm-select-item select option[value="+that.urlItem+"]").attr("selected","selected");
					$selectItem.select2({
						minimumResultsForSearch: -1,
						formatNoMatches: function () {
						 return "没有数据！"; 
						}
					});
				} else {
					$selectItem.select2({
						placeholder: '告警类型',
						minimumResultsForSearch: -1,
						formatNoMatches: function () {
						 return "没有数据！"; 
						}
					});
				}
				$selectStatus.select2({
					placeholder: '告警状态',
					minimumResultsForSearch: -1,
					formatNoMatches: function () {
					 return "没有数据！"; 
					}
				});
				$selectClass.on("change",function(){
					that.selectedLevel = $(this).find("option:selected").attr("value");
					that.searchRender();		
				});
				$selectItem.on("change",function(){
					that.selectedType = $(this).find("option:selected").attr("value");
					that.searchRender();		
				});
				$selectStatus.on("change",function(){
					that.selectedStatus = $(this).find("option:selected").attr("value");
					that.searchRender();		
				});
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
			var objCode = "";
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
		 * 目录树的节点路径
		 * author: luyy
		 * time: 2014-04-16 13:33:44
		 */
        /*
		getTreeNodePath: function(elem){
			$tree = $(elem).find("ul:first-child");
			var $treeNode = $tree.find(".tree-node-selected");
			var nodeSelected = $tree.tree('getNode',$treeNode);
			var pnode = getParentNode(nodeSelected);
			var pathArry = [];
            if(nodeSelected) {
                pathArry.push(nodeSelected.text);
            }
			while(pnode !== null){
				pathArry.push(pnode.text);
				pnode = getParentNode(pnode);
			}
			var path = pathArry.reverse().join("　");
			return path;
			
			function getParentNode(node){
                if(node)
				    return $tree.tree("getParent", node.target);
                else
                    return null;
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
            if(nodeSelected) {
                pathArry.push(nodeSelected.text);
            }
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
                //return $tree.tree("getParent", node.target)
                if(node)
                    return $tree.tree("getParent", node.target);
                else
                    return null;
            }
        },

		/**
		 * 获取查询参数
		 * author: pl
		 * time: 2014-3-23 21:14:02
		 */
		getParam: function() {
		 	var $startDate = $('input[name=startDate]'),
				$endDate = $('input[name=endDate]'),
				that = this;
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
			var param = {};
			//计算时间颗粒度
			param.Particle = $tiansu.common.getParticle($startDate.val(),$endDate.val());
			param.StartTime = $startDate.val();
			param.EndTime = $endDate.val();
			param.dateString = "(" + param.StartTime + " — " + param.EndTime + ")";
			if(nodeSelected){
				param.ObjectId = nodeSelected.id;			
			} else {
				param.ObjectId = that.urlObjCode;
			}
			param.ObjType = objType;
			param.PageIndex = 1;//当前页
			param.PageSize = 18; //页面几行
			
			//下拉框
			param.AlarmLevel = $("select[name=type-class] option:selected").attr("value");
			if (!param.AlarmLevel) {
				param.AlarmLevel = "";
			}
			param.AlarmType = $("select[name=type-item] option:selected").attr("value");
			if (!param.AlarmType ) {
				param.AlarmType = "";
			}
			param.AlarmStatus = $("select[name=type-status] option:selected").attr("value");
			if (!param.AlarmStatus) {
				param.AlarmStatus = "";
			}
            //Start: added by hf 2014-4-23 10:28:07
            if(this.urlAllAlarm){
                param.AllAlarm = this.urlAllAlarm;
            } else {
            	param.AllAlarm = false;
            }

            //End: added by hf 2014-4-23 10:28:20
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
		 * alarm note
		 * @param  {obj} data [从后台获取的接口]
		 * author: luyy
		 * time: 2014-04-17 21:01:28
		 */
		getAlarmNote: function(params) {
			var self = this,
				inputs = params || {},
				$alarmList = $(".alarmList table tbody"),
				html = "";
				inputs = JSON.stringify(inputs);
			$.ajax({
				url: self.getAction().getAlarmInfoUrl,
				dataType: "json",
				type: "POST",
				data: {
					inputs: inputs || {}
				},
				beforeSend: function(){
					$tiansu.common.loading('.alarmNote','show');
				},				
				success: function(json) {
					if(json.ActionInfo.Success){
						self.showRanking(json);
						$tiansu.common.page(json, function(page) {
							params.PageIndex = page;
							self.goPage(params);
						});
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
						content: '数据请求失败'
					});
				},
				complete: function(){
					$tiansu.common.loading('.alarmNote','hide');
				}
			});
		},
		/** 
		 * 显示告警表
		 * author: ghj
		 * time: 2013-06-17 22:14:56
		 */
		showRanking: function(json) {
			var $alarmList = $(".alarmList table tbody"),
				html = "";			
				if(json.data.length > 0){
					html = template.render("alarmNote", json);
					$alarmList.html(html);
					$alarmList.children().fadeIn(400);
				}else{
					$alarmList.html('');//数据库空
					$tiansu.common.info('show', {
						timeout: 1000,
						content: '暂无数据信息'
					});	
				}
		},
		/** 
		 * 翻页请求
		 * @param {num} [page] [翻页请求参数]
		 * author: ghj
		 * time: 2013-06-16 22:23:48
		 */
		goPage: function(params) {
			var self = this,
				inputs = params || {};
				inputs = JSON.stringify(inputs);
			$.ajax({
				url: this.getAction().getAlarmInfoUrl,
				dataType: "json",
				type: "post",
				data: {
					inputs: inputs || {}
				},
				success: function(json) {					
					self.showRanking(json);
					$tiansu.common.page(json, function(page) {
						params.PageIndex = page;
						self.goPage(params);
					});
				},
				error: function() {
					alert("服务器异常，请联系管理员！");
				}
			});
		},
		/** 
		 * export table
		 * author: luyy
		 * time: 2014-04-18 16:25:42
		 */
		exportList: function(param) {
			var _this = this,
			    inputs = param || {};
			    inputs = JSON.stringify(inputs);
			$.ajax({
				url: _this.getAction().getAlarmInfoUrl,
				dataType: "json",
				type: "POST",
				data: {
					inputs: inputs || {}
				},
                beforeSend: function() {
                    $tiansu.common.loading('.alarmNote tbody', 'show');
                },
				success: function(json) {
					if (json.ActionInfo.Success) {
						window.open(json.ActionInfo.ExceptionMsg);
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
						content: '数据请求失败'
					});
				},
                complete: function() {
                    $tiansu.common.loading('.alarmNote tbody', 'hide');
                }
			});
		},
		/** 
		 * Ajax request
		 * author: luyy
		 * time: 2014-04-16 09:15:46
		 */		
		getAction: function() {
			return {
				tree: "action.ashx?action=objectItemTree",
			    select: 'action.ashx?action=GetAlarmType',  //告警类型
			    getAlarmInfoUrl: 'action.ashx?action=GetAlarmList'
			};
		}
	}
	return Alarm;
});