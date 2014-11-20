using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System;

public class RehabMenu : MonoBehaviour {
	public GUIText rightHandGUI;
	public GUIText leftHandGUI;
	public GUIText scoreGUI;
	public GUIText warningGUI;
	public GUIText summaryLine1GUI;
	public GUIText summaryLine2GUI;
	public Camera camera;
	public static int theScore = 0;
	public static int lhScore = 0;
	public static int rhScore = 0;
	public static int rightHandMultiplier = 10;
	public static int leftHandMultiplier = 1;
	public static bool warningRH = false;
	public static bool warningLH = false;
	public int menuTime = 5000;  // in ms
	static Timer _multiplierTimer; // From System.Timers
	
	Timer _textTimer1; 
	Timer _menuTimer1; 

	static int pointValue = 1;

	static int[][] multipliers = new int[][] { new int[] {1,1}, new int[] {1,2}, new int[] {1,5}, new int[] {1,10} };

	void Session_Start(Vector3  focusPoint) {
		Debug.Log ("RehabMenu Session_Start()");
	}
	
	void Session_Update(Vector3 handPoint) {
		Debug.Log ("RehabMenu Session_Update()");
	}
	
	void Session_End() {
		Debug.Log ("RehabMenu Session_End()");
	}

	public  void EnableRHWarning() {
		warningRH = true; 
		_textTimer1.Start ();
	}
	public  void EnableLHWarning() {
		warningLH = true; 
		_textTimer1.Start ();
	}
	//
	// Timers
	//
	
