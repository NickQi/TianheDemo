/**
 * ==========================================================
 * Copyright (c) 2013, NTS-9000 All rights reserved.
 * NTS项目用户登录JS
 * Author: Jinsam
 * Date: 2013-05-17 22:43:55 502000
 * ==========================================================
 */
var initDataA11A = [[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[354.00,354.00,354.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[44.00,44.00,44.00],[39.00,39.00,39.00],[52.00,52.00,52.00],[0.00,0.00,0.00],[6.00,6.00,6.00],[9.00,9.00,9.00],[7.00,7.00,7.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[0.00,0.00,0.00],[6.00,6.00,6.00],[9.00,9.00,9.00],[10.00,10.00,10.00],[0.00,0.00,0.00],[9.00,9.00,9.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[11.00,11.00,11.00],[16.00,16.00,16.00],[13.00,13.00,13.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[2.00,2.00,2.00],[3.00,3.00,3.00],[0.00,0.00,0.00],[11.00,11.00,11.00],[5.00,5.00,5.00],[6.00,6.00,6.00],[0.00,0.00,0.00]
,[0.00,0.00,0.00],[7.00,7.00,7.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[7.00,7.00,7.00],[5.00,5.00,5.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[11.00,11.00,11.00],[3.00,3.00,3.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
,[0.00,0.00,0.00],[0.00,0.00,0.00],[0.00,0.00,0.00],[12.00,12.00,12.00],[9.00,9.00,9.00],[0.00,0.00,0.00],[0.00,0.00,0.00]
];
var initDataA11V = [];
initDataA11V[7] = [401.00,401.00,401.00];
$(function(){
	var myDate = new Date();
	myDate = $tiansu.date.format({
		date: new Date(myDate),
		connect: '-'
	});
	$("#sys-time").html(myDate);//本地时间
	//menu click
	$(document).on("click",".electMenu ul li",function(){
		$(this).addClass("current").siblings().removeClass("current");
	});
	//tab click
	$(document).on("click",".tab ul li",function(){
		var index = $(this).index()+1;
		$(this).addClass("current").siblings().removeClass("current");
		$(".a_"+index).show().siblings().hide();
	});
	//定时器
	setInterval("renderDataOdd()",4000);
	setInterval("renderDataEven()",5000);
	setInterval("renderDataV()",4000);
});
//1a站
function renderDataOdd() {
	$.each(initDataA11A,function(index){
		var index = parseInt(index);
		if(index%2 > 0){
			for(var i=0;i<3;i++){
				var value = null,
					under = null,
					over = null;
				var num = parseFloat($(this)[i]);
				if (num == 0) {
					value = 0.00;
				} else if (num >=100) {
					under = num-(num*0.005);
					over = num+(num*0.005);
					value = fRandomBy(under,over);
					$(".amp_"+(index+1)+" .a"+(i+1)).html(value);
				} else {
					under = num-(num*0.02);
					over = num+(num*0.02);
					value = fRandomBy(under,over);
					$(".amp_"+(index+1)+" .a"+(i+1)).html(value);
				}
			}
		}
	});
}
function renderDataEven() {
	$.each(initDataA11A,function(index){
		var index = parseInt(index);
		if(index%2 == 0){
			for(var i=0;i<3;i++){
				var value = null,
					under = null,
					over = null;
				var num = parseFloat($(this)[i]);
				if (num == 0) {
					value = 0.00;
				} else if (num >=100) {
					under = num-(num*0.005);
					over = num+(num*0.005);
					value = fRandomBy(under,over);
					$(".amp_"+(index+1)+" .a"+(i+1)).html(value);
				} else {
					under = num-(num*0.02);
					over = num+(num*0.02);
					value = fRandomBy(under,over);
					$(".amp_"+(index+1)+" .a"+(i+1)).html(value);
				}
			}
		}
	});
}
function renderDataV() {
	$.each(initDataA11V,function(index){
		var index = parseInt(index);
		for(var i=0;i<3;i++){
			var value = null,
				under = null,
				over = null;
			var num = parseFloat($(this)[i]);
			if (num == 0) {
				value = 0.00;
			} else {
				under = num-(num*0.005);
				over = num+(num*0.005);
				value = fRandomBy(under,over);
				$(".amp_"+(index+1)+" .v"+(i+1)).html(value);
			}
		}
	});	
}
function fRandomBy(under,over) {
	switch(arguments.length){
		case 1: return (Math.random()*under).toFixed(2);
		case 2: return (Math.random()*(over-under)+under).toFixed(2);
		default: return 0;
	}
}