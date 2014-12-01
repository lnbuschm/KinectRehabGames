using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public Transform eggPrefab;

    private float nextEggTime = 1.0f;
    public float spawnRate = 1.2f;

	public float eggSpawnMin = -2.2f;
	public float eggSpawnMax = 2.2f;
	public static bool autoSpawn = true;
	void Update () {
	//	spawnRate = RehabMenu.GetDifficulty ();
   //     if (nextEggTime < Time.time) {
	//		SpawnEgg(); 
	//		nextEggTime =  Time.time + spawnRate;
	//	}
		if (Time.timeScale != 0  && autoSpawn == true)
        {
            SpawnEgg();   // only spawn a new egg when one is destroyed
		//	nextEggTime =  Time.time + spawnRate;
			SpawnerScript.autoSpawn = false;  // spawn 1 egg to start

            //Speed up the spawnrate for the next egg
     //       spawnRate *= 0.995f;
     //       spawnRate = Mathf.Clamp(spawnRate, 0.6f, 99f);
        }
	}
	void Start () {
//		RehabMenu.currentGame = 1;
	//	SpawnEgg(); 
	//	Debug.Log (" SSOAWWN  SPAWN EGG !!!" ) ;
	}
    void SpawnEgg()
    {
		Debug.Log ("Egg Spawned !");
        float addXPos = Random.Range(eggSpawnMin, eggSpawnMax);
        Vector3 spawnPos = transform.position + new Vector3(addXPos,0,0);  // Game 1

		if (RehabMenu.currentGame == 2) {
		//	spawnPos = new Vector3(addXPos,5.0f,-7.6f); 
			spawnPos  = transform.position + new Vector3(4.0f,addXPos-1.0f,0);
		}
		else if (RehabMenu.currentGame == 4) {
			//	spawnPos = new Vector3(addXPos,5.0f,-7.6f); 
			spawnPos  = transform.position + new Vector3(addXPos,0,0);
		}
	//	if (RehabMenu.currentGame == 1) {
        Instantiate(eggPrefab, spawnPos, Quaternion.identity);
	//	}
	//	else if (RehabMenu.currentGame  == 4) {
	//		Instantiate(eggPrefab, spawnPos, Quaternion.identity);
	//	}
    }
}
