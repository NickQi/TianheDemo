<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="NTS.WEB.WebSite.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script type="text/javascript" src="js/lib/jquery/jquery.min.js"></script>
    <script src="Scripts/jquery.plugins.js" type="text/javascript"></script>
    <script src="Scripts/Library/artDialog/jquery.artDialog.min.js" type="text/javascript"></script>
    <%--  <script data-main="/jsrc/view/ranking_main.js" src="/jsrc/lib/require/require.min.js"></script>
    <script src="/jsrc/charts/mapQuery.js"></script>--%>
    <script type="text/javascript">

        var testServerUrl = {
            login: 'action.ashx?action=UserLogin',
            remmberpass: 'action.ashx?action=RemeberMyPass',
            indexwindow: 'action.ashx?action=IndexMap',
            indexmonthenery: 'action.ashx?action=IndexMonthEnery',
            indexCompare: 'action.ashx?action=indexCompare',
            indexOrderList: 'action.ashx?action=indexOrderList',
            indexLook: 'action.ashx?action=indexLook',
            eneryquery: 'action.ashx?action=eneryquery',
            objecttree: 'action.ashx?action=objecttree',
            devicetree: 'action.ashx?action=devicetree',
            lazydevicetree: 'action.ashx?action=devicetree',
            shoporder: 'action.ashx?action=shoporder',
            indexElectricRealLineChart: 'action.ashx?action=indexElectricRealLineChart',
            indexItem: 'action.ashx?action=indexItem',
            deviceRealChart: 'action.ashx?action=deviceRealChart',

            shopordernew: 'action.ashx?action=shopordernew',
            exportExcelDataRankingNew: 'action.ashx?action=exportExcelDataRankingNew',
            indexDeviceList: 'action.ashx?action=indexDeviceList',
            realtime: 'action.ashx?action=realtime',

            GetMenuModule: 'action.ashx?action=GetMenuModule',
    
            objectItemTree: 'action.ashx?action=objectItemTree',
            IndexContrastObjsChart: 'action.ashx?action=IndexContrastObjsChart',
            IndexContrastObjsLst: 'action.ashx?action=IndexContrastObjsLst',
            IndexContrastPeriodsChart: 'action.ashx?action=IndexContrastPeriodsChart',
            IndexContrastPeriodssLst: 'action.ashx?action=IndexContrastPeriodssLst',
            ExportQueryObjs: 'action.ashx?action=ExportQueryObjs',
            ExportQueryPeriod: 'action.ashx?action=ExportQueryPeriod',

            GetUsers: 'action.ashx?action=GetUsers',
            GetSingleUser: 'action.ashx?action=GetSingleUser',
            AddUser: 'action.ashx?action=AddUser',
            UpdateUser: 'action.ashx?action=UpdateUser',
            DeleteUser: 'action.ashx?action=DeleteUser',
            GetUserGroups: 'action.ashx?action=GetUserGroups',
            GetSingleUserGroup: 'action.ashx?action=GetSingleUserGroup',
            AddUserGroup: 'action.ashx?action=AddUserGroup',
            UpdateUserGroup: 'action.ashx?action=UpdateUserGroup',
            DeleteUserGroup: 'action.ashx?action=DeleteUserGroup',
            GetEnergyAnalyseLineChart: 'action.ashx?action=GetEnergyAnalyseLineChart',
            GetEnergyAnalyseCompare: 'action.ashx?action=GetEnergyAnalyseCompare',
            GetEnergyAnalysePie: 'action.ashx?action=GetEnergyAnalysePie',
            ExportExcelEnergyAnalyse: 'action.ashx?action=ExportExcelEnergyAnalyse',
            GetQuotaAnalyseChart: 'action.ashx?action=GetQuotaAnalyseChart',
            GetLoadForecastChart: 'action.ashx?action=GetLoadForecastChart',
            GetAlarmDiagnose: 'action.ashx?action=GetAlarmDiagnose',
            GetAlarmType: 'action.ashx?action=GetAlarmType',
            GetCostQuery: 'action.ashx?action=GetCostQuery',
            ExportExcelCostQuery: 'action.ashx?action=ExportExcelCostQuery',
            objectItemTree4Test: 'action.ashx?action=objectItemTree'
            
        };

        var testRequestJson = {
            loginjson: '{"LoginUser":"admin","LoginPass":"1","IsRemeberPass":true}',
            indexwindow: '{"BuildingNumber":2,"StatisticsDate":"2013-11-30 00:00:00"}',
            eneryquery: '{"Starttime":"2013-10-1","EndTime":"2013-11-30","ItemCode":"00000","ObjectNum":889,ObjectType:1,Unit:1}',
            lazydevicetree: '{"AreaID":1,"ItemCode":"01000"}',

            shoporder: '{"StartTime":"2013-10-1","EndTime":"2013-11-30","ItemCode":"00000","ObjectNum":[2,23,36,54,72,91],"OrderWay":"asc","PageCurrent":1,"PageSize":1,Particle:"all"}',
            shoporderNew: '{"StartTime":"2014-03-21","EndTime":"2014-03-21","ItemCode":"01000","AreaIdLst":[2,23,36,54,72,3],"ObjType":"1","QueryType":"8"}',

            indexElectricRealLineChart: '{"AreaID":1,"ItemCode":"01000"}',
            indexItem: '{"ItemCode":"00000"}',
            deviceRealChart: '{"DeviceId":1}',

            indexDeviceList: '{"ObjectId":111,"ItemCode":"01000","ObjType":2}',
            realtime: '{"DeviceId":1}',
            GetSingleUser: '{"UserID":4}',
            AddUser: '{"UserName":"测试3","Password":"1","Status":0,"GroupId":1}',
            UpdateUser: '{"UserID":"2","UserName":"测试","Password":"1","Status":1,"GroupId":2}',
            DeleteUser: '{"UserID":4}',

            GetSingleUserGroup: '{"UserGroupID":3}',
            AddUserGroup: '{"UserGroupName":"测试","Description":"测试","UserGroupMenuRights":[2,23,36,54,72,91],"UserGroupLiquidRights":[2,23,36,54,72,91],"UserGroupAreaRights":[2,23,36,54,72,91]}',
            UpdateUserGroup: '{"UserGroupID":1,"UserGroupName":"超级用户组","Description":"超级用户组","UserGroupMenuRights":[1,2,3],"UserGroupLiquidRights":[4,5,6],"UserGroupAreaRights":[2,23,36]}',
            DeleteUserGroup: '{"UserGroupID":3}',
            GetEnergyAnalyseLineChart: '{"particle":1,"StartTime":"2014-03-20","EndTime":"2014-03-26","ItemCode":"00000","ObjectId":1,"ObjType":"1","QueryType":"1","IsDevice":0,"ObjectChildren":[]}',
            GetQuotaAnalyseChart: '{"Particle":2,"StartTime":"2014-04-01","ItemCode":"01000","ObjectId":1,"ObjType":"1"}',
            GetAlarmDiagnose: '{"StartTime":"2014-01-02","EndTime":"2014-01-02","ObjectId":1}',
           
            IndexContrastObjsChartAll: '{"StartTime":"2013-3-1","EndTime":"2013-3-1","AreaIdLst":[ 2, 23, 36, 54, 72],"ObjType":2,"ItemCode":"00000","Particle":0,"QueryType":1}',
            IndexContrastObjsChartArea: '{"StartTime":"2013-3-1","EndTime":"2013-3-3","AreaIdLst":[ 2, 23, 36, 54, 72],"ObjType":2,"ItemCode":"00000","Particle":0,"QueryType":2}',
            IndexContrastObjsChartPerNum: '{"StartTime":"2013-3-1","EndTime":"2013-3-3","AreaIdLst":[ 2, 23, 36, 54, 72],"ObjType":2,"ItemCode":"00000","Particle":0,"QueryType":3}',
            IndexContrastObjsLst: '{"StartTime":"2013-3-1","EndTime":"2013-3-3","AreaIdLst":[ 2, 23, 36, 54, 72],"ObjType":2,"ItemCode":"00000","Particle":1,"QueryType":3}',
            IndexContrastPeriodsChart: '{"PeriodLst":[{"StartTime":"2013-3-1","EndTime":"2013-3-3"},{"StartTime":"2013-3-4","EndTime":"2013-3-6"}],"AreaId":2,"ObjType":2,"ItemCode":"00000","Particle":1,"QueryType":1}',
            IndexContrastPeriodsChartArea: '{"PeriodLst":[{"StartTime":"2013-3-1","EndTime":"2013-3-3"},{"StartTime":"2013-3-4","EndTime":"2013-3-6"}],"AreaId":2,"ObjType":2,"ItemCode":"00000","Particle":1,"QueryType":2}',
            IndexContrastPeriodsChartPerNum: '{"PeriodLst":[{"StartTime":"2013-3-1","EndTime":"2013-3-3"},{"StartTime":"2013-3-4","EndTime":"2013-3-6"}],"AreaId":2,"ObjType":2,"ItemCode":"00000","Particle":1,"QueryType":3}',
            IndexContrastPeriodssLst: '{"PeriodLst":[{"StartTime":"2013-3-1","EndTime":"2013-3-3"},{"StartTime":"2013-3-4","EndTime":"2013-3-6"}],"AreaId":2,"ObjType":2,"ItemCode":"00000","Particle":1,"QueryType":1}',
            IndexLoadForecastChart: '{"StartTime":"2013-3-1","EndTime":"2013-3-1","ObjType":1,"ItemCode":"00000","ObjectId":"3","Particle":0,"DateUnit":0}',
            GetCostQuery: '{"Particle":2,"StartTime":"2014-04-01","ItemCode":"03000","ObjectId":2,"ObjType":"1"}',
            objectItemTree4Test: '{"ClassId":1,"Level":-1}'
           
        };
        var ExportIDs = {

            shoporderNew: '#shoporderexport',
            energyAnalyse: '#energyAnalyse',
            costQuery: '#costQuery'
            
        };

        function CheckInterface(objUrl, requestData) {
            $.ajax({
                url: objUrl,
                dataType: "text",
                type: "POST",
                data: { "inputs": requestData },
                success: function (datas) {
                    $("#jsonResult").val(datas);
                    // alert(datas);
                },
                error: function () {
                    alert("server's error!");
                }
            });
        }
        function ExportExcel(chartObj, objUrl, requestData) {
            $(chartObj).FileExport({
                url: objUrl,
                data: { Inputs: requestData, TabId: 0 }
            });

        }
    </script>
    <title></title>
    <style>
        
        ul li{width:50%;float:left;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <textarea id="jsonResult" cols="50" rows="10"></textarea><br />
        <ul>
            <li> <a href="javascript:void();" onclick="CheckInterface(testServerUrl.login,testRequestJson.loginjson)">login</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.remmberpass,'')">remmberpass</a> </li>
            <li>&nbsp;kan</li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexmonthenery,'')">indexmonthenery</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexCompare,'')">indexCompare</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexOrderList,'')"> indexOrderList</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexLook,'')">indexLook</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.eneryquery,testRequestJson.eneryquery)"> basicquery</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.objecttree,'')">objecttree</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.devicetree,'')">devicetree</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.lazydevicetree,testRequestJson.lazydevicetree)">lazydevicetree</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.shoporder,testRequestJson.shoporder)">shoporder</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexCompare,'')">总览页本月能耗总览及电能耗综合评价接口</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.shopordernew,testRequestJson.shoporderNew)">能耗排名接口</a></li>
            <li><a href="javascript:void();" id="shoporderexport" onclick="ExportExcel(ExportIDs.shoporderNew,testServerUrl.exportExcelDataRankingNew,testRequestJson.shoporderNew)">能耗排名导出Excel接口</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetMenuModule,'')"> GetMenuModule</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.realtime,testRequestJson.realtime)">realtime</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexElectricRealLineChart,'')">indexElectricRealLineChart(戴)</a> </li>
            <li></li>
            <li></li>
            <li><a href="javascript:void();"onclick="CheckInterface(testServerUrl.indexItem,testRequestJson.indexItem)">indexItem(戴)</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.deviceRealChart,testRequestJson.deviceRealChart)">deviceRealChart(戴)</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.indexDeviceList,testRequestJson.indexDeviceList)">indexDeviceList(戴)</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetUsers,'')"> 获取用户列表</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetSingleUser,testRequestJson.GetSingleUser)">获取单个用户</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.AddUser,testRequestJson.AddUser)"> 新增用户</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.UpdateUser,testRequestJson.UpdateUser)"> 更新用户</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.DeleteUser,testRequestJson.DeleteUser)"> 删除用户</a></li>
           
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetUserGroups,'')"> 获取用户组列表</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetSingleUserGroup,testRequestJson.GetSingleUserGroup)">获取单个用户组</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.AddUserGroup,testRequestJson.AddUserGroup)"> 新增用户组</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.UpdateUserGroup,testRequestJson.UpdateUserGroup)"> 更新用户组</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.DeleteUserGroup,testRequestJson.DeleteUserGroup)"> 删除用户组</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetEnergyAnalyseLineChart,testRequestJson.GetEnergyAnalyseLineChart)"> 能耗分析Chart及表格</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetEnergyAnalyseCompare,testRequestJson.GetEnergyAnalyseLineChart)"> 能耗分析比较</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetEnergyAnalysePie,testRequestJson.GetEnergyAnalyseLineChart)"> 能耗分析环状图</a></li>
           <li><a href="javascript:void();" id="energyAnalyse" onclick="ExportExcel(ExportIDs.energyAnalyse,testServerUrl.ExportExcelEnergyAnalyse,testRequestJson.GetEnergyAnalyseLineChart)">能耗分析表格导出</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetQuotaAnalyseChart,testRequestJson.GetQuotaAnalyseChart)"> 定额分析</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetAlarmDiagnose,testRequestJson.GetAlarmDiagnose)"> 管理诊断</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetCostQuery,testRequestJson.GetCostQuery)"> 费用查询</a></li>
             <li><a href="javascript:void();" id="costQuery" onclick="ExportExcel(ExportIDs.costQuery,testServerUrl.ExportExcelCostQuery,testRequestJson.GetCostQuery)">费用查询导出</a></li>
                         <!--add by jy-->
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.objectItemTree,'')">2种树</a> 
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastObjsChart,testRequestJson.IndexContrastObjsChartAll)">能耗对比[多对象]默认图</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastObjsChart,testRequestJson.IndexContrastObjsChartArea)">能耗对比[多对象]单位面积图</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastObjsChart,testRequestJson.IndexContrastObjsChartPerNum)">能耗对比[多对象]人均图</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastObjsLst,testRequestJson.IndexContrastObjsLst)">能耗对比[多对象]列表</a> </li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastPeriodsChart,testRequestJson.IndexContrastPeriodsChart)">能耗对比[多时间]默认图</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastPeriodsChart,testRequestJson.IndexContrastPeriodsChartArea)">能耗对比[多时间]单位面积图</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastPeriodsChart,testRequestJson.IndexContrastPeriodsChartPerNum)">能耗对比[多时间]人均图</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.IndexContrastPeriodssLst,testRequestJson.IndexContrastPeriodssLst)">能耗对比[多时间]列表</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.ExportQueryObjs,testRequestJson.IndexContrastObjsChartAll)"> 能耗对比[多对象]图导出</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.ExportQueryPeriod,testRequestJson.IndexContrastPeriodsChart)"> 能耗对比[多时间]图导出</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetLoadForecastChart,testRequestJson.IndexLoadForecastChart)">负荷预测</a></li>
            <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.GetAlarmType,'')">告警类型下拉列表</a></li>
             <li><a href="javascript:void();" onclick="CheckInterface(testServerUrl.objectItemTree4Test,testRequestJson.objectItemTree4Test)">数新算法</a> </li>
            <!--add by jy-->
                <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" /><asp:Label ID="Label1" runat="server"
                    Text="Label"></asp:Label>
        </ul>
       

       
    </div>
    </form>
</body>
</html>
