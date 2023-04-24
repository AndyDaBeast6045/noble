using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalController : MonoBehaviour
{
    // array with all enemy types
    [SerializeField]
    private GameObject[] enemyVarients;

    [SerializeField]
    private Transform leftSpawn;

    [SerializeField]
    private Transform rightSpawn;

    // field to determine if this script is needed
    [SerializeField]
    private bool isSurvivalOn;

    // player points
    private int playerPoints;

    // spawn time
    [SerializeField]
    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (isSurvivalOn)
            StartCoroutine(DoSpawnScheduling());
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
        enemyVarients[enemytoSpawn].SetActive(true);
        if (xPos == 0)
        {
            Instantiate(enemyVarients[enemytoSpawn], leftSpawn.position, Quaternion.identity);
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
