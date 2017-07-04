<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view_competencestate.aspx.cs" Inherits="webTest.websites.view_competencestate" %>

<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="css/vis.css" />
<link rel="stylesheet" type="text/css" href="css/viewcompetencestate.css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src='js/jquery-3.2.1.min.js'></script>
    <script src='js/domainmodel.js'> </script>
    <script src='js/raphael-min.js'></script>
    <script src='js/graph.js'></script>
    <script src='js/vis.js'></script>
</head>
<body>
    <form id="form1" runat="server">
        <a>View competence state:</a><br/>
        <a>Tracking id:</a>
        <asp:TextBox id="trackingidinput" runat="server" rows="20" />
        <asp:Button id="loadcs" runat="server" Text="Load"  OnClick="buttonloadcompetenceStateClicked" /><br/>
        <!--
            <asp:TextBox id="outputcs" TextMode="MultiLine" runat="server" rows="10" cols="40"/>
            -->
        <h3>Timeline:</h3>
        <div id="visualization"></div>

        <h3>Graph representation:</h3>
        <div id="graph" runat="server" style="width:300px;height:400px;border-color: black;border:solid;"></div>
        <div id="nodeInfo" runat="server" style="width:300px;padding:3px;margin-top:15px;"></div>
    </form>
</body>
</html>
