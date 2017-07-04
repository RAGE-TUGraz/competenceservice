<%@ Page Language="C#" Inherits="competenceservice.Entry" ValidateRequest="false" %>
<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="css/styleEntry.css" />
<html>
<head runat="server">
	<title>Default</title>
	<meta charset="UTF-8">
</head>
<body>
	<form id="form1" runat="server" accept-charset="utf-8">
    <div class="headingstart">Competence Service</div>
    <asp:Button class="rectangularbutton" id="buttonloadenterdm" runat="server" Text="Enter new domainmodel!"  OnClick="buttonloadenterdmClicked" />
    <asp:Button class="rectangularbutton" id="buttonviewdomainmodel" runat="server" Text="View domainmodel!"  OnClick="buttonviewdomainmodelClicked" />
    <asp:Button class="rectangularbutton" id="buttonviewcompetencestate" runat="server" Text="View competence state!"  OnClick="buttonviewcompetencestateClicked" /><br>
	</form>
</body>
</html>





