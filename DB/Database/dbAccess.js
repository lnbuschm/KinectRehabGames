/*  Javascript class for accessing SQLite objects.  
     To use it, you need to make sure you COPY Mono.Data.SQLiteClient.dll from wherever it lives in your Unity directory
     to your project's Assets folder
     Originally created by dklompmaker in 2009
     http://forum.unity3d.com/threads/28500-SQLite-Class-Easier-Database-Stuff    
     Modified 2011 by Alan Chatham
     *******************************************************
     Modifed 2014 by Jordan McCall
     - Added project specific functionality          
 */
import          System.Data;  // we import our  data class
import          Mono.Data.Sqlite; // we import sqlite
 
class dbAccess {
    // variables for basic query access
    private var connection : String;
    private var dbcon : IDbConnection;
    private var dbcmd : IDbCommand;
    private var reader : IDataReader;
	public var db_name = "RehabStats.sqdb";
	public var user_table = "UserProfiles";
	public var right_calib_table = "RightCalibration";
	public var left_calib_table = "LeftCalibration";
	public var scores_table = "Results";
	public var cylinder_reach_scene = "ReachBack";
	public var shoulder_rom_scene = "ShoulderROM";
	public var calibration_scene = "CalibrateGlove";
	private var default_first_name = 'Bob';
	private var default_last_name = 'Builder';
	
	/* Database table structures are defined here.  If you make any changes to these structures be sure to walk through
	all processes that touch these tables.  This can be done by searching for the defined table names above in the code base
	
    /* User Profiles Table Structure */
    private var user_field_names = new Array (
		"p_id",
		"created_dtm",	// Date of creation
		"first_name",
		"last_name",
		// Format of time series tables names is: "<Scene><Measurement>_<p_id>"
		// Default example: "ShoulderROMRot_1"
		"shoulder_rom_rot",	// Name of time series rotation table
		"shoulder_rom_pos",	// Name of time series position table
		"cyl_reach_rot",
		"cyl_reach_pos",
		"cyl_reach_glv"
	);
	private var user_field_values = new Array (
		"INTEGER PRIMARY KEY",
		"DATETIME",
		"VARCHAR(100)",
		"VARCHAR(100)",
		"VARCHAR(50)",
		"VARCHAR(50)",
		"VARCHAR(50)",
		"VARCHAR(50)",
		"VARCHAR(50)"
	);
	private var user_constraints = "";
	
	/* Calibration Table Structure */
	private var calib_field_names = new Array (
		"user_id",
		"index_0",
		"index_90",
		"middle_0",
		"middle_90",
		"ring_0",
		"ring_90",
		"pinky_0",
		"pinky_90",
		"knuckle_0",
		"knuckle_90"
	);	
	private var calib_field_values = new Array (
		"INTEGER",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL"
	);
	private var calib_constraints = "FOREIGN KEY(user_id) REFERENCES " + user_table + "(" + user_field_names[0] + ")";
	
	/* Right Calibration Table Defaults */
	private var default_right_calib_data = new Array (
		1,
		365,
		98,
		376,
		295,
		381,
		304,
		395,
		310,
		383,
		251
	);
	
	/* Left Calibration Table Defaults */
	private var default_left_calib_data = new Array (
		1,
		344,
		1,
		381,
		265,
		393,
		273,
		388,
		302,
		383,
		318
	);

	/* Results Table Structure */
	private var scores_field_names = new Array (
		"user_id",
		"reachback_count",
		"reachback_time",
		"l_shoulder_rom_max",
		"l_shoulder_rom_min",
		"r_shoulder_rom_max",
		"r_shoulder_rom_min",
		"l_tpose_time",
		"r_tpose_time",
		"l_arm_front_time",
		"r_arm_front_time"
	);	
	private var scores_field_values = new Array (
		"INTEGER",
		"INTEGER",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL"		
	);
	private var scores_constraints = "FOREIGN KEY(user_id) REFERENCES " + user_table + "(" + user_field_names[0] + ")";
	
