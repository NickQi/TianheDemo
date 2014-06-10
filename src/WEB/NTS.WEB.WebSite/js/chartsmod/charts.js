define(['highcharts','drilldown'], function(drilldown){
	return {

		resetTimezone: function(){
			Highcharts.setOptions({
				global: {
					useUTC: false
				},
				lang:{
					months:["1","2","3","4","5","6","7","8","9","10","11","12"],
					shortMonths:["1","2","3","4","5","6","7","8","9","10","11","12"]
				}
			});
		},
		
		getCharts: function(){
			return Highcharts.charts;
		},

		renderChart: function(opts, chartData, container, from, to, callback){

			var defaults = {
				colors:[
					'#1ABC9C',
					'#81ABA9',
					'#A2D7DD',
					'#84A2D4',
					'#69B076',
					'#ABCF53',
					'#DCCB19',
					'#8F8667',
					'#C7B26F',
					'#E27D37',
					'#946142',
					'#CC8C5C',	
					'#E0C28C',
					'#E4DC8B',
					'#B88884',
					'#E5D2D8',
					'#C3A2BD',
					'#94859C',
					'#2CA9E1',
					'#3E62AE'
				],
				chart: { 
					renderTo : container,
					ignoreHiddenSeries: false,
					plotBackgroundColor:'#FFFFFF',
					style:{
						overflow:'visible'
					}
				},
				credits:{
					enabled: false
				},
				legend:{
					enabled: false,
					itemMarginTop: 5,
					itemMarginBottom: 5
				},
				plotOptions:{
					series:{
					},
					line:{
						lineWidth:2,
						marker:{
							radius:3,
							fillColor:"#FFFFFF",
							lineColor:null,
							lineWidth:1,
							symbol: "circle"
						}
					},
					column:{
						pointPadding: 0,
						groupPadding: 0.1,
						borderWidth: 0,
						tooltip:{
							followPointer: true
						}
					},
					bar:{
						pointPadding: 0,
						groupPadding: 0.15,
						borderWidth: 0,
						tooltip:{
							followPointer: true
						},
                        stickyTracking:false
					},
					pie:{
						allowPointSelect: true,
						size:160,
						innerSize: 140,
						dataLabels:{

							connectorWidth:1,
							distance:10,
							softConnector:false,
							useHTML:true,
							formatter:function(){
								return "<span style='font-size:10px'>" + this.point.name + "</span><br />" + this.y + (this.percentage).toFixed(0) + "%";
							}
						},
						point:{
							events:{
								legendItemClick: function(e){
									return false;
								}
							}
						}
					}
				},
				title: {
					text: null
				},
				tooltip:{
					borderWidth: 1,
					borderColor: '#BDBDBD',
					backgroundColor: {
						linearGradient: [0,0,0, 60],
						stops: [
							[0, '#FFFFFF'],
							[1, '#E5E5E5']
						]
					},
					style:{
						padding:"3px",
						fontSize:"12px"
					},
					//positioner: function (labelWidth, labelHeight, point) {
					//	return { x: point.plotX + chart.plotLeft - labelWidth/2, y: point.plotY + chart.plotTop - labelHeight - 10};
					//},
					useHTML: false,
					//headerFormat: "",
					pointFormat:"{point.y}",
					dateTimeLabelFormats:{
						millisecond:"%A, %b %e, %H:%M:%S.%L",
						second:"%A, %b %e, %H:%M:%S",
						minute:"%A, %b %e, %H:%M",
						hour:"%b月%e日%H时",
						day:"%y年%b月%e日",
						week:"Week from %A, %b %e, %Y",
                        month: '%y年%b月',
						//month:"%y年%b月%e日%H:%M:%S",
						year:"%Y年"
					}
				},
				xAxis:{
					dateTimeLabelFormats:{
						millisecond: '%H:%M:%S.%L',
						second: '%H:%M:%S',
						minute: '%H:%M',
						hour: '%H',
						day: '%b.%e',
						week: '%e. %b',
						month: '%y年%b月',
                        //month:"%y年%b月%e日%H:%M:%S",
						year: '%Y'
					},
					labels:{
						//step:1,
						staggerLines: 1,
						style:{
							fontSize:"12px"
						}
					},
					lineColor: '#C0DDC7',
					alternateGridColor:'#F2F6F2',
					//tickInterval:1,
					startOnTick: false,
					showFirstLabel: true,
					tickPixelInterval:30
				},
				yAxis:{
					title:{
						text:""
					},
					lineWidth:1,
					lineColor: '#C0DDC7',
					gridLineColor: "#C0DDC7",
					gridLineDashStyle: "Dash",
					alternateGridColor: 'rgba(194,240,194,0.35)',
					min:0,
					tickPixelInterval:30
				}
			};
			opts = $.extend(true, defaults, opts);

			if(opts.xAxis.type == "datetime"){
				//根据起止时间设置颗粒度
				var particle = this.getParticle(from, to, true);
				opts.xAxis.tickInterval = particle;
				opts.plotOptions.series.pointInterval = particle;
				//数据点开始的时间
				//如果是按月，必须设置为从每月的1号开始，否则X轴会有偏差
				if(particle == 30 * 24 * 3600 * 1000){
					from.setDate(1);
				//如果是按年，必须设置为从每年的1月1日开始
				}else if(particle == 365 * 24 * 3600 * 1000){
					from.setMonth(0);
					from.setDate(1);
				}
				opts.plotOptions.series.pointStart = from.valueOf();
			}else{
				opts.xAxis.labels.formatter = function(){
					if(opts.chart.type !== "bar" && !opts.xAxis.hasOwnProperty("categories")){
						return this.value + 1;
					}else{
						return this.value;
					}
				};
			}
			var chart = null;

			opts.series = chartData.series;
			//如果超过20个数据，则倾斜横坐标label
			if(opts.series.length > 0 && opts.series[0].data.length > 20 && opts.chart.type !== "bar"){
				opts.xAxis.labels.rotation= 0
			}
			//如果超过23个数据，则横坐标分两行
            if(opts.series.length > 0 && opts.series[0].data.length > 24 && opts.chart.type !== "bar"){
                //opts.xAxis.labels.staggerLines = 2;
                opts.xAxis.labels.step = 2;
            }
			//当X轴为datetime，且只有一个数据时，添入一个null，解决X轴时间显示不对的问题
            if(opts.xAxis.type === "datetime" && opts.series.length > 0 && opts.series[0].data.length == 1){
                var particle = this.getParticle(from, to, true);
                opts.series[0].data.push(null);
                opts.xAxis.tickInterval = particle*2;
            }
			opts.unit = chartData.unit;
			if(opts.chart.type === "bar"){
				opts.series[0].id = "pm-series";
				//设置bar类型的X轴坐标，为各数据的name
				var total = opts.series[0].data.length;
				var xCategories = [];
				//若逆序时，各数据bar颜色跟着倒序
				for(var i = 0, j = total; i < total; i++){
					if(opts.reversed){
						--j;
						opts.series[0].data[i].color = opts.colors[j];
					}else{
						opts.series[0].data[i].color = opts.colors[i];
					}
					xCategories.push(opts.series[0].data[i].name);
				}
				opts.xAxis.categories = xCategories;
			}
			chart = new Highcharts.Chart(opts, callback);
			return chart;
		},

		/*******************************
		* 根据默认规则获取颗粒度
		* @params: {Date} from, {Date} to, {Boolean} if_value
		* if_value 为 true，则返回highcharts需要的时间颗粒度设置值（毫秒）
		*          为 false，则返回文字，H（时）, D（日）, M（月） ,Y（年）
		*******************************/
		getParticle: function(from, to, if_value){
			var diff_day = this.diffDate(from, to);
			var tick_1 = 0;	//一天内
			var tick_2 = 90;
			var tick_3 = 365 * 3;
			if(diff_day == tick_1){
				return if_value ? 3600 * 1000 : "H";
			}else if(diff_day <= tick_2 && diff_day > tick_1){
				return if_value ? 24 * 3600 * 1000 : "D";
			}else if(diff_day <= tick_3 && diff_day > tick_2){
				return if_value ? 31 * 24 * 3600 * 1000 : "M";
			}else{
				return if_value ? 365 * 24 * 3600 * 1000 : "Y";
			}
		},

		/*起止时间之间的天数*/
		diffDate: function(from, to){
			var diff = to.valueOf() - from.valueOf();
			var diff_day = diff/(1000*60*60*24);
			return diff_day;
		},
		
		/*增加数据线*/
		addSeries: function(chart, opts, chartData){
			var series = null;
			opts.data = chartData.series[0].data;
			series = chart.addSeries(opts);

			return series;
		},

		/*改变Series type*/
		changeType: function(chart, type, index){
			var series = chart.series;
			var length = series.length;
			if(typeof(index) === "number"){
				series[index].update({
					type: type
				});
			}else{
				for (var i = 0; i < length; i++) {
					series[i].update({
						type: type
					});
				}
			}
		}

	};
});