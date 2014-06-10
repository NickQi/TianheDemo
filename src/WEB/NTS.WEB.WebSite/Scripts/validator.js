function fn_v() { }
nts.web9000.JsLibrary.validator = fn_v.prototype = {
    name: '验证类',
    dovalid: function (data) {
        var arr = data.rule.split('|');
        var flag = true;
        var msg = '';
        var obj = "#" + data.id;
        $.each(arr, function (n, r) {
            var msgbox = $(obj).attr('dis');
            switch (r) {
                case "empty":
                    var str = $(obj).val() == null ? "" : $(obj).val();
                    if (str.length < 1) {
                        msg += msgbox + "不能为空！";
                        flag = false;
                    }
                    break;
                case "lenrage":
                    var arrrule = data.leninfo.replace("{", "").replace("}", "").split(',');
                    if ($(obj).val().length < arrrule[0]) {
                        msg += "\n" + msgbox + "字符长度不能小于" + arrrule[0] + "个字符！";
                        flag = false;
                    }
                    if ($(obj).val().length > arrrule[1]) {
                        msg += "\n" + msgbox + "字符长度不能大于" + arrrule[1] + "个字符！";
                        flag = false;
                    }
                    break;
                case "email":
                    if ($(obj).val().length != 0) {
                        reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
                        if (!reg.test($(obj).val())) {
                            msg += "\n" + msgbox + "的email格式不正确！";
                            flag = false;
                        }
                    }
                    break;

                default:
                    alert('....');
                    break;
            }
        });
        if (!flag) { alert(msg); $(obj).focus(); }
        return flag;
    },
    binds: function (eventArr, data) {
        var obj = "#" + data.id;
        var flag;
        if (eventArr != '') {
            $(obj).bind(eventArr, function () {
                flag = nts.web9000.JsLibrary.validator.dovalid(data);
            });
        } else {
            flag = nts.web9000.JsLibrary.validator.dovalid(data);
        }
        return flag;
    }
}