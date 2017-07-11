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
                    if (ul.maxonelevel.Equals("true", StringComparison.OrdinalIgnoreCase))
                        newLevel.maxonelevel = true;
                    else if (ul.maxonelevel.Equals("false", StringComparison.OrdinalIgnoreCase))
                        newLevel.maxonelevel = false;
                    else
                        throw new Exception();

                    if (ul.minonecompetence.Equals("true", StringComparison.OrdinalIgnoreCase))
                        newLevel.minonecompetence = true;
                    else if (ul.minonecompetence.Equals("false", StringComparison.OrdinalIgnoreCase))
                        newLevel.minonecompetence = false;
                    else
                        throw new Exception();


                    if (!Double.TryParse(ul.xi, out newLevel.xi))
                        throw new Exception();

                    EvidencePower power;
                    if (ul.power.Equals("low", StringComparison.OrdinalIgnoreCase))
                        power = EvidencePower.Low;
                    else if (ul.power.Equals("medium", StringComparison.OrdinalIgnoreCase))
                        power = EvidencePower.Medium;
                    else if (ul.power.Equals("high", StringComparison.OrdinalIgnoreCase))
                        power = EvidencePower.High;
                    else
                        throw new Exception();

                    
					if (ul.direction.Equals("up", StringComparison.OrdinalIgnoreCase))
						up.Add(power, newLevel);
					else if (ul.direction.Equals("down", StringComparison.OrdinalIgnoreCase))
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

