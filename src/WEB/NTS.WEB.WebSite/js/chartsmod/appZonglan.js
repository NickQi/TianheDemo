define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:"pie"
			},
			legend:{
				enabled: true,
				layout: "vertical",
				align: "right",
				borderWidth: 0,
				backgroundColor:"#F2F2F2",
				symbolWidth:12
			},
			plotOptions:{
				pie:{
					dataLabels:{
						connectorWidth:1,
						distance:5,
						useHtml:true,
						formatter: function(){
							/*
							var name = this.point.name;
							var percent = (this.percentage).toFixed(2) + "%";
							var value = this.point.x;
							var html = "<span style='font-size:12px; display:block; text-align:center'>" + name + "</span><span style='display:block; text-aling:center'>" + value + "</span><span style='display:block; text-align:center'>" + percent + "</span>";
							return html;
							*/
							return (this.percentage).toFixed(2) + "%";
						}
					},
					showInLegend: true
				}
			},
			tooltip:{
					pointFormat:"标准煤：{point.y}T"
				},
		},
		
		renderTo: function(container, chartData){
			var opts = this.opts;
			return charts.renderChart(opts, chartData, container);
		}
	}
});