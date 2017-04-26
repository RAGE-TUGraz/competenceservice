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
using CBKST.Elements;
using CBKST;

namespace storagedb
{
    public class DatabaseHandler
    {
        #region Parameters
        public static string server = "localhost";
        public static string database = "competencedb";
        public static string uid = "root";
        public static string password = "rage";
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

        #endregion
        #region Constructor
        private DatabaseHandler()
        {
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
            //reset domain model database and enter test domain model
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
        /// <returns></returns>
        public int insertdomainmodel(string name, string password, string domainmodel)
        {
            return domainmodeldb.Insert(name,password,domainmodel);
        }

        /// <summary>
        /// Method for deleting a domain model by id - this is a string containing the primary key id of the table
        /// </summary>
        /// <param name="id"> string formated integer - primary key of table to delete doamin model</param>
        /// <returns> true, if deleting was successful, false otherwise</returns>
        public bool deleteDomainModelById(string id)
        {
            return domainmodeldb.DeleteById(id);
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
            string tid = trackingiddb.Insert(dmid, csid);
            if (tid == null)
                return null;


            return tid;
        }

        /// <summary>
        /// Method returning the competence probability vector for a given trackking id
        /// </summary>
        /// <param name="tid"> tracking id supplied</param>
        /// <returns> null if there is an error, the probability vector otherwise</returns>
        public string getCompetenceProbabilitiesByTrackingId(string tid)
        {
            return competencestatedb.getCompetenceState(tid);
        }

        /// <summary>
        /// Method returning the domain model id and competence probability id for a tracking id
        /// </summary>
        /// <param name="tid">tracking id for getting the dm/cp values</param>
        /// <returns>null if there is an error, 
        /// return[0]...domain model id, string[1]...competence probability id otherwise</returns>
        public string[] getDomainmodelIdAndCompetenceProbabilityId(string tid)
        {
            int inttid;
            if (!Int32.TryParse(tid, out inttid))
                return null;

            List<string> wherestatement = new List<string>();
            wherestatement.Add("trackingid="+tid);
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
        public bool performCompetenceProbabilityUpdate(string competenceProbabiltyId, string competenceProbability)
        {
            return competencestatedb.Update(competenceProbabiltyId, competenceProbability);
        }

        /// <summary>
        /// Method returning a list of all trackingids assigned to a domain model specified by dmid
        /// </summary>
        /// <param name="dmid">domain model id</param>
        /// <returns> list of ftrackingids assigned to domain model id</returns>
        public List<string> getTrackingIdsToDomainModelId(string dmid)
        {
            int dmidint;
            if (!Int32.TryParse(dmid, out dmidint))
                return null;

            List<string> whereStatement = new List<string>();
            whereStatement.Add("domainmodelid=" + dmidint.ToString());
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
