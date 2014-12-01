using UnityEngine;
using System.Collections;

public class RecordAndMoveCalibrate : MonoBehaviour {
	
	private bool moving = false;
	private static float amount;
	public GUIText instructions;
//	GloveReader reader;
	
	private dbAccess db_control;
	// These 29 fields represent all the columns in the calibration table.  Storing them in 
	// a large list allows us to simple pass the structure into a database control function
	// for the database insertion. NOTE: Index zero is the active user_id.
	private float[] rightCalibrationPoints = new float[11];
	private float[] leftCalibrationPoints = new float[11];
	
	public const int MAX_TRANSITIONS = 3;
	private int transitionCount = 0;
	private int[] finger_extensions = {100, 0, 100, 0};

	// Database Player Preference Information (from Login Scene)
	private int active_user;
	private string db_name;
	private string right_calibration_table;
	private string left_calibration_table;
	private string user_table;
	
	public string next_level = "SceneSwitch";
	
	/*
	// Use this for initialization
	void Start () {
		moving = false;
//		reader = new GloveReader ();
		amount = 0.25f;
		
		// Set up database info and open connection
		SetupDatabase ();
		rightCalibrationPoints [0] = leftCalibrationPoints [0] = active_user;
		//db_control.InsertInto(calibration_table_name, rightCalibrationPoints);
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		
		// We've gone through all the images at this point so our calibration data structure
		// should be full now.  Let's store this data in the database now.
		if (transitionCount > MAX_TRANSITIONS){
			SaveData();
			Application.LoadLevel(next_level);
		} else {
			// Update GUI text to display proper instructions
			instructions.text = "Copy the hand position \n" +
				"shown in the image. \n" +
				"Finger Extension: " + 
				finger_extensions[transitionCount] + "%";
		}
		
		// Update GUI text to display proper instructions
		instructions.text = "Copy the hand position \n" +
			"shown in the image. \n" +
				"Finger Extension: " + 
				finger_extensions[transitionCount] + "%";
		Debug.Log("Length of finger extensions: " + finger_extensions.Length + "\nTransition Count: " + transitionCount);
		
		if(Input.GetKeyDown("space") && position.x % 20 == 0) {
			Record();
			transitionCount++;
			moving = true;
		}
		if (moving) {
			position.x = position.x + amount;
			if (position.x % 20 == 0) moving = false;
		}
		transform.position = position;
	}
	
	void Record() {
		// If you aren't able to get any data into the source file, use some dummy values for testing
		//float [] values = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
	//	float [] values = reader.getValues();
		int index = 0;
		Debug.Log (values.Length);
		// Set calibrations for the right hand during first 7 transitions
		if (transitionCount < 2) {
			// Set base index for reference
			index = transitionCount + 1;
			rightCalibrationPoints [index] = values[reader.RH_IndexFinger()];
			rightCalibrationPoints [index + 2] = values[reader.RH_MiddleFinger()];	
			rightCalibrationPoints [index + 4] = values[reader.RH_RingFinger()];	
			rightCalibrationPoints [index + 6] = values[reader.RH_PinkyFinger()];
			rightCalibrationPoints [index + 8] = values[reader.RH_Knuckle()];
		} else if (transitionCount < 4) {
			// Set base index for reference
			//Debug.Log ("yo");
			index = transitionCount - 1;
			leftCalibrationPoints [index] = values[reader.LH_IndexFinger()];
			leftCalibrationPoints [index + 2] = values[reader.LH_MiddleFinger()];	
			leftCalibrationPoints [index + 4] = values[reader.LH_RingFinger()];	
			leftCalibrationPoints [index + 6] = values[reader.LH_PinkyFinger()];
			leftCalibrationPoints [index + 8] = values[reader.LH_Knuckle()];
		} else {
			Debug.Log("End of scene. No more data to capture.");
		}
	}
	
	// Setup our database here.  First grab all data saved as PlayerPreferences and capture it locally.
	// Next instantiate our database access object and open the database
	void SetupDatabase() {
		Debug.Log ("Setting up the database for calibration");
		// Capture DB info from Player Preferences
		active_user = PlayerPrefs.GetInt ("ActiveUser", 1);
		db_name = PlayerPrefs.GetString ("DBName", "RehabStats.sqdb");
		right_calibration_table = PlayerPrefs.GetString ("RightCalibrationTable", "RightCalibration");
		left_calibration_table = PlayerPrefs.GetString ("LeftCalibrationTable", "LeftCalibration");
		user_table = PlayerPrefs.GetString ("UserTable", "UserProfiles");
	}
	
	// This function will insert our calibration data points as a row in the calibration database table
	void SaveData(){
		// Instantiate instance of db access controller and open up our database
		db_control = new dbAccess ();
		db_control.OpenDB ();
		db_control.InsertInto(right_calibration_table, rightCalibrationPoints);
		db_control.InsertInto(left_calibration_table, leftCalibrationPoints);
		db_control.CloseDB ();
	}
	
	void OnApplicationQuit() {
		db_control.CloseDB ();
		PlayerPrefs.Save ();
	}
	*/
}
