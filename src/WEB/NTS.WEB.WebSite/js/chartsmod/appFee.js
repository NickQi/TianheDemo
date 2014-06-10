define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart: {
				ignoreHiddenSeries: true,
				type: "column"
			},
			xAxis: {
				type:'datetime'
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
		
		renderTo: function(container, chartData, from, to){
			var opts = this.opts;
			//定义各series的样式和名称
			var seriesLength = chartData.series.length;
			for(var i = 0; i < seriesLength; i++){
				var seriesName = chartData.series[i].name;
				switch(seriesName){
					case "尖时费用":
                    case "第一档费用":
						//chartData.series[i].name = "能耗曲线";
						chartData.series[i].color = "#81ABA9";
						//chartData.series[i].zIndex = 100;
						break;
					case "峰时费用":
                    case "第二档费用":
						//chartData.series[i].name = "定额线";
						chartData.series[i].color = "#A2D7DD";
						//chartData.series[i].type = "column";
						break;
					case "平时费用":
                    case "第三档费用":
						//chartData.series[i].name = "定额能耗差值曲线";
						chartData.series[i].color = "#F7E52B";
						//chartData.series[i].zIndex = 100;
						break;
					case "谷时费用":
                    case "第四档费用":
						chartData.series[i].color = "#94859C";
						break;
					default:
						chartData.series[i].color = "#1BBC9B";
				}
			}
			return charts.renderChart(opts, chartData, container, from, to);
		}
	}
});