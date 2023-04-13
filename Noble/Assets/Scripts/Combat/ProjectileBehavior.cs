using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the behavior of fired projectiles
public class ProjectileBehavior : MonoBehaviour
{
    // variables initialized in start
    private float projectileVelocity;
    private Rigidbody2D rb;
    private GameObject player;
    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        projectileVelocity = 5f;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.position.x - this.transform.position.x < 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        rb.velocity = new Vector2(projectileVelocity * direction, 0);
        StartCoroutine(WaitToDestroy());
    }

    // destroy this object if it comes into contact with the player
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    // check for collisions with non-enemy game objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(
                collision.gameObject.GetComponent<Collider2D>(),
                this.gameObject.GetComponent<Collider2D>()
            );
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // destroys the projectile after the time has elapsed
    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
