using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class for storing and receiving new user
/// </summary>
namespace storagedb.DataBase
{
    internal class DBConnectUser
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnectUser(string newServer, string newDatabase, string newUid, string newPassword)
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
            createTable();
        }

        public void createTable()
        {
            string statement = "create table IF NOT EXISTS users (userid TEXT NOT NULL, password TEXT NOT NULL, PRIMARY KEY(userid(50)));";

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

            if (!this.doesUserIdExist("rage"))
            {
                this.Insert("rage","rage");
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
                Logger.Log("Not able to close database connection, errorcode: " + ex.Number.ToString());
                return false;
            }
        }

        /// <summary>
        /// Insert a user in the database
        /// </summary>
        /// <returns>
        /// 0 - error
        /// </returns>
        /// <param name="userid">unique identifier of the user</param>
        /// <param name="password">password of the user</param>
        public string Insert(string userid, string password)
        {

            string retVal = null;
            string query = "INSERT INTO users (userid, password) VALUES('" + userid + "','" + password + "')";

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
        /// <param name="userid">user id</param>
        public bool doesUserIdExist(string userid)
        {
            List<string> whereStatements = new List<string>();
            whereStatements.Add("userid='" + userid + "'");
            List<string>[] list = Select(whereStatements);
            return list[0].Count > 0;
        }

        //Select statement
        public List<string>[] Select(List<string> where = null)
        {
            string query = "SELECT * FROM users";

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
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();

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
                    list[0].Add(dataReader["userid"] + "");
                    list[1].Add(dataReader["password"] + "");
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


        public bool isUserValid(string userid, string password)
        {
            List<string> whereStatements = new List<string>();
            whereStatements.Add("userid='" + userid + "'");
            List<string>[] list = Select(whereStatements);
            if(list[1].Count==0 || !list[1][0].Equals(password))
                return false;
            return true;
        }

        public void dropTable()
        {

            string statement = "drop table IF EXISTS users;";

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
    }
}
