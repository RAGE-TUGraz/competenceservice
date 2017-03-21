﻿using System;
using System.Collections.Generic;
using consoleTest;

namespace competenceTest
{
	/*
	/// <summary>
	/// Stores the mapping between in-game activities and related update procedure
	/// </summary>
	internal class ActivityMapping
	{
		#region Fields

		/// <summary>
		/// Stores activities as keys and Dictionary (Competences + Array(ULevel+up/down)) as Values 
		/// </summary>
		internal Dictionary<String, Dictionary<String, String[]>> mapping = new Dictionary<string, Dictionary<String, String[]>>();

		#endregion Fields
		#region Constructors

		internal ActivityMapping(DomainModel dm)
		{
			if(dm.relations.activities != null && dm.relations.activities.activities != null)
			{
				foreach (ActivitiesRelation ac in dm.relations.activities.activities)
				{
					Dictionary<String, String[]> newActivityMap = new Dictionary<string, string[]>();
					foreach (CompetenceActivity cac in ac.competences)
						newActivityMap.Add(cac.id, new string[] { cac.power, cac.direction });
					mapping.Add(ac.id, newActivityMap);
				}
			}
		}

		#endregion Constructors
		#region Methods

		/// <summary>
		/// This Methods updates the competence based on an observed activity
		/// </summary>
		/// <param name="activity"> string representing the observed activity </param>
		internal void updateCompetenceAccordingToActivity(String activity)
		{
			//searching for the activity in the mapping
			Dictionary<String, String[]> competencesToUpdate;
			if (!mapping.ContainsKey(activity))
			{
				Logger.Log("The received activity " + activity + " is unknown.");
				return;
			}

			competencesToUpdate = mapping[activity];
			UpdateLevelStorage uls =  CompetenceAssessmentAsset.Handler.updateLevelStorage;

			List<String> competences = new List<string>();
			List<Boolean> evidences = new List<bool>();
			List<EvidencePower> evidencePowers = new List<EvidencePower>();
			foreach(String competence in competencesToUpdate.Keys)
			{
				competences.Add(competence);
				String[] ULevelDirection = competencesToUpdate[competence];
				switch (ULevelDirection[0])
				{
				case "low":  evidencePowers.Add(EvidencePower.Low); break; 
				case "medium":  evidencePowers.Add(EvidencePower.Medium); break; 
				case "high":  evidencePowers.Add(EvidencePower.High); break; 
				default: throw new Exception("UpdateLevel unknown!");
				}
				switch (ULevelDirection[1])
				{
				case "up": evidences.Add(true); break;
				case "down": evidences.Add(false); break;
				default: throw new Exception("Updatedirection unknown!");
				}
			}

			Logger.Log("Performing update based on activity '"+activity+"'.");
			CompetenceAssessmentAsset.Handler.getCAA().updateCompetenceState(competences, evidences, evidencePowers);

		}

		#endregion Methods
	}
	//*/
}

