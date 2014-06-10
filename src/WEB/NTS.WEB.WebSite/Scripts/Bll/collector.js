 function collector() { }
 collector.prototype = {
     name: '采集器js类',
     showcollectorlist: function (current_page) {
         var buildid = $("#sbuild").val();
         //alert(buildid);
         $.ajax({
             url: "action.ashx?method=NTS_BECM.BLL.T_ST_DataCollectionInfo.showcollectorlist&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
             type: 'Post',
             dataType: 'text',
             contentType: "application/x-www-form-urlencoded; charset=utf-8",
             data: { buildid: buildid, current_page: current_page, pagesize: 17 },
             timeout: 1000,
             success: function (data) {
                 //  alert(data);
                 if (data != ']') {
                     data = eval("data=" + data);
                     $("#collectorlist").html('');
                     var htmls = '';
                     for (var i = 0; i < data.length; i++) {
                         htmls += "<li>";
                         htmls += "<table width=\"776\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"menu_tbl\">";
                         htmls += "<tr height=\"30\" valign=\"middle\">";
                         htmls += "<td width=\"90\"><span style=\"float:left\">" + data[i].F_BuildName + "</span></td>";
                         htmls += "<td width=\"111\" style=\"text-align:left\"><span><span class=\"imgclick\"></span> " + data[i].F_CollectionID + "</span></td>";
                        
                         htmls += "<td width=\"90\"><span style=\"float:left\">" + data[i].F_CollectionName + "</span></td>";
                         htmls += "<td width=\"90\"><span style=\"float:left\">" + data[i].F_CollectionURL + "</span></td>";
                         htmls += "<td width=\"90\" align=\"left\"><span style=\"float:left\">" + data[i].F_CollectStartTime.replace('0:00:00', '') + "</span></td>";
                         htmls += "<td width=\"90\"><span style=\"float:left\">" + data[i].F_CollectInterval + "</span></td>";

                         htmls += "<td width=\"148\" valign=\"middle\" align=\"center\"><span class=\"btnbg2\"><input type=\"button\" class=\"button04\" onclick=\"DivRevise4('" + data[i].F_CollectionID + "')\" value=\"修改\"><input type=\"button\" class=\"button04\" onclick=\"Del3('" + data[i].F_CollectionID + "')\" value=\"删除\"></span></td>";
                         htmls += "<td width=\"67\"></td>";
                         htmls += "</tr>";
                         htmls += "</table>";
                         htmls += "</li>";

                     }
                     $("#collectorlist").append(htmls);
                 } else {
                     $("#collectorlist").html('<font color=red>对不起，暂无数据。</font>');
                 }

             }
         });
     },
     deletecollector: function (v) {
         $.ajax({
             url: "action.ashx?method=NTS_BECM.BLL.T_ST_DataCollectionInfo.deletecollector&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
             type: 'Post',
             dataType: 'text',
             contentType: "application/x-www-form-urlencoded; charset=utf-8",
             data: { cid: v },
             timeout: 1000,
             success: function (data) {
                 //alert(data);
                 data = eval("data=" + data);
                 if (data.success) {
                     // alert('添加采集器成功。');
                     window.location = "collector.aspx?id=" + new Date().getDate();
                     return;
                 } else {
                     alert(data.msg);
                     return;
                 }
             }
         });
     },
     updatecollector: function () {
         var F_BuildID = $("#EF_BuildID").val();
         var F_CollectionID = $("#EF_CollectionID").val();
         var F_CollectionName = $("#EF_CollectionName").val();
         var F_CollectionURL = $("#EF_CollectionURL").val();
         var F_CollectStartTime = $("#EF_CollectStartTime").val();
         var F_CollectInterval = $("#EF_CollectInterval").val();
         // alert($("#EF_BuildID").val());
         // return;
         var data = {
             F_BuildID: F_BuildID,
             F_CollectionID: F_CollectionID,
             F_CollectionName: F_CollectionName,
             F_CollectionURL: F_CollectionURL,
             F_CollectStartTime: F_CollectStartTime,
             F_CollectInterval: F_CollectInterval
         }

         if (F_CollectionID.length != 12) {
             alert('采集器的代码长度为12位的数字和字母的组合字符');
             return false;
         }
         if (F_CollectionName.length == 0) {
             alert('请输入采集器的名称。');
             return false;
         }
         if (F_CollectionName.length > 48) {
             alert('采集器的名称长度不能超过48个字符。');
             return false;
         }
         if (F_CollectStartTime.length == 0) {
             alert('请输入采集器的开始使用时间。');
             return false;
         }
         if (F_CollectInterval.length == 0) {
             alert('请输入采集器的采集周期。');
             return false;
         }

         if (F_CollectInterval.length >8) {
             alert('采集周期位数不能大于8位。');
             return false;
         }
         var isint = MyCommValidate({ rule: "number", value: F_CollectInterval });
         if (isint != '') { alert("采集器周期格式错误：" + isint); return false; }
         // 验证输入的信息
         $.ajax({
             url: "action.ashx?method=NTS_BECM.BLL.T_ST_DataCollectionInfo.updatecollector&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
             type: 'Post',
             dataType: 'text',
             contentType: "application/x-www-form-urlencoded; charset=utf-8",
             data: data,
             timeout: 1000,
             // Content-Type = 'text/json;charset=UTF-8',
             success: function (data) {
                 //alert(data);
                 data = eval("data=" + data);
                 if (data.success) {
                     alert('修改采集器成功。');
                     window.location = "collector.aspx?id=" + new Date().getDate();
                     return;
                 } else {
                     alert(data.msg);
                     return;
                 }
             }
         });
     },
     showcollector: function (v) {
         $.ajax({
             url: "action.ashx?method=NTS_BECM.BLL.T_ST_DataCollectionInfo.showcollector&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
             type: 'Post',
             dataType: 'text',
             contentType: "application/x-www-form-urlencoded; charset=utf-8",
             data: { cid: v },
             timeout: 1000,
             success: function (data) {
                 if (data != ']') {
                     data = eval("data=" + data);
                     // 文本框赋值
                     $("ul.sub_mid_collector#edit li").each(function () {
                         //alert('');
                         if ($(this).attr('config') == data[0].F_BuildID) {
                             var wt = $(this).html();
                             //  alert(wt);
                             $("#EF_BuildIDText").val(wt);
                             //alert($(this).attr('config'));
                             $("#EF_BuildID").val($(this).attr('config'));
                         }
                     });
                     $("#EF_CollectionID").val(data[0].F_CollectionID);
                     $("#EF_CollectionName").val(data[0].F_CollectionName);
                     $("#EF_CollectionURL").val(data[0].F_CollectionURL);
                     $("#EF_CollectStartTime").val(data[0].F_CollectStartTime.replace("0:00:00", ""));
                     $("#EF_CollectInterval").val(data[0].F_CollectInterval);

                 }
             }
         });
     },
     addcollector: function () {
         //  alert('xxx');
         //  return;
         var F_BuildID = $("#F_BuildID").val();
         var F_CollectionID = $("#F_CollectionID").val();
         var F_CollectionName = $("#F_CollectionName").val();
         var F_CollectionURL = $("#F_CollectionURL").val();
         var F_CollectStartTime = $("#F_CollectStartTime").val();
         var F_CollectInterval = $("#F_CollectInterval").val();
         var data = {
             F_BuildID: F_BuildID,
             F_CollectionID: F_CollectionID,
             F_CollectionName: F_CollectionName,
             F_CollectionURL: F_CollectionURL,
             F_CollectStartTime: F_CollectStartTime,
             F_CollectInterval: F_CollectInterval
         }

         if (F_CollectionID.length != 12) {
             alert('采集器的代码长度为12位的数字和字母的组合字符');
             return false;
         }
         if (F_CollectionName.length == 0) {
             alert('请输入采集器的名称。');
             return false;
         }
         if (F_CollectionName.length > 48) {
             alert('采集器的名称长度不能超过48个字符。');
             return false;
         }
         if (F_CollectStartTime.length == 0) {
             alert('请输入采集器的开始使用时间。');
             return false;
         }
         if (F_CollectInterval.length == 0) {
             alert('请输入采集器的采集周期。');
             return false;
         }
         if (F_CollectInterval.length > 8) {
             alert('采集周期位数不能大于8位。');
             return false;
         }
         var isint = MyCommValidate({ rule: "number", value: F_CollectInterval });
         if (isint != '') { alert("采集器周期格式错误：" + isint); return false; }

         // 验证输入的信息
         $.ajax({
             url: "action.ashx?method=NTS_BECM.BLL.T_ST_DataCollectionInfo.addcollector&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
             type: 'Post',
             dataType: 'text',
             contentType: "application/x-www-form-urlencoded; charset=utf-8",
             data: data,
             timeout: 1000,
             success: function (data) {
                 //alert(data);
                 data = eval("data=" + data);
                 if (data.success) {
                     alert('添加采集器成功。');
                     window.location = "collector.aspx?id=" + new Date().getDate();
                     return;
                 } else {
                     alert(data.msg);
                     return;
                 }
             }
         });
     }
 }

 $(function () {
     $("#F_BuildIDText").val($(".sub_mid_collector#add li:first").html());
     $("#F_BuildID").val($(".sub_mid_collector#add li:first").attr('config'));
     //alert($("#F_BuildID").val());
 });
 // 初始化
 
 