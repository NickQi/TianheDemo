<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usercookies.aspx.cs" Inherits="InterfaceWeb.usercookies" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btn").click(function () {
                $.ajax({
                    type: "post",
                    data: '',
                    url: "ser.aspx?serivename=userCookies&date=" + new Date().getTime(),
                    dataType: "text",
                    success: function (data) {
                        $("#output").val(data);
                    },
                    error: function () {
                        alert('error');
                    }
                });
            });
        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    服务名称：userCookies
    
    <br/>
    输出：
     <textarea rows="5" cols="40" id="output">
    
    </textarea>

    <input type="button" id="btn" value="click">
    </div>
    </form>
</body>
</html>
