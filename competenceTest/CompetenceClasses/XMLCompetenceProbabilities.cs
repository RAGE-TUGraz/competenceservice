using System;
using System.Xml.Serialization;
using System.Xml;

using consoleTest;
using System.IO;
using System.Collections.Generic;

namespace competenceTest
{
	[XmlRoot("competenceprobabilities")]
	public class XMLCompetenceProbabilities
	{
		#region Properties

		[XmlElement("competence")]
		public List<CompetenceProbability> competenceProbabilityList { get; set; }

		#endregion
		#region Constructors

		/// <summary>
		/// Constructor using a competence state.
		/// </summary>
		/// 
		/// <param name="cs"> competence state which is used to create the structure. </param>
		public XMLCompetenceProbabilities(CompetenceState cs)
		{
			competenceProbabilityList = new List<CompetenceProbability> ();
			foreach (KeyValuePair<Competence, double> entry in cs.pairs)
			{
				competenceProbabilityList.Add (new CompetenceProbability(entry.Key.id,entry.Value));
			}

		}

		public XMLCompetenceProbabilities()
		{
			competenceProbabilityList = new List<CompetenceProbability> ();
		}

		#endregion
		#region Methods

		public static XMLCompetenceProbabilities getCSFromXmlString(String str)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(XMLCompetenceProbabilities));
			using (TextReader reader = new StringReader(str))
			{
				XMLCompetenceProbabilities result = (XMLCompetenceProbabilities)serializer.Deserialize(reader);
				return (result);
			}
		}

		public String toXmlString()
		{
			try
			{
				var xmlserializer = new XmlSerializer(typeof(XMLCompetenceProbabilities));
				var stringWriter = new StringWriter();
				using (var writer = XmlWriter.Create(stringWriter))
				{
					xmlserializer.Serialize(writer, this);
					String xml = stringWriter.ToString();

					return (xml);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred", ex);
			}
		}
			

		#endregion Methods

	}


	public class CompetenceProbability
	{
		#region Properties

		[XmlElement("id")]
		public string name { get; set; }

		[XmlElement("probability")]
		public double value { get; set; }

		#endregion
		#region Constructors

		public CompetenceProbability()
		{}

		public CompetenceProbability(string newName, double newValue)
		{
			name = newName;
			value = newValue;
		}

		#endregion
	}
}

