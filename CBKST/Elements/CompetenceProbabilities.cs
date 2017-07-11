/*
  Copyright 2016 TUGraz, http://www.tugraz.at/
  
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  This project has received funding from the European Union’s Horizon
  2020 research and innovation programme under grant agreement No 644187.
  You may obtain a copy of the License at
  
      http://www.apache.org/licenses/LICENSE-2.0
  
  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
  
  This software has been created in the context of the EU-funded RAGE project.
  Realising and Applied Gaming Eco-System (RAGE), Grant agreement No 644187, 
  http://rageproject.eu/

  Development was done by Cognitive Science Section (CSS) 
  at Knowledge Technologies Institute (KTI)at Graz University of Technology (TUGraz).
  http://kti.tugraz.at/css/

  Created by: Matthias Maurer, TUGraz <mmaurer@tugraz.at>
*/

using System;
using System.Xml.Serialization;
using System.Xml;

using System.IO;
using System.Collections.Generic;

namespace CBKST.Elements
{
	[XmlRoot("competenceprobabilities")]
	public class CompetenceProbabilities
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
		public CompetenceProbabilities(CompetenceState cs)
		{
			competenceProbabilityList = new List<CompetenceProbability> ();
			foreach (KeyValuePair<Competence, double> entry in cs.pairs)
			{
				competenceProbabilityList.Add (new CompetenceProbability(entry.Key.id,entry.Value));
			}

		}

		public CompetenceProbabilities()
		{
			competenceProbabilityList = new List<CompetenceProbability> ();
		}

		#endregion
		#region Methods

		public static CompetenceProbabilities getCPFromXmlString(String str)
		{
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CompetenceProbabilities));
                using (TextReader reader = new StringReader(str))
                {
                    CompetenceProbabilities result = (CompetenceProbabilities)serializer.Deserialize(reader);
                    return (result);
                }
            }
            catch
            {
                return null;
            }

		}

		public String toXmlString()
		{
			try
			{
				var xmlserializer = new XmlSerializer(typeof(CompetenceProbabilities));
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
		public string id { get; set; }

		[XmlElement("probability")]
		public double value { get; set; }

		#endregion
		#region Constructors

		public CompetenceProbability()
		{}

		public CompetenceProbability(string newId, double newValue)
		{
			id = newId;
			value = newValue;
		}

		#endregion
	}
}

