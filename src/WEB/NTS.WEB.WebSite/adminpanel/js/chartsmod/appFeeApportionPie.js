define(['chartsmod/charts'], function(charts){
	return {
		opts : {
			chart:{
				type:'pie'
			},
			legend:{
				enabled: true,
				layout: "vertical",
				align: "left",
                floating: true,
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
					},
                    size: 210,
                    innerSize:190
				}
			},
            title: {
                text: '总费用：',
                align: 'center',
                verticalAlign: 'middle',
                x:0,
                y:0,
                style:{
                    fontSize:"12px",
                    color:"#666"
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

            //设置title
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
            opts.title.text = "总费用：" + sum + "元";
			
			//染色
			var dataLength = chartData.series[0].data.length;
			for(var i = 0; i < dataLength; i++){
				var dataName = chartData.series[0].data[i].name;
				switch(dataName){
					case "分摊前费用":
						chartData.series[0].data[i].color = "#A2D7DD";
						break;
					case "分摊费用":
						chartData.series[0].data[i].color = "#E27D37";
						break;
				}
			}
			
			return charts.renderChart(opts, chartData, container);
		}
	}
});