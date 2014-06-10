

function additional() { }
additional.prototype = {
    name: '系统建筑js类',
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
                //alert(data);
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
    addadditional: function () {
        var F_FileID = $("#F_FileID").val();
        var buildid = $("#buildid").val();
        var F_FileFuncType = $("#F_FileFuncType").val();
        var F_FileFormatType = $("#F_FileFormatType").val();
        var F_FileDesc = $("#F_FileDesc").val();
        var F_FileSize = $("#F_FileSize").val();
        var F_CreateTime = $("#F_CreateTime").val();
        var F_FilePath = $("#F_FilePath").val();

        if (F_FileID.length == 0) {
            alert('请输入附加文件顺序号');
            return false;
        }
        if (F_FileID.length > 14) {
            alert('附加文件顺序号的长度不能大于14个字符');
            return false;
        }
        if (F_FileFuncType.length > 1) {
            alert('文件功能类型不能大于1个字符');
            return false;
        }
        if (F_FileFormatType.length > 14) {
            alert('文件格式类型不能大于1个字符');
            return false;
        }
        if (F_FileDesc.length > 80) {
            alert('文件描述的长度不能大于14个字符');
            return false;
        }
        if (F_FileSize.length > 8) {
            alert('文件大小的长度不能大于8个字符');
            return false;
        }
        var number = MyCommValidate({ rule: "number", value: F_FileSize });
        if (number != '') { alert("文件大小的格式错误：" + number); return false; }

        // 数据验证区域
        var data = {
            F_FileID: F_FileID,
            buildid: buildid,
            F_FileFuncType: F_FileFuncType,
            F_FileFormatType: F_FileFormatType,
            F_FileDesc: F_FileDesc,
            F_FileSize: F_FileSize,
            F_CreateTime: F_CreateTime,
            F_FilePath: F_FilePath
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildAddFile.addadditional&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
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
                    window.location = "building-additional.aspx?bid=" + new Date().getTime();
                    DivCloseUp6();
                    jQuery(".popupbox").hide();
                    jQuery("#popupadd").hide();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showadditional: function (bid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildAddFile.showadditional&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: bid },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);

                    $("#EF_FileID").val(data[0].F_FileID);
                    $("#Ebuildid").val(data[0].F_BuildID);
                    $("#EF_FileFuncType").val(data[0].F_FileFuncType);
                    $("#EF_FileFormatType").val(data[0].F_FileFormatType);
                    $("#EF_FileDesc").val(data[0].F_FileDesc);
                    $("#EF_FileSize").val(data[0].F_FileSize);
                    $("#EF_CreateTime").val(data[0].F_FileDate.replace("0:00:00", ""));
                    $("#EF_FilePath").val(data[0].F_FilePath);
                }
            }
        });
    },
    deleteadditional: function (bid) {

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildAddFile.deleteadditional&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
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
                    window.location = "building-additional.aspx?bid=" + new Date().getTime();
                    //DivCloseUp6()
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    updateadditional: function () {
        var F_FileID = $("#EF_FileID").val();
        var buildid = $("#ebuildid").val();
        var F_FileFuncType = $("#EF_FileFuncType").val();
        var F_FileFormatType = $("#EF_FileFormatType").val();
        var F_FileDesc = $("#EF_FileDesc").val();
        var F_FileSize = $("#EF_FileSize").val();
        var F_CreateTime = $("#EF_CreateTime").val();
        var F_FilePath = $("#EF_FilePath").val();
        var id = $("#id").val();
        if (F_FileID.length == 0) {
            alert('请输入附加文件顺序号');
            return false;
        }
        if (F_FileID.length > 14) {
            alert('附加文件顺序号的长度不能大于14个字符');
            return false;
        }
        if (F_FileFuncType.length > 1) {
            alert('文件功能类型不能大于1个字符');
            return false;
        }
        if (F_FileFormatType.length > 14) {
            alert('文件格式类型不能大于1个字符');
            return false;
        }
        if (F_FileDesc.length > 80) {
            alert('文件描述的长度不能大于14个字符');
            return false;
        }
        if (F_FileSize.length > 8) {
            alert('文件大小的长度不能大于8个字符');
            return false;
        }
        var number = MyCommValidate({ rule: "number", value: F_FileSize });
        if (number != '') { alert("文件大小的格式错误：" + number); return false; }
        // 数据验证区域
        var data = {
            id: id,
            F_FileID: F_FileID,
            buildid: buildid,
            F_FileFuncType: F_FileFuncType,
            F_FileFormatType: F_FileFormatType,
            F_FileDesc: F_FileDesc,
            F_FileSize: F_FileSize,
            F_CreateTime: F_CreateTime,
            F_FilePath: F_FilePath
        }

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildAddFile.updateadditional&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
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
                    window.location = "building-additional.aspx?bid=" + new Date().getTime();
                    DivCloseUp6();
                    jQuery(".popupbox").hide();
                    jQuery("#popupadd").hide();
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

    /*下拉菜单2*/
    $(".dd01#add").mouseover(function () { $(".dd_name_sub#add").show(); });
    $(".dd01#add").mouseout(function () { $(".dd_name_sub#add").hide(); });
    $("ul.sub_mid#add li").click(function () {
        var wt = $(this).html();
        $("#F_DataCenterIDText").val(wt); // 给添加的数据中心赋值
        $("#F_DataCenterID").val($(this).attr('config')); // 给添加的数据中心赋值
        /*ajax方式灰掉不需要填写的*/
        var issend = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_DC_DataCenterBaseInfo.issend&dll=NTS_BECM.BLL&times=" + new Date().getTime(), data: { cid: $(this).attr('config') }, type: 'GET', async: false, cache: false }).responseText;
        if (issend == 'y') {
            // 建筑字母别名
            //$("#F_AliasName").css("background-color", "#eeeeee");
            // $("#F_AliasName").get(0).disabled = true;
            // 建筑业主
            $("#F_BuildOwner").css("background-color", "#eeeeee");
            $("#F_BuildOwner").get(0).disabled = true;
            // 建筑地址
            $("#F_BuildAddr").css("background-color", "#eeeeee");
            $("#F_BuildAddr").get(0).disabled = true;
            // 建筑坐标-经度
            $("#F_BuildLong").css("background-color", "#eeeeee");
            $("#F_BuildLong").get(0).disabled = true;
            // 建筑坐标-纬度
            $("#F_BuildLat").css("background-color", "#eeeeee");
            $("#F_BuildLat").get(0).disabled = true;
            // 建筑年代
            $("#F_BuildYear").css("background-color", "#eeeeee");
            $("#F_BuildYear").get(0).disabled = true;
            // 地上建筑层数
            $("#F_UpFloor").css("background-color", "#eeeeee");
            $("#F_UpFloor").get(0).disabled = true;
            // 地下建筑层数
            $("#F_DownFloor").css("background-color", "#eeeeee");
            $("#F_DownFloor").get(0).disabled = true;
            // 建设体型系数
            $("#F_BodyCoef").css("background-color", "#eeeeee");
            $("#F_BodyCoef").get(0).disabled = true;
            // 检测方案设计单位
            $("#F_DesignDept").css("background-color", "#eeeeee");
            $("#F_DesignDept").get(0).disabled = true;
            // 检测工程实施单位
            $("#F_WorkDept").css("background-color", "#eeeeee");
            $("#F_WorkDept").get(0).disabled = true;
            // 创建操作员名称
            $("#F_CreateUser").css("background-color", "#eeeeee");
            $("#F_CreateUser").get(0).disabled = true;
        } else {
            $("#F_BuildOwner").css("background-color", "#ffffff");
            $("#F_BuildOwner").get(0).disabled = false;
            // 建筑地址
            $("#F_BuildAddr").css("background-color", "#ffffff");
            $("#F_BuildAddr").get(0).disabled = false;
            // 建筑坐标-经度
            $("#F_BuildLong").css("background-color", "#ffffff");
            $("#F_BuildLong").get(0).disabled = false;
            // 建筑坐标-纬度
            $("#F_BuildLat").css("background-color", "#ffffff");
            $("#F_BuildLat").get(0).disabled = false;
            // 建筑年代
            $("#F_BuildYear").css("background-color", "#ffffff");
            $("#F_BuildYear").get(0).disabled = false;
            // 地上建筑层数
            $("#F_UpFloor").css("background-color", "#ffffff");
            $("#F_UpFloor").get(0).disabled = false;
            // 地下建筑层数
            $("#F_DownFloor").css("background-color", "#ffffff");
            $("#F_DownFloor").get(0).disabled = false;
            // 建设体型系数
            $("#F_BodyCoef").css("background-color", "#ffffff");
            $("#F_BodyCoef").get(0).disabled = false;
            // 检测方案设计单位
            $("#F_DesignDept").css("background-color", "#ffffff");
            $("#F_DesignDept").get(0).disabled = false;
            // 检测工程实施单位
            $("#F_WorkDept").css("background-color", "#ffffff");
            $("#F_WorkDept").get(0).disabled = false;
            // 创建操作员名称
            $("#F_CreateUser").css("background-color", "#ffffff");
            $("#F_CreateUser").get(0).disabled = false;
        }
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


$(function () {

    $("#FileUpload1").bind("change", function () {

        //开始提交
        //alert($("#form1"));
        $("#form1").ajaxSubmit({
            beforeSubmit: function (formData, jqForm, options) {
                //隐藏上传按钮
                $(".files #file1").hide();
                //显示LOADING图片
                $(".uploading # uploading1").show();
            },
            success: function (data, textStatus) {
                
                if (data.msg == 1) {
                    $("#F_FilePath").val(data.msbox);
                } else {
                    alert(data.msbox);
                }
                $(".files #file1").show();
                $(".uploading #uploading1").hide();
            },
            error: function (data, status, e) {
                alert("上传失败，错误信息：" + e);
                $(".files #file1").show();
                $(".uploading #uploading1").hide();
            },
            url: "/Tools/SingleUpload.ashx",
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    });


    $("#FileUpload2").bind("change", function () {
        //alert('');
        //开始提交
        //alert($("#form1"));
        $("#form2").ajaxSubmit({
            beforeSubmit: function (formData, jqForm, options) {
                //隐藏上传按钮
                $(".files #file2").hide();
                //显示LOADING图片
                $(".uploading #uploading2").show();
            },
            success: function (data, textStatus) {
                if (data.msg == 1) {
                    $("#EF_FilePath").val(data.msbox);
                } else {
                    alert(data.msbox);
                }
                $(".files #file2").show();
                $(".uploading #uploading2").hide();
            },
            error: function (data, status, e) {
                alert("上传失败，错误信息：" + e);
                $(".files #file2").show();
                $(".uploading #uploading2").hide();
            },
            url: "/Tools/SingleUpload.ashx",
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    });

    /*日期控件部分*/

    /*
    $('#F_CreateTime').DatePicker({
        format: 'Y-m-d',
        date: $('#s').val(),
        current: $('#s').val(),
        starts: 1,
        position: 'r',
        onBeforeShow: function () {
            $('#F_CreateTime').DatePickerSetDate($('#s').val(), true);
        },
        onChange: function (formated, dates) {
            $('#F_CreateTime').val(formated);
            $('#F_CreateTime').DatePickerHide();
        }
    });
    */

    /*日期控件部分*/
    /*
    $('#EF_CreateTime').DatePicker({
        format: 'Y-m-d',
        date: $('#s').val(),
        current: $('#s').val(),
        starts: 1,
        position: 'r',
        onBeforeShow: function () {
            $('#EF_CreateTime').DatePickerSetDate($('#s').val(), true);
        },
        onChange: function (formated, dates) {
            $('#EF_CreateTime').val(formated);
            $('#EF_CreateTime').DatePickerHide();
        }
    });
    */

});