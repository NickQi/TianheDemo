function Sys_Menu() { }
Sys_Menu.prototype = {
    name: '系统菜单处理js类',
    addmenu:function() {
             var menupic = $("#txtImgUrl").val();
             var menuname = $("#menuname").val();
             var parentid = $("#parentid").val();
             var url = $("#url").val();
             var orderid = $("#orderid").val();
             $.ajax({
                 url: "action.ashx?method=NTS_BECM.BLL.menu.addmenu&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                 type: 'Post',
                 contentType: "application/x-www-form-urlencoded; charset=utf-8",
                 data: { menupic: menupic, menuname: menuname,parentid:parentid,url:url,orderid: orderid },
                 timeout: 1000,
                 success: function (data) {
                     eval('data=' + data);
                     if (data.success) {
                         window.location.href = "menu.aspx";
                     } else {
                         alert(data.msg);
                         return;
                     }
                 }
             });
         },
         deletemenu:function(menuid) {
             $.ajax({
                 url: "action.ashx?method=NTS_BECM.BLL.menu.deletemenu&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                 type: 'Post',
                 contentType: "application/x-www-form-urlencoded; charset=utf-8",
                 data: { id: menuid },
                 timeout: 1000,
                 success: function (data) {
                     eval('data=' + data);
                     if (data.success) {
                         // new department().departlist();
                         window.location.href = "menu.aspx";
                     } else {
                         alert(data.msg);
                         return;
                     }
                 }
             });
         },
          showmenu:function(menuid) {
             $.ajax({
                 url: "action.ashx?method=NTS_BECM.BLL.menu.showmenu&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                 type: 'Post',
                 contentType: "application/x-www-form-urlencoded; charset=utf-8",
                 data: { id: menuid },
                 timeout: 1000,
                 success: function (data) {
                     eval('data=' + data);
                     if (data.success) {
                         $("#txtImgUrl").val(data.menupic)
                         $("#menuname").val(data.menuname);
                         $("#parentid").val(data.parentid);
                         $("#url").val(data.url);
                         $("#orderid").val(data.orderid);
                         $("#menuid").val(data.menuid);
                     } else {
                         alert(data.msg);
                         return;
                     }
                 }
             });
         },
          editmenu:function() {
             var menuid = $("#menuid").val();
             var menupic = $("#txtImgUrl").val();
             var menuname = $("#menuname").val();
             var parentid = $("#parentid").val();
             var url = $("#url").val();
             var orderid = $("#orderid").val();
             $.ajax({
                 url: "action.ashx?method=NTS_BECM.BLL.menu.savemenu&dll=NTS_BECM.BLL&times=" + new Date().getTime(),
                 type: 'Post',
                 contentType: "application/x-www-form-urlencoded; charset=utf-8",
                 data: { menuid:menuid,menupic: menupic, menuname: menuname, parentid: parentid, url: url, orderid: orderid },
                 timeout: 1000,
                 success: function (data) {
                     eval('data=' + data);
                     if (data.success) {
                         // new department().departlist();
                         window.location.href = "menu.aspx";
                     } else {
                         alert(data.msg);
                         return;
                     }
                 }
             });
         }
}
$(function () {
    //注册按钮的单击事件
    var obj = new Sys_Menu();
    $("#btn").click(function (e) { obj.Add(); });
});






