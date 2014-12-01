/*
This script will set up database tables accordingly and allow a user to login.
NOTE: If you wish to change the structure of a table and re-create it, you will need to regenerate
the entire database.  This is done by deleting the corresponding database file from the 
projects root folder.  For example "RehabStats.sqdb". Delete that and re-run the script.  It 
will be automatically generated. 
*/

import System.Data;
import Mono.Data.Sqlite;

private var db_control : dbAccess;
private var user_found = false;
private var failed_user_login = false;
private var failed_user_create = false;
private var failed_user_delete = false;
private var force_calibration = false;
private var drop_timeseries_data = false;
private var row;
private var query;
private var scroll_position : Vector2;

private var results : ArrayList = new ArrayList();
private var database_data : ArrayList = new ArrayList();
private var txt_field_style : GUIStyle;

public var display_calibrations = true;
public var display_users = true;
public var display_scores = true;
public var first_name = "First Name";
public var last_name = "Last Name";
public var txt_field_width : int = 300;
public var txt_field_height : int = 40;
public var button_width : int = 200;
public var calibration_button_pos : Rect = new Rect(620, 26, 150, 20);
public var timeseries_button_pos : Rect = new Rect(620, 44, 170, 20);


// Use this for initialization
function Start () {
	// Boolean indicating whether table already exists
	var user_table_exists;
	db_control = new dbAccess();
	// Open up database.  Will create database if it doesn't exist.
	db_control.OpenDB ();

	// Check if user profile table exists and create it if not
	user_table_exists = db_control.TableExists(db_control.user_table);
	if (!user_table_exists)
		db_control.GenerateTables();
		
	db_control.CloseDB();
	
	// Set database table information in player prefs
	SetDatabasePrefs();

}

// Update is called once per frame
function Update () {

}

function OnGUI() {
	// Set up display box.  It's just about full screen
	GUI.Box(new Rect (25,25,Screen.width - 50, Screen.height - 50),"");
	GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height - 100));
	
	// Initialize GUIStyles
	txt_field_style = GUI.skin.textField;
	txt_field_style.fontSize = 20;
	
	// Display our welcome message
	DisplayHorizLabel("Welcome to Some Cool Isshhh. Please Login Below:");
	
	// If a user has not been found already, display login menu
	if (!user_found){
		LoginOptions();
	} else {
		Debug.Log("A User Has Been Found: " + first_name + " " + last_name);
	}
	
	// Display helpful messages for failed user logins, creations, deletions
	if (failed_user_login)
		DisplayHorizLabel("Login Failed. User Not Found");
	if (failed_user_create)
		DisplayHorizLabel("User Already Exists. Please Login");
	if (failed_user_delete)
		DisplayHorizLabel("Failed to Delete User");
		
	// Show all users and calibrations currently in database
	if (!user_found) {
		// Display user profiles
		if (display_users)
			DisplayFullTable(db_control.user_table);
		// Display user calibrations
		else if (display_calibrations) {
			DisplayFullTable(db_control.right_calib_table);
			DisplayFullTable(db_control.left_calib_table);
		}
		// Display user high scores
		else if (display_scores)
			DisplayFullTable(db_control.scores_table);
	}
	GUILayout.EndArea ();
	
}

