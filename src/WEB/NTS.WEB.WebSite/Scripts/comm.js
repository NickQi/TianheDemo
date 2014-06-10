/*公共的js函数部分*/

/*获取当前的月份第一天*/
function GetCuttMonth() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetCuttMonthFirstDate&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

// 获取指定月最后一天
function getCuttMonth(mydate) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.getCuttMonth&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
}

/*获取当前的日期*/
function GetToday() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetToday&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetNow() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetNow&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的周的第一天*/
function GetWeekFirstDay() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetWeekFirstDay&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的周的最后一天*/
function GetWeekLastDay() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetWeekLastDay&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的月的最后一天*/
function GetMonthLast() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetMonthLast&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetPrev(v) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetPrev&dll=NTS.WEB.Common&times=" + new Date().getTime(), data: { t: v }, type: 'GET', async: false, cache: false }).responseText;
}

function GetMiddle(v) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetMiddle&dll=NTS.WEB.Common&times=" + new Date().getTime(), data: { t: v }, type: 'GET', async: false, cache: false }).responseText;
}

function GetLast(v) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetLast&dll=NTS.WEB.Common&times=" + new Date().getTime(), data: { t: v }, type: 'GET', async: false, cache: false }).responseText;
}

function GetThreeMonth(v) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetThreeMonth&dll=NTS.WEB.Common&times=" + new Date().getTime(), data: { t: v }, type: 'GET', async: false, cache: false }).responseText;
}

function GetYear(v) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetYear&dll=NTS.WEB.Common&times=" + new Date().getTime(), data: { t: v }, type: 'GET', async: false, cache: false }).responseText;
}


function GetCuttYear() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetCuttYear&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}


function changeTwoDecimal(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        //alert('function:changeTwoDecimal->parameter error');
        //return false;
        return "";
    }
    var f_x = Math.round(x * 100) / 100;

    return f_x;
}


function GetMyNextMonth(mydate) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetMyNextMonth&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
}

function GetMyMonth(mydate) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetMyMonth&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
}

function GetMyDate(mydate) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetMyDate&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
}


function GetLastYearMyDate(mydate) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.GetLastYearMyDate&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
}

/** 
* 判断开始时间是否大于结束时间 
* @param beginTime 开始时间 
* @param endTime   结束时间 
* @return  boolean 
*/
function compareTime(startTime, endTime) {
    if (Date.parse(startTime.replace(/-/g, '/ ')) > Date.parse(endTime.replace(/-/g, '/ '))) {
        alert("请起止时间小于终止时间");
        return false;

    } else {

        var maxDays = 90;
        var flag = $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.MaxIntervals&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { startTime: startTime, endTime: endTime, maxDays: maxDays }, async: false, cache: false }).responseText;
        if (flag > 0) {
            return true;
        } else {
            alert("终止时间与起止时间的间隔不能大于" + maxDays + "天");
            return false;
        }
    }
}

function getCuttDate(mydate) {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.TimeParser.getCuttTimeDate&dll=NTS.WEB.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
}

function preview(oper) {
    if (oper < 10) {
        bdhtml = window.document.body.innerHTML; //获取当前页的html代码
        sprnstr = "<!--show start-->"; //设置打印开始区域
        eprnstr = "<!--show end-->"; //设置打印结束区域
        prnhtml = bdhtml.substring(bdhtml.indexOf(sprnstr) + 18); //从开始代码向后取html

        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr)); //从结束代码向前取html
        window.document.body.innerHTML = prnhtml;
        window.print();
        window.document.body.innerHTML = bdhtml;

    } else {
        window.print();
    }

}


function addCookie(objName, objValue, objHours) {//添加cookie
    var str = objName + "=" + escape(objValue);
    if (objHours > 0) {//为0时不设定过期时间，浏览器关闭时cookie自动消失
        var date = new Date();
        var ms = objHours * 3600 * 1000;
        date.setTime(date.getTime() + ms);
        str += "; expires=" + date.toGMTString();
    }
    document.cookie = str;

}


function getCookie(objName) {//获取指定名称的cookie的值
    var arrStr = document.cookie.split("; ");
    for (var i = 0; i < arrStr.length; i++) {
        var temp = arrStr[i].split("=");
        if (temp[0] == objName)
            return unescape(temp[1]);
    }
}

function getCheckMsg(dataModel,msgModel) {
    var msg = '';
    for (var key in dataModel) {
        if (dataModel[key] == '') {
            return msgModel[key] + "不能为空";
        }
    }
    return msg;
}

function setDefault() {
    ReportData.BaseData.starttime = $("#starttime").val();
    ReportData.BaseData.endtime = $("#endtime").val();
    ReportData.BaseData.objectid = $("#objectid").val();
    ReportData.BaseData.unit = $("#unit").val();
    ReportData.BaseData.itemcode = $("#itemcode").val();
}
function setCompareDefault() {
    ReportCompareData.BaseData.objectid = $("#objectid").val();
    ReportCompareData.BaseData.unit = $("#unit").val();
    ReportCompareData.BaseData.itemcode = $("#itemcode").val();
}


function setManyDefault() {

    var isDevice = "flag";
    var deptUnit = "deptunit";
    $("#objectid").val(getvaluelist());
    $("#objectname").val(getnamelist());
    //alert($("#objectid").val());
    ReportData.BaseData.starttime = $("#starttime").val();
    ReportData.BaseData.endtime = $("#endtime").val();
    ReportData.BaseData.objectid = $("#objectid").val();
    ReportData.BaseData.unit = $("#unit").val();
    ReportData.BaseData.itemcode = $("#itemcode").val();
    ReportData.BaseData[isDevice] = $("#clickunit").val();
    ReportData.BaseData[deptUnit] = $("#deptunit").val();
}

function getvaluelist() {
    var chooseobject = '';


    $(".checkbox").each(function () {
        var classname = $(this).parent().parent().parent().attr("class");
        switch (classname) {
            case "nav_first":
                classname = "select_a";
                break;
            case "nav_second":
                classname = "select_b";
                break;
            case "nav_fourth":
                classname = "select_d";
                break;
            default:
                classname = "select_c";
                break;
        }

        var oo = $(this).parent().parent();
        if (oo.hasClass(classname)) {
            //alert('bb');
            chooseobject += ',' + $(this).attr('config');
        }
    });
    return chooseobject;
}

function getnamelist() {
    var chooseobject = '';
    $(".checkbox").each(function () {
        var classname = $(this).parent().parent().parent().attr("class");
        switch (classname) {
            case "nav_first":
                classname = "select_a";
                break;
            case "nav_second":
                classname = "select_b";
                break;
            case "nav_fourth":
                classname = "select_d";
                break;
            default:
                classname = "select_c";
                break;
        }
        var oo = $(this).parent().parent();
        // alert(classname);
        if (oo.hasClass(classname)) {
            //alert('bb');
            chooseobject += ',' + $(this).attr('nconfig');
        }
    });
    return chooseobject;
}


