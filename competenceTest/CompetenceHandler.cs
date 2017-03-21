using System;

namespace competenceTest
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

		//contains information on how to perform updates
		public static UpdateLevelStorage updateLevelStorage = null;

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
		#region Methods

		/// <summary>
		/// Requests the trackingcode. A game can request a new trackingcode when
		/// supplying the domain model. A new playerid (for storing the competence state) 
		/// is created an will be stored untill deleted.
		/// </summary>
		/// <returns>
		/// -1.....domain model unknown
		/// else...The trackingcode
		/// </returns>
		/// <param name="domainmodelName">Domainmodel name.</param>
		public int requestTrackingcode(string domainmodelName){

			//request domain model for creating tracking code
			DBConnectDomainModel dbdm = new DBConnectDomainModel();
			string dmstring = dbdm.getDomainModelByName (domainmodelName);
			if ( dmstring == null)
				return -1;
			DomainModel dm = DomainModel.getDMFromXmlString (dmstring);

			//create default starting competence state

			//store competence state with userid

			//store connection between userid/domainmodelid with trackingid


			return 0;
		}

		public void storeCompetenceState(CompetenceState cs){

		}


		public void loadCompetenceState(){

		}

		#endregion
	}
}