	/* Structure for our time series glove table */
	private var gloves_field_names = new Array (
		"time_id",
		"l_index",
		"l_middle",
		"l_ring",
		"l_pinky",
		"l_thumb",
		"l_knuckle",
		"r_index",
		"r_middle",
		"r_ring",
		"r_pinky",
		"r_thumb",
		"r_knuckle"
	);	
	private var gloves_field_values = new Array (
		"DATETIME",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL"
	);
	private var gloves_constraints = "";
	
	/* Structure for our time series rotation and position tables */
	private var ts_field_names = new Array (
		"time_id",
		"l_shoulder_x",
		"l_shoulder_y",
		"l_shoulder_z",
		"r_shoulder_x",
		"r_shoulder_y",
		"r_shoulder_z",
		"l_elbow_x",
		"l_elbow_y",
		"l_elbow_z",
		"r_elbow_x",
		"r_elbow_y",
		"r_elbow_z",
		"l_wrist_x",
		"l_wrist_y",
		"l_wrist_z",
		"r_wrist_x",
		"r_wrist_y",
		"r_wrist_z",
		"l_hand_x",
		"l_hand_y",
		"l_hand_z",
		"r_hand_x",
		"r_hand_y",
		"r_hand_z",
		"head_x",
		"head_y",
		"head_z",
		"neck_x",
		"neck_y",
		"neck_z",
		"l_collar_x",
		"l_collar_y",
		"l_collar_z",
		"r_collar_x",
		"r_collar_y",
		"r_collar_z",
		"torso_x",
		"torso_y",
		"torso_z",
		"waist_x",
		"waist_y",
		"waist_z",
		"l_hip_x",
		"l_hip_y",
		"l_hip_z",
		"r_hip_x",
		"r_hip_y",
		"r_hip_z",
		"l_knee_x",
		"l_knee_y",
		"l_knee_z",
		"r_knee_x",
		"r_knee_y",
		"r_knee_z",
		"l_ankle_x",
		"l_ankle_y",
		"l_ankle_z",
		"r_ankle_x",
		"r_ankle_y",
		"r_ankle_z",
		"l_foot_x",
		"l_foot_y",
		"l_foot_z",
		"r_foot_x",
		"r_foot_y",
		"r_foot_z"
	);	
	private var ts_field_values = new Array (
		"DATETIME",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL",
		"REAL"
	);
	private var ts_constraints = "";
	 
    function OpenDB(){
    connection = "URI=file:" + db_name; // we set the connection to our database
    dbcon = new SqliteConnection(connection);
    dbcon.Open();
    //Debug.Log("Openned Database");
    }
    
    /* Return a rotations time series table name given a base scene and user id */
    function RotationsTableName(scene_name : String, id : int) {
    	return scene_name + "Rot_" + id;	
    }
    /* Return a positions time series table name given a base scene and user id */
    function PositionsTableName(scene_name : String, id : int) {
    	return scene_name + "Pos_" + id;
    }
    /* Return a glove data time series table name given a base scene and user id */
    function GlovesTableName(scene_name : String, id : int) {
    	return scene_name + "Glv_" + id;
    }
    
    /* Create necessary time series tables for a new user given their primary id */
    function CreateTimeSeriesTables(id : int) {
    	CreateTable(RotationsTableName(cylinder_reach_scene, id), ts_field_names, ts_field_values, ts_constraints);
		CreateTable(PositionsTableName(cylinder_reach_scene, id), ts_field_names, ts_field_values, ts_constraints);
		CreateTable(RotationsTableName(shoulder_rom_scene, id), ts_field_names, ts_field_values, ts_constraints);
		CreateTable(PositionsTableName(shoulder_rom_scene, id), ts_field_names, ts_field_values, ts_constraints);
		CreateTable(GlovesTableName(cylinder_reach_scene, id), gloves_field_names, gloves_field_values, gloves_constraints);
    }
    
    function InsertTimeSeriesRotations(id : int, scene_name : String, values : Array) {
    	var table_name = RotationsTableName (scene_name, id);
    	var query = "INSERT INTO " + table_name + " VALUES (datetime('now')";
    	for (var i = 0; i < values.length; i++) {
    		query += "," + values[i];
    	}
    	query += ")";
    	dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
    }
    
