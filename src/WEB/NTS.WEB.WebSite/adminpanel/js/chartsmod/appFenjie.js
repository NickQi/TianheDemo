define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'pie',
                backgroundColor: null,
                plotBackgroundColor: null
			},
			title: {
	            text: '用电总量',
	            align: 'center',
	            verticalAlign: 'middle',
	            x:-58,
	            y:-5,
	            style:{
	            	fontSize:"12px",
	            	color:"#FFF",
                    fontFamily:"Microsoft Yahei"
	            }
	        },
			legend:{
				enabled: true,
				layout: "vertical",
				align: "right",
				borderWidth: 0,
				backgroundColor:"#151516",
				symbolWidth:12,
				//width:120,
                labelFormatter: function(){
                    return this.name.length > 8 ? this.name.substring(0, 7) + "..." : this.name;
                },
                itemStyle:{
                    color:"#FFF",
                    fontFamily:"Microsoft Yahei"
                },
                itemHoverStyle:{
                    color:"#FFF",
                    fontFamily:"Microsoft Yahei",
                    cursor:"default"
                }
			},
			plotOptions:{
				pie:{
					showInLegend: true,
                    size:120,
                    innerSize:108,
                    borderColor:null,
					dataLabels:{
						useHTML:true,
                        distance:5,
                        color:"#FFF",
                        style:{
                            fontFamily:"Microsoft Yahei",
                            fontSize:"12px"
                        },
						formatter:function(){}
					}
				}
			},
			tooltip:{}
		},
		
		renderTo: function(container, chartData){
			var opts = this.opts;
			var unit = chartData.unit;
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
				//return "<span style='font-size:12px'>" + this.point.name + "</span><br />" + this.y + "kWh<br/>" + (this.percentage).toFixed(2) + "%";
                //return "<span style='font-size:12px'>" + this.point.name + "</span><br />" + (this.percentage).toFixed(2) + "%";
                //return this.y + "kWh<br/>" + (this.percentage).toFixed(2) + "%";
				return (this.percentage).toFixed(2) + "%";
			};
			//设置tooltip
			opts.tooltip.pointFormat = "{point.y}kWh";
			//设置含总和的title
			opts.title.text = '用电总量<br>'+sum+"kWh";
			return charts.renderChart(opts, chartData, container);
		}
	}
});