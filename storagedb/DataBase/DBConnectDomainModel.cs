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
using System.Collections.Generic;
// https://www.codeproject.com/articles/43438/connect-c-to-mysql
using System;

namespace storagedb
{

    internal class DBConnectDomainModel
    {
	    private MySqlConnection connection;
	    private string server;
	    private string database;
	    private string uid;
	    private string password;

	    //Constructor
	    public DBConnectDomainModel()
	    {
		    Initialize();
            createTable();
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
		    string statement = "drop table IF EXISTS domainmodels;";

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
		    string statement = "create table IF NOT EXISTS domainmodels (id TEXT NOT NULL, name VARCHAR(30), password VARCHAR(30), structure TEXT, PRIMARY KEY(id(50)));";

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
	    /// Insert a structure with specified name and password.
	    /// </summary>
	    /// <returns>
	    /// 0 - error
        /// else - id of the structure
	    /// </returns>
	    /// <param name="name">structure name</param>
	    /// <param name="password">structure password</param>
	    /// <param name="structure">Structure</param>
	    public bool Insert(string id, string name, string password, string structure)
	    {
		    string query = "INSERT INTO domainmodels (id, name, password, structure) VALUES('"+id+"','"+name+"', '"+password+"', '"+structure+"')";

		    //open connection
		    if (this.OpenConnection() == true)
		    {
			    //create command and assign the query and connection from the constructor
			    MySqlCommand cmd = new MySqlCommand(query, connection);

			    //Execute command
			    cmd.ExecuteNonQuery();

			    //close connection
			    this.CloseConnection();
		    }else
                return false;

		    return true;
	    }

	    //Update statement
	    public void Update()
	    {
		    /*
		    string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

		    //Open connection
		    if (this.OpenConnection() == true)
		    {
			    //create mysql command
			    MySqlCommand cmd = new MySqlCommand();
			    //Assign the query using CommandText
			    cmd.CommandText = query;
			    //Assign the connection using Connection
			    cmd.Connection = connection;

			    //Execute query
			    cmd.ExecuteNonQuery();

			    //close connection
			    this.CloseConnection();
		    }
		    */
	    }

        //Delete statement
        public bool DeleteById(string id)
        {
            bool retval = false;

            string query = "DELETE FROM domainmodels WHERE id='" + id + "'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                retval = true;
            }
            return retval;
        }

        //Delete statement
        public void DeleteByName(string name)
	    {
		    string query = "DELETE FROM domainmodels WHERE name='"+name+"'";

		    if (this.OpenConnection() == true)
		    {
			    MySqlCommand cmd = new MySqlCommand(query, connection);
			    cmd.ExecuteNonQuery();
			    this.CloseConnection();
		    }
	    }

