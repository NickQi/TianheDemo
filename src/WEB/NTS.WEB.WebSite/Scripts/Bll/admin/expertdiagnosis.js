function expertdiagnosis() { }
expertdiagnosis.prototype = {
    showmaxlist: function (data) {
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: data, async: false, cache: false }).responseText;
        //alert(maxpage);
        maxpage = Math.ceil(maxpage / data.pagesize);
        $("#cp").val(maxpage);
        $('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: 1, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp").val()) { return; }
                new expertdiagnosis().showpaddinglist(data, pages);
            }
        });
        new expertdiagnosis().showpaddinglist(data, 1);

    },
    showtxlist: function (data) {
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetTXPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: data, async: false, cache: false }).responseText;
        // alert(maxpage);
        maxpage = Math.ceil(maxpage / data.pagesize);
        $("#cp1").val(maxpage);
        $('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: 1, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp1").val()) { return; }
                new expertdiagnosis().showtxpaddinglist(data, pages);
            }
        });
        new expertdiagnosis().showtxpaddinglist(data, 1);

    },
    showtxpaddinglist: function (data1, pages) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetTXPaddingList&dll=NTS_BECM.BLL&page=" + pages + "&times=" + new Date().getTime(),
            type: 'Post',
            data: data1,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#sysnlist").html('');
                // alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += "<li>";
                        html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_data\">";
                        html += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        html += "<td width=\"24%\"><span><span class=\"imgclick\"></span>" + data[i].F_Time + "</span></td>";
                        html += "<td width=\"24%\"><span>" + data[i].F_Content + "</span></td>";
                        html += "<td width=\"20%\"><span>" + GetDeviceName(data[i].F_DeviceID, data[i].AlarmType) + "</span></td>";
                        html += "<td width=\"16%\"><span></span></td>";
                        html += "<td width=\"16%\"><span></span></td>";
                        html += "</tr>";
                        html += "</table>";
                        html += "</li>";
                        $("#sysnlist").append(html);
                    }
                } else {
                    $("#sysnlist").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    },
    showarealist: function (buildid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(),
            type: 'Post',
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".lf_list_cn ul[name=a]").html('');
                $("#sareaid").val('');
                // 清除设备的选中
                $('.lf_list_cn2 ul').html('');
                if (data != ']') {
                    eval('data=' + data);
                    for (var i = 0; i < data.length; i++) {
                        $(".lf_list_cn ul[name=a]").append("<li config='" + data[i].BAreaID + "'> " + data[i].BAreaName + "</li>");
                    }
                    jQuery(".lf_list_cn ul[name=a] li").click(function () {
                        var index = jQuery(".lf_list_cn ulul[name=a] li").index(this);
                        jQuery(this).addClass("clicksub").siblings().removeClass("clicksub");
                        $("#sareaid").val($(this).attr('config'));

                        new expertdiagnosis().showdevicelist($(this).attr('config'));
                    });
                } else {
                    $(".lf_list_cn ul[name=a]").html('');
                }
            }
        });
    },
    showdevicelist: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&areaid=" + v + "&times=" + new Date().getTime(),
            type: 'Post',
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $(".lf_list_cn2 ul").html('');
                if (data != ']') {
                    eval('data=' + data);
                    for (var i = 0; i < data.length; i++) {
                        $(".lf_list_cn2 ul").append("<li style='cursor:pointer' config='" + data[i].F_MeterID + "'><span>" + data[i].F_MeterName + "</span><em style=\"display:none\"></em></li>");
                    }

                    jQuery(".lf_list_cn2 ul li").click(function () {

                        jQuery(this).addClass("clicksub999").siblings().removeClass("clicksub999");
                        $("#deviceid").val($(this).attr('config'));
                        /*
                        if ($(this).hasClass("clicksub999")) {
                        $(this).removeClass("clicksub999");

                        } else {
                        $(this).addClass("clicksub999");
                        }
                        */
                    });

                } else {
                    $(".lf_list_cn2 ul").html('');
                }
            }
        });
    },
    showpaddinglist: function (data1, pages) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.HisoryWaring.GetPaddingList&dll=NTS_BECM.BLL&page=" + pages + "&times=" + new Date().getTime(),
            type: 'Post',
            data: data1,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#sysnlist").html('');
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += "<li>";
                        html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_data\">";
                        html += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        html += "<td width=\"24%\"><span><span class=\"imgclick\"></span>" + data[i].WarnDate + "</span></td>";
                        html += "<td width=\"24%\"><span>" + data[i].Content + "</span></td>";
                        html += "<td width=\"20%\"><span>" + GetName(data[i].Build_Zone_Dev_ID, data[i].AlarmType) + "</span></td>";
                        html += "<td width=\"16%\"><span>" + data[i].F_AbnormalValue + "</span></td>";
                        html += "<td width=\"16%\"><span></span>" + data[i].F_NormalValue + "</td>";
                        html += "</tr>";
                        html += "</table>";
                        html += "</li>";
                        $("#sysnlist").append(html);
                    }
                } else {
                    $("#sysnlist").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    },
    showbuildcommdatalist: function (data) {
        $(".table_title").html(GetBuildName(data.buildid)); // 统计的标题
        $(".table_smalltt").html($(".datesort_show em").html());
        // 建筑附加json
        GetBuildFun(data.buildid);

        // 能耗总计
        showbuildenery(data);

        // 同比增幅
        showbuildcompare(data);

        // 相比标杆
        showbuildbg(data);

        // 超限总计
        showbuildoutlist(data);
    },
    showareacommdatalist: function (data) {
        $(".table_title").html(GetAreaName(data.areaid)); // 统计的标题
        $(".table_smalltt").html($("#defaultmonth").val() + "月");
        // 区域附加json
        GetAreaFun(data.areaid);

        // 能耗总计
        showareaenery(data);

        // 同比增幅
        showareacompare(data);

        // 相比标杆
        showareabg(data);

        // 超限总计
        showareaoutlist(data);
    },
    showdevicecommdatalist: function (data) {
        //alert(data.deviceid);
        $(".table_title").html(GetDeviceName(data.deviceid)); // 统计的标题
        $(".table_smalltt").html($("#defaultmonth").val() + "月");
        $(".messageshow ul").html('');
        // 能耗总计
        showdeviceenery(data);

        // 同比增幅
        showdevicecompare(data);

        // 相比标杆
        // showdevicebg(data);

        // 超限总计
        showdeviceoutlist(data);
    }
}


