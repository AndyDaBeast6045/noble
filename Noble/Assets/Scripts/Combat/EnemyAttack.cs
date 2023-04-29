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

    private bool doingAttack;

    // Start is called before the first frame update
    void Start()
    {
        sword = this.gameObject.transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody2D>();
        enemyState = this.GetComponent<EnemyStateController>();
        animator = this.GetComponent<Animator>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("can I attack?" + canAttack + " Am i attacking?" + doingAttack);
        // switch to an idle animation when the enemy is staggered

        // perform an attack if the enemy is able to
        if (
            enemyState.GetCurrentFightState().Equals("WAIT")
            && enemyState.GetIfPlayerIsInSight(0, false)
            && canAttack
            && !doingAttack
        )
        {
            animator.Play("EnemyAttack");
            doingAttack = true;
        }
        else
        {
            if (
                !enemyState.GetCurrentFightState().Equals("WAIT")
                && !enemyState.GetCurrentFightState().Equals("STAGGERED")
                && !doingAttack
            )
            {
                animator.Play("EnemyWalking");
            }
            else if (
                enemyState.GetCurrentFightState().Equals("STAGGERED")
                || (enemyState.GetCurrentFightState().Equals("WAIT") && !doingAttack)
            )
            {
                Debug.Log("is this happeninggg" + canAttack);
                animator.Play("EnemyIdle");
            }
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

    void SetCanAttack()
    {
        Debug.Log("hereee");
        doingAttack = false;
        canAttack = false;
        StartCoroutine(CountdownBetweenAttacks());
    }

    public void Init()
    {
        canAttack = true;
        doingAttack = false;
        DisableSwordCollider();
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
        // canAttack = false;
        StartCoroutine(CountdownBetweenAttacks());
    }
}
