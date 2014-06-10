function datacenter() { }
datacenter.prototype = {
    name: '系统数据中心js类',
    AddDataCenter: function () {
        var F_DataCenterID = $("#F_DataCenterID").val();
        var F_DataCenterName = $("#F_DataCenterName").val();
        var F_DataCenterType = $("#F_DataCenterType").val();
        var DataCenterManager = $("#DataCenterManager").val();
        var F_DataCenterDesc = $("#F_DataCenterDesc").val();
        var F_DataCenterTel = $("#F_DataCenterTel").val();
        var DataCenterContact = $("#DataCenterContact").val();
       // alert('ok');
      //  return;
        if (F_DataCenterID.length != 6) {
            alert('请输入正确的数据中心的代码号，标准代码号为6位，只能由英文或数字组成。');
            return false;
        }
        if (F_DataCenterName == "") {
            alert('请输入数据中心的名称。');
            return false;
        }
        if (F_DataCenterName.length > 10) {
            alert('数据中心的名称长度不能大于10个字符。');
            return false;
        }
        if (DataCenterManager.length > 48) {
            alert('数据中心主管单位的长度不能大于48个字符。');
            return false;
        }

        if (F_DataCenterDesc.length > 800) {
            alert('数据中心主管单位的长度不能大于800个字符。');
            return false;
        }
        if (DataCenterContact.length > 48) {
            alert('数据中心联系人的长度不能大于48个字符。');
            return false;
        }
        if (F_DataCenterTel.length > 0) {

            var isphone = MyCommValidate({ rule: "phone", value: F_DataCenterTel });
            // alert(isphone);
            if (isphone != '') { alert(isphone); return false; }
        }
        var data = {
            F_DataCenterID: F_DataCenterID,
            F_DataCenterName: F_DataCenterName,
            F_DataCenterType: F_DataCenterType,
            DataCenterManager: DataCenterManager,
            F_DataCenterDesc: F_DataCenterDesc,
            F_DataCenterTel: F_DataCenterTel,
            DataCenterContact: DataCenterContact
        };
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_DC_DataCenterBaseInfo.AddDataCenter&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('添加成功');
                    window.location = "datacenter.aspx";
                    DivCloseUp6()
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    showdatacenter: function (centerid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_DC_DataCenterBaseInfo.showdatacenter&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { centerid: centerid },
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    $("#eF_DataCenterID").val(data.F_DataCenterID);
                    $("#eF_DataCenterName").val(data.F_DataCenterName);
                    $("#eF_DataCenterType").val(data.F_DataCenterType);

                    $("#eF_DataCenterManager").val(data.DataCenterManager);
                    $("#eF_DataCenterDesc").val(data.F_DataCenterDesc);
                    $("#eF_DataCenterTel").val(data.F_DataCenterTel);
                    $("#eF_DataCenterContact").val(data.DataCenterContact);
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    EditDataCenter: function () {
        var F_DataCenterID = $("#eF_DataCenterID").val();
        var F_DataCenterName = $("#eF_DataCenterName").val();
        var F_DataCenterType = $("#eF_DataCenterType").val();
        var DataCenterManager = $("#eF_DataCenterManager").val();
        var F_DataCenterDesc = $("#eF_DataCenterDesc").val();
        var F_DataCenterTel = $("#eF_DataCenterTel").val();
        var DataCenterContact = $("#eF_DataCenterContact").val();
        if (F_DataCenterID.length != 6) {
            alert('请输入正确的数据中心的代码号，标准代码号为6位，只能由英文或数字组成。');
            return false;
        }
        if (F_DataCenterName == "") {
            alert('请输入数据中心的名称。');
            return false;
        }
        if (F_DataCenterName.length > 10) {
            alert('数据中心的名称长度不能大于10个字符。');
            return false;
        }
        if (DataCenterManager.length > 48) {
            alert('数据中心主管单位的长度不能大于48个字符。');
            return false;
        }

        if (F_DataCenterDesc.length > 800) {
            alert('数据中心主管单位的长度不能大于800个字符。');
            return false;
        }
        if (DataCenterContact.length > 48) {
            alert('数据中心联系人的长度不能大于48个字符。');
            return false;
        }
        if (F_DataCenterTel.length > 0) {

            var isphone = MyCommValidate({ rule: "phone", value: F_DataCenterTel });
            //alert(isphone);
            if (isphone != '') { alert(isphone); return false; }
        }
        var data = {
            F_DataCenterID: F_DataCenterID,
            F_DataCenterName: F_DataCenterName,
            F_DataCenterType: F_DataCenterType,
            DataCenterManager: DataCenterManager,
            F_DataCenterDesc: F_DataCenterDesc,
            F_DataCenterTel: F_DataCenterTel,
            DataCenterContact: DataCenterContact
        };
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_DC_DataCenterBaseInfo.EditDataCenter&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: data,
            timeout: 1000,
            success: function (data) {
                // alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    alert('修改成功');
                    DivCloseUp6();
                    window.location = "datacenter.aspx";
                } else {
                    alert(data.msg);
                }
            }
        });
    },
    deletecenter: function (cid) {
        $.ajax({
            url: "action.ashx?method=NTS_BECM.BLL.T_DC_DataCenterBaseInfo.deletecenter&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
            type: 'Post',
            dataType: 'text',
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { cid: cid },
            timeout: 1000,
            success: function (data) {
                //alert(data);
                data = eval("data=" + data);
                if (data.success) {
                    DivCloseUp6();
                    window.location = "datacenter.aspx";
                } else {
                    alert(data.msg);
                    return;
                }
            }
        });
    }
}

