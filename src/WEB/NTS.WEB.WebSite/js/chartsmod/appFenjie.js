define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'pie'
			},
			title: {
	            text: '所选对象能耗总值',
	            align: 'center',
	            verticalAlign: 'middle',
	            x:-70,
	            y:-5,
	            style:{
	            	fontSize:"12px",
	            	color:"#666"
	            }
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
			var unit = chartData.unit;
			var titleText = chartData.titleText;
			//计算总值
			var sum = 0;
			var dataArray = chartData.series[0].data;
			var dataLength = dataArray.length;
			//for(var elem in dataArray){
			for(var i = 0; i < dataLength; i++){
				if(dataArray[i].hasOwnProperty("y")){
					sum += dataArray[i].y;
				}else{
					sum += dataArray[i][1];
				}
			}
			sum = sum.toFixed(2);

			//设置含单位的dataLabel
			opts.plotOptions.pie.dataLabels.formatter = function(){
				//return "<span style='font-size:12px'>" + this.point.name + "</span><br />" + this.y + unit + "<br/>" + (this.percentage).toFixed(2) + "%";
				return (this.percentage).toFixed(2) + "%";
			};
			//设置tooltip
			opts.tooltip.pointFormat = "{point.y}" + unit;
			//设置含总和的title
            if(titleText){
                opts.title.text = titleText + '<br>' + sum + unit;
            }else {
                opts.title.text = '所选能耗总值<br>' + sum + unit;
            }
			return charts.renderChart(opts, chartData, container);
		}
	}
});