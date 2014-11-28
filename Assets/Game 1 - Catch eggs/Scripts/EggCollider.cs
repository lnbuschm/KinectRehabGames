using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]

public class EggCollider : MonoBehaviour {
	public AudioClip eggScoreSound;
    PlayerScript myPlayerScript;

    //Automatically run when a scene starts
    void Awake()
    {
        myPlayerScript = transform.parent.GetComponent<PlayerScript>();
    }

    //Triggered by Unity's Physics
	void OnTriggerEnter(Collider theCollision)
    {
        //In this game we don't need to check *what* we hit; it must be the eggs
    ///    GameObject collisionGO = theCollision.gameObject;
    //    Destroy(collisionGO);

		Destroy (theCollision.gameObject);
		GameObject.Find ("SpawnObject").SendMessage("SpawnEgg");
	//	RehabMenu.theScore++;
    //    myPlayerScript.theScore++;
		RehabMenu.Score ();
		GameObject.Find ("SoundFX").audio.PlayOneShot(eggScoreSound, 1.0f);
    }
}
