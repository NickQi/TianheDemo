

/*获取选择对象的列表*/

function getolistarray() {
    var cnum = jQuery(".nav_third li.select_c").length
    if ($("#sdeviceid").val() == '' || cnum>0) {
        
        if (cnum == 0) {
            alert('请选择要对比的设备');
            return;
        }
        else if (cnum > 10) {
            alert('要对比的设备最多10个。');
            return;
        }
        else {
            $(".equip_list").html('');
            var i = 1;
            jQuery("span.checkbox").each(function () {
                var obj = $(this).parent().parent();

                if (obj.hasClass("select_c")) {
                    // alert(obj)
                    var html = '';
                    html += "<span config=" + $(this).attr('config') + ">";
                    html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
                    html += "<tr valign=\"middle\" align=\"left\">";
                    html += "<td width=\"27\" valign=\"top\" align=\"left\"><img src=\"images/energy-equ/equ_icon0" + i + ".jpg\" width=\"21\" height=\"21\"/></td>";
                    html += "<td width=\"84\" align=\"left\" valign=\"middle\"><div class=\"txtbig\">" + $(this).attr('nconfig').replace('、', '') + "</div></td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</span>";
                    // alert(html);
                    $(".equip_list").append(html);
                    i++;
                }
            });

            var devicelist = '';
            $(".equip_list span").each(function () {
                devicelist += "," + $(this).attr('config');
            });
            devicelist = devicelist.length > 0 ? devicelist.substring(1) : devicelist;
            $("#sdeviceid").val(devicelist);

            //  alert($(".equip_list").html());

        } 
    }
}


function cmaindevice() { }