    function InsertTimeSeriesPositions(id : int, scene_name : String, values : Array) {
    	var table_name = PositionsTableName (scene_name, id);
    	var query = "INSERT INTO " + table_name + " VALUES (datetime('now')";
    	for (var i = 0; i < values.length; i++) {
    		query += "," + values[i];
    	}
    	query += ")";
    	dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
    }
    
    function InsertTimeSeriesGloveData(id : int, scene_name : String, values : Array) {
    	var table_name = GlovesTableName (scene_name, id);
    	var query = "INSERT INTO " + table_name + " VALUES (datetime('now')";
    	for (var i = 0; i < values.length; i++) {
    		query += "," + values[i];
    	}
    	query += ")";
    	dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
    }
    
    /* Regenerate ALL Database Tables */
    function GenerateTables(){
    	var default_id = 1;
    	
		var query = "INSERT INTO " + user_table + " VALUES (NULL,datetime('now'),'" + default_first_name + "','" + 
			default_last_name + "','" + RotationsTableName(shoulder_rom_scene, default_id) + 
			"','" + PositionsTableName(shoulder_rom_scene, default_id) + 
			"','" + RotationsTableName(cylinder_reach_scene, default_id) +
			"','" + PositionsTableName(cylinder_reach_scene, default_id) +
			"','" + GlovesTableName(cylinder_reach_scene, default_id) + "');";
		// Create user table and insert defaults
		CreateTable (user_table, user_field_names, user_field_values, user_constraints);
		BasicQuery(query);

		// Create results table and initialize a column for new user
		query = "INSERT INTO " + scores_table + " VALUES (1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);";
		CreateTable (scores_table, scores_field_names, scores_field_values, scores_constraints);
		BasicQuery(query); 

		// Create and initialize right and left calibration tables. Default data is loaded for default user
		CreateTable (right_calib_table, calib_field_names, calib_field_values, calib_constraints);
		InsertInto (right_calib_table, default_right_calib_data);
		CreateTable (left_calib_table, calib_field_names, calib_field_values, calib_constraints);
		InsertInto (left_calib_table, default_left_calib_data);
		
		CreateTimeSeriesTables(default_id);
		
    }
    
    /* Attempt to login to User Profiles table using a first and last name.  Returns TRUE if login
    is successful and FALSE if it fails */
    function AttemptLogin(first : String, last : String) {
    	var query = "SELECT first_name, last_name FROM " + user_table + " WHERE first_name='" + 
			first + 
			"' AND last_name='" +
			last +
			"';";
		var results = BasicQuery(query);
		// If nothing was returned from the query
		if (results.Count == 0) 
			return false;
		else
			return true;
    }
    
    /* Create a user if the specified first and last name doesn't already exist in the database.  Returns
    the user's p_id if successful creation and -1 if no user was created */
    function CreateUser(first : String, last : String) {
    	var query;
    	var results;
    	var user_id = GetUserId(first, last);
    	
    	if (user_id < 0) {
			// Insert user's name into database
			query = "INSERT INTO " + user_table + " VALUES (NULL,datetime('now'),'" + first + "','" + last + "',NULL,NULL,NULL,NULL,NULL);";
			results = BasicQuery(query);
			
			Debug.Log("Creating User: " + first + " " + last);
			// Get the new users p_id from user profiles table
			user_id = GetUserId(first, last);
			// Create a row in the high scores table for the user
			query = "INSERT INTO " + scores_table + " (user_id) " + "VALUES (" + user_id + ");";
			results = BasicQuery(query);
			
			// Create time series tables for user and add their names to profiles table
			query = "UPDATE " + user_table + " SET shoulder_rom_rot='" + RotationsTableName(shoulder_rom_scene, user_id) + 
				"',shoulder_rom_pos='" + PositionsTableName(shoulder_rom_scene, user_id) + 
				"',cyl_reach_rot='" + RotationsTableName(cylinder_reach_scene, user_id) + 
				"',cyl_reach_pos='" + PositionsTableName(cylinder_reach_scene, user_id) + 
				"',cyl_reach_glv='" + GlovesTableName(cylinder_reach_scene, user_id) +
				"' WHERE p_id=" + user_id + ";";
			results = BasicQuery(query);

			CreateTimeSeriesTables(user_id);
			
			return user_id;
    	}
    	return -1;
    	
    }
    
