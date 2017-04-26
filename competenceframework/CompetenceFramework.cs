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

using storagedb;
using CBKST;
using CBKST.Elements;
using System.Collections.Generic;
using System;

namespace competenceframework
{
    public class CompetenceFramework
    {
        #region Parameters

        #endregion
        #region Fields

        /// <summary>
        /// Instance of the class DatabaseHandler - Singelton pattern
        /// </summary>
        static readonly CompetenceFramework instance = new CompetenceFramework();

        #endregion
        #region Constructor
        private CompetenceFramework()
        {
        }
        #endregion
        #region Properties

        /// <summary>
        /// Getter for Instance of the DatabaseHandler - Singelton pattern
        /// </summary>
        public static CompetenceFramework Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
        #region Utilitymethods

        /// <summary>
        /// Method resetting the whole storage
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public static bool resetStorage()
        {
            return DatabaseHandler.Instance.resetDatabases();
        }

        /// <summary>
        /// Method for creating/storing testdata
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public static bool createTestdata()
        {
            return DatabaseHandler.Instance.createTestdata();
        }

        #endregion
        #region Methods

        /// <summary>
        /// Method for storing a domainmodel, an id for this domainmodel is returned
        /// </summary>
        /// <param name="dm"> domain model to be stored</param>
        /// <returns> null if unsuccessful, the id otherwise</returns>
        public static string storedm(string dm)
        {
            //structure name/password omitted in this version
            return DatabaseHandler.Instance.insertdomainmodel("","", dm);
        }

        /// <summary>
        /// Method for deleting a domainmodel and all related competencestates and trackingids
        /// </summary>
        public static bool deletedm(string dmid)
        {
            List<string> trackingIdsAffected = DatabaseHandler.Instance.getTrackingIdsToDomainModelId(dmid);
            foreach(string tid in trackingIdsAffected)
            {
                if (!deletetid(tid))
                    return false;
            }

            bool success = DatabaseHandler.Instance.deleteDomainModelById(dmid);
            
            return success;
        }

        /// <summary>
        /// Method for getting the domain model for a given domain model id (dmid)
        /// </summary>
        /// <param name="dmid"> id of the requested domain model</param>
        /// <returns> xml representation of the domain model if it exists, null otherwise</returns>
        public static string getdm(string dmid)
        {
            return DatabaseHandler.Instance.getDomainModelById(dmid);
        }

        /// <summary>
        /// Method for creating a trackingid for a given domainmodel speziffied by id (dmid)
        /// </summary>
        /// <param name="dmid"> id of the domain model for which the tracking id is created</param>
        /// <returns>null if an error occured, the tracking id otherwise</returns>
        public static string createtrackingid(string dmid)
        {
            string tid = DatabaseHandler.Instance.createTrackingId(dmid);
            return tid;
        }

        /// <summary>
        /// Method for updating the competence state of a payer by trackingid
        /// </summary>
        /// <param name="tid"> tracking id of the player</param>
        /// <param name="evidence"> xml representation of the evidence </param>
        /// <returns>true if the update is successful, false otherwise</returns>
        public static bool updatecompetencestate(string tid, string evidence)
        {
            //get dmid/csid to tracking id
            string[] ids = DatabaseHandler.Instance.getDomainmodelIdAndCompetenceProbabilityId(tid);
            if (ids == null)
                return false;

            //load dm/cp
            string dmstring = getdm(ids[0]);
            string cpstring = getcpByCpId(ids[1]);
            if (dmstring == null || cpstring == null)
                return false;

            //perform update
            DomainModel dm = DomainModel.getDMFromXmlString(dmstring);
            CompetenceProbabilities cp = CompetenceProbabilities.getCPFromXmlString(cpstring);
            EvidenceSet es = EvidenceSet.getESFromXmlString(evidence);
            CompetenceProbabilities newcp = CompetenceHandler.Instance.updateCompetenceState(dm,cp,es);

           
            //store cs
            return DatabaseHandler.Instance.performCompetenceProbabilityUpdate(ids[1],newcp.toXmlString());
        }

        /// <summary>
        /// Method for deleting a tracing id and the related competence state
        /// </summary>
        /// <param name="tid"> tracking id to delete</param>
        /// <returns>true if successful, false otherwise</returns>
        public static bool deletetid(string tid)
        {
            return DatabaseHandler.Instance.deleteTrackingId(tid);
        }

        /// <summary>
        /// Method for returning the competence state of a player by tracking id
        /// </summary>
        /// <param name="tid"> tracking id of the player</param>
        /// <returns>null if there is an error, otherwise a xml representation of the player's competence state</returns>
        public static string getcpByTid(string tid)
        {
            return DatabaseHandler.Instance.getCompetenceProbabilitiesByTrackingId(tid);
        }

        /// <summary>
        /// Method for returning the competence state of a player by tracking id
        /// </summary>
        /// <param name="tid"> tracking id of the player</param>
        /// <returns>null if there is an error, otherwise a xml representation of the player's competence state</returns>
        public static string getcpByCpId(string cpid)
        {
            return DatabaseHandler.Instance.getCompetenceProbabilitiesByCpId(cpid);
        }

        #endregion Methods
    }
}