cmaindevice.prototype = {
    name: '设备对比js处理',
    showdevicechart: function (v) {
        if (v > 1 && v != 9) {
            getolistarray();
        }
        var data = {
            deviceid: $("#sdeviceid").val(),
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            ismax: $("#ismax").val(),
            imagetype: $("#imagetype").val(),
            itemcode: $("span.val").attr('config'),
            mycdate: $("#mycdate").val(),
            issametime: $("#issametime").val(),
            sametype: $("#sametype").val()
        };
        switch (v) {
            case "1": // 初始状态
                data.issametime = 'no';
                var myDate = new Date();
                data.deviceid = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetTopDevice&dll=NTS_BECM.BLL&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
                data.starttime = GetCuttMonth();
                data.endtime = GetMonthLast();
                $("#sdeviceid").val(data.deviceid);
                $("#starttime").val(data.starttime);
                $("#endtime").val(data.endtime);

                $(".equip_list").html('');

                var strs = new Array(); //定义一数组
                strs = data.deviceid.split(","); //字符分割
                //alert(data.deviceid);
                var i = 1;
                $.each(strs, function (index, v) {
                    // alert(v);
                    var html = '';
                    html += "<span config=" + v + ">";
                    html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
                    html += "<tr valign=\"middle\" align=\"left\">";
                    html += "<td width=\"27\" valign=\"top\" align=\"left\"><img src=\"images/energy-equ/equ_icon0" + i + ".jpg\" width=\"21\" height=\"21\"/></td>";
                    html += "<td width=\"84\" align=\"left\" valign=\"middle\"><div class=\"txtbig\">" + GetDeviceName(v) + "</div></td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</span>";
                    // alert(html);
                    $(".equip_list").append(html);
                    i++;
                });
                break;
            case "2": // 单击查询按钮
                data.issametime = 'no';

                /*
                var devicelist = '';
                $(".equip_list span").each(function () {
                    devicelist += "," + $(this).attr('config');
                });
                devicelist = devicelist.length > 0 ? devicelist.substring(1) : devicelist;
                $("#sdeviceid").val(devicelist);
                */


                //alert(devicelist);
                //return;
                if ($("#hour01").val() != '') {
                    data.starttime = $("#date01").val() + " " + $("#hour01").val() + ":00:00";
                }
                if ($("#hour02").val() != '') {
                    data.endtime = $("#date02").val() + " " + $("#hour02").val() + ":00:00";
                }
                $("#starttime").val(data.starttime);
                $("#endtime").val(data.endtime);
                break;

            case "3": // 月份
                data.issametime = 'no';
                data.starttime = GetCuttMonth();
                data.endtime = GetMonthLast();
                $("#starttime").val(GetCuttMonth());
                $("#endtime").val(GetToday());
                break;
            case "4":
                data.issametime = 'no';
                var time = new Date();
                time.setDate(time.getDate() - time.getDay() + 1);
                data.starttime = GetWeekFirstDay();
                data.endtime = GetWeekLastDay();
                $("#starttime").val(data.starttime);
                $("#endtime").val(data.endtime);
                break; // 周
            case "5":
                data.issametime = 'no';
                var myDate = new Date();
                data.starttime = GetToday();
                data.endtime = GetNow();
                $("#starttime").val(data.starttime);
                $("#endtime").val(data.endtime);
                //alert(data.starttime);
                break; // 日
            case "6":
                data.issametime = 'no';
                //alert('xsaxsa');
                $(".equip_list").html('');

                var strs = new Array(); //定义一数组
                strs = $("#sdeviceid").val().split(","); //字符分割
                //alert(data.deviceid);
                var i = 1;
                $.each(strs, function (index, v) {
                    // alert(v);
                    var html = '';
                    html += "<span config=" + v + ">";
                    html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
                    html += "<tr valign=\"middle\" align=\"left\">";
                    html += "<td width=\"27\" valign=\"top\" align=\"left\"><img src=\"images/energy-equ/equ_icon0" + i + ".jpg\" width=\"21\" height=\"21\"/></td>";
                    html += "<td width=\"84\" align=\"left\" valign=\"middle\"><div class=\"txtbig\">" + (v.indexOf('BG_') > -1 ? "<b><font color=red>" + GetDeviceName(v.replace('BG_', '')) + "(标杆)</font></b>" : GetDeviceName(v)) + "</div></td>";
                    html += "</tr>";
                    html += "</table>";
                    html += "</span>";
                    // alert(html);
                    $(".equip_list").append(html);
                    i++;
                });

                if ($("#hour01").val() != '') {
                    data.starttime = $("#date01").val() + " " + $("#hour01").val() + ":00:00";
                }
                if ($("#hour02").val() != '') {
                    data.endtime = $("#date02").val() + " " + $("#hour02").val() + ":00:00";
                }
                $("#starttime").val(data.starttime);
                $("#endtime").val(data.endtime);
                break;
            default:  // 直接刷新查询

                break;
        }
        $("#showbox_title").html("<font color='green'>" + jQuery("span.val").html() + "</font>");
        
        //$("#querytime").html(data.starttime + " - " + data.endtime);

        if (jQuery(".search_button span.select").html() == "本周") {
            $("#querytime").html(data.starttime.split(' ')[0] + " - " + data.endtime);
        } else {
            $("#querytime").html(data.starttime + " - " + data.endtime);
        }

        var arr = new Array();
        arr = data.deviceid.split(",");
        if (arr.length > 10) {
            alert('对不起,设备对比最多选择10个设备。');
            return;
        }
        new cmaindevice().showbasechart(data);
        // new cmaindevice().showdatalist(data);
    },
    showbasechart: function (data) {
        // alert("/enerycharts/DeviceManyQueryChart.aspx?ismax=" + data.ismax + "&itemcode=" + data.itemcode + "&deviceid=" + data.deviceid + "&starttime=" + data.starttime + "&endtime=" + data.endtime);
       //  $(".equ_img").load("/enerycharts/CDeviceManyQueryChart.aspx?ismax=" + data.ismax + "&itemcode=" + data.itemcode + "&deviceid=" + data.deviceid + "&starttime=" + data.starttime + "&endtime=" + data.endtime + "&d=" + new Date().getTime(), data);
        // alert(data.issametime);
        if (data.issametime != '') {
            $(".equ_img").load("/enerycharts/DeviceManyQueryChart.aspx?d=" + new Date().getTime(), data);
        } else {
            //alert('xsaxsaca');
            $(".equ_img").load("/enerycharts/CDeviceManyQueryChart.aspx?d=" + new Date().getTime(), data);
        }
    },
    showdiylist: function () {
        $.ajax({
            //diy类库的方法
            url: "action.ashx?method=NTS_BECM.BLL.DiyDeviceSearch.showdiylist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            //data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                $("#diy").html('');
                if (data != ']') {
                    eval('data=' + data);
                    for (var i = 0; i < data.length; i++) {
                        $("#diy").append("<span config=" + data[i].id + ">" + data[i].diyname + "</span>");
                    }
                } else { $("#diy").html(''); }

                jQuery(".datesort_custom span").click(function () {
                    jQuery("#custombox").hide();
                    // 显示图表
                    $("#diyid").val($(this).attr('config'));
                    new cmaindevice().showdevicechart('6');
                });
            }
        });
    },
    savediyconditon: function (data) {
        var bb11 = jQuery("#date01").val();
        var bb12 = jQuery("#hour01").val();
        var bb21 = jQuery("#date02").val();
        var bb22 = jQuery("#hour02").val();
        var data = {
            starttime: bb11 + ' ' + bb12 + ':00:00',
            endtime: bb21 + ' ' + bb22 + ':00:00',
            itemcode: jQuery("span.val").attr('config'),
            deviceid: jQuery("#devicelist ul li.clicksub999").attr('config'),
            diyname: $('#equip_sort_add').val()
        }
        if ($('#equip_sort_add').val() == '' || $('#equip_sort_add').val() == '请输入自定义查询名称') {
            alert('请输入自定义查询条件的名称。');
            return;
        }
        // return;
        // ajax方式保存自定义查询条件
        // new mainquery().savediyconditon(data);
        $.ajax({
            //diy类库的方法
            url: "action.ashx?method=NTS_BECM.BLL.DiyDeviceSearch.savediyconditon&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                eval('data=' + data);
                if (data.success) {
                    alert('自定义查询信息保存成功。');
                    // 改变一下查询条件的列表
                    new cmaindevice().showdiylist();
                    DivCloseUp();
                    return;
                } else { alert(data.msg); }
            }
        });
    },
    showcdevicelist: function () {
        $.ajax({
            //diy类库的方法
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.showcdevicelist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            async: false,
            success: function (data) {
                $("#cdevicelist").html("");
                //alert(data);
                if (data != ']') {
                    eval('data=' + data);
                    for (var i = 0; i < data.length; i++) {
                        $("#cdevicelist").append("<li title='" + data[i].F_MeterName + "' config=" + data[i].F_MeterID + " style='width:93px;'>" + (data[i].F_MeterName.length > 6 ? data[i].F_MeterName.substring(0, 6) + "..." : data[i].F_MeterName) + " <a onclick=\"new cmaindevice().showcdevicelistdel('" + data[i].F_MeterID + "');\">×</a> </li>");
                    }
                } else {
                    $("#cdevicelist").html("");
                }

                $("#cdevicelist li").click(function () {
                    // alert('xx');
                    if ($("#sdeviceid").val() == '') {
                        alert('对比的设备列表不能为空。');
                    } else {
                        var olddevicelist = $("#sdeviceid").val();
                        //alert(olddevicelist.indexOf($(this).attr('config') + ","));
                        olddevicelist = olddevicelist.replace(new RegExp('BG_', 'g'), ""); // 替换标杆
                        if (olddevicelist.indexOf($(this).attr('config') + ",") > -1) {
                            // alert('aaa');
                            olddevicelist = olddevicelist.replace($(this).attr('config') + ',', '')
                            olddevicelist = "BG_" + $(this).attr('config') + ',' + olddevicelist;
                            //alert(olddevicelist);
                        }
                        else if (olddevicelist.indexOf($(this).attr('config')) > -1) {
                            olddevicelist = olddevicelist.replace(',' + $(this).attr('config'), '')
                            olddevicelist = "BG_" + $(this).attr('config') + ',' + olddevicelist;
                        }
                        else {
                            olddevicelist = "BG_" + $(this).attr('config') + ',' + olddevicelist;
                        }
                        $("#sdeviceid").val(olddevicelist); // 最新的设备的对比的列表
                        new cmaindevice().showdevicechart('6');
                    }

                });
            }
        });
    },
    showcdevicelistdel: function (v) {
        $("#deldeviceid").val(v);
        $("#delbg").show();
        $("#equip_del").show();
    },
    btndatadel: function () {
        var deviceid = $("#deldeviceid").val();
        $.ajax({
            //diy类库的方法
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.updatecdevice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: { deviceid: deviceid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval("data=" + data);
                if (data.success) {
                    alert('保存成功。');
                    new cmaindevice().showcdevicelist();
                    $("#delbg").hide();
                    $("#equip_del").hide();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
    },
    btnlistdel: function () {
        var deviceid = $("#deldeviceid").val();
        $("#cdevicelist li").remove("li[config=" + deviceid + "]");
        $("#delbg").hide();
        $("#equip_del").hide();
    }
}






function DivCloseUp() {
    jQuery(".popupbox").hide();
    jQuery("#button_add").hide();
}

$(function () {

    new cmaindevice().showdevicechart('1');

    // 添加标杆事件


    /*增加按钮显示单选设备框*/
    jQuery("#zengjia").click(function () {
        jQuery(".popupbox").show();
        jQuery("#equip_up3").show();
        jQuery("#equip_up3").find("input").attr("checked", "");
        jQuery("#equip_up3 .selectedboxcn").html("" + "<em></em>");
        jQuery("#equip_up4").find("input[checked]").each(function (i, val) {
            var tt = $(this).parent().find("em").html();
            if (tt) {
                jQuery("#equip_up3 .selectedboxcn em").before(tt + "、");
            }
        });
    });

    // 显示标杆设备列表
    new cmaindevice().showcdevicelist();

    /*保存按钮*/
    jQuery("#save01").click(function () {
        jQuery("#button_add").show();
        jQuery(".energy_popup17").show();
        var bb11 = jQuery("#date01").val();
        var bb12 = jQuery("#hour01").val();
        jQuery("#bb1").html(bb11 + "日" + bb12 + "点");
        var bb21 = jQuery("#date02").val();
        var bb22 = jQuery("#hour02").val();
        jQuery("#bb2").html(bb21 + "日" + bb22 + "点");
        var bb3 = jQuery(".datesort_fast_list ul li.data_select").html();
        jQuery("#bb3").html(bb3);
        var bb4 = jQuery("span.val").html();
        jQuery("#bb4").html(bb4);
        var bb5 = jQuery("#devicelist ul li.clicksub999").html();
        jQuery("#bb5").html(bb5);
        jQuery("#buildsort").html("设备信息");
        $("#equip_sort_add").val('请输入自定义查询名称');
        if (bb5 == null) { alert('自定义查询条件中设备信息是必须项，请选择后重试！'); DivCloseUp(); return; }

    });


    $("#btnquery").click(function () {
        new cmaindevice().showdevicechart('2');
    });

    // 注册导出按钮事件
    $(".exportbtn").click(function () {

        new cmaindevice().outexcel();
    });

    $("#save01").click(function () {


    });
    // new cmaindevice().showdiylist();
    //  new cmaindevice().showdevicechart('1');

    /*
    $('#date01').DatePicker({
    format: 'Y-m-d',
    date: $('#s').val(),
    current: $('#s').val(),
    starts: 1,
    position: 'r',
    onBeforeShow: function () {
    $('#date01').DatePickerSetDate($('#s').val(), true);
    },
    onChange: function (formated, dates) {
    $('#date01').val(formated);
    $('#date01').DatePickerHide();
    }
    });

    $('#date02').DatePicker({
    format: 'Y-m-d',
    date: $('#s').val(),
    current: $('#s').val(),
    starts: 1,
    position: 'r',
    onBeforeShow: function () {
    $('#date02').DatePickerSetDate($('#s').val(), true);
    },
    onChange: function (formated, dates) {
    $('#date02').val(formated);
    $('#date02').DatePickerHide();
    }
    });
    */

    jQuery("#rank03").click(function () {
        /*
        $("#isall").val('0');
        $(".unitusebox").css("display", "none");
        jQuery(this).removeClass("rank03");
        jQuery(this).addClass("rank03");
        */
        /*设备 弹出框--选中部分*/
        jQuery("#bg").show();
        jQuery("#equip_up").show();
        /*jQuery("#equip_up").find("input").attr("checked", "checked");
        jQuery("#equip_up2").find("input").attr("checked", "checked");

        $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAllDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
        //alert(data);
        if (data != ']') {
        eval("data=" + data);
        for (var i = 0; i < data.length; i++) {
        var htmlt = $("#equip_up .selectedboxcn em:first").html();
        if (htmlt.indexOf(data[i].F_MeterName) == -1) {
        $("#equip_up .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
        }
        }
        }
        }
        });*/
    });



    /*设备 弹出框--不选中取消*/

    $("#equip_up input[type=checkbox]").each(function () {

        $(this).click(function () {
           // alert('');
            //alert('');
            var ts = $("#equip_up .selectedboxcn").html();
            if ($(this).attr("checked")) { // 复选框选中状态
                //  alert($(this).attr("checked"));
                if ($(this).attr('name').indexOf('dbp') > -1) { // 父类的文本框
                    var chname = 'dcha_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {

                        $(this).attr("checked", pobj.attr('checked'));
                        // alert($(this).val());
                        // 建筑范围设备全选
                        /*
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                            type: 'Post',
                            contentType: "application/x-www-form-urlencoded; charset=utf-8",
                            data: { areaid: $(this).val() },
                            timeout: 1000,
                            success: function (data) {
                                //alert(data);
                                if (data != ']') {
                                    eval("data=" + data);
                                    for (var i = 0; i < data.length; i++) {
                                        var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                        if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                                            $("#equip_up .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                                        }
                                    }
                                   
                                }
                            }
                        });*/
                        
                    });
                } else {
                    var cpname = 'dbp' + $(this).attr('name').replace("dcha", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", chobj.attr('checked'))
                    });
                    $.ajax({
                        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                        type: 'Post',
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { areaid: $(this).val() },
                        timeout: 1000,
                        success: function (data) {
                            //alert(data);
                            if (data != ']') {
                                eval("data=" + data);
                                for (var i = 0; i < data.length; i++) {
                                    var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                    if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                                        $("#equip_up .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else { // 复选框未选中状态
                if ($(this).attr('name').indexOf('dbp') > -1) { // 父类的文本框
                    var chname = 'dcha_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        $(this).attr("checked", false);
                        // 建筑范围设备全取消
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                            type: 'Post',
                            contentType: "application/x-www-form-urlencoded; charset=utf-8",
                            data: { areaid: $(this).val() },
                            timeout: 1000,
                            success: function (data) {
                                //alert(data);
                                if (data != ']') {
                                    eval("data=" + data);
                                    for (var i = 0; i < data.length; i++) {
                                        if ($("#equip_up .selectedboxcn em:first span[config=" + data[i].F_MeterID + "]")) {
                                            $("#equip_up .selectedboxcn em:first span").remove("span[config=" + data[i].F_MeterID + "]");

                                        }
                                    }
                                }
                            }
                        });
                    });
                } else {
                    var cpname = 'dbp' + $(this).attr('name').replace("dcha", "");
                    var chobj = $(this);
                    $.ajax({
                        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                        type: 'Post',
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { areaid: $(this).val() },
                        timeout: 1000,
                        success: function (data) {
                            // alert(data);
                            if (data != ']') {
                                eval("data=" + data);
                                for (var i = 0; i < data.length; i++) {
                                    if ($("#equip_up .selectedboxcn em:first span[config=" + data[i].F_MeterID + "]")) {
                                        $("#equip_up .selectedboxcn em:first span").remove("span[config=" + data[i].F_MeterID + "]");

                                    }
                                }
                            }
                        }
                    });
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false)
                    });
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false);
                    });
                }
            }
        });
    });



    jQuery("#equip_up #chongzhi").click(function () {
        $("#equip_up .selectedboxcn").html("" + "<em></em>");
    });
    jQuery("#equip_up2 #chongzhi").click(function () {
        $("#equip_up2 table").find("input").each(function (i, val) {
            var tt = $(this).parent().find("em").html();
            if (tt) {
                //alert("0");
                $("#equip_up .selectedboxcn em").remove();
                var tts = $("#equip_up .selectedboxcn").html();
                var ttc = tt + "、";
                if (tts.indexOf(ttc) > 0) {
                    //alert("3");
                    var tts = tts.replace(ttc, "");
                    $("#equip_up .selectedboxcn").html(tts + "<em></em>");
                }
            } else {
                //alert("无效");
            }
        });
    });




    /*区域-设备弹出框按钮*/
    $("#choice img").click(function () {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { areaid: $(this).attr('config') },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    $("#dtext").html('');
                    for (var i = 0; i < data.length; i++) {
                        var html = "";
                        html += "<span>";
                        html += "<input config='" + data[i].F_MeterName + "' name=dch_" + data[i].F_AreaID + "  type=\"checkbox\" value=\"" + data[i].F_MeterID + "\" />";
                        html += " <em>" + data[i].F_MeterName + "</em>";
                        html += "</span>";
                        $("#dtext").append(html);
                    }
                    /*编写弹出设备的选择和取消事件*/
                   
                    $("#dtext input").each(function () {
                        $(this).click(function () {
                            if ($(this).attr('checked')) {
                                var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                if (htmlt.indexOf($(this).attr('config')) == -1) {
                                    $("#equip_up .selectedboxcn em:first").append('<span config="' + $(this).val() + '">' + $(this).attr('config') + '、</span>');
                                }
                            } else {
                                var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                if ($("#equip_up .selectedboxcn em:first span[config=" + $(this).val() + "]")) {
                                    $("#equip_up .selectedboxcn em:first span").remove("span[config=" + $(this).val() + "]");

                                }
                            }
                        });

                    });

                    $("#equip_up2").css("display", "block");
                    $(this).attr("src", "images/energy-ranking/arrow_btn_down.gif");
                    $("#bg").show();
                    $("#equip_up2").show();
                } else {
                    $("#dtext").html('');
                    alert('该区域下暂无设备信息。');
                }

            }
        });
    });






    /*添加标杆设备状态*/

    $("#equip_up3 input[type=checkbox]").each(function () {
        $(this).click(function () {
            //alert('');
            var ts = $("#equip_up3 .selectedboxcn").html();
            if ($(this).attr("checked")) { // 复选框选中状态
                //  alert($(this).attr("checked"));
                if ($(this).attr('name').indexOf('dbp') > -1) { // 父类的文本框
                    var chname = 'dcha_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {

                        $(this).attr("checked", pobj.attr('checked'));
                        // alert($(this).val());
                        // 建筑范围设备全选
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                            type: 'Post',
                            contentType: "application/x-www-form-urlencoded; charset=utf-8",
                            data: { areaid: $(this).val() },
                            timeout: 1000,
                            success: function (data) {
                                //alert(data);
                                if (data != ']') {
                                    eval("data=" + data);
                                    for (var i = 0; i < data.length; i++) {
                                        var htmlt = $("#equip_up3 .selectedboxcn em:first").html();
                                        if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                                            $("#equip_up3 .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                                        }
                                    }
                                    /*var htmlt = $("#equip_up .selectedboxcn em:first").html();
                                    if ($("#equip_up .selectedboxcn em:first span[config=" + $(this).val() + "]")) {
                                    $("#equip_up .selectedboxcn em:first span").remove("span[config=" + $(this).val() + "]");

                                    }*/
                                }
                            }
                        });
                    });
                } else {
                    var cpname = 'dbp' + $(this).attr('name').replace("dcha", "");
                    var chobj = $(this);
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", chobj.attr('checked'))
                    });
                    $.ajax({
                        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                        type: 'Post',
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { areaid: $(this).val() },
                        timeout: 1000,
                        success: function (data) {
                            //alert(data);
                            if (data != ']') {
                                eval("data=" + data);
                                for (var i = 0; i < data.length; i++) {
                                    var htmlt = $("#equip_up3 .selectedboxcn em:first").html();
                                    if (htmlt.indexOf(data[i].F_MeterName) == -1) {
                                        $("#equip_up3 .selectedboxcn em:first").append('<span config="' + data[i].F_MeterID + '">' + data[i].F_MeterName + '、</span>');
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else { // 复选框未选中状态
                if ($(this).attr('name').indexOf('dbp') > -1) { // 父类的文本框
                    var chname = 'dcha_' + $(this).val();
                    var pobj = $(this);
                    $("input[name=" + chname + "]").each(function () {
                        $(this).attr("checked", false);
                        // 建筑范围设备全取消
                        $.ajax({
                            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                            type: 'Post',
                            contentType: "application/x-www-form-urlencoded; charset=utf-8",
                            data: { areaid: $(this).val() },
                            timeout: 1000,
                            success: function (data) {
                                //alert(data);
                                if (data != ']') {
                                    eval("data=" + data);
                                    for (var i = 0; i < data.length; i++) {
                                        if ($("#equip_up3 .selectedboxcn em:first span[config=" + data[i].F_MeterID + "]")) {
                                            $("#equip_up3 .selectedboxcn em:first span").remove("span[config=" + data[i].F_MeterID + "]");

                                        }
                                    }
                                }
                            }
                        });
                    });
                } else {
                    var cpname = 'dbp' + $(this).attr('name').replace("dcha", "");
                    var chobj = $(this);
                    $.ajax({
                        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                        type: 'Post',
                        contentType: "application/x-www-form-urlencoded; charset=utf-8",
                        data: { areaid: $(this).val() },
                        timeout: 1000,
                        success: function (data) {
                            // alert(data);
                            if (data != ']') {
                                eval("data=" + data);
                                for (var i = 0; i < data.length; i++) {
                                    if ($("#equip_up3 .selectedboxcn em:first span[config=" + data[i].F_MeterID + "]")) {
                                        $("#equip_up3 .selectedboxcn em:first span").remove("span[config=" + data[i].F_MeterID + "]");

                                    }
                                }
                            }
                        }
                    });
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false)
                    });
                    $("input[name=" + cpname + "]").each(function () {
                        $(this).attr("checked", false);
                    });
                }
            }
        });
    });




    $("#only_choice img").click(function () {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetAreaDeviceList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { areaid: $(this).attr('config') },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    $("#ddtext").html('');
                    for (var i = 0; i < data.length; i++) {
                        var html = "";
                        html += "<span>";
                        html += "<input config='" + data[i].F_MeterName + "' name=dch_" + data[i].F_AreaID + "  type=\"checkbox\" value=\"" + data[i].F_MeterID + "\" />";
                        html += " <em>" + data[i].F_MeterName + "</em>";
                        html += "</span>";
                        $("#ddtext").append(html);
                    }
                    /*编写弹出设备的选择和取消事件*/
                    $("#ddtext input").each(function () {
                        $(this).click(function () {
                            if ($(this).attr('checked')) {
                                var htmlt = $("#equip_up3 .selectedboxcn em:first").html();
                                if (htmlt.indexOf($(this).attr('config')) == -1) {
                                    $("#equip_up3 .selectedboxcn em:first").append('<span config="' + $(this).val() + '">' + $(this).attr('config') + '、</span>');
                                }
                            } else {
                                var htmlt = $("#equip_up3 .selectedboxcn em:first").html();
                                if ($("#equip_up3 .selectedboxcn em:first span[config=" + $(this).val() + "]")) {
                                    $("#equip_up3 .selectedboxcn em:first span").remove("span[config=" + $(this).val() + "]");

                                }
                            }
                        });

                    });

                    $("#equip_up4").css("display", "block");
                    $(this).attr("src", "images/energy-ranking/arrow_btn_down.gif");
                    $("#bg2").show();
                    $("#equip_up4").show();
                } else {
                    $("#ddtext").html('');
                    alert('该区域下暂无设备信息。');
                }

            }
        });
    });



});


