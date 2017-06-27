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

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace storagedb
{
    internal class DBConnectCompetenceDevelopment
    {
        //private Dictionary<string,MySqlConnection> connection = new Dictionary<string, MySqlConnection>();
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnectCompetenceDevelopment(string newServer, string newDatabase, string newUid, string newPassword)
        {
            server = newServer;
            database = newDatabase;
            uid = newUid;
            password = newPassword;
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        public void dropTables(List<string> tids)
        {
            foreach (string tid in tids)
                dropTable(tid);
        }

        public void dropTable(string tid)
        {
            string statement = "drop table IF EXISTS compdev_" + tid + ";";

            //open connection
            if (this.OpenConnection(tid) == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(statement, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection(tid);
            }
        }

        public void createTable(string tid)
        {
            string statement = "create table IF NOT EXISTS compdev_"+tid+" (id INT NOT NULL AUTO_INCREMENT, competencestate TEXT, datetime TEXT, PRIMARY KEY(id));";

            //open connection
            if (this.OpenConnection(tid) == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(statement, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection(tid);
            }
        }

        //open connection to database
        private bool OpenConnection(string tid)
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Logger.Log("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Logger.Log("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection(string tid)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Logger.Log("Not able to close database connection, errorcode: " + ex.Number.ToString());
                return false;
            }
        }

        /// <summary>
        /// Insert a competencestructure with specified name and password.
        /// </summary>
        /// <returns>
        /// null - error
        /// else - id of inserted record
        /// </returns>
        /// <param name="competencestate">xml representation of the competence state</param>
        public string Insert(string tid, string competencestate)
        {
            string retVal = null;

            string query = "INSERT INTO compdev_" + tid + " (competencestate, datetime) VALUES('" + competencestate + "',UTC_TIMESTAMP())";

            //open connection
            if (this.OpenConnection(tid) == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();
                retVal = cmd.LastInsertedId.ToString();

                //close connection
                this.CloseConnection(tid);
            }
            return retVal;
        }

        /// <summary>
        /// Doeses the user id exist - return true if it does.
        /// </summary>
        /// <returns>true, if user id exist, false otherwise.</returns>
        /// <param name="name">user id</param>
        public bool doesTableExist(string tid)
        {
            bool returnValue = false;
            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME =compdev_" + tid + ";";

            //open connection
            if (this.OpenConnection(tid) == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                returnValue = dataReader.HasRows;

                //close connection
                this.CloseConnection(tid);
            }
            return returnValue;
        }

        //Select statement
        public List<string>[] Select(string tid, List<string> where = null)
        {
            string query = "SELECT * FROM compdev_" + tid + "";

            if (where != null && where.Count != 0)
            {
                query += " WHERE ";
                for (int count = 0; count < where.Count; count++)
                {
                    query += where[count];
                    if (count == where.Count - 1)
                        query += ";";
                    else
                        query += " AND ";
                }
            }
            else
            {
                query += ";";
            }

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection(tid) == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["competencestate"] + "");
                    list[2].Add(dataReader["datetime"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection(tid);

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// Gets the state of the competence.
        /// </summary>
        /// <returns>The competence state.</returns>
        /// <param name="userid">Userid.</param>
        public List<string> [] getCompetenceDevelopment(string tid)
        {
            return Select(tid, null);
        }

    }
}

