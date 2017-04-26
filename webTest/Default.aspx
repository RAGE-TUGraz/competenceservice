<%@ Page Language="C#" Inherits="competenceservice.Default" ValidateRequest="false" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>Default</title>
	<meta charset="UTF-8">
</head>
<body>
	<form id="form1" runat="server" accept-charset="utf-8">
	<!---
		<asp:Button id="button2" runat="server" Text="Click me!" OnClick="button1Clicked" />
		<br/>
		<a href="webservice.asmx">webservice</a>
		-->
	<a>Enter new domainmodel:</a><br>

	<br>
	structure:<br>
	<asp:TextBox id="inputstructure" TextMode="MultiLine" runat="server" rows="10" cols="40"/>
	<br>
	<asp:Button id="button1" runat="server" Text="Submit!"  OnClick="button1Clicked" />
	</form>
</body>
</html>