	    //Select statement
	    public List <string> [] Select(List<string> where = null)
	    {  string query = "SELECT * FROM domainmodels";

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
		    List< string >[] list = new List< string >[4];
		    list[0] = new List< string >();
		    list[1] = new List< string >();
		    list[2] = new List< string >();
		    list[3] = new List< string >();

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
				    list[0].Add(dataReader["id"] + "");
				    list[1].Add(dataReader["name"] + "");
				    list[2].Add(dataReader["password"] + "");
				    list[3].Add(dataReader["structure"] + "");
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

	    //Count statement
	    public int Count()
	    {
		    string query = "SELECT Count(*) FROM domainmodels";
		    int Count = -1;

		    //Open Connection
		    if (this.OpenConnection() == true)
		    {
			    //Create Mysql Command
			    MySqlCommand cmd = new MySqlCommand(query, connection);

			    //ExecuteScalar will return one value
			    Count = int.Parse(cmd.ExecuteScalar()+"");

			    //close Connection
			    this.CloseConnection();

			    return Count;
		    }
		    else
		    {
			    return Count;
		    }
	    }

	    //Backup
	    public void Backup()
	    {
		    /*
		    try
		    {
			    DateTime Time = DateTime.Now;
			    int year = Time.Year;
			    int month = Time.Month;
			    int day = Time.Day;
			    int hour = Time.Hour;
			    int minute = Time.Minute;
			    int second = Time.Second;
			    int millisecond = Time.Millisecond;

			    //Save file to C:\ with the current date as a filename
			    string path;
			    path = "C:\\MySqlBackup" + year + "-" + month + "-" + day + 
				    "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
			    StreamWriter file = new StreamWriter(path);


			    ProcessStartInfo psi = new ProcessStartInfo();
			    psi.FileName = "mysqldump";
			    psi.RedirectStandardInput = false;
			    psi.RedirectStandardOutput = true;
			    psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", 
				    uid, password, server, database);
			    psi.UseShellExecute = false;

			    Process process = Process.Start(psi);

			    string output;
			    output = process.StandardOutput.ReadToEnd();
			    file.WriteLine(output);
			    process.WaitForExit();
			    file.Close();
			    process.Close();
		    }
		    catch (IOException ex)
		    {
			    MessageBox.Show("Error , unable to backup!");
		    }
		    */
	    }

	    //Restore
	    public void Restore()
	    {
		    /*
		        try
		        {
		            //Read file from C:\
		            string path;
		            path = "C:\\MySqlBackup.sql";
		            StreamReader file = new StreamReader(path);
		            string input = file.ReadToEnd();
		            file.Close();

		            ProcessStartInfo psi = new ProcessStartInfo();
		            psi.FileName = "mysql";
		            psi.RedirectStandardInput = true;
		            psi.RedirectStandardOutput = false;
		            psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", 
					    uid, password, server, database);
		            psi.UseShellExecute = false;

		        
		            Process process = Process.Start(psi);
		            process.StandardInput.WriteLine(input);
		            process.StandardInput.Close();
		            process.WaitForExit();
		            process.Close();
		        }
		        catch (IOException ex)
		        {
		            MessageBox.Show("Error , unable to Restore!");
		        } 
		    */
	    }

	    /// <summary>
	    /// Doeses the structure name exist - return true if it does.
	    /// </summary>
	    /// <returns>true, if structure name exist, false otherwise.</returns>
	    /// <param name="name">name of the structure</param>
	    public bool doesStructureNameExist(string name){
		    List<string> whereStatements = new List<string> ();
		    whereStatements.Add ("name='" + name + "'");
		    List<string>[] list = Select (whereStatements);
		    return list [0].Count > 0;
	    }

	    public void enterTestData(){
            string structure = "<?xml version=\"1.0\" encoding=\"utf-16\"?><domainmodel xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
            structure += "<elements><competences><competence id=\"C1\" /><competence id=\"C2\" /><competence id=\"C3\" /><competence id=\"C4\" /><competence id=\"C5\" /><competence id=\"C6\" /><competence id=\"C7\" /><competence id=\"C8\" /><competence id=\"C9\" /><competence id=\"C10\" /></competences>";
            structure += "<situations><situation id=\"gs1\" /><situation id=\"gs2\" /><situation id=\"gs3\" /><situation id=\"gs4\" /><situation id=\"gs5\" /><situation id=\"gs6\" /><situation id=\"gs7\" /><situation id=\"gs8\" /><situation id=\"gs9\" /><situation id=\"gs10\" /></situations>";
            structure += "<activities><activity id=\"activityc1\"></activity></activities>";
            structure += "</elements><relations><competenceprerequisites><competence id=\"C5\"><prereqcompetence id=\"C1\" /><prereqcompetence id=\"C2\" /></competence><competence id=\"C6\"><prereqcompetence id=\"C4\" /></competence><competence id=\"C7\"><prereqcompetence id=\"C4\" /></competence><competence id=\"C8\"><prereqcompetence id=\"C3\" /><prereqcompetence id=\"C6\" /></competence><competence id=\"C9\"><prereqcompetence id=\"C5\" /><prereqcompetence id=\"C8\" /></competence><competence id=\"C10\"><prereqcompetence id=\"C9\" /><prereqcompetence id=\"C7\" /></competence></competenceprerequisites>";
            structure += "<situations><situation id=\"gs1\"><competence id=\"C1\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs2\"><competence id=\"C2\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs3\"><competence id=\"C3\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs4\"><competence id=\"C4\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs5\"><competence id=\"C5\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C1\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C2\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs6\"><competence id=\"C6\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C4\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs7\"><competence id=\"C4\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C7\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs8\"><competence id=\"C8\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C6\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C3\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs9\"><competence id=\"C9\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C5\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C8\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs10\"><competence id=\"C10\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C9\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C7\" levelup=\"medium\" leveldown=\"medium\" /></situation></situations>";
            structure += "<activities><activity id=\"activityc1\"><competence id=\"C1\" power=\"medium\" direction=\"up\"></competence></activity></activities>";
            structure += "</relations><updatelevels><level direction=\"up\" power=\"low\" xi=\"1.2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"up\" power=\"medium\" xi=\"2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"up\" power=\"high\" xi=\"4\" minonecompetence=\"true\" maxonelevel=\"false\" /><level direction=\"down\" power=\"low\" xi=\"1.2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"down\" power=\"medium\" xi=\"2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"down\" power=\"high\" xi=\"4\" minonecompetence=\"true\" maxonelevel=\"false\" /></updatelevels></domainmodel>";
		    Insert ("1","dm1","dm1",structure);
	    }

	    public int getDomainModelIdByName(string name){
		    int retVal = -1;
		    List<string> whereStatements = new List<string> ();
		    whereStatements.Add ("name='" + name + "'");
		    List<string>[] list = Select (whereStatements);
		    Int32.TryParse (list [0] [0], out retVal);
		    return retVal;
	    }

	    public string getDomainModelByName(string name){
		    List<string> whereStatements = new List<string> ();
		    whereStatements.Add ("name='" + name + "'");
		    List<string>[] list = Select (whereStatements);
		    if (list [3].Count == 0)
			    return null;
		    return list [3] [0];
	    }

        public string getDomainModelById(string id)
        {
            List<string> whereStatements = new List<string>();
            whereStatements.Add("id='" + id+ "'");
            List<string>[] list = Select(whereStatements);
            if (list[3].Count == 0)
                return null;
            return list[3][0];
        }

        /// <summary>
        /// Doeses the domain model id exist - return true if it does.
        /// </summary>
        /// <returns>true, if user id exist, false otherwise.</returns>
        /// <param name="domainmodelid">domain model id</param>
        public bool doesIdExist(string domainmodelid)
        {
            List<string> whereStatements = new List<string>();
            whereStatements.Add("id='" + domainmodelid + "'");
            List<string>[] list = Select(whereStatements);
            return list[0].Count > 0;
        }

    }

}