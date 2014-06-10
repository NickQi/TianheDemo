function systemrole() { }
systemrole.prototype = {
    name: '系统角色js类',
    showrolelist: function () {
        var htmls = "";
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.showjobrolelist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                //eval('data=' + data);
                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    htmls += "<li><input type=radio name='jobrole' value=" + myobj[i].id + ">" + myobj[i].jobname + " <a href='#' onclick='new systemrole().showeditrole(" + myobj[i].id + ") '>编辑</a></li>";
                }
                $("#jobroleTable").html(htmls);
            }
        });
    },
    showrolelistbydepartmentid: function (departmentid) {
        var htmls = "";
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.showjobrolelistBydepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { departmentid: departmentid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#rolelist").html('');
                $(".role_fd").html('');
                if (data != "]") {
                    data = eval('data=' + data);
                    var myobj = eval(data);
                    for (var i = 0; i < myobj.length; i++) {
                        $("#rolelist").append("<li title='" + myobj[i].jobname + "' config='" + myobj[i].id + "'>" + (myobj[i].jobname.length > 6 ? myobj[i].jobname.substring(0, 6) + "..." : myobj[i].jobname) + "</li>");
                        $(".role_fd").append("<span><input type='button' class='button01 left' onclick=\"DivLimit(" + myobj[i].id + ")\"  value='权限'/><input onclick=\"DivRevise2(" + myobj[i].id + ")\" type='button' class='button01 right'  value='编辑'/></span>");
                        // onclick="DivLimit('1')"  onclick="DivRevise2('1')"  htmls += "<li><input type=radio name='jobrole' value=" + myobj[i].id + ">" + myobj[i].jobname + " <a href='#' onclick='new systemrole().showeditrole() '>编辑</a></li>";
                    }
                    $("#rolelist li").each(function () {
                        $(this).click(function () {
                            if (!$(this).hasClass("rl_select")) {
                                $(this).addClass("rl_select").siblings();
                            } else {
                                $(this).addClass("rl_select").removeClass("rl_select");
                            }
                        });
                    });
                }

                // $("#jobroleTable").html(htmls);
            }
        });
    },
    roleselectlist: function (departmentid, objselect) {
        // alert(departmentid);
        /*
        if (departmentid == '请选择...') {
        return;
        }*/
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.showjobrolelistBydepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { departmentid: departmentid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                //eval('data=' + data);

                $("#" + objselect).find('option').remove();
                if (data == "]") {
                    return;
                }
                var myobj = eval(data);

                if (myobj) {
                    for (var i = 0; i < myobj.length; i++) {
                        $("#" + objselect).append("<option value='" + myobj[i].id + "'>" + myobj[i].jobname + "</option>"); //为Select追加一个Option(下拉项)
                        //htmls += "<li><input type=radio name='department' value=" + myobj[i].did + ">" + myobj[i].departmentname + " <a href='#' onclick='new department().showeditdepartment() '>编辑</a></li>";
                    }
                }
                // $("#departmentTable").html(htmls);
            }
        });
    },
    addrole: function () {
        var departmentid = '';
        if ($("#departmentTable span.dp_select").length > 0) {
            //alert($("#departmentTable span.dp_select").attr('config'));
            departmentid = $("#departmentTable span.dp_select").attr('config');
        } else {
            alert('请选择角色所在的部门。');
            return false;
        }
        //.attr('config')
        // return;
        var rolename = $("#rolename").val();
        if (rolename == "") {
            alert('请输入角色的名称。');
            return false;
        }

        if (rolename.length > 10) {
            alert('角色的名称长度不能大于10个字符。');
            return false;
        }
        var isbadsql = MyCommValidate({ rule: "sql", value: rolename });
        if (isbadsql != '') { alert(isbadsql); $("#rolename").focus(); return false; }
        var badhtml = MyCommValidate({ rule: "badhtml", value: rolename });
        if (badhtml != '') { alert(badhtml); $("#rolename").focus(); return false; }

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.addrole&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { departmentid: departmentid, rolename: rolename },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval('data=' + data);
                if (data.success) {
                    // alert('角色添加成功。');
                    //  new systemrole().showrolelist();
                    new systemrole().showrolelistbydepartmentid(departmentid);
                    DivCloseUp2();
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showeditrole: function (id) {

        new department().departselectlist("editdepartment");
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.showeditrole&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { id: id },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval('data=' + data);
                if (data.success) {
                    $("#editdepartment").val(data.department);
                    $("#editrolename").val(data.rolename);
                    $("#id").val(data.id);
                } else {
                    alert(data.msg);
                }
            }
        });
        // $("#editdepartment").val(id);

    },
    saverole: function () {
        var departmentidold = '';
        if ($("#departmentTable span.dp_select").length > 0) {
            departmentidold = $("#departmentTable span.dp_select").attr('config');
        } else {
            alert('请选择角色所在的部门。');
            return false;
        }
        var id = $("#id").val();
        var departmentid = $("#editdepartment").val();
        var rolename = $("#editrolename").val();
        if (rolename == "") {
            alert('请输入角色的名称。');
            return false;
        }

        if (rolename.length > 10) {
            alert('角色的名称长度不能大于10个字符。');
            return false;
        }
        var isbadsql = MyCommValidate({ rule: "sql", value: rolename });
       // alert(rolename);
        //return;
        if (isbadsql != '') { alert(isbadsql); return false; }
        var badhtml = MyCommValidate({ rule: "badhtml", value: rolename });
        if (badhtml != '') { alert(badhtml); return false; }

        $.ajax({
            // url: "action.ashx?method=84F7E30EB11BBB5A0B24A2DA324969DEED3379CB87CFE5A577BECF584577C335BDDAE5DD59A1B7224E48D69CC7875548C99CFAE79576931A&times=" + new Date().getTime(),
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.saverole&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { departmentid: departmentid, rolename: rolename, id: id },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval('data=' + data);
                if (data.success) {
                    new systemrole().showrolelistbydepartmentid(departmentidold);
                    DivCloseUp2();
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    },
    showroledepartmentlist: function () {
        var htmls = "";
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.department.showdepartmentlist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert('');
                //return;
                if (data != ']') {
                    var myobj = eval(data);
                    $("#departmentTable").html("");
                    //$("#departmenteditlist").html("");

                    for (var i = 0; i < myobj.length; i++) {

                        if (i == 0) {
                            $("#departmentTable").append("<span style='cursor:pointer'  config='" + myobj[i].did + "' class=\"dp_select\">" + myobj[i].departmentname + "</span>");
                            new systemrole().showrolelistbydepartmentid(myobj[i].did);
                        } else {
                            $("#departmentTable").append("<span style='cursor:pointer'  config='" + myobj[i].did + "'>" + myobj[i].departmentname + "</span>");
                        }
                    }
                    $("#departmentTable span").each(function () {
                        $(this).click(function () {
                            //$(this).addClass("dp_select");
                            $(this).addClass("dp_select").siblings().removeClass("dp_select");
                            SubSelect($(this).attr('config'));
                            new systemrole().showrolelistbydepartmentid($(this).attr('config'));
                            //alert($(this).attr('config'));
                        });
                    });
                }
            }
        });
    },
    deleterole: function () {
        var did = '';
        var departmentid = '';
        if ($("#departmentTable span.dp_select").length > 0) {
            //alert($("#departmentTable span.dp_select").attr('config'));
            departmentid = $("#departmentTable span.dp_select").attr('config');
        }
        $("#rolelist li").each(function () {
            if ($(this).hasClass("rl_select")) {
                did += "," + $(this).attr('config');
            }
        });
        if (did.length > 0) {
            did = did.substring(1);
        } else {
            alert('请选择要删除的角色');
            return;
        }

        $.ajax({
            // url: "action.ashx?method=84F7E30EB11BBB5A0B24A2DA324969DEED3379CB87CFE5A577BECF584577C335BDDAE5DD59A1B7224E48D69CC7875548C99CFAE79576931A&times=" + new Date().getTime(),
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.deleterole&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: { id: did },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval('data=' + data);
                if (data.success) {
                    // new systemrole().showrolelist();
                    // window.location.href = "list.aspx";
                    new systemrole().showrolelistbydepartmentid(departmentid);
                    DivCloseUp2();
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    },
    showrolepermission: function (roleid) {
        $("#allread").attr("checked", false);
        $("#allwrite").attr("checked", false);
        $("#all").attr("checked", false);
        $("#allread").attr("disabled", false);
        $("#allwrite").attr("disabled", false);
        $("#all").attr("disabled", false);
        $("input[name=read]").each(
                    function () {
                        //alert($(this).val());
                        // alert($.ajax({ url: "action.ashx?method=NTS_BECM.BLL.rolerelation.IsChecked&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + $(this).val() + "&[__DOTNET__]System.Int32=" + roleid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText);
                        $(this).attr("checked", $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.rolerelation.IsChecked&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + $(this).val() + "&[__DOTNET__]System.Int32=" + roleid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText);
                    }
                );
        $("input[name=write]").each(
                    function () {
                        $(this).attr("checked", $.ajax({ url: "action.ashx?method=NTS_BECM.BLL.rolerelation.IsChecked&dll=NTS_BECM.BLL&[__DOTNET__]System.String=" + $(this).val() + "&[__DOTNET__]System.Int32=" + roleid + "&times=" + new Date().getTime(), type: 'GET', async: false, cache: false }).responseText);
                    }
                );
    },
    saverolepermission: function (roleid) {
        var rolep = "";
        var menu = "";
        $("input[name=read]").each(
                    function () {
                        if ($(this)[0].checked) {
                            rolep += "," + $(this).val();
                            menu += "," + $(this).attr("config");
                        }
                    }
                );
        $("input[name=write]").each(
                    function () {
                        if ($(this)[0].checked) {
                            rolep += "," + $(this).val();
                            menu += "," + $(this).attr("config");
                        }
                    }
                );
        // alert(menu);
        // return;
        if (rolep.length == 0) {
            alert('请选择权限');
            return false;
        } else {
            rolep = rolep.substr(1);
            menu = menu.substr(1);
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.rolerelation.saverolepermission&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: { roleid: roleid, rolep: rolep, menu: menu },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                eval('data=' + data);
                if (data.success) {
                    alert('保存权限成功！');
                    DivCloseUp2();
                    //  new systemrole().showrolelist();
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    }
}

$(function () {
    var obj = new systemrole();
    obj.showroledepartmentlist();
    //new systemrole().showrolelist();
    $("#btnadd").click(function () {
        obj.addrole();
    });
    // 全选读取
    $("#allread").click(function (e) {
        $("input[name=read]").each(
                    function () {
                        $(this).attr("checked", $("#allread")[0].checked);
                    }
                );
    }
            );

    // 全选更新
    $("#allwrite").click(function (e) {
        $("input[name=write]").each(
                    function () {
                        $(this).attr("checked", $("#allwrite")[0].checked);
                    }
                );
    }
            );

    // 全选
    $("#all").click(function (e) {
        if ($("#all").attr("checked")) {
            $("#allread").attr("checked", true);
            $("#allwrite").attr("checked", true);
            $("#allread").attr("disabled", "disabled");
            $("#allwrite").attr("disabled", "disabled");
        } else {
            $("#allread").attr("checked", false);
            $("#allwrite").attr("checked", false);
            $("#allread").attr("disabled", "");
            $("#allwrite").attr("disabled", "");
        }
        $("input[name=write]").each(
                    function () {
                        $(this).attr("checked", $("#all")[0].checked);
                    }
                );
        $("input[name=read]").each(
                    function () {
                        $(this).attr("checked", $("#all")[0].checked);
                    }
                );
    }
            );
});