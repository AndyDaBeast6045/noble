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

    // make sure the enemy is in sight before firing
    private RaycastHit2D hit;
    private bool playerIsInSight;
    private int direction;

    // start function
    void Start()
    {
        playerIsInSight = false;
        enemyState = this.gameObject.GetComponent<EnemyStateController>();
        timer = 5.0f;
        timerDelta = 0;
    }

    // fixed update
    void FixedUpdate()
    {
        // get the direction the enemy is facing before sending the raycast
        if (enemyState.GetCurrentDirection().Equals("Left"))
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        // check and see if the player is in the line of sight of the enemy
        hit = Physics2D.Raycast(this.transform.position, new Vector2(direction, 0));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerIsInSight = true;
            }
            else
            {
                playerIsInSight = false;
            }
        }
    }

    // update method
    void Update()
    {
        // check to see if in range
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < minDistanceToFire)
        {
            timerDelta -= Time.deltaTime; // countdown timer when in range

            // only fire when the timer delta is ready, the enemy is in an attack state,
            // and the enemy is facing the player
            // raycast fixes bug where the enemy will fire the projectile in the wrong direction, when rotating
            if (
                timerDelta <= 0
                && enemyState.GetCurrentFightState().Equals("Attack")
                && playerIsInSight
            )
            {
                Instantiate(projectile, projectileFireTransform.position, Quaternion.identity);
                timerDelta = timer;
            }
        }
    }
}