    /* Delete any and all data for specified user before removing the user from our user profiles table.
    Returns the p_id of the deleted user or -1 on failure. */
	function DeleteUser(first : String, last : String) {
		var query;
		var results;
		var user_id = GetUserId(first, last);

		// If a user was found attempt to delete any calibration data that may exist
		if (user_id > 0) {			
			// Delete data from all tables that use the primary key to reference information
			DeleteCalibrations(user_id);
			DeleteResults(user_id);
			DropTimeSeriesTables(user_id);
			
			// Delete user from UserProfiles
			query = "DELETE FROM " + user_table + " WHERE first_name='" + first + "' AND last_name='" + last + "';";
			results = BasicQuery(query);
			return user_id;
		} else {
			return -1;
		}
	}
	
	/* Given a user's id, this function will drop and recreate the user's associative time series tables */
	function ResetTimeSeriesTables(id : int){
		DropTimeSeriesTables(id);
		CreateTimeSeriesTables(id);
	}
	
	/* Delete associative time series tables for a user */
	function DropTimeSeriesTables(id : int) {
		DropTable(RotationsTableName(shoulder_rom_scene, id));
		DropTable(PositionsTableName(shoulder_rom_scene, id));
		DropTable(RotationsTableName(cylinder_reach_scene, id));
		DropTable(PositionsTableName(cylinder_reach_scene, id));
		DropTable(GlovesTableName(cylinder_reach_scene, id));
	}
	
	/* Delete all calibration data for this user. */
	function DeleteCalibrations(user_id : int) {
		var query = "DELETE FROM " + right_calib_table + " WHERE user_id=" + user_id + ";";
		var results = BasicQuery(query);
		query = "DELETE FROM " + left_calib_table + " WHERE user_id=" + user_id + ";";
		results = BasicQuery(query);
	}
	
	/* Delete any high score data for given user */
	function DeleteResults(user_id : int) {
		var query = "DELETE FROM " + scores_table + " WHERE user_id=" + user_id +";";
		var results = BasicQuery(query);
	}
    
    /* Return a user's p_id based on first and last name input */
    function GetUserId(first : String, last : String) {
		var query = "SELECT p_id FROM " + user_table + " WHERE first_name='" + first + "' AND last_name='" + last + "';";
		var results = BasicQuery(query);
		if (results.Count > 0)
			return results[0][0];
		else
			return -1;
	}
	
	/* Check if a user has calibration data in the database.  The function will check for data
	in the right calibration table only.  There should not exist a case where only one side is calibrated */
	function HasCalibrations(user_id : int) {
		var query = "SELECT COUNT(*) FROM " + right_calib_table + " WHERE user_id=" + user_id + ";";
		var results = BasicQuery(query);

		// Return false is 0 is empty
		if (results[0][0] < 1)
			return false;
		else
			return true;
	}
 
    function BasicQuery(q : String){ // run a baic Sqlite query
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = q; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        var readArray = new ArrayList();
        while(reader.Read()){ 
            var lineArray = new ArrayList();
            for (i = 0; i < reader.FieldCount; i++)
                lineArray.Add(reader.GetValue(i)); // This reads the entries in a row
            readArray.Add(lineArray); // This makes an array of all the rows
        }
        return readArray; // return matches
        
    }
    
   	function TableExists(tableName : String){
 		var query : String;
 		var tableExists;
 		query = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '" + tableName + "'";
 		dbcmd = dbcon.CreateCommand();
 		dbcmd.CommandText = query;
 		reader = dbcmd.ExecuteReader();
 		if (reader.GetValue(0) == 0)
 			return false;
 		else
 			return true;
 	}
 	
