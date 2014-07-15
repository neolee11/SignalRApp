﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingletonWebPage.aspx.cs" Inherits="WebFormHost.SingletonWebPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--<button id="btnSendMsg">Send Message</button>--%>
            <input type="button" id="btnSendMsg" value="Send Message"  />
            
        </div>

        <div>
            <label>This page can be only written by one user at a time</label>
            <br />
            <br />
            <label>Input : </label>
            <textarea id="myTextArea" style="width: 400px"></textarea>
            <br />
            <br />
            <asp:Button runat="server" ID="btnSave" Text="Save" />  
        </div>
    </form>

    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/jquery.signalR-2.1.0.js"></script>
    <script src="/signalr/hubs"></script>

    <script type="text/javascript">
        var accessManager;

        $(function () {
            $('#btnSendMsg').click(sendClientMessage);

            //One way

            $.connection.hub.logging = true;
            //$.connection.hub.url = "http://localhost:63213/signalr";

            accessManager = $.connection.accessManager;
            accessManager.client.getMessage = getSenderMessage;
            //$.connection.hub.start({transport: 'longPolling'}); //if the client insists on using long polling
            $.connection.hub.start();



            $.connection.hub.error(function (err) {
                alert(err);
            });
        });

        function getSenderMessage(message) {
            //alert("getting: " + message);
            $('#myTextArea').val(message);
        }

        function sendClientMessage() {
            //var msg = $('#myTextArea').val();

            if (accessManager) {
                accessManager.server.broadCastMessage('aaddaf');
            } else {
                alert('hub not set up');
            }
        }

        //$.connection.hub.error(function (err) {
        //    alert(err);
        //});


    </script>


</body>
</html>
