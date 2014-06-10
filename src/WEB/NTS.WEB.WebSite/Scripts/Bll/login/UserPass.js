function LabManager_UserPass() { }
LabManager_UserPass.prototype = {
name:'用户密码js类',
SavePwd:function() {
        if ($("#txtNewPass").val() == "" || $("#txtRePass").val() == "") {
            msgShow("系统提示", "新密码和确认密码必须填写", "warning");
            return false;
        }
        if ($("#txtNewPass").val() != $("#txtRePass").val()) {
            msgShow("系统提示", "两次密码不一致", "warning");
            return false;
        }
        $.messager.confirm('系统提示', '确定要重置密码吗?', function(r) {
            if (r) {
                $.ajax({
                    url: "action.ashx?method=LabManager.BLL.TB_PERSON.EditUserPass&dll=LabManager.BLL&times=" + new Date().getTime(),
                    type: 'Post',
                    data: { password: $("#txtNewPass").val() },
                    timeout: 1000,
                    success: function(data) {
                        eval('data=' + data);
                        if (data.success) {
                            msgShow("系统提示", "密码重置成功");
                        } else {
                            msgShow("系统提示", data.msg, "error");
                            return;
                        }
                    }
                });
            }
        });
    }
}

$(function () {

    var obj = new LabManager_UserPass();
   // var roleid = GetQueryString("RoleId");
    //var userid = GetQueryString("UserId");
    /*
    $('#usersel').combobox({
    url: "Service/PersonService.ashx?Method=GetPersonJson",
    valueField: 'ID',
    textField: 'Name',
    required: true,
    editable: false
    });

    $('#usersel').combobox("setValue", userid);
    */
    $("#btnEp").click(function () {
        obj.SavePwd();
    });

});