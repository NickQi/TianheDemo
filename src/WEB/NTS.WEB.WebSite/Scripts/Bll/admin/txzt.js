$(function () {

    $("#querybtn").click(function () {

        new txzt().showdatalist('all');
    });

    $("#sbtn").click(function () {
        $("#sbuild").val('');
        $("#sareaid").val('');
        var devicename = $("#devicename").val() == "请输入搜索设备" ? "" : $("#devicename").val();
        new txzt().showkeylist(devicename);
        new txzt().showdatalist('');

    });
    
    new txzt().showdatalist('');
});

function txzt() { }
txzt.prototype = {
    name: '通讯状态',
    showkeylist: function (devicename) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.showkeylist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { devicename: devicename },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    $("#slist").html('');
                    eval("data=" + data);
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        htmls += "<li style='color:#666'>";
                        htmls += GetDeviceName(data[i].DeviceID, data[i].DeviceClass);
                        htmls += "</li>";
                    }
                    $("#slist").append(htmls);
                    jQuery(".data_list ul li").click(function () {
                        var ba = jQuery(this).html();
                        jQuery(".table_bigtt").html(ba);
                        //  alert(ba);
                        new txzt().showdatalist(ba);
                    });
                } else {
                    $("#slist").html("<li><font color=red>对不起，暂无设备状态的信息。</font></li>");

                }
            }
        });
    },
    showdatalist: function (keys) {
        var firstp = 1;
        var buildid = $("#sbuild").val();
        var areaid = $("#sareaid").val();
        var wherestr = '';
        if (keys != "all") {
            if (keys == '') {
                wherestr = $("#devicename").val() == "请输入搜索设备" ? "" : $("#devicename").val();
            } else {
                wherestr = keys;
            } 
        }
        //alert(wherestr);
        /*图表初始化结束*/
        var data = {
            buildid: buildid,
            areaid: areaid,
            keys: wherestr
        }
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: data, async: false, cache: false }).responseText;
        maxpage = Math.ceil(maxpage / 19);
        $("#cp").val(maxpage);
        $('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: firstp, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp").val()) { return; }
                new txzt().showpaddinglist(pages, data);
            }
        });
        new txzt().showpaddinglist(1, data); // 第一次加载时
    },
    showpaddinglist: function (pages, data1) {
        $("#padding-main").html('');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.GetPaddingList&dll=NTS_BECM.BLL&pagesize=19&pages=" + pages + "&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data1,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        //alert(data[i].DeviceStatus);
                        htmls += "<li style=\"width:100%\">";
                        htmls += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" class=\"tbl_data\" align='center' cellspacing=\"0\">";

                        htmls += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        htmls += "<td width=\"30%\" align='center'><span style=\"padding-left:10px\">" + $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.getbuildname&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { deviceid: data[i].DeviceID, DeviceClass: data[i].DeviceClass }, async: false, cache: false }).responseText + "</span></td>";
                        htmls += "<td width=\"27%\" align='center'><span>&nbsp;" + $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.getareaname&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { deviceid: data[i].DeviceID, DeviceClass: data[i].DeviceClass }, async: false, cache: false }).responseText + "</span></td>";
                        htmls += "<td width=\"30%\" align='center'><span>" + GetDeviceName(data[i].DeviceID, data[i].DeviceClass) + "</span></td>";
                        htmls += "<td width=\"13%\" align='center'><span>" + (data[i].DeviceStatus == "0" ? "<font color=red>中断</font>" : "正常") + "</span></td>";
                        htmls += " </tr>";
                        htmls += " </table>";
                        htmls += " </li>";
                    }
                    // alert(htmls);
                    $("#padding-main").html(htmls);
                } else {
                    $("#padding-main").html("<br/><font color=red>对不起，暂无设备状态的信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    },
    showbuildinglist: function (buildgroupid) {
        var htmls = "";
        $(".lf_list_cn[name=b] ul").html('');
        $(".lf_list_cn[name=a] ul").html('');
        $("#sbuild").val('');
        $("#sareaid").val('');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.GetBuildings&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            async: false,
            type: 'Post',
            dataType: 'text',
            data: { buildgroupid: buildgroupid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                if (data != "]") {
                    var myobj = eval(data);
                    for (var i = 0; i < myobj.length; i++) {
                        htmls += "<li name='builds' config=" + myobj[i].F_BuildID + ">" + myobj[i].F_BuildName + "</li>";
                    }
                    $("#jianzhu").html(htmls);
                    // alert($("#jianzhu").html());
                    jQuery(".lf_list_cn[name=b] ul li").click(function () {

                        var index = jQuery(".lf_list_cn[name=b] ul li").index(this);
                        jQuery(this).addClass("clicksub").siblings().removeClass("clicksub");
                        $("#sbuild").val($(this).attr('config'));
                        $("#sareaid").val('');
                        // var title = $("#t1").html() + " >" + $(this).html();
                        $("#t2").html($(this).html());
                        // 显示区域
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetBAreaList&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + $(this).attr('config') + "&times=" + new Date().getTime(),
                            type: 'Post',
                            timeout: 1000,
                            success: function (data) {
                                $(".lf_list_cn[name=a] ul").html('');
                                if (data != ']') {
                                    eval('data=' + data);
                                    for (var i = 0; i < data.length; i++) {
                                        $(".lf_list_cn[name=a] ul").append("<li config='" + data[i].BAreaID + "'> " + data[i].BAreaName + "</li>");
                                    }
                                    jQuery(".lf_list_cn[name=a] ul li").click(function () {
                                        var index = jQuery(".lf_list_cn[name=b] ul li").index(this);
                                        jQuery(this).addClass("clicksub").siblings().removeClass("clicksub");
                                        $("#sareaid").val($(this).attr('config'));
                                        // $("#t3").html($("#t2").html() + " >" + $(this).html());
                                        $("#t3").html($(this).html());
                                    });
                                } else {
                                    $(".lf_list_cn[name=a] ul").html('');
                                }
                            }
                        });

                    });

                    /*
                    jQuery(".navcn_list ul li[name=builds]").click(function () {
                    var index = jQuery(".navcn_list ul li[name=builds]").index(this);
                    jQuery(this).addClass("selectlist").siblings().removeClass("selectlist");
                    var v = $(this).attr("bconfig");
                    $("#CurrentBuild").val(v);
                    $("#CurrentBAreaID").val('');
                    new Sys_IndexPage().ShowBuildInfo(v);
                    });
                    */
                }
                else {
                    $("#jianzhu").html("");
                }
            }
        });
    }
}

/*

function GetDeviceName(deviceid,deviceclass) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.DeviceRunStatus.getdevicename&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', data: { deviceid: deviceid, DeviceClass: deviceclass }, async: false, cache: false }).responseText;
}
*/