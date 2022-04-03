using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesController : MonoBehaviour
{
    public int maxWave;
    public Transform[] spawnPoints;
    public Teleport[] teleportPoints;
    public GameObject[] enemies;
    private bool isStarted, isFinished;
    private int totalEnemiesSpawnedInCurrentWave, totalEnemiesPerWave, waveCounter;
    private Transform lastSpawnPoint;

    void Start() 
    {
        isStarted = isFinished = false;
        totalEnemiesSpawnedInCurrentWave = 0;
        
        waveCounter = 1;
        totalEnemiesPerWave = 4;
    }

    void FixedUpdate() 
    {
        if(isStarted && !isFinished)
        {
            if(totalEnemiesSpawnedInCurrentWave < totalEnemiesPerWave)
            {
                if(GameObject.FindGameObjectsWithTag("Enemy").Length < 10)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
                    while(spawnPoint == lastSpawnPoint)
                        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject.Instantiate(
                        enemies[Random.Range(0, enemies.Length - 1)], 
                        spawnPoint.position, 
                        transform.rotation
                    );

                    lastSpawnPoint = spawnPoint;
                    totalEnemiesSpawnedInCurrentWave++;
                }
            }
            else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                totalEnemiesPerWave += (int)(totalEnemiesPerWave/2);
                waveCounter++;

                if(waveCounter > maxWave)
                {
                    isFinished = true;
                    foreach(Teleport tp in teleportPoints)
                        tp.isActive = true;
                    Debug.Log("All enemies defeated");
                }
                else
                    Debug.Log(string.Format("Wave: {0} Enemies:{1}", waveCounter, totalEnemiesPerWave));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(!isStarted && collider.gameObject.tag == "Player")
        {
            isStarted = true;
            foreach(Teleport tp in teleportPoints)
                tp.isActive = false;
        }
    }

}
