using UnityEngine;
using System.Collections;
using System.Timers;

// Take a screenshot every X seconds (timer1)


public class Screenshot : MonoBehaviour {
	Timer timer1; 
	private int ssCount = 0;
	private bool ssCapture = false;
	public int screenshotIntervalMS = 500;
	// Use this for initialization
	void Start () {
		timer1 = new Timer(screenshotIntervalMS); // Set up the timer for x milliseconds
		timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
		timer1.AutoReset = true;
	//	timer1.Enabled = true; // Enable it
		timer1.Start(); // Enable it
	}

	void Capture() {
		Application.CaptureScreenshot("Screenshot" + ssCount.ToString("D5") + ".png");
		ssCount++;
		ssCapture = false;
		timer1.Start();
	//	Debug.Log("Screenshot captured");
	}

	void timer1_Elapsed(object sender, ElapsedEventArgs e)
	{
	//	Capture ();

		ssCapture = true;
	//	timer1.
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
