<%@ Page Language="C#" Inherits="competenceservice.Login" ValidateRequest="false" %>

<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="style.css" />
<html>
<head runat="server">
	<title>Default</title>
	<meta charset="UTF-8">
</head>
<body>
<h2>Please Login:</h2>


<p>
<asp:Label id="lblInvalid" runat="server" />
</p>

<form runat="server">

	Username: <asp:TextBox id="txtUsername" runat="server" /><br />
	Password: <asp:TextBox id="txtPassword" TextMode="password" runat="server" /><br />
	<br />
	<asp:Button id="btnLogin" runat="server" text="Login" OnClick="btnLogin_Click"/>
	<asp:Button id="btnLogout" runat="server" text="Logout" OnClick="btnLogout_Click"/>
</form>
</body>
</html>





