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
using System.Linq;

namespace CBKST.Elements
{
	/// <summary>
	/// Class representing the Competence-Tree of the Domainmodel. 
    /// Used for Updates of competence probabilities. 
	/// </summary>
	public class CompetenceStructure
	{
		#region Fields 

		/// <summary>
		/// Domainmodel-ID, consistent of concatenation of all competences in lexicographic order
		/// </summary>
		internal String domainModelId;

		/// <summary>
		/// List of competences forming the competence-structure
		/// </summary>
		internal List<Competence> competences = new List<Competence>();

		/// <summary>
		/// Algorithm-parameters for updating a competence state áccording to this competence-structure.
		/// </summary>
		private double xi0 = CompetenceHandler.xi0;
		private double xi1 = CompetenceHandler.xi1;
		private double epsilon = CompetenceHandler.epsilon;

		#endregion Fields
		#region Constructors

		/// <summary>
		/// Constructor using a DomainModel.
		/// </summary>
		/// 
		/// <param name="dm"> DomainModel which is used to create the CompetenceStructure. </param>
		public CompetenceStructure(DomainModel dm)
		{

			//adding competences
			foreach (CompetenceDesc comd in dm.elements.competences.competenceList)
			{
				competences.Add(new Competence(comd.id, comd.title, this));
			}

			//adding prerequisites and successors
			foreach (CompetenceP comp in dm.relations.competenceprerequisites.competences)
			{
				foreach(Prereqcompetence pcom in comp.prereqcompetences)
				{
					getCompetenceById(comp.id).addPrerequisite(getCompetenceById(pcom.id));
					getCompetenceById(pcom.id).addSuccessor(getCompetenceById(comp.id));
				}
			}

			List<String> competenceNames = new List<string>();
			foreach (Competence comp in this.competences)
				competenceNames.Add(comp.id);
			competenceNames.Sort();

			domainModelId = "";
			foreach (String id in competenceNames)
				domainModelId +="&"+id;
		}

		#endregion Constructors
		#region Methods

		/// <summary>
		/// Method for getting Competence by ID from competence structure.
		/// </summary>
		/// 
		/// <param name="id"> Unique competence-id within a competence structure. </param>
		///
		/// <returns>
		/// Competence specified by the given id.
		/// </returns>
		public Competence getCompetenceById(String id)
		{
			foreach (Competence com in competences)
			{
				if (com.id.Equals(id))
				{
					return (com);
				}
			}
			return (null);
		}

		/// <summary>
		/// Diagnostic-method for displaying the competence structure
		/// </summary>
		public void print()
		{
			Logger.Log("Printing competence-structure:");
			Logger.Log("==============================");

			foreach (Competence com in competences)
			{
				com.print();
			}
		}

		/// <summary>
		/// Method for updating a competence state with a set of evidences.
		/// </summary>
		/// 
		/// <param name="cs"> Specifies competence state to update. </param>
		/// <param name="compList"> Speciefies for which Competences evidences are observed. </param>
		/// <param name="evidenceList"> Specifies if evidences are observed for (true) or against (false) possessing a competence. </param>
		/// <param name="evidencePowers"> Algorithm parameter for updating competence probabilities -> defines xi values and update power </param>
		internal void updateCompetenceState(CompetenceState cs, List<Competence> compList, List<Boolean> evidenceList, List<EvidencePower> evidencePowers, UpdateLevelStorage uls)
		{
			Dictionary<string, double> sum = new Dictionary<string, double>();

			//initialise all sum-values with zero
			foreach (Competence comp in cs.getCurrentValues().Keys.ToList())
			{
				sum[comp.id] = 0.0;
			}

			Dictionary<string, double> tmp;
			for (int i = 0; i < compList.Count; i++)
			{
				tmp = updateCompetenceStateWithOneEvidence(cs, compList[i], evidenceList[i], evidencePowers[i], uls);

				foreach (Competence comp in cs.getCurrentValues().Keys.ToList())
				{
					sum[comp.id] = sum[comp.id] + tmp[comp.id];
				}
			}

			foreach (Competence comp in cs.getCurrentValues().Keys.ToList())
			{
				cs.setCompetenceValue(comp, sum[comp.id] / compList.Count);
			}
        }

