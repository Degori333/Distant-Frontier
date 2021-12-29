using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemiesPrefabs;
    public TextMeshProUGUI waveCount;
    public TextMeshProUGUI inputInfo;
    public GameObject shopUI;
    public GameObject playerUI;
    private GameManager gameManager;
    private PlayerController playerStatus;
    private int enemyCount;
    public int waveNumber = 1;
    private int maxNumberOfEnemies = 100;
    private int enemiesPerWave = 1;
    private int enemiesToSpawn = 10;

    public float waveCooldown = 5.0f;
    public float enemySpawnCooldown = 1.5f;

    private bool canSpawn = true;
    public bool shopOpen = false;

    private float horizontalSpawnPoint = 28.0f;
    private float verticalSpawnPoint = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(CooldownBetweenWaves());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus.alive && gameManager.gameStarted)
        {
        enemyCount = FindObjectsOfType<EnemyMovement>().Length;
            if (canSpawn && enemyCount <= maxNumberOfEnemies && enemiesPerWave > 0)
            {
                Invoke("SpawnForWave", 0.0f);
                enemiesPerWave--;
                StartCoroutine(CooldownBetweenSpawns());
            }
            else if (enemyCount == 0 && enemiesPerWave <= 0)
            {
                waveNumber++;
                StartCoroutine(CooldownBetweenWaves());
            }
        }
    }

    IEnumerator CooldownBetweenSpawns()
    {
        canSpawn = false;
        yield return new WaitForSeconds(enemySpawnCooldown);
        canSpawn = true;
    }

    IEnumerator CooldownBetweenWaves()
    {
        waveCount.text = "" + waveNumber;
        canSpawn = false;
        enemiesPerWave = waveNumber * (enemiesToSpawn - (waveNumber * 2));
        enemiesToSpawn += waveNumber;
        if(enemySpawnCooldown > 1.0f)
        {
            enemySpawnCooldown -= 0.1f;
        }
        if (!shopOpen && waveNumber != 1)
        {
            shopOpen = true;
            shopUI.SetActive(true);
            playerUI.SetActive(false);
            Time.timeScale = 0;
        }
        yield return new WaitForSeconds(waveCooldown);
        inputInfo.gameObject.SetActive(false);
        canSpawn = true;
    }

    private void SpawnForWave()
    {
        int typeOfenemy = Random.Range(0, 101);
            if (typeOfenemy < 60)
            {
                SpawnEnemyOfType(0);
            }
            else if (typeOfenemy < 90)
            {
                SpawnEnemyOfType(1);
            }
            else
            {
                SpawnEnemyOfType(2);
            }
        
    }

    private void SpawnEnemyOfType(int type)
    {
        Instantiate(enemiesPrefabs[type], GenerateRandomPos(), enemiesPrefabs[type].transform.rotation);
    }



    private Vector3 GenerateRandomPos()
    {
    float spawnPos;
    Vector3 spawnCoordinates;
    int randomSide = Random.Range(1, 5); 
        // top spawn
        if(randomSide == 1)
            {
                spawnPos = Random.Range(-horizontalSpawnPoint, horizontalSpawnPoint);
                spawnCoordinates = new Vector3(spawnPos, 0, verticalSpawnPoint); 
            }
        // bottom spawn
        else if (randomSide == 2)
            {
                spawnPos = Random.Range(-horizontalSpawnPoint, horizontalSpawnPoint);
                spawnCoordinates = new Vector3(spawnPos, 0, -verticalSpawnPoint);
            }
        // left spawn
        else if(randomSide == 3)
            {
                spawnPos = Random.Range(-verticalSpawnPoint, verticalSpawnPoint);
                spawnCoordinates = new Vector3(-horizontalSpawnPoint, 0, spawnPos);
            }
        // right spawn
        else
            {
            spawnPos = Random.Range(-verticalSpawnPoint, verticalSpawnPoint);
            spawnCoordinates = new Vector3(horizontalSpawnPoint, 0, spawnPos);
            }
    
    return spawnCoordinates;
    }

}
