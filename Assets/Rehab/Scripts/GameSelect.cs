using UnityEngine;
using System.Collections;
using System;

public class GameSelect : MonoBehaviour {
	public Texture cursorTexture;
	public Texture game1Snapshot;
	public Texture game2Snapshot;
	public Texture game3Snapshot;
	public Texture game4Snapshot;
	public GUIText chooseGameText;
	public GUIText chosenGameText;
	public double maxHoverTimeMS = 4000;   // 0.4f = 4 seconds
	public int cursorHeight = 100;
	public int cursorWidth = 100;
	public int padding = 50;
	public int handPositionBiasX = 300;  // 300 worked well for LNB at home
	private int centerBias = 100; // add this to both hands x position
	private int xPosBias = 0; // add this only to the left hand x position

	private DateTime hoverTimeStart; // = 0;
	private int selectedGame = 0;

	static int xPadding = Screen.width / 16;
	static int yPadding = Screen.height / 16;

	//		{Screen.width/14+xPadding, Screen.height/12+yPadding, Screen.width*7/20, Screen.height*7/20 },  // game 1 box

	private float[][] gameBoxes;


	void OnGUI () {
		if (RehabGestures.rightHandActive) {
			xPosBias = centerBias;
		}
		else {
			xPosBias = handPositionBiasX+centerBias;
		}


	//	rightHandGUI.pixelOffset = new Vector2(Screen.width/2-guiPadding , Screen.height/2-guiPadding);
	//	leftHandGUI.pixelOffset = new Vector2(Screen.width/2-guiPadding , Screen.height/2-rightHandGUI.fontSize-2*guiPadding);
	


		// Make a background box
		GUI.Box(new Rect(Screen.width/10,Screen.height/10,Screen.width*4/5,Screen.height*4.1f/5), "");

		// game1
	//	for (int i=0; i<gameBoxes.Length; i++) {
	//		Debug.Log ("Sizes: " + gameBoxes[i][0] + " , " + gameBoxes[i][1] + " , " + gameBoxes[i][2] + " , " + gameBoxes[i][3]); 
	//	}


		GUI.DrawTexture (new Rect(gameBoxes[0][0],gameBoxes[0][1],gameBoxes[0][2],gameBoxes[0][3] ), game1Snapshot);
		GUI.DrawTexture (new Rect(gameBoxes[1][0],gameBoxes[1][1],gameBoxes[1][2],gameBoxes[1][3] ), game2Snapshot);
		GUI.DrawTexture (new Rect(gameBoxes[2][0],gameBoxes[2][1],gameBoxes[2][2],gameBoxes[2][3] ), game3Snapshot);
		GUI.DrawTexture (new Rect(gameBoxes[3][0],gameBoxes[3][1],gameBoxes[3][2],gameBoxes[3][3] ), game4Snapshot);

	//	chooseGameText.pixelOffset = new Vector2(Screen.width/2, 15); // Screen.height/16); //Screen.height/3);

		/*
		// Make the first button. If it is pressed, /pplication.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,Screen.width*1/3,Screen.height*1/3), "Level 1")) {
		//	Application.LoadLevel(1);
			Application.LoadLevel("Zig Eggcatch");
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(20,70,Screen.width*1/3,Screen.height*1/3), "Level 2")) {
			Application.LoadLevel("GameSelect");
		}

*/

	
		// Create pointer
		GUI.DrawTexture (new Rect (RehabGestures.pointerXpos*RehabGestures.moveSensitivity+xPosBias, -RehabGestures.pointerYpos*RehabGestures.moveSensitivity, cursorWidth, cursorHeight), cursorTexture);
		
	//	Debug.Log ("GameSelect OnGUI()");

	}

