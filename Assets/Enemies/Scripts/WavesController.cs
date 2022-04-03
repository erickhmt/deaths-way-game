using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemies;
    private bool isStarted;
    private int totalEnemiesSpawnedInCurrentWave, totalEnemiesPerWave, waveCounter;

    void Start() 
    {
        isStarted = false;
        totalEnemiesSpawnedInCurrentWave = 0;
        
        waveCounter = 1;
        totalEnemiesPerWave = 4;
    }

    void FixedUpdate() 
    {
        if(totalEnemiesSpawnedInCurrentWave < totalEnemiesPerWave)
        {
            if(GameObject.FindGameObjectsWithTag("Enemy").Length < (totalEnemiesPerWave / 4 < 10 ? totalEnemiesPerWave / 4 : 10))
            {
                GameObject.Instantiate(
                    enemies[Random.Range(0, enemies.Length - 1)], 
                    spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position, 
                    transform.rotation
                );
                
                totalEnemiesSpawnedInCurrentWave++;
            }
        }
        else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            totalEnemiesPerWave += (int)(totalEnemiesPerWave/2);
            waveCounter++;
            Debug.Log(string.Format("Wave: {0} Enemies:{1}", waveCounter, totalEnemiesPerWave));
        }
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(!isStarted)
            isStarted = true;
    }

}