		/// <summary>
		/// Method for updating a competence state with a set of evidences.
		/// </summary>
		/// 
		/// <param name="cs"> Specifies competence state to update. </param>
		/// <param name="compList"> Speciefies for which Competences (by id) evidences are observed. </param>
		/// <param name="evidenceList"> Specifies if evidences are observed for (true) or against (false) possessing a competence. </param>
		/// <param name="xi0List"> Algorithm parameter for updating competence probabilities. </param>
		/// <param name="xi1List"> Algorithm parameter for updating competence probabilities. </param>
		/// <param name="additionalInformation"> Specifies if updating a competence is able to get a successor-competence in the competence state or for sure removes a prerequisite competence from the competence state by modifying xi0 or xi1.</param>
		internal void updateCompetenceState(CompetenceState cs, List<String> compList, List<Boolean> evidenceList, List<EvidencePower> evidencePowers, UpdateLevelStorage uls)
		{
			List<Competence> cList = new List<Competence>();
			foreach (String str in compList)
			{
				if (getCompetenceById(str) != null)
					cList.Add(getCompetenceById(str));
			}

			updateCompetenceState(cs, cList, evidenceList, evidencePowers, uls);
        }

        /// <summary>
        /// Method for updating a competence state with a set of evidences.
        /// </summary>
        /// 
        /// <param name="cs"> Specifies competence state to update. </param>
        /// <param name="compList"> Speciefies for which Competences (by id) evidences are observed. </param>
        /// <param name="evidenceList"> Specifies if evidences are observed for (true) or against (false) possessing a competence. </param>
        /// <param name="xi0List"> Algorithm parameter for updating competence probabilities. </param>
        /// <param name="xi1List"> Algorithm parameter for updating competence probabilities. </param>
        /// <param name="additionalInformation"> Specifies if updating a competence is able to get a successor-competence in the competence state or for sure removes a prerequisite competence from the competence state by modifying xi0 or xi1.</param>
        internal CompetenceProbabilities updateCompetenceState(CompetenceProbabilities cp, List<String> compList, List<Boolean> evidenceList, List<EvidencePower> evidencePowers, UpdateLevelStorage uls)
        {
            CompetenceState cs = new CompetenceState();
            foreach (CompetenceProbability competenceProbability in cp.competenceProbabilityList)
            {
                if (getCompetenceById(competenceProbability.id) != null)
                    cs.setCompetenceValue(getCompetenceById(competenceProbability.id), competenceProbability.value);
                else
                {
                    return null;
                    //throw new Exception("A supplied competence probability id do not match a domain model competence id!");
                }
            }

            updateCompetenceState(cs, compList, evidenceList, evidencePowers, uls);

            return new CompetenceProbabilities(cs);
        }