function showbuildenery(data) {
    var tt=$(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#timeslabel").html("日均能耗");
            break;
        default:
            $("#timeslabel").html("月均能耗");
            break;
    }

    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetbuildTotal&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert(data);
            $("#etotal").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].Ovalue + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].RealValue + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].OutValue + "</span></td>";
                    html += "<td width=\"52\"><span>" + builddata[i].OutPcent + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].EveryEnery + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].AreaEnery + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].MensEnery + "</span></td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#etotal").append(html);
                }
            }
        }
    }); 
}

/*建筑的查询*/
function ShowQueryBuild() {
    var basetime = $(".datesort_show em").html();
    //var bclass = $(".datesort_fast_list li.data_select").html();
    var bclass = $(".search_button span.select").html(); 
    //alert(basetime);
    //return false;
    var starttime = "";
    var endtime = "";
    var timetype = '';
    switch (bclass) {
        case "年":
            starttime = basetime.replace("年", "") + "-01-01";
            endtime = basetime.replace("年", "") + "-12-31";
            timetype = 1;
            break;
        case "季度":
            timetype = 3;
            if (basetime.indexOf("第1季度") > -1) {
                starttime = basetime.substring(0, 4) + "-01-01";
                endtime = basetime.substring(0, 4) + "-03-31";
            }
            else if (basetime.indexOf("第2季度") > -1) {
                starttime = basetime.substring(0, 4) + "-04-01";
                endtime = basetime.substring(0, 4) + "-06-30";
            }
            else if (basetime.indexOf("第3季度") > -1) {
                starttime = basetime.substring(0, 4) + "-07-01";
                endtime = basetime.substring(0, 4) + "-09-30";
            }
            else {
                starttime = basetime.substring(0, 4) + "-10-01";
                endtime = basetime.substring(0, 4) + "-12-31";
            }
            break;
        default:
            timetype = 2;
            starttime = basetime.replace("月", "") + "-01";
            endtime = GetMyNextMonth(basetime.replace("月", "") + "-01");
            break;
    }
    //alert(starttime);
   // alert(endtime);
   // return;
    var queryclass = 1;
    var buildidinto = $("#sbuild").val();
    if (buildidinto == "") {
        alert("对不起，请选择需要查询的建筑信息。");
        return;
    }
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        buildid: buildidinto,
        timetype:timetype,
        timetitle: $(".datesort_show em").html()
    }
    new expertdiagnosis().showbuildcommdatalist(data);
}


