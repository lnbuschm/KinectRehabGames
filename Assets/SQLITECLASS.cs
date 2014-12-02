using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;

public class SQLITECLASS : MonoBehaviour
{
    //----------------------------------------------------------------------------------------
    // Author: Zumwalt
    // Date: 8/30/2010
    // Unity Version: U3B6
    // Known issues: None
    //----------------------------------------------------------------------------------------
    // There are different ways to do this, both of these work however
    // at the time of the writing of this code, the Password feature
    // does NOT work in U3B6, it might or might not work in future releases
    // or when Unity gets out of Beta for U3
    //----------------------------------------------------------------------------------------
    //string constr = "URI=file:gamedata.db,Version=3,Compress=True";
    string constr = "Data Source=gamedata.db;Version=3;Compress=False";    // something to use to populate our database, since Unity has Raknet client running already    // lets use it and grab the IP of the machine and the GUID assigned to the player just    // for fun and to use in our database table we are creating...
  //  NetworkPlayer p;
	void Start () {
    //    p = Network.player;        // Step 1, lets create the database if it does not exist
        Debug.Log("CreateDatabase being called");
        CreateDatabase();        // Step 2, lets put some data in that then read it back to the log
        Debug.Log("InsertRecord being called");
		InsertRecord(DateTime.Now, 1.2345f, 2.3456f, 3.45678f);
     //   InsertRecord();
     //   ReadAllRecords();        // Step 3, lets use the Update command and change the value of the records GUID and see the new value
    //    Debug.Log("UpdateAllRecords being called");
     //   UpdateAllRecords();
    //    ReadAllRecords();        // Step 4, lets now purge the data and see what the table looks like
     //   Debug.Log("DeleteAllRecords being called");
    //    DeleteAllRecords();
    //    ReadAllRecords();

        Debug.Log("Done with SQLITE Test");	}    // This section holds our class methods to deal with SQLLite     // a few things to note, All DLL's used are in the Assets folder    // when the program is compiled, those DLL's must exist in the same    // folder as your game BINARY    //    // The SQLLITE3 binary is included one level above the assets folder    // this little program allows you to open and view or use the contents    // of the database created by SQLLite, reffer to the SQLLite manual at
    // http://www.sqlite.org/    // Method to create a database if it does not exist, in this case    // it will create the database in our data folder which is one level    // above our assets folder
	string tableName;
    void CreateDatabase()
    {
        // example query, but this query creates the database which will be housed in our gamedata.db file if that file
        // does not exist, it will create the file automatically for us and there is nothing to write to make that happen
		//string tableName = DateTime.Now.ToString("G");
		//DateTime.Now.ToString("MM_dd_yy__HH_mm_ss") 
		tableName = "_" + DateTime.Now.ToString("MM_dd_yy__HH_mm_ss") + "__" + RehabMenu.currentGame;
		//tablexName = "d";
		//   "Insert into Table1 (Name,Value,[Date]) values(@Name, @Value,@Date)", con

		//string sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (pk INTEGER NOT NULL PRIMARY KEY, guid varchar(64),lastIP varchar(32)); ";

		string sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (t DATETIME, x FLOAT, y FLOAT, z FLOAT); "; 
			//	  " (x FLOAT, y FLOAT, z FLOAT); ";
	//	" (t DATETIME, x FLOAT, y FLOAT, z FLOAT); "; 


	//		" (pk INTEGER NOT NULL PRIMARY KEY, guid varchar(64),lastIP varchar(32)); ";



		try
        {
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // create a command object, using our sql string above against our connection object
                SqliteCommand cmd = new SqliteCommand(sql, con);
                Debug.Log("executing create using executenonquery");
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

	void InsertRecord(DateTime d, float x, float y, float z)
	{
		/*
		CREATE TABLE t1(
			t  TEXT,     -- text affinity by rule 2
			nu NUMERIC,  -- numeric affinity by rule 5
			i  INTEGER,  -- integer affinity by rule 1
			r  REAL,     -- real affinity by rule 4
			no BLOB      -- no affinity by rule 3
			);
		
		-- Values stored as TEXT, INTEGER, INTEGER, REAL, TEXT.
			INSERT INTO t1 VALUES('500.0', '500.0', '500.0', '500.0', '500.0');
*/
		// an insert statement that uses command parameters, NOTE, see the NULL, this is because the first value
		// is our primary key, this is an INTEGER, Automatically incremented, and not null, but we do nothing with it
		// however it is in the way and has to be used in the query, so we just say we are sending nothing and trusting
		// SQLLite to know what to do with it

		// sqlite reads datetime as yyyy-MM-dd HH:mm:ss
		string sql = "INSERT INTO " + tableName + " (t, x, y, z) " +
			"VALUES ('" + d.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'," + x + "," + y + "," + z + "); ";

	//	string sql = "INSERT INTO " + tableName + " (x, y, z) " +
	//		"VALUES (" + x + "," + y + "," + z + "); ";


		try
		{
			// Create a new connection using our connection string defined at the start of the class
			using (SqliteConnection con = new SqliteConnection(constr))
			{
				con.Open();

				
				try
				{
					// create a command object, using our sql string above against our connection object
					SqliteCommand cmd = new SqliteCommand(sql, con);
					// execute the command
					cmd.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}

	/*
	public void Insert(string Name, float Value, DateTime DateTime)
	{
		string connectionString = ConfigurationManager.ConnectionStrings["FatherDB"].ToString();
		using (SqlConnection con = new SqlConnection(connectionString))
		{
			using (SqlCommand cmd = new SqlCommand("Insert into Table1 ((Name) values(@Name) ,(Value) values(@Value),(Date) values(@Date))", con))
			{
				con.Open();
				cmd.Parameters.AddWithValue("@Name", Name);
				cmd.Parameters.AddWithValue("@Value", Value);
				cmd.Parameters.AddWithValue("@Date", DateTime);
				cmd.ExecuteNonQuery();
				con.Close();                 
			}
		}
	}
	*/

    void ReadAllRecords()
    {
        string sql = "SELECT * FROM gamedata;";
        try
        {
            // Create a new connection using our connection string defined at the start of the class
            // NOTE: in the reading, we are using the reader to close the connection, so using statement
            // would be redundant and attempt to a double close of the object which is no longer valid
            SqliteConnection con = new SqliteConnection(constr);
            con.Open();
            // create a command object, using our sql string above against our connection object
            SqliteCommand cmd = new SqliteCommand(sql, con);
            SqliteDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (rdr.HasRows == true)
            {
                while (rdr.Read())
                {
                    Debug.Log("Record information in database");
                    Debug.Log("Primary Key: " + rdr["pk"].ToString());
                    Debug.Log("GUID: " + rdr["guid"].ToString());
                    Debug.Log("IP: " + rdr["lastip"].ToString());
                    Debug.Log("----------------------------------");
                }
            }
            else
            {
                Debug.Log("Database contains no records in gamedata table");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void DeleteAllRecords()
    {
        // this statement purges all records from the gamedata table, to only delete 1 record
        // use a WHERE clause
        string sql = "DELETE FROM gamedata";
        try
        {
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // create a command object, using our sql string above against our connection object
                SqliteCommand cmd = new SqliteCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void UpdateAllRecords()
    {
        try
        {
            // this is VERY generic, this will update ALL records and replace the GUID to this value
            // use WHERE clauses to update just one record by what ever logic you want
            string sql = "UPDATE gamedata SET GUID='1234567890';";
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // create a command object, using our sql string above against our connection object
                SqliteCommand cmd = new SqliteCommand(sql, con);
                Debug.Log("updating the record with some junk data for test UPDATE");
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    void InsertRecord()
    {
        // an insert statement that uses command parameters, NOTE, see the NULL, this is because the first value
        // is our primary key, this is an INTEGER, Automatically incremented, and not null, but we do nothing with it
        // however it is in the way and has to be used in the query, so we just say we are sending nothing and trusting
        // SQLLite to know what to do with it
        string sql = "INSERT INTO gamedata (pk, guid, lastip) Values (NULL, ?, ?); ";

        try
        {
            // Create a new connection using our connection string defined at the start of the class
            using (SqliteConnection con = new SqliteConnection(constr))
            {
                con.Open();
                // we shall keep things clean, so lets use parameters to store our values and pass up into our table
                SqliteParameter p0 = new SqliteParameter();
                p0.ParameterName = "@pguid";
                p0.DbType = DbType.String;
				p0.Value = "Value";// p.guid;

                SqliteParameter p1 = new SqliteParameter();
                p1.ParameterName = "@plastip";
                p1.DbType = DbType.String;
				p1.Value = "IPaddress";// p.ipAddress;

                try
                {
                    // create a command object, using our sql string above against our connection object
                    SqliteCommand cmd = new SqliteCommand(sql, con);
                    // add our parameters to the command object
                    cmd.Parameters.Add(p0);
                    cmd.Parameters.Add(p1);
                    // execute the command
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }}