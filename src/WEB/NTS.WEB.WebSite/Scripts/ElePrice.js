function fn() { }
nts.web9000.JsLibrary.eleprice = fn.prototype = {
    showelepriceapplist: function (page, data) {
        // alert(page);
        var newdata = {
            page: page,
            pagesize: data.pagesize,
            starttime: data.starttime,
            endtime: data.endtime,
            lineclass: data.lineclass,
            areaclass: data.areaclass,
            levelclass: data.levelclass,
            tclass: data.tclass
        };
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.ElePriceMain.getelepricelist&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'json',
            async: false,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            //data: { buildid: buildid, current_page: current_page, pagesize: 6, areaid: areaid },
            data: newdata,
            timeout: 1000,
            success: function (data) {
                // alert(data);
                // return;
                if (data) {
                    if (data.success) {
                        $("#mainrecord").html('');
                        if (data.list.length > 0) {
                            var htmls = '';
                            for (var i = 0; i < data.list.length; i++) {
                                htmls += "<li style='width:1540px'>";
                                htmls += '<table width="1540" border="0" cellpadding="0" cellspacing="0" >';
                                htmls += '<tr height="25">';
                                htmls += '<td width="120"><span style="padding-left:15px">' + data.list[i].powerindex + '</span></td>';
                                htmls += '<td width="120">&nbsp;' + data.list[i].描述名 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].费用 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].总电量 + '</td>';
                                htmls += '<td width="120">&nbsp;' + data.list[i].非分时电量 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].电量基价 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].小时电价 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].峰时电价 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].平时电价 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].谷时电价 + '</td>';
                                htmls += '<td width="100">&nbsp;' + data.list[i].启用分时计量 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].峰时电量 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].平时电量 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].谷时电量 + '</td>';
                                htmls += '<td width="100">&nbsp;' + data.list[i].线路 + '</td>';
                                htmls += '<td width="100">&nbsp;' + data.list[i].区域 + '</td>';
                                htmls += '<td width="80">&nbsp;' + data.list[i].电压等级 + '</td>';
                                htmls += "</tr>";
                                htmls += "</table>";
                                htmls += "</li>";

                            }
                            $("#mainrecord").append(htmls);
                        }
                        else {
                            $("#mainrecord").html('<font color=red>对不起，暂无数据。</font>');
                        }

                        $("#chartmaps").load('ElePrice.aspx?t=' + new Date().getDate(), newdata);
                        $("#chartmaps1").load('ElePrice.aspx?type=line&t=' + new Date().getDate(), newdata);
                        $("#url").val($.ajax({ url: "action.ashx?method=nts.web9000.bll.ElePriceMain.exportexcel&dll=nts.web9000.bll&times=" + new Date().getTime(), data: newdata, type: 'GET', async: false, cache: false }).responseText);
                    }
                    else {
                        alert(data.message);
                        return;
                    }

                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
                return;
            }
        });
    }
}



$(function () {

    // 
    var data = {
        page: 1,
        pagesize: 20,
        starttime: $("#starttime").val(),
        endtime: $("#endtime").val(),
        lineclass: $("#LineClass").val(),
        areaclass: $("#AreaClass").val(),
        levelclass: $("#LevelClass").val(),
        tclass: $("#tclass").val()
    }
    var maxpage = $.ajax({ url: "action.ashx?method=nts.web9000.bll.ElePriceMain.getelepricelistcount&dll=nts.web9000.bll&times=" + new Date().getTime(), type: 'GET', data: data, async: false, cache: false }).responseText;

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

            nts.web9000.JsLibrary.eleprice.showelepriceapplist(pages, data);
        }
    });
    nts.web9000.JsLibrary.eleprice.showelepriceapplist(1, data);
    // 注册按钮事件

    $("#exportbtn").click(function () {
        window.location = $("#url").val();
    });
    $(".alarm_search").click(function () {
        var data = {
            page: 1,
            pagesize: 20,
            starttime: $("#starttime").val(),
            endtime: $("#endtime").val(),
            lineclass: $("#LineClass").val(),
            areaclass: $("#AreaClass").val(),
            levelclass: $("#LevelClass").val(),
            tclass: 0
        }
        // alert(data.sy);
        var maxpage = $.ajax({ url: "action.ashx?method=nts.web9000.bll.ElePriceMain.getelepricelistcount&dll=nts.web9000.bll&times=" + new Date().getTime(), type: 'GET', data: data, async: false, cache: false }).responseText;
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

                nts.web9000.JsLibrary.eleprice.showelepriceapplist(pages, data);
            }
        });
        nts.web9000.JsLibrary.eleprice.showelepriceapplist(1, data);
    });

    $(".thisdate").click(function () {
        $(this).addClass("selectdate").siblings().removeClass("selectdate");
        var starttime = '';
        var endtime = '';
        var tclass = $(this).attr('config');


        switch (Number($(this).attr('config'))) {
            case 1:
                starttime = GetToday();
                endtime = GetNow();
                break;
            case 2:
                starttime = GetCuttMonth();
                endtime = GetToday();
                break;
            case 3:
                starttime = GetCuttYear();
                endtime = GetToday();
                break;
            default:
                starttime = GetCuttYear();
                endtime = GetToday();
                tclass = $("#HisClass").val();


                if (Number(tclass) == 4) {
                    starttime = $("#HisDate").val();
                    endtime = '2112-1-1';
                }
                else if (Number(tclass) == 5) {
                    starttime = $("#HisYear").val() + '-' + $("#HisMonth").val() + "-1";
                    endtime = '2112-1-1';
                }
                else {
                    starttime = $("#HisYear").val() + "-1-1";
                    endtime = '2112-1-1';
                }
                break;
        }



        var data = {
            page: 1,
            pagesize: 20,
            starttime: starttime,
            endtime: endtime,
            lineclass: $("#LineClass").val(),
            areaclass: $("#AreaClass").val(),
            levelclass: $("#LevelClass").val(),
            tclass: tclass
        }
        // alert(data.sy);
        var maxpage = $.ajax({ url: "action.ashx?method=nts.web9000.bll.ElePriceMain.getelepricelistcount&dll=nts.web9000.bll&times=" + new Date().getTime(), type: 'GET', data: data, async: false, cache: false }).responseText;
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

                nts.web9000.JsLibrary.eleprice.showelepriceapplist(pages, data);
            }
        });
        nts.web9000.JsLibrary.eleprice.showelepriceapplist(1, data);
    });
})


