using UnityEngine;
using System.Collections;

public class EggScript : MonoBehaviour {

    void Awake()
    {
        //rigidbody.AddForce(new Vector3(0, -100, 0), ForceMode.Force);
    }
	public float minFallSpeed = 1.5f;

	public float maxFallSpeed = 3.0f;
	public Transform eggPrefab;


	// use logistic regression for fallspeed
	//  takes x = 1, 2, 3, 4
	//   f(x)  outputs 1.5, 2.25, 2.75, 3.25
	//   y = 1.235663303 ln(x) + 1.455748877

    //Update is called by Unity every frame
	void Update () {
		// random fall speed, between given threshold
//		System.Random rand = new System.Random ();
//		float randFallSpeed = (float)rand.NextDouble () * (maxFallSpeed - minFallSpeed) + minFallSpeed;
//		float fallSpeed = randFallSpeed * Time.deltaTime;
		if (Time.timeScale == 0) {
		//	Debug.Log ("EGgscript Destroy!");
			Destroy (gameObject);
		}
		// fall speed based on difficulty
	//	float fallSpeed =  0.7094323671f * Mathf.Log((float)RehabMenu.GetDifficulty ()) + 1.4f * Time.deltaTime;
		float fallSpeed =   (1.235663303f * Mathf.Log((float)RehabMenu.GetDifficulty()) + 1.455748877f ) * Time.deltaTime;
	//	float fallSpeed =  3.25f * Time.deltaTime;

		//float fallSpeed =  2.0f * Time.deltaTime;
		//	Debug.Log("Fall Speed: " + fallSpeed);

//		Debug.Log("Fall Speed: " + 0.7094323671f * Mathf.Log((float)RehabMenu.GetDifficulty ()) + 0.9988464371);
	//	Debug.Log("Difficulty: " + RehabMenu.GetDifficulty());
//		Debug.Log ("fall sp: " + Screen.height / (eggPrefab.renderer.bounds.size.y/0.4136584));
       // float fallSpeed = 2 * Time.deltaTime;
        transform.position -= new Vector3(0, fallSpeed, 0);

        if (transform.position.y < -1 || transform.position.y >= 20)
        {
            //Destroy this gameobject (and all attached components)
            Destroy(gameObject);
			GameObject.Find ("SpawnObject").SendMessage("SpawnEgg");
			//Debug.Log ("YOlo destroy");
        }

	}
}
