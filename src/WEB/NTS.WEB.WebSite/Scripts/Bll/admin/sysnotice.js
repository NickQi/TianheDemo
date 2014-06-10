function sysnotice() { }
sysnotice.prototype = {
    name: '系统公告js处理类',
    addnotice: function () {
        var adddate = $("#adddate").val();
        var title = $("#title").val();
        var content = $("#content").val();
        var hits = 0;
        var BuildID = '';
        var data = {
            BuildID: BuildID,
            Title: title,
            Content: content,
            Hits: hits,
            UpdateTime: adddate
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.addnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                if (data.success) {
                    alert('系统公告添加成功。');
                    new sysnotice().shownoticepaddinglist();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
    },
    deletenotice: function () {
        var id = $("#nid").val();
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.deletenotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: id },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                if (data.success) {
                    //alert('系统公告修改成功。');
                    new sysnotice().shownoticepaddinglist();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
        DivClosedel();
    },
    deletegzxx: function () {
        var id = $("#bnid").val();
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.deletenotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { id: id },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                if (data.success) {
                    //alert('系统公告修改成功。');
                    new sysnotice().showjzgzlist();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
        RfClosedel();
    },
    editgzxx: function () {
        var id = $("#bnid").val();
        var adddate = $("#egstartdate").val();
        var enddate = $("#egenddate").val();
        var title = $("#eguser").val();
        var content = $("#egcontent").val();
        var hits = 1;
        var BuildID = $("#egbuildid").val();
        //alert(BuildID);
        var data = {
            id: id,
            BuildID: BuildID,
            Title: title,
            Content: content,
            Hits: hits,
            UpdateTime: adddate,
            EndTime: enddate
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.editnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                if (data.success) {
                    alert('改造信息修改成功。');
                    new sysnotice().showjzgzlist();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
    },
    editnotice: function () {
        var id = $("#nid").val();
        var adddate = $("#editdate").val();
        var title = $("#etitle").val();
        var content = $("#econtent").val();
        var hits = 0;
        var BuildID = '';
        var data = {
            id: id,
            BuildID: BuildID,
            Title: title,
            Content: content,
            Hits: hits,
            UpdateTime: adddate
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.editnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                if (data.success) {
                    alert('系统公告修改成功。');
                    new sysnotice().shownoticepaddinglist();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
    },
    showeditgzxx: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.showeditnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { nid: v },
            timeout: 1000,
            success: function (data) {
                if (data != ']') {
                    eval("data=" + data);
                    $("#bnid").val(v);
                    $("#egstartdate").val(data[0].UpdateTime.replace("0:00:00", ""));
                    $("#egenddate").val(data[0].EndTime.replace("0:00:00", ""));
                    $("#eguser").val(data[0].Title);
                    $("#egcontent").val(data[0].Content);
                    $("#egbuildid").val(data[0].BuildID);
                    $("ul.sub_mid_bdbuildno#edit li").click(function () {
                        var wt = $(this).html();
                        $(".dd_bdbuildno#edit").val(wt);
                        $("#egbuildid").val($(this).attr('config'));
                        // alert($("#egbuildid").val());
                    });

                    $("ul.sub_mid_bdbuildno#edit li").each(function () {
                        // alert($(this).attr('config'));
                        if ($(this).attr('config') == data[0].BuildID) {
                            $(".dd_bdbuildno#edit").val($(this).html());
                        }
                    });
                } else {
                    alert('该改造信息可能已经不存在。');
                }
            }
        });
    },
    showeditnotice: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.showeditnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { nid: v },
            timeout: 1000,
            success: function (data) {
                if (data != ']') {
                    eval("data=" + data);
                    $("#nid").val(v);
                    $("#editdate").val(data[0].UpdateTime.replace("0:00:00", ""));
                    $("#etitle").val(data[0].Title);
                    $("#econtent").val(data[0].Content);
                } else {
                    alert('该公告信息可能已经不存在。');
                }
            }
        });
    },
    showpaddinglist: function (page) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.showpaddingnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: {
                pages: page,
                pagesize: 20,
                classid: 0
            },
            timeout: 1000,
            success: function (data) {
                $("#sysnlist").html('');
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += "<li style='width:auto'>";
                        html += "<table width=\"876\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"menu_tbl\">";
                        html += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        html += "<td width=\"142\"><span style=\"width:142px;height:30px;line-height:30px;display:block;overflow:hidden;\"><span class=\"imgclick\"></span>" + data[i].UpdateTime.replace("0:00:00", "") + "</span></td>";
                        html += "<td width=\"182\"><span style=\"width:182px;height:30px;line-height:30px;display:block;overflow:hidden;\">" + data[i].Title + "</span></td>";
                        html += "<td width=\"408\"><span style=\"width:408px;height:30px;line-height:30px;display:block;overflow:hidden;\">" + data[i].Content + "</span></td>";
                        html += "<td width=\"108\" align=\"center\"><span class=\"btnbg\"><input type=\"button\" class=\"button04\" onclick=\"DivRev(" + data[i].id + ")\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"DivDel(" + data[i].id + ")\" value=\"删除\"></span></td>";
                        html += "<td width=\"36\"><span></span></td>";
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
    shownoticepaddinglist: function () {
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: { classid: 0 }, async: false, cache: false }).responseText;
        maxpage = Math.ceil(maxpage / 10);
        //alert(maxpage);
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
                new sysnotice().showpaddinglist(pages);
            }
        });
        new sysnotice().showpaddinglist(1);

    },
    showjzgzpaddinglist: function (page) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.showpaddingnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: {
                pages: page,
                pagesize: 20,
                classid: 1
            },
            timeout: 1000,
            success: function (data) {
                $("#gzinfolist").html('');
                //alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        var html = '';
                        html += "<li style='width:auto'>";
                        html += "<table width=\"876\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"menu_tbl\">";
                        html += "<tr height=\"30\" valign=\"middle\" align=\"left\">";
                        html += "<td width=\"110\"><span style=\"width:110px;height:30px;line-height:30px;display:block;overflow:hidden;\"><span class=\"imgclick\"></span>" + data[i].UpdateTime.replace('0:00:00', '') + "</span></td>";
                        html += "<td width=\"110\"><span style=\"width:110px;height:30px;line-height:30px;display:block;overflow:hidden;\">" + data[i].EndTime.replace('0:00:00', '') + "</span></td>";
                        html += "<td width=\"160\"><span style=\"width:160px;height:30px;line-height:30px;display:block;overflow:hidden;\">" + GetBuildName(data[i].BuildID) + "</span></td>";
                        html += "<td width=\"110\"><span style=\"width:110px;height:30px;line-height:30px;display:block;overflow:hidden;\">" + data[i].Title + "</span></td>";
                        html += "<td width=\"262\"><span style=\"width:262px;height:30px;line-height:30px;display:block;overflow:hidden;\">" + data[i].Content + "</span></td>";
                        html += "<td width=\"108\" align=\"center\"><span class=\"btnbg\"><input type=\"button\" class=\"button04\" onclick=\"RfRevisebtn(" + data[i].id + ")\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"RfDelbtn(" + data[i].id + ")\" value=\"删除\"></span></td>";
                        html += "<td width=\"16\"><span></span></td>";
                        html += "</tr>";
                        html += "</table>";
                        html += "</li>";
                        $("#gzinfolist").append(html);
                    }
                } else {
                    $("#gzinfolist").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#gzplist').html('');
                }
            }
        });
    },
    showjzgzlist: function () {
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', data: { classid: 1 }, async: false, cache: false }).responseText;
        maxpage = Math.ceil(maxpage / 10);
        //alert(maxpage);
        $("#cp1").val(maxpage);
        $('.pagination#gzplist').html('');
        $('.pagination#gzplist').html('<a href="#" class="first" data-action="first">&laquo;</a><a href="#" class="previous" data-action="previous">&lsaquo;</a><input type="text" readonly="readonly" data-max-page="40" /><a href="#" class="next" data-action="next">&rsaquo;</a><a href="#" class="last" data-action="last">&raquo;</a>');
        $('.pagination#gzplist').jqPagination({
            link_string: '/?page={page_number}',
            current_page: 1, //设置当前页 默认为1
            max_page: maxpage, //设置最大页 默认为1
            page_string: '当前第{current_page}页,共{max_page}页',
            paged: function (pages) {
                if (pages > $("#cp1").val()) { return; }
                new sysnotice().showjzgzpaddinglist(pages);
            }
        });
        new sysnotice().showjzgzpaddinglist(1);

    },
    addgzxx: function () {
        var adddate = $("#gstartdate").val();
        var enddate = $("#genddate").val();
        var title = $("#guser").val();
        var content = $("#gcontent").val();
        var hits = 1;
        var BuildID = $("#gbuildid").val();
        var data = {
            BuildID: BuildID,
            Title: title,
            Content: content,
            Hits: hits,
            UpdateTime: adddate,
            EndTime: enddate
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.SystemNotice.addnotice&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval("data=" + data);
                if (data.success) {
                    alert('建筑改造信息添加成功。');
                    new sysnotice().showjzgzpaddinglist();
                } else {
                    alert(data.msg);
                    return false;
                }
            }
        });
    }
}
function GetBuildName(buildid) {
    return $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildBaseInfo.GetBuildName&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + buildid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText;
}
$(function () {
    new sysnotice().shownoticepaddinglist();
    new sysnotice().showjzgzlist();
});
