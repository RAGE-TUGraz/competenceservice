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
                outputcs.Text = "Id unknown";
                return;
            }
            outputcs.Text = competenceProbabilities;

            string dmid = competenceframework.CompetenceFramework.getDomainModelIdByTrackingId(tid);
            string dmstring = competenceframework.CompetenceFramework.getdm(dmid);

            string dm = "\"" + dmstring.Replace("\"", "'") + "\"";
            string cp = "\"" + competenceProbabilities.Replace("\"", "'") + "\"";
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "drawDomainModel(" + dm + ");", true);
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey2", "drawCompetenceState(" + cp + ");", true);


            //timeline basic @ http://visjs.org/timeline_examples.html
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey3", createTimelineJSCode(), true);

        }

        private string createTimelineJSCode()
        {
            string js = "var container = document.getElementById('visualization');";
            js += "var items = new vis.DataSet([";
            js += createOneTimelineEntry("1", "item1", "2014-04-20 20:15:20") + "," + createOneTimelineEntry("2", "item2", "2014-04-20 05:15:20") + ",";
            js += createOneTimelineEntry("3", "item3", "2014-04-20 19:15:55") + "," + createOneTimelineEntry("4", "item4", "2014-04-20 10:15:20") + ",";
            js += createOneTimelineEntry("5", "item5", "2014-04-20 18:15:20") + "," + createOneTimelineEntry("6", "item6", "2014-04-20 15:15:20");
            js += " ]); ";
            js += "var options = {};";
            js += "var timeline = new vis.Timeline(container, items, options);";
            return js;
        }

        //start format: 2014-04-18 20:15:20
        private string createOneTimelineEntry(string id, string content, string start)
        {
            return "{id:"+id+", content: '"+content+"', start:'"+start+ "', type: 'point'}";
        }
    }
}