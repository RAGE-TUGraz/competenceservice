using System;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Text;

namespace webTest
{
	public class webservice : System.Web.Services.WebService
	{
		public webservice()
		{}


		[WebMethod]
		public int sum(int a, int b){
			return a + b;
		}


		[WebMethod]
		public XmlDocument test(){
			StringBuilder sb = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(sb);

			writer.WriteStartDocument();
			writer.WriteStartElement("People");

			writer.WriteStartElement("Person");
			writer.WriteAttributeString("Name", "Nick");
			writer.WriteEndElement();

			writer.WriteStartElement("Person");
			writer.WriteStartAttribute("Name");
			writer.WriteValue("Nick");
			writer.WriteEndAttribute();
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndDocument();

			writer.Flush();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(sb.ToString());
			return xmlDocument;
		}

	}
}

