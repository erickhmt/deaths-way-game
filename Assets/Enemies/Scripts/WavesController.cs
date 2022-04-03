using UnityEngine;
using TMPro;

public class WavesController : MonoBehaviour
{
    public int maxWave;
    public Transform[] spawnPoints;
    public Teleport[] teleportPoints;
    public GameObject simpleEnemy, hardEnemy;
    public Transform waveStatusPanel;
    private bool isStarted, isFinished;
    private int totalEnemiesSpawnedInCurrentWave, totalEnemiesPerWave, waveCounter;
    private Transform lastSpawnPoint;
    public Color finishedColor, inProgressColor;
    

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

                    if(Random.Range(0, 100) > 90)
                        GameObject.Instantiate(
                            hardEnemy, 
                            spawnPoint.position, 
                            transform.rotation
                        );
                    else
                        GameObject.Instantiate(
                            simpleEnemy, 
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
