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
using System.Collections.Generic;

namespace CBKST.Elements
{
	//*
	public class CompetenceState
	{
		#region Fields

		/// <summary>
		/// Dictionary containing the key/value pairs of competences and probability of possession assosiated with the competence
		/// </summary>
		public Dictionary<Competence, double> pairs = new Dictionary<Competence, double>();

		/// <summary>
		/// Limit: Values of CompetenceAssessmentAsset.Handler.transitionProbability and above are assumed to indicate mastery of a competence by a learner 
		/// </summary>
		public double transitionProbability = CompetenceHandler.transitionProbability;

		#endregion Fields
		#region Constructors

		/// <summary>
		/// CompetenceState ctor
		/// </summary>
		/// 
		/// <param name="cst"> Spezifies CompetenceStructure this CompetenceState is created for. </param>
		public CompetenceState(CompetenceStructure cst)
		{
			setInitialCompetenceState(cst);
		}

        public CompetenceState()
        {
            pairs = new Dictionary<Competence, double>();
        }

		#endregion Constructors
		#region Methods

		/// <summary>
		/// Method for accessing the current competence-state values.
		/// </summary>
		///
		/// <returns>
		/// Dictionary containing Competences and the probability of possessing them.
		/// </returns>
		public Dictionary<Competence, double> getCurrentValues()
		{
			return (pairs);
		}

		/// <summary>
		/// Method for accessing the probability of possessing a competence.
		/// </summary>
		/// 
		/// <param name="competence"> Spezifies for which competence the probability is returned. </param>
		///
		/// <returns>
		/// Probability of possessing the spezified competence.
		/// </returns>
		public double getValue(Competence competence)
		{
			return (pairs[competence]);
		}

		/// <summary>
		/// Method for accessing the probability of possessing a competence.
		/// </summary>
		/// 
		/// <param name="competence"> String- specifying for which competence the probability is returned. </param>
		///
		/// <returns>
		/// Probability of possessing the specified competence - if available; -1 otherwise.
		/// </returns>
		public double getValue(String competenceId)
		{
			foreach (KeyValuePair<Competence, double> entry in pairs)
			{
				if (entry.Key.id == competenceId)
					return (entry.Value);
			}
			return (-1.0);
		}


		/// <summary>
		/// Method for setting an initial competence state according to "Remarks on the Simplified Update Rule - Cord Hockemeyer"
		/// </summary>
		/// 
		/// <param name="cst"> Spezifies the competence structure for which the competence state is created. </param>
		private void setInitialCompetenceState(CompetenceStructure cst)
		{
			Dictionary<string, double> ups = new Dictionary<string, double>();
			Dictionary<string, double> downs = new Dictionary<string, double>();

			//initialize upsets/downsets
			foreach (Competence com in cst.competences)
			{
				ups[com.id] = 0.0;
				downs[com.id] = 0.0;
			}

			//calculate upsets/downsets
			foreach (Competence com1 in cst.competences)
			{
				foreach (Competence com2 in cst.competences)
				{
					if (com1.isIndirectPrerequesiteOf(com2))
					{
						ups[com1.id] = ups[com1.id] + 1.0;
						downs[com2.id] = downs[com2.id] + 1.0;
					}
				}
			}

			foreach (Competence com in cst.competences)
			{
				pairs[com] = (cst.competences.Count + ups[com.id] - downs[com.id] + 1.0) / (2.0 * cst.competences.Count + 2.0);
			}

		}

		/// <summary>
		/// Diagnostic method for displaying competence state
		/// </summary>
		public void print()
		{
			Logger.Log("Competence State:");
			//Logger.Log("=================");
			String str = "";
			foreach (var pair in pairs)
			{
				str += "(" + pair.Key.id + ":" + Math.Round(pair.Value, 2) + ")";
				//Logger.Log("Key: " + pair.Key.id + " Value: " + Math.Round(pair.Value,2));
			}
			Logger.Log(str);

		}

		/// <summary>
		/// Methd for printing out only the mastered competences
		/// </summary>
		public void printMasteredCompetences()
		{
			Logger.Log("Competences mastered:");
			//Logger.Log("=================");
			String str = "";
			foreach (var pair in this.getMasteredCompetences())
			{
				str += "(" + pair.id + ":" + Math.Round(this.getValue(pair.id), 2) + ")";
				//Logger.Log("Key: " + pair.Key.id + " Value: " + Math.Round(pair.Value,2));
			}
			Logger.Log(str);
		}

		/// <summary>
		/// Method for setting a competence probability by competence name
		/// </summary>
		/// 
		/// <param name="str"> String- specifying for which competence the probability is set. </param>
		/// <param name="v"> Probability value </param>
		private void setCompetenceValue(string str, double v)
		{
			foreach (KeyValuePair<Competence, double> entry in pairs)
			{
				if (entry.Key.id == str)
					pairs[entry.Key] = v; ;
			}

		}

		/// <summary>
		/// Method for setting a competence probability by competence 
		/// </summary>
		/// 
		/// <param name="str"> Specifying for which competence the probability is set. </param>
		/// <param name="v"> Probability value </param>
		public void setCompetenceValue(Competence com, double v)
		{
			pairs[com] = v; ;
		}

		/// <summary>
		/// Returns all mastered Competences.
		/// </summary>
		/// 
		/// <returns> List of all Competences which are assumed to be mastered. </returns>
		public List<Competence> getMasteredCompetences()
		{
			List<Competence> mastered = new List<Competence>();

			foreach (KeyValuePair<Competence, double> entry in pairs)
			{
				if (entry.Value >= transitionProbability)
					mastered.Add(entry.Key);
			}

			return mastered;
		}

		/// <summary>
		/// Returns all mastered Competences as string-list.
		/// </summary>
		/// 
		/// <returns> List of all Competences as strings which are assumed to be mastered. </returns>
		public List<String> getMasteredCompetencesString()
		{
			List<Competence> mastered = getMasteredCompetences();
			List<String> masteredString = new List<string>();
			foreach (Competence com in mastered)
				masteredString.Add(com.id);

			return masteredString;
		}

		#endregion Methods

	}

	//*/
}

