using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float projectileVelocity;

    private Rigidbody2D rb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        projectileVelocity = 5f;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - this.transform.position;
        rb.velocity = new Vector2(direction.x, 0).normalized * projectileVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
            Destroy(this.gameObject);
    }
}
