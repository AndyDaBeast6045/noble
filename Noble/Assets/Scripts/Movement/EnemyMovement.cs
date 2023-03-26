using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    GameObject player;

    [SerializeField]
    float speed;

    [SerializeField]
    bool isRotated;

    private EnemyStateController enemyState;
    private Vector3 enemySpawnLocation;
    private bool hasSetVelocity;

    // Start is called before the first frame update
    void Start()
    {
        isRotated = false;
        enemyState = this.gameObject.GetComponent<EnemyStateController>();
        enemyState.SetCurrentFightState("Patrol");
        enemyState.SetCurrentDirection("Right");
        enemySpawnLocation = this.transform.position;
        hasSetVelocity = false;
    }

    void FixedUpdate()
    {
        // approach the player
        if (enemyState.GetCurrentFightState().Equals("Attack"))
        {
            TryRotateEnemy(player.transform.position);
            rb.velocity = new Vector3(speed, 0);
        }

        // return to the spawn point and continue patrolling
        if (enemyState.GetCurrentFightState().Equals("Patrol"))
        {
            if (this.transform.position.x - enemySpawnLocation.x < -5)
            {
                if (speed < 0)
                    speed = -speed; // make speed positive if it is a negative value

                TryRotateEnemy(enemySpawnLocation);
                rb.velocity = new Vector3(speed, 0);
            }
            else if (this.transform.position.x - enemySpawnLocation.x > 5)
            {
                if (speed > 0)
                    speed = -speed;

                TryRotateEnemy(enemySpawnLocation);
                rb.velocity = new Vector3(speed, 0);
            }
            else
            {
                rb.velocity = new Vector3(speed, 0);
            }
            hasSetVelocity = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.velocity.x);
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < 5f)
        {
            enemyState.SetCurrentFightState("Attack");
        }
        else
        {
            enemyState.SetCurrentFightState("Patrol");
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
            && enemyState.GetCurrentDirection().Equals("Left")
        )
        {
            rb.velocity = Vector2.zero;
            this.transform.Rotate(new Vector3(0, 180, 0));
            enemyState.SetCurrentDirection("Right");
            speed = -speed;
            isRotated = false;
        }
        else if (
            referencePosition.x < this.transform.position.x
            && enemyState.GetCurrentDirection().Equals("Right")
        )
        {
            rb.velocity = Vector2.zero;
            this.transform.Rotate(new Vector3(0, -180, 0));
            enemyState.SetCurrentDirection("Left");
            speed = -speed;
            isRotated = true;
        }
    }
}
