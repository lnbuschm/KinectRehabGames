using UnityEngine;
using System.Collections;

public class BallThrow : MonoBehaviour {
	Vector3 origPos;
	Quaternion origRot;

	void Awake()
	{
		Pitch ();
	//	rigidbody.AddForce(new Vector3(0, -100, 0), ForceMode.Force);
	}
	
	// Use this for initialization
	void Start () {
		origPos = transform.position;
		origRot = transform.rotation;

	}
	void Pitch()
	{
		float forceAmount = 500f;
		Vector3 force = new Vector3(0,0, -1 * forceAmount);
		
		
		transform.position = origPos;
		transform.rotation = origRot;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		
		rigidbody.AddForce(force);
		
	}
	// Update is called once per frame
	void Update () {
	//	ForceMode.velocityChange()
	//	transform.position -= new Vector3(0, 1*Time.deltaTime , 2.0f*3.0f);
		if (Time.timeScale == 0) {
			//	Debug.Log ("EGgscript Destroy!");
			Destroy (gameObject);
			return;
		}
		
		if (transform.position.y < -1 || transform.position.y >= 20 || transform.position.z < -10.0f)
		{
			//Destroy this gameobject (and all attached components)
			Destroy(gameObject);
			GameObject.Find ("EggPitchSpawn").SendMessage("SpawnEgg");
			Debug.Log ("ERROR:  BAll pitch script is running");
		}
	}
}
