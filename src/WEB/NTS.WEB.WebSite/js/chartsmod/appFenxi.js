define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'column'
			},
			xAxis: {
				type:'datetime',
				labels: {
					
				}
			},
			yAxis: {
				//tickPixelInterval: 50,
				labels: {
					format: '{value}',
					step: 1
				}
			}
		},
		
		setSeriesType: function(seriesType){
			var typeOpts = {
				chart:{
					type: seriesType
				}
			}
			this.opts = $.extend(true, this.opts, typeOpts);
		},
		
		renderTo: function(container, chartData, startTime, endTime, callback){
			var opts = this.opts;
			return charts.renderChart(opts, chartData, container, startTime, endTime, callback);
		},

		/*增加同比值*/
		addTBSeries: function(chart, chartData){
			var opts = {
				type:"line",
				color:"#E27D37",
				tooltip:{
					headerFormat: "",
					pointFormat:"同比值：{point.y}"
				},
				marker:{
					fillColor:'#FFFFFF',
					lineColor:null,
					lineWidth:1
				},
				lineWidth:0,
				events:{
					mouseOver:function(){
						this.update({
							lineWidth:1
						});
						chart.series[0].update({
							color:"rgba(26,188,156,0.5)"
						});
					},
					mouseOut: function(){
						this.update({
							lineWidth:0
						});
						chart.series[0].update({
							color:"rgba(26,188,156,1)"
						});
					}
				},
				zIndex:20
			};
			
			return charts.addSeries(chart, opts, chartData);
		},

		/*增加环比值*/
		addHBSeries: function(chart, chartData){
			var opts = {
				type:"line",
				color:"#E27D37",
				tooltip:{
					headerFormat: "",
					pointFormat:"环比值：{point.y}"
				},
				marker:{
					fillColor:'#FFFFFF',
					lineColor:null,
					lineWidth:1
				},
				lineWidth:0,
				events:{
					mouseOver:function(){
						this.update({
							lineWidth:1
						});
						chart.series[0].update({
							color:"rgba(26,188,156,0.5)"
						});
					},
					mouseOut: function(){
						this.update({
							lineWidth:0
						});
						chart.series[0].update({
							color:"rgba(26,188,156,1)"
						});
					}
				},
				zIndex:20
			};
			
			return charts.addSeries(chart, opts, chartData);
		}
	}
});