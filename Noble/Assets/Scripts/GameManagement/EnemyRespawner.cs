using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class implements utility functions that pertain to core game mechanics
public class EnemyRespawner : MonoBehaviour
{
    // array of inactive game objects
    private Queue<GameObject> enemiesToRespawn;

    // start method
    void Start()
    {
        enemiesToRespawn = new Queue<GameObject>();
    }

    // adds slain enemy to reset array
    public void AddSlainEnemy(GameObject enemy)
    {
        enemiesToRespawn.Enqueue(enemy);
    }

    // respawn slain enemies
    public void ResetEnemies()
    {
        while (enemiesToRespawn.Count > 0)
        {
            GameObject tempEnemy = enemiesToRespawn.Dequeue();
            tempEnemy.SetActive(true);
        }
    }
}
