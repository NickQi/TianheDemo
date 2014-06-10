function collectoruse() { }
collectoruse.prototype = {
    name: '设备js类',
    showsearcharealist: function (buildid) {
        // searcharea
        //alert(buildid);
        $("#sareaid").val('');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&[__DOTNET__]System.String=" + buildid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            // data: { buildid: buildid },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $("#searcharea").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#searcharea").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $("#searcharea").append(" <em config='" + data[i].BAreaID + "'>" + data[i].BAreaName + "</em>");
                    }

                    jQuery("#searcharea em").click(function () {
                        //alert('');
                        var ba = jQuery(this).html();
                        jQuery("#dome_area").html(ba);
                        jQuery("#c_path2").html(ba);
                        var index = jQuery("#dome_1_3 em").index(this);
                        jQuery(this).css({ 'background-color': '#4b4b4b' }).siblings().css({ 'background-color': '' });
                        $("#sareaid").val($(this).attr('config'));
                        // 显示仪表数据信息
                        //alert('');
                        var firstp = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.BackfirstPage&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
                        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetPageCount&dll=NTS_BECM.BLL&[__DOTNET__]System.Int32=17&[__DOTNET__]System.String=" + $("#sbuild").val() + "&times=" + new Date().getTime(), type: 'GET', data: { areaid: $("#sareaid").val() }, async: false, cache: false }).responseText;
                        
                        $("#cp").val(maxpage);
                        // alert(maxpage);
                      //  $('.pagination#all').css("display", "none");
                        $('.pagination#classid').html('');
                        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
                        $('.pagination#classid').jqPagination({
                            link_string: '/?page={page_number}',
                            current_page: firstp, //设置当前页 默认为1
                            max_page: maxpage, //设置最大页 默认为1
                            page_string: '当前第{current_page}页,共{max_page}页',
                            paged: function (pages) {
                                if (pages > $("#cp").val()) { return; }
                                new collectoruse().showpaddingcollectoruselist(pages);
                            }
                        });
                        new collectoruse().showpaddingcollectoruselist(1); // 第一次加载时 

                    });
                } else {
                    $("#searcharea").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });
    },
    addcollectoruse: function () {

        var F_MeterID = $("#F_MeterID").val();
        var F_BuildID = $("#F_BuildID").val();
        var F_AreaID = $("#F_AreaID").val();
        var F_MeterName = $("#F_MeterName").val();
        var F_DeviceModelType = $("#F_DeviceModelType").val();
       // var F_DeviceModelType = $("input[name=F_DeviceModelType]:checked").val();
        //alert(F_DeviceModelType);
        // return false;
        var F_CollectionID = $("#F_CollectionID").val();
        var F_Rate = '';
        if ($("#F_Rate").attr("checked")) {
            F_Rate = "1";
        } else {
            F_Rate = "0";
        }
        if (F_MeterID.length != 14) {
            alert("设备仪器的代码长度为14位数字与字母的组合字符。");
            return false;
        }
        if (F_MeterName.length == 0) {
            alert("设备仪器的名称不能为空。");
            return false;
        }
        if (F_MeterName.length > 48) {
            alert("设备仪器的名称长度不能大于48个字符。");
            return false;
        }
        // alert(F_AreaID);
        if (F_AreaID == '') {
            alert("请选择建筑区域");
            return false;
        }
        var data = {
            F_MeterID: F_MeterID,
            F_BuildID: F_BuildID,
            F_AreaID: F_AreaID,
            F_MeterName: F_MeterName,
            F_DeviceModelType: F_DeviceModelType,
            F_CollectionID: F_CollectionID,
            F_Rate: F_Rate
        }
        // 验证输入信息
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.addcollectoruse&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('仪表设备添加成功。');
                    DivCloseUp6();
                    window.location = 'collector-use.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showcollectoruseinfo: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.showcollectoruseinfo&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { cuid: v },
            timeout: 1000,
            success: function (data) {
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#EF_MeterID").val(data[0].F_MeterID);
                    $("#EF_BuildID").val(data[0].F_BuildID);
                    $("#EF_AreaID").val(data[0].F_AreaID);
                    $("#EF_MeterName").val(data[0].F_MeterName);
                    // $("#EF_DeviceModelType").val(data[0].F_DeviceModelType);
                    /*
                    $("input[name=EF_DeviceModelType]").each(function () {
                    //  alert($(this).val());
                    if ($(this).val() == data[0].F_DeviceModelType) {
                    $(this).attr("checked", true);
                    }
                    });
                    */
                    $("ul.sub_mid_equiptype#edit li").each(function () {
                        if ($(this).attr('config') == data[0].F_DeviceModelType) {
                            $("input.dd_equiptype#edit").val($(this).html());
                            $("#EF_DeviceModelType").val($(this).attr('config'));
                        }

                    });
                    //  $("input.dd_equiptype#edit").val($("ul.sub_mid_equiptype#edit li:first").html());
                    //  $("#EF_DeviceModelType").val($("ul.sub_mid_equiptype#add li:first").attr('config'));

                    $("#EF_CollectionID").val(data[0].F_CollectionID);
                    if (data[0].F_Rate == "1") {
                        $("#EF_Rate").attr("checked", true);
                    } else {
                        $("#EF_Rate").attr("checked", false);
                    }
                    //$("#EF_Rate").val();

                    // 设置初始化的下拉框信息

                    $("#UpdateCollectionUl li").each(function () {
                        if ($(this).attr('config') == data[0].F_CollectionID) {
                            $("#EF_CollectionIDText").val($(this).html());
                            $("#EF_CollectionID").val($(this).attr('config'));
                        }
                    });

                    $(".sub_mid_jianzhu#edit li").each(function () {
                        //alert(data[0].F_BuildID);

                        if ($(this).attr('config') == data[0].F_BuildID) {
                            $(".dd_jianzhu#edit").val($(this).html());
                            $("#EF_BuildID").val($(this).attr('config'));
                            //显示区域的列表
                            new collectoruse().showeditarealist($(this).attr('config'));
                        }
                    });

                    $(".sub_mid_jianzhuarea#edit li").each(function () {
                        //alert($(this).attr('config'));
                        // alert(data[0].F_AreaID);
                        if ($(this).attr('config') == data[0].F_AreaID) {
                            $(".dd_jianzhuarea#edit").val($(this).html());
                            $("#EF_AreaID").val($(this).attr('config'));
                            //显示区域的列表
                            // new collectoruse().showeditarealist($(this).attr('config'));
                        }
                    });


                }

            }
        });
    },
    updatecollectoruse: function () {
        var F_MeterID = $("#EF_MeterID").val();
        var F_BuildID = $("#EF_BuildID").val();
        var F_AreaID = $("#EF_AreaID").val();
        var F_MeterName = $("#EF_MeterName").val();
        var F_DeviceModelType = $("#EF_DeviceModelType").val();
        //  var F_DeviceModelType = $("input[name=EF_DeviceModelType]:checked").val();
        // var F_DeviceModelType = $("#EF_DeviceModelType").val();
        var F_CollectionID = $("#EF_CollectionID").val();
        var F_Rate = '';
        if ($("#EF_Rate").attr("checked")) {
            F_Rate = "1";
        } else {
            F_Rate = "0";
        }
        if (F_MeterID.length != 14) {
            alert("设备仪器的代码长度为14位数字与字母的组合字符。");
            return false;
        }
        if (F_MeterName.length == 0) {
            alert("设备仪器的名称不能为空。");
            return false;
        }
        if (F_MeterName.length > 48) {
            alert("设备仪器的名称长度不能大于48个字符。");
            return false;
        }
        var data = {
            F_MeterID: F_MeterID,
            F_BuildID: F_BuildID,
            F_AreaID: F_AreaID,
            F_MeterName: F_MeterName,
            F_DeviceModelType: F_DeviceModelType,
            F_CollectionID: F_CollectionID,
            F_Rate: F_Rate
        }
        // alert(F_CollectionID);
        // return;
        // 验证输入信息
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.updatecollectoruse&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('仪表设备修改成功。');
                    window.location = 'collector-use.aspx?d=' + new Date().getTime();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    deletecollectoruse: function () {
        var cuid = $("#dcuid").val();
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.deletecollectoruse&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { cuid: cuid },
            timeout: 1000,
            success: function (data) {
                data = eval("data=" + data);
                if (data.success) {
                    window.location = 'collector-use.aspx?d=' + new Date().getDate();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showarealist: function (buildid) {
        //alert(buildid);
        //清空下拉框
        $(".dd_jianzhuarea#add").val('');
        $("#F_AreaID").val('');

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&[__DOTNET__]System.String=" + buildid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".sub_mid_jianzhuarea#add").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $(".sub_mid_jianzhuarea#add").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $(".sub_mid_jianzhuarea#add").append(" <li config='" + data[i].BAreaID + "'>" + data[i].BAreaName + "</li>");
                    }

                    jQuery(".sub_mid_jianzhuarea#add li").click(function () {
                        var wt = $(this).html();
                        $(".dd_jianzhuarea#add").val(wt);
                        $("#F_AreaID").val($(this).attr('config'));
                    });
                } else {
                    $(".sub_mid_jianzhuarea#add").html('');
                }

            }
        });
    },
    showeditarealist: function (buildid) {
        $(".dd_jianzhuarea#edit").val('');
        $("#EF_AreaID").val('');

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&[__DOTNET__]System.String=" + buildid + "&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            async: false,
            success: function (data) {
                //alert(data);
                $(".sub_mid_jianzhuarea#edit").html('');
                if (data != ']') {
                    data = eval("data=" + data);
                    $(".sub_mid_jianzhuarea#edit").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        $(".sub_mid_jianzhuarea#edit").append(" <li config='" + data[i].BAreaID + "'>" + data[i].BAreaName + "</li>");
                    }

                    jQuery(".sub_mid_jianzhuarea#edit li").click(function () {
                        var wt = $(this).html();
                        $(".dd_jianzhuarea#edit").val(wt);
                        $("#EF_AreaID").val($(this).attr('config'));
                    });
                } else {
                    $(".sub_mid_jianzhuarea#add").html('');
                }

            }
        });
    },
    showpaddingcollectoruselist: function (current_page) {
        var buildid = $("#sbuild").val();
        var areaid = $("#sareaid").val();
       // alert(buildid);
       // alert(areaid);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.showcollectoruselist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { buildid: buildid, current_page: current_page, pagesize: 17, areaid: areaid },
            timeout: 1000,
            success: function (data) {
                 // alert(data);
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#collectoruselist").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        htmls += "<li>";
                        htmls += "<table width=\"776\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"menu_tbl\">";
                        htmls += "<tr height=\"30\" valign=\"middle\">";
                        
                        htmls += "<td width=\"90\"><span style=\"float:left\">" +GetBuildName(data[i].F_BuildID) + "</span></td>";
                        htmls += "<td width=\"90\"><span style=\"float:left\">" + GetAreaName(data[i].F_AreaID) + "</span></td>";
                        htmls += "<td width=\"111\" style=\"text-align:left\"><span><span class=\"imgclick\"></span> " +GetCollectName(data[i].F_CollectionID) + "</span></td>";
                        htmls += "<td width=\"90\"><span style=\"float:left\">" + data[i].F_MeterID + "</span></td>";
                        htmls += "<td width=\"90\" align=\"left\"><span style=\"float:left\">" + data[i].F_MeterName + "</span></td>";
                        htmls += "<td width=\"90\"><span style=\"float:left\">" + GetObjectName(data[i].F_DeviceModelType) + "</span></td>";
                        htmls += "<td width=\"67\"><span style=\"float:left\">" + (data[i].F_Rate == '1' ? '是' : '否') + "</span></td>";
                        htmls += "<td width=\"148\" valign=\"middle\"><span class=\"btnbg2\"><input type=\"button\" class=\"button04\" onclick=\"DivRevise4('" + data[i].F_MeterID + "')\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"Del3('" + data[i].F_MeterID + "')\" value=\"删除\"></span></td>";
                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";

                    }
                    $("#collectoruselist").append(htmls);
                } else {
                    $("#collectoruselist").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });
    }
}


