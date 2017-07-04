using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webTest.websites
{
    public partial class view_competencestate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //tmp: load tracking id 1
            trackingidinput.Text = "1";
            buttonloadcompetenceStateClicked(null,null);
        }

        protected void buttonloadcompetenceStateClicked(object sender, EventArgs e)
        {
            string tid = trackingidinput.Text;
            string competenceProbabilities = competenceframework.CompetenceFramework.getcpByTid(tid);
            if (competenceProbabilities == null)
            {
                //outputcs.Text = "Id unknown";
                return;
            }
            //outputcs.Text = competenceProbabilities;

            string dmid = competenceframework.CompetenceFramework.getDomainModelIdByTrackingId(tid);
            string dmstring = competenceframework.CompetenceFramework.getdm(dmid);

            string dm = "\"" + dmstring.Replace("\"", "'") + "\"";
            string cp = "\"" + competenceProbabilities.Replace("\"", "'") + "\"";
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "drawDomainModel(" + dm + ");", true);
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey2", "drawCompetenceState(" + cp + ");", true);

            //history, timeline basic @ http://visjs.org/timeline_examples.html
            string updateHistory = "\"" + competenceframework.CompetenceFramework.getTrackingHistory(tid).Replace("\"", "'") + "\"";
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey2.5", "drawUpdateHistory(" + updateHistory + ");", true);
            
        }
    }
}