function systemaccount() { }
systemaccount.prototype = {
    name: '系统用户js类',
    addaccount: function () {
        var userid = $("#userid").val();
        var userpass = $("#userpass").val();
        //  var repass = $("#repass").val();
        var realname = $("#realname").val();
        var departmentid = $("#departmentuser").val();
        var roleid = $("#userroleselect").val();
        //var portrait = $("#portrait").val();
        var portrait = 'images/jianzhu/member_photo.jpg';
        var repass = $("#crepass").val();
        //alert($("#crepass").val())
        if (userid == "") {
            alert('请输入用户帐号');
            $("#userid").focus();
            return false;
        }
        if (userpass == "") {
            alert('请输入密码');
            $("#userpass").focus();
            return false;
        }
        if (userpass != repass) {
            //alert(userpass);
            // alert(repass);
            alert('两次输入的密码不一致，请确认');
            $("#repass").focus();
            return false;
        }
        if (departmentid == "0") {
            alert('请选择所在的部门');
            $("#departmentuser").focus();
            return false;
        }
        if (roleid == "0") {
            alert('请选择拥有的角色');
            $("#userroleselect").focus();
            return false;
        }
        if (realname == "") {
            alert('请填写你的大名');
            $("#realname").focus();
            return false;
        }
        if (userid.length > 50) { alert('用户名长度不能超过50个字符。'); return false; }
        if (userpass.length > 50) { alert('密码长度不能超过50个字符。'); return false; }

        //alert(departmentid);
        //return false;
        var mydata = {
            userid: userid,
            userpass: userpass,
            realname: realname,
            departmentid: departmentid,
            roleid: roleid,
            portrait: portrait
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.addaccount&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: mydata,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                eval('data=' + data);
                if (data.success) {
                    alert("用户添加成功。");
                    $("#departmentTable span").each(function () {
                        if ($(this).attr('config') == mydata.departmentid) {
                            $(this).addClass("dp_select").siblings().removeClass("dp_select");
                            SubSelect($(this).attr('config'));
                            new systemaccount().usershowrolelistbydepartmentid($(this).attr('config'));
                            new systemaccount().getuserlistbydepartmentid($(this).attr('config'));
                        }
                    });
                    DivCloseUp3();
                    return false;
                }
                else {
                    alert(data.msg);
                }
            }
        });
    },
    deleteaccount: function () {
        var userid = '';
        var departmentid = '';
        if ($("#departmentTable span.dp_select").length > 0) {
            //alert($("#departmentTable span.dp_select").attr('config'));
            departmentid = $("#departmentTable span.dp_select").attr('config');
        }
        $("#puserlist li").each(function () {
            if ($(this).hasClass("yh_select")) {
                userid += "," + $(this).attr('config');
            }
        });
        if (userid.length > 0) {
            userid = userid.substring(1);
        } else {
            alert('请选择要删除的用户');
            return;
        }

        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.deleteaccount&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { userid: userid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                eval('data=' + data);
                if (data.success) {
                    // alert("用户删除成功。");
                    new systemaccount().getuserlistbydepartmentid(departmentid);
                    DivCloseUp3();
                    return false;
                }
                else {
                    alert(data.msg);
                }
            }
        });
    },
    editaccount: function () {
        var userid = $("#edituserid").val();
        var userpass = $("#edituserpass").val();
        var repass = $("#editrepass").val();
        var realname = $("#editrealname").val();
        var departmentid = $("#editdepartmentuser").val();
        var roleid = $("#edituserroleselect").val();
        //var portrait = $("#portrait").val();
        var portrait = 'images/jianzhu/member_photo.jpg';
        if (userid == "") {
            alert('请输入用户帐号');
            $("#userid").focus();
            return false;
        }

        if (userpass != repass) {
            alert('两次输入的密码不一致，请确认');
            $("#repass").focus();
            return false;
        }
        if (departmentid == "0") {
            alert('请选择所在的部门');
            $("#departmentuser").focus();
            return false;
        }
        if (roleid == "0") {
            alert('请选择拥有的角色');
            $("#userroleselect").focus();
            return false;
        }
        if (realname == "") {
            alert('请填写你的大名');
            $("#realname").focus();
            return false;
        }
        var mydata = {
            userid: userid,
            userpass: userpass,
            realname: realname,
            departmentid: departmentid,
            roleid: roleid,
            portrait: portrait
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.editaccount&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: mydata,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                eval('data=' + data);
                if (data.success) {
                    alert("用户修改成功。");
                    DivCloseUp3();
                    return false;
                }
                else {
                    alert(data.msg);
                }
            }
        });
    },
    showeditaccount: function (userid) {
        // alert('友情提醒：密码留空表示不修改。');
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.showaccount&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { userid: userid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);

                if (data != ']') {
                    // 赋值操作
                    // alert(data);
                    data = eval('data=' + data);
                    //  alert(data[0].userid);
                    $("#edituserid").val(data[0].userid);
                    $("#edituserpass").val('');
                    $("#editrepass").val('');
                    $("#editrealname").val(data[0].name);
                    $("#editdepartmentuser").val(data[0].departmentid);
                    new systemaccount().editroleselectlist($("#editdepartmentuser").val(), "edituserroleselect", data[0].jobrole);
                    //$("#edituserroleselect").val();
                }
                else {
                    alert(data.msg);
                }
            }
        });
    },
    Getuseraccount: function (roleid) {
        // alert(roleid);
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.getsystemaccount&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { roleid: roleid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#puserlist").html('');
                $(".user_fd").html('');
                if (data != "]") {
                    data = eval('data=' + data);
                    var myobj = eval(data);
                    for (var i = 0; i < myobj.length; i++) {
                        $("#puserlist").append("<li config='" + myobj[i].userid + "'>" + myobj[i].name + "</li>");
                        $(".user_fd").append("<span><input type='button' class='button01' onclick=\"DivRevise3('" + myobj[i].userid + "')\"  value='编辑'/></span>");
                        // onclick="DivLimit('1')"  onclick="DivRevise2('1')"  htmls += "<li><input type=radio name='jobrole' value=" + myobj[i].id + ">" + myobj[i].jobname + " <a href='#' onclick='new systemrole().showeditrole() '>编辑</a></li>";
                    }
                    $("#puserlist li").each(function () {
                        $(this).click(function () {
                            if (!$(this).hasClass("yh_select")) {
                                $(this).addClass("yh_select").siblings();
                            } else {
                                $(this).addClass("yh_select").removeClass("yh_select");
                            }
                        });
                    });
                }
            }
        });
    },
    usershowroledepartmentlist: function () {
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
                            new systemaccount().usershowrolelistbydepartmentid(myobj[i].did);
                        } else {
                            $("#departmentTable").append("<span style='cursor:pointer'  config='" + myobj[i].did + "'>" + myobj[i].departmentname + "</span>");
                        }
                    }
                    $("#departmentTable span").each(function () {
                        $(this).click(function () {
                            //$(this).addClass("dp_select");
                            $(this).addClass("dp_select").siblings().removeClass("dp_select");
                            SubSelect($(this).attr('config'));
                            new systemaccount().usershowrolelistbydepartmentid($(this).attr('config'));
                            new systemaccount().getuserlistbydepartmentid($(this).attr('config'));
                            //alert($(this).attr('config'));
                        });
                    });
                }
            }
        });
    },
    usershowrolelistbydepartmentid: function (departmentid) {
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
                // $(".role_fd").html('');
                if (data != "]") {
                    data = eval('data=' + data);
                    var myobj = eval(data);
                    for (var i = 0; i < myobj.length; i++) {
                        $("#rolelist").append("<li style='cursor:pointer' config='" + myobj[i].id + "'>" + myobj[i].jobname + "</li>");
                        //  $(".role_fd").append("<span><input type='button' class='button01 left' onclick=\"DivLimit(" + myobj[i].id + ")\"  value='权限'/><input onclick=\"DivRevise2(" + myobj[i].id + ")\" type='button' class='button01 right'  value='编辑'/></span>");
                        // onclick="DivLimit('1')"  onclick="DivRevise2('1')"  htmls += "<li><input type=radio name='jobrole' value=" + myobj[i].id + ">" + myobj[i].jobname + " <a href='#' onclick='new systemrole().showeditrole() '>编辑</a></li>";
                    }
                    $("#rolelist li").each(function () {
                        $(this).click(function () {
                            $(this).addClass("juese").siblings().removeClass("juese");
                            // 显示角色下的用户
                            new systemaccount().Getuseraccount($(this).attr('config'));
                            // $(this).addClass("dp_select").siblings().removeClass("dp_select");
                        });
                    });
                }
                new systemaccount().getuserlistbydepartmentid(departmentid);
                // $("#jobroleTable").html(htmls);
            }
        });
    },
    roleselectlist: function (departmentid, objselect) {
        // alert(departmentid);
        if (departmentid == '0') {
            return;
        }
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.jobrole.showjobrolelistBydepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { departmentid: departmentid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            async: false,
            timeout: 1000,
            success: function (data) {
                //alert(data);
                //eval('data=' + data);

                $("#" + objselect).find('option').remove();
                //$("#" + objselect).append("<option value='0'>请选择...</option>");
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
    editroleselectlist: function (departmentid, objselect, v) {
        // alert(departmentid);
        if (departmentid == '0') {
            return;
        }
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
                $("#" + objselect).append("<option value='0'>请选择...</option>");
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
                $("#edituserroleselect").val(v);
                // $("#departmentTable").html(htmls);
            }
        });
    },
    getuserlistbydepartmentid: function (departmentid) {
        var htmls = "";
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.account.getuserlistbydepartmentid&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: { departmentid: departmentid },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                $("#puserlist").html('');
                $(".user_fd").html('');
                if (data != "]") {
                    data = eval('data=' + data);
                    var myobj = eval(data);
                    for (var i = 0; i < myobj.length; i++) {
                        $("#puserlist").append("<li config='" + myobj[i].userid + "'>" + myobj[i].name + "</li>");
                        $(".user_fd").append("<span><input type='button' class='button01' onclick=\"DivRevise3('" + myobj[i].userid + "')\"  value='编辑'/></span>");
                        // onclick="DivLimit('1')"  onclick="DivRevise2('1')"  htmls += "<li><input type=radio name='jobrole' value=" + myobj[i].id + ">" + myobj[i].jobname + " <a href='#' onclick='new systemrole().showeditrole() '>编辑</a></li>";
                    }
                    $("#puserlist li").each(function () {
                        $(this).click(function () {
                            if (!$(this).hasClass("yh_select")) {
                                $(this).addClass("yh_select").siblings();
                            } else {
                                $(this).addClass("yh_select").removeClass("yh_select");
                            }
                        });
                    });
                }

                // $("#jobroleTable").html(htmls);
            }
        });
    }
}

$(function () {
    var obj = new systemaccount();
    obj.usershowroledepartmentlist();
    new department().departselectlist("departmentuser");
    $("#departmentuser").change(function () {
    obj.roleselectlist($("#departmentuser").val(), "userroleselect");
});
new department().departselectlist("editdepartmentuser");
$("#editdepartmentuser").change(function () {
    obj.roleselectlist($("#editdepartmentuser").val(), "edituserroleselect");
});

});