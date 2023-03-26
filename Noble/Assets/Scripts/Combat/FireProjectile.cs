using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the scheuling of projectile firing
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
    private Ray2D ray;
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

    //
    void FixedUpdate()
    {
        if (enemyState.GetCurrentDirection().Equals("Left"))
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        Debug.DrawRay(transform.position, new Vector2(direction * 5, 0), Color.green);
        hit = Physics2D.Raycast(this.transform.position, new Vector2(direction * 5, 0));
        if (hit.collider != null)
        {
            Debug.Log("We are hitting" + hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                playerIsInSight = true;
            }
            else
            {
                playerIsInSight = false;
            }
        }
        else
        {
            Debug.Log("The ray is not hitting anything");
        }
    }

    // update method
    void Update()
    {
        Debug.Log("Player is in sight? " + playerIsInSight);
        // check to see if in range
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < minDistanceToFire)
        {
            timerDelta -= Time.deltaTime; // countdown timer when in range
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
