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
	public static int rightHandMultiplier = 1;
	public static int leftHandMultiplier = 1;
	public static bool warningRH = false;
	public static bool warningLH = false;
	public static bool tutorialEnabled = false;
	public int menuTime = 5000;  // in ms
	public Timer _roundTimer; // From System.Timers
	public int gameNumber;
	public static int currentGame;
	static System.Random rand = new System.Random ();
	static Boolean roundEnd = false; // start on nextRound screen
	Timer _textTimer1; 
	Timer _menuTimer1; 

	private static int difficultySequence = 1;
	public static int roundDifficulty = 1;
	public int guiPadding = 10;

	static int pointValue = 1;

	static int[][] difficulty = new int[][] { new int[] {1,2,3,4}, new int[] {1,2,4,3}, 
		new int[] {1,3,2,4}, new int[] {1,3,4,2}, new int[] {1,4,2,3}, new int[] {1,4,3,2} };

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
	void _roundTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		RehabMenu.roundEnd = true;
		RehabMenu.roundNum++;
		//screenToggle = true;
		if (RehabGestures.rightHandDominant) {

			RehabMenu.rightHandMultiplier = RehabMenu.difficulty [RehabMenu.difficultySequence] [RehabMenu.roundNum];
			RehabMenu.roundDifficulty = RehabMenu.rightHandMultiplier;
			RehabMenu.leftHandMultiplier = 1;
		} else {
			RehabMenu.rightHandMultiplier = 1;
			RehabMenu.leftHandMultiplier = difficulty [RehabMenu.difficultySequence] [RehabMenu.roundNum];
			RehabMenu.roundDifficulty = RehabMenu.leftHandMultiplier ;
		}
		Debug.Log ("Round " + RehabMenu.roundNum + " ended");
		Debug.Log ("LH Multiplier: " + leftHandMultiplier);
		Debug.Log ("RH Multiplier: " + rightHandMultiplier);
