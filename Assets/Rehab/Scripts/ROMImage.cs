using UnityEngine;
using System.Collections;

public class ROMImage : MonoBehaviour {
	public Texture rom1;
	public Texture rom2;
	public Texture rom3;
	public Texture rom4;
	public static int romImage = 1;
	public int left = 50;
	public int top = 50;
	public int width = 400;
	public int height = 400;
	public int spacing = 50;
	// Use this for initialization
	void Start () {
		GUI.enabled = true;
	}
	void OnGUI () {

		GUI.DrawTexture (new Rect(left, top, width, height ), rom1);
		GUI.DrawTexture (new Rect(left + width + spacing, top, width, height ), rom2);
		GUI.DrawTexture (new Rect(left, top + height + spacing, width, height ), rom3);
		GUI.DrawTexture (new Rect(left + width + spacing, top + height + spacing, width, height ), rom4);


		/*
		switch (romImage) {
		case 1:
			GUI.DrawTexture (new Rect(left, top, width, height ), rom1);
			break;
		case 2:
			GUI.DrawTexture (new Rect(left, top, width, height ), rom2);
			break;

		case 3:
			GUI.DrawTexture (new Rect(left, top, width, height ), rom3);
			break;
		case 4:
			GUI.DrawTexture (new Rect(left, top, width, height ), rom4);
			break;
		default:
			break;

		}
		*/
	


	}
	// Update is called once per frame
	void Update () {

		if ((Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) && romImage != 1) {
			romImage = 1;
			GameObject.Find ("Database").SendMessage("ROM_Stamp_DB");
		}
		else if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) && romImage != 2) {
			romImage = 2;
			GameObject.Find ("Database").SendMessage("ROM_Stamp_DB");
		}
		else if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) && romImage != 3) {
			romImage = 3;
			GameObject.Find ("Database").SendMessage("ROM_Stamp_DB");
		}
		else if ((Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) && romImage != 4) {
			romImage = 4;
			GameObject.Find ("Database").SendMessage("ROM_Stamp_DB");
		}

	}
}
