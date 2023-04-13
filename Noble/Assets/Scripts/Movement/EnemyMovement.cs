using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the movement of an eneymy game object
public class EnemyMovement : MonoBehaviour
{
    // how close can the enemy get to the player before attacking
    [SerializeField]
    private float waitThreshold;

    // will be stationary if speed is set to 0
    [SerializeField]
    private float speed;

    // true if the enemy has a wait state
    [SerializeField]
    private bool hasWaitState;

    // player reference
    private GameObject player;

    // enemy rigidbody
    private Rigidbody2D rb;

    // playermovement and enemystate script references
    private PlayerMovement playerMovement;
    private EnemyStateController enemyState;

    // stores where the enemy spawns in at
    private Vector3 enemySpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody2D>();
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        enemyState = this.GetComponent<EnemyStateController>();

        // initialize the direction of the enemy
        enemyState.SetCurrentFightState("PATROL");
        if (speed > 0)
        {
            enemyState.SetCurrentDirection("RIGHT");
        }
        else
        {
            enemyState.SetCurrentDirection("LEFT");
            this.transform.Rotate(new Vector3(0, -180, 0));
        }
        enemySpawnLocation = this.transform.position;
    }

    void FixedUpdate()
    {
        // approach the player
        if (enemyState.GetCurrentFightState().Equals("ATTACK"))
        {
            TryRotateEnemy(player.transform.position);
            rb.velocity = Vector2.right * speed;
        }
        // stop moving towards the player but still execute attack
        else if (
            enemyState.GetCurrentFightState().Equals("WAIT")
            || enemyState.GetCurrentFightState().Equals("STAGGERED")
        )
        {
            rb.velocity = Vector2.zero;
            TryRotateEnemy(player.transform.position);
        }
        // return to the spawn point and patrol that location
        else if (enemyState.GetCurrentFightState().Equals("PATROL"))
        {
            if (this.transform.position.x - enemySpawnLocation.x < -5)
            {
                if (speed < 0)
                    speed = -speed; // make speed positive if it is a negative value

                TryRotateEnemy(enemySpawnLocation);
            }
            else if (this.transform.position.x - enemySpawnLocation.x > 5)
            {
                if (speed > 0)
                    speed = -speed;

                TryRotateEnemy(enemySpawnLocation);
            }
            // modify speed
            rb.velocity = Vector2.right * speed;
        }
        else // the enemy just respawned after being in a dead state
        {
            this.transform.position = enemySpawnLocation;
            enemyState.SetCurrentFightState("PATROL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyState.GetCurrentFightState().Equals("STAGGERED"))
        {
            // the enemy should attack the player if its within 20f and the player is in sight
            if (Vector2.Distance(this.transform.position, player.transform.position) <= 10f)
            {
                if (enemyState.GetIfPlayerIsInSight(0, true))
                {
                    if (
                        (
                            Vector3.Distance(this.transform.position, player.transform.position)
                            <= waitThreshold
                        ) && hasWaitState
                    )
                    {
                        enemyState.SetCurrentFightState("WAIT");
                    }
                    else
                    {
                        enemyState.SetCurrentFightState("ATTACK");
                    }
                }
            }
            else
            {
                enemyState.SetCurrentFightState("PATROl");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // ignore collisions with Background tat
        if (collider.gameObject.CompareTag("Background") || collider.gameObject.CompareTag("Enemy"))
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collider);
        // disable this gameobject if it runs into a trap
        if (collider.gameObject.CompareTag("Trap"))
            this.gameObject.SetActive(false);
    }

    // rotate the enemy sprite
    public void TryRotateEnemy(Vector3 referencePosition)
    {
        if (
            referencePosition.x > this.transform.position.x
            && enemyState.GetCurrentDirection().Equals("LEFT")
        )
        {
            rb.velocity = Vector2.zero;
            this.transform.Rotate(new Vector3(0, 180, 0));
            enemyState.SetCurrentDirection("RIGHT");
            speed = -speed;
        }
        else if (
            referencePosition.x < this.transform.position.x
            && enemyState.GetCurrentDirection().Equals("RIGHT")
        )
        {
            rb.velocity = Vector2.zero;
            this.transform.Rotate(new Vector3(0, -180, 0));
            enemyState.SetCurrentDirection("LEFT");
            speed = -speed;
        }
    }
}
