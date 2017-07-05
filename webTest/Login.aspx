<%@ Page Language="C#" Inherits="competenceservice.Login" ValidateRequest="false" %>

<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="websites/css/login.css" />
<link rel="stylesheet" type="text/css" href="websites/css/general.css" />
<html>
<head runat="server">
	<title>Default</title>
	<meta charset="UTF-8">
</head>
<body>


<div id="headingdiv">
    Competence Service
</div>
    <div id="warningdiv">
    <p><asp:Label id="lblInvalid" runat="server" /></p>
</div>



<form runat="server">
    <div id="logindiv">
        <h3>Please Login:</h3>
        <table>
          <tr>
            <td>Username:</td>
            <td><asp:TextBox id="txtUsername" runat="server" /></td>
          </tr>
          <tr>
            <td>Password:</td>
            <td><asp:TextBox id="txtPassword" TextMode="password" runat="server" /></td>
          </tr>
        </table> 

	    <br />
	    <asp:Button id="btnLogin" CssClass="button" runat="server" text="Login" OnClick="btnLogin_Click"/>
    </div>
</form>
</body>
</html>