function ShowQueryArea() {
    var basetime = $(".datesort_show em").html();
   // var bclass = $(".datesort_fast_list li.data_select").html();
    var bclass = $(".search_button span.select").html(); 
    //alert(basetime);
    //return false;
    var starttime = "";
    var endtime = "";
	var timetype = '';
    switch (bclass) {
        case "年":
		timetype=1;
            starttime = basetime.replace("年", "") + "-01-01";
            endtime = basetime.replace("年", "") + "-12-31";
            break;
        case "季度":
		timetype=3;
            if (basetime.indexOf("第1季度") > -1) {
                starttime = basetime.substring(0, 4) + "-01-01";
                endtime = basetime.substring(0, 4) + "-03-31";
            }
            else if (basetime.indexOf("第2季度") > -1) {
                starttime = basetime.substring(0, 4) + "-04-01";
                endtime = basetime.substring(0, 4) + "-06-30";
            }
            else if (basetime.indexOf("第3季度") > -1) {
                starttime = basetime.substring(0, 4) + "-07-01";
                endtime = basetime.substring(0, 4) + "-09-30";
            }
            else {
                starttime = basetime.substring(0, 4) + "-10-01";
                endtime = basetime.substring(0, 4) + "-12-31";
            }
            break;
        default:
		timetype=2;
            starttime = basetime.replace("月", "") + "-01";
            endtime = GetMyNextMonth(basetime.replace("月", "") + "-01");
            break;
    }
    var queryclass = 2;
    var buildidinto = $("#sareaid").val();
    if (buildidinto == "") {
        alert("对不起，请选择需要查询的建筑区域信息。");
        return;
    }
    //alert(buildidinto);
    //return;
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        areaid: buildidinto,
		timetype:timetype,
        timetitle: $(".datesort_show em").html()
    }
    new expertdiagnosis().showareacommdatalist(data);
}

function ShowQueryDevice() {
    var basetime = $(".datesort_show em").html();
   // var bclass = $(".datesort_fast_list li.data_select").html();
    var bclass = $(".search_button span.select").html(); 
    //alert(basetime);
    //return false;
    var starttime = "";
    var endtime = "";
	var timetype="";
    switch (bclass) {
        case "年":
		timetype=1;
            starttime = basetime.replace("年", "") + "-01-01";
            endtime = basetime.replace("年", "") + "-12-31";
            break;
        case "季度":
		timetype=3;
            if (basetime.indexOf("第1季度") > -1) {
                starttime = basetime.substring(0, 4) + "-01-01";
                endtime = basetime.substring(0, 4) + "-03-31";
            }
            else if (basetime.indexOf("第2季度") > -1) {
                starttime = basetime.substring(0, 4) + "-04-01";
                endtime = basetime.substring(0, 4) + "-06-30";
            }
            else if (basetime.indexOf("第3季度") > -1) {
                starttime = basetime.substring(0, 4) + "-07-01";
                endtime = basetime.substring(0, 4) + "-09-30";
            }
            else {
                starttime = basetime.substring(0, 4) + "-10-01";
                endtime = basetime.substring(0, 4) + "-12-31";
            }
            break;
        default:
		timetype=2;
            starttime = basetime.replace("月", "") + "-01";
            endtime = GetMyNextMonth(basetime.replace("月", "") + "-01");
            break;
    }
    var queryclass = 3;
    var buildidinto = $("#deviceid").val();
    if (buildidinto == "") {
        alert("对不起，请选择需要查询的设备信息。");
        return;
    }
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        deviceid: buildidinto,
		timetype:timetype,
        timetitle: $(".datesort_show em").html()
    }
    new expertdiagnosis().showdevicecommdatalist(data);
}

