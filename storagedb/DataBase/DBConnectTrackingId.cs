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
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace storagedb
{
	internal class DBConnectTrackingId
	{
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;

		//Constructor
		public DBConnectTrackingId()
		{
			Initialize();
		}

		//Initialize values
		private void Initialize()
		{
            server = DatabaseHandler.server;
            database = DatabaseHandler.database;
            uid = DatabaseHandler.uid;
            password = DatabaseHandler.password;
            string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
				database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

			connection = new MySqlConnection(connectionString);
		}

		public void dropTable(){
			string statement = "drop table IF EXISTS trackingids;";

			//open connection
			if (this.OpenConnection() == true)
			{
				//create command and assign the query and connection from the constructor
				MySqlCommand cmd = new MySqlCommand(statement, connection);

				//Execute command
				cmd.ExecuteNonQuery();

				//close connection
				this.CloseConnection();
			}
		}

		public void createTable(){
			string statement = "create table IF NOT EXISTS trackingids (trackingid INT NOT NULL AUTO_INCREMENT, domainmodelid INT NOT NULL, competencestateid INT NOT NULL, PRIMARY KEY(trackingid));";

			//open connection
			if (this.OpenConnection() == true)
			{
				//create command and assign the query and connection from the constructor
				MySqlCommand cmd = new MySqlCommand(statement, connection);

				//Execute command
				cmd.ExecuteNonQuery();

				//close connection
				this.CloseConnection();
			}
		}

		//open connection to database
		private bool OpenConnection()
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
		private bool CloseConnection()
		{
			try
			{
				connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				Logger.Log("Not able to close database connection, errorcode: "+ex.Number.ToString());
				return false;
			}
		}

		/// <summary>
		/// Insert a competencestructure with specified name and password.
		/// </summary>
		/// <returns>
		/// 0 - error
		/// else - id of inserted record
		/// </returns>
		/// <param name="userid">unique identifier of the competence state</param>
		/// <param name="competencestate">xml representation of the competence state</param>
		public string Insert(string domainmodelid, string competencestateid)
		{
            int dmid, csid;
            if (!Int32.TryParse(domainmodelid, out dmid) || !Int32.TryParse(competencestateid, out csid))
                return null;

			string retVal = null;
			string query = "INSERT INTO trackingids (domainmodelid, competencestateid) VALUES("+dmid+", "+csid+")";

			//open connection
			if (this.OpenConnection() == true)
			{
				//create command and assign the query and connection from the constructor
				MySqlCommand cmd = new MySqlCommand(query, connection);

				//Execute command
				cmd.ExecuteNonQuery();
				retVal = cmd.LastInsertedId.ToString();

				//close connection
				this.CloseConnection();
			}

			return retVal;
		}

		/// <summary>
		/// Doeses the user id exist - return true if it does.
		/// </summary>
		/// <returns>true, if user id exist, false otherwise.</returns>
		/// <param name="name">user id</param>
		public bool doesTrackingIdExist(int trackingid){
			List<string> whereStatements = new List<string> ();
			whereStatements.Add ("trackingid=" + trackingid + "");
			List<string>[] list = Select (whereStatements);
			return list [0].Count > 0;
		}

		//Select statement
		public List <string> [] Select(List<string> where = null)
		{  string query = "SELECT * FROM trackingids";

			if (where != null && where.Count != 0) {
				query += " WHERE ";
				for (int count = 0; count < where.Count; count++) {
					query += where [count];
					if (count == where.Count - 1)
						query += ";";
					else
						query += " AND ";
				}
			}

			//Create a list to store the result
			List< string >[] list = new List< string >[3];
			list[0] = new List< string >();
			list[1] = new List< string >();
			list[2] = new List< string >();

			//Open connection
			if (this.OpenConnection() == true)
			{
				//Create Command
				MySqlCommand cmd = new MySqlCommand(query, connection);
				//Create a data reader and Execute the command
				MySqlDataReader dataReader = cmd.ExecuteReader();

				//Read the data and store them in the list
				while (dataReader.Read())
				{
					list[0].Add(dataReader["trackingid"] + "");
					list[1].Add(dataReader["domainmodelid"] + "");
					list[2].Add(dataReader["competencestateid"] + "");
				}

				//close Data Reader
				dataReader.Close();

				//close Connection
				this.CloseConnection();

				//return list to be displayed
				return list;
			}
			else
			{
				return list;
			}
		}

        //Delete statement
        public bool DeleteById(string id)
        {
            bool retval = false;
            int idint;
            if (!Int32.TryParse(id, out idint))
                return retval;

            string query = "DELETE FROM trackingids WHERE trackingid=" + idint + "";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                retval = true;
            }
            return retval;
        }

    }
}

