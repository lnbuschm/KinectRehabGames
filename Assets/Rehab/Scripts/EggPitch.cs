﻿using UnityEngine;
using System.Collections;

public class EggPitch : MonoBehaviour {

	void Awake()
	{
		//rigidbody.AddForce(new Vector3(0, -100, 0), ForceMode.Force);
	}
	public float minFallSpeed = 1.5f;
	
	public float maxFallSpeed = 3.0f;
	//public Transform eggPrefab;
	
	public float yDamp = 1.0f;
	// use logistic regression for fallspeed
	//  takes x = 1, 2, 3, 4
	//   f(x)  outputs 1.5, 2.25, 2.75, 3.25
	//   y = 1.235663303 ln(x) + 1.455748877
	
	//Update is called by Unity every frame
	void Update () {
		// random fall speed, between given threshold
		//		System.Random rand = new System.Random ();
		//		float randFallSpeed = (float)rand.NextDouble () * (maxFallSpeed - minFallSpeed) + minFallSpeed;
		//		float throwSpeed = randFallSpeed * Time.deltaTime;
		if (Time.timeScale == 0.0f) {
		//		Debug.Log (" Destroy Pitch!");
			Destroy (gameObject);
			return;
		}

		float fallSpeed = 0.5f * Time.deltaTime;
		// fall speed based on difficulty
		//	float throwSpeed =  0.7094323671f * Mathf.Log((float)RehabMenu.GetDifficulty ()) + 1.4f * Time.deltaTime;
		//float throwSpeed =   (1.235663303f * Mathf.Log((float)RehabMenu.GetDifficulty()) + 1.455748877f ) * Time.deltaTime;


		//	float throwSpeed =  3.25f * Time.deltaTime;
		
		//float throwSpeed =  2.0f * Time.deltaTime;
		//	Debug.Log("Fall Speed: " + throwSpeed);
		
		//		Debug.Log("Fall Speed: " + 0.7094323671f * Mathf.Log((float)RehabMenu.GetDifficulty ()) + 0.9988464371);
		//	Debug.Log("Difficulty: " + RehabMenu.GetDifficulty());
		//		Debug.Log ("fall sp: " + Screen.height / (eggPrefab.renderer.bounds.size.y/0.4136584));
		// float throwSpeed = 2 * Time.deltaTime;


		Time.timeScale = (RehabMenu.GetDifficulty()*0.8f + 1.25f) * RehabGestures.timeScaleMultiplier;
		transform.position -= new Vector3(0, yDamp*Time.deltaTime, Time.deltaTime*3.0f);

	//	transform.position -= new Vector3(0, yDamp*Time.deltaTime, throwSpeed*2.5f);
		
		if (transform.position.y < -1 || transform.position.y >= 20 || transform.position.z < -10.0f)
		{
			//Destroy this gameobject (and all attached components)
			Destroy(gameObject);
			GameObject.Find ("EggPitchSpawn").SendMessage("SpawnEgg");
			Debug.Log ("Egg missed and destroyed");
		}
		
	}
}