function LoginOptions() {
	// This first block allows us to enter new entries into our table
	GUILayout.BeginHorizontal();
	//GUILayout.FlexibleSpace();
	first_name = GUILayout.TextField(first_name, txt_field_style, GUILayout.Width (txt_field_width), GUILayout.Height(txt_field_height));
	last_name = GUILayout.TextField(last_name, GUILayout.Width (txt_field_width), GUILayout.Height(txt_field_height));
	force_calibration = GUI.Toggle(calibration_button_pos, force_calibration, "Force Calibration");
	drop_timeseries_data = GUI.Toggle(timeseries_button_pos, drop_timeseries_data, "Reset Time Series Data");

	//GUILayout.FlexibleSpace();
	GUILayout.EndHorizontal();
	
	// Display all buttons for login/creation/deletion
	GUILayout.BeginHorizontal();
	//GUILayout.FlexibleSpace();
	
	/*****************************************************************/
	// Attempt to login with user name and load next scene if successful. Else fall back
	if (GUILayout.Button("Login", GUILayout.Width(button_width))){
		// A user creation or deletion was not atempted
		failed_user_create = false;
		failed_user_delete = false;
		db_control.OpenDB();
		if (db_control.AttemptLogin(first_name, last_name)) {
		db_control.CloseDB();
			user_found = true;
			failed_user_login = false;
			failed_user_create = false;
			// Set active user for project
			SetActiveUser(first_name, last_name);
			LoadNextScene();
		} else {
			db_control.CloseDB();
			failed_user_login = true;
			failed_user_create = false;
			Debug.Log("User Does Not Exists");
		}
	}
	
	/*******************************************************************/
	// Attempt to create a user here
	if (GUILayout.Button("Create User", GUILayout.Width(button_width))) {
		db_control.OpenDB();
		// A user deletion or login was not attempted
		failed_user_delete = false;
		failed_user_login = false;
		
		if (db_control.CreateUser(first_name, last_name) < 0) {
			failed_user_create = true;
			Debug.Log("This User Already Exists");
			db_control.CloseDB();
		} else {
			db_control.CloseDB();
			user_found = true;
			failed_user_create = false;
			// Set active user for project and load next scene
			SetActiveUser(first_name, last_name);
			LoadNextScene();
		}		
	}
	/*******************************************************************/
	// Attempt to delete a user
	if (GUILayout.Button("Delete User", GUILayout.Width(button_width))) {
		db_control.OpenDB();
		db_control.DeleteUser(first_name, last_name);
		db_control.CloseDB();
		failed_user_create = false;
		failed_user_login = false;		
	}
	
	//GUILayout.FlexibleSpace();
	GUILayout.EndHorizontal();
}

/* Display all rows in the specified table.  This is helpful for debugging purposes. */
function DisplayFullTable(table_name : String){
	// Display Table Name
	DisplayHorizLabel("Contents of: " + table_name);
	db_control.OpenDB();
	database_data = db_control.ReadFullTable(table_name);
	db_control.CloseDB();
	//GUILayout.Label("Database Contents");
	scroll_position = GUILayout.BeginScrollView(scroll_position, GUILayout.Height(100));
	for (var line : ArrayList in database_data){
		GUILayout.BeginHorizontal();
		for (var s in line){
			GUILayout.Label(s.ToString(), GUILayout.Width(0));
		}
		GUILayout.EndHorizontal();
	}
	GUILayout.EndScrollView();
}

/* Display a horizontal label. This label will get its own horizontal space */
function DisplayHorizLabel(msg : String){
	GUILayout.BeginHorizontal();
	GUILayout.Label(msg);
	GUILayout.EndHorizontal();
}

/* Set the active user for the project. Do this using PlayerPrefs as they can be accessed throughout the project */
function SetActiveUser(first_name : String, last_name : String) {
	db_control.OpenDB();
	var user = db_control.GetUserId(first_name, last_name);
	db_control.CloseDB();
	// Set the PlayerPref
	PlayerPrefs.SetInt("ActiveUser", user);
}

/* Set database preferences for future processes */
function SetDatabasePrefs() {
	PlayerPrefs.SetString("DBName", db_control.db_name);
	PlayerPrefs.SetString("RightCalibrationTable", db_control.right_calib_table);
	PlayerPrefs.SetString("LeftCalibrationTable", db_control.left_calib_table);
	PlayerPrefs.SetString("UserTable", db_control.user_table);
	PlayerPrefs.SetString("ScoresTable", db_control.scores_table);
}

/* 
Load the next appropriate scene.  If the user does not have any calibration data, they will be
directed to the calibration scene. Other wise to the game scene 
*/
function LoadNextScene(){
	var user_id = PlayerPrefs.GetInt("ActiveUser");
	db_control.OpenDB();
	// Check if time series tables should be reset
	if (drop_timeseries_data)
		db_control.ResetTimeSeriesTables(user_id);
	// If a zero count is returned by the query OR force_calibration is true, proceed to calibration scene
	if ((!db_control.HasCalibrations(user_id)) || (force_calibration)){
		// If calibration is forced by the user, attempt to delete any existing calibration data
		if (force_calibration) {
			Debug.Log("Forcing Calibration for user");
			db_control.DeleteCalibrations(PlayerPrefs.GetInt("ActiveUser"));
		}
		db_control.CloseDB();
		Application.LoadLevel(db_control.calibration_scene);
		Debug.Log("Loading Calibration Scene");
	// If a calibration entry is found, go straight to our game scene
	} else {
	Debug.Log("Loading Scene Selection");
		db_control.CloseDB();
		Application.LoadLevel("SceneSwitch");
	}	
}

/* 
When play mode is stopped make sure to close the database.  This helps avoid some database
issues upon early exit.
*/
function OnApplicationQuit() {
	db_control.CloseDB ();
	PlayerPrefs.Save();
}