 	function DropTable(tableName : String){
 		var query : String;
 		query = "DROP TABLE " + tableName;
 		dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
 	}
 
 	// This returns a 2 dimensional ArrayList with all the
    //  data from the table requested
    function ReadFullTable(tableName : String){
        var query : String;
        query = "SELECT * FROM " + tableName;	
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
        var readArray = new ArrayList();
        while(reader.Read()){ 
            var lineArray = new ArrayList();
            for (i = 0; i < reader.FieldCount; i++)
                lineArray.Add(reader.GetValue(i)); // This reads the entries in a row
            readArray.Add(lineArray); // This makes an array of all the rows
        }
        return readArray; // return matches
    }
    
    function SelectTableWhere(tableName : String, field : String, compare){
        var query : String;
        if (typeof compare === int)
        	query = "SELECT * FROM " + tableName + " WHERE " + field + "="  + compare;	
        else
        	query = "SELECT * FROM " + tableName + " WHERE " + field + "='"  + compare + "'";
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
        var readArray = new ArrayList();
        while(reader.Read()){ 
            var lineArray = new ArrayList();
            for (i = 0; i < reader.FieldCount; i++)
                lineArray.Add(reader.GetValue(i)); // This reads the entries in a row
            readArray.Add(lineArray); // This makes an array of all the rows
        }
        return readArray; // return matches
    }
 
    // This function deletes all the data in the given table.  Forever.  WATCH OUT! Use sparingly, if at all
    function DeleteTableContents(tableName : String){
    var query : String;
    query = "DELETE FROM " + tableName;
    dbcmd = dbcon.CreateCommand();
    dbcmd.CommandText = query; 
    reader = dbcmd.ExecuteReader();
    }
 
    function CreateTable(name : String, col : Array, colType : Array, constraints : String){ // Create a table, name, column array, column type array
        var query : String;
        query  = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
        for(var i = 1; i < col.length; i++){
            query += ", " + col[i] + " " + colType[i];
        }
        if (constraints) {
        	query += ", " + constraints;
        }
        query += ")";
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = query; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
 
    }
 
    function InsertIntoSingle(tableName : String, colName : String, value : String){ // single insert 
        var query : String;
        query = "INSERT INTO " + tableName + "(" + colName + ") " + "VALUES (" + value + ")";
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = query; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
    }
 
    function InsertIntoSpecific(tableName : String, col : Array, values : Array){ // Specific insert with col and values
        var query : String;
        query = "INSERT INTO " + tableName + "(" + col[0];
        for(var i=1; i<col.length; i++){
            query += ", " + col[i];
        }
        query += ") VALUES ('" + values[0];
        for(i=1; i<values.length; i++){
            query += "', '" + values[i];
        }
        query += "')";
        Debug.Log("Attempted query: " + query);
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
    }
 
    function InsertInto(tableName : String, values : Array){ // basic Insert with just values
        var query : String;
        query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for(var i=1; i<values.length; i++){
            query += ", " + values[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader(); 
    }
 
    // This function reads a single column
    //  wCol is the WHERE column, wPar is the operator you want to use to compare with, 
    //  and wValue is the value you want to compare against.
    //  Ex. - SingleSelectWhere("puppies", "breed", "earType", "=", "floppy")
    //  returns an array of matches from the command: SELECT breed FROM puppies WHERE earType = floppy;
    function SingleSelectWhere(tableName : String, itemToSelect : String, wCol : String, wPar : String, wValue : String){ // Selects a single Item
        var query : String;
        query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + wValue;	
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
        var readArray = new Array();
        while(reader.Read()){ 
            readArray.Push(reader.GetString(0)); // Fill array with all matches
        }
        return readArray; // return matches
    } 
 
    function CloseDB(){
    	if (reader != null)
        	reader.Close(); // clean everything up
        reader = null; 
        if (dbcmd != null)
       	 dbcmd.Dispose(); 
        dbcmd = null; 
        if (dbcon != null) {
        dbcon.Close(); 
        dbcon = null;
        Debug.Log("Closed Database");
        } 
        //Debug.Log("Closed Database");
    }
 
}