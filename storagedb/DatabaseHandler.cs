﻿/*
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
using CBKST.Elements;
using CBKST;

namespace storagedb
{
    public class DatabaseHandler
    {
        #region Parameters
        internal static string server = "localhost";
        internal static string database = "competencedb";
        internal static string uid = "root";
        internal static string password = "rage";

        internal string allowedCharsInId = "0123456789abcdefghijklmnopqrstuvwxyz"; //ABCDEFGHIJKLMNOPQRSTUVWXYZ
        internal int idLength = 3;

        #endregion
        #region Fields

        /// <summary>
        /// Instance of the class DatabaseHandler - Singelton pattern
        /// </summary>
        static readonly DatabaseHandler instance = new DatabaseHandler();

        /// <summary>
        /// The tracking id database handler instance.
        /// </summary>
        private readonly DBConnectTrackingId trackingiddb = new DBConnectTrackingId();

        /// <summary>
        /// The domainmodel database handler instance.
        /// </summary>
        private readonly DBConnectDomainModel domainmodeldb = new DBConnectDomainModel();

        /// <summary>
        /// The competence state database handler instance.
        /// </summary>
        private readonly DBConnectCompetenceState competencestatedb = new DBConnectCompetenceState();

        /// <summary>
        /// The competence development database handler instance.
        /// </summary>
        private readonly DBConnectCompetenceDevelopment competencedevelopmentdb = new DBConnectCompetenceDevelopment();

        #endregion
        #region Constructor
        private DatabaseHandler()
        {
            idLength = Math.Min(idLength,50);
        }
        #endregion
        #region Properties

        /// <summary>
        /// Getter for Instance of the DatabaseHandler - Singelton pattern
        /// </summary>
        public static DatabaseHandler Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
        #region Utilitymethods

        /// <summary>
        /// Method for testing - resetsa database
        /// </summary>
        /// <returns></returns>
        public bool resetDatabases()
        {
            //get all trackingids and deleted related competence development tables
            List<string> trackingids = trackingiddb.getAllTrackingIds();
            competencedevelopmentdb.dropTables(trackingids);

            //reset domain model database 
            domainmodeldb.dropTable();
            domainmodeldb.createTable();

            //reset competence probability database
            competencestatedb.dropTable();
            competencestatedb.createTable();

            //reset tracking id database
            trackingiddb.dropTable();
            trackingiddb.createTable();

            return true;
        }

        /// <summary>
        /// Method for creating/storing testdata
        /// </summary>
        /// <returns></returns>
        public bool createTestdata()
        {
            domainmodeldb.enterTestData();
            return true;
        }

        /// <summary>
        /// Method for creating a random tracking code
        /// </summary>
        /// <param name="trackingIdLength"> length of the tracking code to create</param>
        /// <param name="allowedChars"> characters which are allowed for the tracking code</param>
        /// <returns> a string of length 'trackingIdLength' with random characters from 'allowedChars'</returns>
        private string createNewRandomString(int trackingIdLength,string allowedChars)
        {
            Random rng = new Random();
            int length = trackingIdLength;
            int setLength = allowedChars.Length;
            char[] chars = new char[length];

            for (int i = 0; i < length; ++i)
            {
                chars[i] = allowedChars[rng.Next(setLength)];
            }

            return new string(chars);
        }

        /// <summary>
        /// Method for creating a new, not existing tracking id
        /// </summary>
        /// <returns></returns>
        private string createNewTrackingId()
        {
            string retStr;
            do
            {
                retStr = createNewRandomString(idLength, allowedCharsInId);
            } while (trackingiddb.doesTrackingIdExist(retStr));

            return retStr;
        }

        /// <summary>
        /// Method for creating a new, not existing domain model id
        /// </summary>
        /// <returns></returns>
        private string createNewDomainmodelId()
        {
            string retStr;
            do
            {
                retStr = createNewRandomString(idLength, allowedCharsInId);
            } while (domainmodeldb.doesIdExist(retStr));

            return retStr;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Method returning the domain model xml for a given domainmodel id
        /// </summary>
        /// <param name="dmid">domain model id</param>
        /// <returns> 
        /// null if the id cant be parsed to an int or the domain model does not exists, 
        /// the domainmodel otherwise
        /// </returns>
        public string getDomainModelById(string dmid)
        {
            return domainmodeldb.getDomainModelById(dmid);
        }

        /// <summary>
        /// Method for insering a domain model into the database
        /// </summary>
        /// <param name="name"> omitted in this version </param>
        /// <param name="password"> omitted in this version </param>
        /// <param name="domainmodel"> xml representation of a domain model</param>
        /// <returns>null if unsuccessful, the id otherwise</returns>
        public string insertdomainmodel(string name, string password, string domainmodel)
        {
            string dmid = createNewDomainmodelId();
            if (!domainmodeldb.Insert(dmid, name, password, domainmodel))
                return null;
            return dmid;
        }

        /// <summary>
        /// Method for deleting a domain model by id - this is a string containing the primary key id of the table
        /// </summary>
        /// <param name="id"> string formated integer - primary key of table to delete doamin model</param>
        /// <returns> true, if deleting was successful, false otherwise</returns>
        public bool deleteDomainModelById(string dmid)
        {
            return domainmodeldb.DeleteById(dmid);
        }

        /// <summary>
        /// Method creating a tracking id and competence state (this represents a player) for a given domain model, represented by id
        /// </summary>
        /// <param name="dmid"> id of the domainmodel used to create the trackig id</param>
        /// <returns>0 if an error occured, the created tracking id otherwise</returns>
        public string createTrackingId(string dmid)
        {
            //load domain model
            string dmstring = getDomainModelById(dmid);
            if (dmstring == null)
                return null;

            //create initial probability structure
            CompetenceProbabilities ps = CompetenceHandler.Instance.createInitialCompetenceProbabilities(DomainModel.getDMFromXmlString(dmstring));


            //store cs -> get csid
            string csid = competencestatedb.Insert(ps.toXmlString());
            if (csid == null)
                return null;

            //use cs id to create tracking id
            string tid = createNewTrackingId();
            if (trackingiddb.Insert(tid, dmid, csid) == null)
                return null;

            //create competence development table
            competencedevelopmentdb.createTable(tid);
            //enter first dataset into competence development table
            competencedevelopmentdb.Insert(tid,ps.toXmlString());


            return tid;
        }

        /// <summary>
        /// Method returning the competence probability vector for a given trackking id
        /// </summary>
        /// <param name="tid"> tracking id supplied</param>
        /// <returns> null if there is an error, the probability vector otherwise</returns>
        public string getCompetenceProbabilitiesByTrackingId(string tid)
        {
            string competenceprobabilitiesid = trackingiddb.getCpidByTrackingId(tid);
            if (competenceprobabilitiesid == null)
                return null;
            return competencestatedb.getCompetenceState(competenceprobabilitiesid);
        }

        /// <summary>
        /// Method returning the competence probability vector for a given comp.prob. id
        /// </summary>
        /// <param name="cpid"> cp id supplied</param>
        /// <returns> null if there is an error, the probability vector otherwise</returns>
        public string getCompetenceProbabilitiesByCpId(string cpid)
        {
            return competencestatedb.getCompetenceState(cpid);
        }

        /// <summary>
        /// Method returning the domain model id and competence probability id for a tracking id
        /// </summary>
        /// <param name="tid">tracking id for getting the dm/cp values</param>
        /// <returns>null if there is an error, 
        /// return[0]...domain model id, string[1]...competence probability id otherwise</returns>
        public string[] getDomainmodelIdAndCompetenceProbabilityId(string tid)
        {
            List<string> wherestatement = new List<string>();
            wherestatement.Add("trackingid='"+tid+"'");
            List<string>[] values = trackingiddb.Select(wherestatement);
            if (values == null || values[0].Count == 0)
                return null;


            string[] retVal= new string[2];
            retVal[0] = values[1][0];
            retVal[1] = values[2][0];

            return retVal;
        }

        /// <summary>
        /// Method updating the competence probabilty value in the database
        /// </summary>
        /// <param name="competenceProbabiltyId"> id of the entry</param>
        /// <param name="competenceProbability"> new value of the entry</param>
        /// <returns></returns>
        public bool performCompetenceProbabilityUpdate(string tid, string competenceProbabiltyId, string competenceProbability)
        {
            //store new value in competence development table
            competencedevelopmentdb.Insert(tid, competenceProbability);

            return competencestatedb.Update(competenceProbabiltyId, competenceProbability);
        }

        /// <summary>
        /// Method returning a list of all trackingids assigned to a domain model specified by dmid
        /// </summary>
        /// <param name="dmid">domain model id</param>
        /// <returns> list of ftrackingids assigned to domain model id</returns>
        public List<string> getTrackingIdsToDomainModelId(string dmid)
        {
            List<string> whereStatement = new List<string>();
            whereStatement.Add("domainmodelid='" + dmid+"'");
            List<string> [] data = trackingiddb.Select(whereStatement);
            
            return data[0];
        }

        /// <summary>
        /// Method for deleting a competence probability entry in the database
        /// </summary>
        /// <param name="cpid"> id of entry to delete </param>
        /// <returns>true if successful, false otherwise </returns>
        public bool deleteCompetenceProbabilityEntryById(string cpid)
        {
            return competencestatedb.DeleteById(cpid);
        }

        /// <summary>
        /// Method deleting a tracking id and the coresponding competence probabilities from the database
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public bool deleteTrackingId(string tid)
        {
            //get csid
            string[] ids = getDomainmodelIdAndCompetenceProbabilityId(tid);
            if (ids == null)
                return false;

            //delete competence development
            competencedevelopmentdb.dropTable(tid);

            //delete cp
            if (!deleteCompetenceProbabilityEntryById(ids[1]))
                return false;

            //delete tid
            if (!trackingiddb.DeleteById(tid))
                return false;

            return true;
        }

        
        #endregion Methods
    }
}
