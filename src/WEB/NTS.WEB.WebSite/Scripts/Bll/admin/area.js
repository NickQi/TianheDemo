var area = function () { };
area.prototype = {
    name: '建筑区域的js类',
    addarea: function () {
        var BuildID = $("#BuildID").val();
        var BAreaID = $("#BAreaID").val();
        var BAreaName = $("#BAreaName").val();
        var BAreaAcreage = $("#BAreaAcreage").val();
        var BAreaAirAcreage = $("#BAreaAirAcreage").val();
        var BAreaHeatAcreage = $("#BAreaHeatAcreage").val();
        var IsStandard = $("#IsStandard").val();
        var BAreaMens = $("#BAreaMens").val();
        var BAreaDis = $("#BAreaDis").val();
        var AreaPic = $("#AreaPic").val();


        if (BAreaID.length != 10) {
            alert("建筑区域的代码长度只能为10个字符的字母和数字组合。");
            return false;
        }
        if (BAreaID == '10个数字或字母组成') {
            alert("请输入建筑区域的代码。");
            return false;
        }

        if (BAreaName.length == "") {
            alert("建筑区域名称不能为空。");
            return false;
        }
        if (BAreaName.length > 50) {
            alert("建筑区域名称长度不能大于50个字符。");
            return false;
        }
        if (BAreaAcreage.length == 0) {
            alert("请输入建筑区域的总面积。");
            return false;
        }
        if (BAreaAirAcreage.length == 0) {
            alert("请输入建筑区域空调的总面积。");
            return false;
        }
        if (BAreaHeatAcreage.length == 0) {
            alert("请输入建筑区域暖气的总面积。");
            return false;
        }
        if (BAreaMens.length == 0) {
            alert("请输入建筑区域的人数。");
            return false;
        }
        if (BAreaDis.length > 500) {
            alert("建筑区域描述长度不能大于500个字符。");
            return false;
        }
        var isdemail = MyCommValidate({ rule: "isdemail", value: BAreaAcreage });
        if (isdemail != '') { alert("建筑区域总面积格式错误：" + isdemail); return false; }

        var isdemail2 = MyCommValidate({ rule: "isdemail", value: BAreaAirAcreage });
        if (isdemail2 != '') { alert("建筑区域空调面积格式错误：" + isdemail2); return false; }
        var isdemail3 = MyCommValidate({ rule: "isdemail", value: BAreaHeatAcreage });
        if (isdemail3 != '') { alert("建筑区域采暖面积格式错误：" + isdemail3); return false; }

        var isint = MyCommValidate({ rule: "number", value: BAreaMens });
        if (isint != '') { alert("建筑区域的人数格式错误：" + isint); return false; }

        var data = {
            BuildID: BuildID,
            BAreaID: BAreaID,
            BAreaName: BAreaName,
            BAreaAcreage: BAreaAcreage,
            BAreaAirAcreage: BAreaAirAcreage,
            BAreaHeatAcreage: BAreaHeatAcreage,
            IsStandard: IsStandard,
            BAreaMens: BAreaMens,
            BAreaDis: BAreaDis,
            AreaPic: AreaPic
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.addarea&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                data = eval("data=" + data);
                if (data.success) {
                    alert('添加成功');
                    window.location = "area.aspx?areaid=" + new Date().getTime();
                    DivCloseUp6();
                } else {
                    if (data.msg != undefined) {
                        alert(data.msg);
                    }
                }
            }
        });
    },
    showarea: function (areaid) {
        var cid = '';
        var bid = '';
        var bg = '';
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.showarea&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { areaid: areaid },
            timeout: 1000,
            async: false,
            success: function (data) {
                if (data != ']') {
                    data = eval("data=" + data);
                    $("#EBuildID").val(data[0].BuildID);
                    $("#EBAreaID").val(data[0].BAreaID);
                    $("#EBAreaName").val(data[0].BAreaName);
                    $("#EBAreaAcreage").val(data[0].BAreaAcreage);
                    $("#EBAreaAirAcreage").val(data[0].BAreaAirAcreage);
                    $("#EBAreaHeatAcreage").val(data[0].BAreaHeatAcreage);
                    $("#EIsStandard").val(data[0].IsStandard);
                    $("#EBAreaMens").val(data[0].BAreaMens);
                    $("#EBAreaDis").val(data[0].BAreaDis);
                    $("#EAreaPic").val(data[0].BAreaPic);
                    cid = data[0].F_BuildGroupID;
                    bid = data[0].BuildID;
                    bg = data[0].IsStandard;
                }
            }
        });


        $("#editgroup li").each(function () {
            $(this).click(function () {
                ShowBuildings2($(this).attr('config'));
                $("#EBuildIDText").val($(".sub_mid_function#edit li:first").html());
                $("#EBuildID").val($(".sub_mid_function#edit li:first").attr('config'));
                //$("#buildgroupid").val($(this).attr('config'));
                $(".sub_mid_function#edit li").click(function () {
                    var ww = $(this).html();
                    $("#EBuildIDText").val(ww);
                    $("#EBuildID").val($(this).attr('config'));
                });
            });
        });

        // 下拉框显示
        $("#editgroup li").each(function () {
            $(".sub_mid_function#edit li").click(function () {
                var ww = $(this).html();
                $("#EBuildIDText").val(ww);
                $("#EBuildID").val($(this).attr('config'));
                // alert($("#EBuildID").val());
            });
            // alert(bid);
            if (cid != '') {
                // alert($(this).attr('config'));
                if ($(this).attr('config') == cid) {
                    $("#egtxt").val($(this).html());
                    ShowBuildings2($(this).attr('config'));
                    $(".sub_mid_function#edit li").each(function () {
                        if (bid != '') {
                            if ($(this).attr('config') == bid) {
                                var ww = $(this).html();
                                $("#EBuildIDText").val(ww);
                                $("#EBuildID").val($(this).attr('config'));
                            }
                        }
                    });

                }
            }
        });

        var edd13 = $("ul.sub_mid_biaogan#edit li"); // 建筑群
        edd13.each(function () {
            if ($(this).attr('config') == bg) {
                $("#EIsStandardText").val($(this).html());
                $("#EIsStandard").val($(this).attr('config'));
            }
        });
    },
    updatearea: function () {
        var BuildID = $("#EBuildID").val();
        //alert(BuildID);
        //return false;
        var BAreaID = $("#EBAreaID").val();
        var BAreaName = $("#EBAreaName").val();
        var BAreaAcreage = $("#EBAreaAcreage").val();
        var BAreaAirAcreage = $("#EBAreaAirAcreage").val();
        var BAreaHeatAcreage = $("#EBAreaHeatAcreage").val();
        var IsStandard = $("#EIsStandard").val();
        var BAreaMens = $("#EBAreaMens").val();
        var BAreaDis = $("#EBAreaDis").val();
        var AreaPic = $("#EAreaPic").val();

        if (BAreaID.length != 10) {
            alert("建筑区域的代码长度只能为10个字符的字母和数字组合。");
            return false;
        }
        if (BAreaName.length == "") {
            alert("建筑区域名称不能为空。");
            return false;
        }
        if (BAreaName.length > 50) {
            alert("建筑区域名称长度不能大于50个字符。");
            return false;
        }
        if (BAreaAcreage.length == 0) {
            alert("请输入建筑区域的总面积。");
            return false;
        }
        if (BAreaAirAcreage.length == 0) {
            alert("请输入建筑区域空调的总面积。");
            return false;
        }
        if (BAreaHeatAcreage.length == 0) {
            alert("请输入建筑区域暖气的总面积。");
            return false;
        }
        if (BAreaMens.length == 0) {
            alert("请输入建筑区域的人数。");
            return false;
        }
        if (BAreaDis.length > 500) {
            alert("建筑区域描述长度不能大于500个字符。");
            return false;
        }
        var isdemail = MyCommValidate({ rule: "isdemail", value: BAreaAcreage });
        if (isdemail != '') { alert("建筑区域总面积格式错误：" + isdemail); return false; }

        var isdemail2 = MyCommValidate({ rule: "isdemail", value: BAreaAirAcreage });
        if (isdemail2 != '') { alert("建筑区域空调面积格式错误：" + isdemail2); return false; }
        var isdemail3 = MyCommValidate({ rule: "isdemail", value: BAreaHeatAcreage });
        if (isdemail3 != '') { alert("建筑区域采暖面积格式错误：" + isdemail3); return false; }

        var isint = MyCommValidate({ rule: "number", value: BAreaMens });
        if (isint != '') { alert("建筑区域的人数格式错误：" + isint); return false; }

        var data = {
            BuildID: BuildID,
            BAreaID: BAreaID,
            BAreaName: BAreaName,
            BAreaAcreage: BAreaAcreage,
            BAreaAirAcreage: BAreaAirAcreage,
            BAreaHeatAcreage: BAreaHeatAcreage,
            IsStandard: IsStandard,
            BAreaMens: BAreaMens,
            BAreaDis: BAreaDis,
            AreaPic: AreaPic
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.updatearea&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                data = eval("data=" + data);
                if (data.success) {
                    alert('修改成功');
                    window.location = "area.aspx?areaid=" + new Date().getTime();
                    DivCloseUp6();
                } else {
                    if (data.msg != undefined) {
                        alert(data.msg);
                    }
                }
            }
        });
    },
    deletearea: function (areaid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.deletearea&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { areaid: areaid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                data = eval("data=" + data);
                if (data.success) {
                    //alert('添加成功');
                    window.location = "area.aspx?areaid=" + new Date().getTime();
                    DivCloseUp6();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showarealist: function () {
        var firstp = 1;
        var maxpage = $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.BArea.GetPageCount&dll=NTS_BECM.BLL&times=" + new Date().getTime(), contentType: "application/x-www-form-urlencoded; charset=utf-8", type: 'Post', async: false, cache: false }).responseText;
        maxpage = Math.ceil(maxpage / 20);
        //alert(maxpage);
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
                new area().showpaddinglist(pages);
            }
        });
        new area().showpaddinglist(1); // 第一次加载时
        // paddingarealist
    },
    showpaddinglist: function (page) {
        $("#padding-main").html('');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.BArea.GetPaddingList&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { page: page, pagesize: 20 },
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                if (data != ']') {
                    eval("data=" + data);
                    var htmls = '';
                    for (var i = 0; i < data.length; i++) {

                        htmls += "<li>";
                        htmls += "<table width=\"1349\" border=\"0\" cellpadding=\"0\" class=\"menu_tbl\" align='center' cellspacing=\"0\">";

                        htmls += "<tr height=\"30\" valign=\"middle\" class='AC1' align=\"left\">";

                        htmls += "<td width=\"111\" align='center'><span style=\"width:111px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].F_DataCenterName + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].F_BuildGroupName + "</span></td>";
                        htmls += "<td width=\"160\" align='center'><span style=\"width:160px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].F_BuildName + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].BAreaID + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].BAreaName + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].BAreaAcreage + "M<SUP>2</SUP></span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].BAreaAirAcreage + "M<SUP>2</SUP></span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].BAreaHeatAcreage + "M<SUP>2</SUP></span></td>";
                        htmls += "<td width=\"100\" align='center'><span style=\"width:100px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + (data[i].IsStandard == "1" ? "是" : "否") + "</span></td>";
                        htmls += "<td width=\"90\" align='center'><span style=\"width:90px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + data[i].BAreaMens + "人</span></td>";
                        htmls += "<td width=\"200\" align='center'><span style=\"width:200px;height:30px;line-height:30px;display:block;text-align:center;overflow:hidden;\">" + (data[i].BAreaDis.length > 50 ? data[i].BAreaDis.substring(0, 50) + "..." : data[i].BAreaDis) + "</span></td>";
                        htmls += "<td width=\"148\" align='center'><span class=\"btnbg2\"><input type=\"button\" class=\"button04\" onclick=\"DivRevise4('" + data[i].BAreaID + "')\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"Del3('" + data[i].BAreaID + "')\" value=\"删除\"></span></td>";
                        htmls += " </tr>";
                        htmls += " </table>";
                        htmls += " </li>";
                    }
                    // alert(htmls);
                    $("#padding-main").html(htmls);
                } else {
                    $("#padding-main").html("<br/><font color=red>对不起，暂无数据信息。</font>");
                    $('.pagination#classid').html('');
                }
            }
        });
    }
}


