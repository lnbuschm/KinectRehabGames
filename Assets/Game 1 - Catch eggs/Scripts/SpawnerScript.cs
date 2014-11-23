using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public Transform eggPrefab;

    private float nextEggTime = 0.0f;
    public float spawnRate = 1.2f;

	public float eggSpawnMin = -2.2f;
	public float eggSpawnMax = 2.2f;
	public static bool autoSpawn = true;
	void Update () {
	//	spawnRate = RehabMenu.GetDifficulty ();
 //       if (nextEggTime < Time.time)
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
	//	SpawnEgg(); 
	//	Debug.Log (" SSOAWWN  SPAWN EGG !!!" ) ;
	}
    void SpawnEgg()
    {
		Debug.Log ("Egg Spawned !");
        float addXPos = Random.Range(eggSpawnMin, eggSpawnMax);
        Vector3 spawnPos = transform.position + new Vector3(addXPos,0,0);
        Instantiate(eggPrefab, spawnPos, Quaternion.identity);
    }
}