function showbuildcompare(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    $("#last03").html(tt);
    switch (tt) {
        case "月":
            $("#timeslabel1").html("日均能耗增幅");
            break;
        default:
            $("#timeslabel1").html("月均能耗增幅");
            break;
    }
   // alert('xaxa');
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetbuildCompareLast&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        async: false,
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert(data);
            $("#ecompare").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComEveryEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComAreaEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComMensEnery + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";
                   
                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#ecompare").append(html);
                }
            }
        }
    }); 
}


function showbuildbg(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#bgtitle").html("日均能耗增幅");
            break;
        default:
            $("#bgtitle").html("月均能耗增幅");
            break;
    }
    
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetbuildCompareBG&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
           // alert(data);
            $("#ebiaogan").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"100\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].BgName + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComEveryEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComAreaEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComMensEnery + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#ebiaogan").append(html);
                }
            }
        }
    }); 
}


function showbuildoutlist(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#outlabel").html("阀值");
            break;
        default:
            $("#outlabel").html("阀值");
            break;
    }
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetbuildOutWarnning&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
           // alert(data);
            $("#oute").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li style=\"width:600px;overflow:hidden;\">";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].F_NormalValue + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].F_AbnormalValue + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].OutPcent + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#oute").append(html);
                }
            }
        }
    }); 
    
}
/*显示建筑的附加的基本信息*/
function GetBuildFun(buildid) {
    $(".messageshow ul").html('');
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildFun&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: { buildid: buildid },
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            if (data != '') {
                eval("builddata=" + data);
                $(".messageshow ul").append("<li>建筑功能：" + builddata.buildfun + "</li>");
                $(".messageshow ul").append("<li style=\"width:200px\">建筑面积（平方米）：" + builddata.area + "</li>");
                $(".messageshow ul").append("<li>人数：" + builddata.mens + " 人</li>");
                $(".messageshow ul").append("<li>设备数量：" + builddata.dnums + " 个</li>");
            }
        }
    }); 
}








/*建筑区域实现部分*/

function showareaenery(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#timeslabel").html("日均能耗");
            break;
        default:
            $("#timeslabel").html("月均能耗");
            break;
    }

    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetareaTotal&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert(data);
            $("#etotal").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].Ovalue + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].RealValue + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].OutValue + "</span></td>";
                    html += "<td width=\"52\"><span>" + builddata[i].OutPcent + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].EveryEnery + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].AreaEnery + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].MensEnery + "</span></td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#etotal").append(html);
                }
            }
        }
    });
}

function showareacompare(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    $("#last03").html(tt);
    switch (tt) {
        case "月":
            $("#timeslabel1").html("日均能耗增幅");
            break;
        default:
            $("#timeslabel1").html("月均能耗增幅");
            break;
    }
    // alert('xaxa');
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetareaCompareLast&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        async: false,
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert(data);
            $("#ecompare").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComEveryEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComAreaEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComMensEnery + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#ecompare").append(html);
                }
            }
        }
    });
}


function showareabg(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#bgtitle").html("日均能耗增幅");
            break;
        default:
            $("#bgtitle").html("月均能耗增幅");
            break;
    }

    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetareaCompareBG&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            // alert(data);
            $("#ebiaogan").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"100\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].BgName + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComEveryEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComAreaEnery + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].ComMensEnery + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#ebiaogan").append(html);
                }
            }
        }
    });
}


function showareaoutlist(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#outlabel").html("阀值");
            break;
        default:
            $("#outlabel").html("阀值");
            break;
    }
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetareaOutWarnning&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            // alert(data);
            $("#oute").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].F_NormalValue + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].F_AbnormalValue + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].OutPcent + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#oute").append(html);
                }
            }
        }
    });

}

