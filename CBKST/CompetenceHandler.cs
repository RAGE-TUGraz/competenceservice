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

using CBKST.Elements;
using System;
using System.Collections.Generic;

namespace CBKST
{
	public class CompetenceHandler
	{
		#region Fields
		/// <summary>
		/// Instance of the class DomainModelHandler - Singelton pattern
		/// </summary>
		static readonly CompetenceHandler instance = new CompetenceHandler();

		//probability value for which a competence is assumed to be mastered
		public static double transitionProbability = 0.7;

		//algorithm parameter for updates
		public static double xi0 = 2.0;
		public static double xi1 = 2.0;
		public static double epsilon = 0.0000001;

		#endregion
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the CompetenceHandler class.
		/// </summary>
		private CompetenceHandler()
		{
		}

		#endregion 
		#region Properties

		/// <summary>
		/// Getter for Instance of the DomainModelAsset - Singelton pattern
		/// </summary>
		public static CompetenceHandler Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion 
        #region Publicmethods

        /// <summary>
        /// Method for performing an update of competences within a domain model with given possession probabilities of the competences and observed evidence.
        /// </summary>
        /// <param name="domainModel"> Structure defining the relationship between competences</param>
        /// <param name="competenceProbabilities"> Structure holding information about the probability of possession of each competence</param>
        /// <param name="evidenceSet">Information on possession or lack of which competences was observed</param>
        /// <returns> The updated probability of possession of each competence</returns>
        public CompetenceProbabilities updateCompetenceState(DomainModel domainModel, CompetenceProbabilities competenceProbabilities, EvidenceSet evidenceSet)
        {
            //structure for updating the competence probability vector
            CompetenceStructure competenceStructure = new CompetenceStructure(domainModel);

            //components needed for the update - gained from the Evidence Set
            List<string> competenceIdList = new List<string>();
            List<bool> evidenceDirectionList = new List<bool>();
            List<EvidencePower> evidencePowerList = new List<EvidencePower>();
            ActivityMapping am = new ActivityMapping(domainModel);
            GameSituationMapping gsm = new GameSituationMapping(domainModel);
            foreach (Evidence evidence in evidenceSet.evidences)
            {
                if (evidence.type == EvidenceType.Competence)
                {
                    //update based on a competence
                    competenceIdList.Add(evidence.competenceId);
                    evidenceDirectionList.Add(evidence.direction);
                    evidencePowerList.Add(evidence.evidencePower);
                }else if(evidence.type == EvidenceType.Activity)
                {
                    //update based on an activity mapping
                    if(am.mapping == null)
                        throw new Exception("Activity mapping not found in domain model while updating based on an activity!");
                    if (!am.mapping.ContainsKey(evidence.activity))
                        throw new Exception("The received activity " + evidence.activity + " is not included in the activity mapping in the domain model!");

                    Dictionary<String, String[]> competencesToUpdate = am.mapping[evidence.activity];

                    foreach (String competence in competencesToUpdate.Keys)
                    {
                        competenceIdList.Add(competence);
                        String[] ULevelDirection = competencesToUpdate[competence];
                        switch (ULevelDirection[0])
                        {
                            case "low": evidencePowerList.Add(EvidencePower.Low); break;
                            case "medium": evidencePowerList.Add(EvidencePower.Medium); break;
                            case "high": evidencePowerList.Add(EvidencePower.High); break;
                            default: throw new Exception("UpdateLevel '"+ ULevelDirection[0] + "' unknown! (low/medium/high)");
                        }
                        switch (ULevelDirection[1])
                        {
                            case "up": evidenceDirectionList.Add(true); break;
                            case "down": evidenceDirectionList.Add(false); break;
                            default: throw new Exception("Updatedirection '"+ ULevelDirection[1] + "' unknown! (up/down)");
                        }
                    }
                }
                else if(evidence.type == EvidenceType.Gamesituation)
                {
                    //update based on a gamesituation
                    if (gsm.mappingDown == null || gsm.mappingUp == null)
                        throw new Exception("Gamesituation mapping not found in domain model while updating based on a gamesituation!");

                    Dictionary<String, String> competencesToUpdate;
                    Dictionary<String, Dictionary<String, String>> mapping = evidence.direction ? gsm.mappingUp : gsm.mappingDown;
                    if (!mapping.ContainsKey(evidence.gamesituationId))
                        throw new Exception("The received gamesituation id " + evidence.gamesituationId + " is not included in the gamesituation mapping in the domain model!");

                    competencesToUpdate = mapping[evidence.gamesituationId];
                    foreach (String competence in competencesToUpdate.Keys)
                    {
                        competenceIdList.Add(competence);
                        String ULevel = competencesToUpdate[competence];
                        switch (ULevel)
                        {
                            case "low": evidencePowerList.Add(EvidencePower.Low); break;
                            case "medium": evidencePowerList.Add(EvidencePower.Medium); break;
                            case "high": evidencePowerList.Add(EvidencePower.High); break;
                            default: throw new Exception("UpdateLevel unknown!");
                        }
                        evidenceDirectionList.Add(evidence.direction);
                    }
                }
                else
                {
                    throw new Exception("Evidence type not specified correctly! (Competence/Activity/Gamesituation)");
                }
            }

            UpdateLevelStorage uls = new UpdateLevelStorage(domainModel);

            //the update
            CompetenceProbabilities cpnew =  competenceStructure.updateCompetenceState(competenceProbabilities, competenceIdList, evidenceDirectionList, evidencePowerList, uls);


            return cpnew;
        }

        /// <summary>
        /// Method creating an initial CompetenceProbabilities structure
        /// </summary>
        /// <param name="dm"> Structure defining the relationship between competences </param>
        /// <returns></returns>
        public CompetenceProbabilities createInitialCompetenceProbabilities(DomainModel dm)
        {
            CompetenceStructure compstruct = new CompetenceStructure(dm);
            CompetenceState cs = new CompetenceState(compstruct);
            return new CompetenceProbabilities(cs);
        }


        #endregion
    }
}

