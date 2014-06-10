function fn() { }
nts.web9000.JsLibrary.report = fn.prototype = {
    name: '报表js主类',
    showcommchart: function (query) {
        $(".date_period").html("报表统计时间：" + query.starttime + " - " + query.endtime);
        $('.main_select_name div').html('已选择分析的统计量：' + nts.web9000.JsLibrary.report.getrulename(query.ruleid, query.chart));
        $(".show_picture").load("/charts/reportserver.aspx?t=" + new Date().getTime(), query);
    },
    excledata: function (query) {
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.chartbll.excledata&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            async: false,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: query,
            timeout: 1000,
            success: function (data) {
                if (data != ']') {
                    eval('data=' + data);
                    if (data.success) {
                        // alert(data.excelpath);
                        window.location = data.excelpath;
                    } else {
                        alert(data.msg);
                        return;
                    }

                } else {
                    return;
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
                return;
            }
        });
    },
    showdefaultchart: function (chart, ruleid, inval) {
        var query = { ruleid: ruleid, inval: inval, chart: chart, starttime: '', endtime: '', chartwidth: 653, chartheight: 550, chartsharp: 0 }
        if (inval == 0) {
            query.starttime = GetToday();
            query.endtime = GetNow();
        } else {
            query.starttime = GetCuttMonth();
            query.endtime = GetToday();
        }

        /*统计特征量赋值*/
        $("#s_ruleid").val(query.ruleid);
        $("#s_inval").val(query.inval);
        $("#s_chart").val(query.chart);
        $("#s_starttime").val(query.starttime);
        $("#s_endtime").val(query.endtime);
        $("#s_chartwidth").val(query.chartwidth);
        $("#s_chartheight").val(query.chartheight);
        $("#s_chartsharp").val(query.chartsharp);

        $(".date_period").html("报表统计时间：" + query.starttime + " - " + query.endtime);
        $('.main_select_name div').html('已选择分析的统计量：' + nts.web9000.JsLibrary.report.getrulename(query.ruleid, query.chart));
        //alert($('#A2').html());
        $(".show_picture").load("/charts/reportserver.aspx?t=" + new Date().getTime(), query);
        //$('.main_select_name div').html('');
    },
    getrulename: function (rulelist, chart) {
        var d = '';
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.chartbll.getdefaultrulelistname&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            async: false,
            data: { rulelist: rulelist, chart: chart },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                d = data;

            }
        });
        return d;
    },
    showchartlist: function (data) { //data为应用的json实体对象
        var v = data.supportmaps;
        if (v == '') { alert('该报表应用未配置公式。'); return; }
        v = v.substring(1);
        var arr = v.split(',');
        $("#chartlist").html('');

        if (arr.length == 1) {
            switch (Number(arr[0])) {
                case 1:
                    $("#chartlist").append('<li><input checked name="pic"  type="radio" value="' + arr[0] + '" /> <em>统计图</em> <img src="images/down_btn.gif"/></li>');
                    break;
                case 2:
                    $("#chartlist").append('<li><input checked name="pic"  type="radio" value="' + arr[0] + '" /> <em>对比图</em> <img src="images/down_btn.gif"/></li>');
                    break;
                case 3:
                    $("#chartlist").append('<li><input checked name="pic"  type="radio" value="' + arr[0] + '" /> <em>饼图占比</em> <img src="images/down_btn.gif"/></li>');
                    break;
                default:
                    $("#chartlist").append('<li><input checked name="pic"  type="radio" value="' + arr[0] + '" /> <em>数据表格</em> <img src="images/down_btn.gif"/></li>');
                    break;
            }

        } else {
            for (var i = 0; i < arr.length; i++) {
                switch (Number(arr[i])) {
                    case 1:
                        $("#chartlist").append('<li><input name="pic"  type="radio" value="' + arr[i] + '" /> <em>统计图</em> <img src="images/down_btn.gif"/></li>');
                        break;
                    case 2:
                        $("#chartlist").append('<li><input name="pic"  type="radio" value="' + arr[i] + '" /> <em>对比图</em> <img src="images/down_btn.gif"/></li>');
                        break;
                    case 3:
                        $("#chartlist").append('<li><input name="pic"  type="radio" value="' + arr[i] + '" /> <em>饼图占比</em> <img src="images/down_btn.gif"/></li>');
                        break;
                    default:
                        $("#chartlist").append('<li><input name="pic"  type="radio" value="' + arr[i] + '" /> <em>数据表格</em> <img src="images/down_btn.gif"/></li>');
                        break;
                }
            }
            $("input[name='pic']:first").attr('checked', true);
        }

        $(".pic_type ul li img").click(function () {
            //单选改变
            $(this).parent().find("input").attr("checked", true);
            $("#s_chart").val($(this).parent().find("input:checked").val());
            $(".popupbox").show();
            $(".picture_type_dialog").show();
            $("#key1").val('请输入搜索应用');
            nts.web9000.JsLibrary.report.showapplicationrulelist();
            if ($(this).parent().find("em").html() == "统计图") {
                // 显示应用报表下面的公式列表

                $(".secondpic_btn").hide();
            } else {
                $(".secondpic_btn").show();
            }
        });

        // 注册li的事件
        $("#chartlist input[type=radio]").click(function () {
            if (Number($(this).val()) > 2) {
                $(".linepic").hide();
            } else {
                $(".linepic").show();
            }
            $("#s_chart").val($(this).val());
            var query = { ruleid: $("#s_ruleid").val(), inval: $("#s_inval").val(), chart: $(this).val(), starttime: $("#s_starttime").val(), endtime: $("#s_endtime").val(), chartwidth: $("#s_chartwidth").val(), chartheight: $("#s_chartheight").val(), chartsharp: $("#s_chartsharp").val() }
            nts.web9000.JsLibrary.report.showcommchart(query);
        });

        // 判断显示默认的报表
        if ($("input[name='pic']:checked").val() == '1') {
            // 统计图,
            nts.web9000.JsLibrary.report.showdefaultchart(1, data.rulecollection, data.interval);

        } else {
            nts.web9000.JsLibrary.report.showdefaultchart($("input[name='pic']:checked").val(), data.rulecollection, data.interval);
        }
    },
    showapplicationrulelist: function () {
        var applicationid = $("#applicationid").val();
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.chartbll.getrulelistbyapplication&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            data: { applicationid: applicationid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $(".secondpic_box").html('');
                if (data != ']') {
                    eval('data=' + data);
                    //alert(data.length);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += '<span class="secondpic_type" config="' + data[i].ID + '">';
                        html += '<span class="application_left"></span>';
                        html += '<span  class="name">' + data[i].CNAME + '</span>';
                        html += '<span class="application_right"></span>';
                        html += '<span class="clear"></span>';
                        html += '</span>';
                        $('.secondpic_box').append(html);
                    }


                    $(".secondpic_type").click(function () {
                        var secondpic_type;
                        $(".pic_type input").each(function () {
                            if ($(this).attr("checked") == true) {
                                secondpic_type = $(this).parent().find("em").html();
                            }
                        });
                        if (secondpic_type == "统计图") {

                            $(this).addClass("secondpic_select").siblings().removeClass("secondpic_select");
                            $('.main_select_name div').html('已选择分析的统计量：' + $(this).find('.name').html());
                            $("#s_ruleid").val($(this).attr('config'));
                            var query = { ruleid: $("#s_ruleid").val(), inval: $("#s_inval").val(), chart: $("#s_chart").val(), starttime: $("#s_starttime").val(), endtime: $("#s_endtime").val(), chartwidth: $("#s_chartwidth").val(), chartheight: $("#s_chartheight").val(), chartsharp: $("#s_chartsharp").val() }
                            nts.web9000.JsLibrary.report.showcommchart(query);

                            //关闭对话框
                            $(".popupbox").hide();
                            $(".picture_type_dialog").hide();
                            $(".pic_type").find("input:checked").parent().find("img").attr("src", "images/up_btn.gif");
                        } else {
                            if ($(this).hasClass("secondpic_select")) {
                                $(this).removeClass("secondpic_select");
                            } else {
                                $(this).addClass("secondpic_select");
                            }
                            var main_select_name = "";
                            $('.secondpic_type').each(function (i, val) {
                                if ($(this).hasClass('secondpic_select')) {
                                    main_select_name = main_select_name + $(this).find('.name').html() + '、';
                                }
                            });
                            $('.main_select_name div').html('已选择分析的统计量：' + main_select_name);
                        }
                    });

                } else {
                    return;
                }
            }
        });
    }
}