function GetAreaFun(areaid) {
    $(".messageshow ul").html('');
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BArea.GetAreaFun&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: { areaid: areaid },
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            if (data != '') {
                eval("builddata=" + data);
               // $(".messageshow ul").append("<li></li>");
                $(".messageshow ul").append("<li style=\"width:200px\">区域面积(平方米)：" + builddata.area + " &nbsp;&nbsp;</li>");
                $(".messageshow ul").append("<li>人数：" + builddata.mens + " 人</li>");
                $(".messageshow ul").append("<li>设备数量：" + builddata.dnums + " 个</li>");
                $(".messageshow ul").append("<li></li>");
            }
        }
    }); 
}


/*设备实现部分*/

function showdeviceenery(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#timeslabel").html("日均能耗");
            break;
        default:
            $("#timeslabel").html("月均能耗");
            break;
    }

    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetDeviceTotal&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert(data);
            $("#edtotal").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].Ovalue + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].RealValue + "</span></td>";
                    html += "<td width=\"62\"><span>" + builddata[i].OutValue + "</span></td>";
                    html += "<td width=\"52\"><span>" + builddata[i].OutPcent + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].MaxEnery + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].MinsEnery + "</span></td>";
                    html += "<td width=\"72\"><span>" + builddata[i].EveryEnery + "</span></td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#edtotal").append(html);
                }
            }
        }
    });
}

function showdevicecompare(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    $("#last03").html(tt);
    switch (tt) {
        case "月":
            $("#timeslabel1").html("日均能耗增幅");
            break;
        default:
            $("#timeslabel1").html("月均能耗增幅");
            break;
    }
    // alert('xaxa');
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetdeviceCompareLast&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        async: false,
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert(data);
            $("#edcompare").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li>";
                    html += "<table width=\"458\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"91\"><span>" + builddata[i].ComMaxEnery + "</span></td>";
                    html += "<td width=\"91\"><span>" + builddata[i].ComMinEnery + "</span></td>";
                    html += "<td width=\"91\"><span>" + builddata[i].ComEveryEnery + "</span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#edcompare").append(html);
                }
            }
        }
    });
}


function showdeviceoutlist(data) {
    var tt = $(".datesort_fast_list ul li.data_select").html();
    switch (tt) {
        case "月":
            $("#outlabel").html("阀值");
            break;
        default:
            $("#outlabel").html("阀值");
            break;
    }
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.GetdeviceOutWarnning&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            // alert(data);
            $("#doute").html('');
            if (data != ']') {
                eval("builddata=" + data);
                for (var i = 0; i < builddata.length; i++) {
                    var html = '';
                    html += "<li style=\"width:600px;overflow:hidden;\">";
                    html += "<table width=\"618\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"tbl_diagnosis\">";
                    html += "<tr height=\"21\" valign=\"middle\" align=\"left\">";
                    html += "<td width=\"82\"><span style=\"padding-left:10px\">" + builddata[i].Otime + "</span></td>";
                    html += "<td width=\"82\"><span>" + builddata[i].Oclass + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].F_NormalValue + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].F_AbnormalValue + "</span></td>";
                    html += "<td width=\"120\"><span>" + builddata[i].OutPcent + "</span></td>";
                    html += "<td width=\"94\"><span></span></td>";

                    html += "</tr>";
                    html += "</table>";
                    html += "</li>";
                    $("#doute").append(html);
                }
            }
        }
    });

}

/*
function GetCuttMonth() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() + 1;
    if (month < 10) {
        month = "0" + month;
    }
    var firstDay = year + "-" + month + "-" + "01";
    return firstDay;
}
*/



/*
function GetNextMonth() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() + 2;
    if (month < 10) {
        month = "0" + month;
    }
    else if (month > 12) {
    month = "01";
        year = year + 1;
    }
    var firstDay = year + "-" + month + "-" + "01";
    return firstDay;
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

function GetToday() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() + 1;
    var date = myDate.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    return year + "-" + month + "-" + date;
}
*/

function showdefaultlist() {
    var starttime = $("#defaultmonth").val() + "-01";
    var endtime = GetMonthLast();
    var queryclass = 1;
    var buildidinto = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetFirstBuild&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
    eval("bdata=" + buildidinto);
    $("#sbuild").val(bdata.buildid);
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        buildid: bdata.buildid,
		timetype:1,
        timetitle: $(".datesort_show em").html()
    }
    new expertdiagnosis().showbuildcommdatalist(data);
}

