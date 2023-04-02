using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float speed;

    // playermovement and enemystate script references
    private PlayerMovement playerMovement;
    private EnemyStateController enemyState;

    // stores where the enemy spawns in at
    private Vector3 enemySpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        enemyState = this.gameObject.GetComponent<EnemyStateController>();

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
        else if (enemyState.GetCurrentFightState().Equals("WAIT"))
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
        // the enemy should attack the player if its within 20f and the player is in sight
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= 10f)
        {
            if (
                enemyState.GetIfPlayerIsInSight(0, true)
                && !(Vector3.Distance(this.transform.position, player.transform.position) <= 5f)
            )
            {
                enemyState.SetCurrentFightState("ATTACK");
            }
            else
            {
                enemyState.SetCurrentFightState("WAIT");
            }
        }
        else
        {
            enemyState.SetCurrentFightState("PATROl");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
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
