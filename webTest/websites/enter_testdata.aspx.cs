using System;
using System.Web.UI;
using System.Web.Security;

namespace webTest.websites
{
    public partial class enter_testdata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void postdmClicked(object sender, EventArgs args)
        {
            if (domainmodelinput.Text.Equals(""))
            {
                domainmodelinput.Text = "data missing";
                return;
            }

            //int retVal = DatabaseHandler.Instance.insertdomainmodel(inputname.Text,inputpassword.Text,inputstructure.Text);
            string retVal = competenceframework.CompetenceFramework.storedm(domainmodelinput.Text);

            if (retVal == null)
                domainmodelinput.Text = "structure cannot be stored - it is not valid!";
            else
                domainmodelinput.Text = "structure stored with id " + retVal;
        }

        public void getdmClicked(object sender, EventArgs args)
        {
            string dmid = domainmodelid.Text;
            string dm = competenceframework.CompetenceFramework.getdm(dmid);
            if (dm == null)
                getdomainmodelreturn.Text = "Cannot find domain model for supplied id '"+dmid+"'.";
            else
                getdomainmodelreturn.Text = dm;
        }

        public void deletedmClicked(object sender, EventArgs args)
        {
            string dmid = dmidtodelete.Text;
            if(competenceframework.CompetenceFramework.deletedm(dmid))
                deletedmreturninfo.Text = "Successfully deleted domain model with id '"+dmid+"'.";
            else
                deletedmreturninfo.Text = "It was not able to delete domain model with id '" + dmid + "'.";
        }

        public void gettidClicked(object sender, EventArgs args)
        {
            string dmid = dmidfortid.Text;
            string datetime = datetimetid.Text;
            string tid;
            if(datetime.Equals("") || datetime.Equals("JJJJ-MM-DD HH:MM:SS"))
            {
                tid = competenceframework.CompetenceFramework.createtrackingid(dmid);
            }
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(datetime, out temp))
                {
                    gettidreturn.Text = "Update failed. (Supplied datetime unsuitable)";
                    return;
                }

                tid = competenceframework.CompetenceFramework.createtrackingid(dmid,datetime);
            }
            if (tid == null)
                gettidreturn.Text = "Unable to create tracking id to domain model id '"+dmid+"'";
            else
                gettidreturn.Text = "Tracking id '"+tid+"' generated.";
        }

        public void deletetidClicked(object sender, EventArgs args)
        {
            string tid = tidtodelete.Text;
            if (competenceframework.CompetenceFramework.deletetid(tid))
                deletetidreturn.Text = "Tracking id '" + tid + "' deleted.";
            else
                deletetidreturn.Text = "Unable to delete tracking id '"+tid+"'.";
        }

        public void getcsClicked(object sender, EventArgs args)
        {
            string tid = tidgetcs.Text;
            string cs = competenceframework.CompetenceFramework.getcpByTid(tid);
            if(cs==null)
                getcsreturn.Text = "Unable to load competence state for tracking id '"+tid+"'.";
            else
                getcsreturn.Text = cs;
        }

        public void updatecsClicked(object sender, EventArgs args)
        {
            string tid = updatecstid.Text;
            string updatexml = updatecsxml.Text;
            string datetime = datetimeupdate.Text;
            if (datetime == "" || datetime ==  "JJJJ-MM-DD HH:MM:SS")
            {
                if (competenceframework.CompetenceFramework.updatecompetencestate(tid, updatexml))
                    updatecsreturn.Text = "Update was successful.";
                else
                    updatecsreturn.Text = "Update failed.";
            }
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(datetime, out temp))
                {
                    updatecsreturn.Text = "Update failed. (Supplied datetime unsuitable)";
                    return;
                }

                if (competenceframework.CompetenceFramework.updatecompetencestate(tid, updatexml, datetime))
                    updatecsreturn.Text = "Update was successful.";
                else
                    updatecsreturn.Text = "Update failed.";
            }

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