$(function () {

    // 默认显示时间
    var defaultmonth = $("#defaultmonth").val() + "月";
    //alert(defaultmonth);
    jQuery(".datesort_show em").html(defaultmonth);
    // jQuery(".datesort_fast_list ul li.data_select").html("月");
    $(".search_button span").each(function () {
        $(this).removeClass("select");
    });
    $(".search_button span:last").addClass("select");

    // 显示默认的数据信息
    showdefaultlist();
    // new expertdiagnosis().showfzlist();
    $("#btnquery").click(function () {
        if ($("#sareaid").val() != '') {
            $("#tjclass").val('2');
        } else {
            $("#tjclass").val('1');
        }

        // alert($("#tjclass").val());
        //  return false;
        //根据建筑或区域选择，显示表格相应变化
        if ($("ul.nav_second li").hasClass("select_b")) {
            $("#buildarea").show();
            $("#build02").hide();
            $("#build04").hide();
            $("#build01").show();
            $("#build03").show();
        } else if ($("ul.nav_third li").hasClass("select_c")) {
            $("#buildarea").show();
            $("#build01").hide();
            $("#build03").hide();
            $("#build02").show();
            $("#build04").show();
        }
        //根据时间选择，显示表格相应变化
        if ($(".search_button span.select").html() == "年") {
            $("#last01").show();
            $("#last02").hide();
            $("#last03").hide();
            $("#last04").show();
            $("#last05").hide();
            $("#last06").hide();
        } else if ($(".search_button span.select").html() == "季度") {
            $("#last01").hide();
            $("#last02").show();
            $("#last03").hide();
            $("#last04").hide();
            $("#last05").show();
            $("#last06").hide();
        } else {
            $("#last01").hide();
            $("#last02").hide();
            $("#last03").show();
            $("#last04").hide();
            $("#last05").hide();
            $("#last06").show();
        }

        // 注册按钮的事件
        var tjclass = $("#tjclass").val();
        // alert(tjclass);
        switch (tjclass) {
            case "1":
                ShowQueryBuild();
                break;
            case "2":
                ShowQueryArea();
                break;
            default:
                ShowQueryDevice();
                break;
        }

    });

    $(".exportbtn").click(function () {
        var tjclass = $("#tjclass").val();
        //alert(tjclass)
        switch (tjclass) {
            case "1":
                ExcelQueryBuild();
                break;
            case "2":
                ExcelQueryArea();
                break;
            default:
                ExcelQueryDevice();
                break;
        }
    });
});

function ExcelQueryBuild() {
    var basetime = $(".datesort_show em").html();
   // var bclass = $(".datesort_fast_list li.data_select").html();
    var bclass = $(".search_button span.select").html(); 
    //alert(basetime);
    //return false;
    var starttime = "";
    var endtime = "";
	var timetype="";
    switch (bclass) {
        case "年":
		timetype=1;
            starttime = basetime.replace("年", "") + "-01-01";
            endtime = basetime.replace("年", "") + "-12-31";
            break;
        case "季度":
		timetype=3;
            if (basetime.indexOf("第1季度") > -1) {
                starttime = basetime.substring(0, 4) + "-01-01";
                endtime = basetime.substring(0, 4) + "-03-31";
            }
            else if (basetime.indexOf("第2季度") > -1) {
                starttime = basetime.substring(0, 4) + "-04-01";
                endtime = basetime.substring(0, 4) + "-06-30";
            }
            else if (basetime.indexOf("第3季度") > -1) {
                starttime = basetime.substring(0, 4) + "-07-01";
                endtime = basetime.substring(0, 4) + "-09-30";
            }
            else {
                starttime = basetime.substring(0, 4) + "-10-01";
                endtime = basetime.substring(0, 4) + "-12-31";
            }
            break;
        default:
		timetype=2;
            starttime = basetime.replace("月", "") + "-01";
            endtime = GetMyNextMonth(basetime.replace("月", "") + "-01");
            break;
    }
    var queryclass = 1;
    var buildidinto = $("#sbuild").val();
    if (buildidinto == "") {
        alert("对不起，请选择需要查询的建筑信息。");
        return;
    }
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        buildid: buildidinto,
		timetype:timetype,
        timetitle: $(".datesort_show em").html()
    }
   // alert(starttime);
   // alert(endtime);
   // return;
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.OutExcelBuildSheet&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        async: false,
        data: data,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            //alert("导出成功");
            window.location = data;
        }
    }); 
}


