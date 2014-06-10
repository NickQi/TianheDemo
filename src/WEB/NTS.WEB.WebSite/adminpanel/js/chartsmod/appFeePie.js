define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'pie'
			},
			legend:{
				enabled: true,
				layout: "vertical",
				align: "right",
				borderWidth: 0,
				backgroundColor:"#F2F2F2",
				symbolWidth:12,
				width:120
			},
			plotOptions:{
				pie:{
					showInLegend: true,
					dataLabels:{
						useHTML:true,
						formatter:function(){}
					}
				}
			},
			tooltip:{}
		},
		
		renderTo: function(container, chartData){
			var opts = this.opts;

			//设置含单位的dataLabel
			opts.plotOptions.pie.dataLabels.formatter = function(){
				//return "<span style='font-size:12px'>" + this.point.name + "</span><br />" + this.y + "元 " + (this.percentage).toFixed(2) + "%";
				return "<span style='font-size:12px'>" + this.point.name + "</span><br />" + (this.percentage).toFixed(2) + "%";
			};
			//设置tooltip
			opts.tooltip.headerFormat = "";
			opts.tooltip.pointFormat = "{point.y}元";
			
			//染色
			var dataLength = chartData.series[0].data.length;
			for(var i = 0; i < dataLength; i++){
				var dataName = chartData.series[0].data[i].name;
				switch(dataName){
					case "尖时电费":
                    case "第一档费用":
						chartData.series[0].data[i].color = "#81ABA9";
						break;
					case "峰时电费":
                    case "第二档费用":
						chartData.series[0].data[i].color = "#A2D7DD";
						break;
					case "平时电费":
                    case "第三档费用":
						chartData.series[0].data[i].color = "#F7E52B";
						break;
					case "谷时电费":
                    case "第四档费用":
						chartData.series[0].data[i].color = "#94859C";
				}
			}
			
			return charts.renderChart(opts, chartData, container);
		}
	}
});