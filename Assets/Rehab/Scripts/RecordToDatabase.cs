﻿using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;

public class RecordToDatabase : MonoBehaviour {
	public string databaseFilename = "gamedata.db";
	string constr ;
	string tableName;  // new table is created each time a game is started (using dateTime)
	public int skipFrames = 1;
	private int skipCounter;


	ZigJointId[] jointList = new ZigJointId[] { 
		ZigJointId.Head,  // 1
        ZigJointId.Neck,   // 2
		//	ZigJointId.Torso,  // 3       // kinect v2 only
				     //  #4: Waist is skipped
		//	ZigJointId.LeftCollar,  // 5   // kinect v2 only
		ZigJointId.LeftShoulder,   // 6
		ZigJointId.LeftElbow,  // 7
		ZigJointId.LeftWrist,   // 8
		ZigJointId.LeftHand,  // 9
		//	ZigJointId.LeftFingertip,  // 10   // kinect v2 only
		//	ZigJointId.RightCollar,  // 11     // kinect v2 only
		ZigJointId.RightShoulder,  // 12 
		ZigJointId.RightElbow,  // 13
		ZigJointId.RightWrist,  // 14
		ZigJointId.RightHand,  // 15
		//	ZigJointId.RightFingertip   // 16    // kinect v2 only
	};  

	// Use this for initialization
	void Start () {
		constr = "Data Source=" + databaseFilename + ";Version=3;Compress=False";
		skipCounter = skipFrames;
		Debug.Log("CreateDatabase being called");
		CreateDatabase();
		// Step 2, lets put some data in that then read it back to the log
	//	Debug.Log("InsertRecord being called");
//		InsertRecord(DateTime.Now, 1.2345f, 2.3456f, 3.45678f);

	//	Debug.Log("dbTest() being called");
	//	dbTest();

	//	Debug.Log("Done with SQLITE Test");
	}


	void CreateDatabase()
	{
		// example query, but this query creates the database which will be housed in our gamedata.db file if that file
		// does not exist, it will create the file automatically for us and there is nothing to write to make that happen

		tableName = "_" + DateTime.Now.ToString("MM_dd_yy__HH_mm_ss") + "__" + RehabMenu.currentGame;

		//		string sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (t DATETIME, x FLOAT, y FLOAT, z FLOAT); "; 

		string sql = "CREATE TABLE IF NOT EXISTS " + tableName + " (t DATETIME NOT NULL PRIMARY KEY";
		foreach (ZigJointId zig in jointList) {
			sql += ", x" + (int)zig + " FLOAT, y" + (int)zig + " FLOAT, z" + (int)zig + " FLOAT";
		}
		sql += "); "; 
		Debug.Log (sql);
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

	
	void InsertRecord(String sql)
	{
		// an insert statement that uses command parameters, NOTE, see the NULL, this is because the first value
		// is our primary key, this is an INTEGER, Automatically incremented, and not null, but we do nothing with it
		// however it is in the way and has to be used in the query, so we just say we are sending nothing and trusting
		// SQLLite to know what to do with it
		
		// sqlite reads datetime as yyyy-MM-dd HH:mm:ss
	//	string sql = "INSERT INTO " + tableName + 
	//		" (t, x" + (int)zigId + ", y" + (int)zigId + ", z" + (int)zigId + ") " +
	//		"VALUES ('" + d.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'," + x + "," + y + "," + z + "); ";
		
		//	string sql = "INSERT INTO " + tableName + " (x, y, z) " +
		//		"VALUES (" + x + "," + y + "," + z + "); ";

		//Debug.Log(sql);
		
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

	void dbTest() {
		DateTime sampleStart = DateTime.Now;
		string sql = "INSERT INTO " + tableName + " (t";
		float ttt = 1.0f;
		string insertVals =  ") " + "VALUES ('" + sampleStart.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
		foreach (ZigJointId joint in  jointList)
		{
			// Filter out joints that aren't used in Seated Mode
			if ((int)joint < 17 && (int)joint != 4) {
				
				sql += ", x" + (int)joint + ", y" + (int)joint + ", z" + (int)joint;
				insertVals += ", " + ttt+1.234f + ", " + ttt+2.345f + ", " + ttt+3.456f ;
				ttt++;
				
			}
		}
		insertVals += "); ";

		sql += insertVals;

		Debug.Log (sql);
		
		InsertRecord(sql);
	}


	void Zig_UpdateUser(ZigTrackedUser user)
	{
		skipCounter--;
		if (skipCounter < 0) {
			skipCounter = skipFrames;
		}
		else return;

		if (user.SkeletonTracked)
		{
			DateTime sampleStart = DateTime.Now;
			string sql = "INSERT INTO " + tableName + " (t";

			string insertVals =  ") " + "VALUES ('" + sampleStart.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
			foreach (ZigInputJoint joint in  user.Skeleton)
			{
				// Filter out joints that aren't used in Seated Mode
				if (joint.GoodPosition && ((int)joint.Id < 17 && (int)joint.Id != 4)) {

					sql += ", x" + (int)joint.Id + ", y" + (int)joint.Id + ", z" + (int)joint.Id;
					insertVals += ", " + joint.Position.x + ", " + joint.Position.y + ", " + joint.Position.z ;


				}
			}
			insertVals += "); ";
			
			sql += insertVals;
			
	//		Debug.Log (sql);
			
			InsertRecord(sql);
		}
	}
	
}