function ExcelQueryArea() {
    var basetime = $(".datesort_show em").html();
//    var bclass = $(".datesort_fast_list li.data_select").html();
    var bclass = $(".search_button span.select").html(); 
    //alert(basetime);
    //return false;
    var starttime = "";
    var endtime = "";
	var timetype="";
    switch (bclass) {
        case "年":
		timetype=1;
            starttime = basetime.replace("年", "") + "-01-01";
            endtime = basetime.replace("年", "") + "-12-31";
            break;
        case "季度":
		timetype=3;
            if (basetime.indexOf("第1季度") > -1) {
                starttime = basetime.substring(0, 4) + "-01-01";
                endtime = basetime.substring(0, 4) + "-03-31";
            }
            else if (basetime.indexOf("第2季度") > -1) {
                starttime = basetime.substring(0, 4) + "-04-01";
                endtime = basetime.substring(0, 4) + "-06-30";
            }
            else if (basetime.indexOf("第3季度") > -1) {
                starttime = basetime.substring(0, 4) + "-07-01";
                endtime = basetime.substring(0, 4) + "-09-30";
            }
            else {
                starttime = basetime.substring(0, 4) + "-10-01";
                endtime = basetime.substring(0, 4) + "-12-31";
            }
            break;
        default:
		timetype=2;
            starttime = basetime.replace("月", "") + "-01";
            endtime = GetMyNextMonth(basetime.replace("月", "") + "-01");
            break;
    }
    var queryclass = 2;
    var buildidinto = $("#sareaid").val();
    if (buildidinto == "") {
        alert("对不起，请选择需要查询的建筑区域信息。");
        return;
    }
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        areaid: buildidinto,
		timetype:timetype,
        timetitle: $(".datesort_show em").html()
    }

    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.OutExcelAreaSheet&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            window.location = data;
        }
    }); 
}


function ExcelQueryDevice() {
    var basetime = $(".datesort_show em").html();
   // var bclass = $(".datesort_fast_list li.data_select").html();
    var bclass = $(".search_button span.select").html(); 
    //alert(basetime);
    //return false;
    var starttime = "";
    var endtime = "";
	var timetype="";
    switch (bclass) {
        case "年":
		timetype=1;
            starttime = basetime.replace("年", "") + "-01-01";
            endtime = basetime.replace("年", "") + "-12-31";
            break;
        case "季度":
		timetype=3;
            if (basetime.indexOf("第1季度") > -1) {
                starttime = basetime.substring(0, 4) + "-01-01";
                endtime = basetime.substring(0, 4) + "-03-31";
            }
            else if (basetime.indexOf("第2季度") > -1) {
                starttime = basetime.substring(0, 4) + "-04-01";
                endtime = basetime.substring(0, 4) + "-06-30";
            }
            else if (basetime.indexOf("第3季度") > -1) {
                starttime = basetime.substring(0, 4) + "-07-01";
                endtime = basetime.substring(0, 4) + "-09-30";
            }
            else {
                starttime = basetime.substring(0, 4) + "-10-01";
                endtime = basetime.substring(0, 4) + "-12-31";
            }
            break;
        default:
		timetype=2;
            starttime = basetime.replace("月", "") + "-01";
            endtime = GetMyNextMonth(basetime.replace("月", "") + "-01");
            break;
    }
    var queryclass = 3;
    var buildidinto = $("#deviceid").val();
    if (buildidinto == "") {
        alert("对不起，请选择需要查询的设备信息。");
        return;
    }
    var data = {
        starttime: starttime,
        endtime: endtime,
        queryclass: queryclass,
        deviceid: buildidinto,
		timetype:timetype,
        timetitle: $(".datesort_show em").html()
    }

    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.BllChart.mainquerymap.OutExcelDeviceSheet&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: data,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            window.location = data;
        }
    }); 
}


function GetAreaName(areaid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.BArea.GetAreaName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + areaid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetDeviceName(deviceid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + deviceid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetBuildName(buildid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}