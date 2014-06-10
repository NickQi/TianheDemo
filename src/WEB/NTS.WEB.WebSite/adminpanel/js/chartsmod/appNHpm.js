define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'bar'
			},
			xAxis: {
				tickInterval:1,
				labels:{
					step:1
				}
			},
			yAxis: {
				labels: {
					format: '{value}'
					//step: 1
				}
			}, 
			plotOptions: {
				series:{
				}
			},
            tooltip:{
                positioner:null
            }
		},
		
		renderTo: function(container, chartData, reversed){
			var opts = this.opts;
			//设置是否倒序，因为要同时反转颜色数组
			if(reversed){
				opts.reversed = true;
			}else{
				opts.reversed = false;
			}

			return charts.renderChart(opts, chartData, container);
		},

		setPanel: function(chart, container, clip){
			var dataArray = chart.series[0].data;
			var itemList = "<ul class='clearfix'>";
			for(var elem in dataArray){
				var elemName = dataArray[elem].name;
				var title = "";
				//截取前5位
				if(clip && elemName.length > 5){
					elemName = elemName.substring(0,5) + "...";
					title = " title='" + dataArray[elem].name + "'";
				}
				
				itemList += "<li data-treeid='" + dataArray[elem].id + "'><a " + title + " href=''><span class='color' style='background:" + dataArray[elem].color + ";'></span><span class='series-name'>" + elemName + "</span><span class='series-close'> X</span></a></li>";
			}
			itemList += "</ul>";
			if(dataArray.length > 0){
				itemList += "<div class='removeAll'><a href=''>remove all</a></div>";
			}
			$(container).html(itemList);
		}
	}
});