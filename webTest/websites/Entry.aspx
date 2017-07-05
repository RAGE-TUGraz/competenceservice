<%@ Page Language="C#" Inherits="competenceservice.Entry" ValidateRequest="false" %>
<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="css/styleEntry.css" />
<link rel="stylesheet" type="text/css" href="css/menu.css" />
<link rel="stylesheet" type="text/css" href="css/general.css" />
<html>
<head runat="server">
	<title>Competence Service Start</title>
    <link rel="shortcut icon" type="image/ico" href="images/tabpic.ico" />
	<meta charset="UTF-8">
    <script src='js/menu.js'></script>
    <script src='js/jquery-3.2.1.min.js'></script>
    <script src='js/entryTooltip.js'></script>
</head>
<body>
	<form id="form1" runat="server" accept-charset="utf-8">
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
            <div class="headingstart">Competence Service</div>
            <div id="wrapper">
                <div id="firstdiv">
                    <asp:Button class="rectangularbutton button" id="buttonloadenterdm" runat="server" Text="Enter Domainmodel!"  OnClick="buttonloadenterdmClicked" /><br>
                    <asp:Button class="rectangularbutton button" id="buttonviewdomainmodel" runat="server" Text="View Domainmodel!"  OnClick="buttonviewdomainmodelClicked" /><br>
                    <asp:Button class="rectangularbutton button" id="buttonviewcompetencestate" runat="server" Text="View Competencestate!"  OnClick="buttonviewcompetencestateClicked" /><br>
                </div>
                <div id="seconddiv">
                    <div id="infoDiv">
                    </div>
                </div>
            </div>
        </div>
	</form>
</body>
</html>





