using UnityEngine;
using System.Collections;

public class EggPitchSpawn : MonoBehaviour {

	
	public Transform eggPrefab;
	
	private float nextEggTime = 0.0f;
	public float spawnRate = 1.2f;
	
	public float eggSpawnMin = -3.0f;
	public float eggSpawnMax = 3.0f;
	public static bool autoSpawn = true;
	void Update () {
		//	spawnRate = RehabMenu.GetDifficulty ();
		//       if (nextEggTime < Time.time)
		if (Time.timeScale != 0.0f  && autoSpawn == true)
		{
			Debug.Log ("Auto spawn one egg");
			SpawnEgg();   // only spawn a new egg when one is destroyed
			//	nextEggTime =  Time.time + spawnRate;
			EggPitchSpawn.autoSpawn = false;  // spawn 1 egg to start
			
			//Speed up the spawnrate for the next egg
			//       spawnRate *= 0.995f;
			//       spawnRate = Mathf.Clamp(spawnRate, 0.6f, 99f);
		}
	}
	void Start () {
	//	RehabMenu.currentGame = 3;
		//	SpawnEgg(); 
		//	Debug.Log (" SSOAWWN  SPAWN EGG !!!" ) ;
	}
	void SpawnEgg()
	{
		Debug.Log ("Ball Pitch Spawned !");
		float addXPos = Random.Range(eggSpawnMin, eggSpawnMax);
		Vector3 spawnPos = transform.position + new Vector3(addXPos,1.5f,9.0f);
		Instantiate(eggPrefab, spawnPos, Quaternion.identity);
	}
}