/**设备按钮**/

function LeftBtnOk1() { // 左侧设备确定第一层
    jQuery("#bg").hide();
    jQuery("#equip_up").hide();
    var cnum = jQuery("#equip_up .selectedboxcn em span").length
    if (cnum == 0) {
        alert('请选择要对比的设备');
        return;
    }
    else if (cnum > 10) {
        alert('要对比的设备最多10个。');
        return;
    }
    else {
        $(".equip_list").html('');
        var i=1;
        $("#equip_up .selectedboxcn em span").each(function () {
            var html = '';
            html += "<span config=" + $(this).attr('config') + ">";
            html += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
            html += "<tr valign=\"middle\" align=\"left\">";
            html += "<td width=\"27\" valign=\"top\" align=\"left\"><img src=\"images/energy-equ/equ_icon0" + i + ".jpg\" width=\"21\" height=\"21\"/></td>";
            html += "<td width=\"84\" align=\"left\" valign=\"middle\"><div class=\"txtbig\">" + $(this).html().replace('、', '') + "</div></td>";
            html += "</tr>";
            html += "</table>";
            html += "</span>";
           // alert(html);
            $(".equip_list").append(html);
            i++;
        });

        var devicelist = '';
        $(".equip_list span").each(function () {
            devicelist += "," + $(this).attr('config');
        });
        devicelist = devicelist.length > 0 ? devicelist.substring(1) : devicelist;
        $("#sdeviceid").val(devicelist);

      //  alert($(".equip_list").html());
        
    }
    //equip_list
}

