function CheckLength(v, msg) {
    if (v.length > 0) {
        if (v.length > 8) {
            alert(msg + "长度不能大于8个字符");
            return false;
        }
        var isint = MyCommValidate({ rule: "number", value: v });
        if (isint != '') { alert(msg + "格式错误：" + isint); return false; }
    }
    return true;
}

function buildextent() { }
buildextent.prototype = {
    name: '系统建筑扩展js类',
    showbuildbygroupid: function () {
        $(".sub_mid_code#add").html('');
        $(".dd_code#add").val('');
        $("#buildid").val('');
        var buildgroupid = $("#defaultgroupid").val();
        // alert(buildgroupid);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.showbuildbygroupid&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { buildgroupid: buildgroupid },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        //alert(data[i].F_BuildName);
                        $(".sub_mid_code#add").append("<li config=" + data[i].F_BuildID + ">" + data[i].F_BuildName + "</li>");

                    }
                }
                $(".dd_code#add").val($('.sub_mid_code#add li:first').html());
                $("#buildid").val($('.sub_mid_code#add li:first').attr('config'));

                $(".add_dd01#add").mouseover(function () { $(".dd_code_sub#add").show(); });
                $(".add_dd01#add").mouseout(function () { $(".dd_code_sub#add").hide(); });
                // alert($(".sub_mid_code#add li").length);
                $(".sub_mid_code#add li").click(function () {
                    var wt = $(this).html();
                    $(".dd_code#add").val(wt);
                    $("#buildid").val($(this).attr('config'));
                });
            }
        });

    },
    addbuildextent: function () {

        var buildid = $("#buildid").val();
        var F_WorkerNum = $("#F_WorkerNum").val();
        var F_CustomerNum = $("#F_CustomerNum").val();
        var F_OpenHours = $("#F_OpenHours").val();
        var F_ServiceLevel = $("#F_ServiceLevel").val();
        var F_HotelLiveRate = $("#F_HotelLiveRate").val();
        var F_HotelBedNum = $("#F_HotelBedNum").val();

        var F_VisitorNum = $("#F_VisitorNum").val();
        var F_StudentNum = $("#F_StudentNum").val();
        var F_HospitalStandard = $("#F_HospitalStandard").val();
        var F_HospitalType = $("#F_HospitalType").val();
        var F_PatientNum = $("#F_PatientNum").val();
        var F_HospitalBedNum = $("#F_HospitalBedNum").val();
        var F_SpectatorNum = $("#F_SpectatorNum").val();
        var F_GroupFunc = $("#F_GroupFunc").val();
        var F_ExtendFunc = $("#F_ExtendFunc").val();
        var F_ElectriPrice = $("#F_ElectriPrice").val();
        var F_WaterPrice = $("#F_WaterPrice").val();

        var F_GasPrice = $("#F_GasPrice").val();
        var F_HeatPrice = $("#F_HeatPrice").val();
        var F_OtherPrice = $("#F_OtherPrice").val();

        if (!CheckLength(F_WorkerNum, "办公人数")) { return false; }
        if (!CheckLength(F_CustomerNum, "商场日均客流量")) { return false; }
        if (!CheckLength(F_OpenHours, "运营时间小时数")) { return false; }
        if (!CheckLength(F_HotelBedNum, "宾馆床位数量")) { return false; }
        if (!CheckLength(F_VisitorNum, "建筑参观人数")) { return false; }
       if (! CheckLength(F_StudentNum, "学生人数")) { return false; }
       if (!CheckLength(F_PatientNum, "就诊人数")) { return false; }

        // 数据验证区域
        var data = {
            buildid: buildid,
            F_WorkerNum: F_WorkerNum,
            F_CustomerNum: F_CustomerNum,
            F_OpenHours: F_OpenHours,
            F_ServiceLevel: F_ServiceLevel,
            F_HotelLiveRate: F_HotelLiveRate,
            F_HotelBedNum: F_HotelBedNum,
            F_VisitorNum: F_VisitorNum,
            F_StudentNum: F_StudentNum,
            F_HospitalStandard: F_HospitalStandard,
            F_HospitalType: F_HospitalType,
            F_PatientNum: F_PatientNum,
            F_HospitalBedNum: F_HospitalBedNum,
            F_SpectatorNum: F_SpectatorNum,
            F_GroupFunc: F_GroupFunc,
            F_ExtendFunc: F_ExtendFunc,
            F_ElectriPrice: F_ElectriPrice,
            F_WaterPrice: F_WaterPrice,
            F_GasPrice: F_GasPrice,
            F_HeatPrice: F_HeatPrice,
            F_OtherPrice: F_OtherPrice
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildExInfo.addbuildextent&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('添加成功');
                    window.location = "building-extend.aspx?bid=" + new Date().getTime();
                    DivCloseUp6()
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showbuildextent: function (bid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildExInfo.showbuildextent&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: bid },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);

                    $("#ebuildid").val(data[0].F_BuildID);
                    $("#EF_WorkerNum").val(data[0].F_WorkerNum);
                    $("#EF_CustomerNum").val(data[0].F_CustomerNum);
                    $("#EF_OpenHours").val(data[0].F_OpenHours);
                    $("#EF_ServiceLevel").val(data[0].F_ServiceLevel);
                    $("#EF_HotelLiveRate").val(data[0].F_HotelLiveRate);
                    $("#EF_HotelBedNum").val(data[0].F_HotelBedNum);

                    $("#EF_VisitorNum").val(data[0].F_VisitorNum);
                    $("#EF_StudentNum").val(data[0].F_StudentNum);
                    $("#EF_HospitalStandard").val(data[0].F_HospitalStandard);
                    $("#EF_HospitalType").val(data[0].F_HospitalType);
                    $("#EF_PatientNum").val(data[0].F_PatientNum);
                    $("#EF_HospitalBedNum").val(data[0].F_HospitalBedNum);
                    $("#EF_SpectatorNum").val(data[0].F_SpectatorNum);
                    $("#EF_GroupFunc").val(data[0].F_GroupFunc);
                    $("#EF_ExtendFunc").val(data[0].F_ExtendFunc);
                    $("#EF_ElectriPrice").val(data[0].F_ElectriPrice);
                    $("#EF_WaterPrice").val(data[0].F_WaterPrice);

                    $("#EF_GasPrice").val(data[0].F_GasPrice);
                    $("#EF_HeatPrice").val(data[0].F_HeatPrice);
                    $("#EF_OtherPrice").val(data[0].F_OtherPrice);
                    $(".dd_code#edit").val($.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.getbuildname&dll=NTS_BECM.BLL&buildid=" + data[0].F_BuildID + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText);
                    /*
                    $("#EF_FileID").val(data[0].F_FileID);
                    $("#Ebuildid").val(data[0].F_BuildID);
                    $("#EF_FileFuncType").val(data[0].F_FileFuncType);
                    $("#EF_FileFormatType").val(data[0].F_FileFormatType);
                    $("#EF_FileDesc").val(data[0].F_FileDesc);
                    $("#EF_FileSize").val(data[0].F_FileSize);
                    $("#EF_CreateTime").val(data[0].F_FileDate.replace("0:00:00", ""));
                    $("#EF_FilePath").val(data[0].F_FilePath);
                    */
                }
            }
        });
    },
    deletebuildextent: function (bid) {

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildExInfo.deletebuildextent&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: bid },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    //alert('添加成功');
                    window.location = "building-extend.aspx?bid=" + new Date().getTime();
                    //DivCloseUp6()
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    updatebuildextent: function () {
        var buildid = $("#ebuildid").val();
        var F_WorkerNum = $("#EF_WorkerNum").val();
        var F_CustomerNum = $("#EF_CustomerNum").val();
        var F_OpenHours = $("#EF_OpenHours").val();
        var F_ServiceLevel = $("#EF_ServiceLevel").val();
        var F_HotelLiveRate = $("#EF_HotelLiveRate").val();
        var F_HotelBedNum = $("#EF_HotelBedNum").val();

        var F_VisitorNum = $("#EF_VisitorNum").val();
        var F_StudentNum = $("#EF_StudentNum").val();
        var F_HospitalStandard = $("#EF_HospitalStandard").val();
        var F_HospitalType = $("#EF_HospitalType").val();
        var F_PatientNum = $("#EF_PatientNum").val();
        var F_HospitalBedNum = $("#EF_HospitalBedNum").val();
        var F_SpectatorNum = $("#EF_SpectatorNum").val();
        var F_GroupFunc = $("#EF_GroupFunc").val();
        var F_ExtendFunc = $("#EF_ExtendFunc").val();
        var F_ElectriPrice = $("#EF_ElectriPrice").val();
        var F_WaterPrice = $("#EF_WaterPrice").val();

        var F_GasPrice = $("#EF_GasPrice").val();
        var F_HeatPrice = $("#EF_HeatPrice").val();
        var F_OtherPrice = $("#EF_OtherPrice").val();

        if (!CheckLength(F_WorkerNum, "办公人数")) { return false; }
        if (!CheckLength(F_CustomerNum, "商场日均客流量")) { return false; }
        if (!CheckLength(F_OpenHours, "运营时间小时数")) { return false; }
        if (!CheckLength(F_HotelBedNum, "宾馆床位数量")) { return false; }
        if (!CheckLength(F_VisitorNum, "建筑参观人数")) { return false; }
        if (!CheckLength(F_StudentNum, "学生人数")) { return false; }
        if (!CheckLength(F_PatientNum, "就诊人数")) { return false; }

        // 数据验证区域
        var data = {
            buildid: buildid,
            F_WorkerNum: F_WorkerNum,
            F_CustomerNum: F_CustomerNum,
            F_OpenHours: F_OpenHours,
            F_ServiceLevel: F_ServiceLevel,
            F_HotelLiveRate: F_HotelLiveRate,
            F_HotelBedNum: F_HotelBedNum,
            F_VisitorNum: F_VisitorNum,
            F_StudentNum: F_StudentNum,
            F_HospitalStandard: F_HospitalStandard,
            F_HospitalType: F_HospitalType,
            F_PatientNum: F_PatientNum,
            F_HospitalBedNum: F_HospitalBedNum,
            F_SpectatorNum: F_SpectatorNum,
            F_GroupFunc: F_GroupFunc,
            F_ExtendFunc: F_ExtendFunc,
            F_ElectriPrice: F_ElectriPrice,
            F_WaterPrice: F_WaterPrice,
            F_GasPrice: F_GasPrice,
            F_HeatPrice: F_HeatPrice,
            F_OtherPrice: F_OtherPrice
        }

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildExInfo.updatebuildextent&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('修改成功');
                    window.location = "building-extend.aspx?bid=" + new Date().getTime();
                    DivCloseUp6();
                } else {
                    alert(data.msg);
                }
            }
        });
    }
}


