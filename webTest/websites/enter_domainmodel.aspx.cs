using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

            inputstructure.Text = "structure stored with id " + retVal;
        }
    }
}