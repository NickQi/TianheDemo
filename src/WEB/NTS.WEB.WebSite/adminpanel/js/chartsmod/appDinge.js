define(['chartsmod/charts', 'highchartsmore'], function(charts){
	return {
		opts : {
			chart:{
				type:'gauge',
				backgroundColor: null,
				plotBackgroundColor: null,
				plotBackgroundImage: null,
				plotBorderWidth: 0,
				plotShadow: false,
                style:{
                    overflow: 'hidden',
                    paddingTop: '30px'
                },
                height:170
			},

			pane: {
				startAngle: -90,
				endAngle: 90,
				background: [{
					backgroundColor: {
						linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
						stops: [
							[0, '#FFF'],
							[1, '#E4E4E4']
						]
					},
					borderWidth: 1,
					borderColor: "#BFBFBF",
					outerRadius: '125%'
				},
				{
					backgroundColor: {
						linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
						stops: [
							[0, '#1B2837'],
							[1, '#2C3E50']
						]
					},
					borderWidth: 0,
					borderColor: "#BFBFBF",
					outerRadius: '118%'
				},
				{
					backgroundColor: {
						linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
						stops: [
							[0, '#314559'],
							[1, '#2C3E50']
						]
					},
					borderWidth: 0,
					borderColor: "#BFBFBF",
					outerRadius: '110%'
				},
				{
					backgroundColor: {
						linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
						stops: [
							[0, '#26384B'],
							[1, '#26384B']
						]
					},
					borderWidth: 0,
					borderColor: "#BFBFBF",
					outerRadius: '32%'
				},
				{
					backgroundColor: {
						linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
						stops: [
							[0, '#34495E'],
							[1, '#34495E']
						]
					},
					borderWidth: 0,
					borderColor: "#BFBFBF",
					outerRadius: '30%'
				},
				{
					backgroundColor: {
						linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
						stops: [
							[0, '#26384B'],
							[1, '#26384B']
						]
					},
					borderWidth: 0,
					borderColor: "#BFBFBF",
					outerRadius: '20%'
				}],
				center:["50%", "80%"],
				size: 200
			},
			
			plotOptions: {
				gauge:{
					dataLabels:{
						y: 40,
						style:{
							fontWeight: "normal",
							color:"#000"
						},
						borderWidth: 1,
						borderColor: '#BDBDBD',
						backgroundColor: {
							linearGradient: [0,0,0, 60],
							stops: [
								[0, '#FFFFFF'],
								[1, '#E5E5E5']
							]
						}
					},
					dial:{
						backgroundColor:"#FFF"
					},
					pivot:{
						backgroundColor:"#FFF"
					}
				}
			},
			
			tooltip:{
				enabled: false
			},
			   
			// the value axis
			yAxis: {
				min: 0,
				max: 180,
				
				minorTickInterval: 'auto',
				minorTickWidth: 0,
				minorTickLength: 10,
				minorTickPosition: 'inside',
				minorTickColor: '#666',
		
				tickInterval: 10,
				tickWidth: 0,
				tickPosition: 'inside',
				tickLength: 10,
				tickColor: '#666',
				labels: {
					step: 3,
					//rotation: 'auto',
					format:'{value}%',
					style:{
						color:"#FFF"
					}
				},
				title: {
					text: ''
				},
				plotBands: [{
					from: 0,
					to: 100,
					color: '#ABCF53' 
				}, {
					from: 100,
					to: 130,
					color: '#ED6801' 
				}, {
					from: 130,
					to: 180,
					color: '#65727F' 
				}],
				lineWidth: 0
			},
		
			series: [{
			}]
		},
		
		renderTo: function(container, chartData){
			var opts = this.opts;
			opts.plotOptions.gauge.dataLabels.formatter = function(){
				return chartData.ActualPercent + "%　" + chartData.ActualValue;
			};
			return charts.renderChart(opts, chartData, container);
		}
	}
});