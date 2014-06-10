var pageSize = 10;
var cpage = 0;
/* sets the class of the tr containing the checked checkbox to selected */
function set_tr_class(element, selected) {
    if (selected) {
        element.attr("class", "selected " + element.attr("class"))
    } else {
        var css = element.attr("class");
        var position = css.indexOf('selected');

        element.attr("class", css.substring(position + 9));
    }
}

function GetAlarmList(page, flag) {
    var data = {
        PageCurrent: page,
        PageSize: pageSize
    };
    data = JSON.stringify(data);

    $.ajax({
        url: "action.ashx?action=GetAlarmScaleList",
        dataType: "json",
        type: "POST",
        data: { "Inputs": data },
        async: false,
        success: function (json) {
            if (json.AlarmScaleList != null && json.AlarmScaleList.length > 0) {

                var $tbody = $('#tbl_lst');
                var items = template('alarmLst-tpl', json);
                // 列表
                $tbody.html(items);
            }

            if (flag) {
                var total = json.Page.Total;
                var countNum = Math.ceil(total / pageSize);
                $("#pagination").ntspadding({
                    recordnum: total,
                    count: countNum,
                    onChange: function (data) {
                        GetAlarmList(data.page, false);
                    }
                });
            }
            cpage = $("#pages").val();
        },
        error: function (error) {
            console.log(error);
        }
    });
}


$(document).ready(function () {
    //告警分值配制列表
    GetAlarmList(1, true);

    // 删除
    $('span.dialog-deleteSwitch').live('click', function () {
        if (confirm("是否确认删除数据?")) {
            var alarmScaleId = $(this).attr('alarmScaleId');
            $.ajax({
                url: "action.ashx?action=DeleteAlarmScaleByID",
                dataType: "json",
                type: "POST",
                data: { "Inputs": alarmScaleId },
                async: false,
                success: function (json) {
                    if (json.IsSucess) {
                        GetAlarmList(1, true);
                    }
                }
            });
        }
    });

    // 修改
    $('span.dialog-editSwitch').live('click', function () {
        var alarmScaleId = $(this).attr('alarmScaleId');
        var number = $(this).attr('number');
        var showId = eval("divShow" + number);
        var editId = eval("divEdit" + number);
        var title = $(this).text();
        if (title == "修改") {
            $(showId).hide();
            $(editId).show();
            $(this).text("保存");
        } else if (title == "保存") {
            var alarmType = $(this).attr('alarmType');
            var v = "#iptScale" + number;
            var scaleValue = $(v).val();
            if (!RegNumber(scaleValue)) {
                alert("分值只能是正整数！");
                return false;
            }
            var data = {
                ID: alarmScaleId,
                AlarmType: alarmType,
                Scale: scaleValue
            };
            data = JSON.stringify(data);

            $.ajax({
                url: "action.ashx?action=SaveAlarmScale",
                dataType: "json",
                type: "POST",
                data: { "Inputs": data },
                async: false,
                success: function (json) {
                    if (json.IsSucess) {
                        GetAlarmList(1, true);
                    }
                }
            });

            $(showId).show();
            $(editId).hide();
            $(this).text("修改");
        }
    });
});


// 正整数验证
function RegNumber(str) {
    var s = /^[0-9]+$/;
    return s.test(str);
}