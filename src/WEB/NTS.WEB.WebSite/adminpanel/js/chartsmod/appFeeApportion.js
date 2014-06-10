define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart: {
				ignoreHiddenSeries: true,
				type: "column"
			},
			xAxis: {
                labels:{
                }
			},
			yAxis: {
				tickPixelInterval: 35,
				labels: {
					format: '{value}',
					step: 1
				},
				min: 0
			},
			plotOptions:{
				column: {
					stacking:"normal"
					//groupPadding: 0.2
				}
			},
			legend:{
				enabled: true,
				itemMarginTop: 0,
				itemMarginBottom: 0
			},
			tooltip:{
				formatter: function() {
                    return this.series.name +': '+ this.y +'<br/>' + '总费用: '+ this.point.stackTotal;
                }
			}
		},
		
		renderTo: function(container, chartData){
			var opts = this.opts;
			//定义各series的样式和名称
			var seriesLength = chartData.series.length;
			for(var i = 0; i < seriesLength; i++){
				var seriesName = chartData.series[i].name;
				switch(seriesName){
					case "分摊前费用":
						//chartData.series[i].name = "能耗曲线";
						chartData.series[i].color = "#A2D7DD";
						//chartData.series[i].zIndex = 100;
						break;
					case "分摊费用":
						//chartData.series[i].name = "定额线";
						chartData.series[i].color = "#E27D37";
						//chartData.series[i].type = "column";
						break;
					default:
						chartData.series[i].color = "#1BBC9B";
				}
			}
            //控制柱子的宽度
            var dataLength = chartData.series[0].data.length;
            if(dataLength < 14){
                opts.plotOptions.column.pointWidth = 25;
            }

            //设置横坐标为对比对象名
            var categories = [];
            var dataArray = chartData.series[0].data;
            for(var i = 0; i < dataLength; i++){
                categories.push(dataArray[i].name);
            }
            opts.xAxis.categories = categories;

			return charts.renderChart(opts, chartData, container);
		}
	}
});