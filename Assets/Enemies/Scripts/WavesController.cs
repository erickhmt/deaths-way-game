using System.Collections;
using UnityEngine;
using TMPro;

public class WavesController : MonoBehaviour
{
    public int maxWave;
    public Transform[] spawnPoints;
    public Teleport[] teleportPoints;
    public GameObject simpleEnemy, hardEnemy;
    public Transform waveStatusPanel;
    private bool isStarted, isFinished, canSpawn;
    private int totalEnemiesSpawnedInCurrentWave, totalEnemiesPerWave, waveCounter;
    private Transform lastSpawnPoint;
    public Color finishedColor, inProgressColor;
    public GameObject enemyHealthBar;
    

    void Start() 
    {
        canSpawn = true;
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
                if(GameObject.FindGameObjectsWithTag("Enemy").Length < 10 && canSpawn)
                {
                    StartCoroutine(Spawn());
                }
            }
            else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                totalEnemiesSpawnedInCurrentWave = 0;
                totalEnemiesPerWave += (int)(totalEnemiesPerWave/2);
                waveCounter++;

                if(waveCounter > maxWave)
                {
                    isFinished = true;
                    foreach(Teleport tp in teleportPoints)
                        tp.isActive = true;
                    waveStatusPanel.Find("StatusLabel").GetComponent<TextMeshProUGUI>().SetText("WAVE FINISHED");
                    waveStatusPanel.Find("StatusLabel").GetComponent<TextMeshProUGUI>().color = finishedColor;
                    waveStatusPanel.Find("CounterLabel").GetComponent<TextMeshProUGUI>().color = finishedColor;
                }
                else
                    waveStatusPanel.Find("CounterLabel").GetComponent<TextMeshProUGUI>().SetText(
                        string.Format("{0}/{1}", waveCounter, maxWave)
                    );
            }
        }
    }

    IEnumerator Spawn() 
    {
        canSpawn = false;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
        while(spawnPoint == lastSpawnPoint)
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

        GameObject tmpBar;
        tmpBar =  GameObject.Instantiate(
            enemyHealthBar, 
            Vector3.zero, 
            new Quaternion(0f, 0f, 0f, 0f)
        );
        tmpBar.transform.SetParent(GameObject.Find("Canvas/EnemiesBars/").transform);

        GameObject newEnemy;
        if(Random.Range(0, 100) > 90)
        {
            newEnemy = GameObject.Instantiate(
                hardEnemy, 
                spawnPoint.position, 
                transform.rotation
            );
            tmpBar.GetComponent<UpdateEnemyHealthBar>().hardEnemy = newEnemy.GetComponent<DistanceEnemy>();
        }
        else
        {
            newEnemy = GameObject.Instantiate(
                simpleEnemy, 
                spawnPoint.position, 
                transform.rotation
            );
            tmpBar.GetComponent<UpdateEnemyHealthBar>().normalEnemy = newEnemy.GetComponent<Enemy>();
        }
        tmpBar.GetComponent<UpdateEnemyHealthBar>().offset = new Vector2(0f, 70f);
        tmpBar.GetComponent<UpdateEnemyHealthBar>().enemyTransform = newEnemy.transform;

        lastSpawnPoint = spawnPoint;
        totalEnemiesSpawnedInCurrentWave++;

        yield return new WaitForSeconds(1.3f);
        canSpawn = true;
    } 
    
    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(!isStarted && collider.gameObject.tag == "Player")
        {
            isStarted = true;
            waveStatusPanel.gameObject.SetActive(true);
            waveStatusPanel.Find("StatusLabel").GetComponent<TextMeshProUGUI>().color = inProgressColor;
            waveStatusPanel.Find("CounterLabel").GetComponent<TextMeshProUGUI>().color = inProgressColor;
            waveStatusPanel.Find("StatusLabel").GetComponent<TextMeshProUGUI>().SetText("WAVE IN PROGRESS");
            waveStatusPanel.Find("CounterLabel").GetComponent<TextMeshProUGUI>().SetText(
                string.Format("1/{0}", maxWave)
            );
            foreach(Teleport tp in teleportPoints)
                tp.isActive = false;
        }
    }

}
