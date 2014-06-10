/*公共的js函数部分*/

/*获取当前的月份第一天*/
function GetCuttMonth() {
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetCuttMonthFirstDate&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的日期*/
function GetToday() {
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetToday&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetNow() {
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetNow&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的周的第一天*/
function GetWeekFirstDay() {
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetWeekFirstDay&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的周的最后一天*/
function GetWeekLastDay() {
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetWeekLastDay&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

/*获取当前的月的最后一天*/
function GetMonthLast() {
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetMonthLast&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}


function GetAreaName(areaid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.BArea.GetAreaName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + areaid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetBuildName(buildid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}
function GetCollectName(collectid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_DataCollectionInfo.GetCollectName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + collectid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetDeviceName(deviceid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + deviceid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetColor() {
    return $.ajax({ url: "action.ashx?method=NTS.WEB.Common.Strings.GetStrColor&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
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
    return $.ajax({ url: "action.ashx?method=MyCommon.TimeParser.GetMyNextMonth&dll=NTS_BECM.Common&times=" + new Date().getTime(), type: 'GET', data: { mydate: mydate }, async: false, cache: false }).responseText;
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
    }
    
    return true;
}


