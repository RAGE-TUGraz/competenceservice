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
	
	public partial class Entry : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            //tmp automatic redirect view competence state:
            Response.Redirect("enter_domainmodel.aspx");
        }


        protected void buttonloadenterdmClicked(object sender, EventArgs e)
        {
            Response.Redirect("enter_domainmodel.aspx");
        }

        protected void buttonviewdomainmodelClicked(object sender, EventArgs e)
        {
            Response.Redirect("view_domainmodel.aspx");
        }

        protected void buttonviewcompetencestateClicked(object sender, EventArgs e)
        {
            Response.Redirect("view_competencestate.aspx");
        }

        #region sidenavi
        protected void btnEnterDomainmodel(object sender, EventArgs e)
        {
            Response.Redirect("enter_domainmodel.aspx");
        }

        protected void btnViewDomainmodel(object sender, EventArgs e)
        {
            Response.Redirect("view_domainmodel.aspx");
        }

        protected void btnViewCompetencestate(object sender, EventArgs e)
        {
            Response.Redirect("view_competencestate.aspx");
        }

        protected void btnEnterEntry(object sender, EventArgs e)
        {
            Response.Redirect("Entry.aspx");
        }

        protected void btnLogout(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect(@"..\Login.aspx");
        }
        #endregion
    }
}

