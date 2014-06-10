var PaddingdataModel = null;
NTSBOM = function () {
} .prototype = {
    name: '综合查询',
    ShowComReportList: function () {
        setManyDefault();
        if (ReportData.BaseData.objectid == '') {
            alert('请选择需要统计分析的对象');
            return;
        }
        if ($("#mainLayer").get(0).options[$("#mainLayer").get(0).selectedIndex].text == '设备') {
            ReportData.BaseData.objecttype = 1;
            $(".unitusebox").hide();
        } else {
            ReportData.BaseData.objecttype = 0;
            $(".unitusebox").show();
        }
        

        $(".export_ttsmall").html(ReportData.BaseData.starttime + ' - ' + ReportData.BaseData.endtime);
        $("#qlabel,#gundongtxt").html($("#objectname").val());

        $("#ListPagingID").AjaxPaging({
            listId: "PagingList",
            url: AjaxUrl.ShowComReportList,
            data: ReportData.BaseData,
            pageSize: 22,
            callback: function (listId, result) {
                ((PaddingdataModel != null) || ((PaddingdataModel = ko.mapping.fromJS(result)) && ko.applyBindings(PaddingdataModel, $("#PagingListData")[0]))) && ko.mapping.fromJS(result, PaddingdataModel);
            }
        });
    }
};

/*页面级别的js部分*/



/*返回主页*/
function ReturnShow(flag) {
    jQuery("#A" + flag).show();
    jQuery("#B" + flag).show();
    jQuery("#C" + flag).show();
    jQuery("#D" + flag).hide();
}
function ReturnHide(flag) {
    jQuery("#A" + flag).hide();
    jQuery("#B" + flag).hide();
    jQuery("#C" + flag).hide();
    jQuery("#D" + flag).show();
}
function ShowBox() {
    jQuery("#A" + flag).hide();
    jQuery("#B" + flag).hide();
    jQuery("#C" + flag).hide();
    jQuery("#D" + flag).show();
}





/*确定按钮*/
/**排序导出按钮**/
function ConfirmDiv0() {
    jQuery(".popupbox").hide();
    jQuery("#paixubox").hide();
}

function ConfirmDivs0() {
    jQuery(".popupbox").hide();
    jQuery("#paixubox").hide();
}

function ConfirmDiv3() {
    jQuery(".popupbox").hide();
    jQuery("#equip_up2").hide();
}







$(function () {

    $("#starttime").val(GetCuttMonth());
    $("#endtime").val(GetToday());
    $("#itemcode").val('1');
    $("#deptunit").val('0');
    $("#unit").val('3');

    /*新闻播报*/
    jQuery(".ranking_ttbig span.rank_btn").click(function () {
        jQuery(".ranking_ttbig .gundongbox").show(1000);
    });
    jQuery(".ranking_ttbig .gundongbox span").click(function () {
        jQuery(".ranking_ttbig .gundongbox").hide(1000);
    });
    // alert($("#gundongtxt").html());
    /*排序 导出*/
    jQuery("#arrow01").click(function () {
        jQuery("#rankbox01").hide();
        jQuery("#rankbox02").show();
        $("#order").val('desc');
    });
    jQuery("#arrow02").click(function () {
        jQuery("#rankbox01").show();
        jQuery("#rankbox02").hide();
        $("#order").val('asc');
    });

    /*单位用量*/
    jQuery(".unitusebox").mouseover(function () {
        jQuery(".unitusebox ul").show();
    });
    jQuery(".unitusebox").mouseout(function () {
        jQuery(".unitusebox ul").hide();
    });

    /*导出表格按钮*/
    jQuery("#exportbtn").click(function () {
        ReportData.BaseData.type = $("#deptunit").val();
        $(this).FileExport({
            url: AjaxUrl.ExportReportList,
            data: ReportData.BaseData
        });
    });

    $(".unitusebox li").click(function () {
        $("#chooselabel").html($(this).html());
        $("#deptunit").val($(this).attr('config'));
        ShowChartQueryReport();
    });

    jQuery(".sidecolumn_middle .title span.query_senior").click(function () {
        jQuery(".sidecolumn_middle .title span.query").removeClass("pitch_on");
        jQuery(this).addClass("pitch_on");
        // jQuery(".search_button").hide();
        // jQuery(".date_button").show();
        //jQuery("#custombox").hide();
        $("body").append('<div class="custom_box"><table width="240" style="color:#5D5D5D;float:left"><tr height="18"><td colspan="2"></td></tr><tr height="35"><td width="85" align="right">开始时间：</td><td width="155" align="left"><input class="alarm_datetime" id="stime" type="text" Readonly value="" onclick="return SelectDate(this,\'yyyy-MM-dd\')" style="width:120px;padding-left:8px"></td></tr><tr height="35"><td width="85" align="right">结束时间：</td><td width="155" align="left"><input Readonly class="alarm_datetime" type="text" id="etime" value="" onclick="return SelectDate(this,\'yyyy-MM-dd\')" style="width:120px;padding-left:8px"></td></tr><tr height="40" align="center"><td width="85"></td><td width="155" align="left"><span class="custom_submit">确定</span><span class="custom_cancel">取消</span></td></tr></table></div>');
        //input box change	   
        $("input.alarm_datetime").focus(function () {
            $(this).css('border', '1px solid #167fc5');
        });
        $("input.alarm_datetime").blur(function () {
            $(this).css('border', '1px solid #cecece');
        });
        //submit
        $("span.custom_submit").click(function () {
            if ($("#stime").val() == '' || $("#etime").val() == '') {
                alert('请输入统计的起始和结束时间。');
                return;
            } else {
                var stime = $("#stime").val();
                var etime = $("#etime").val();
                if (compareTime(stime, etime)) {
                    $(".popupbox").hide();
                    $(".custom_box").remove();
                    $("#unit").val('5');
                    $("#starttime").val(stime);
                    $("#endtime").val(etime);
                    $(".search_button span").removeClass("select");
                    ShowChartQueryReport();
                }
            }

        });
        //cancel
        $("span.custom_cancel").click(function () {
            $(".popupbox").hide();
            $(".custom_box").remove();
        });
    });
    
    /*缩进*/


    /*功能区*/
    jQuery(".search_button span").click(function () {
        var ss = jQuery(this).html();
        $(this).addClass("select").siblings().removeClass("select");
        switch (ss) {
            case "本月":
                $("#starttime").val(GetCuttMonth());
                $("#endtime").val(GetToday());
                $("#unit").val('3');
                break;
            case "本周":
                $("#starttime").val(GetWeekFirstDay());
                $("#endtime").val(GetToday());
                $("#unit").val('2');
                break;
            default:
                $("#starttime").val(GetToday());
                $("#endtime").val(GetNow());
                $("#unit").val('1');
        }
        ShowChartQueryReport();
    });


});

function ShowChartQueryReport() {
    NTSBOM.ShowComReportList();
}

