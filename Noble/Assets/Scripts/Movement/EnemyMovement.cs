using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    GameObject player;

    private PlayerMovement playerMovement;

    [SerializeField]
    float speed;

    private EnemyStateController enemyState;
    private Vector3 enemySpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player name is..." + player.name);
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        enemyState = this.gameObject.GetComponent<EnemyStateController>();
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

        if (enemyState.GetCurrentFightState().Equals("WAIT"))
        {
            rb.velocity = Vector2.zero;
        }
        // return to the spawn point and continue patrolling
        if (enemyState.GetCurrentFightState().Equals("PATROL"))
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

            rb.velocity = Vector2.right * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Is the player grounded??...   " + playerMovement.GetIsGrounded());
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= 20f)
        {
            if (enemyState.GetIfPlayerIsInSight(0, true) || (!playerMovement.GetIsOnPlatform()))
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
        if (collider.gameObject.CompareTag("Trap"))
            Destroy(this.gameObject);
    }

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
