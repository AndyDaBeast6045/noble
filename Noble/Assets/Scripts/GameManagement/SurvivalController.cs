using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurvivalController : MonoBehaviour
{
    // array with all enemy types
    [SerializeField]
    private GameObject[] enemyVarients;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject rightBoundary;

    [SerializeField]
    private Canvas menu;

    [SerializeField]
    private Vector3 playerSpawnPos;

    [SerializeField]
    private Transform leftSpawn;

    [SerializeField]
    private Transform rightSpawn;

    // spawn time
    [SerializeField]
    private float spawnTimer;

    private EnemyRespawner respawner;

    // sountrack
    private AudioSource soundtrack;

    // player points
    private int playerPoints;

    // is survival on
    private bool isSurvivalOn;

    // check if the game has started
    private bool hasStarted;

    // start method
    void Start()
    {
        respawner = this.GetComponent<EnemyRespawner>();
        soundtrack = this.GetComponent<AudioSource>();
        isSurvivalOn = false;
        hasStarted = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !hasStarted)
        {
            isSurvivalOn = true;
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.L) && !hasStarted)
        {
            StartGame();
        }
    }

    // start game here
    private void StartGame()
    {
        menu.gameObject.SetActive(false);
        soundtrack.Play();
        player.SetActive(true);
        if (isSurvivalOn)
        {
            StartCoroutine(DoSpawnScheduling());
        }
        else
        {
            SetupLevel();
        }
        hasStarted = true;
    }

    // setup level
    public void SetupLevel()
    {
        respawner.ResetEnemies();
        rightBoundary.transform.position = new Vector3(250, 0, 0);
    }

    // scheduler
    private IEnumerator DoSpawnScheduling()
    {
        do
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTimer);

            // check score and modify the timer here
        } while (true);
    }

    // spawn the enemy in here
    private void SpawnEnemy()
    {
        int enemytoSpawn = Random.Range(0, enemyVarients.Length);
        int xPos = Random.Range(0, 2);
        // get actual x coordinate to spawn in at
        if (xPos == 0)
        {
            Instantiate(enemyVarients[enemytoSpawn], rightSpawn.position, Quaternion.identity);
        }
        else
        {
            Instantiate(enemyVarients[enemytoSpawn], rightSpawn.position, Quaternion.identity);
        }
    }

    // update points here
    public void IncrementPoints(GameObject slainEnemy)
    {
        if (slainEnemy.name.Equals("SlimeEnemy"))
        {
            playerPoints += 2;
        }
        else
        {
            playerPoints += 3;
        }
    }
}
