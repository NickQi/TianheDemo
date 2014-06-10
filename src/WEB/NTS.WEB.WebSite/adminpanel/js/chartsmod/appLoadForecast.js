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
				},
				min: null
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
					case "负荷预测值":
						//chartData.series[i].name = "能耗曲线";
						chartData.series[i].color = "#1ABC9C";
						//chartData.series[i].zIndex = 100;
						break;
					case "能耗实际值":
						//chartData.series[i].name = "定额线";
						chartData.series[i].color = "#E27D37";
						//chartData.series[i].type = "column";
						break;
				}
			}
			return charts.renderChart(opts, chartData, container, from, to);
		}
	}
});