using System;
using System.Web.UI;
using System.Web.Security;

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
                //inputstructure.Text = "Id unknown!";
                return;
            }
            //inputstructure.Text = dmstructure;
            string dm = "\"" + dmstructure.Replace("\"", "'")+ "\"";
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey0", "showVisualisation();", true);
            Page.ClientScript.RegisterStartupScript(GetType(),"MyKey", "drawDomainModel(" + dm+");", true);


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

        protected void btnEnterTestdata(object sender, EventArgs e)
        {
            Response.Redirect("enter_testdata.aspx");
        }

        protected void btnLogout(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect(@"..\Login.aspx");
        }
        #endregion
    }
}