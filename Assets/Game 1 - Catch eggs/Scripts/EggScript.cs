using UnityEngine;
using System.Collections;

public class EggScript : MonoBehaviour {

    void Awake()
    {
        //rigidbody.AddForce(new Vector3(0, -100, 0), ForceMode.Force);
    }
	public float minFallSpeed = 1.5f;

	public float maxFallSpeed = 3.0f;


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
		float fallSpeed = (float)RehabMenu.GetDifficulty () * 1.2f * Time.deltaTime;
	//	Debug.Log("Difficulty: " + RehabMenu.GetDifficulty());

       // float fallSpeed = 2 * Time.deltaTime;
        transform.position -= new Vector3(0, fallSpeed, 0);

        if (transform.position.y < -1 || transform.position.y >= 20)
        {
            //Destroy this gameobject (and all attached components)
            Destroy(gameObject);

			//Debug.Log ("YOlo destroy");
        }

	}
}
