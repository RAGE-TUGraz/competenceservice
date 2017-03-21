using System;
using System.Collections.Generic;
using consoleTest;

namespace competenceTest
{
	/*
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

		#endregion Methods
	}
	//*/

}

