using UnityEngine;
using System.Collections;
using System.Timers;

// Take a screenshot every X seconds (timer1)


public class Screenshot : MonoBehaviour {
	Timer timer1; 
	private int ssCount = 0;
	private bool ssCapture = false;
	// Use this for initialization
	void Start () {
		timer1 = new Timer(1000); // Set up the timer for x seconds
		timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
		timer1.Enabled = true; // Enable it
	}

	void Capture() {
		Application.CaptureScreenshot("Screenshot" + ssCount + ".png");
		Debug.Log("Screenshot captured");
	}

	void timer1_Elapsed(object sender, ElapsedEventArgs e)
	{
		ssCount++;
		ssCapture = true;
	}
	void OnMouseDown() {
	//	Application.CaptureScreenshot("Screenshot.png");
	//	Debug.Log("Screenshot captured");
	}
	// Update is called once per frame
	void Update () {
		if (ssCapture){  // Input.GetKeyDown("space")
			Capture();
		}
	}
}
