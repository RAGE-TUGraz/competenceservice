/*
  Copyright 2016 TUGraz, http://www.tugraz.at/
  
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  This project has received funding from the European Union’s Horizon
  2020 research and innovation programme under grant agreement No 644187.
  You may obtain a copy of the License at
  
      http://www.apache.org/licenses/LICENSE-2.0
  
  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
  
  This software has been created in the context of the EU-funded RAGE project.
  Realising and Applied Gaming Eco-System (RAGE), Grant agreement No 644187, 
  http://rageproject.eu/

  Development was done by Cognitive Science Section (CSS) 
  at Knowledge Technologies Institute (KTI)at Graz University of Technology (TUGraz).
  http://kti.tugraz.at/css/

  Created by: Matthias Maurer, TUGraz <mmaurer@tugraz.at>
*/

using System;
using System.Web.Security;

namespace competenceservice
{
	
	public partial class Login : System.Web.UI.Page
	{

        protected void Page_Load(object sender, EventArgs e)
        {
            //tmp automatic login:
            //FormsAuthentication.RedirectFromLoginPage("rage", true);
            //Response.Redirect("websites/Entry.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!competenceframework.CompetenceFramework.canConnectToDatabase())
            {
                lblInvalid.Text = "Cannot connect to database!";
                return;
            }


            if (competenceframework.CompetenceFramework.isUserValid(txtUsername.Text, txtPassword.Text))
            {
                FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, true);
                Response.Redirect("websites/Entry.aspx");
            }
            else
            {
                lblInvalid.Text = "Username/Password incorrect!";
            }
        }
    }
}

