using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public Transform eggPrefab;

    private float nextEggTime = 0.0f;
    public float spawnRate = 1.4f;
	
	public float eggSpawnMin = -2.2f;
	public float eggSpawnMax = 2.2f;

	void Update () {
	//	spawnRate = RehabMenu.GetDifficulty ();
        if (nextEggTime < Time.time)
        {
            SpawnEgg();
            nextEggTime = Time.time + spawnRate;

            //Speed up the spawnrate for the next egg
     //       spawnRate *= 0.995f;
     //       spawnRate = Mathf.Clamp(spawnRate, 0.6f, 99f);
        }
	}

    void SpawnEgg()
    {
        float addXPos = Random.Range(eggSpawnMin, eggSpawnMax);
        Vector3 spawnPos = transform.position + new Vector3(addXPos,0,0);
        Instantiate(eggPrefab, spawnPos, Quaternion.identity);
    }
}