        /// <summary>
        /// Method for updating a competence state with one evidence.
        /// </summary>
        /// 
        /// <param name="cs"> Specifies competence state to update. </param>
        /// <param name="com"> Specifies for which competence an evidence is available. </param>
        /// <param name="evidence"> Specifies if the evidence indicates possesion (true) of the competence or not (false). </param>
        /// <param name="newXi0"> Algorithm parameter for updating the competence-probabilities. </param>
        /// <param name="newXi1"> Algorithm parameter for updating the competence-probabilities. </param>
        /// <param name="additionalInformation"> Specifies if updating a competence is able to get a successor-competence in the competence state or for sure removes a prerequisite competence from the competence state by modifying xi0 or xi1.</param>
        ///
        /// <returns>
        /// Dictionary with key/value pairs of competence-id and updated probability of pessesing the competence. 
        /// </returns>
        internal Dictionary<string, double> updateCompetenceStateWithOneEvidence(CompetenceState cs, Competence com, Boolean evidence, EvidencePower evidencePower, UpdateLevelStorage uls)
		{
            //set update parameter
            ULevel ulevel = evidence ? uls.up[evidencePower] : uls.down[evidencePower];


			Dictionary<string, double> pairs = new Dictionary<string, double>();
			Double denominator;

			//additionaInformation structure: {downgrading->lose a competence for sure?, upgrading->gaine a competence for sure?, upgrading-> is it possible to gaine more than one competence?}
			double[] updateValues = getUpdateValues(ulevel, evidence, cs, com);
			double newXi0 = updateValues[0];
			double newXi1 = updateValues[1];

			//starting the update procedure
			foreach (Competence comp in cs.getCurrentValues().Keys.ToList())
			{
				pairs[comp.id] = 0.0;
			}

			if (evidence)
				denominator = newXi0 * cs.getValue(com.id) + (1 - cs.getValue(com.id));
			else
				denominator = cs.getValue(com.id) + newXi1 * (1 - cs.getValue(com.id));

			foreach (Competence competence in this.competences)
			{
				if (com.isIndirectPrerequesiteOf(competence) && com.id != competence.id)
				{
					if (evidence)
						pairs[competence.id] = (newXi0 * cs.getValue(competence.id)) / denominator;
					else
						pairs[competence.id] = cs.getValue(competence.id) / denominator;
				}
				else if (competence.isIndirectPrerequesiteOf(com))
				{
					if (evidence)
						pairs[competence.id] = (newXi0 * cs.getValue(com.id) + (cs.getValue(competence.id) - cs.getValue(com.id))) / denominator;
					else
						pairs[competence.id] = (cs.getValue(com.id) + newXi1 * (cs.getValue(competence.id) - cs.getValue(com.id))) / denominator;
				}
				else
				{
					pairs[competence.id] = cs.getValue(competence.id);
				}
			}

			checkConsistency(pairs, evidence);

			return (pairs);
		}

