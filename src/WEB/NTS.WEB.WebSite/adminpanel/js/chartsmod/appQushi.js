define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			xAxis: {
				type:'datetime'
                //categories: ['00', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23']
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
			}
		},
		
		renderTo: function(container, chartData, date){
			var opts = this.opts;
			/*
			var today = new Date();
			var today_year = today.getFullYear();
			var today_month = today.getMonth();
			var today_date = today.getDate();
			var date_from = new Date(today_year, today_month, today_date);
			var date_to = date_from;
			*/
			return charts.renderChart(opts, chartData, container, date, date);
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