	void Zig_UpdateUser(ZigTrackedUser user)
	{
	//	Debug.Log ("ZigUpdateUser()");
		if (user.SkeletonTracked) {
	//		Debug.Log ("Skeleton Tracked");
			//Debug.Log (" OK GAMESELECT ZIG");
	//		if (RehabGestures.rightHandActive) Debug.Log ("RH ACTIVE");
	//		else Debug.Log ("LH ACTIVE");
	//		Debug.Log ("Gameboxes: " + gameBoxes[0][0] + " " + gameBoxes[0][1] + " " + gameBoxes[0][2] + " " + gameBoxes[0][3]);
			float pointerLocationX = RehabGestures.pointerXpos*RehabGestures.moveSensitivity+xPosBias+cursorWidth/3;
			float pointerLocationY = -RehabGestures.pointerYpos*RehabGestures.moveSensitivity;
	//		Debug.Log ("Pointer X: " + pointerLocationX + " Y : " + pointerLocationY);

			/*
			if (pointerLocationX > gameBoxes[0][0] && pointerLocationX < gameBoxes[0][0]+gameBoxes[0][2] &&
			    pointerLocationY > gameBoxes[0][1] && pointerLocationY < gameBoxes[0][1]+gameBoxes[0][3]) {
	//			Debug.Log("Game 1 highlighted");
				chosenGameText.text = "Game 1 Selected";
				if (selectedGame != 1)  hoverTimeStart = DateTime.Now;
				selectedGame = 1;
				Debug.Log ("hover time : " + hoverTimeStart);
			}
			else {
				chosenGameText.text = "No Game Selected";
				//hoverTimeStart = 0.0f;
				hoverTimeStart = DateTime.Now.AddMinutes(-10);
				selectedGame = 0;
			}
			*/

	
			bool gameSelected = false;
			for (int i=0; i<gameBoxes.Length; i++) {
				if (pointerLocationX > gameBoxes[i][0] && pointerLocationX < gameBoxes[i][0]+gameBoxes[i][2] &&
				    pointerLocationY > gameBoxes[i][1] && pointerLocationY < gameBoxes[i][1]+gameBoxes[i][3]) {
					int game = i+1;
					chosenGameText.text = "Game " + game  + " Selected";
					if (selectedGame != game)  hoverTimeStart = DateTime.Now;
					selectedGame = game;
		//			Debug.Log ("hover time : " + hoverTimeStart);
					gameSelected = true;
				}

			}
			if (!gameSelected) {
				chosenGameText.text = "No Game Selected";
				selectedGame = 0;
				hoverTimeStart = DateTime.Now.AddMinutes(-10);
			}



			if (hoverTimeStart.AddMilliseconds(maxHoverTimeMS) < DateTime.Now) {
				switch(selectedGame){

				case 0:  // no game selected
					break;

				case 1:
					Application.LoadLevel("Game1-EggDrop");
					break;

				case 2:
					Application.LoadLevel("Game2-Stars");
					break;

				case 3:
					Application.LoadLevel("Game3-Bat");
					break;

				case 4:
					Application.LoadLevel("Game4-FriedEgg");
					break;

				default:
					Debug.LogWarning("Invalid game selected");
					break;
				}
		//		if (selectedGame == 1 ) Application.LoadLevel("Zig Eggcatch");

			}

		}

	}

	// Rectangles for Game select screenshots
	private float ssHeight = Screen.height*7/20;
	private float ssWidth = Screen.width*7/20;
	private float ssWidthPos = Screen.width/10; // /14
	private float ssHeightPos = Screen.height/10;

	// Use this for initialization
	void Start () {
		GUI.enabled = true;
		chooseGameText.text = "Hover Cursor Over A Game For " + (int)(maxHoverTimeMS/1000) + " Seconds To Select";
		hoverTimeStart = DateTime.Now.AddMinutes(-10); //0;
	//	Rect a = new Rect(
		gameBoxes = new float[4][] { 
			new float[] {ssWidthPos+xPadding, ssHeightPos+yPadding, ssWidth, ssHeight },  // game 1 box
			new float[] {ssWidthPos+xPadding*2+ssWidth, ssHeightPos+yPadding, ssWidth, ssHeight },  // game 2 box
			new float[] {ssWidthPos+xPadding, ssHeightPos+yPadding*2+ssHeight, ssWidth, ssHeight },  // game 3 box
			new float[] {ssWidthPos+xPadding*2+ssWidth, ssHeightPos+yPadding*2+ssHeight, ssWidth, ssHeight },  // game 4 box
		}; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
