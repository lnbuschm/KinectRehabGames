using UnityEngine;
using System.Collections;

public class BGMusic : MonoBehaviour {
	//myMusic : AudioClip[];
	
//	void Update() { if(!audio.isPlaying) playRandomMusic(); }
	
	//function playRandomMusic() { audio.clip = myMusic[Random.Range(0, myMusic.length)]; audio.Play(); }

	public AudioClip[] bgMusic;

	void playRandomMusic() {
		audio.clip = bgMusic[Random.Range(0, bgMusic.Length)]; 
		audio.volume = 0.5f;
		audio.Play ();   // commented out for debug :)

	}
	// Use this for initialization
	void Start () {
		playRandomMusic(); 
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying) playRandomMusic(); 
	}
}