	void _textTimer1_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (warningLH) 
				warningLH = false;
		if (warningRH)
				warningRH = false; 
	}
	static void _multiplierTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		//theScore += 20;
		System.Random rand = new System.Random ();

		int rand1 = (int)Math.Floor ((double)rand.Next(0, multipliers.Length));
		int rand2 = (int)Math.Floor ((double)rand.Next (0, multipliers[rand1].Length));
		rightHandMultiplier = multipliers [rand1] [0];
		leftHandMultiplier = multipliers [rand1] [1];
	}
	private bool summaryScreenToggle = false;

	private int menuScreenCount = 0;
	void _menuTimer1_Elapsed(object sender, ElapsedEventArgs e)
	{

		_menuTimer1.Stop ();
		summaryScreenToggle = true;
	}
	bool showNextRoundScreen1 = false;
	void showNextRoundScreen(){
	//	Debug.Log ("SHOW NEXT ROUND SCREEN!!");
		//	summaryScreenToggle = true;
	//	camera.transform.Rotate (new Vector3 (0, 180, 0));

		//  randomize the next round difficulty

		if (RehabGestures.rightHandDominant) {
						summaryLine1GUI.text = "Eggs will fall FAST when you use your right side";
						summaryLine2GUI.text = "Eggs will fall SLOW if you use your left side";
				} else {

						summaryLine1GUI.text = "Eggs will fall SLOW when you use your right side";
						summaryLine2GUI.text = "Eggs will fall FAST if you use your left side";
				}

		scoreGUI.text = "The next round is about to begin";
		_menuTimer1.Start ();
		showNextRoundScreen1 = false;
	}
		
	void showRoundSummaryScreen() {
	//	Debug.Log ("SHOW ROUDN SUMMARY!!");
	//	summaryScreenToggle = true;
		Time.timeScale = 0;
		camera.transform.Rotate (new Vector3 (0, 180, 0));
		int strongScore = 0;

		int weakScore = 0;
		if (RehabGestures.rightHandDominant) {
						strongScore = rhScore;
						weakScore = lhScore;
				} else {
						strongScore = lhScore;
						weakScore = rhScore;
				}
		summaryLine1GUI.text =  "You caught " + strongScore + " eggs with your strong side!";
		summaryLine2GUI.text =  "You caught " + weakScore + " eggs with your weak side!";
		scoreGUI.text = "Total Score: " + theScore;
		_menuTimer1.Start ();
	}

	public static void Score() {
		if (RehabGestures.rightHandActive) {
			rhScore++;
						theScore += rightHandMultiplier * pointValue;
				} else {
			lhScore++;
						theScore += leftHandMultiplier * pointValue;
				}
	}

	//OnGUI is called multiple times per frame. Use this for GUI stuff only!
	void OnGUI()
	{
		//We display the game GUI from the playerscript
		//It would be nicer to have a seperate script dedicated to the GUI though...
		//GUILayout.Label(
		//GUILayout.Label("Score: " + theScore);

		if (summaryScreenToggle) {
			menuScreenCount++;
			summaryScreenToggle=false;
			if (menuScreenCount == 1) {
				showNextRoundScreen1 = true;
			
			} 
			else {
				camera.transform.Rotate (new Vector3 (0, 180, 0));
				Time.timeScale=1;
				summaryLine1GUI.text = "";
				summaryLine2GUI.text = "";
				scoreGUI.text = "";
				menuScreenCount = 0;
			}


		}
		if (showNextRoundScreen1) showNextRoundScreen();

		rightHandGUI.text = "Right Hand: " + rightHandMultiplier + "X";
		leftHandGUI.text = "Left Hand : " + leftHandMultiplier + "X";

		if (RehabGestures.rightHandActive) {
			rightHandGUI.color = Color.yellow;
			leftHandGUI.color = Color.black;
		}
		else {
			leftHandGUI.color = Color.yellow;
			rightHandGUI.color = Color.black;
		}

	//	scoreGUI.text = "Score: " + theScore;

		if (warningRH || warningLH) {
			warningGUI.text = "Keep Hand Level";
	//		Debug.LogWarning ("WARNING GUI ENABLED");
		}
		else warningGUI.text = "";
			//	warningGUI.enabled = false;
					



		//scoreGUI.transform.TransformDirection( -Screen.width/2, Screen.height/2, 0);
	//	guiText.transform.position = Camera.mainCamera.WorldToViewportPoint(transform.parent.position + new Vector3(-Screen.width/2, Screen.height/2, 0)); 
		//GameObject.FindGameObjectWithTag(
	//	Debug.Log ("Kakaka");
		//guiText = "lalala";
	//	GameObject.Find (-1063142)
	}   


	public int guiPadding = 10;
	// Use this for initialization
	void Start () {
		Debug.Log ("RehabMenu Start()");
		GUI.enabled = true;
	//	warningGUI.enabled = false;
		warningGUI.text = "";
		_multiplierTimer = new Timer(3000); // Set up the timer for 3 seconds
		_multiplierTimer.Elapsed += new ElapsedEventHandler(_multiplierTimer_Elapsed);
		_multiplierTimer.Enabled = true; // Enable it
		
		_textTimer1 = new Timer(3000); // Set up the timer for 3 seconds
		_textTimer1.Elapsed += new ElapsedEventHandler(_textTimer1_Elapsed);
		_textTimer1.Enabled = true; // Enable it

		_menuTimer1 = new Timer(menuTime); // Set up the timer for 3 seconds
		_menuTimer1.Elapsed += new ElapsedEventHandler(_menuTimer1_Elapsed);
		_menuTimer1.Enabled = true; // Enable it

	//	showNextRoundScreen ();
		showRoundSummaryScreen ();

	//	scoreGUI.pixelOffset = new Vector2(-Screen.width/2+guiPadding , Screen.height/2-guiPadding);
		rightHandGUI.pixelOffset = new Vector2(Screen.width/2-guiPadding , Screen.height/2-guiPadding);
		leftHandGUI.pixelOffset = new Vector2(Screen.width/2-guiPadding , Screen.height/2-rightHandGUI.fontSize-2*guiPadding);
	}

//	RehabGestures myRehabGestures;
	//Automatically run when a scene starts
	void Awake()
	{
	//	myRehabGestures = transform.parent.GetComponent<RehabGestures>();
	}
	// Update is called once per frame
	void Update () {
	//	Debug.Log ("REhab Mnu update");
	//	checkVelocity();
	}


	


}
