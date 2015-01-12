using UnityEngine;
using System.Collections;

public class BatCollision : MonoBehaviour {
	public AudioClip scoreSound;
	public Transform particles;
	//Triggered by Unity's Physics
	void OnTriggerEnter(Collider theCollision)
	{
		//In this game we don't need to check *what* we hit; it must be the eggs
		if (theCollision.gameObject.name == "default") return;
	//	Debug.Log ("Colision name:" + theCollision.gameObject.name);
	//	Vector3 spawnPos = theCollision.gameObject.transform.position; // .position + new Vector3(0.0f,1.5f,9.0f);
	//	Instantiate(particles, spawnPos, Quaternion.identity);   // uncomment to enable particles on hit



		///    GameObject collisionGO = theCollision.gameObject;
		//    Destroy(collisionGO);
		
		GameObject.Find ("SoundFX").audio.PlayOneShot(scoreSound, 1.0f);

		GameObject.Find ("EggPitchSpawn").SendMessage("SpawnEgg");
		//Debug.Log ("Ball hit,..  spawn Pitch");
		//	RehabMenu.theScore++;
		//    myPlayerScript.theScore++;
		RehabMenu.Score ();
		//	GameObject.Find ("SoundFX").audio.PlayOneShot(scoreSound, 1.0f);  // this sound fx fucks up the spawning
	//	audio.PlayOneShot(scoreSound, 1.0f);  // this sound fx fucks up the spawning
		Destroy (theCollision.gameObject);
	

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