//		int rand1 = (int)Math.Floor ((double)rand.Next(0, multipliers.Length));
//		int rand2 = (int)Math.Floor ((double)rand.Next (0, multipliers[rand1].Length));
//		rightHandMultiplier = multipliers [rand1] [0];
//		leftHandMultiplier = multipliers [rand1] [1];



	}
	private bool screenToggle = false;

	private int menuScreenCount = 0;
	void _menuTimer1_Elapsed(object sender, ElapsedEventArgs e)
	{

		_menuTimer1.Stop ();
		screenToggle = true;
	}
	bool showNextRoundScreen1 = false;
	void showNextRoundScreen(){
	//	Debug.Log ("SHOW NEXT ROUND SCREEN!!");
		//	summaryScreenToggle = true;
	//	camera.transform.Rotate (new Vector3 (0, 180, 0));
		if (menuScreenCount > 0) RehabMenu.tutorialEnabled = false;
		if (RehabGestures.rightHandDominant) {
						summaryLine1GUI.text = "Eggs will move at " + RehabMenu.rightHandMultiplier + "X rate when you use your right side";
			summaryLine2GUI.text = "Eggs will move at 1X rate if you use your left side";  // "Difficulty will be 1X when you use your left side" ;
		} else {

						summaryLine1GUI.text = "Eggs will move at 1X when you use your right side";
						summaryLine2GUI.text = "Eggs will move at " + RehabMenu.leftHandMultiplier + "X rate when you use your left side";
				}

		scoreGUI.text = "The next round is about to begin";
		_menuTimer1.Start ();
		showNextRoundScreen1 = false;
	}
	private bool showRoundSummaryScreen1 = false;
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
		if (menuScreenCount == 0) RehabMenu.tutorialEnabled = true;
		else {
		summaryLine1GUI.text =  "You scored " + strongScore + " points with your strong side!";
		summaryLine2GUI.text =  "You scored " + weakScore + " points with your weak side!";
		scoreGUI.text = "Total Points Scored: " + theScore;
		}
	//	showRoundSummaryScreen1 = false;
		_menuTimer1.Start ();
	}

	public static int GetDifficulty() { 
	//	Debug.Log ("Round num: " + roundNum);
		if (difficultySequence >= difficulty.Length || roundNum >= difficulty[difficultySequence].Length) return 0;
		else if (RehabGestures.rightHandActive == RehabGestures.rightHandDominant) 
						return difficulty [difficultySequence] [roundNum];
				else
						return 1;  // always return difficulty 1 if using weak side

	}
	public static int roundNum = 0;
		                   
	public static void Score() {
		if (RehabGestures.rightHandActive) {
			rhScore++;
			theScore += pointValue; // * rightHandMultiplier;
			RehabMenu.rhCaughtEggs++;
		} else {
			lhScore++;
			theScore +=  pointValue; // * leftHandMultiplier;
			RehabMenu.lhCaughtEggs++;
		}
	}


	//OnGUI is called multiple times per frame. Use this for GUI stuff only!
	void OnGUI()
	{
		//We display the game GUI from the playerscript
		//It would be nicer to have a seperate script dedicated to the GUI though...
		//GUILayout.Label(
		//GUILayout.Label("Score: " + theScore);


		if (screenToggle) {
			menuScreenCount++;
			screenToggle=false;


		    if (menuScreenCount == 9) {
				_roundTimer.Stop() ;
				_textTimer1.Stop () ;
				_menuTimer1.Stop ();
				GameObject.Find ("Database").SendMessage("WriteToDB");
				summaryLine1GUI.text = "Game Completed!";
				summaryLine2GUI.text = "Relax your arms.";
				scoreGUI.text = "Total points scored: " + RehabMenu.theScore;

				_menuTimer1.Start ();
			}
			else if (menuScreenCount > 9) {
				Time.timeScale = 1.0f;
				_roundTimer.Stop() ;
				_textTimer1.Stop () ;
				_menuTimer1.Stop ();
				Application.LoadLevel("GameSelect");
			}
			else if ((menuScreenCount % 2 == 1) && (menuScreenCount < 9)) { // == 1) {

				GameObject.Find ("Database").SendMessage("WriteToDB");
				showNextRoundScreen1 = true;
				_roundTimer.Stop ();
				InitDBLog();
			} 
			else if (menuScreenCount < 9) {
				camera.transform.Rotate (new Vector3 (0, 180, 0));
				Time.timeScale=1;
			//	RehabMenu.lhTime = 0;
			//	RehabMenu.rhTime = 0;
				SpawnerScript.autoSpawn = true;
				EggPitchSpawn.autoSpawn = true;
				Debug.Log ("RehabMenu Autospawn"); 
				summaryLine1GUI.text = "";
				summaryLine2GUI.text = "";
				scoreGUI.text = "";
			//	menuScreenCount = 0;

				_roundTimer.Start ();
			}


		}

		if (RehabMenu.roundEnd) {
			showRoundSummaryScreen ();
			RehabMenu.roundEnd = false;
		}


		if (showNextRoundScreen1) 
						showNextRoundScreen ();
		else if (showRoundSummaryScreen1)
				showRoundSummaryScreen();
		//	RehabMenu.AdjustDifficulty();
				
	//	Debug.Log ("RH MULTIPLIEZZ: " + RehabMenu.rightHandMultiplier);
		rightHandGUI.text = "Right Hand: " + RehabMenu.rightHandMultiplier + "X";
		leftHandGUI.text = "Left Hand : " + RehabMenu.leftHandMultiplier + "X";

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
	//		Time.timeScale = 0.01f;
	//		Debug.LogWarning ("WARNING GUI ENABLED");
		}
		else {
			warningGUI.text = "";
	//		Time.timeScale = 1;
		}	//	warningGUI.enabled = false;
					



		//scoreGUI.transform.TransformDirection( -Screen.width/2, Screen.height/2, 0);
	//	guiText.transform.position = Camera.mainCamera.WorldToViewportPoint(transform.parent.position + new Vector3(-Screen.width/2, Screen.height/2, 0)); 
		//GameObject.FindGameObjectWithTag(
	//	Debug.Log ("Kakaka");
		//guiText = "lalala";
	//	GameObject.Find (-1063142)
	}   

	public int roundTimeMS = 10000; // 30000; // 30 seconds

	// variables for DB logging
	public static int totalEggs;
	public static float rhTime;
	public static float lhTime;
	public static int rhCaughtEggs;
	public static int lhCaughtEggs;
	public float timeScaleMultiplier = 1.0f;
	public static float timeScale;

	void InitDBLog() {

		// init variables for logging to DB at end of each round
		RehabMenu.totalEggs = 0;
		RehabMenu.rhTime = 0.0f;
		RehabMenu.lhTime = 0.0f;
		RehabMenu.rhCaughtEggs = 0;
		RehabMenu.lhCaughtEggs = 0;
		RehabMenu.timeScale = timeScaleMultiplier;
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("RehabMenu Start()");
		GUI.enabled = true;
	//	warningGUI.enabled = false;
		warningGUI.text = "";
		_roundTimer = new Timer(roundTimeMS); // Set up the timer for 30 seconds
		_roundTimer.Elapsed += new ElapsedEventHandler(_roundTimer_Elapsed);
		_roundTimer.Enabled = false; // Enable it when the round starts
		
		_textTimer1 = new Timer(3000); // Set up the timer for 3 seconds
		_textTimer1.Elapsed += new ElapsedEventHandler(_textTimer1_Elapsed);
		_textTimer1.Enabled = true; // Enable it

		_menuTimer1 = new Timer(menuTime); // Set up the timer for 3 seconds
		_menuTimer1.Elapsed += new ElapsedEventHandler(_menuTimer1_Elapsed);
		_menuTimer1.Enabled = true; // Enable it

		// set difficulty sequence
		difficultySequence = RehabMenu.rand.Next(difficulty.Length); // returns non-negative random # less than the argument
		Debug.Log ("Difficulty sequence: " + difficultySequence);

		menuScreenCount = 0; // begin at -1   // 0
		RehabMenu.tutorialEnabled = true;
		showRoundSummaryScreen ();

		RehabMenu.currentGame = gameNumber;

		RehabMenu.roundNum = 0; // begin at -1   // 0
	//	showNextRoundScreen ();
//		Time.timeScale = 0;
//		camera.transform.Rotate (new Vector3 (0, 180, 0));
//		menuScreenCount = 1;  // initialize to next round screen

		InitDBLog();



	//	scoreGUI.pixelOffset = new Vector2(-Screen.width/2+guiPadding , Screen.height/2-guiPadding);
		rightHandGUI.pixelOffset = new Vector2(Screen.width/2-guiPadding , Screen.height/2-guiPadding);
		leftHandGUI.pixelOffset = new Vector2(Screen.width/2-guiPadding , Screen.height/2-rightHandGUI.fontSize-2*guiPadding);
	
		summaryLine1GUI.pixelOffset = new Vector2(0, Screen.height/2-guiPadding);
		summaryLine2GUI.pixelOffset = new Vector2(0, Screen.height/2-2*guiPadding-summaryLine1GUI.GetScreenRect().height);
	
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
	//	if (menuScreenCount % 2 == 0 && menuScreenCount>0 && menuScreenCount < 8) Debug.Log ("Menu Logggg");
	//	if (Time.timeScale == 0 ) Debug.Log("TImescale  1 11 1 1 " + Time.deltaTime);
		if (Time.timeScale == 1) {
			if (RehabGestures.rightHandActive) rhTime += Time.deltaTime;
			else lhTime += Time.deltaTime;
		}
	}


	


}
