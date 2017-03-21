using System;
using System.Collections.Generic;
using competenceTest;

namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			DBConnectDomainModel dbc = new DBConnectDomainModel ();
			dbc.dropTable ();
			dbc.createTable ();
			dbc.enterTestData ();


			CompetenceHandler ch = CompetenceHandler.Instance;
			int tc = ch.requestTrackingcode ("dm1");

			Console.WriteLine ("Requested tracking code: "+tc.ToString());

			/*
			List<string> list = new List<string>();
			list.Add ("name='dm1'");
			string dmstring = dbc.Select (list) [3] [0];
			Console.WriteLine (dmstring);
			DomainModel dm = DomainModel.getDMFromXmlString (dmstring);
			dm.print ();
			//*/

			/*
			dbc.Insert ("n1","p1","<es><67>");
			int b = dbc.Insert ("n2","p2","s2");

			if (b == 1)
				Console.WriteLine (" not inserted - structure name already exists");

			List<string> list = new List<string>();
			list.Add ("name='n1'");
			list.Add ("password='p1'");
			Console.WriteLine (dbc.Select(list)[3][0]);

			//Console.WriteLine ("Hello World!");

			*/
		}
	}
}
