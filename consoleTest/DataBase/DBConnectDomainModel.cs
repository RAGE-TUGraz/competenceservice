using MySql.Data.MySqlClient;
using consoleTest;
using System.Collections.Generic;
using System.Data.Common;

// https://www.codeproject.com/articles/43438/connect-c-to-mysql
using System;

public class DBConnectDomainModel
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
	}

	//Initialize values
	private void Initialize()
	{
		server = "localhost";
		database = "testdb";
		uid = "root";
		password = "rage";
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
		string statement = "create table IF NOT EXISTS domainmodels (id INT NOT NULL AUTO_INCREMENT, name VARCHAR(30), password VARCHAR(30), structure TEXT, PRIMARY KEY(id));";

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
	/// 0 - structure inserted
	/// 1 - structurename already exists
	/// </returns>
	/// <param name="name">structure name</param>
	/// <param name="password">structure password</param>
	/// <param name="structure">Structure</param>
	public int Insert(string name, string password, string structure)
	{
		if (doesStructureNameExist (name))
			return 1;
		string query = "INSERT INTO domainmodels (name, password, structure) VALUES('"+name+"', '"+password+"', '"+structure+"')";

		//open connection
		if (this.OpenConnection() == true)
		{
			//create command and assign the query and connection from the constructor
			MySqlCommand cmd = new MySqlCommand(query, connection);

			//Execute command
			cmd.ExecuteNonQuery();

			//close connection
			this.CloseConnection();
		}
		return 0;
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
	public void Delete(string name)
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
		string structure = "<?xml version=\"1.0\" encoding=\"utf-16\"?><domainmodel xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><elements><competences><competence id=\"C1\" /><competence id=\"C2\" /><competence id=\"C3\" /><competence id=\"C4\" /><competence id=\"C5\" /><competence id=\"C6\" /><competence id=\"C7\" /><competence id=\"C8\" /><competence id=\"C9\" /><competence id=\"C10\" /></competences><situations><situation id=\"gs1\" /><situation id=\"gs2\" /><situation id=\"gs3\" /><situation id=\"gs4\" /><situation id=\"gs5\" /><situation id=\"gs6\" /><situation id=\"gs7\" /><situation id=\"gs8\" /><situation id=\"gs9\" /><situation id=\"gs10\" /></situations></elements><relations><competenceprerequisites><competence id=\"C5\"><prereqcompetence id=\"C1\" /><prereqcompetence id=\"C2\" /></competence><competence id=\"C6\"><prereqcompetence id=\"C4\" /></competence><competence id=\"C7\"><prereqcompetence id=\"C4\" /></competence><competence id=\"C8\"><prereqcompetence id=\"C3\" /><prereqcompetence id=\"C6\" /></competence><competence id=\"C9\"><prereqcompetence id=\"C5\" /><prereqcompetence id=\"C8\" /></competence><competence id=\"C10\"><prereqcompetence id=\"C9\" /><prereqcompetence id=\"C7\" /></competence></competenceprerequisites><situations><situation id=\"gs1\"><competence id=\"C1\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs2\"><competence id=\"C2\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs3\"><competence id=\"C3\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs4\"><competence id=\"C4\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs5\"><competence id=\"C5\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C1\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C2\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs6\"><competence id=\"C6\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C4\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs7\"><competence id=\"C4\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C7\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs8\"><competence id=\"C8\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C6\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C3\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs9\"><competence id=\"C9\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C5\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C8\" levelup=\"medium\" leveldown=\"medium\" /></situation><situation id=\"gs10\"><competence id=\"C10\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C9\" levelup=\"medium\" leveldown=\"medium\" /><competence id=\"C7\" levelup=\"medium\" leveldown=\"medium\" /></situation></situations></relations><updatelevels><level direction=\"up\" power=\"low\" xi=\"1.2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"up\" power=\"medium\" xi=\"2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"up\" power=\"high\" xi=\"4\" minonecompetence=\"true\" maxonelevel=\"false\" /><level direction=\"down\" power=\"low\" xi=\"1.2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"down\" power=\"medium\" xi=\"2\" minonecompetence=\"false\" maxonelevel=\"true\" /><level direction=\"down\" power=\"high\" xi=\"4\" minonecompetence=\"true\" maxonelevel=\"false\" /></updatelevels></domainmodel>";
		Insert ("dm1","dm1",structure);
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
}