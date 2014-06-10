function department() { }
department.prototype = {
    name: '部门相关js类',
    departlist: function () {

        var htmls = "";
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.department.showdepartmentlist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                if (data != ']') {
                    var myobj = eval(data);
                    $("#departmentTable").html("");
                    $("#departmenteditlist").html("");
                    for (var i = 0; i < myobj.length; i++) {

                        $("#departmentTable").append("<li style='cursor:pointer' config='" + myobj[i].did + "'>" + myobj[i].departmentname + "</li>");
                        //htmls += "<li><input type=radio name='department' value=" + myobj[i].did + ">" + myobj[i].departmentname + " <a style='cursor:pointer' onclick='OutEditFun(" + myobj[i].did + ")'>编辑</a></li>";

                        $("#departmenteditlist").append("<span><input type=\"button\" class=\"button01\" onclick=\"DivRevise(" + myobj[i].did + ")\" value=\"编辑\"/></span>");
                    }
                    $("#departmentTable li").each(function () {
                        $(this).click(function () {
                            if (!$(this).hasClass("dpt_sub")) {
                                $(this).addClass("dpt_sub").siblings();
                            } else {
                                $(this).addClass("dpt_sub").removeClass("dpt_sub");
                            }
                        });
                    }
                );
                }
            }
        });
    },
    departselectlist: function (objselect) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.department.showdepartmentlist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'json',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                  alert(data);
                //eval('data=' + data);
                $("#" + objselect).empty();
               // $("#" + objselect).append("<option>请选择...</option>");

                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    $("#" + objselect).append("<option value='" + myobj[i].did + "'>" + myobj[i].departmentname + "</option>"); //为Select追加一个Option(下拉项)
                    //htmls += "<li><input type=radio name='department' value=" + myobj[i].did + ">" + myobj[i].departmentname + " <a href='#' onclick='new department().showeditdepartment() '>编辑</a></li>";
                }

                

                // $("#departmentTable").html(htmls);
            }
        });
    },
    adddepartment: function () {
        var departmentname = $("#departmentname").val();
        var parentid = 0;
        //var orderid = $("#orderid").val();
        var orderid = 0;
        if (departmentname == "") {
            alert('请输入部门的名称。');
            $("#departmentname").focus();
            return false;
        }
        if (departmentname.length > 8) {
            alert('部门的名称不能超过8个字符。');
            $("#departmentname").focus();
            return false;
        }
        var isbadsql = MyCommValidate({ rule: "sql", value: departmentname });
        if (isbadsql != '') { alert(isbadsql); $("#departmentname").focus(); return false; }
        var isbadhtml = MyCommValidate({ rule: "badhtml", value: departmentname });
        if (isbadhtml != '') { alert(isbadhtml); $("#departmentname").focus(); return false; }
        /*
        if (orderid == "") {
        alert('请输入部门的排序号。');
        $("#orderid").focus();
        return false;
        }
        */
        $.ajax({
            // url: "action.ashx?method=84F7E30EB11BBB5A0B24A2DA324969DEED3379CB87CFE5A577BECF584577C335BDDAE5DD59A1B7224E48D69CC7875548C99CFAE79576931A&times=" + new Date().getTime(),
            url: "action.ashx?method=NTS_BECM.BLL.department.adddepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { departmentname: departmentname, parentid: 0, orderid: orderid },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval('data=' + data);
                if (data.success) {
                    $("#departmentname").val('');
                    new department().departlist();
                    DivCloseUp();
                    //window.location.href = "list.aspx";
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    },
    showeditdepartment: function (did) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.department.showeditdepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { did: did },
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                eval('data=' + data);
                if (data.success) {
                    $("#editdepartmentname").val(data.department);
                    // $("#editorderid").val(data.orderid);
                    $("#did").val(did);
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    },
    savedepartment: function () {
        var did = $("#did").val();
        var departmentname = $("#editdepartmentname").val();
        var parentid = 0;
        // var orderid = $("#editorderid").val();
        var orderid = 0;
        if (departmentname == "") {
            alert('请输入部门的名称。');
            $("#editdepartmentname").focus();
            return false;
        }
        if (departmentname.length > 8) {
            alert('部门的名称不能超过8个字符。');
            $("#editdepartmentname").focus();
            return false;
        }
        var isbadsql = MyCommValidate({ rule: "sql", value: departmentname });
        if (isbadsql != '') { alert(isbadsql); $("#editdepartmentname").focus(); return false; }
        var isbadhtml = MyCommValidate({ rule: "badhtml", value: departmentname });
        if (isbadhtml != '') { alert(isbadhtml); $("#editdepartmentname").focus(); return false; }
        /*
        if (orderid == "") {
        alert('请输入部门的排序号。');
        $("#orderid").focus();
        return false;
        }*/
        $.ajax({
            // url: "action.ashx?method=84F7E30EB11BBB5A0B24A2DA324969DEED3379CB87CFE5A577BECF584577C335BDDAE5DD59A1B7224E48D69CC7875548C99CFAE79576931A&times=" + new Date().getTime(),
            url: "action.ashx?method=NTS_BECM.BLL.department.savedepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { departmentname: departmentname, parentid: 0, orderid: orderid, did: did },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval('data=' + data);
                if (data.success) {
                    new department().departlist();
                    DivCloseUp();
                    // window.location.href = "list.aspx";
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    },
    deletedepartment: function () {
        var did = "";
        /*
        var did = $("input[name='department']:checked").val();
        if (confirm('您确定要删除吗？')) {
           
        }
        if (!$(this).hasClass("dpt_sub")) {
        $(this).addClass("dpt_sub").siblings();
        } else {
        $(this).addClass("dpt_sub").removeClass("dpt_sub");
        }
        $("#departmentTable").append("<li config='" + myobj[i].did + "'>" + myobj[i].departmentname + "</li>");
        */
        $("#departmentTable li").each(function () {
            if ($(this).hasClass("dpt_sub")) {
                did += "," + $(this).attr("config");
            }
        });
        // alert(did);
        if (did == "") {
            alert('请选择要操作的部门');
            return false;
        }
        did = did.substring(1);
        // alert(did);
        //return;
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.department.deletedepartment&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            data: { did: did },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //alert(data);
                eval('data=' + data);
                if (data.success) {
                    new department().departlist();
                    DivCloseUp();
                    // window.location.href = "list.aspx";
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    }
}
function OutEditFun(v) {
   //alert(v);
    new department().showeditdepartment(v);
}
$(function () {
    var obj = new department();
    obj.departlist();
    $("#btnadd").click(function () { obj.adddepartment();  });
    $("#btneditd").click(function () { obj.savedepartment();  });
    $("#btndel").click(function () { obj.deletedepartment() });
});