$(function () {
    new area().showarealist(1);
    $("#buildgrouplist li").each(function () {
        $(this).click(function () {
            ShowBuildings($(this).attr('config'));
            $("#buildgroupid").val($(this).attr('config'));
            $("#buildlist li").click(function () {
                var ww = $(this).html();
                $("span.xltt3").html(ww);
                $("#buildid").val($(this).attr('config'));
            });
        });
    });

    $("#addgroup li").each(function () {
        $(this).click(function () {
            ShowBuildings1($(this).attr('config'));
            $("#BuildIDText").val($(".sub_mid_function#add li:first").html());
            $("#BuildID").val($(".sub_mid_function#add li:first").attr('config'));
            //$("#buildgroupid").val($(this).attr('config'));
            $(".sub_mid_function#add li").click(function () {
                var ww = $(this).html();
                $("#BuildIDText").val(ww);
                $("#BuildID").val($(this).attr('config'));
            });
        });
    });







    $("#FileUpload1").bind("change", function () {
        //开始提交
        //alert($("#form1"));
        $("#form1").ajaxSubmit({
            beforeSubmit: function (formData, jqForm, options) {
                //隐藏上传按钮
                $(".files #file1").hide();
                //显示LOADING图片
                $(".uploading # uploading1").show();
            },
            //target: '#output2',
            success: function (data, textStatus) {
                if (data.msg == 1) {
                    $("#AreaPic").val(data.msbox);
                } else {
                    alert(data.msbox);
                }
                $(".files #file1").show();
                $(".uploading #uploading1").hide();
                return false;
            },
            error: function (data, status, e) {
                alert("上传失败，错误信息：" + e);
                $(".files #file1").show();
                $(".uploading #uploading1").hide();
            },
            url: "/Tools/SingleUpload.ashx",
            type: "post",
            dataType: "json",
            timeout: 600000
        });
        // $("#form1").ajaxSubmit();
        return false;
    });


    $("#FileUpload2").bind("change", function () {
        //alert('');
        //开始提交
        //alert($("#form1"));
        $("#form2").ajaxSubmit({
            beforeSubmit: function (formData, jqForm, options) {
                //隐藏上传按钮
                $(".files #file2").hide();
                //显示LOADING图片
                $(".uploading #uploading2").show();
            },
            success: function (data, textStatus) {
                if (data.msg == 1) {
                    $("#EAreaPic").val(data.msbox);
                } else {
                    alert(data.msbox);
                }
                $(".files #file2").show();
                $(".uploading #uploading2").hide();
            },
            error: function (data, status, e) {
                alert("上传失败，错误信息：" + e);
                $(".files #file2").show();
                $(".uploading #uploading2").hide();
            },
            url: "/Tools/SingleUpload.ashx",
            type: "post",
            dataType: "json",
            timeout: 600000
        });
    });

});

