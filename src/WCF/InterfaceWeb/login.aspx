<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="InterfaceWeb.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/Json.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btn").click(function () {
                $.ajax({
                    type: "post",
                    data: { inputs: $("#input").val() },
                    url: "ser.aspx?serivename=loginService&date=" + new Date().getTime(),
                    dataType: "text",
                    success: function (data) {
                        $("#output").val(data);
                    },
                    error: function () {
                        alert('ssssssss');
                    }
                });
            });

            $("#userid").blur(function () {
                getJson();
            });
            $("#userpass").blur(function () {
                getJson();
            });
            $("#isremeber").blur(function () {
                getJson();
            });
        });

        function getJson() {
            var data = {
                LoginUser: $("#userid").val(),
                LoginPass: $("#userpass").val(),
                IsRemberPass: Boolean($("#isremeber").val())
            };

            $("#input").val(JSONUtil.encode(data));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        服务名称：loginService
        <p>
            -------------------------------------------------------------------
            <br />
            参数：输入参数自动生成输入json
            <p>
                -------------------------------------------------------------------
                <p>
                    userid:<input type="text" id="userid" /><br />
                    userpass:<input type="text" id="userpass" /><br />
                    isremeber:<input type="text" id="isremeber" /><br />
                    <br />
                    -------------------------------------------------------------------
                    <p>
                        前端输入json：
                        <textarea rows="5" cols="40" id="input">
    
    </textarea>
                        <br />
                        后台输出json：
                        <textarea rows="5" cols="40" id="output">
    
    </textarea>
                        <input type="button" id="btn" value="click" />
    </div>
    </form>
</body>
</html>
