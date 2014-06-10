
// 时间显示函数Thursday, June 18, 2009
today=new Date();       
function initArray(){       
 this.length=initArray.arguments.length;       
 for(var i=0;i<this.length;i++){       
 this[i+1]=initArray.arguments[i];} 
}       
var d=new initArray(       
  "星期天",       
  "星期一",       
  "星期二",       
  "星期三",       
  "星期四",       
  "星期五",       
  "星期六"); 
var m=new initArray(       
  "1月",       
  "2月",       
  "3月",       
  "4月",       
  "5月",       
  "6月",
  "7月",       
  "8月",       
  "9月",       
  "10月",       
  "11月",
  "12月");

var lastdate = today.getFullYear() + "年" + m[today.getMonth() + 1] +today.getDate() + "日," + d[today.getDay() + 1];
$(".homedate").html(lastdate);    
// 时间显示函数结束
