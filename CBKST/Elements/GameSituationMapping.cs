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
	/// <summary>
	/// Stores the mapping between game situations and related update procedure
	/// </summary>
	internal class GameSituationMapping
	{
		#region Fields

		/// <summary>
		/// Stores game situation as keys and Dictionary (Competences + ULevel) as Values 
		/// </summary>
		internal Dictionary<String, Dictionary<String, String>> mappingUp = new Dictionary<string, Dictionary<String, String>>();
		internal Dictionary<String, Dictionary<String, String>> mappingDown = new Dictionary<string, Dictionary<String, String>>();

		#endregion Fields
		#region Constructors

		internal GameSituationMapping(DomainModel dm)
		{
			if(dm.relations.situations != null && dm.relations.situations.situations != null)
			{
				foreach (SituationRelation sr in dm.relations.situations.situations)
				{
					Dictionary<String, String> newSituationMapUp = new Dictionary<string, string>();
					Dictionary<String, String> newSituationMapDown = new Dictionary<string, string>();
					foreach (CompetenceSituation cs in sr.competences)
					{
						newSituationMapUp.Add(cs.id, cs.up);
						newSituationMapDown.Add(cs.id, cs.down);
					}
					mappingUp.Add(sr.id, newSituationMapUp);
					mappingDown.Add(sr.id, newSituationMapDown);
				}
			}
		}

		#endregion Constructors
		#region Methods
        /*
		/// <summary>
		/// This Methods updates the competence based on a gamesituation and information about success/failure
		/// </summary>
		/// <param name="gamesituationId"> string representing the played game situation </param>
		/// <param name="success"> string giving information about the player's success during the game situation </param>
		internal void updateCompetenceAccordingToGamesituation(String gamesituationId, Boolean success)
		{
			//searching for the activity in the mapping
			Dictionary<String, String> competencesToUpdate;
			Dictionary<String, Dictionary<String, String>> mapping = success ? mappingUp : mappingDown;
			if (!mapping.ContainsKey(gamesituationId))
			{
				Logger.Log("The received game situation "+gamesituationId+" is unknown.");
				return;
			}

			competencesToUpdate = mapping[gamesituationId];
			UpdateLevelStorage uls = CompetenceAssessmentAsset.Handler.updateLevelStorage;

			List<String> competences = new List<string>();
			List<Boolean> evidences = new List<bool>();
			List<EvidencePower> evidencePowers = new List<EvidencePower>();
			foreach (String competence in competencesToUpdate.Keys)
			{
				competences.Add(competence);
				String ULevel = competencesToUpdate[competence];
				switch (ULevel)
				{
				case "low": evidencePowers.Add(EvidencePower.Low); break;
				case "medium": evidencePowers.Add(EvidencePower.Medium); break;
				case "high": evidencePowers.Add(EvidencePower.High); break;
				default: throw new Exception("UpdateLevel unknown!");
				}
				evidences.Add(success);
			}

			Logger.Log("Performing update based on game situation.");
			CompetenceAssessmentAsset.Handler.getCAA().updateCompetenceState(competences, evidences, evidencePowers);
		}
        */
		#endregion Methods
	}
	//*/

}

