<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="enter_testdata.aspx.cs" Inherits="webTest.websites.enter_testdata" %>

<!DOCTYPE html>
<link rel="stylesheet" type="text/css" href="css/menu.css" />
<link rel="stylesheet" type="text/css" href="css/general.css" />
<link rel="stylesheet" type="text/css" href="css/entertestdata.css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enter Testdata</title>
    <link rel="shortcut icon" type="image/ico" href="images/tabpic.ico" />
    <script src='js/jquery-3.2.1.min.js'></script>
    <script src='js/menu.js'></script>
    <script src='js/entertestdata.js'></script>
</head>
<body>
    <form id="form1" runat="server"  accept-charset="utf-8">
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

            <div id="headingdiv">Enter Testdata</div>
            <div id="wrapper">
                <div id="methodchoice">
                  <h3>Available Methods:</h3>
                  <input id="postdm" type="radio" name="method" onclick="handleRBClick(this);" value="postdm" > POST domainmodel<br>
                  <input id="getdm" type="radio" name="method" onclick="handleRBClick(this);" value="getdm" > GET domainmodel<br>
                  <input id="deletedm" type="radio" name="method" onclick="handleRBClick(this);" value="deletedm" > DELETE domainmodel  <br>
                  <input id="gettid" type="radio" name="method" onclick="handleRBClick(this);" value="gettid" > GET tracking id<br>
                  <input id="postcu" type="radio" name="method" onclick="handleRBClick(this);" value="postcu" > POST competence update<br>
                  <input id="deletetid" type="radio" name="method" onclick="handleRBClick(this);" value="deletetid" > DELETE tracking id <br>
                  <input id="getcs" type="radio" name="method" onclick="handleRBClick(this);" value="getcs" > GET competence state <br>
                </div> 
                <div id="methodinfo">
                    ...
                </div>
            </div>
            <div class="methoddiv" id="postdmdiv">
                <h3>REST call (POST)</h3>
                <p class="url">url:port/rest/competenceservice/storedm </p>
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                Enter domain model here:<br>
	            <asp:TextBox id="domainmodelinput" TextMode="MultiLine" runat="server" rows="10" cols="100"/><br>
	            <asp:Button class='button' id="submitdm" runat="server" Text="Post domain model"  OnClick="postdmClicked" />
            </div>
            <div class="methoddiv" id="getdmdiv" style="display: none;">
                <h3>REST call (GET)</h3>
                <p class="url">url:port/rest/competenceservice/getdm/{dmid}  </p>
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                Enter domain model id here:
                <asp:TextBox id="domainmodelid" runat="server" rows="1" cols="20"/>
                <asp:Button class='button' id="getdomainmodel" runat="server" Text="Get domain model"  OnClick="getdmClicked" /><br>
                <p>Returned information:</p>
                <asp:TextBox id="getdomainmodelreturn" TextMode="MultiLine" runat="server" rows="10" cols="100"/>
            </div>
            <div class="methoddiv" id="deletedmdiv" style="display: none;">
                <h3>REST call (DELETE)</h3>
                <p class="url">url:port/rest/competenceservice/deletedm/{dmid}   </p>
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                Enter domain model id here:
                <asp:TextBox id="dmidtodelete" runat="server" rows="1" cols="20"/>
                <asp:Button class='button' id="deletedmbyid" runat="server" Text="Delete domain model"  OnClick="deletedmClicked" /><br>
                <p>Returned information: <asp:Label id="deletedmreturninfo" runat="server" rows="1" cols="20"/> </p>
            </div>
            <div class="methoddiv" id="gettiddiv" style="display: none;">
                <h3>REST call (GET)</h3>
                <p class="url">url:port/rest/competenceservice/createtrackingid/{dmid}   </p>
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                Enter domain model id here:
                <asp:TextBox id="dmidfortid" runat="server" rows="1" cols="20"/>
                <asp:Button class='button' id="gettidbtn" runat="server" Text="Get tracking id"  OnClick="gettidClicked" /><br>
                <p>Returned information: <asp:Label id="gettidreturn" runat="server" rows="1" cols="20"/> </p>
            </div>
            <div class="methoddiv" id="postcudiv" style="display: none;">
                <h3>REST call (POST)</h3>
                <p class="url">url:port/rest/competenceservice/updatecompetencestate/{tid}  </p>
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                <div id="wrapper2">
                    <div id="firstdiv">
                        
                        <p>
                            Enter tracking id here:
                            <asp:TextBox id="updatecstid" runat="server" rows="1" cols="20"/> <br>
                            Enter datetime (optional):
                            <asp:TextBox id="datetimeupdate" runat="server" value="JJJJ-MM-DD HH:MM:SS" rows="1" cols="20"/>
                        </p>
                        Enter update xml here:<br>
	                    <asp:TextBox id="updatecsxml" TextMode="MultiLine" runat="server" rows="10" cols="100"/><br>
	                    <asp:Button class='button' id="updatecsbtn" runat="server" Text="Update competence state"  OnClick="updatecsClicked" />
                        <p>Returned information: <asp:Label id="updatecsreturn" runat="server" rows="1" cols="20"/> </p>
                    </div>
                    <div id="seconddiv">
                        <h4>Create update xml</h4>
                        <p>
                            In this section you can create an update information xml, which can be transfered to the 'Direct Access' xml input field. 
                        </p>
                        <p>Update type:
                            <select onchange="selectChange(this.value);">
                                <option value="competence">Competence</option>
                                <option value="activity">Activity</option>
                                <option value="gamesituation">Gamesituation</option>
                            </select> 
                        </p>
                        <div id="updatecreationdiv">
                        </div>
                    </div>
                </div>
            </div>
            <div class="methoddiv" id="deletetiddiv" style="display: none;">
                <h3>REST call (DELETE)</h3>
                <p class="url">url:port/rest/competenceservice/deletetrackingid/{tid}  </p> 
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                Enter tracking id here:
                <asp:TextBox id="tidtodelete" runat="server" rows="1" cols="20"/>
                <asp:Button class='button' id="deletetidbtn" runat="server" Text="Delete tracking id"  OnClick="deletetidClicked" /><br>
                <p>Returned information: <asp:Label id="deletetidreturn" runat="server" rows="1" cols="20"/> </p>
            </div>
            <div class="methoddiv" id="getcsdiv" style="display: none;">
                <h3>REST call (GET)</h3>
                <p class="url">url:port/rest/competenceservice/getcompetencestate/{tid}  </p>
                <p>This REST-call needs to be identified with basic HTTP authentication (username/password from login).</p>

                <h3>Direct Access</h3>
                Enter tracking id here:
                <asp:TextBox id="tidgetcs" runat="server" rows="1" cols="20"/>
                <asp:Button class='button' id="getcsbtn" runat="server" Text="Get competence state"  OnClick="getcsClicked" /><br>
                <p>Returned information:</p>
                <asp:TextBox id="getcsreturn" TextMode="MultiLine" runat="server" rows="10" cols="100"/>
            </div>
        </div>
    </form>
</body>
</html>