$(function () {
    /*下拉菜单1-1*/
    $(".xialabox").mouseover(function () { $(".sub_box").show(); });
    $(".xialabox").mouseout(function () { $(".sub_box").hide(); });
    $(".sub_box ul li").click(function () {
        var ww = $(this).html();
        $("span.xltt").html(ww);
        //alert($(this).attr('config'));
        // $("#defaultdatacenterid").val($(this).attr('config'));


    });
    /*下拉菜单1-2*/
    $(".xialabox2").mouseover(function () { $(".sub_box2").show(); });
    $(".xialabox2").mouseout(function () { $(".sub_box2").hide(); });
    $(".sub_box2 ul li").click(function () {
        var ww = $(this).html();
        $("span.xltt2").html(ww);
        $("#defaultgroupid").val($(this).attr('config'));
    });

    

    $(".dd13#add").mouseover(function () { $(".dd_group_sub#add").show(); });
    $(".dd13#add").mouseout(function () { $(".dd_group_sub#add").hide(); });
    $("ul.sub_mid_group#add li").click(function () {
        var wt = $(this).html();
        $("#F_BuildGroupIDText").val(wt);
        $("#F_BuildGroupID").val($(this).attr('config'));
        // $(".dd_group").val(wt);
    });


    /*下拉菜单3*/
    $(".dd02#add").mouseover(function () { $(".dd_check_sub#add").show(); });
    $(".dd02#add").mouseout(function () { $(".dd_check_sub#add").hide(); });
    $("ul.sub_mid_check#add li").click(function () {
        var wt = $(this).html();
        // $(".dd_check").val(wt);
        $("#F_StateText").val(wt);
        $("#F_State").val($(this).attr('config'));
    });
    /*下拉菜单4*/
    $(".dd03#add").mouseover(function () { $(".dd_function_sub#add").show(); });
    $(".dd03#add").mouseout(function () { $(".dd_function_sub#add").hide(); });
    $("ul.sub_mid_function#add li").click(function () {
        var wt = $(this).html();
        //$(".dd_function").val(wt);
        $("#BuildFuncText").val(wt);
        $("#BuildFunc").val($(this).attr('config'));
    });
    /*下拉菜单5*/
    $(".dd04#add").mouseover(function () { $(".dd_aircond_sub#add").show(); });
    $(".dd04#add").mouseout(function () { $(".dd_aircond_sub#add").hide(); });
    $("ul.sub_mid_aircond#add li").click(function () {
        var wt = $(this).html();
        $("#F_AirTypeText").val(wt);
        $("#F_AirType").val($(this).attr('config'));

        // $(".dd_aircond#add").val(wt);
    });
    /*下拉菜单6*/
    $(".dd05#add").mouseover(function () { $(".dd_heating_sub#add").show(); });
    $(".dd05#add").mouseout(function () { $(".dd_heating_sub#add").hide(); });
    $("ul.sub_mid_heating#add li").click(function () {
        var wt = $(this).html();
        // $(".dd_heating").val(wt);
        $("#F_HeatTypeText").val(wt);
        $("#F_HeatType").val($(this).attr('config'));
    });

    /*下拉菜单7*/
    $(".dd06#add").mouseover(function () { $(".dd_structure_sub#add").show(); });
    $(".dd06#add").mouseout(function () { $(".dd_structure_sub#add").hide(); });
    $("ul.sub_mid_structure#add li").click(function () {
        var wt = $(this).html();
        //$(".dd_structure").val(wt);
        $("#F_StruTypeText").val(wt);
        $("#F_StruType").val($(this).attr('config'));
    });
    /*下拉菜单8*/
    $(".dd07#add").mouseover(function () { $(".dd_wallmaterial_sub#add").show(); });
    $(".dd07#add").mouseout(function () { $(".dd_wallmaterial_sub#add").hide(); });
    $("ul.sub_mid_wallmaterial#add li").click(function () {
        var wt = $(this).html();
        $("#F_WallMatTypeText").val(wt);
        $("#F_WallMatType").val($(this).attr('config'));
    });

    /*下拉菜单9*/
    $(".dd08#add").mouseover(function () { $(".dd_wallheating_sub#add").show(); });
    $(".dd08#add").mouseout(function () { $(".dd_wallheating_sub#add").hide(); });
    $("ul.sub_mid_wallheating#add li").click(function () {
        var wt = $(this).html();
        $("#F_WallWarmTypeText").val(wt);
        $("#F_WallWarmType").val($(this).attr('config'));
    });
    /*下拉菜单10*/
    $(".dd09#add").mouseover(function () { $(".dd_outwindow_sub#add").show(); });
    $(".dd09#add").mouseout(function () { $(".dd_outwindow_sub#add").hide(); });
    $("ul.sub_mid_outwindow#add li").click(function () {
        var wt = $(this).html();
        $("#F_WallWinTypeText").val(wt);
        $("#F_WallWinType").val($(this).attr('config'));
    });
    /*下拉菜单11*/
    $(".dd10#add").mouseover(function () { $(".dd_glass_sub#add").show(); });
    $(".dd10#add").mouseout(function () { $(".dd_glass_sub#add").hide(); });
    $("ul.sub_mid_glass#add li").click(function () {
        var wt = $(this).html();
        //$(".dd_glass").val(wt);
        $("#F_GlassTypeText").val(wt);
        $("#F_GlassType").val($(this).attr('config'));
    });
    /*下拉菜单12*/
    $(".dd11#add").mouseover(function () { $(".dd_windowmaterial_sub#add").show(); });
    $(".dd11#add").mouseout(function () { $(".dd_windowmaterial_sub#add").hide(); });
    $("ul.sub_mid_windowmaterial#add li").click(function () {
        var wt = $(this).html();
        $("#F_WinFrameTypeText").val(wt);
        $("#F_WinFrameType").val($(this).attr('config'));
    });
    /*下拉菜单13*/
    $(".dd12#add").mouseover(function () { $(".dd_biaogan_sub#add").show(); });
    $(".dd12#add").mouseout(function () { $(".dd_biaogan_sub#add").hide(); });
    $("ul.sub_mid_biaogan#add li").click(function () {
        var wt = $(this).html();
        $("#F_IsStandardText").val(wt);
        $("#F_IsStandard").val($(this).attr('config'));

    });
    // 初始化录入下拉框
    $(function () {



        $("#F_BuildID").blur(function () {
            if ($(this).val().length == 10) {
                var n = $(this).val().substring(6, 7);
                var dcode = $(this).val().substring(0, 6);
                $("#F_AliasName").val(n);
                $("#F_DistrictCode").val(dcode);
            }
        });

        /*初始化时间*/
        $('#F_CreateTime').val($('#s').val());
        $('#F_MonitorDate').val($('#s').val());
        $('#F_AcceptDate').val($('#s').val());

        var dd2 = $("ul.sub_mid_group#add li:first");
        $("#F_BuildGroupIDText").val(dd2.html());
        $("#F_BuildGroupID").val(dd2.attr('config'));
        var dd3 = $("ul.sub_mid_check#add li:first");
        $("#F_StateText").val(dd3.html());
        $("#F_State").val(dd3.attr('config'));
        var dd4 = $("ul.sub_mid_function#add li:first");
        $("#BuildFuncText").val(dd4.html());
        $("#BuildFunc").val(dd4.attr('config'));
        var dd5 = $("ul.sub_mid_aircond#add li:first");
        $("#F_AirTypeText").val(dd5.html());
        $("#F_AirType").val(dd5.attr('config'));
        var dd6 = $("ul.sub_mid_heating#add li:first");
        $("#F_HeatTypeText").val(dd6.html());
        $("#F_HeatType").val(dd6.attr('config'));
        var dd7 = $("ul.sub_mid_structure#add li:first");
        $("#F_StruTypeText").val(dd7.html());
        $("#F_StruType").val(dd7.attr('config'));
        var dd8 = $("ul.sub_mid_wallmaterial#add li:first");
        $("#F_WallMatTypeText").val(dd8.html());
        $("#F_WallMatType").val(dd8.attr('config'));
        var dd9 = $("ul.sub_mid_wallheating#add li:first");
        $("#F_WallWarmTypeText").val(dd9.html());
        $("#F_WallWarmType").val(dd9.attr('config'));
        var dd10 = $("ul.sub_mid_outwindow#add li:first");
        $("#F_WallWinTypeText").val(dd10.html());
        $("#F_WallWinType").val(dd10.attr('config'));
        var dd11 = $("ul.sub_mid_glass#add li:first");
        $("#F_GlassTypeText").val(dd11.html());
        $("#F_GlassType").val(dd11.attr('config'));
        var dd12 = $("ul.sub_mid_windowmaterial#add li:first");
        $("#F_WinFrameTypeText").val(dd12.html());
        $("#F_WinFrameType").val(dd12.attr('config'));
        var dd13 = $("ul.sub_mid_biaogan#add li:first");
        $("#F_IsStandardText").val(dd13.html());
        $("#F_IsStandard").val(dd13.attr('config'));
    });

    /*列表点击*/
    jQuery(".dd_center_list ul li").click(function () {
        var index = jQuery(".dd_center_list ul li").index(this);
        jQuery(this).addClass("imgclicksub").siblings().removeClass("imgclicksub");
        //jQuery(".top").append(boarddiv);			 
    });
});


