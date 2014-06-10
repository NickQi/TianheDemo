function buildings() { }
buildings.prototype = {
    name: '系统数据中心js类',
    AddBuildGroup: function () {
        var F_BuildGroupID = $("#F_BuildGroupID").val();
        var F_BuildGroupName = $("#F_BuildGroupName").val();
        var F_GroupAliasName = $("#F_GroupAliasName").val();
        var F_GroupDesc = $("#F_GroupDesc").val();
        var F_GroupPic = $("#F_GroupPic").val();
        if (F_BuildGroupID.length != 10) {
            alert('对不起，建筑群的代码只能是10个字符');
            $("#F_BuildGroupID").focus();
            return false;
        }

        if (F_BuildGroupName.length == 0) {
            alert('请输入建筑群的名称。');
            $("#F_BuildGroupName").focus();
            return false;
        }
        if (F_BuildGroupName.length > 48) {
            alert('建筑群的名称不能超过48个字符。');
            $("#F_BuildGroupName").focus();
            return false;
        }

        if (F_GroupAliasName.length > 16) {

            alert('建筑群的别名不能超过16个字符。');
            $("#F_GroupAliasName").focus();
            return false;
        }
        if (F_GroupDesc.length > 800) {
            alert('建筑群的描述不能超过800个字符。');
            $("#F_GroupDesc").focus();
            return false;
        }

        var data = {
            F_BuildGroupID: F_BuildGroupID,
            F_BuildGroupName: F_BuildGroupName,
            F_GroupAliasName: F_GroupAliasName,
            F_GroupDesc: F_GroupDesc,
            F_GroupPic: F_GroupPic
        };
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.AddBuildGroup&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                //alert(data);

                data = eval("data=" + data);
                if (data.success) {
                    alert('添加成功');
                    DivCloseUp6();
                    window.location = "buildings.aspx";
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showbuildings: function (v) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.showbuildings&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { F_BuildGroupID: v },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    $("#EF_BuildGroupID").val(data.F_BuildGroupID);
                    $("#EF_BuildGroupName").val(data.F_BuildGroupName);
                    $("#EF_GroupAliasName").val(data.F_GroupAliasName);
                    $("#EF_GroupDesc").val(data.F_GroupDesc);
                    $("#EF_GroupPic").val(data.F_GroupPic);
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    deletebuildings: function () {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.deletebuildings&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { F_BuildGroupID: $("#bid").val() },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    DivCloseUp6();
                    window.location = "buildings.aspx";
                } else {
                    alert('删除建筑群失败');
                }
            }
        });
    },
    EditBuildGroup: function () {
        var F_BuildGroupID = $("#EF_BuildGroupID").val();
        var F_BuildGroupName = $("#EF_BuildGroupName").val();
        var F_GroupAliasName = $("#EF_GroupAliasName").val();
        var F_GroupDesc = $("#EF_GroupDesc").val();
        var F_GroupPic = $("#EF_GroupPic").val();


        if (F_BuildGroupID.length != 10) {
            alert('对不起，建筑群的代码只能是10个字符');
            $("#F_BuildGroupID").focus();
            return false;
        }

        if (F_BuildGroupName.length == 0) {
            alert('请输入建筑群的名称。');
            $("#F_BuildGroupName").focus();
            return false;
        }
        if (F_BuildGroupName.length > 48) {
            alert('建筑群的名称不能超过48个字符。');
            $("#F_BuildGroupName").focus();
            return false;
        }

        if (F_GroupAliasName.length > 16) {

            alert('建筑群的别名不能超过16个字符。');
            $("#F_GroupAliasName").focus();
            return false;
        }
        if (F_GroupDesc.length > 800) {
            alert('建筑群的描述不能超过800个字符。');
            $("#F_GroupDesc").focus();
            return false;
        }

        var data = {
            F_BuildGroupID: F_BuildGroupID,
            F_BuildGroupName: F_BuildGroupName,
            F_GroupAliasName: F_GroupAliasName,
            F_GroupDesc: F_GroupDesc,
            F_GroupPic: F_GroupPic
        };
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_BD_BuildGroupBaseInfo.EditBuildGroup&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            data: data,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('修改成功');
                    DivCloseUp6();
                    window.location = "buildings.aspx";
                } else {
                    alert(data.msg);
                }
            }
        });
    }
}