function fn() { }

NTSAlarm = fn.prototype = {
    ShowAlarmApplist: function(page, data) {
        // alert(page);
        var newdata = {
            page: page,
            pagesize: data.pagesize,
            starttime: data.starttime,
            endtime: data.endtime,
            alarmclass: data.alarmclass,
            sy: data.sy
        };
        NTS.NTSAjax(AjaxUrl9K.AlarmList, newdata, function(data) {
            $("#kgbwl").css("display", "none");
            $("#soe").css("display", "none");
            $("#bhsj").css("display", "none");
            $("#yk").css("display", "none");
            $("#ycyx").css("display", "none");
            $("#jdxx").css("display", "none");
            $("#mainrecord").html('');
            var htmls = '';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    switch (Number(newdata.alarmclass)) {
                    case 1:
                        htmls += "<li>";
                        htmls += '<table width="100%" border="0" cellpadding="0" cellspacing="0" >';
                        htmls += '<tr height="25">';


                        htmls += '<td width="22%"><span style="padding-left:15px">' + data[i].DATETIME + '</span></td>';
                        htmls += '<td width="20%">' + data[i].STACNAME + '&nbsp;</td>';
                        htmls += '<td width="15%">' + data[i].DEVCNAME + '&nbsp;</td>';
                        htmls += '<td width="16%">' + data[i].OBJECTNAME + '&nbsp;</td>';
                        htmls += '<td width="10%">' + data[i].ACTIONTYPE + '&nbsp;</td>';
                        htmls += '<td width="17%">' + GetSystemName(data[i].SYSTEMID) + '&nbsp;</td>';
                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";
                        $("#kgbwl").css("display", "");
                        break;
                    case 2:
                        htmls += "<li>";
                        htmls += '<table width="100%" border="0" cellpadding="0" cellspacing="0" >';
                        htmls += '<tr height="25">';


                        htmls += '<td width="22%"><span style="padding-left:15px">' + data[i].DATETIME + '</span></td>';
                        htmls += '<td width="20%">' + data[i].STATIONNAME + '</td>';
                        htmls += '<td width="32%">' + data[i].DEVCNAME + '</td>';
                        htmls += '<td width="16%">' + data[i].OBJECTNAME + '</td>';
                        htmls += '<td width="10%">' + data[i].EVENTTYPE + '</td>';

                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";
                        $("#soe").css("display", "");
                        break;
                    case 3:
                        htmls += "<li>";
                        htmls += '<table width="100%" border="0" cellpadding="0" cellspacing="0" >';
                        htmls += '<tr height="25">';


                        htmls += '<td width="22%"><span style="padding-left:15px">' + data[i].DATETIME + '</span></td>';
                        htmls += '<td width="20%">' + data[i].STATIONNAME + '</td>';
                        htmls += '<td width="20%">' + data[i].DEVCNAME + '</td>';
                        htmls += '<td width="16%">' + data[i].OBJECTNAME + '</td>';
                        htmls += '<td width="22%">' + data[i].EVENTTYPE + '</td>';

                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";
                        $("#bhsj").css("display", "");
                        break;
                    case 4:
                        htmls += "<li>";
                        htmls += '<table width="100%" border="0" cellpadding="0" cellspacing="0" >';
                        htmls += '<tr height="25">';


                        htmls += '<td width="22%"><span style="padding-left:15px">' + data[i].DATETIME + '</span></td>';
                        htmls += '<td width="15%">' + data[i].STACNAME + '</td>';
                        htmls += '<td width="12%">' + data[i].DEVCNAME + '</td>';
                        htmls += '<td width="14%">' + data[i].OBJECTNAME + '</td>';
                        htmls += '<td width="10%">' + data[i].OPERATIONTYPE + '</td>';
                        htmls += '<td width="10%">' + data[i].OPERATOR + '</td>';
                        htmls += '<td width="17%">' + GetSystemName(data[i].SYSTEMID) + '&nbsp;</td>';
                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";
                        $("#yk").css("display", "");
                        break;
                    case 5:
                        htmls += "<li>";
                        htmls += '<table width="100%" border="0" cellpadding="0" cellspacing="0" >';
                        htmls += '<tr height="25">';


                        htmls += '<td width="22%"><span style="padding-left:15px">' + data[i].DATETIME + '</span></td>';
                        htmls += '<td width="15%">' + data[i].STACNAME + '</td>';
                        htmls += '<td width="20%">' + data[i].DEVCNAME + '</td>';
                        htmls += '<td width="16%">' + data[i].OBJECTNAME + '</td>';
                        htmls += '<td width="10%">' + data[i].ACTIONTYPE + '</td>';
                        htmls += '<td width="17%">' + GetSystemName(data[i].SYSTEMID) + '&nbsp;</td>';
                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";
                        $("#ycyx").css("display", "");
                        break;
                    default:
                        htmls += "<li>";
                        htmls += '<table width="100%" border="0" cellpadding="0" cellspacing="0" >';
                        htmls += '<tr height="25">';


                        htmls += '<td width="22%"><span style="padding-left:15px">' + data[i].DATETIME + '</span></td>';
                        htmls += '<td width="20%">' + data[i].PRJCNAME + '</td>';
                        htmls += '<td width="32%">' + data[i].NODENAME + '</td>';
                        htmls += '<td width="16%">' + data[i].ACTIONNAME + '</td>';
                        htmls += '<td width="10%"></td>';

                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";
                        $("#jdxx").css("display", "");
                        break;
                    }
                }
                $("#mainrecord").append(htmls);
            } else {
                $("#mainrecord").html('<font color=red>对不起，暂无数据。</font>');
            }
        });

    }
};

