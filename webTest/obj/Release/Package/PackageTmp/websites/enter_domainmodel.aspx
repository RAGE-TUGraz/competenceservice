<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="enter_domainmodel.aspx.cs" Inherits="webTest.Websites.WebForm1" %>

<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="css/menu.css" />
<link rel="stylesheet" type="text/css" href="css/general.css" />
<link rel="stylesheet" type="text/css" href="css/enterdomainmodel.css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enter Domainmodel</title>
    <link rel="shortcut icon" type="image/ico" href="images/tabpic.ico" />
    <script src='js/menu.js'></script>
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
            
          <asp:Button runat="server" id="menubtnentertestdata" style="display:none" onclick="btnEnterTestdata" />
          <a href="#" onclick="document.getElementById('<%= menubtnentertestdata.ClientID %>').click()">Enter Testdata</a>

          <asp:Button runat="server" id="menubtlogout" style="display:none" onclick="btnLogout" />
          <a href="#" onclick="document.getElementById('<%= menubtlogout.ClientID %>').click()">Logout</a>
        </div>
        <!-- end sidenav -->

        <div id="main">
            <div id="headingdiv">Enter Domainmodel</div>
	        
            <p>Coming soon!</p>
        </div>
	</form>
</body>
</html>
