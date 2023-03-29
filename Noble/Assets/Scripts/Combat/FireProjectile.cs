using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the scheduling of projectile firing
public class FireProjectile : MonoBehaviour
{
    // player references
    [SerializeField]
    GameObject player;

    // projectile reference
    [SerializeField]
    GameObject projectile;

    // projectile transform reference
    [SerializeField]
    Transform projectileFireTransform;

    // timer
    [SerializeField]
    float timer;

    // min distance to fire
    [SerializeField]
    float minDistanceToFire;

    // current timer time
    [SerializeField]
    float timerDelta;

    // enemy that has this script attached
    private EnemyStateController enemyState;

    // start function
    void Start()
    {
        enemyState = this.gameObject.GetComponent<EnemyStateController>();
        timer = 5.0f;
        timerDelta = 0;
    }

    // update method
    void Update()
    {
        // check to see if in range
        if (enemyState.GetCurrentFightState().Equals("ATTACK"))
        {
            timerDelta -= Time.deltaTime; // ountdown timer when in range
            // only fire when the timer delta is ready, the enemy is in an attack state,
            // and the enemy is facing the player
            // raycast fixes bug where the enemy will fire the projectile in the wrong direction, when rotating
            if (
                timerDelta <= 0
                && enemyState.GetIfPlayerIsInSight(0, false)
                && Mathf.Abs(player.transform.position.x - this.transform.position.x)
                    < minDistanceToFire
            )
            {
                Instantiate(projectile, projectileFireTransform.position, Quaternion.identity);
                timerDelta = timer;
            }
        }
    }
}