function GetSystemName(systemid) {
    return $.ajax({ url: "action.ashx?method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDCA2A20EAD185735F6FAA35B9BE12E3313B1918834FFB67D1&dll=A247FF928CF85A77F2813AEDCFFB056B&times=" + new Date().getTime(), type: 'GET', data: { systemid: systemid }, async: false, cache: false }).responseText;
}


$(function () {
    var data = {
        page: 1,
        pagesize: 20,
        starttime: $("#starttime").val(),
        endtime: $("#endtime").val(),
        alarmclass: $("#AlarmClass").val(),
        sy: ''
    };
    var maxpage = $.ajax({ url: "action.ashx?method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDCA2A20EAD185735F64E04A43C5FA708F04AF78C5C2872DAA36191A9D6017BF0B&dll=A247FF928CF85A77F2813AEDCFFB056B&times=" + new Date().getTime(), type: 'GET', data: data, async: false, cache: false }).responseText;
    $("#cp").val(maxpage);
    $("#classid").remove();
    $("#pp").append("<div class='pagination' id='classid'></div>");
    //$('.pagination#classid').html('');
    $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
    $('.pagination#classid').jqPagination({
        link_string: '/?page={page_number}',
        current_page: 1, //设置当前页 默认为1
        max_page: maxpage, //设置最大页 默认为1
        page_string: '当前第{current_page}页,共{max_page}页',
        paged: function (pages) {
            if (pages > $("#cp").val()) { return; }

            NTSAlarm.ShowAlarmApplist(pages, data);
        }
    });
    NTSAlarm.ShowAlarmApplist(1, data);
    // 注册按钮事件

    $(".alarm_search").click(function () {
        var data = {
            page: 1,
            pagesize: 20,
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            alarmclass: $("#AlarmClass").val(),
            sy: $("#SystemClass").val()
        };
        // alert(data.sy);
        var maxpage = $.ajax({ url: "action.ashx?method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDCA2A20EAD185735F64E04A43C5FA708F04AF78C5C2872DAA36191A9D6017BF0B&dll=A247FF928CF85A77F2813AEDCFFB056B&times=" + new Date().getTime(), type: 'GET', data: data, async: false, cache: false }).responseText;
        $("#cp").val(maxpage);
        $("#classid").remove();
        $("#pp").append("<div class='pagination' id='classid'></div>");
        //$('.pagination#classid').html('');
        $('.pagination#classid').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#classid').jqPagination({
            link_string: '/?page={page_number}',
            current_page: 1, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp").val()) { return; }

                NTSAlarm.ShowAlarmApplist(pages, data);
            }
        });
        NTSAlarm.ShowAlarmApplist(1, data);
    });
})