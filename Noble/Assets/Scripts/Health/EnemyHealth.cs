using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class keeps track of the health of the enemy
public class EnemyHealth : MonoBehaviour
{
    // health variables
    [SerializeField]
    private float startingHealth;
    private float currentHealth;

    // player and enemy references
    private SurvivalController survivalController;
    private EnemyRespawner respawner;
    private GameObject enemy;
    private EnemyStateController enemyStateController;
    private GameObject player;
    private PlayerAnimation playerAnimation;

    // coroutine reference
    private Coroutine staggerTimer;

    // Start is called before the first frame update
    void Start()
    {
        survivalController = GameObject.Find("GameController").GetComponent<SurvivalController>();
        respawner = GameObject.Find("GameController").GetComponent<EnemyRespawner>();
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
            survivalController.IncrementPoints(this.gameObject);
            respawner.AddSlainEnemy(this.gameObject);
            currentHealth = 100;
            enemyStateController.SetCurrentFightState("DEAD");
            this.gameObject.SetActive(false);
        }
    }

    // check for collisions with the player's weapons
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerSword"))
        {
            // deal 1 hit damage unless player is on 3rd swing
            if (playerAnimation.GetCurrentAttack() < 3)
            {
                currentHealth -= 50;
            }
            else
            {
                currentHealth -= 100;
            }
            collider.enabled = false;
            if (staggerTimer != null)
                StopCoroutine(staggerTimer);
            staggerTimer = StartCoroutine(CountdownStaggerTimer());
        }
    }

    // length of time that the enemy will be staggered for
    IEnumerator CountdownStaggerTimer()
    {
        enemyStateController.SetCurrentFightState("STAGGERED");
        yield return new WaitForSeconds(0.5f);
        enemyStateController.SetCurrentFightState("ATTACK");
    }
}