function LeftBtnOk2() {
    jQuery(".popupbox").hide();
    jQuery("#equip_up2").hide();
}
function LeftBtnClose1() {
    jQuery("#bg").hide();
    jQuery("#equip_up").hide();
    jQuery("#equip_up .selectedboxcn").html("" + "<em></em>");
    jQuery("#equip_up").find("input").attr("checked", "");
}
function LeftBtnClose2() {
    $("#equip_up2").find("input[checked]").each(function (i, val) {
        var tt = $(this).parent().find("em").html();
        if (tt) {
            var ttc = tt + "、";
            var tm = $("#equip_up .selectedboxcn").html();
            var tm = tm.replace(ttc, "");
            $("#equip_up .selectedboxcn").html(tm);
        }
    });
    jQuery(".popupbox").hide();
    jQuery("#equip_up2").hide();
}

function ConfirmDiv4() {
    jQuery("#bg2").hide();
    jQuery("#equip_up4").hide();
}

function DivClosed4() {
    jQuery("#equip_up3 .selectedboxcn").html("" + "<em></em>");
    jQuery("#bg2").hide();
    jQuery("#equip_up4").hide();
}

function DivClosed() {
    jQuery(".popupbox").hide();
    jQuery("#equip_up3 .selectedboxcn").html("" + "<em></em>");
    jQuery("#equip_up3").hide();
}

