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
using competenceTest;
using consoleTest;

namespace CompetenceAssessmentAssetNameSpace
{
	/*
	/// <summary>
	/// Singelton Class for handling Competence Assessment
	/// </summary>
	internal class CompetenceAssessmentHandler
	{
		#region AlgorithmParameters

		/// <summary>
		/// Algorithm variable for upgrading probabilities.
		/// </summary>
		internal double xi0 = 2;

		/// <summary>
		/// Algorithm variable for downgrading probabilities.
		/// </summary>
		internal double xi1 = 2;

		/// <summary>
		/// epsilon used for mantaining competence structure consistency 
		/// </summary>
		internal double epsilon = 0.000000001;

		/// <summary>
		/// Limit: Probabilities equal or higher as this value are assumed to indicate mastery of a competence by a learner 
		/// </summary>
		public double transitionProbability = 0.7;

		#endregion AlgorithmParameters
		#region Fields

		/// <summary>
		/// Dictionary containing all key/value pairs of playerId and competence structure.
		/// </summary>
		private CompetenceStructure competenceStructure = null;

		/// <summary>
		/// Structure containg the mapping between in-game activities and the related competence updates
		/// </summary>
		internal ActivityMapping activityMapping = null;

		/// <summary>
		/// Structure containg the mapping between game situations and the related competence updates for success/failure
		/// </summary>
		internal GameSituationMapping gameSituationMapping = null;

		/// <summary>
		/// Structure storing the possible update properties/powers within the asset
		/// </summary>
		internal UpdateLevelStorage updateLevelStorage = null;

		/// <summary>
		/// Dictinary containing the current competence states.
		/// </summary>
		private CompetenceState competenceState = null;

		/// <summary>
		/// If true logging is done, otherwise no logging is done.
		/// </summary>
		private Boolean doLogging = true;

		#endregion Fields
		#region Constructors 

		/// <summary>
		/// Private ctor - Singelton pattern
		/// </summary>
		public CompetenceAssessmentHandler() {}

		#endregion Constructors
		#region Properties

		/// <summary>
		/// If set to true - logging is done, otherwise no logging is done.
		/// </summary>
		public Boolean DoLogging
		{
			set
			{
				doLogging = value;
			}
			get
			{
				return doLogging;
			}
		}

		#endregion Properties
		#region InternalMethods


		/// <summary>
		/// Method for creating a competence-structure with an id (= playerId).
		/// </summary>
		/// 
		/// <param name="dm"> Specifies Domainmodel used to create the competence-structure. </param>
		///
		/// <returns>
		/// Competence-structure according to the specified domainmodel.
		/// </returns>
		internal CompetenceStructure createCompetenceStructure(DomainModel dm)
		{
			if (competenceStructure != null)
				loggingCA("CompetenceStructure already exists - overwrite!");

			CompetenceStructure cst = new CompetenceStructure(dm);
			competenceStructure = cst;
			return (cst);
		}

		/// <summary>
		/// Method for creating a competence state.
		/// </summary>
		/// 
		/// <param name="cst"> Specifies competence-structure this competence state is created for. </param>
		///
		/// <returns>
		/// Competence state according to the specified competence-structure.
		/// </returns>
		internal CompetenceState createCompetenceState(CompetenceStructure cst)
		{
			if (competenceState != null)
				loggingCA("Competence state already exists! Create new one.");

			CompetenceState cs = new CompetenceState(cst);
			competenceState = cs;
			return cs;
		}

		/// <summary>
		/// Method for performing all neccessary operations to run update methods.
		/// </summary>
		/// 
		/// <param name="dm"> Specifies the domain model used for the following registration. </param>
		internal void registerNewPlayer(DomainModel dm)
		{
			CompetenceStructure cstr = createCompetenceStructure(dm);


			createCompetenceState(cstr);
			this.updateLevelStorage = new UpdateLevelStorage(dm);
			this.gameSituationMapping = new GameSituationMapping(dm);
			this.activityMapping = new ActivityMapping(dm);

			loadCompetenceStateFromGameStorage();
		}

		/// <summary>
		/// Method for updating the competence state of a player.
		/// </summary>
		/// 
		/// <param name="playerId"> Player Id for the update - specifies for which player the competence state gets updated. </param>
		/// <param name="compList"> List of Strings - each String describes a competence.  </param>
		/// <param name="evidenceList"> Specifies if the evidences are speaking for or against the competence. </param>
		/// <param name="evidencePowers"> Contains the power of the evidence (Low,Medium,High) </param>
		internal void updateCompetenceState(List<String> compList, List<Boolean> evidenceList, List<EvidencePower> evidencePowers)
		{
			//ATTENTION: when updating more than one competence -> take mean - xi-limits may not work! [maybe replace mean by max/min]

			for (int i = 0; i < compList.Count; i++)
			{
				string evi = evidenceList[i] ? "up" : "down";
				string power = (evidencePowers[i] == EvidencePower.Low) ? "low" : (evidencePowers[i] == EvidencePower.Medium) ? "medium" : "high";
				loggingCA("updating " + compList[i] + ":" + evi+" ("+power+")");
			}

			if (competenceState == null)
			{
				loggingCA("ERROR: There is no competence state persistent!");
				return;
			}

			if (competenceStructure == null)
			{
				loggingCA("ERROR: There is no competence structure persistent!");
				return;
			}



			CompetenceState csta = competenceState;
			CompetenceStructure cstr = competenceStructure;

			//before the update, load the competence state, if needed
			loadCompetenceStateFromGameStorage();

			cstr.updateCompetenceState(csta, compList, evidenceList,  evidencePowers);

		}

		/// <summary>
		/// Method for sending the current probabilities for possessing a competence to the tracker
		/// </summary>
		internal void storeCompetenceStateToGameStorage()
		{
			CompetenceAssessmentAssetSettings caas = (CompetenceAssessmentAssetSettings) getCAA().Settings;
			String model = "CompetenceAssessmentAsset_" + caas.PlayerId + "_" + competenceStructure.domainModelId;


			CompetenceState cs =  getCompetenceState();
			Dictionary<Competence,double> competenceValues =  cs.getCurrentValues();

			//storing the data
			GameStorageClientAsset storage = getGameStorageAsset();
			foreach (Competence competence in competenceValues.Keys)
				storage[model][competence.id].Value =  competenceValues[competence];

			//storing the updated data
			storage.SaveData(model, StorageLocations.Local, SerializingFormat.Xml);
			loggingCA("Competencestate stored locally.");

			//send data to the tracker
			sendCompetenceValuesToTracker();
		}

		/// <summary>
		/// Method for loading the competence state.
		/// </summary>
		internal void loadCompetenceStateFromGameStorage()
		{
			GameStorageClientAsset storage = getGameStorageAsset();

			CompetenceAssessmentAssetSettings caas = (CompetenceAssessmentAssetSettings) getCAA().Settings;
			String model = "CompetenceAssessmentAsset_" + caas.PlayerId + "_" + competenceStructure.domainModelId;


			storage.LoadData(model, StorageLocations.Local, SerializingFormat.Xml);

			//storing data in data structure
			CompetenceState cs = getCompetenceState();
			Dictionary<Competence, double> competenceValues = cs.getCurrentValues();


			foreach (Node node in storage[model].Children)
				cs.setCompetenceValue(competenceStructure.getCompetenceById(node.Name), (double)node.Value);


			loggingCA("Competence values restored from local file.");



		}
			
		/// <summary>
		/// Method for sending competence structure to the tracker for dashboard visualisation
		/// </summary>
		internal void sendCompetenceStructureToTracker()
		{
			sendToTracker(sendCompetenceStructure);
		}

		/// <summary>
		/// Core-code for sending competence structure to the tracker for dashboard visualisation
		/// </summary>
		internal void sendCompetenceStructure()
		{
			loggingCA("Sending competence structure to the tracker.");
			tracker.Start();
			Dictionary<Competence, Double> cs = getCompetenceState().getCurrentValues();
			foreach (Competence competence in cs.Keys)
			{
				foreach(Competence prerequisite in competence.prerequisites)
				{
					tracker.setVar(competence.id+prerequisite.id, competence.id);
				}
			}
			tracker.Completable.Completed("CompetenceAssessmentAssetStructure");
		}

		/// <summary>
		/// Method for sending data to the tracker
		/// </summary>
		/// <param name="myMethod"> Method without parameters, returning void - performes individual trace sending</param>
		internal void sendToTracker(Action myMethod)
		{
			//get the tracker
			if (tracker == null)
			{
				if (AssetManager.Instance.findAssetsByClass("TrackerAsset").Count >= 1)
				{
					tracker = (TrackerAsset)AssetManager.Instance.findAssetsByClass("TrackerAsset")[0];
					loggingCA("Found tracker for tracking competences!");
				}
				else
				{
					//no tracking
					loggingCA("No tracker implemented - competence entities not send to the server");
					return;
				}
			}

			if (tracker.CheckHealth())
			{
				loggingCA(tracker.Health);
				CompetenceAssessmentAssetSettings caas = (CompetenceAssessmentAssetSettings) getCAA().Settings;
				if (tracker.Login(caas.TrackerName, caas.TrackerPassword))
				{
					loggingCA("logged in - tracker");
				}
				else
				{
					loggingCA("Maybe you forgot to store name/password for the tracker to the Competence Assessment Asset Settings.");
				}
			}

			if (tracker.Connected)
			{
				myMethod.Invoke();


				//TEST MULTITHREADING
				#if !PORTABLE
				new Thread(() =>
					{
						//next line: thread is killed after all foreground threads are dead
						Thread.CurrentThread.IsBackground = true;
						tracker.Flush();
					}).Start();
				#else
				tracker.Flush();
				#endif
			}
			else
			{
				loggingCA("Not connected to tracker.");
			}
		}

		/// <summary>
		/// Core-code for sending competence values to the tracker  
		/// </summary>
		internal void sendCompetenceValues()
		{
			loggingCA("Sending competence values to the tracker.");
			tracker.Start();
			Dictionary<Competence, Double> cs = getCompetenceState().getCurrentValues();
			foreach (Competence competence in cs.Keys)
			{
				tracker.setVar(competence.id, cs[competence].ToString());
			}
			tracker.Completable.Completed("CompetenceAssessmentAssetValues");
		}

		/// <summary>
		/// Method for sending the competence state to the tracker
		/// </summary>
		internal void sendCompetenceValuesToTracker()
		{
			sendToTracker(sendCompetenceValues);
		}

		/// <summary>
		/// Returns the competence state of the player.
		/// </summary>
		/// 
		/// <returns> Competence state of the player. </returns>
		internal CompetenceState getCompetenceState()
		{
			if (competenceState== null)
			{
				loggingCA("Player not associated with a competence state.");
				return null;
			}

			return competenceState;
		}

		/// <summary>
		/// Method for resetting the current competence state to the starting competence state
		/// </summary>
		public void resetCompetenceState()
		{
			//registerNewPlayer(getDMA().getDomainModel());
			String model = "CompetenceAssessmentAsset_" + ((CompetenceAssessmentAssetSettings) getCAA().Settings).PlayerId + "_" + competenceStructure.domainModelId;
			//getCAA().getCompetenceState();
			CompetenceState cs = new CompetenceState(new CompetenceStructure(getDMA().getDomainModel()));
			foreach (Competence competence in cs.getCurrentValues().Keys)
				gameStorage[model][competence.id].Value = cs.getCurrentValues()[competence];

			//storing the updated data
			gameStorage.SaveData(model, StorageLocations.Local, SerializingFormat.Xml);
			loadCompetenceStateFromGameStorage();
			loggingCA("Competencestate reset.");
			//registerNewPlayer(getDMA().getDomainModel());

		}

		#endregion InternalMethods
		#region TestMethods

		/// <summary>
		/// Method for diagnostic logging.
		/// </summary>
		/// 
		/// <param name="msg"> Message to be logged. </param>
		internal void loggingCA(String msg)
		{
			if (DoLogging)
			{
				Logger.Log("[CAA]: " + msg);
			}
		}

		#endregion TestMethods

	}
		





	//*/
}

