using UnityEngine;
using System.Collections;

public class ROMImage : MonoBehaviour {
	public Texture rom1;
	public Texture rom2;
	public Texture rom3;
	public Texture rom4;
	public static int romImage = 1;
	public int left = 10;
	public int top = 10;
	public int width = 200;
	public int height = 200;
	// Use this for initialization
	void Start () {
		GUI.enabled = true;
	}
	void OnGUI () {
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

	}
	// Update is called once per frame
	void Update () {
	}
}
