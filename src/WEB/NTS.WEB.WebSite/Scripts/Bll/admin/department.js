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
                //  alert(data);
                //eval('data=' + data);
                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    htmls += "<li><input type=radio name='department' value=" + myobj[i].did + ">" + myobj[i].departmentname + " <a href='#' onclick='new department().showeditdepartment(" + myobj[i].did + ") '>编辑</a></li>";
                }
                $("#departmentTable").html(htmls);
            }
        });
    },
    departselectlist: function (objselect) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.department.showdepartmentlist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'json',
            async: false,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                //  alert(data);
                //eval('data=' + data);
                $("#" + objselect).empty();
                // $("#" + objselect).append("<option value='0'>请选择...</option>");

                var myobj = eval(data);
                for (var i = 0; i < myobj.length; i++) {
                    $("#" + objselect).append("<option value='" + myobj[i].did + "'>" + myobj[i].departmentname + "</option>"); //为Select追加一个Option(下拉项)
                    //htmls += "<li><input type=radio name='department' value=" + myobj[i].did + ">" + myobj[i].departmentname + " <a href='#' onclick='new department().showeditdepartment() '>编辑</a></li>";
                }

                // $("#departmentTable").html(htmls);
            }
        });
        //alert($("#" + objselect).val());
      new department().userroleselectlist($("#" + objselect).val(), "userroleselect");
    },
    adddepartment: function () {
        var departmentname = $("#departmentname").val();
        var parentid = 0;
        var orderid = $("#orderid").val();
        if (departmentname == "") {
            alert('请输入部门的名称。');
            $("#departmentname").focus();
            return false;
        }
        if (orderid == "") {
            alert('请输入部门的排序号。');
            $("#orderid").focus();
            return false;
        }
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
                    new department().departlist();
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
                    $("#editorderid").val(data.orderid);
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
        var orderid = $("#editorderid").val();
        if (departmentname == "") {
            alert('请输入部门的名称。');
            $("#departmentname").focus();
            return false;
        }
        if (orderid == "") {
            alert('请输入部门的排序号。');
            $("#orderid").focus();
            return false;
        }
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
                    // window.location.href = "list.aspx";
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    },
    userroleselectlist: function (departmentid, objselect) {
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
    deletedepartment: function () {
        var did = $("input[name='department']:checked").val();
        if (confirm('您确定要删除吗？')) {
            if (did == undefined) {
                alert('请选择要操作的部门');
                return false;
            }
            $.ajax({
                // url: "action.ashx?method=84F7E30EB11BBB5A0B24A2DA324969DEED3379CB87CFE5A577BECF584577C335BDDAE5DD59A1B7224E48D69CC7875548C99CFAE79576931A&times=" + new Date().getTime(),
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
                        // window.location.href = "list.aspx";
                    } else {
                        alert(data.msg);
                        return;
                    }
                }
            });
        }
    }
}

