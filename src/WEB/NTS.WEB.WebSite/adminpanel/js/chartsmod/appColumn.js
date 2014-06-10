define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'column',
                backgroundColor: null,
                plotBackgroundColor: null,
                marginTop:0
			},
			xAxis: {
                type:"category",
                lineColor:"#FFF",
                alternateGridColor:null,
                tickLength:0,
                labels:{
                    style:{
                        color:"#FFF",
                        fontSize:"12px"
                    },
                    rotation:-90
                }
			},
			yAxis: {
				lineWidth:0,
                gridLineWidth:0,
                alternateGridColor:null,
                labels:{
                    enabled:false
                }
			},
            plotOptions:{
                column:{
                    color:"#FFF",
                    pointWidth:11
                }
            }
		},
		
		renderTo: function(container, chartData){
			var opts = this.opts;
			return charts.renderChart(opts, chartData, container);
		}
	}
});