using System;
using System.Web.Security;

namespace webTest.Websites
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void button1Clicked(object sender, EventArgs args)
        {
            if (inputstructure.Text.Equals(""))
            {
                inputstructure.Text = "data missing";
                return;
            }

            //int retVal = DatabaseHandler.Instance.insertdomainmodel(inputname.Text,inputpassword.Text,inputstructure.Text);
            string retVal = competenceframework.CompetenceFramework.storedm(inputstructure.Text);

            if(retVal==null)
                inputstructure.Text = "structure cannot be stored - it is not valid!";
            else
                inputstructure.Text = "structure stored with id " + retVal;
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