function ShowBuildings2(buildgroupid) {
    var htmls = "";
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.GetBuildings&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        async: false,
        type: 'Post',
        dataType: 'text',
        data: { buildgroupid: buildgroupid },
        timeout: 1000,
        success: function (data) {
            //alert(data);
            if (data != "]") {
                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    htmls += "<li name='addbuilds' config=" + myobj[i].F_BuildID + ">" + myobj[i].F_BuildName + "</li>";
                }
                $(".sub_mid_function#edit").html(htmls);
            }
            else {
                $(".sub_mid_function#edit").html("");
                $("#EBuildIDText").val('');
                $("#EBuildID").val('');
            }
        }
    });
}

function ShowBuildings1(buildgroupid) {
    var htmls = "";
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.GetBuildings&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        async: false,
        type: 'Post',
        dataType: 'text',
        data: { buildgroupid: buildgroupid },
        timeout: 1000,
        success: function (data) {
            //alert(data);
            if (data != "]") {
                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    htmls += "<li name='addbuilds' config=" + myobj[i].F_BuildID + ">" + myobj[i].F_BuildName + "</li>";
                }
                $(".sub_mid_function#add").html(htmls);
            }
            else {
                $(".sub_mid_function#add").html("");
                $("#BuildIDText").val('');
                $("#BuildID").val('');
            }
        }
    });
}

function ShowBuildings(buildgroupid) {
    var htmls = "";
    $.ajax({
        url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.GetBuildings&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
        async: false,
        type: 'Post',
        dataType: 'text',
        data: { buildgroupid: buildgroupid },
        timeout: 1000,
        success: function (data) {
            //alert(data);
            if (data != "]") {
                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    htmls += "<li name='builds' config=" + myobj[i].F_BuildID + ">" + myobj[i].F_BuildName + "</li>";
                }
                $("#buildlist").html(htmls);
            }
            else {
                $("#buildlist").html("");
                $("span.xltt3").html('');
            }
        }
    });
}