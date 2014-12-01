import System.Data;
import Mono.Data.Sqlite;

private var db_control : dbAccess;
private var menu_scroll : Vector2;
private var data_scroll : Vector2;

private var txt_field_style : GUIStyle;
private var userselect_button : GUIStyle;
private var menu_button : GUIStyle;
private var skin : GUISkin;
private var menu_box : Rect;
private var display_box : Rect;
private var active_table : String;
private var active_user : int;
private var display_time_series : boolean = false;

public var first_name = "First Name";
public var last_name = "Last Name";
public var txt_field_width : int = 300;
public var txt_field_height : int = 40;
public var button_width : int = 200;
public var user_selection = true;
public var display_all_rows = false;


// Use this for initialization
function Start () {
	db_control = new dbAccess();
	active_table = db_control.user_table;
	active_user = 1;
	// Define GUI area sizes
	// Menu box 1/4 the screen with
	menu_box = new Rect(50, 50, Screen.width / 5, Screen.height - 100);
	// Start display box to the left of menu box with a little space in between (also 3/4) screen width
	display_box = new Rect(menu_box.xMax + 25, 50, Screen.width * 0.80, Screen.height - 100);
}

// Update is called once per frame
function Update () {

}

function OnGUI() {
	// Set up GUI styles
	skin = GUI.skin;
	userselect_button = new GUIStyle(skin.GetStyle("Button"));
	menu_button = new GUIStyle(skin.GetStyle("Button"));
	txt_field_style = new GUIStyle(skin.GetStyle("TextField"));
	txt_field_style.fontSize = 20;
	userselect_button.alignment = TextAnchor.MiddleCenter;
	menu_button.alignment = TextAnchor.MiddleLeft;
	
	// Set up display box.  It's just about full screen
	GUI.Box(new Rect (25,25,Screen.width - 50, Screen.height - 50),"");
	
	// Menu Area Begin 
	GUILayout.BeginArea(menu_box);
	//menu_scroll = DisplayFullTable(db_control.user_table, menu_scroll);
	menu_scroll = DisplayMenu(menu_scroll);
	// Menu Area End
	GUILayout.EndArea();
	
	// Display Area Begin 
	GUILayout.BeginArea(display_box);
	if (display_all_rows || user_selection || display_time_series)
		data_scroll = DisplayFullTable(active_table, data_scroll, null);
	else
		data_scroll = DisplayFullTable(active_table, data_scroll, active_user);
	// Display Area End
	GUILayout.EndArea();
	
}

/* Diplay a scroll menu of all user options */
function DisplayMenu(scroll_position : Vector2) {
	// Button Dimensions
	var button_width : int = 160;
	var button_height : int = 40;
	
	GUILayout.Label("Active User: " + active_user);
	// Toggle button to enable full table views instead of user specific views
	display_all_rows = GUILayout.Toggle(display_all_rows, "Display All Rows");
	
	scroll_position = GUILayout.BeginScrollView(scroll_position, GUILayout.Height(Screen.height - 100));
	
	GUILayout.BeginHorizontal();
	// Select user ?
	if (GUILayout.Button("User Select", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		user_selection = true;
		display_time_series = false;
	}
	GUILayout.EndHorizontal();
	GUILayout.BeginHorizontal();
	// Display left hand calibrations
	if (GUILayout.Button("Display Calibrations \nLeft", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.left_calib_table;
		display_time_series = false;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.BeginHorizontal();
	// Display right hand calibrations
	if (GUILayout.Button("Display Calibrations \nRight", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.right_calib_table;
		display_time_series = false;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.BeginHorizontal();
	// Display the scores table
	if (GUILayout.Button("Display Results", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.scores_table;
		display_time_series = false;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.BeginHorizontal();
	// Display time series positions for ReachBack
	if (GUILayout.Button("Display ReachBack \nTimeseries Positions", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.PositionsTableName(db_control.cylinder_reach_scene, active_user);
		display_time_series = true;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.BeginHorizontal();
	// Display time series rotations for ReachBack
	if (GUILayout.Button("Display ReachBack \nTimeseries Rotations", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.RotationsTableName(db_control.cylinder_reach_scene, active_user);
		display_time_series = true;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.BeginHorizontal();
	// Display time series positions for ShoulderROM
	if (GUILayout.Button("Display ShoulderROM \nTimeseries Positions", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.PositionsTableName(db_control.shoulder_rom_scene, active_user);
		display_time_series = true;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.BeginHorizontal();
	// Display time series rotations for ShoulderROM
	if (GUILayout.Button("Display ShoulderROM \nTimeseries Rotations", menu_button, GUILayout.Height(button_height), GUILayout.Width(button_width))) {
		active_table = db_control.RotationsTableName(db_control.shoulder_rom_scene, active_user);
		display_time_series = true;
	}
	GUILayout.EndHorizontal();
	
	GUILayout.EndScrollView();
	
	return scroll_position;

}

/* Display all rows in the specified table.  All but one table will displayed in the same way.  The users table will be displayed with
 * selection buttons so the user can change users.  Also the users table will only display it's relevant columns.  This function will
 * use the global variable "user_selection" to determine whether or not a user table format will be used
 * TODO: Update this doc string. This function is kinda complicated now.  Probably should have broken it up..byte. */
function DisplayFullTable(table_name : String, scroll_position : Vector2, user_id){
	// Set if user table name is passed in
	var database_data : ArrayList = new ArrayList();
	// Display Table Name
	if (user_selection)
		DisplayHorizLabel("Please Select a User");
	else
		DisplayHorizLabel("Table Name: " + table_name);
	// Open, read, and close database
	db_control.OpenDB();
	// If user_id is passed in undefined OR we have a time series table, read and display the entire table
	if (user_id == null)
		database_data = db_control.ReadFullTable(table_name);
	// Else select all where user_id = our active user
	else {
		var field = (table_name == db_control.user_table) ? "p_id" : "user_id";
		database_data = db_control.SelectTableWhere(table_name, field, user_id);
	}
	
	db_control.CloseDB();
	//GUILayout.Label("Database Contents");
	scroll_position = GUILayout.BeginScrollView(scroll_position, GUILayout.Height(Screen.height - 100));
	
	for (var line : ArrayList in database_data){
	
		GUILayout.BeginHorizontal();
		if (user_selection) {
			// Display first 4 fields of user profiles table, id, created datetime, and name (first, last)
			GUILayout.Label(line[0].ToString(), GUILayout.Width(0));
			GUILayout.Label(line[1].ToString(), GUILayout.Width(0));
			GUILayout.Label(line[2].ToString(), GUILayout.Width(0));
			GUILayout.Label(line[3].ToString(), GUILayout.Width(0));

			// Check if the user has made a selection
			if (GUILayout.Button("Select", userselect_button, GUILayout.Width(100))) {
				Debug.Log("You selected user: " + line[0]);
				active_user = line[0];
				user_selection = false;
			}
		} else {
			// Print every column value in row
			for (var s in line)
				GUILayout.Label(s.ToString(), GUILayout.Width(0));
		}
		GUILayout.EndHorizontal();
	}
	GUILayout.EndScrollView();
	
	return scroll_position;
}

/* Display a horizontal label. This label will get its own horizontal space */
function DisplayHorizLabel(msg : String){
	GUILayout.BeginHorizontal();
	GUILayout.Label(msg);
	GUILayout.EndHorizontal();
}


/* 
When play mode is stopped make sure to close the database.  This helps avoid some database
issues upon early exit.  If database is not open, nullreference exception will be thrown. It's 
ok to ignore this.
*/
function OnApplicationQuit() {
	db_control.CloseDB ();
}
