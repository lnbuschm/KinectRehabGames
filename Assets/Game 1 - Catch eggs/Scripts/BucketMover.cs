using UnityEngine;
using System.Collections;
using System;

public class BucketMover : MonoBehaviour {
	//private float xDelta = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	private Boolean xDirection = true;
	private float maxMove = 1.0f;

	// Update is called once per frame
	void Update () {

		float moveSpeed = 1 * Time.deltaTime;

		if (transform.position.x <= -maxMove && xDirection == false) {
						xDirection = true;
		//				Debug.LogWarning ("Xdir = true");
				} else if (transform.position.x >= maxMove && xDirection == true) {
						xDirection = false;
		//				Debug.LogWarning ("Xdir = false");
				}
	//	Debug.Log ("Here");

		if (xDirection) {
						transform.position += new Vector3 (moveSpeed, 0, 0);
		//				Debug.Log ("Here");
				} else {
						transform.position -= new Vector3 (moveSpeed, 0, 0);
		//				Debug.Log ("Not Here");
				}
	//	if (transform.position.y < -1 || transform.position.y >= 20) {
//				}
	}
}
