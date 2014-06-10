define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			xAxis: {
				type:'datetime',
				labels:{}
			},
			yAxis: {
				//tickPixelInterval: 50,
				labels: {
					format: '{value}',
					step: 1
				}
			},
			plotOptions: {
				series:{
					marker:{
						symbol:"circle",
						enabled:false
					},
					lineWidth:1,
					events: {
						mouseOver: function(){
							var index = this.index;
							var seriesArray = this.chart.series;
							for (var i = 0; i < seriesArray.length; i++) {
								seriesArray[i].update({
									marker: {
										enabled: false
									},
									lineWidth: 1
								})
							}
		
							this.update({
								marker: {
									enabled: true,
									radius: 4
								},
								lineWidth: 4
							});
							
							//更新图例面板当前样式
							$("#chart-contrast-panel li>a").removeClass("current").eq(index).addClass("current");
						}
					},
					states:{
						hover:{
							marker: {
								enabled: true,
								radius: 4
							},
							lineWidth: 4
						}
					}
				}
			},
			tooltip: {
			}
		},
		
		renderTo: function(container, chartData, startTime, endTime, callback){
			//判断多对象还是多时间
			var compareType = chartData.CompareType;
			if(compareType === "object"){
				this.opts.xAxis.type = 'datetime';
				this.opts.tooltip.headerFormat = '<span style="font-size: 10px">{point.key}</span><br/>';
			}else{
				this.opts.xAxis.type = 'linear';
				this.opts.tooltip.headerFormat = '';
			}

			var opts = this.opts;
			return charts.renderChart(opts, chartData, container, startTime, endTime, callback);
		},

		//生成图例面板
		setPanel: function(chart, container, clip){
			var seriesArray = chart.series;
			var itemList = "<ul class='clearfix'>";
			for(var elem in seriesArray){
				var elemName = seriesArray[elem].name;
				var title = "";
				if(clip && elemName.length > 5){
					elemName = elemName.substring(0,5) + "...";
					title = " title='" + seriesArray[elem].name + "'";
				}
				
				//对对象对比时，图例面板带上树节点的ID
				var treeId = "";
				//console.log(seriesArray[elem]);
				if(seriesArray[elem].userOptions.id){
					treeId = seriesArray[elem].userOptions.id;
				}
				
				if(elem == 0){
					itemList += "<li data-treeid='" + treeId + "'><a class='current' " + title + " href=''><span class='color' style='background:" + seriesArray[elem].color + ";'></span><span class='series-name'>" + elemName + "</span><span class='series-close'> X</span></a></li>";
				}else{
					itemList += "<li data-treeid='" + treeId + "'><a " + title + " href=''><span class='color' style='background:" + seriesArray[elem].color + ";'></span><span class='series-name'>" + elemName + "</span><span class='series-close'> X</span></a></li>";
				}
			}
			itemList += "</ul>";
			if(seriesArray.length > 0){
				itemList += "<div class='removeAll'><a href=''>remove all</a></div>";
			}
			$(container).html(itemList);
			
		}
	}
});