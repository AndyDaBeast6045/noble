using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class implements enemy melee attacks
public class EnemyAttack : MonoBehaviour
{
    // temporary reference to the sword that the player is holding
    // will remove when using an actual enemy sprite
    private GameObject sword;

    // player reference
    private GameObject player;

    // this enemy's rigid body reference
    private Rigidbody2D rb;

    // this enemy's state controller
    private EnemyStateController enemyState;

    // animator reference
    private Animator animator;

    // state if the enemy can attack
    private bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        sword = this.gameObject.transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody2D>();
        enemyState = this.GetComponent<EnemyStateController>();
        animator = this.GetComponent<Animator>();
        canAttack = true;
        DisableSwordCollider();
    }

    // Update is called once per frame
    void Update()
    {
        // switch to an idle animation when the enemy is staggered
        if (enemyState.GetCurrentFightState().Equals("STAGGERED"))
        {
            animator.Play("NobleIdle");
        }
        // perform an attack if the enemy is able to
        if (
            enemyState.GetCurrentFightState().Equals("WAIT")
            && enemyState.GetIfPlayerIsInSight(0, false)
            && canAttack
        )
        {
            canAttack = false;
            animator.Play("SwordAttackA");
            Debug.Log("Am i hereee");
            StartCoroutine(CountdownBetweenAttacks());
        }
    }

    // cooldown period between attacking
    IEnumerator CountdownBetweenAttacks()
    {
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    // enable the enemy's sword collider
    void EnableSwordCollider()
    {
        sword.GetComponent<Collider2D>().enabled = true;
    }

    // disable the enemy's sword collider
    void DisableSwordCollider()
    {
        sword.GetComponent<Collider2D>().enabled = false;
    }

    // Temporary methods that will be removed once there is an actual enemy using this script
    // ______________________________________________________________________________________

    void RemoveAttackFromInputBuffer()
    {
        // do nothing
    }

    void StartSwordResetTimer()
    {
        // do nothing
    }

    public void SetCurrentAnimation()
    {
        Debug.Log("is this being called");
        this.animator.Play("NobleIdle");
        canAttack = false;
        StartCoroutine(CountdownBetweenAttacks());
    }
}
