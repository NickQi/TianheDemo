$(function () {
    //alert($(".dd02#edit")[0]);
    /*下拉菜单2*/
    $(".dd01#edit").mouseover(function () { $(".dd_name_sub#edit").show(); });
    $(".dd01#edit").mouseout(function () { $(".dd_name_sub#edit").hide(); });
    // alert($(".dd01#edit")[0]);
    
    $("ul.sub_mid#edit li").click(function () {
    var wt = $(this).html();
    $("#EF_DataCenterIDText").val(wt); // 给添加的数据中心赋值
    $("#EF_DataCenterID").val($(this).attr('config')); // 给添加的数据中心赋值
       
    var issend = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_DC_DataCenterBaseInfo.issend&dll=NTS_BECM.BLL&times=" + new Date().getTime(), data: { cid: $(this).attr('config') }, type: 'GET', async: false, cache: false }).responseText;
    if (issend == 'y') {
    // 建筑字母别名
    //$("#F_AliasName").css("background-color", "#eeeeee");
    // $("#F_AliasName").get(0).disabled = true;
    // 建筑业主
    $("#EF_BuildOwner").css("background-color", "#eeeeee");
    $("#EF_BuildOwner").get(0).disabled = true;
    // 建筑地址
    $("#EF_BuildAddr").css("background-color", "#eeeeee");
    $("#EF_BuildAddr").get(0).disabled = true;
    // 建筑坐标-经度
    $("#EF_BuildLong").css("background-color", "#eeeeee");
    $("#EF_BuildLong").get(0).disabled = true;
    // 建筑坐标-纬度
    $("#EF_BuildLat").css("background-color", "#eeeeee");
    $("#EF_BuildLat").get(0).disabled = true;
    // 建筑年代
    $("#EF_BuildYear").css("background-color", "#eeeeee");
    $("#EF_BuildYear").get(0).disabled = true;
    // 地上建筑层数
    $("#F_UpFloor").css("background-color", "#eeeeee");
    $("#F_UpFloor").get(0).disabled = true;
    // 地下建筑层数
    $("#EF_DownFloor").css("background-color", "#eeeeee");
    $("#EF_DownFloor").get(0).disabled = true;
    // 建设体型系数
    $("#EF_BodyCoef").css("background-color", "#eeeeee");
    $("#EF_BodyCoef").get(0).disabled = true;
    // 检测方案设计单位
    $("#EF_DesignDept").css("background-color", "#eeeeee");
    $("#EF_DesignDept").get(0).disabled = true;
    // 检测工程实施单位
    $("#EF_WorkDept").css("background-color", "#eeeeee");
    $("#EF_WorkDept").get(0).disabled = true;
    // 创建操作员名称
    $("#EF_CreateUser").css("background-color", "#eeeeee");
    $("#EF_CreateUser").get(0).disabled = true;
    } else {
    $("#EF_BuildOwner").css("background-color", "#ffffff");
    $("#EF_BuildOwner").get(0).disabled = false;
    // 建筑地址
    $("#EF_BuildAddr").css("background-color", "#ffffff");
    $("#EF_BuildAddr").get(0).disabled = false;
    // 建筑坐标-经度
    $("#EF_BuildLong").css("background-color", "#ffffff");
    $("#EF_BuildLong").get(0).disabled = false;
    // 建筑坐标-纬度
    $("#EF_BuildLat").css("background-color", "#ffffff");
    $("#EF_BuildLat").get(0).disabled = false;
    // 建筑年代
    $("#EF_BuildYear").css("background-color", "#ffffff");
    $("#EF_BuildYear").get(0).disabled = false;
    // 地上建筑层数
    $("#EF_UpFloor").css("background-color", "#ffffff");
    $("#EF_UpFloor").get(0).disabled = false;
    // 地下建筑层数
    $("#EF_DownFloor").css("background-color", "#ffffff");
    $("#EF_DownFloor").get(0).disabled = false;
    // 建设体型系数
    $("#EF_BodyCoef").css("background-color", "#ffffff");
    $("#EF_BodyCoef").get(0).disabled = false;
    // 检测方案设计单位
    $("#EF_DesignDept").css("background-color", "#ffffff");
    $("#EF_DesignDept").get(0).disabled = false;
    // 检测工程实施单位
    $("#EF_WorkDept").css("background-color", "#ffffff");
    $("#EF_WorkDept").get(0).disabled = false;
    // 创建操作员名称
    $("#EF_CreateUser").css("background-color", "#ffffff");
    $("#EF_CreateUser").get(0).disabled = false;
    }
    
    });

    $(".dd13#edit").mouseover(function () { $(".dd_group_sub#edit").show(); });
    $(".dd13#edit").mouseout(function () { $(".dd_group_sub#edit").hide(); });
    $("ul.sub_mid_group#edit li").click(function () {
        //alert('xasxsaccsa');
        var wt = $(this).html();
        $("#EF_BuildGroupIDText").val(wt);
        $("#EF_BuildGroupID").val($(this).attr('config'));
        // $(".dd_group").val(wt);
    });


    /*下拉菜单3*/
    $(".dd02#edit").mouseover(function () { $(".dd_check_sub#edit").show(); });
    $(".dd02#edit").mouseout(function () { $(".dd_check_sub#edit").hide(); });
    $("ul.sub_mid_check#edit li").click(function () {
        var wt = $(this).html();
        // $(".dd_check").val(wt);
        $("#EF_StateText").val(wt);
        $("#EF_State").val($(this).attr('config'));
    });
    /*下拉菜单4*/
    $(".dd03#edit").mouseover(function () { $(".dd_function_sub#edit").show(); });
    $(".dd03#edit").mouseout(function () { $(".dd_function_sub#edit").hide(); });
    $("ul.sub_mid_function#edit li").click(function () {
        var wt = $(this).html();
        //$(".dd_function").val(wt);
        $("#EBuildFuncText").val(wt);
        $("#EBuildFunc").val($(this).attr('config'));
    });
    /*下拉菜单5*/
    $(".dd04#edit").mouseover(function () { $(".dd_aircond_sub#edit").show(); });
    $(".dd04#edit").mouseout(function () { $(".dd_aircond_sub#edit").hide(); });
    $("ul.sub_mid_aircond#edit li").click(function () {
        var wt = $(this).html();
        $("#EF_AirTypeText").val(wt);
        $("#EF_AirType").val($(this).attr('config'));

        // $(".dd_aircond#edit").val(wt);
    });
    /*下拉菜单6*/
    $(".dd05#edit").mouseover(function () { $(".dd_heating_sub#edit").show(); });
    $(".dd05#edit").mouseout(function () { $(".dd_heating_sub#edit").hide(); });
    $("ul.sub_mid_heating#edit li").click(function () {
        var wt = $(this).html();
        // $(".dd_heating").val(wt);
        $("#EF_HeatTypeText").val(wt);
        $("#EF_HeatType").val($(this).attr('config'));
    });

    /*下拉菜单7*/
    $(".dd06#edit").mouseover(function () { $(".dd_structure_sub#edit").show(); });
    $(".dd06#edit").mouseout(function () { $(".dd_structure_sub#edit").hide(); });
    $("ul.sub_mid_structure#edit li").click(function () {
        var wt = $(this).html();
        //$(".dd_structure").val(wt);
        $("#EF_StruTypeText").val(wt);
        $("#EF_StruType").val($(this).attr('config'));
    });
    /*下拉菜单8*/
    $(".dd07#edit").mouseover(function () { $(".dd_wallmaterial_sub#edit").show(); });
    $(".dd07#edit").mouseout(function () { $(".dd_wallmaterial_sub#edit").hide(); });
    $("ul.sub_mid_wallmaterial#edit li").click(function () {
        var wt = $(this).html();
        $("#EF_WallMatTypeText").val(wt);
        $("#EF_WallMatType").val($(this).attr('config'));
    });

    /*下拉菜单9*/
    $(".dd08#edit").mouseover(function () { $(".dd_wallheating_sub#edit").show(); });
    $(".dd08#edit").mouseout(function () { $(".dd_wallheating_sub#edit").hide(); });
    $("ul.sub_mid_wallheating#edit li").click(function () {
        var wt = $(this).html();
        $("#EF_WallWarmTypeText").val(wt);
        $("#EF_WallWarmType").val($(this).attr('config'));
    });
    /*下拉菜单10*/
    $(".dd09#edit").mouseover(function () { $(".dd_outwindow_sub#edit").show(); });
    $(".dd09#edit").mouseout(function () { $(".dd_outwindow_sub#edit").hide(); });
    $("ul.sub_mid_outwindow#edit li").click(function () {
        var wt = $(this).html();
        $("#EF_WallWinTypeText").val(wt);
        $("#EF_WallWinType").val($(this).attr('config'));
    });
    /*下拉菜单11*/
    $(".dd10#edit").mouseover(function () { $(".dd_glass_sub#edit").show(); });
    $(".dd10#edit").mouseout(function () { $(".dd_glass_sub#edit").hide(); });
    $("ul.sub_mid_glass#edit li").click(function () {
        var wt = $(this).html();
        //$(".dd_glass").val(wt);
        $("#EF_GlassTypeText").val(wt);
        $("#EF_GlassType").val($(this).attr('config'));
    });
    /*下拉菜单12*/
    $(".dd11#edit").mouseover(function () { $(".dd_windowmaterial_sub#edit").show(); });
    $(".dd11#edit").mouseout(function () { $(".dd_windowmaterial_sub#edit").hide(); });
    $("ul.sub_mid_windowmaterial#edit li").click(function () {
        var wt = $(this).html();
        $("#EF_WinFrameTypeText").val(wt);
        $("#EF_WinFrameType").val($(this).attr('config'));
    });
    /*下拉菜单13*/
    $(".dd12#edit").mouseover(function () { $(".dd_biaogan_sub#edit").show(); });
    $(".dd12#edit").mouseout(function () { $(".dd_biaogan_sub#edit").hide(); });
    $("ul.sub_mid_biaogan#edit li").click(function () {
        var wt = $(this).html();
        $("#EF_IsStandardText").val(wt);
        $("#EF_IsStandard").val($(this).attr('config'));

    });
});
    // 初始化录入下拉框
    $(function () {

        
        /*
        $("#F_BuildID").blur(function () {
            if ($(this).val().length == 10) {
                var n = $(this).val().substring(6, 7);
                var dcode = $(this).val().substring(0, 6);
                $("#F_AliasName").val(n);
                $("#F_DistrictCode").val(dcode);
            }
        });*/

        
        
    });