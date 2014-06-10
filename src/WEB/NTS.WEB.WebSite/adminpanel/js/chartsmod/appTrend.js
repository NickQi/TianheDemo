define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart: {
				ignoreHiddenSeries: true
			},
			xAxis: {
				type:'datetime'
			},
			yAxis: {
				tickPixelInterval: 35,
				labels: {
					format: '{value}',
					step: 1
				}
			},
			plotOptions:{
				series:{
					color: '#3598DB',
					lineWidth:2
				}
			},
			legend:{
				enabled: true,
				itemMarginTop: 0,
				itemMarginBottom: 0
			}
		},
		
		renderTo: function(container, chartData, from, to){
			var opts = this.opts;
			//定义各series的样式和名称
			var seriesLength = chartData.series.length;
			for(var i = 0; i < seriesLength; i++){
				var seriesName = chartData.series[i].name;
				switch(seriesName){
					case "ActualLine":
						chartData.series[i].name = "能耗曲线";
						chartData.series[i].color = "#1ABC9C";
						chartData.series[i].zIndex = 100;
						break;
					case "QuotaLine":
						chartData.series[i].name = "定额线";
						//chartData.series[i].color = "rgba(187,209,211,0.6)";
						//chartData.series[i].type = "column";
                        chartData.series[i].marker = {
                            radius:0
                        };
						break;
					case "ForeCastLine":
						chartData.series[i].name = "趋势预测线";
						chartData.series[i].dashStyle = "ShortDot";
                        chartData.series[i].color = "#E27D37";
						chartData.series[i].marker = {
							radius: 0,
							states: {
								hover: {
									radius: 4
								}
							}
						};
						chartData.series[i].zIndex = 100;
						break;
				}
			}
			return charts.renderChart(opts, chartData, container, from, to);
		}
	}
});