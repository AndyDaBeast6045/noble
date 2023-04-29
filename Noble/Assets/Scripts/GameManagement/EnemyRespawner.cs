using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class implements utility functions that pertain to core game mechanics
public class EnemyRespawner : MonoBehaviour
{
    // Queue of inactive game objects
    private List<GameObject> enemiesToRespawn;
    private List<Vector3> spawnLocations;

    // start method
    void Start()
    {
        enemiesToRespawn = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        spawnLocations = new List<Vector3>();
        foreach (GameObject obj in enemiesToRespawn)
        {
            spawnLocations.Add(obj.transform.position);
        }
        DisableEnemies();
    }

    // disables each enemy in the queue
    public void DisableEnemies()
    {
        foreach (GameObject obj in enemiesToRespawn)
        {
            obj.SetActive(false);
        }
    }

    // respawn slain enemies
    public void ResetEnemies()
    {
        for (int i = 0; i < enemiesToRespawn.Count; i++)
        {
            GameObject enemy = enemiesToRespawn[i];
            enemy.SetActive(true);
            enemy.transform.position = spawnLocations[i];
            EnemyStateController enemyState = enemy.GetComponent<EnemyStateController>();
            enemyState.SetCurrentFightState("PATROL");
        }
    }
}
