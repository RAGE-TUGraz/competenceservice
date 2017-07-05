<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view_competencestate.aspx.cs" Inherits="webTest.websites.view_competencestate" %>

<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="css/vis.css" />
<link rel="stylesheet" type="text/css" href="css/viewcompetencestate.css" />
<link rel="stylesheet" type="text/css" href="css/menu.css" />
<link rel="stylesheet" type="text/css" href="css/general.css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src='js/jquery-3.2.1.min.js'></script>
    <script src='js/domainmodel.js'> </script>
    <script src='js/raphael-min.js'></script>
    <script src='js/graph.js'></script>
    <script src='js/vis.js'></script>
    <script src='js/menu.js'></script>
    <script src='js/viewcompetencestate.js'></script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- start sidenav -->
        <span id="menubutton" onclick="openNav()">&#9776; menu</span>
        <div id="mySidenav" class="sidenav">
          <a href="javascript:void(0)" class="closemenubtn" onclick="closeNav()">&times;</a>

          <asp:Button runat="server" id="menubtndisplayEnteryPage" style="display:none" onclick="btnEnterEntry" />
          <a href="#" onclick="document.getElementById('<%= menubtndisplayEnteryPage.ClientID %>').click()">Start</a>

          <asp:Button runat="server" id="menubtndisplayEnterDomainModel" style="display:none" onclick="btnEnterDomainmodel" />
          <a href="#" onclick="document.getElementById('<%= menubtndisplayEnterDomainModel.ClientID %>').click()">Enter Domainmodel</a>

          <asp:Button runat="server" id="menubtndisplayDomainModel" style="display:none" onclick="btnViewDomainmodel" />
          <a href="#" onclick="document.getElementById('<%= menubtndisplayDomainModel.ClientID %>').click()">View Domainmodel</a>

          <asp:Button runat="server" id="menubtndisplayCompetenceState" style="display:none" onclick="btnViewCompetencestate" />
          <a href="#" onclick="document.getElementById('<%= menubtndisplayCompetenceState.ClientID %>').click()">View Competencestate</a>

          <asp:Button runat="server" id="menubtlogout" style="display:none" onclick="btnLogout" />
          <a href="#" onclick="document.getElementById('<%= menubtlogout.ClientID %>').click()">Logout</a>
        </div>
        <!-- end sidenav -->

        <div id="main">
            <div id="headingdiv">View Competencestate</div>
            <div id="trackiniddiv">
                <a>Tracking id:</a>
                <asp:TextBox id="trackingidinput" runat="server" rows="20" />
                <asp:Button id="loadcs" runat="server" Text="Load"  OnClick="buttonloadcompetenceStateClicked" /><br/>
            </div>
            <div id="visualizationdiv">
                <div id="timelinediv">
                    <h3>Timeline:</h3>
                    <div id="visualization"></div>
                </div>
                <div id="graphdiv">
                    <h3>Graph representation:</h3>
                    <div id="graphwrapperdiv">
                        <div id="graph" runat="server" ></div>
                        <div id="nodeInfo" runat="server" ></div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
