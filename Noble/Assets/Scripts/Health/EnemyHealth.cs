using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class keeps track of the health of the enemy
public class EnemyHealth : MonoBehaviour
{
    // game manager reference
    [SerializeField]
    private EnemyRespawner respawner;

    // health variables
    [SerializeField]
    private float startingHealth;
    private float currentHealth;

    // player and enemy references
    private GameObject enemy;
    private EnemyStateController enemyStateController;
    private GameObject player;
    private PlayerAnimation playerAnimation;

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.GetComponent<GameObject>();
        enemyStateController = this.GetComponent<EnemyStateController>();
        player = GameObject.Find("Player");
        playerAnimation = player.GetComponent<PlayerAnimation>();
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // if the enemy runs out of health, disable its gameobject
        if (currentHealth <= 0)
        {
            respawner.AddSlainEnemy(this.gameObject);
            currentHealth = 100;
            enemyStateController.SetCurrentFightState("DEAD");
            this.gameObject.SetActive(false);
        }
    }

    // check for collisions with the player's weapons
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerSword"))
        {
            // deal 1 hit damage unless player is on 3rd swing
            if (playerAnimation.GetCurrentSwordState() < 2)
            {
                currentHealth -= 50;
            }
            else
            {
                currentHealth -= 100;
            }
            collision.collider.enabled = false;
        }
    }
}
