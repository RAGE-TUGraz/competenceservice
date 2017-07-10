using System;
using System.Web.Security;

namespace webTest.Websites
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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