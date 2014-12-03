using UnityEngine;
using System.Collections;
using System.Timers;

public class Tutorial : MonoBehaviour {
	public Texture[] tutorialImages;
	private int imgCount = 0;
	public Timer timer;
	public float padding = 20;
	public GUIText textLine1;
	public GUIText textLine2;


	// Use this for initialization
	void Start () {
		timer = new Timer(700); // Set up the timer for 3 seconds
		timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
		timer.Enabled = true; // Enable it
		GUI.enabled = true;
		Debug.Log ("Sreen width: " + Screen.width + "   Screen height: " + Screen.height);
	//	Debug.Log ("Camera width: " + Camera.current.pixelWidth + "   Camera height: " + Camera.current.pixelHeight);
	}

	void timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		imgCount++;
		if (imgCount >= tutorialImages.Length) imgCount = 0;
	}

	void OnGUI() {
		if (RehabMenu.tutorialEnabled == false) return;

		if (!tutorialImages[imgCount]) {
			Debug.LogError("Assign a Texture in the inspector.");
			return;
		}
		//GUI.DrawTexture(new Rect(Camera.current.pixelWidth/2-tutorialImages[imgCount].width*imgCount, Camera.current.pixelHeight/2, Camera.current.pixelWidth/2, Camera.current.pixelHeight/2), tutorialImages[imgCount], ScaleMode.ScaleToFit, true);
		//Camera.current.pixelWidth
		int i=0;
		//float sizeMultiplier = 0.00033f;
		float sizeOverlap = 0; // 0.12f;
		//	float imgWidth =     // +Screen.width*sizeOverlap*tutorialImages.Length

		///  Camera.current.pixelWidth
		float imgWidth = (Screen.width-2*padding)/(tutorialImages.Length-0);
		foreach(Texture t in tutorialImages) {
			GUI.DrawTexture(new Rect(i*imgWidth-i*sizeOverlap*t.width+padding,Screen.height*1.0f/3.0f,imgWidth,t.height*imgWidth/t.width),t,ScaleMode.StretchToFill);
		//	GUI.DrawTexture(new Rect(i*imgWidth-i*sizeOverlap*t.width+padding,0+padding,imgWidth,t.height*imgWidth/t.width),t,ScaleMode.StretchToFill);

			//	GUI.DrawTexture(new Rect(i*t.width*sizeMultiplier-i*sizeOverlap*t.width+padding,0+padding,t.width*sizeMultiplier*Screen.width,t.height*sizeMultiplier*Screen.height),t,ScaleMode.StretchToFill);
			i++;
		}

	//	textLine1.pixelOffset = new Vector2(Screen.width/2, 65);
	//	textLine2.pixelOffset = new Vector2(Screen.width/2, 10);
		switch (RehabMenu.currentGame) {
			case 0:
				break;
			case 1: // egg catch
				textLine1.text = "Hold forearm out level"; 
				textLine2.text = "and move side to side to control";
				break;
			case 2:   // stars
				textLine1.text = "Raise hand up and down"; 
				textLine2.text = "to control";
			break;
			case 3:  // bat
				textLine1.text = "Raise forearm and use it"; 
				textLine2.text = "to control the movement of the bat";
			break;
			case 4:  // fried egg
				textLine1.text = "Hold forearm up with hand pointing upwards"; 
				textLine2.text = "and move side to side to control";
			break;
		}


	}

	/*
	// Update is called once per frame
	void Update () {
		for(int i=0; i<tutorialImages.Length; i++){
			if (i==imgCount) tutorialImages[i].enabled = true;
			else tutorialImages[i].enabled = false;
		}
	
	}
	*/
}
