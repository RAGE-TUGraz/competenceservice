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
            //temp: create tracking history
            /*
            EvidenceSet es = new EvidenceSet();
            Evidence e1 = new Evidence();
            e1.type = EvidenceType.Competence;
            e1.competenceId = "C8";
            e1.direction = true;
            e1.evidencePower = EvidencePower.Medium;
            es.evidences = new List<Evidence>();
            es.evidences.Add(e1);
            updatecompetencestate("1",es.toXmlString());
            */
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

        //Method for dropping all database tables
        public static void deleteDatabaseTables()
        {
            DatabaseHandler.Instance.deleteDatabaseTables();
        }

        /// <summary>
        /// Method resetting the whole storage
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public static bool resetStorage()
        {
            return DatabaseHandler.Instance.resetDatabases();
        }

        /// <summary>
        /// This method allows to change the database access data
        /// </summary>
        /// <param name="newServer"> server name </param>
        /// <param name="newDatabase"> database name </param>
        /// <param name="newUid"> user id </param>
        /// <param name="newPassword"> user password</param>
        public static void setDatabaseAccessData(string newServer, string newDatabase, string newUid, string newPassword)
        {
            throw new NotImplementedException();
            //DatabaseHandler.Instance.setDatabaseAccessData(newServer, newDatabase, newUid, newPassword);
        }

        /// <summary>
        /// Method for updating the competence state of a payer by trackingid with given date. format:"2017-07-04 10:41:54"
        /// </summary>
        /// <param name="tid"> tracking id of the player</param>
        /// <param name="evidence"> xml representation of the evidence </param>
        /// <returns>true if the update is successful, false otherwise</returns>
        public static bool updatecompetencestate(string tid, string evidence, string date)
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
            EvidenceSet es = EvidenceSet.getESFromXmlString(evidence);
            if (es == null)
                return false;
            DomainModel dm = DomainModel.getDMFromXmlString(dmstring);
            CompetenceProbabilities cp = CompetenceProbabilities.getCPFromXmlString(cpstring);
            CompetenceProbabilities newcp = CompetenceHandler.Instance.updateCompetenceState(dm, cp, es);
            if (newcp == null)
                return false;


            //store cs
            return DatabaseHandler.Instance.performCompetenceProbabilityUpdate(tid, ids[1], newcp.toXmlString(), evidence, date);
        }

        /// <summary>
        /// Method for creating a trackingid for a given domainmodel speziffied by id (dmid) with given date. format:"2017-07-04 10:41:54"
        /// </summary>
        /// <param name="dmid"> id of the domain model for which the tracking id is created</param>
        /// <returns>null if an error occured, the tracking id otherwise</returns>
        public static string createtrackingid(string dmid, string date)
        {
            string tid = DatabaseHandler.Instance.createTrackingId(dmid, date);
            return tid;
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
            //check validity of the structure
            if (DomainModel.getDMFromXmlString(dm) == null)
                return null;

            return DatabaseHandler.Instance.insertdomainmodel(dm);
        }

        /// <summary>
        /// Method for deleting a domainmodel and all related competencestates and trackingids
        /// </summary>
        public static bool deletedm(string dmid)
        {
            //does dmid exist?
            if (!DatabaseHandler.Instance.doesDomainModelIdExist(dmid))
                return false;

            List<string> trackingIdsAffected = DatabaseHandler.Instance.getTrackingIdsToDomainModelId(dmid);
            foreach (string tid in trackingIdsAffected)
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
            EvidenceSet es = EvidenceSet.getESFromXmlString(evidence);
            if (es == null)
                return false;
            DomainModel dm = DomainModel.getDMFromXmlString(dmstring);
            CompetenceProbabilities cp = CompetenceProbabilities.getCPFromXmlString(cpstring);
            CompetenceProbabilities newcp = CompetenceHandler.Instance.updateCompetenceState(dm,cp,es);
            if (newcp == null)
                return false;

           
            //store cs
            return DatabaseHandler.Instance.performCompetenceProbabilityUpdate(tid, ids[1],newcp.toXmlString(), evidence);
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

        /// <summary>
        /// Method for checking if userid/password combination is valid
        /// </summary>
        /// <param name="userid"> userid to check</param>
        /// <param name="password">password to check</param>
        /// <returns>true, if userid and password fit together</returns>
        public static bool isUserValid(string userid, string password)
        {
            return DatabaseHandler.Instance.isUserValid(userid, password);
        }

        /// <summary>
        /// Returns true, if the mysql database server is running and database exists
        /// </summary>
        /// <returns></returns>
        public static bool canConnectToDatabase()
        {
            return DatabaseHandler.Instance.canConnectToDatabase();
        }

        /// <summary>
        /// Method returning the Domain model id to an tracking id
        /// </summary>
        /// <param name="tid">tracking id</param>
        /// <returns>domain model id</returns>
        public static string getDomainModelIdByTrackingId(string tid)
        {
            string[] data = DatabaseHandler.Instance.getDomainmodelIdAndCompetenceProbabilityId(tid);
            if (data == null)
                return null;

            return data[0];
        }

        /// <summary>
        /// Method returning the update history of a tracking id
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static string getTrackingHistory(string tid)
        {
            UpdateHistory uh = DatabaseHandler.Instance.getTrackingHistory(tid);
            if (uh == null)
                return null;
            return uh.toXmlString();
        }

        #endregion Methods
    }
}
