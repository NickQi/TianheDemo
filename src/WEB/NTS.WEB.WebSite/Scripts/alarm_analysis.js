$(function () {
    //input box change	   
    $("input.alarm_datetime").focus(function () {
        $(this).css('border', '1px solid #167fc5');
    });
    $("input.alarm_datetime").blur(function () {
        $(this).css('border', '1px solid #cecece');
    });
    //search button click
    $("span.alarm_search").click(function () {

    });
    //this day ,this weak, this month click
    $("input.thisdate,#btn").click(function () {
        $(this).addClass("selectdate").siblings().removeClass("selectdate");
        if ($(this).attr('id') == 'btn') { $("input.thisdate").removeClass("selectdate"); } else { $("#btn").removeClass("selectdate"); }
        var starttime = '';
        var endtime = '';
        switch (Number($(this).attr('config'))) {
            case 1:
                starttime = GetToday();
                endtime = GetToday();
                break;
            case 2:
                starttime = GetWeekFirstDay();
                endtime = GetToday();
                break;
            default:
                starttime = GetCuttMonth();
                endtime = GetToday();
                break;
        }

        var data = {
            page: 1,
            pagesize: 20,
            starttime: starttime,
            endtime: endtime,
            alarmclass: $("#AlarmClass").val(),
            sy: $("#SystemClass").val()
        };
        var maxpage = $.ajax({ url: "action.ashx?method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDCA2A20EAD185735F64E04A43C5FA708F04AF78C5C2872DAA36191A9D6017BF0B&dll=A247FF928CF85A77F2813AEDCFFB056B&times=" + new Date().getTime(), type: 'GET', data: data, async: false, cache: false }).responseText;
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

                NTSAlarm.ShowAlarmApplist(pages, data);
            }
        });
        NTSAlarm.ShowAlarmApplist(1, data);
    });
});