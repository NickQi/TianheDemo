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
        url: "action.ashx?action=GetAlarmTypeList",
        dataType: "json",
        type: "POST",
        data: { "Inputs": data },
        async: false,
        success: function (json) {
            if (json.AlarmTypeList != null && json.AlarmTypeList.length > 0) {

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
    //告警配制列表
    GetAlarmList(1, true);

    var data = {
        PageCurrent: 1,
        PageSize: 10000
    };

    data = JSON.stringify(data);

    //用户列表
    $.ajax({
        url: "action.ashx?action=GetUserGroupList",
        dataType: "json",
        type: 'POST',
        data: { 'Inputs': data },
        async: false,
        success: function (json) {
            if (json.ResultInfo.Success) {
                if (json.UserGroupList && json.UserGroupList.length > 0) {
                    var $tbody = $('#tbl_usergroup');
                    var items = template('userLst-tpl', json);
                    // 列表
                    $tbody.html(items);
                }
            }
        },
        error: function (error) {
            console.log(error);
        }
    });

    // 发送短信事件，全选按钮
    $("table input[class=checkall]").live("click", function (event) {
        var checked = $(this).attr("checked");

        $("table input[type=checkbox]").each(function () {
            if (this.checked == checked)
                return;
            this.checked = checked;

            if (checked) {
                set_tr_class($(this).parent().parent(), true);
            } else {
                set_tr_class($(this).parent().parent(), false);
            }
        });
    });

    /* sets the class of the table tr when a checkbox within the table is checked */
    $("table input[type=checkbox]").live("click", function (event) {
        if ($(this).attr("checked")) {
            set_tr_class($(this).parent().parent(), true);
        } else {
            set_tr_class($(this).parent().parent(), false);
        }
    });

    $("#dialog-form-play").dialog({
        autoOpen: false,
        resizable: false,
        height: 240,
        width: 350,
        modal: true,
        buttons: {
            ' 提 交 ': function () {

                var aeid = $(this).attr('alarmEventId');
                // 播放音频 (事件、执行方式、音频文件)
                var eventValue = 0;
                // 触发事件
                $(this).find('input[name=checkboxex]').each(function (i, n) {
                    var id = $(n).attr('id');
                    id = id[id.length - 1];
                    if (n.checked == true) {
                        eventValue += id * 1;
                    }
                });
                // 执行方式
                var runMode = 0;
                var runCount = 0;
                var runTime = 0;
                var selTrig = $(this).find('select[name=trigMode]')[0];
                var modeID = $(selTrig).val();
                var arr;
                if (modeID != null && modeID.length > 0) {
                    arr = modeID.split('_');
                    runMode = arr[0];
                }
                if (arr != null) {
                    if (arr.length == 2) {
                        if (arr[0] == 1) {
                            runCount = arr[1];
                        } else if (arr[0] == 2) {
                            runTime = arr[1];
                        }
                    }
                }
                // 音频文件
                var selOptions = $(this).find('select[name=options]')[0];
                var optionsID = $(selOptions).val();
                var optionsValue = $("select[name=options] option[value='" + optionsID + "']").text();

                // TTS语音（事件、执行方式）

                // 发送短信(事件、用户组)
                var userName = "";
                $(this).find('input[name=checkboxUser]').each(function (i, n) {
                    if (n.checked == true) {
                        var userId = $(n).attr('userId');
                        userName += "用户" + userId + ",";
                    }
                });
                // 推视频、推画面 （事件）

                var config = $("#hifConfig").val();
                var data = "";
                switch (config) {
                    case "1":   //播放音频
                        data = { "ID": aeid, "TrigMode": eventValue, "RunMode": runMode, "RunCount": runCount, "RunTime": runTime, "Options": optionsValue };
                        break;
                    case "2":   //推视频
                    case "3":   //推画面
                        data = { "ID": aeid, "TrigMode": eventValue };
                        break;
                    case "4":   //TTS语音配制
                        data = { "ID": aeid, "TrigMode": eventValue, "RunMode": runMode, "RunCount": runCount, "RunTime": runTime };
                        break;
                    case "5":   //发送短信
                        data = { "ID": aeid, "TrigMode": eventValue, "Options": userName };
                        break;
                }

                data = JSON.stringify(data);
                $.ajax({
                    url: "action.ashx?action=UpdateAlarmEventByID",
                    dataType: "json",
                    type: 'POST',
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    data: { 'Inputs': data },
                    async: false,
                    success: function (result) {
                        var pa = result.IsSucess;
                        if (pa) {
                            alert('操作成功!');
                            GetAlarmList(1, true);
                        }
                        else {
                            alert('操作失败!');
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
                $(this).dialog('close');
            },
            ' 重 置 ': function () {
                //$(this).dialog('close');
                $(this).find('form')[0].reset();
            }
        },
        open: function (event, ui) {
            $(this).find('form')[0].reset();
        },
        close: function () {
            //allFields.val('').removeClass('ui-state-error');
        }
    });

    $("#dialog-form-powerSwitch").dialog({
        autoOpen: false,
        resizable: false,
        height: 240,
        width: 350,
        modal: true,
        buttons: {
            ' 提 交 ': function () {

                var atid = $(this).attr('alarmTypeId')
                var span = $('span.dialog-powerSwitch[alarmTypeId=' + atid + ']');
                var alarmTypeId = span.attr('alarmTypeId');
                var alarmLevelId = span.attr('alarmLevelId');
                var alarmEvents = []; var alarmEventIds = [];
                span.parent().prev().find('span[config]').each(function (i, n) {
                    alarmEvents[i] = $(n).attr('config');
                    alarmEventIds[i] = $(n).attr('alarmEventId');
                });
                var update = [];
                var del = [];
                var chkInputs = $(this).find('input[name=checkboxex]');
                chkInputs.each(function (i, n) {
                    var id = $(n).attr('id');
                    var plugIn = $(n).attr('plugIn');
                    id = id[id.length - 1];
                    var updId = -1;
                    var f = false;
                    $.each(alarmEvents, function (i, n) {
                        if (n == id) {
                            f = true;
                            updId = alarmEventIds[i];
                        }
                    });
                    if (n.checked) {
                        update.push({ "ID": updId, "AlarmTypeId": alarmTypeId, "PlugIn": plugIn, "Options": '' });
                    }
                    else {
                        if (f)
                            del.push(updId);
                    }
                });
                var sel = $(this).find('select[id=select]')[0];
                var alarmLevelId_new = $(sel).val();

                var data = { "AlarmTypeId": alarmTypeId, "Update": update, "Del": del };
                if (alarmLevelId != alarmLevelId_new)
                    data["AlarmLevelId"] = alarmLevelId_new;
                data = JSON.stringify(data);
                $.ajax({
                    url: getAction().updateAlarmEvent,
                    dataType: "json",
                    type: 'POST',
                    contentType: "application/x-www-form-urlencoded; charset=utf-8",
                    data: { 'Inputs': data },
                    async: false,
                    success: function (result) {
                        var pa = result.IsSucess;
                        if (pa) {
                            alert('操作成功!');
                            GetAlarmList(1, true);
                        }
                        else {
                            alert('操作失败!');
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });

                $(this).dialog('close');
            },
            ' 重 置 ': function () {
                //$(this).dialog('close');
                $(this).find('form')[0].reset();
            }
        },
        open: function (event, ui) {
            $(this).find('form')[0].reset();
        },
        close: function (event, ui) {
            //allFields.val('').removeClass('ui-state-error');
        }
    });

    $('span.dialog-play').live('click', function () {
        var dlgForm = $("#dialog-form-play");
        var config = $(this).attr("config");
        var titleTxt = $(this).text();
        if (config == 'undefined' || config == '')
            return false;

        dlgForm.dialog("option", "title", titleTxt);
        switch (config) {
            case "1":   //播放音频
                dlgForm.find('div.field').eq(1).show();
                dlgForm.find('div.field').eq(2).show();
                dlgForm.find('div.field').eq(3).hide();
                dlgForm.dialog("option", "height", 240);
                break;
            case "2":   //推视频
            case "3":   //推画面
                dlgForm.find('div.field').eq(1).hide();
                dlgForm.find('div.field').eq(2).hide();
                dlgForm.find('div.field').eq(3).hide();
                dlgForm.dialog("option", "height", 140);
                break;
            case "4":   //TTS语音配制
                dlgForm.find('div.field').eq(1).show();
                dlgForm.find('div.field').eq(2).hide();
                dlgForm.find('div.field').eq(3).hide();
                dlgForm.dialog("option", "height", 180);
                break;
            case "5":   //发送短信
                dlgForm.find('div.field').eq(1).hide();
                dlgForm.find('div.field').eq(2).hide();
                dlgForm.find('div.field').eq(3).show();
                dlgForm.dialog("option", "height", 280);
                break;
        }
        dlgForm.dialog("open");

        $("#hifConfig").val(config);
        //绑定数据
        var alarmEventId = $(this).attr('alarmEventId');
        var trigMode = $(this).attr("trigMode");
        var runMode = $(this).attr("runMode");
        var runCount = $(this).attr("runCount");
        var runTime = $(this).attr("runTime");
        var options = $(this).attr("options");
        //触发时间

        dlgForm.find('input[name=checkboxex]').each(function (i, n) {
            var id = $(n).attr('id');
            id = id[id.length - 1];
            switch (id) {
                case "1": //告警产生0x01
                    if ((parseInt(trigMode) & 1) == 1)
                        n.checked = true;
                    break;
                case "2": //告警确认0x02
                    if ((parseInt(trigMode) & 2) == 2)
                        n.checked = true;
                    break;
                case "4": //告警恢复0x04
                    if ((parseInt(trigMode) & 4) == 4)
                        n.checked = true;
                    break;
                //                case "3": //告警取消0x03            
                //                    if ((parseInt(trigMode) & 3) == 3)            
                //                        n.checked = true;            
                //                    break;            
            }
        });
        //执行方式
        var selVel = '';
        switch (runMode) {
            case "1": //按次数
                selVel = runMode + "_" + runCount;
                break;
            case "2": //按时间
                selVel = runMode + "_" + runTime;
                break;
            case "3": //执行至告警确认
            case "4": //执行至告警恢复
                selVel = runMode;
                break;
        }
        var sel = dlgForm.find('select[name=trigMode]')[0];
        $(sel).val(selVel);
        //音频文件
        var sel = dlgForm.find('select[name=options]')[0];
        var id = options[options.length - 1];
        $(sel).val(id);
        //接收用户组
        if (options.length > 0) {
            options = options.substring(0, options.length - 1);
            var arr = options.split(',');

            var ids = [];
            for (var i = 0; i < arr.length; i++) {
                var id = arr[i];
                id = id.substring(2);
                ids[i] = id;
            }

            //绑定
            for (var i in ids) {
                var chkInput = dlgForm.find('input[id=chbUser' + ids[i] + ']')[0];
                if (chkInput) {
                    chkInput.checked = true
                }
            }
        }

        dlgForm.attr('alarmEventId', alarmEventId);

        return false;
    });

    $('span.dialog-powerSwitch').live('click', function () {
        var alarmTypeId = $(this).attr('alarmTypeId');
        var alarmLevelId = $(this).attr('alarmLevelId');
        var alarmEvents = [];
        $(this).parent().prev().find('span[config]').each(function (i, n) {
            alarmEvents[i] = $(n).attr('config');
        });

        var titleTxt = $(this).parent().prev().prev().prev().text();

        var dlgForm = $("#dialog-form-powerSwitch");
        dlgForm.dialog("option", "title", titleTxt);
        dlgForm.dialog("open");

        //绑定
        for (var i in alarmEvents) {
            var chkInput = dlgForm.find('input[id=checkbox-' + alarmEvents[i] + ']')[0];
            if (chkInput) {
                chkInput.checked = true
            }
        }
        var sel = dlgForm.find('select[id=select]')[0];
        $(sel).val(alarmLevelId);
        dlgForm.attr('alarmTypeId', alarmTypeId);


        return false;
    });
});

/**
* Ajax request
* author: ghj
* time: 2013-12-15 15:26:10
*/
function getAction() {
    return {
        usergroup: "action.ashx?action=GetUserGroup",
        alarm: 'action.ashx?action=GetAlarmTypeList',
        updateAlarmEvent: 'action.ashx?action=UpdateAlarmEvent'
    };
}