$(function () {
    // 显示第一个报表应用
    $.ajax({
        url: "action.ashx?method=nts.web9000.bll.chartbll.getfirstapp&dll=nts.web9000.bll&times=" + new Date().getTime(),
        type: 'Post',
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        timeout: 1000,
        success: function (data) {
            if (data != ']') {
                eval('data=' + data);
                $(".app_name").html(data[0].applicationname);
                nts.web9000.JsLibrary.report.showchartlist(data[0]);
                $(".exportbtn").css("display", data[0].isexportexcel == '1' ? "block" : "none");
                $("#applicationid").val(data[0].id);
                // 判断显示默认的报表

            } else {
                alert('对不起，您还未添加报表的应用。');
                return;
            }
        }
    });


    $(".linepic").mouseover(function () {
        $(this).find("ul").show();
    });
    $(".linepic").mouseout(function () {
        $(this).find("ul").hide();
    });
    $(".linepic ul li").click(function () {
        $(this).parent().parent().find("span").html($(this).html());
        $(this).parent().hide();
        $("#s_chartsharp").val($(this).attr('config'));
        var query = { ruleid: $("#s_ruleid").val(), inval: $("#s_inval").val(), chart: $("#s_chart").val(), starttime: $("#s_starttime").val(), endtime: $("#s_endtime").val(), chartwidth: $("#s_chartwidth").val(), chartheight: $("#s_chartheight").val(), chartsharp: $(this).attr('config') }
        nts.web9000.JsLibrary.report.showcommchart(query);
    });

    // 注册按钮事件
    $("#btnquery").click(function () {
        if ($("#starttime").val() == '' || $("#endtime").val() == '') {
            alert('请输入查询的时间');
            return;
        } else {
            if (compareTime($("#starttime").val(), $("#endtime").val())) {
                $("#s_starttime").val($("#starttime").val());
                $("#s_endtime").val($("#endtime").val());
                var query = { ruleid: $("#s_ruleid").val(), inval: $("#s_inval").val(), chart: $("#s_chart").val(), starttime: $("#s_starttime").val(), endtime: $("#s_endtime").val(), chartwidth: $("#s_chartwidth").val(), chartheight: $("#s_chartheight").val(), chartsharp: $("#s_chartsharp").val() }
                nts.web9000.JsLibrary.report.showcommchart(query);
            }
        }


    });

    $(".exportbtn").click(function () {
        var query = { ruleid: $("#s_ruleid").val(), inval: $("#s_inval").val(), chart: $("#s_chart").val(), starttime: $("#s_starttime").val(), endtime: $("#s_endtime").val(), chartwidth: $("#s_chartwidth").val(), chartheight: $("#s_chartheight").val(), chartsharp: $("#s_chartsharp").val() }
        nts.web9000.JsLibrary.report.excledata(query);
    });
});