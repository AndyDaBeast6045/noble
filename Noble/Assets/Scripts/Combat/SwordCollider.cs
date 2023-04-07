using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// collider script specifically for the player's sword
public class SwordCollider : MonoBehaviour
{
    // gameobject reference
    private GameObject sword;

    // player health reference
    private HealthConstructor playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        sword = this.gameObject;
        playerHealth = this.GetComponentInParent<HealthConstructor>();
    }

    // checks to see if swords collided
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemySword"))
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponentInParent<EnemyAttack>().SetCurrentAnimation();
            playerHealth.healUnit(50);
        }
    }
}
