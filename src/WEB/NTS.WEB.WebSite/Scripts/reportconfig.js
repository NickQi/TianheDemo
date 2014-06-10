function fn() { }
nts.web9000.JsLibrary.reportconfig = fn.prototype = {
    addreportconfig: function () {
        var applicationname = $("#applicationname").val();
        var psupportmaps = '';
        $('input:checkbox[name=supportmaps][checked]').each(function () {
            psupportmaps += "," + $(this).val();
        });
        var isexportexcel = $("#isexportexcel").attr('checked') ? "1" : "0";
        var rulecollections = $("#rules").val() == null ? "" : $("#rules").val().toString();

        /*js验证部分*/
        if (!nts.web9000.JsLibrary.validator.binds("", { id: "applicationname", rule: "empty|lenrage", leninfo: "1,50" })) { return; }
        if (!nts.web9000.JsLibrary.validator.binds("", { id: "rules", rule: "empty" })) { return; }

        if (psupportmaps == "") { alert('应用中至少要选择一个统计图形。'); return false; }
        if (rulecollections.indexOf(',') == -1) {
            if (psupportmaps.indexOf('2') > -1 || psupportmaps.indexOf('3') > -1) {
                alert('对不起，您只选一个公式，无法进行对比展示和饼图展示');
                return;
            }
        }
        var datas = {
            applicationname: applicationname, // 应用名称
            supportmaps: psupportmaps, // 支持的图形报表
            isexportexcel: isexportexcel, // 支持excel导出
            groupid: $("#RuleGroupID").val(),
            Interval: $("#Interval").val(),
            rulecollection: rulecollections // 公式集合
        }
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.addreportconfig&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            data: datas,
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                if (data) {
                    if (data.success) {
                        alert('添加报表应用配置信息成功。');
                        window.location = 'reportconfig.aspx';
                    }
                    else {
                        alert(data.msg);
                    }
                }

            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    },
    loadrulelist: function () {
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.getrulelist&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            async: false,
            data: { groupid: $("#RuleGroupID").val(), Interval: $("#Interval").val() },
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                $("#rules").html('');
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        $("#rules").append("<option value='" + data[i].ID + "'>" + data[i].CNAME + "</option>");
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    },
    loadrulelistselect: function (select) {
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.getrulelist&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            async: false,
            data: { groupid: $("#RuleGroupID").val(), Interval: $("#Interval").val() },
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                $("#rules").html('');
                if (data != ']') {
                    eval("data=" + data);
                    for (var i = 0; i < data.length; i++) {
                        $("#rules").append("<option value='" + data[i].ID + "'>" + data[i].CNAME + "</option>");
                    }

                    var id_Ojbect = select.split(","); //分割为Ojbect数组。 
                    var count = $("#rules option").length; //获取下拉框的长度。 
                    for (var c = 0; c < id_Ojbect.length; c++) {
                        for (var c_i = 0; c_i < count; c_i++) {
                            if ($("#rules").get(0).options[c_i].value == id_Ojbect[c]) {
                                //alert('');
                                $("#rules").get(0).options[c_i].selected = true; //设置为选中。 
                            }
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    },
    getsupportmaps: function (str) {
        if (str == '') { return ''; }
        var reportstr = '';
        if (str.indexOf(',1') > -1) {
            reportstr += "统计图";
        }
        if (str.indexOf(',2') > -1) {
            reportstr += "&nbsp;对比图";
        }
        if (str.indexOf(',3') > -1) {
            reportstr += "&nbsp;饼图占比";
        }
        if (str.indexOf(',4') > -1) {
            reportstr += "&nbsp;数据表格";
        }
        return reportstr;
    },
    getreportconfig: function (id) {
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.getreportconfig&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            data: { id: id },
            dataType: 'text',
            async: false,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    eval("datainfo=" + data);
                    $("#applicationname").val(datainfo[0].applicationname);
                    datainfo[0].supportmaps = datainfo[0].supportmaps + ",";
                    $('input:checkbox[name=supportmaps]').attr("checked", false);
                    $('input:checkbox[name=supportmaps]').each(function () {
                        if (datainfo[0].supportmaps.indexOf($(this).val()) > -1) {
                            $(this).attr("checked", true);
                        }
                    });

                    $("#isexportexcel").attr("checked", (datainfo[0].isexportexcel == 1 ? true : false));
                    $("#RuleGroupID").val(datainfo[0].groupid);
                    $("#Interval").val(datainfo[0].interval);
                    nts.web9000.JsLibrary.reportconfig.loadrulelistselect(datainfo[0].rulecollection);
                    //  $("#rules").val(datainfo[0].rulecollection);
                    

                    $("#appid").val(datainfo[0].id);
                }
                return "";
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    },
    getrulecollection: function (collections) {
        return $.ajax({ url: "action.ashx?method=nts.web9000.bll.TB_RULE.GetruleNameList&dll=nts.web9000.bll&times=" + new Date().getTime(), type: 'GET', data: { collections: collections }, async: false, cache: false }).responseText;
    },
    deleteconfigapp: function (id) {
        if (confirm('你确定删除该报表应用吗？')) {
            $.ajax({
                url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.deletereportconfig&dll=nts.web9000.bll&times=" + new Date().getTime(),
                type: 'Post',
                data: { id: id },
                dataType: 'text',
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                timeout: 1000,
                success: function (data) {
                    //alert(data);
                    if (data != ']') {
                        eval("datainfo=" + data);
                        if (datainfo.success) {
                            window.location = 'reportconfig.aspx';
                        } else {
                            alert('出现异常的错误，删除失败');
                            return;
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
    },
    updateconfigapp: function () {
        var applicationname = $("#applicationname").val();
        var psupportmaps = '';
        $('input:checkbox[name=supportmaps][checked]').each(function () {
            psupportmaps += "," + $(this).val();
        });
        var isexportexcel = $("#isexportexcel").attr('checked') ? "1" : "0";
        var rulecollections = $("#rules").val() == null ? "" : $("#rules").val().toString();

        /*js验证部分*/
        if (!nts.web9000.JsLibrary.validator.binds("", { id: "applicationname", rule: "empty|lenrage", leninfo: "1,50" })) { return; }
        if (!nts.web9000.JsLibrary.validator.binds("", { id: "rules", rule: "empty" })) { return; }

        if (psupportmaps == "") { alert('应用中至少要选择一个统计图形。'); return false; }
        if (rulecollections.indexOf(',') == -1) {
            if (psupportmaps.indexOf('2') > -1 || psupportmaps.indexOf('3') > -1) {
                alert('对不起，您只选一个公式，无法进行对比展示和饼图展示');
                return;
            }
        }
        var datas = {
            id: $("#appid").val(),
            applicationname: applicationname, // 应用名称
            supportmaps: psupportmaps, // 支持的图形报表
            isexportexcel: isexportexcel, // 支持excel导出
            groupid: $("#RuleGroupID").val(),
            Interval: $("#Interval").val(),
            rulecollection: rulecollections // 公式集合
        }

        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.updatereportconfig&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            data: datas,
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                if (data) {
                    if (data.success) {
                        alert('修改报表应用配置信息成功。');
                        window.location = 'reportconfig.aspx';
                    }
                    else {
                        alert(data.msg);
                    }
                }

            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    },
    showconfigapplist: function (data) {
        $.ajax({
            url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.getconfiglist&dll=nts.web9000.bll&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            anysc:false,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            //data: { buildid: buildid, current_page: current_page, pagesize: 6, areaid: areaid },
            data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#configlist").html('');
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {
                        htmls += "<li>";
                        htmls += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"line-height: 25px;\">";
                        htmls += "<tr>";

                        htmls += "<td width=\"10%\" valign=\"middle\" align=\"left\"><span class=\"configure_name\">" + data[i].userid + "</span></td>";
                        htmls += "<td width=\"35%\" valign=\"middle\" align=\"left\"><span class=\"configure_name\">" + data[i].applicationname + "</span></td>";
                        htmls += "<td width=\"29%\" valign=\"middle\" align=\"left\"><span class=\"configure_type\">" + nts.web9000.JsLibrary.reportconfig.getsupportmaps(data[i].supportmaps) + "</span></td>";
                       // htmls += "<td width=\"25%\" valign=\"middle\" align=\"left\"><span class=\"configure_formula\">" + nts.web9000.JsLibrary.reportconfig.getrulecollection(data[i].rulecollection) + "</span></td>";
                        htmls += "<td width=\"11%\" valign=\"middle\" align=\"left\"><span class=\"configure_export\">" + (data[i].isexportexcel == '1' ? '支持' : '<font color=red>不支持</font>') + "</span></td>";
                        htmls += "<td width=\"25%\" valign=\"middle\" align=\"left\"><span class=\"configure_operate\"><a href=\"#\"></a> <em config='" + data[i].id + "' class=\"configure_revise\">修改</em>&nbsp;&nbsp;<em config='" + data[i].id + "' class=\"configure_delete\">删除</em></span></td>";
                        htmls += "</tr>";
                        htmls += "</table>";
                        htmls += "</li>";

                    }
                    $("#configlist").append(htmls);

                    $(".configure_revise").click(function () {
                        nts.web9000.JsLibrary.reportconfig.getreportconfig($(this).attr('config'));
                        //tab change
                        $(".tab_box input:last").removeClass("tab_disabled");
                        $(".tab_box input:last").addClass("tab_select");
                        $(".tab_box input:first").removeClass("tab_select");
                        //button value
                        $("#configbtn").css("display", "none");
                        $("#configbtnsave").css("display", "block");
                        
                    });

                    $(".configure_delete").click(function () {
                        nts.web9000.JsLibrary.reportconfig.deleteconfigapp($(this).attr('config'));
                    });

                } else {
                    $("#configlist").html('<font color=red>对不起，暂无数据。</font>');
                }

            }
        });

    }
}

$(function () {
    nts.web9000.JsLibrary.reportconfig.loadrulelist();
    $("#configbtn").click(function () {
        nts.web9000.JsLibrary.reportconfig.addreportconfig();
    });
    $("#configbtnsave").click(function () {
        nts.web9000.JsLibrary.reportconfig.updateconfigapp();
    });
    $("#RuleGroupID,#Interval").change(function () {
        nts.web9000.JsLibrary.reportconfig.loadrulelist();
    });

    

    var data = {
        page: 1,
        pagesize: 15,
        wherestr: '',
        orderby: 'userid'
    }
    var maxpage = $.ajax({ url: "action.ashx?method=nts.web9000.bll.Ts_UserAppConfig.getconfiglistcount&dll=nts.web9000.bll&times=" + new Date().getTime(), type: 'GET', data: { wherestr: data.wherestr, pagesize: data.pagesize }, async: false, cache: false }).responseText;
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
            var newdata = { page: pages, pagesize: 15, wherestr: '', orderby: 'userid' }
            nts.web9000.JsLibrary.reportconfig.showconfigapplist(newdata);
        }
    });

    nts.web9000.JsLibrary.reportconfig.showconfigapplist(data);
});