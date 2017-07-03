﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view_domainmodel.aspx.cs" Inherits="webTest.websites.view_domainmodel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <a>Enter domain model id:</a><br/>
        <asp:TextBox id="dmidinput" runat="server" rows="20" />
        <asp:Button id="loaddm" runat="server" Text="Load"  OnClick="buttonloaddomainmodelClicked" /><br/>
        <asp:TextBox id="inputstructure" TextMode="MultiLine" runat="server" rows="10" cols="40"/>  
    </form>
    <div id="graph" style="width:300px;height:400px;border-color: black;border:solid;"></div>
    <div id="nodeInfo" style="width:300px;padding:3px;margin-top:15px;"></div>
</body>
</html>