/**增加按钮**/
function ConfirmDiv3() {
    var cnum=jQuery("#equip_up3 .selectedboxcn em span").length
    switch (cnum) {
        case 0:
            alert('对不起，你还未选择标杆设备，无法添加');
            return;
        case 1:
            // 显示保存数据的窗口

            jQuery(".popupbox").hide();
            jQuery("#equip_up3").hide();
            $("#bg6").show();
            $("#equip_up6").show();
            break;
        default:
            alert('对不起，标杆设备只能选择一个');
            return;
    }


}

function ConfirmDiv9() {

    var deviceid = $("#equip_up3 .selectedboxcn em span").attr('config');
    $.ajax({
        //diy类库的方法
        url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.updatecdeviceok&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        type: 'Post',
        data: { deviceid: deviceid },
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            // alert(data);
            eval("data=" + data);
            if (data.success) {
                alert('保存成功。');
                new cmaindevice().showcdevicelist();
                $("#bg6").hide();
                $("#equip_up6").hide();
            } else {
                alert(data.msg);
                return false;
            }
        }
    });
 
}


function DivClosed9() {
    var deviceid = $("#equip_up3 .selectedboxcn em span").attr('config');
    var devicename = $("#equip_up3 .selectedboxcn em span").html();
    //alert(deviceid);
    var flag = '0';
    $("#cdevicelist li").each(function () {
        if ($(this).attr('config') == deviceid) {
            alert('对不起，该设备已经在标杆设备列表中了。');
            $("#bg6").hide();
            $("#equip_up6").hide();
            flag = '1';
        }
    });
    if (flag == '0') {
        $("#cdevicelist").append("<li title='" + devicename + "' config=" + deviceid + " style='width:93px;'>" + (devicename.length > 6 ? devicename.substring(0, 6) + "..." : devicename) + " <a onclick=\"new cmaindevice().showcdevicelistdel('" + deviceid + "');\">×</a> </li>");
    }

    $("#cdevicelist li").click(function () {
        // alert('xx');
        if ($("#sdeviceid").val() == '') {
            alert('对比的设备列表不能为空。');
        } else {
            var olddevicelist = $("#sdeviceid").val();
            //alert(olddevicelist.indexOf($(this).attr('config') + ","));
            olddevicelist = olddevicelist.replace(new RegExp('BG_', 'g'), ""); // 替换标杆
            if (olddevicelist.indexOf($(this).attr('config') + ",") > -1) {
                // alert('aaa');
                olddevicelist = olddevicelist.replace($(this).attr('config') + ',', '')
                olddevicelist = "BG_" + $(this).attr('config') + ',' + olddevicelist;
                //alert(olddevicelist);
            }
            else if (olddevicelist.indexOf($(this).attr('config')) > -1) {
                olddevicelist = olddevicelist.replace(',' + $(this).attr('config'), '')
                olddevicelist = "BG_" + $(this).attr('config') + ',' + olddevicelist;
            }
            else {
                olddevicelist = "BG_" + $(this).attr('config') + ',' + olddevicelist;
            }
            $("#sdeviceid").val(olddevicelist); // 最新的设备的对比的列表
            new cmaindevice().showdevicechart('6');
        }

    });

    $("#bg6").hide();
    $("#equip_up6").hide();
}




/*
function GetAreaName(areaid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.BArea.GetAreaName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + areaid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetDeviceName(deviceid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_ST_MeterUseInfo.GetDeviceName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + deviceid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

function GetBuildName(buildid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}

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
