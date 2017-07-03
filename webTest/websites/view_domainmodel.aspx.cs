using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webTest.websites
{
    public partial class view_domainmodel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void buttonloaddomainmodelClicked(object sender, EventArgs e)
        {
            //load dm with id and display it in textbox
            string dmid = dmidinput.Text;
            string dmstructure = competenceframework.CompetenceFramework.getdm(dmid);
            if (dmstructure == null)
            {
                inputstructure.Text = "Id unknown!";
                return;
            }
            inputstructure.Text = dmstructure;
            string dm = "\"" + dmstructure.Replace("\"", "'")+ "\"";
            Page.ClientScript.RegisterStartupScript(GetType(),"MyKey", "drawDomainModel(" + dm+");", true);


        }
    }
}