		/// <summary>
		/// Method for adapting the xi-values to the given additional information about the update
		/// </summary>
		/// <param name="ulevel"> Update information (original xi values and additional information)</param>
		/// <param name="evidence"> indicates if there is an up- or downgrade</param>
		/// <param name="cs"> competence state</param>
		/// <param name="com">competence, which gets updated</param>
		/// <returns> the adopted xi values </returns>
		private double[] getUpdateValues(ULevel ulevel, Boolean evidence, CompetenceState cs, Competence com)
		{

			List<Competence> possibleCompetencesToShiftMinOneLevel = new List<Competence>();
			Boolean isCompetenceMastered = cs.getMasteredCompetences().Contains(com);

			double newXi0 = ulevel.xi;
			double newXi1 = ulevel.xi;
			double xi0 = newXi0;
			double xi1 = newXi1;

			//add competence for minonelevel-property 
			if (evidence && (ulevel.minonecompetence || ulevel.maxonelevel))
			{
				if (!isCompetenceMastered)
				{
					List<Competence> candidatesToShift = new List<Competence>();
					candidatesToShift.Add(com);

					List<Competence> prerequisitesNotMastered;
					while (candidatesToShift.Count > 0)
					{
						prerequisitesNotMastered = candidatesToShift[0].getPrerequisitesNotMastered(cs);
						if (prerequisitesNotMastered.Count == 0)
							possibleCompetencesToShiftMinOneLevel.Add(candidatesToShift[0]);
						else
							foreach (Competence c in prerequisitesNotMastered)
								candidatesToShift.Add(c);
						candidatesToShift.RemoveAt(0);
					}
				}
				else
				{
					List<Competence> candidatesToGetShiftElements = new List<Competence>();
					candidatesToGetShiftElements.Add(com);
					while (candidatesToGetShiftElements.Count > 0)
					{
						foreach (Competence c in candidatesToGetShiftElements[0].successors)
						{
							if (cs.getMasteredCompetences().Contains(c))
								candidatesToGetShiftElements.Add(c);
							else
								if (c.allPrerequisitesMet(cs))
									possibleCompetencesToShiftMinOneLevel.Add(c);
						}
						candidatesToGetShiftElements.RemoveAt(0);
					}
				}
			}
			else if ((!evidence) && (ulevel.minonecompetence || ulevel.maxonelevel))
			{
				if (!isCompetenceMastered)
				{
					List<Competence> candidatesToGetShiftElements = new List<Competence>();
					candidatesToGetShiftElements.Add(com);
					while (candidatesToGetShiftElements.Count > 0)
					{
						foreach (Competence c in candidatesToGetShiftElements[0].prerequisites)
						{
							if (!cs.getMasteredCompetences().Contains(c))
								candidatesToGetShiftElements.Add(c);
							else
								possibleCompetencesToShiftMinOneLevel.Add(c);
						}
						candidatesToGetShiftElements.RemoveAt(0);
					}
				}
				else
				{
					List<Competence> candidateShiftElements = new List<Competence>();
					candidateShiftElements.Add(com);

					List<Competence> successorsMastered;
					while (candidateShiftElements.Count > 0)
					{
						successorsMastered = new List<Competence>();
						foreach (Competence c in candidateShiftElements[0].successors)
						{
							if (cs.getMasteredCompetences().Contains(c))
								successorsMastered.Add(c);
						}
						if (successorsMastered.Count == 0)
							possibleCompetencesToShiftMinOneLevel.Add(candidateShiftElements[0]);
						else
							foreach (Competence succomp in successorsMastered)
								candidateShiftElements.Add(succomp);
						candidateShiftElements.RemoveAt(0);
					}
				}
			}

			//upgrading->gaine a competence for sure?
			if (ulevel.minonecompetence && evidence && possibleCompetencesToShiftMinOneLevel.Count > 0)
			{
				double lowestXiNeededForUpdate = 0;
				double currentXiNeededForUpdate;
				foreach (Competence competence in possibleCompetencesToShiftMinOneLevel)
				{
					currentXiNeededForUpdate = competence.calculateXi(com, cs.transitionProbability + epsilon, cs, evidence);
					if (lowestXiNeededForUpdate==0 || (lowestXiNeededForUpdate > currentXiNeededForUpdate))
						lowestXiNeededForUpdate = currentXiNeededForUpdate;
				}
				newXi0 = Math.Max(lowestXiNeededForUpdate, newXi0);
			}

			//downgrading->lose a competence for sure?
			if (ulevel.minonecompetence && (!evidence) && possibleCompetencesToShiftMinOneLevel.Count > 0)
			{
				double lowestXiNeededForUpdate = 0;
				double currentXiNeededForUpdate;
				foreach (Competence competence in possibleCompetencesToShiftMinOneLevel)
				{
					currentXiNeededForUpdate = competence.calculateXi(com, cs.transitionProbability - epsilon, cs, evidence);
					if (lowestXiNeededForUpdate == 0 || (lowestXiNeededForUpdate > currentXiNeededForUpdate))
						lowestXiNeededForUpdate = currentXiNeededForUpdate; 
				}
				newXi1 = Math.Max(lowestXiNeededForUpdate, newXi1);
			}

			//handling maxonelevel-property
			if (ulevel.maxonelevel && possibleCompetencesToShiftMinOneLevel.Count > 0)
			{
				List<Competence> possibleCompetencesToShiftMaxOneLevel = new List<Competence>();
				if (evidence)
				{
					foreach (Competence competence in possibleCompetencesToShiftMinOneLevel)
						foreach (Competence comp in competence.getSuccessorsWithAllPrerequisitesMasteredButThis(cs))
							if ((!possibleCompetencesToShiftMaxOneLevel.Contains(comp)) && (com.isIndirectPrerequesiteOf(comp) || comp.isIndirectPrerequesiteOf(com)))
								possibleCompetencesToShiftMaxOneLevel.Add(comp);
				}
				else
				{
					foreach (Competence competence in possibleCompetencesToShiftMinOneLevel)
						foreach (Competence comp in competence.getPrerequisiteWithAllSuccessorsNotInCompetenceStateButThis(cs))
							if ((!possibleCompetencesToShiftMaxOneLevel.Contains(comp)) && (com.isIndirectPrerequesiteOf(comp) || comp.isIndirectPrerequesiteOf(com)))
								possibleCompetencesToShiftMaxOneLevel.Add(comp);
				}


				//upgrading->gaine not more than one competence level
				if (evidence && possibleCompetencesToShiftMaxOneLevel.Count > 0)
				{
					double maxXiAllowedForUpdate = 0;
					double currentXiAllowedForUpdate;
					foreach (Competence competence in possibleCompetencesToShiftMaxOneLevel)
					{
						currentXiAllowedForUpdate = competence.calculateXi(com, cs.transitionProbability - epsilon, cs, evidence);
						if ((maxXiAllowedForUpdate ==0 || (maxXiAllowedForUpdate > currentXiAllowedForUpdate))&& currentXiAllowedForUpdate>1)
							maxXiAllowedForUpdate = currentXiAllowedForUpdate;
					}
					newXi0 = (maxXiAllowedForUpdate > 1) ? Math.Min(maxXiAllowedForUpdate, newXi0) : newXi0;
					//newXi0 = Math.Max(newXi0, 1 + epsilon);
				}


				//downgrading->make sure to lose not more than one competence level
				if ((!evidence) && possibleCompetencesToShiftMaxOneLevel.Count > 0)
				{
					double maxXiAllowedForUpdate = 0;
					double currentXiAllowedForUpdate;
					foreach (Competence competence in possibleCompetencesToShiftMaxOneLevel)
					{
						currentXiAllowedForUpdate = competence.calculateXi(com, cs.transitionProbability + epsilon, cs, evidence);
						if ((maxXiAllowedForUpdate == 0 || (maxXiAllowedForUpdate > currentXiAllowedForUpdate)) && currentXiAllowedForUpdate > 1)
							maxXiAllowedForUpdate = currentXiAllowedForUpdate;
					}
					newXi1 = (maxXiAllowedForUpdate>1) ? Math.Min(maxXiAllowedForUpdate, newXi1) : newXi1;
					//newXi1 = Math.Max(newXi1, 1 + epsilon);
				}
			}

			//logging
			if (evidence && (xi0 != newXi0))
			{
				Logger.Log("xi0 changed from " + xi0 + " to " + newXi0 + " due to additional information.");
				if (newXi0 < 1)
					throw new Exception("Internal error Competence Assessment Asset: Value not allowed!");
			}
			else if ((!evidence) && (xi1 != newXi1))
			{
				Logger.Log("xi1 changed from " + xi1 + " to " + newXi1 + " due to additional information.");
				if (newXi1 < 1)
					throw new Exception("Internal error Competence Assessment Asset: Value not allowed!");
			}

			double[] updateValues = { newXi0,newXi1};
			return updateValues;
		}

		/// <summary>
		/// Method for checking (and restore) competence structure consistency - prerequisites must have a higher probability of beeing possessed.
		/// </summary>
		/// 
		/// <param name="pairs"> Dictionary with key/value pairs of competence-id and probability of pessessing the competence. </param>
		/// <param name="evidence"> Specifies if the evidence indicates possesion (true) of the competence or not (false). </param>
		private void checkConsistency(Dictionary<String, double> pairs, Boolean evidence)
		{
			Boolean changes = true;
			while (changes)
			{
				changes = false;
				foreach (Competence com1 in competences)
				{
					foreach (Competence com2 in competences)
					{
						if (com1.id != com2.id && com1.isIndirectPrerequesiteOf(com2) && pairs[com1.id] <= pairs[com2.id])
						{
							if (evidence)
								pairs[com1.id] = Math.Min(1 - epsilon, pairs[com2.id] + epsilon);
							else
								pairs[com2.id] = Math.Max(epsilon, pairs[com1.id] - epsilon);
							changes = true;
						}
					}
				}
			}
		}

		#endregion Methods

	}
    
}

