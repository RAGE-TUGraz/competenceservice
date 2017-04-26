using System;
using System.Web;
using System.Web.UI;

namespace webTest
{
	
	public partial class Default : System.Web.UI.Page
	{
		public void button1Clicked (object sender, EventArgs args)
		{
			if (inputname.Text.Equals ("") || inputpassword.Text.Equals ("") || inputstructure.Text.Equals ("")) {
				inputname.Text += " - data missing";
				return;
			}

            DBConnectDomainModel dbc = new DBConnectDomainModel();
			int retVal = dbc.Insert (inputname.Text,inputpassword.Text,inputstructure.Text);

			if (retVal == 0) {
				inputname.Text = "";
				inputpassword.Text = "";
				inputstructure.Text = "";
			} else {
				inputname.Text += " - NAME already taken!";
			}
		}
	}
}

