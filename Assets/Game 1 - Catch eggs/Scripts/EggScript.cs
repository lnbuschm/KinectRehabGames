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
		System.Random rand = new System.Random ();
		
		float randFallSpeed = (float)rand.NextDouble () * (maxFallSpeed - minFallSpeed) + minFallSpeed;


		float fallSpeed = randFallSpeed * Time.deltaTime;
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
