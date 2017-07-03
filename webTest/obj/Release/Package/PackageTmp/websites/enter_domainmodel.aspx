<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="enter_domainmodel.aspx.cs" Inherits="webTest.Websites.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