function GetObjectName(v) {
    if (v == "01000") {
        return "电";
    }
    else if (v == "02000") {
        return "水";
    }
    else if (v == "03000") {
        return "气";
    }
    else if (v == "04000") {
        return "供热";
    }
    else if (v == "05000") {
        return "供冷";
    }
    else{
        return "其他";
    }
}
$(function () {
    /*下拉菜单14*/
    $(".dd14#add").mouseover(function () { $(".dd_jianzhu_sub#add").show(); });
    $(".dd14#add").mouseout(function () { $(".dd_jianzhu_sub#add").hide(); });
    $(".dd14#edit").mouseover(function () { $(".dd_jianzhu_sub#edit").show(); });
    $(".dd14#edit").mouseout(function () { $(".dd_jianzhu_sub#edit").hide(); });
    $("ul.sub_mid_jianzhu#edit li").click(function () {
        var wt = $(this).html();
        $(".dd_jianzhu#edit").val(wt);
        $("#EF_BuildID").val($(this).attr('config'));
        new collectoruse().showeditarealist($(this).attr('config'));
    });
    $("ul.sub_mid_jianzhu#add li").click(function () {
        var wt = $(this).html();
        $("#F_BuildIDText").val(wt);
        $("#F_BuildID").val($(this).attr('config'));
        new collectoruse().showarealist($(this).attr('config'));
    });

    /*下拉菜单15*/
    $(".dd15#add").mouseover(function () { $(".dd_jianzhuarea_sub#add").show(); });
    $(".dd15#add").mouseout(function () { $(".dd_jianzhuarea_sub#add").hide(); });
    $(".dd15#edit").mouseover(function () { $(".dd_jianzhuarea_sub#edit").show(); });
    $(".dd15#edit").mouseout(function () { $(".dd_jianzhuarea_sub#edit").hide(); });
    $("ul.sub_mid_jianzhuarea#edit li").click(function () {
        var wt = $(this).html();
        $(".dd_jianzhuarea#edit").val(wt);
    });
    $("ul.sub_mid_jianzhuarea#add li").click(function () {
        var wt = $(this).html();
        $(".dd_jianzhuarea#add").val(wt);
        $("#F_AreaID").val($(this).attr('config'));
    });

    // 添加时第一个采集器默认值
    $("#F_CollectionIDText").val($("#CollectionListUl li:first").html());
    $("#F_CollectionID").val($("#CollectionListUl li:first").attr('config'));




});

 




