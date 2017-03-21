using System;
using System.Collections.Generic;
using consoleTest;

namespace competenceTest
{
	//*
	/// <summary>
	/// Class for storing the possible update properties/powers within the asset
	/// </summary>
	public class UpdateLevelStorage
	{
		#region Fields

		internal Dictionary<EvidencePower, ULevel> up = new Dictionary<EvidencePower, ULevel>();
		internal Dictionary<EvidencePower, ULevel> down = new Dictionary<EvidencePower, ULevel>();

		#endregion Fields
		#region Constructors

		internal UpdateLevelStorage(DomainModel dm)
		{
			if(dm.updateLevels != null && dm.updateLevels.updateLevelList != null)
			{
				foreach (UpdateLevel ul in dm.updateLevels.updateLevelList)
				{
					ULevel newLevel = new ULevel();
					newLevel.maxonelevel = ul.maxonelevel.Equals("true") ? true : false;
					newLevel.minonecompetence = ul.minonecompetence.Equals("true") ? true : false;
					newLevel.xi = Double.Parse(ul.xi);
					EvidencePower power = (ul.power.Equals("low")) ? EvidencePower.Low : (ul.power.Equals("medium")) ? EvidencePower.Medium : EvidencePower.High;
					if (ul.direction.Equals("up"))
						up.Add(power, newLevel);
					else if (ul.direction.Equals("down"))
						down.Add(power, newLevel);
				}

			}
			else
			{
				Logger.Log("No update-levels specified for the competence assessment!");
				throw new Exception("No update-levels specified for the competence assessment!");
			}
		}

		#endregion Constructors
		#region Methods
		#endregion Methods
	}

	//*/
}

