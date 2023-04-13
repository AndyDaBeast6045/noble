using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the scheduling of projectile firing
public class FireProjectile : MonoBehaviour
{
    // player references
    private GameObject player;

    // projectile reference
    [SerializeField]
    private GameObject projectile;

    // projectile transform reference
    [SerializeField]
    private Transform projectileFireTransform;

    // timer
    [SerializeField]
    private float timer;

    // min distance to fire
    [SerializeField]
    private float minDistanceToFire;

    // current timer time
    [SerializeField]
    private float timerDelta;

    // enemy that has this script attached
    private EnemyStateController enemyState;

    // enemy animator reference
    private Animator enemyAnimator;

    // start function
    void Start()
    {
        player = GameObject.Find("Player");
        enemyState = this.gameObject.GetComponent<EnemyStateController>();
        enemyAnimator = this.GetComponent<Animator>();
        timer = 5.0f;
        timerDelta = 0;
    }

    // update method
    void Update()
    {
        // check to see if in range
        if (
            enemyState.GetCurrentFightState().Equals("ATTACK")
            || enemyState.GetCurrentFightState().Equals("WAIT")
        )
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
                Debug.Log("we are here per usual");
                enemyAnimator.Play("SlimeAttack");
                timerDelta = timer;
            }
        }
    }

    // do the projectile attack and animation
    public void DoProjectileAttack()
    {
        Debug.Log("is this being called?");
        Instantiate(projectile, projectileFireTransform.position, Quaternion.identity);
    }
}
